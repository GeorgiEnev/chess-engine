using ChessEngine.Core.Enums;

namespace ChessEngine.Core.Tests.Enums;

public sealed class PieceTypeTests
{
    [Fact]
    public void PieceType_HasExpectedPieceKinds()
    {
        Assert.Equal(0, (int)PieceType.Pawn);
        Assert.Equal(1, (int)PieceType.Knight);
        Assert.Equal(2, (int)PieceType.Bishop);
        Assert.Equal(3, (int)PieceType.Rook);
        Assert.Equal(4, (int)PieceType.Queen);
        Assert.Equal(5, (int)PieceType.King);
    }
}
