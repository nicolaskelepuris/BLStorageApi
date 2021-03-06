using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces;
public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
}
