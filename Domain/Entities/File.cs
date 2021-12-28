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
        Company = company;
    }

    public string Name { get; }
    public Folder Parent { get; set; }
    public Company Company { get; set; }
}
