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
    public void EvaluateForCount_NotNullQueryAndSpecification_ShouldNotThrow()
    {
        var query = new List<SomeEntity>().AsQueryable();
        var specification = new Specification();
        var evaluator = new SpecificationEvaluator<SomeEntity>();
        
        var evaluateForCount = () => evaluator.EvaluateForCount(query, specification);

        evaluateForCount.Should().NotThrow<ArgumentNullException>();
    }

    [Fact]
    public void EvaluateForCount_NullQuery_ShouldThrow()
    {
        var specification = new Specification();
        var evaluator = new SpecificationEvaluator<SomeEntity>();

        var evaluateForCount = () => evaluator.EvaluateForCount(query: null!, specification);

        evaluateForCount.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void evaluateForCount_NullSpecification_ShouldThrow()
    {
        var query = new List<SomeEntity>().AsQueryable();
        var evaluator = new SpecificationEvaluator<SomeEntity>();

        var evaluateForCount = () => evaluator.EvaluateForCount(query, specification: null!);

        evaluateForCount.Should().ThrowExactly<ArgumentNullException>();
    }
}
