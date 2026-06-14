using ChessEngine.Core.Enums;

namespace ChessEngine.Core.Tests.Enums;

public sealed class ColorTests
{
    [Fact]
    public void Color_HasExpectedSides()
    {
        Assert.Equal(0, (int)Color.White);
        Assert.Equal(1, (int)Color.Black);
    }
}
