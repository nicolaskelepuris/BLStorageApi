using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;

public class FolderTests
{
    [Fact]
    public void AddFolder_ValidFolder_ShouldAdd()
    {
        var folder = new Folder("name");
        var subFolder = new Folder("subFolder", parent: folder);

        folder.AddSubFolder(subFolder);

        folder.SubFolders.Should().HaveCount(1);
        folder.SubFolders.Should().Contain(subFolder);
        subFolder.Parent.Should().Be(folder);
    }
}
