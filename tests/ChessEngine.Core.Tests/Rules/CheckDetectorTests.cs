using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.Positions;
using ChessEngine.Core.Rules;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.Rules;

public sealed class CheckDetectorTests
{
    [Fact]
    public void IsInCheck_WhenWhiteKingIsAttackedByRook_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e1"), new Piece(Color.White, PieceType.King));
        board.SetPiece(Square.FromName("e8"), new Piece(Color.Black, PieceType.Rook));
        Position position = CreatePosition(board);

        bool isInCheck = CheckDetector.IsInCheck(position, Color.White);

        Assert.True(isInCheck);
    }

    [Fact]
    public void IsInCheck_WhenBlackKingIsAttackedByBishop_ReturnsTrue()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e8"), new Piece(Color.Black, PieceType.King));
        board.SetPiece(Square.FromName("h5"), new Piece(Color.White, PieceType.Bishop));
        Position position = CreatePosition(board);

        bool isInCheck = CheckDetector.IsInCheck(position, Color.Black);

        Assert.True(isInCheck);
    }

    [Fact]
    public void IsInCheck_WhenAttackIsBlocked_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e1"), new Piece(Color.White, PieceType.King));
        board.SetPiece(Square.FromName("e8"), new Piece(Color.Black, PieceType.Rook));
        board.SetPiece(Square.FromName("e4"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board);

        bool isInCheck = CheckDetector.IsInCheck(position, Color.White);

        Assert.False(isInCheck);
    }

    [Fact]
    public void IsInCheck_WhenKingIsAttackedOnlyBySameColorPiece_ReturnsFalse()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e1"), new Piece(Color.White, PieceType.King));
        board.SetPiece(Square.FromName("e8"), new Piece(Color.White, PieceType.Rook));
        Position position = CreatePosition(board);

        bool isInCheck = CheckDetector.IsInCheck(position, Color.White);

        Assert.False(isInCheck);
    }

    [Fact]
    public void IsInCheck_WhenKingIsMissing_ThrowsInvalidOperationException()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        Position position = CreatePosition(board);

        Assert.Throws<InvalidOperationException>(() => CheckDetector.IsInCheck(position, Color.White));
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
