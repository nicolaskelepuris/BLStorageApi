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
        var root = Folder.CreateRoot("root");
        var file = new File("file", parent: root);

        file.Parent.Should().Be(root);
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
}
