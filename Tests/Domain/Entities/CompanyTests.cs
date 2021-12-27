using System;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;
public class CompanyTests
{
    [Fact]
    public void ConstructCompany_ValidRoot_ShouldConstruct()
    {
        var companyName = "company";
        var root = Folder.CreateRoot(companyName);
        var company = new Company(companyName, root);

        company.Root.Should().Be(root);
        company.Root.Name.Should().Be(company.Name);
    }

    [Fact]
    public void ConstructCompany_NotValidRootNotARoot_ShouldThrow()
    {
        var companyName = "company";
        var root = Folder.CreateRoot("root");
        var notARoot = new Folder(companyName, root);

        var constructor = () => new Company(companyName, notARoot);

        constructor.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void ConstructCompany_NotValidRootDifferentName_ShouldThrow()
    {
        var root = Folder.CreateRoot("root");

        var constructor = () => new Company("company", root);

        constructor.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void ConstructCompany_NullRoot_ShouldThrow()
    {
        var constructor = () => new Company("company", root: null);

        constructor.Should().ThrowExactly<ArgumentNullException>();
    }
}
