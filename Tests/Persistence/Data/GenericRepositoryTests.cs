using System;
using System.Threading.Tasks;
using Domain.Entities.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
        dbContext.Entities.Should().HaveCount(0);
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
        dbContext.Entities.Should().HaveCount(0);
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
    public async Task Delete_ValidEntity_ShouldEnterDeletedStateAsync()
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
}
