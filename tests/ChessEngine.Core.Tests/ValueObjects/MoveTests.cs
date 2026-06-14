using ChessEngine.Core.Enums;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.ValueObjects;

public sealed class MoveTests
{
    [Fact]
    public void Move_StoresFromAndToSquares()
    {
        Square from = Square.FromFileRank(4, 1);
        Square to = Square.FromFileRank(4, 3);

        Move move = new(from, to);

        Assert.Equal(from, move.From);
        Assert.Equal(to, move.To);
    }

    [Fact]
    public void Move_WithoutPromotion_HasNoPromotionPiece()
    {
        Move move = new(Square.FromFileRank(4, 1), Square.FromFileRank(4, 3));

        Assert.Null(move.Promotion);
    }

    [Fact]
    public void Move_WithPromotion_StoresPromotionPiece()
    {
        Move move = new(
            Square.FromFileRank(4, 6),
            Square.FromFileRank(4, 7),
            PieceType.Queen);

        Assert.Equal(PieceType.Queen, move.Promotion);
    }

    [Fact]
    public void Move_WithSameSquaresAndPromotion_IsEqual()
    {
        Move first = new(Square.FromFileRank(0, 6), Square.FromFileRank(0, 7), PieceType.Queen);
        Move second = new(Square.FromFileRank(0, 6), Square.FromFileRank(0, 7), PieceType.Queen);

        Assert.Equal(first, second);
    }

    [Fact]
    public void Move_WithDifferentTargetSquare_IsNotEqual()
    {
        Move first = new(Square.FromFileRank(4, 1), Square.FromFileRank(4, 3));
        Move second = new(Square.FromFileRank(4, 1), Square.FromFileRank(4, 2));

        Assert.NotEqual(first, second);
    }
}
