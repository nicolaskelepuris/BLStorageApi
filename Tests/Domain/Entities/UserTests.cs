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

        var user = new User(company, "email", "userName");

        user.Company.Should().Be(company);
        user.Email.Should().Be("email");
        user.UserName.Should().Be("userName");
    }

    [Fact]
    public void UserConstructor_NullCompany_ShouldThrow()
    {
        var user = () => new User(company: null, "email", "userName");

        user.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void UserConstructor_NullEmail_ShouldThrow()
    {
        var company = new Company("company");
        var user = () => new User(company, email: null, "userName");

        user.Should().ThrowExactly<ArgumentNullException>();
    }
}
