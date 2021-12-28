using System;
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
        var genericRepository = () => new GenericRepository<SomeEntity>(context: null);

        genericRepository.Should().ThrowExactly<ArgumentNullException>();
    }
}
