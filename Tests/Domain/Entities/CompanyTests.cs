using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Tests.Domain.Entities;
public class CompanyTests
{
    [Fact]
    public void ConstructCompany_ValidRoot_ShouldConstruct()
    {
        var companyName = "company";
        var root = Folder.CreateRoot(companyName);
        var company = new Company(companyName, root);

        company.Root.Should().Be(root);
        company.Root.Name.Should().Be(company.Name);
    }
}