using System;
using System.Linq;
using Domain.Entities.Base;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data;
public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    private IQueryable<TEntity> query;
    private readonly ISpecification<TEntity> _specification;
    public SpecificationEvaluator(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(specification);

        this.query = query;
        _specification = specification;
    }

    public IQueryable<TEntity> EvaluateForCount()
    {
        ApplyCriteria();

        return query;
    }

    public IQueryable<TEntity> Evaluate()
    {
        ApplyCriteria();
        ApplyOrderBy();
        ApplyOrderByDescending();
        ApplyIncludes();
        ApplyPagination();

        return query;
    }

    private void ApplyCriteria()
    {
        if (_specification.Criteria != null)
        {
            query = query.Where(_specification.Criteria);
        }
    }

    private void ApplyOrderBy()
    {
        if (_specification.OrderBy != null)
        {
            query = query.OrderBy(_specification.OrderBy);
        }
    }

    private void ApplyOrderByDescending()
    {
        if (_specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(_specification.OrderByDescending);
        }
    }

    private void ApplyPagination()
    {
        if (_specification.IsPaginationEnabled)
        {
            query = query.Skip(_specification.Skip).Take(_specification.Take);
        }
    }

    private void ApplyIncludes()
    {
        if (_specification.Includes.Any())
        {
            query = _specification.Includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (_specification.IncludesByString.Any())
        {
            query = _specification.IncludesByString.Aggregate(query, (current, include) => current.Include(include));
        }
    }
}
