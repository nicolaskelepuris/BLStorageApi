using System;

namespace Domain.Entities;
public class User
{
    public User(Company company, string email)
    {
        ArgumentNullException.ThrowIfNull(company);

        Company = company;
        Email = email;
    }

    public Company Company { get; }
    public string Email { get; set; }
}
