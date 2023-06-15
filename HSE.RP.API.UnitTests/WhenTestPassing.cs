using FluentAssertions;
using Xunit;

namespace HSE.RP.API.UnitTests;

public class WhenTestPassing
{
    [Fact]
    public void PassingTest()
    {
        true.Should().BeTrue();
    }
}
