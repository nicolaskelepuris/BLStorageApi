using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Data;
using Persistence.Interfaces;
using Xunit;

namespace Tests.Persistence.Data;
public class GenericRepositoryTests
{
    public class SomeEntity : BaseEntity
    {
    }

    public class SomeDbContext : DbContext
    {
        public SomeDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<SomeEntity> Entities { get; set; } = null!;
    }

    private DbContextOptions<SomeDbContext> dbContextOptions
    {
        get
        {
            var optionsBuilder = new DbContextOptionsBuilder<SomeDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return optionsBuilder.Options;
        }
    }

    private Mock<ISpecificationEvaluator<SomeEntity>> GetSpecificationEvaluator(SomeDbContext dbContext)
    {
        var mock = new Mock<ISpecificationEvaluator<SomeEntity>>();
        mock.Setup(_ => _.Evaluate(dbContext.Entities, It.IsAny<ISpecification<SomeEntity>>())).Returns(dbContext.Entities);

        return mock;
    }

    private Mock<ISpecification<SomeEntity>> specificationMock
    {
        get
        {
            var mock = new Mock<ISpecification<SomeEntity>>();
            mock.Setup(_ => _.Criteria).Returns<Expression<Func<SomeEntity, bool>>>(null);
            mock.Setup(_ => _.OrderBy).Returns<Expression<Func<SomeEntity, object>>>(null);
            mock.Setup(_ => _.OrderByDescending).Returns<Expression<Func<SomeEntity, object>>>(null);
            mock.Setup(_ => _.Includes).Returns(new List<Expression<Func<SomeEntity, object>>>());
            mock.Setup(_ => _.IncludesByString).Returns(new List<string>());
            mock.Setup(_ => _.IsPaginationEnabled).Returns(false);
            return mock;
        }
    }

    [Fact]
    public void Constructor_Valid_ShouldConstruct()
    {
        var dbContext = new SomeDbContext(dbContextOptions);

        var genericRepository = () => new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);

        genericRepository.Should().NotThrow();
    }

    [Fact]
    public void Constructor_NullContext_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = () => new GenericRepository<SomeEntity>(context: null!, GetSpecificationEvaluator(dbContext).Object);

        genericRepository.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Add_ValidEntity_ShouldEnterAddedState()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);
        var entity = new SomeEntity();

        genericRepository.Add(entity);

        dbContext.Entry(entity).State.Should().Be(EntityState.Added);
        dbContext.Entities.Should().BeEmpty();
    }

    [Fact]
    public void Add_NullEntity_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);

        var add = () => genericRepository.Add(entity: null!);

        add.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Update_ValidEntity_ShouldEnterModifiedState()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);
        var entity = new SomeEntity();

        genericRepository.Update(entity);

        dbContext.Entry(entity).State.Should().Be(EntityState.Modified);
        dbContext.Entities.Should().BeEmpty();
    }

    [Fact]
    public void Update_NullEntity_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);

        var update = () => genericRepository.Update(entity: null!);

        update.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public async Task Delete_ValidEntity_ShouldEnterDeletedState()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);
        var entity = new SomeEntity();
        await dbContext.Entities.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        genericRepository.Delete(entity);

        dbContext.Entry(entity).State.Should().Be(EntityState.Deleted);
        dbContext.Entities.Should().HaveCount(1);
    }

    [Fact]
    public void Delete_NullEntity_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);

        var delete = () => genericRepository.Delete(entity: null!);

        delete.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public async Task GetEntityByIdAsync_ValidId_ShouldGetEntity()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);
        var entity = new SomeEntity();
        await dbContext.Entities.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        var id = entity.Id;

        var entityFound = await genericRepository.GetEntityByIdAsync(id);

        entityFound.Should().NotBeNull();
        entityFound!.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetEntityByIdAsync_NotFoundEntity_ShouldReturnNull()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);

        var entityFound = await genericRepository.GetEntityByIdAsync(Guid.NewGuid());

        entityFound.Should().BeNull();
    }

    [Fact]
    public async Task ListAllAsync_ContainItems_ShouldList()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);
        var entities = new List<SomeEntity>()
        {
            new SomeEntity(),
            new SomeEntity(),
            new SomeEntity()
        };
        await dbContext.Entities.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();

        var entitiesFound = await genericRepository.ListAllAsync();

        entitiesFound.Should().HaveCount(entities.Count);
    }

    [Fact]
    public async Task ListAllAsync_Empty_ShouldReturnEmptyList()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);

        var entitiesFound = await genericRepository.ListAllAsync();

        entitiesFound.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEntityAsyncWithSpec_NotFound_ShouldReturnNull()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);

        var entity = await genericRepository.GetEntityAsyncWithSpec(new Mock<ISpecification<SomeEntity>>().Object);

        entity.Should().BeNull();
    }

    [Fact]
    public async Task GetEntityAsyncWithSpec_ShouldEvaluateSpecification()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var specificationMock = new Mock<ISpecification<SomeEntity>>();
        var specificationEvaluatorMock = GetSpecificationEvaluator(dbContext);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, specificationEvaluatorMock.Object);

        var entity = await genericRepository.GetEntityAsyncWithSpec(specificationMock.Object);

        specificationEvaluatorMock.Verify(_ => _.Evaluate(dbContext.Entities, specificationMock.Object), Times.Once);
    }

    [Fact]
    public async Task GetEntityAsyncWithSpec_EntityFound_ShouldReturnEntity()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var entities = new List<SomeEntity>()
        {
            new SomeEntity()
        };
        dbContext.Entities.AddRange(entities);
        await dbContext.SaveChangesAsync();
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, GetSpecificationEvaluator(dbContext).Object);

        var entity = await genericRepository.GetEntityAsyncWithSpec(specificationMock.Object);

        entity.Should().Be(entities[0]);
    }
}
