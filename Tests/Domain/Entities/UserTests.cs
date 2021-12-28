using System;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;
public class UserTests
{
    [Fact]
    public void UserConstructor_Valid_ShouldConstruct()
    {
        var company = new Company("company");

        var user = new User(company);

        user.Company.Should().Be(company);
    }

    [Fact]
    public void UserConstructor_NullCompany_ShouldThrow()
    {
        var user = () => new User(company: null);

        user.Should().ThrowExactly<ArgumentNullException>();
    }
}
