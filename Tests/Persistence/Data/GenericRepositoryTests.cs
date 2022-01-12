using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Data;
using Xunit;

namespace Tests.Persistence.Data;
public class GenericRepositoryTests
{
    private class SomeEntity : BaseEntity
    {

    }

    private class SomeDbContext : DbContext
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

    public bool IsPaginationEnabled => throw new NotImplementedException();

    [Fact]
    public void Constructor_Valid_ShouldConstruct()
    {
        var dbContext = new DbContext(new DbContextOptions<DbContext>());

        var genericRepository = () => new GenericRepository<SomeEntity>(dbContext);

        genericRepository.Should().NotThrow();
    }

    [Fact]
    public void Constructor_NullContext_ShouldThrow()
    {
        var genericRepository = () => new GenericRepository<SomeEntity>(context: null!);

        genericRepository.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Add_ValidEntity_ShouldEnterAddedState()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);
        var entity = new SomeEntity();

        genericRepository.Add(entity);

        dbContext.Entry(entity).State.Should().Be(EntityState.Added);
        dbContext.Entities.Should().BeEmpty();
    }

    [Fact]
    public void Add_NullEntity_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);

        var add = () => genericRepository.Add(entity: null!);

        add.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Update_ValidEntity_ShouldEnterModifiedState()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);
        var entity = new SomeEntity();

        genericRepository.Update(entity);

        dbContext.Entry(entity).State.Should().Be(EntityState.Modified);
        dbContext.Entities.Should().BeEmpty();
    }

    [Fact]
    public void Update_NullEntity_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);

        var update = () => genericRepository.Update(entity: null!);

        update.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public async Task Delete_ValidEntity_ShouldEnterDeletedState()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);
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
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);

        var delete = () => genericRepository.Delete(entity: null!);

        delete.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public async Task GetEntityByIdAsync_ValidId_ShouldGetEntity()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);
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
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);

        var entityFound = await genericRepository.GetEntityByIdAsync(Guid.NewGuid());

        entityFound.Should().BeNull();
    }

    [Fact]
    public async Task ListAllAsync_ContainItems_ShouldList()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);
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
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);

        var entitiesFound = await genericRepository.ListAllAsync();

        entitiesFound.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEntityAsyncWithSpec_NotFound_ShouldReturnNull()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext);

        var entity = await genericRepository.GetEntityAsyncWithSpec(new Mock<ISpecification<SomeEntity>>().Object);

        entity.Should().BeNull();
    }
}
