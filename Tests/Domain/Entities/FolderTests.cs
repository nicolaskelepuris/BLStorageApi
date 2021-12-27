using System.Collections.Generic;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;

public class FolderTests
{
    [Fact]
    public void AddFolder_EmptySubFolders_ShouldAdd()
    {
        var folder = new Folder("name");
        var subFolder = new Folder("subFolder");

        folder.AddSubFolder(subFolder);

        folder.SubFolders.Should().HaveCount(1);
        folder.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(folder);
    }

    [Fact]
    public void AddFolder_NotEmptySubFolders_ShouldAdd()
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
}
