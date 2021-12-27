using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;
public class CompanyTests
{
    [Fact]
    public void ConstructCompany_ValidRoot_ShouldConstruct()
    {
        var root = Folder.CreateRoot("root");
        var company = new Company("company", root);

        company.Root.Should().Be(root);
    }
}
