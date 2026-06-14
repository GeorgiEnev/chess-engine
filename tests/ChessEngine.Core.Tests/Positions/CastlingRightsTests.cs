using ChessEngine.Core.Positions;

namespace ChessEngine.Core.Tests.Positions;

public sealed class CastlingRightsTests
{
    [Fact]
    public void CastlingRights_All_IncludesAllCastlingOptions()
    {
        Assert.True(CastlingRights.All.HasFlag(CastlingRights.WhiteKingside));
        Assert.True(CastlingRights.All.HasFlag(CastlingRights.WhiteQueenside));
        Assert.True(CastlingRights.All.HasFlag(CastlingRights.BlackKingside));
        Assert.True(CastlingRights.All.HasFlag(CastlingRights.BlackQueenside));
    }

    [Fact]
    public void CastlingRights_None_HasNoCastlingOptions()
    {
        Assert.False(CastlingRights.None.HasFlag(CastlingRights.WhiteKingside));
        Assert.False(CastlingRights.None.HasFlag(CastlingRights.WhiteQueenside));
        Assert.False(CastlingRights.None.HasFlag(CastlingRights.BlackKingside));
        Assert.False(CastlingRights.None.HasFlag(CastlingRights.BlackQueenside));
    }
}
