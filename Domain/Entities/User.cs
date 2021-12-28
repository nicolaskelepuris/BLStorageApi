using System;

namespace Domain.Entities;
public class User
{
    public User(Company company, string email, string userName)
    {
        ArgumentNullException.ThrowIfNull(company);
        ArgumentNullException.ThrowIfNull(email);

        Company = company;
        Email = email;
        UserName = userName;
    }

    public Company Company { get; }
    public string Email { get; set; }
    public string UserName { get; set; }
}
