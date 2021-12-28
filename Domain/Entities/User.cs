using System;

namespace Domain.Entities;
public class User
{
    public User(Company company)
    {
        ArgumentNullException.ThrowIfNull(company);
        
        Company = company;
    }

    public Company Company { get; }
}
