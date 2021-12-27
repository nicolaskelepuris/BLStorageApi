namespace Domain.Entities;

public class File
{
    public File(string name, Folder parent)
    {
        Name = name;
        Parent = parent;
    }

    public string Name { get; private set; }
    public Folder Parent { get; private set; }
}
