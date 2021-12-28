using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;
public interface IContextHelper
{
    string GetUserId();
    Guid GetUserCompanyId();
    Task<User> GetUserAsync();
    bool IsAdminUser();
}
