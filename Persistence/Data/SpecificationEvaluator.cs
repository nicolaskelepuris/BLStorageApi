using System;
using System.Linq;
using Domain.Entities.Base;
using Domain.Interfaces;

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
}
