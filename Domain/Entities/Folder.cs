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

    private Folder(string name)
    {
        Name = name;
        subFolders = new List<Folder>();
        files = new List<File>();
    }

    public Folder(string name, Folder parent, Company company) : this(name, company)
    {
        ArgumentNullException.ThrowIfNull(parent);

        Parent = parent;
    }

    public static Folder CreateRoot(string name)
    {
        return new Folder(name);
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

    public void AddSubFolder(Folder subFolder)
    {
        ArgumentNullException.ThrowIfNull(subFolder);

        subFolder.Parent = this;
        subFolder.Company = Company;
        subFolders.Add(subFolder);
    }

    public void AddFile(File file)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(Company);

        file.Parent = this;
        file.Company = Company;
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
