namespace Domain.Entities;

public class File
{
    public File(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
    public Folder? Parent { get; set; }
}
