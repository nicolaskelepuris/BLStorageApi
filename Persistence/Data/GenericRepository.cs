using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly DbContext _context;
    public GenericRepository(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public void Add(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetEntityByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<T>> ListAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<T> GetEntityAsyncWithSpec(ISpecification<T> spec)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(ISpecification<T> spec)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<T>> ListAsyncWithSpec(ISpecification<T> spec)
    {
        throw new NotImplementedException();
    }
}
