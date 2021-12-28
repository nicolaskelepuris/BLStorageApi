using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;
public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
}
