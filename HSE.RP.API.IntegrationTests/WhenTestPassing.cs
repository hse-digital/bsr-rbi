using FluentAssertions;
using Xunit;


namespace HSE.PAC.API.IntegrationTests;

public class WhenTestPassing
{
    [Fact]
    public void PassingTest()
    {
        true.Should().BeTrue();
    }
}
