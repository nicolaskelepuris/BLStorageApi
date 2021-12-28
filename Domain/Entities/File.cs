using System;

namespace Domain.Entities;

public class File
{
    public File(string name, Folder parent, Company company)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(company);

        Name = name;
        Parent = parent;
        Parent.MoveFile(this, Parent);
        Company = company;
    }

    public File(string name, Folder parent)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(parent.Company);

        Name = name;
        Parent = parent;
        Company = parent.Company;
        Parent.MoveFile(this, Parent);
    }

    public string Name { get; }
    public Folder Parent { get; set; }
    public Company Company { get; set; }

    public void MoveTo(Folder destination)
    {
        Parent.MoveFile(this, destination);
    }
}
