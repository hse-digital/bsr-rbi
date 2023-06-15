using FluentAssertions;
using Xunit;


namespace HSE.RP.API.IntegrationTests;

public class WhenTestPassing
{
    [Fact]
    public void PassingTest()
    {
        true.Should().BeTrue();
    }
}
