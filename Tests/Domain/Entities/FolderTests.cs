using System;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;

public class FolderTests
{
    [Fact]
    public void AddSubFolder_EmptySubFolders_ShouldAdd()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var subFolder = new Folder("subFolder", root, company);

        root.AddSubFolder(subFolder);

        root.SubFolders.Should().HaveCount(1);
        root.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(root);
    }

    [Fact]
    public void AddSubFolder_ManySubFolders_ShouldAdd()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var subFolder = new Folder("subFolder", root, company);
        var anotherSubFolder = new Folder("another subFolder", root, company);

        root.AddSubFolder(subFolder);
        root.AddSubFolder(anotherSubFolder);

        root.SubFolders.Should().HaveCount(2);
        root.SubFolders.Should().Contain(subFolder);
        root.SubFolders.Should().Contain(anotherSubFolder);
        subFolder.Parent.Should().Be(root);
        anotherSubFolder.Parent.Should().Be(root);
    }

    [Fact]
    public void AddSubFolder_NullSubFolder_ShouldThrow()
    {
        var root = Folder.CreateRoot("root");

        root.Invoking(_ => _.AddSubFolder(subFolder: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void AddFile_EmptyFiles_ShouldAdd()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var file = new File("file", parent: root, company);

        root.AddFile(file);

        root.Files.Should().HaveCount(1);
        root.Files.Should().Contain(file);
        file.Parent.Should().Be(root);
    }

    [Fact]
    public void AddFile_ManyFiles_ShouldAdd()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var file = new File("file", parent: root, company);
        var anotherFile = new File("any file", parent: root, company);

        root.AddFile(file);
        root.AddFile(anotherFile);

        root.Files.Should().HaveCount(2);
        root.Files.Should().Contain(file);
        root.Files.Should().Contain(anotherFile);
        file.Parent.Should().Be(root);
        anotherFile.Parent.Should().Be(root);
    }

    [Fact]
    public void AddFile_NullFile_ShouldThrow()
    {
        var root = Folder.CreateRoot("root");

        root.Invoking(_ => _.AddFile(file: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveSubFolder_ValidSubFolder_ShouldMove()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var subFolder = new Folder("subFolder", root, company);
        root.AddSubFolder(subFolder);
        var destination = new Folder("another folder", root, company);

        root.MoveSubFolder(subFolder, destination);

        root.SubFolders.Should().BeEmpty();
        destination.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(destination);
    }

    [Fact]
    public void MoveSubFolder_NotFoundSubFolder_ShouldMove()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var subFolder = new Folder("subFolder", root, company);
        var destination = new Folder("another folder", root, company);

        root.MoveSubFolder(subFolder, destination);

        root.SubFolders.Should().BeEmpty();
        destination.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(destination);
    }

    [Fact]
    public void MoveSubFolder_ToNullDestination_ShouldThrow()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var subFolder = new Folder("subFolder", root, company);

        root.Invoking(_ => _.MoveSubFolder(subFolder, destination: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveSubFolder_NullSubFolder_ShouldThrow()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var destination = new Folder("destination", root, company);

        root.Invoking(_ => _.MoveSubFolder(subFolder: null, destination))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveFile_ValidFile_ShouldMove()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var file = new File("file", root, company);
        root.AddFile(file);
        var anotherFolder = new Folder("another folder", root, company);

        root.MoveFile(file, anotherFolder);

        root.Files.Should().BeEmpty();
        anotherFolder.Files.Should().Contain(file);
        file.Parent.Should().Be(anotherFolder);
    }

    [Fact]
    public void MoveFile_NotFoundFile_ShouldMove()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var file = new File("file", root, company);
        var anotherFolder = new Folder("another folder", root, company);

        root.MoveFile(file, anotherFolder);

        root.Files.Should().BeEmpty();
        anotherFolder.Files.Should().Contain(file);
        file.Parent.Should().Be(anotherFolder);
    }

    [Fact]
    public void MoveFile_ToNullDestination_ShouldMove()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var file = new File("file", parent: root, company);

        root.Invoking(_ => _.MoveFile(file, destination: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveFile_NullFile_ShouldThrow()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var destination = new Folder("destination", parent: root, company);

        root.Invoking(_ => _.MoveFile(file: null, destination))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ConstructFolder_ValidParent_ShouldConstruct()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var folder = new Folder("name", parent: root, company);

        folder.Parent.Should().Be(root);
        folder.Company.Should().Be(company);
    }

    [Fact]
    public void ConstructFolder_NullParent_ShouldThrow()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var folder = () => new Folder("name", parent: null, company);

        folder.Should().ThrowExactly<ArgumentNullException>();
    }    

    [Fact]
    public void ConstructFolder_NullCompany_ShouldThrow()
    {
        var root = Folder.CreateRoot("root");
        var folder = () => new Folder("name", parent: root, company: null);

        folder.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ConstructFolder_NullName_ShouldThrow()
    {
        var root = Folder.CreateRoot("company");
        var company = new Company("company", root);
        var folder = () => new Folder(name: null, parent: root, company);

        folder.Should().ThrowExactly<ArgumentNullException>();
    }
}
