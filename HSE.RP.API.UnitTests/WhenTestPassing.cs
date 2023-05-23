using FluentAssertions;
using Xunit;

namespace HSE.PAC.API.UnitTests;

public class WhenTestPassing
{
    [Fact]
    public void PassingTest()
    {
        true.Should().BeTrue();
    }
}
