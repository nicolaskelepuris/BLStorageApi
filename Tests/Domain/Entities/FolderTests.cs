using System;
using System.Collections.Generic;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;

public class FolderTests
{
    [Fact]
    public void AddSubFolder_EmptySubFolders_ShouldAdd()
    {
        var folder = new Folder("name");
        var subFolder = new Folder("subFolder");

        folder.AddSubFolder(subFolder);

        folder.SubFolders.Should().HaveCount(1);
        folder.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(folder);
    }

    [Fact]
    public void AddSubFolder_NotEmptySubFolders_ShouldAdd()
    {
        var subFolders = new List<Folder>(){
            new Folder("any subFolder")
        };
        var folder = new Folder("name", subFolders, new List<File>());
        var subFolder = new Folder("subFolder");

        folder.AddSubFolder(subFolder);

        folder.SubFolders.Should().HaveCount(2);
        folder.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(folder);
    }

    [Fact]
    public void AddSubFolder_NullSubFolder_ShouldThrow()
    {
        var folder = new Folder("name");

        folder.Invoking(_ => _.AddSubFolder(subFolder: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void AddFile_EmptyFiles_ShouldAdd()
    {
        var folder = new Folder("name");
        var file = new File("file");

        folder.AddFile(file);

        folder.Files.Should().HaveCount(1);
        folder.Files.Should().Contain(file);
        file.Parent.Should().Be(folder);
    }

    [Fact]
    public void AddFile_NotEmptyFiles_ShouldAdd()
    {
        var files = new List<File>(){
            new File("any file")
        };
        var folder = new Folder("name", new List<Folder>(), files);
        var file = new File("file");

        folder.AddFile(file);

        folder.Files.Should().HaveCount(2);
        folder.Files.Should().Contain(file);
        file.Parent.Should().Be(folder);
    }

    [Fact]
    public void AddFile_NullFile_ShouldThrow()
    {
        var folder = new Folder("name");

        folder.Invoking(_ => _.AddFile(file: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveSubFolder_ValidSubFolder_ShouldMove()
    {
        var folder = new Folder("name");
        var subFolder = new Folder("subFolder");
        folder.AddSubFolder(subFolder);
        var destination = new Folder("another folder");

        folder.MoveSubFolder(subFolder, destination);

        folder.SubFolders.Should().BeEmpty();
        destination.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(destination);
    }

    [Fact]
    public void MoveSubFolder_NotFoundSubFolder_ShouldMove()
    {
        var folder = new Folder("name");
        var subFolder = new Folder("subFolder");
        var destination = new Folder("another folder");

        folder.MoveSubFolder(subFolder, destination);

        folder.SubFolders.Should().BeEmpty();
        destination.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(destination);
    }

    [Fact]
    public void MoveSubFolder_ToNullDestination_ShouldThrow()
    {
        var folder = new Folder("name");
        var subFolder = new Folder("subFolder");
        folder.Invoking(_ => _.MoveSubFolder(subFolder, destination: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveSubFolder_NullSubFolder_ShouldThrow()
    {
        var folder = new Folder("name");
        var destination = new Folder("destination");

        folder.Invoking(_ => _.MoveSubFolder(subFolder: null, destination))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveFile_ValidFile_ShouldMove()
    {
        var folder = new Folder("name");
        var file = new File("file");
        folder.AddFile(file);
        var anotherFolder = new Folder("another folder");

        folder.MoveFile(file, anotherFolder);

        folder.Files.Should().BeEmpty();
        anotherFolder.Files.Should().Contain(file);
        file.Parent.Should().Be(anotherFolder);
    }

    [Fact]
    public void MoveFile_NotFoundFile_ShouldMove()
    {
        var folder = new Folder("name");
        var file = new File("file");
        var anotherFolder = new Folder("another folder");

        folder.MoveFile(file, anotherFolder);

        folder.Files.Should().BeEmpty();
        anotherFolder.Files.Should().Contain(file);
        file.Parent.Should().Be(anotherFolder);
    }

    [Fact]
    public void MoveFile_ToNullDestination_ShouldMove()
    {
        var folder = new Folder("name");
        var file = new File("file");

        folder.Invoking(_ => _.MoveFile(file, destination: null))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void MoveFile_NullFile_ShouldThrow()
    {
        var folder = new Folder("name");
        var destination = new Folder("destination");

        folder.Invoking(_ => _.MoveFile(file: null, destination))
            .Should().ThrowExactly<ArgumentNullException>();
    }
}
