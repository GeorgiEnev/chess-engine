using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.Board;

public sealed class ChessBoardTests
{
    [Fact]
    public void CreateEmpty_CreatesBoardWithNoPieces()
    {
        ChessBoard board = ChessBoard.CreateEmpty();

        for (int index = 0; index < 64; index++)
        {
            Assert.True(board.IsEmpty(new Square(index)));
        }
    }

    [Fact]
    public void SetPiece_PlacesPieceOnSquare()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        Square square = Square.FromFileRank(4, 3);
        Piece piece = new(Color.White, PieceType.Queen);

        board.SetPiece(square, piece);

        Assert.Equal(piece, board.GetPiece(square));
        Assert.False(board.IsEmpty(square));
    }

    [Fact]
    public void RemovePiece_ClearsSquare()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        Square square = Square.FromFileRank(4, 3);

        board.SetPiece(square, new Piece(Color.White, PieceType.Queen));
        board.RemovePiece(square);

        Assert.Null(board.GetPiece(square));
        Assert.True(board.IsEmpty(square));
    }

    [Theory]
    [InlineData(0, PieceType.Rook)]
    [InlineData(1, PieceType.Knight)]
    [InlineData(2, PieceType.Bishop)]
    [InlineData(3, PieceType.Queen)]
    [InlineData(4, PieceType.King)]
    [InlineData(5, PieceType.Bishop)]
    [InlineData(6, PieceType.Knight)]
    [InlineData(7, PieceType.Rook)]
    public void CreateStartingBoard_PlacesWhiteBackRank(int file, PieceType expectedType)
    {
        ChessBoard board = ChessBoard.CreateStartingBoard();

        Assert.Equal(new Piece(Color.White, expectedType), board.GetPiece(Square.FromFileRank(file, 0)));
    }

    [Fact]
    public void CreateStartingBoard_PlacesWhitePawns()
    {
        ChessBoard board = ChessBoard.CreateStartingBoard();

        for (int file = 0; file < 8; file++)
        {
            Assert.Equal(new Piece(Color.White, PieceType.Pawn), board.GetPiece(Square.FromFileRank(file, 1)));
        }
    }

    [Theory]
    [InlineData(0, PieceType.Rook)]
    [InlineData(1, PieceType.Knight)]
    [InlineData(2, PieceType.Bishop)]
    [InlineData(3, PieceType.Queen)]
    [InlineData(4, PieceType.King)]
    [InlineData(5, PieceType.Bishop)]
    [InlineData(6, PieceType.Knight)]
    [InlineData(7, PieceType.Rook)]
    public void CreateStartingBoard_PlacesBlackBackRank(int file, PieceType expectedType)
    {
        ChessBoard board = ChessBoard.CreateStartingBoard();

        Assert.Equal(new Piece(Color.Black, expectedType), board.GetPiece(Square.FromFileRank(file, 7)));
    }

    [Fact]
    public void CreateStartingBoard_PlacesBlackPawns()
    {
        ChessBoard board = ChessBoard.CreateStartingBoard();

        for (int file = 0; file < 8; file++)
        {
            Assert.Equal(new Piece(Color.Black, PieceType.Pawn), board.GetPiece(Square.FromFileRank(file, 6)));
        }
    }

    [Fact]
    public void CreateStartingBoard_LeavesMiddleSquaresEmpty()
    {
        ChessBoard board = ChessBoard.CreateStartingBoard();

        for (int rank = 2; rank <= 5; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Assert.True(board.IsEmpty(Square.FromFileRank(file, rank)));
            }
        }
    }

    [Fact]
    public void Copy_CreatesSeparateBoardWithSamePiecePlacement()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b1"), new Piece(Color.White, PieceType.Knight));
        board.SetPiece(Square.FromName("e8"), new Piece(Color.Black, PieceType.King));

        ChessBoard copy = board.Copy();

        Assert.Equal(new Piece(Color.White, PieceType.Knight), copy.GetPiece(Square.FromName("b1")));
        Assert.Equal(new Piece(Color.Black, PieceType.King), copy.GetPiece(Square.FromName("e8")));
    }

    [Fact]
    public void Copy_WhenCopyIsChanged_DoesNotChangeOriginalBoard()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b1"), new Piece(Color.White, PieceType.Knight));

        ChessBoard copy = board.Copy();
        copy.RemovePiece(Square.FromName("b1"));
        copy.SetPiece(Square.FromName("c3"), new Piece(Color.White, PieceType.Knight));

        Assert.Equal(new Piece(Color.White, PieceType.Knight), board.GetPiece(Square.FromName("b1")));
        Assert.Null(board.GetPiece(Square.FromName("c3")));
    }
}
