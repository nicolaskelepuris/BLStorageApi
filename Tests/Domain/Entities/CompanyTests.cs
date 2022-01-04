using System;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;
public class CompanyTests
{
    [Fact]
    public void ConstructCompany_Valid_ShouldConstruct()
    {
        var company = new Company("name");

        company.Root.Should().NotBeNull();
        company.Root.Company.Should().Be(company);
    }

    [Fact]
    public void ConstructCompany_NullName_ShouldThrow()
    {        
        var constructor = () => new Company(name: null!);

        constructor.Should().ThrowExactly<ArgumentNullException>();
    }
}
