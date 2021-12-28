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
        var file = new File("file", parent: company.Root);

        file.Parent.Should().Be(company.Root);
        file.Company.Should().Be(company);
        file.Parent.Files.Should().Contain(file);
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
        var company = new Company("company");

        var file = () => new File(name: null, parent: company.Root);

        file.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveTo_Valid_ShouldMoveFile()
    {
        var company = new Company("company");
        var file = new File("name", company.Root);
        var otherFolder = new Folder("folder", company.Root);

        file.MoveTo(otherFolder);

        file.Parent.Should().Be(otherFolder);
        otherFolder.Files.Should().HaveCount(1);
        otherFolder.Files.Should().Contain(file);
        company.Root.Files.Should().BeEmpty();
    }
}
