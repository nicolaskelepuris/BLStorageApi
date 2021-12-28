using System;

namespace Domain.Entities;
public class Company
{
    public Company(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        Root = Folder.CreateRoot(name, this);
    }

    public string Name { get; }
    public Folder Root { get; private set; }
}
