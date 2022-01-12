using System;
using System.Threading.Tasks;
using Domain.Entities.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Xunit;

namespace Tests.Persistence.Data;
public class UnitOfWorkTests
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

    [Fact]
    public async Task Complete_ShouldCallContextSaveChanges()
    {
        var savedChanges = false;
        var dbContext = new SomeDbContext(dbContextOptions);
        dbContext.SavedChanges += (sender, args) => savedChanges = true;
        var unitOfWork = new UnitOfWork(dbContext);

        await unitOfWork.Complete();

        savedChanges.Should().BeTrue();
    }
}