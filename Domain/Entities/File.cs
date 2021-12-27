using System;

namespace Domain.Entities;

public class File
{
    public File(string name, Folder parent)
    {
        ArgumentNullException.ThrowIfNull(parent);
        
        Name = name;
        Parent = parent;
    }

    public string Name { get; }
    public Folder Parent { get; set; }
}
