using ChessEngine.Core.Enums;
using ChessEngine.Core.Notation;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.Notation;

public sealed class FenParserTests
{
    [Fact]
    public void Parse_WithStartingFen_CreatesStartingPosition()
    {
        Position position = FenParser.Parse("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

        Assert.Equal(Color.White, position.SideToMove);
        Assert.Equal(CastlingRights.All, position.CastlingRights);
        Assert.Null(position.EnPassantTarget);
        Assert.Equal(0, position.HalfmoveClock);
        Assert.Equal(1, position.FullmoveNumber);
        Assert.Equal(new Piece(Color.White, PieceType.King), position.Board.GetPiece(Square.FromFileRank(4, 0)));
        Assert.Equal(new Piece(Color.Black, PieceType.King), position.Board.GetPiece(Square.FromFileRank(4, 7)));
    }

    [Fact]
    public void Parse_WithEmptyBoard_CreatesEmptyBoardPosition()
    {
        Position position = FenParser.Parse("8/8/8/8/8/8/8/8 b - - 12 34");

        Assert.Equal(Color.Black, position.SideToMove);
        Assert.Equal(CastlingRights.None, position.CastlingRights);
        Assert.Null(position.EnPassantTarget);
        Assert.Equal(12, position.HalfmoveClock);
        Assert.Equal(34, position.FullmoveNumber);

        for (int index = 0; index < 64; index++)
        {
            Assert.True(position.Board.IsEmpty(new Square(index)));
        }
    }

    [Fact]
    public void Parse_WithEnPassantTarget_StoresTargetSquare()
    {
        Position position = FenParser.Parse("8/8/8/8/8/8/8/8 b - e3 0 3");

        Assert.Equal(Square.FromName("e3"), position.EnPassantTarget);
    }

    [Fact]
    public void Parse_WithPartialCastlingRights_StoresCastlingRights()
    {
        Position position = FenParser.Parse("8/8/8/8/8/8/8/8 w Kq - 0 1");

        Assert.Equal(CastlingRights.WhiteKingside | CastlingRights.BlackQueenside, position.CastlingRights);
    }

    [Theory]
    [InlineData("")]
    [InlineData("8/8/8/8/8/8/8/8 w - - 0")]
    [InlineData("8/8/8/8/8/8/8/8 w - - 0 1 extra")]
    public void Parse_WithWrongFieldCount_Throws(string fen)
    {
        Assert.Throws<ArgumentException>(() => FenParser.Parse(fen));
    }

    [Theory]
    [InlineData("8/8/8/8/8/8/8 w - - 0 1")]
    [InlineData("8/8/8/8/8/8/8/8/8 w - - 0 1")]
    [InlineData("9/8/8/8/8/8/8/8 w - - 0 1")]
    [InlineData("7/8/8/8/8/8/8/8 w - - 0 1")]
    [InlineData("11pppppp/8/8/8/8/8/8/8 w - - 0 1")]
    [InlineData("x7/8/8/8/8/8/8/8 w - - 0 1")]
    public void Parse_WithInvalidPiecePlacement_Throws(string fen)
    {
        Assert.Throws<ArgumentException>(() => FenParser.Parse(fen));
    }

    [Theory]
    [InlineData("8/8/8/8/8/8/8/8 x - - 0 1")]
    [InlineData("8/8/8/8/8/8/8/8 white - - 0 1")]
    public void Parse_WithInvalidSideToMove_Throws(string fen)
    {
        Assert.Throws<ArgumentException>(() => FenParser.Parse(fen));
    }

    [Theory]
    [InlineData("8/8/8/8/8/8/8/8 w A - 0 1")]
    [InlineData("8/8/8/8/8/8/8/8 w K- - 0 1")]
    [InlineData("8/8/8/8/8/8/8/8 w KK - 0 1")]
    public void Parse_WithInvalidCastlingRights_Throws(string fen)
    {
        Assert.Throws<ArgumentException>(() => FenParser.Parse(fen));
    }

    [Theory]
    [InlineData("8/8/8/8/8/8/8/8 w - z9 0 1")]
    [InlineData("8/8/8/8/8/8/8/8 w - e 0 1")]
    [InlineData("8/8/8/8/8/8/8/8 w - e4 0 1")]
    public void Parse_WithInvalidEnPassantTarget_Throws(string fen)
    {
        Assert.Throws<ArgumentException>(() => FenParser.Parse(fen));
    }

    [Theory]
    [InlineData("8/8/8/8/8/8/8/8 w - - -1 1")]
    [InlineData("8/8/8/8/8/8/8/8 w - - x 1")]
    public void Parse_WithInvalidHalfmoveClock_Throws(string fen)
    {
        Assert.Throws<ArgumentException>(() => FenParser.Parse(fen));
    }

    [Theory]
    [InlineData("8/8/8/8/8/8/8/8 w - - 0 0")]
    [InlineData("8/8/8/8/8/8/8/8 w - - 0 -1")]
    [InlineData("8/8/8/8/8/8/8/8 w - - 0 x")]
    public void Parse_WithInvalidFullmoveNumber_Throws(string fen)
    {
        Assert.Throws<ArgumentException>(() => FenParser.Parse(fen));
    }
}
