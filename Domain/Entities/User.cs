using System;

namespace Domain.Entities;
public class User
{
    public User(string id, Company company, string email, string userName)
    {
        ArgumentNullException.ThrowIfNull(company);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(userName);

        Company = company;
        Email = email;
        UserName = userName;
        Id = id;
    }

    public Company Company { get; }
    public string Email { get; }
    public string UserName { get; }
    public string Id { get; }
}
