using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entities.Base;
using Domain.Interfaces;
using FluentAssertions;
using Persistence.Data;
using Xunit;

namespace Tests.Persistence.Data;
public class SpecificationEvaluatorTests
{
    private class SomeEntity : BaseEntity
    {
        
    }

    private class Specification : ISpecification<SomeEntity>
    {
        public Expression<Func<SomeEntity, bool>> Criteria => throw new NotImplementedException();

        public Expression<Func<SomeEntity, object>> OrderBy => throw new NotImplementedException();

        public Expression<Func<SomeEntity, object>> OrderByDescending => throw new NotImplementedException();

        public List<Expression<Func<SomeEntity, object>>> Includes => throw new NotImplementedException();

        public List<string> IncludesByString => throw new NotImplementedException();

        public int Take => throw new NotImplementedException();

        public int Skip => throw new NotImplementedException();

        public bool IsPaginationEnabled => throw new NotImplementedException();
    }

    [Fact]
    public void Construct_Valid_ShouldConstruct()
    {
        var query = new List<SomeEntity>().AsQueryable();
        var specification = new Specification();

        var evaluator = () => new SpecificationEvaluator<SomeEntity>(query, specification);

        evaluator.Should().NotThrow();
    }

    [Fact]
    public void Construct_NullQuery_ShouldThrow()
    {
        var specification = new Specification();

        var evaluator = () => new SpecificationEvaluator<SomeEntity>(query: null!, specification);

        evaluator.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Construct_NullSpecification_ShouldThrow()
    {
        var query = new List<SomeEntity>().AsQueryable();

        var evaluator = () => new SpecificationEvaluator<SomeEntity>(query, specification: null!);

        evaluator.Should().ThrowExactly<ArgumentNullException>();
    }
}
