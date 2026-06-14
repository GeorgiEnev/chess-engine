using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.Notation;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.Notation;

public sealed class FenSerializerTests
{
    [Fact]
    public void Serialize_WithStartingPosition_ReturnsStartingFen()
    {
        string fen = FenSerializer.Serialize(Position.CreateStartingPosition());

        Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", fen);
    }

    [Fact]
    public void Serialize_WithEmptyBoard_ReturnsEmptyPiecePlacement()
    {
        Position position = new(
            ChessBoard.CreateEmpty(),
            Color.White,
            CastlingRights.None,
            enPassantTarget: null,
            halfmoveClock: 0,
            fullmoveNumber: 1);

        string fen = FenSerializer.Serialize(position);

        Assert.Equal("8/8/8/8/8/8/8/8 w - - 0 1", fen);
    }

    [Fact]
    public void Serialize_WithBlackToMove_WritesBlackSideToMove()
    {
        Position position = new(
            ChessBoard.CreateEmpty(),
            Color.Black,
            CastlingRights.None,
            enPassantTarget: null,
            halfmoveClock: 0,
            fullmoveNumber: 1);

        string fen = FenSerializer.Serialize(position);

        Assert.Equal("8/8/8/8/8/8/8/8 b - - 0 1", fen);
    }

    [Fact]
    public void Serialize_WithPartialCastlingRights_WritesRightsInFenOrder()
    {
        Position position = new(
            ChessBoard.CreateEmpty(),
            Color.White,
            CastlingRights.WhiteKingside | CastlingRights.BlackQueenside,
            enPassantTarget: null,
            halfmoveClock: 0,
            fullmoveNumber: 1);

        string fen = FenSerializer.Serialize(position);

        Assert.Equal("8/8/8/8/8/8/8/8 w Kq - 0 1", fen);
    }

    [Fact]
    public void Serialize_WithEnPassantTarget_WritesTargetSquare()
    {
        Position position = new(
            ChessBoard.CreateEmpty(),
            Color.Black,
            CastlingRights.None,
            Square.FromFileRank(4, 2),
            halfmoveClock: 0,
            fullmoveNumber: 3);

        string fen = FenSerializer.Serialize(position);

        Assert.Equal("8/8/8/8/8/8/8/8 b - e3 0 3", fen);
    }

    [Fact]
    public void Serialize_WithMoveClocks_WritesMoveClocks()
    {
        Position position = new(
            ChessBoard.CreateEmpty(),
            Color.White,
            CastlingRights.None,
            enPassantTarget: null,
            halfmoveClock: 17,
            fullmoveNumber: 42);

        string fen = FenSerializer.Serialize(position);

        Assert.Equal("8/8/8/8/8/8/8/8 w - - 17 42", fen);
    }

    [Fact]
    public void Serialize_WithPiecesAndEmptySquares_CompressesEmptySquaresByRank()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromFileRank(3, 3), new Piece(Color.White, PieceType.Queen));
        board.SetPiece(Square.FromFileRank(7, 3), new Piece(Color.Black, PieceType.King));

        Position position = new(
            board,
            Color.White,
            CastlingRights.None,
            enPassantTarget: null,
            halfmoveClock: 0,
            fullmoveNumber: 1);

        string fen = FenSerializer.Serialize(position);

        Assert.Equal("8/8/8/8/3Q3k/8/8/8 w - - 0 1", fen);
    }
}
