using System.Collections.Generic;
using Domain.Entities.Base;

namespace Domain.Entities;

public class Folder : BaseEntity
{
    public Folder(string name, ICollection<Folder> subFolders, ICollection<File> files, Folder? parent)
    {
        Name = name;
        this.subFolders = subFolders;
        this.files = files;
        Parent = parent;
    }

    public Folder(string name) : this(name, new List<Folder>(), new List<File>(), parent: null)
    {
    }

    public Folder(string name, Folder parent) : this(name, new List<Folder>(), new List<File>(), parent)
    {
    }

    public string Name { get; private set; }

    public IReadOnlyCollection<Folder> SubFolders
    {
        get { return new List<Folder>(subFolders).AsReadOnly(); }
    }
    private ICollection<Folder> subFolders;

    public IReadOnlyCollection<File> Files
    {
        get { return new List<File>(files).AsReadOnly(); }
    }
    private ICollection<File> files;

    public Folder? Parent { get; private set; }

    public void AddSubFolder(Folder folder)
    {
        subFolders.Add(folder);
    }
}
