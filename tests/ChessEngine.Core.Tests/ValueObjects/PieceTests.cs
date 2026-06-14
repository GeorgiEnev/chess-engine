using ChessEngine.Core.Enums;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.ValueObjects;

public sealed class PieceTests
{
    [Fact]
    public void Piece_StoresColorAndType()
    {
        Piece piece = new(Color.White, PieceType.King);

        Assert.Equal(Color.White, piece.Color);
        Assert.Equal(PieceType.King, piece.Type);
    }

    [Fact]
    public void Piece_WithSameColorAndType_IsEqual()
    {
        Piece first = new(Color.Black, PieceType.Queen);
        Piece second = new(Color.Black, PieceType.Queen);

        Assert.Equal(first, second);
    }

    [Fact]
    public void Piece_WithDifferentColorOrType_IsNotEqual()
    {
        Piece whiteQueen = new(Color.White, PieceType.Queen);
        Piece blackQueen = new(Color.Black, PieceType.Queen);
        Piece whiteRook = new(Color.White, PieceType.Rook);

        Assert.NotEqual(whiteQueen, blackQueen);
        Assert.NotEqual(whiteQueen, whiteRook);
    }
}
