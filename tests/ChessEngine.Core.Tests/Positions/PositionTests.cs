using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.Positions;

public sealed class PositionTests
{
    [Fact]
    public void CreateStartingPosition_CreatesStandardInitialState()
    {
        Position position = Position.CreateStartingPosition();

        Assert.Equal(Color.White, position.SideToMove);
        Assert.Equal(CastlingRights.All, position.CastlingRights);
        Assert.Null(position.EnPassantTarget);
        Assert.Equal(0, position.HalfmoveClock);
        Assert.Equal(1, position.FullmoveNumber);
    }

    [Fact]
    public void CreateStartingPosition_CreatesStandardInitialBoard()
    {
        Position position = Position.CreateStartingPosition();

        Assert.Equal(new Piece(Color.White, PieceType.King), position.Board.GetPiece(Square.FromFileRank(4, 0)));
        Assert.Equal(new Piece(Color.Black, PieceType.King), position.Board.GetPiece(Square.FromFileRank(4, 7)));
        Assert.Equal(new Piece(Color.White, PieceType.Pawn), position.Board.GetPiece(Square.FromFileRank(0, 1)));
        Assert.Equal(new Piece(Color.Black, PieceType.Pawn), position.Board.GetPiece(Square.FromFileRank(0, 6)));
    }

    [Fact]
    public void Constructor_StoresCustomState()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        Square enPassantTarget = Square.FromFileRank(4, 2);

        Position position = new(
            board,
            Color.Black,
            CastlingRights.BlackKingside,
            enPassantTarget,
            halfmoveClock: 12,
            fullmoveNumber: 8);

        Assert.Same(board, position.Board);
        Assert.Equal(Color.Black, position.SideToMove);
        Assert.Equal(CastlingRights.BlackKingside, position.CastlingRights);
        Assert.Equal(enPassantTarget, position.EnPassantTarget);
        Assert.Equal(12, position.HalfmoveClock);
        Assert.Equal(8, position.FullmoveNumber);
    }

    [Fact]
    public void Constructor_WithNegativeHalfmoveClock_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Position(
            ChessBoard.CreateEmpty(),
            Color.White,
            CastlingRights.None,
            enPassantTarget: null,
            halfmoveClock: -1,
            fullmoveNumber: 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidFullmoveNumber_Throws(int fullmoveNumber)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Position(
            ChessBoard.CreateEmpty(),
            Color.White,
            CastlingRights.None,
            enPassantTarget: null,
            halfmoveClock: 0,
            fullmoveNumber: fullmoveNumber));
    }
}
