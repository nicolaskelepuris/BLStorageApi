using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        throw new System.NotImplementedException();
    }
}