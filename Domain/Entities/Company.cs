namespace Domain.Entities;
public class Company
{
    public Company(string name, Folder root)
    {
        Name = name;
        Root = root;
    }

    public string Name { get; }
    public Folder Root { get; private set; }
}
