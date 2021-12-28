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
        var company1 = new Company("company 1");
        var subFolder = new Folder("subFolder", new Company("company 2").Root);

        company1.Root.AddSubFolder(subFolder);

        company1.Root.SubFolders.Should().HaveCount(1);
        company1.Root.SubFolders.Should().Contain(subFolder);
        subFolder.Company.Should().Be(company1);
        subFolder.Parent.Should().Be(company1.Root);
    }

    [Fact]
    public void AddSubFolder_ManySubFolders_ShouldAdd()
    {
        var company1 = new Company("company 1");
        var subFolder = new Folder("subFolder", new Company("company 2").Root);
        var anotherSubFolder = new Folder("another subFolder", new Company("company 3").Root);

        company1.Root.AddSubFolder(subFolder);
        company1.Root.AddSubFolder(anotherSubFolder);

        company1.Root.SubFolders.Should().HaveCount(2);
        company1.Root.SubFolders.Should().Contain(subFolder);
        company1.Root.SubFolders.Should().Contain(anotherSubFolder);
        subFolder.Parent.Should().Be(company1.Root);
        anotherSubFolder.Parent.Should().Be(company1.Root);
        subFolder.Company.Should().Be(company1);
        anotherSubFolder.Company.Should().Be(company1);
    }

    [Fact]
    public void AddSubFolder_CascadingSubFolders_ShouldAdd()
    {
        var company1 = new Company("company 1");
        var subFolder1 = new Folder("subFolder 1", new Company("company 2").Root);
        var subFolder2 = new Folder("subFolder 2", new Company("company 3").Root);

        company1.Root.AddSubFolder(subFolder1);
        subFolder1.AddSubFolder(subFolder2);

        company1.Root.SubFolders.Should().HaveCount(1);
        company1.Root.SubFolders.Should().Contain(subFolder1);
        subFolder1.SubFolders.Should().HaveCount(1);
        subFolder1.SubFolders.Should().Contain(subFolder2);
        subFolder1.Parent.Should().Be(company1.Root);
        subFolder2.Parent.Should().Be(subFolder1);
        subFolder1.Company.Should().Be(company1);
        subFolder2.Company.Should().Be(company1);
    }

    [Fact]
    public void AddSubFolder_NullSubFolder_ShouldThrow()
    {
        var company = new Company("company");

        company.Root.Invoking(_ => _.AddSubFolder(subFolder: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void AddSubFolder_DuplicateSubFolder_ShouldNotAdd()
    {
        var company = new Company("company");
        var folder = new Folder("folder", company.Root);
        company.Root.AddSubFolder(folder);

        company.Root.SubFolders.Should().HaveCount(1);
    }

    [Fact]
    public void AddFile_EmptyFiles_ShouldAdd()
    {
        var company1 = new Company("company 1");
        var company2 = new Company("company 2");
        var file = new File("file", parent: company2.Root, company2);

        company1.Root.AddFile(file);

        company1.Root.Files.Should().HaveCount(1);
        company1.Root.Files.Should().Contain(file);
        file.Parent.Should().Be(company1.Root);
        file.Company.Should().Be(company1);
    }

    [Fact]
    public void AddFile_ManyFiles_ShouldAdd()
    {
        var company1 = new Company("company 1");
        var file = new File("file", parent: company1.Root, company1);
        var company2 = new Company("company 2");
        var anotherFile = new File("any file", parent: company2.Root, company2);

        company1.Root.AddFile(file);
        company1.Root.AddFile(anotherFile);

        company1.Root.Files.Should().HaveCount(2);
        company1.Root.Files.Should().Contain(file);
        company1.Root.Files.Should().Contain(anotherFile);
        file.Parent.Should().Be(company1.Root);
        anotherFile.Parent.Should().Be(company1.Root);
        file.Company.Should().Be(company1);
        anotherFile.Company.Should().Be(company1);
    }

    [Fact]
    public void AddFile_NullFile_ShouldThrow()
    {
        var company = new Company("company");

        company.Root.Invoking(_ => _.AddFile(file: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void AddFile_DuplicateFile_ShouldNotAdd()
    {
        var company1 = new Company("company 1");
        var file = new File("file", parent: company1.Root, company1);

        company1.Root.AddFile(file);

        company1.Root.Files.Should().HaveCount(1);
        company1.Root.Files.Should().Contain(file);
    }

    [Fact]
    public void MoveSubFolder_ValidSubFolder_ShouldMove()
    {
        var company = new Company("company");
        var subFolder = new Folder("subFolder", company.Root);
        var destination = new Folder("another folder", company.Root);

        company.Root.MoveSubFolder(subFolder, destination);

        company.Root.SubFolders.Should().HaveCount(1);
        company.Root.SubFolders.Should().Contain(destination);
        destination.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(destination);
    }

    [Fact]
    public void MoveSubFolder_NotFoundSubFolder_ShouldMove()
    {
        var company = new Company("company");
        var subFolder = new Folder("subFolder", company.Root);
        var destination = new Folder("another folder", company.Root);

        company.Root.MoveSubFolder(subFolder, destination);

        company.Root.SubFolders.Should().HaveCount(1);
        company.Root.SubFolders.Should().Contain(destination);
        destination.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(destination);
    }

    [Fact]
    public void MoveSubFolder_ToNullDestination_ShouldThrow()
    {
        var company = new Company("company");
        var subFolder = new Folder("subFolder", company.Root);

        company.Root.Invoking(_ => _.MoveSubFolder(subFolder, destination: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveSubFolder_NullSubFolder_ShouldThrow()
    {
        var company = new Company("company");
        var destination = new Folder("destination", company.Root);

        company.Root.Invoking(_ => _.MoveSubFolder(subFolder: null, destination))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveFile_ValidFile_ShouldMove()
    {
        var company = new Company("company");
        var file = new File("file", company.Root, company);
        company.Root.AddFile(file);
        var anotherFolder = new Folder("another folder", company.Root);

        company.Root.MoveFile(file, anotherFolder);

        company.Root.Files.Should().BeEmpty();
        anotherFolder.Files.Should().Contain(file);
        file.Parent.Should().Be(anotherFolder);
    }

    [Fact]
    public void MoveFile_NotFoundFile_ShouldMove()
    {
        var company = new Company("company");
        var file = new File("file", company.Root, company);
        var anotherFolder = new Folder("another folder", company.Root);

        company.Root.MoveFile(file, anotherFolder);

        company.Root.Files.Should().BeEmpty();
        anotherFolder.Files.Should().Contain(file);
        file.Parent.Should().Be(anotherFolder);
    }

    [Fact]
    public void MoveFile_ToNullDestination_ShouldMove()
    {
        var company = new Company("company");
        var file = new File("file", parent: company.Root, company);

        company.Root.Invoking(_ => _.MoveFile(file, destination: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveFile_NullFile_ShouldThrow()
    {
        var company = new Company("company");
        var destination = new Folder("destination", parent: company.Root);

        company.Root.Invoking(_ => _.MoveFile(file: null, destination))
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
        var folder = () => new Folder("name", parent: null);

        folder.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ConstructFolder_NullName_ShouldThrow()
    {
        var company = new Company("company");
        var folder = () => new Folder(name: null, parent: company.Root);

        folder.Should().ThrowExactly<ArgumentNullException>();
    }
}
