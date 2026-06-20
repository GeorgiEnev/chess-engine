using ChessEngine.Core.Enums;
using ChessEngine.Core.Extensions;

namespace ChessEngine.Core.Tests.Enums;

public sealed class ColorTests
{
    [Fact]
    public void Color_HasExpectedSides()
    {
        Assert.Equal(0, (int)Color.White);
        Assert.Equal(1, (int)Color.Black);
    }

    [Fact]
    public void Opposite_ForWhite_ReturnsBlack()
    {
        Assert.Equal(Color.Black, Color.White.Opposite());
    }

    [Fact]
    public void Opposite_ForBlack_ReturnsWhite()
    {
        Assert.Equal(Color.White, Color.Black.Opposite());
    }
}
