using System;

namespace Domain.Entities;
public class Company
{
    public Company(string name, Folder root)
    {
        if (root.Name != name) throw new ArgumentException("Root folder has to have same name as Company name");

        Name = name;
        Root = root;
    }

    public string Name { get; }
    public Folder Root { get; private set; }
}