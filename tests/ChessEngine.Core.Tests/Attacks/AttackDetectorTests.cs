using ChessEngine.Core.Attacks;
using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.Attacks;

public sealed class AttackDetectorTests
{
    [Fact]
    public void IsSquareAttacked_WhenWhitePawnAttacksSquare_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e5"),
            Color.White);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenBlackPawnAttacksSquare_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d5"), new Piece(Color.Black, PieceType.Pawn));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenPawnBelongsToDifferentColor_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e5"),
            Color.Black);

        Assert.False(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenPawnIsDirectlyInFrontOfSquare_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e4"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e5"),
            Color.White);

        Assert.False(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenTargetIsNearBoardEdge_SkipsOffBoardPawnSources()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b2"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("a3"),
            Color.White);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenKnightAttacksSquare_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("f5"), new Piece(Color.Black, PieceType.Knight));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e3"),
            Color.Black);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenKnightBelongsToDifferentColor_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("f5"), new Piece(Color.White, PieceType.Knight));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e3"),
            Color.Black);

        Assert.False(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenKnightDoesNotAttackSquare_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e5"), new Piece(Color.Black, PieceType.Knight));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e3"),
            Color.Black);

        Assert.False(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenTargetIsNearBoardEdge_SkipsOffBoardKnightSources()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b3"), new Piece(Color.Black, PieceType.Knight));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("a1"),
            Color.Black);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenKingAttacksAdjacentSquare_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e5"), new Piece(Color.Black, PieceType.King));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenKingBelongsToDifferentColor_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e5"), new Piece(Color.White, PieceType.King));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.False(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenKingIsTwoSquaresAway_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e6"), new Piece(Color.Black, PieceType.King));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.False(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenTargetIsNearBoardEdge_SkipsOffBoardKingSources()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b2"), new Piece(Color.Black, PieceType.King));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("a1"),
            Color.Black);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenBishopAttacksDiagonally_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("h7"), new Piece(Color.Black, PieceType.Bishop));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenRookAttacksStraight_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e8"), new Piece(Color.Black, PieceType.Rook));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenQueenAttacksDiagonally_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("h7"), new Piece(Color.Black, PieceType.Queen));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenQueenAttacksStraight_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e8"), new Piece(Color.Black, PieceType.Queen));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.True(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenSlidingPieceIsBlocked_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e8"), new Piece(Color.Black, PieceType.Rook));
        board.SetPiece(Square.FromName("e6"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.False(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenWrongSlidingPieceIsOnLine_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e8"), new Piece(Color.Black, PieceType.Bishop));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.False(isAttacked);
    }

    [Fact]
    public void IsSquareAttacked_WhenSlidingPieceBelongsToDifferentColor_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e8"), new Piece(Color.White, PieceType.Rook));
        Position position = CreatePosition(board);

        bool isAttacked = AttackDetector.IsSquareAttacked(
            position,
            Square.FromName("e4"),
            Color.Black);

        Assert.False(isAttacked);
    }

    private static Position CreatePosition(ChessBoard board)
    {
        return new Position(
            board,
            Color.White,
            CastlingRights.None,
            enPassantTarget: null,
            halfmoveClock: 0,
            fullmoveNumber: 1);
    }
}
