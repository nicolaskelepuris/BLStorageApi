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
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var file = new File("file", parent: root, company);

        file.Parent.Should().Be(root);
        file.Company.Should().Be(company);
    }

    [Fact]
    public void ConstructFile_NullParent_ShouldThrow()
    {
        var file = () => new File("file", parent: null);

        file.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ConstructFile_NullName_ShouldThrow()
    {
        var root = Folder.CreateRoot("root");
        var file = () => new File(name: null, parent: root);

        file.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ConstructFile_NullCompany_ShouldThrow()
    {
        var root = Folder.CreateRoot("root");
        var file = () => new File("name", parent: root, company: null);

        file.Should().ThrowExactly<ArgumentNullException>();
    }
}
