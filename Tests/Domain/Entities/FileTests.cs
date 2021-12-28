using System;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;
public class FileTests
{
    [Fact]
    public void ConstructFile_ValidParent_ShouldConstruct()
    {
        var company = new Company("company");
        var file = new File("file", parent: company.Root, company);

        file.Parent.Should().Be(company.Root);
        file.Company.Should().Be(company);
        file.Parent.Files.Should().Contain(file);
    }

    [Fact]
    public void ConstructFile_NullParent_ShouldThrow()
    {
        var company = new Company("company");

        var file = () => new File("file", parent: null, company);

        file.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ConstructFile_NullName_ShouldThrow()
    {
        var company = new Company("company");

        var file = () => new File(name: null, parent: company.Root, company);

        file.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ConstructFile_NullCompany_ShouldThrow()
    {
        var company = new Company("company");
        var file = () => new File("name", parent: company.Root, company: null);

        file.Should().ThrowExactly<ArgumentNullException>();
    }
}
