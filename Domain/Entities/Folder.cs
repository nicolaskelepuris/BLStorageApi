using System;
using System.Collections.Generic;
using Domain.Entities.Base;

namespace Domain.Entities;

public class Folder : BaseEntity
{
    private Folder(string name, Company company)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(company);

        Name = name;
        Company = company;
        subFolders = new List<Folder>();
        files = new List<File>();
    }

    public Folder(string name, Folder parent) : this(name, parent?.Company ?? throw new ArgumentNullException(nameof(parent)))
    {
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(parent.Company);

        Parent = parent;
        Parent.AddSubFolder(this);
    }

    public static Folder CreateRoot(string name, Company company)
    {
        return new Folder(name, company);
    }

    public string Name { get; }

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
    public Company Company { get; set; }

    private void AddSubFolder(Folder subFolder)
    {
        ArgumentNullException.ThrowIfNull(subFolder);
        if (subFolders.Contains(subFolder)) return;

        subFolder.Parent = this;
        subFolder.Company = Company;
        subFolders.Add(subFolder);
    }

    private void AddFile(File file)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(Company);
        if (files.Contains(file)) return;

        file.Parent = this;
        file.Company = Company;
        files.Add(file);
    }

    public void MoveSubFolder(Folder subFolder, Folder destination)
    {
        ArgumentNullException.ThrowIfNull(subFolder);
        ArgumentNullException.ThrowIfNull(destination);
        if (this == destination) return;

        subFolders.Remove(subFolder);
        destination.AddSubFolder(subFolder);
        subFolder.Company = destination.Company;
    }

    public void MoveFile(File file, Folder destination)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(destination);

        files.Remove(file);
        destination.AddFile(file);
    }
}
