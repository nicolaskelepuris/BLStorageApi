using System;
using System.Collections.Generic;
using Domain.Entities.Base;

namespace Domain.Entities;

public class Folder : BaseEntity
{
    public Folder(string name, ICollection<Folder> subFolders, ICollection<File> files)
    {
        Name = name;
        this.subFolders = subFolders;
        this.files = files;
    }

    public Folder(string name) : this(name, new List<Folder>(), new List<File>())
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

    public void AddSubFolder(Folder subFolder)
    {
        ArgumentNullException.ThrowIfNull(subFolder);

        subFolder.Parent = this;
        subFolders.Add(subFolder);
    }

    public void AddFile(File file)
    {
        ArgumentNullException.ThrowIfNull(file);

        file.Parent = this;
        files.Add(file);
    }

    public void MoveSubFolder(Folder subFolder, Folder destination)
    {
        ArgumentNullException.ThrowIfNull(subFolder);
        ArgumentNullException.ThrowIfNull(destination);

        subFolders.Remove(subFolder);
        destination.AddSubFolder(subFolder);
    }

    public void MoveFile(File file, Folder destination)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(destination);

        files.Remove(file);
        destination.AddFile(file);
    }
}
