using System;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;

public class FolderTests
{
    [Fact]
    public void MoveSubFolder_ValidSubFolder_ShouldMove()
    {
        var company = new Company("company");
        var subFolder = new Folder("subFolder", company.Root);
        var company2 = new Company("company 2");
        var destination = new Folder("another folder", company2.Root);

        company.Root.MoveSubFolder(subFolder, destination);

        company.Root.SubFolders.Should().BeEmpty();
        destination.SubFolders.Should().HaveCount(1);
        destination.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(destination);
        subFolder.Company.Should().Be(destination.Company);
    }

    [Fact]
    public void MoveSubFolder_ToNullDestination_ShouldThrow()
    {
        var company = new Company("company");
        var subFolder = new Folder("subFolder", company.Root);

        company.Root.Invoking(_ => _.MoveSubFolder(subFolder, destination: null!))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveSubFolder_NullSubFolder_ShouldThrow()
    {
        var company = new Company("company");
        var destination = new Folder("destination", company.Root);

        company.Root.Invoking(_ => _.MoveSubFolder(subFolder: null!, destination))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveFile_ValidFile_ShouldMove()
    {
        var company = new Company("company");
        var file = new File("file", company.Root);
        var company2 = new Company("company 2");
        var anotherFolder = new Folder("another folder", company2.Root);

        file.Parent.MoveFile(file, anotherFolder);

        company.Root.Files.Should().BeEmpty();
        anotherFolder.Files.Should().Contain(file);
        file.Parent.Should().Be(anotherFolder);
        file.Company.Should().Be(anotherFolder.Company);
    }

    [Fact]
    public void MoveFile_ToNullDestination_ShouldThrow()
    {
        var company = new Company("company");
        var file = new File("file", parent: company.Root);

        company.Root.Invoking(_ => _.MoveFile(file, destination: null!))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveFile_NullFile_ShouldThrow()
    {
        var company = new Company("company");
        var destination = new Folder("destination", parent: company.Root);

        company.Root.Invoking(_ => _.MoveFile(file: null!, destination))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ConstructFolder_Valid_ShouldConstruct()
    {
        var company = new Company("company");
        var folder = new Folder("name", parent: company.Root);

        folder.Parent.Should().Be(company.Root);
        folder.Company.Should().Be(company);
        folder.Parent!.SubFolders.Should().Contain(folder);
    }

    [Fact]
    public void ConstructFolder_NullParent_ShouldThrow()
    {
        var folder = () => new Folder("name", parent: null!);

        folder.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ConstructFolder_NullName_ShouldThrow()
    {
        var company = new Company("company");
        var folder = () => new Folder(name: null!, parent: company.Root);

        folder.Should().ThrowExactly<ArgumentNullException>();
    }
}
