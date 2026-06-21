using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.Moves;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.Moves;

public sealed class MoveApplierTests
{
    [Fact]
    public void Apply_ForNormalMove_MovesPieceToTargetSquare()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b1"), new Piece(Color.White, PieceType.Knight));
        Position position = CreatePosition(board, Color.White);
        Move move = new(Square.FromName("b1"), Square.FromName("c3"));

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Null(nextPosition.Board.GetPiece(Square.FromName("b1")));
        Assert.Equal(new Piece(Color.White, PieceType.Knight), nextPosition.Board.GetPiece(Square.FromName("c3")));
    }

    [Fact]
    public void Apply_DoesNotMutateOriginalPosition()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b1"), new Piece(Color.White, PieceType.Knight));
        Position position = CreatePosition(board, Color.White);
        Move move = new(Square.FromName("b1"), Square.FromName("c3"));

        MoveApplier.Apply(position, move);

        Assert.Equal(new Piece(Color.White, PieceType.Knight), position.Board.GetPiece(Square.FromName("b1")));
        Assert.Null(position.Board.GetPiece(Square.FromName("c3")));
    }

    [Fact]
    public void Apply_ForCapture_ReplacesTargetPiece()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("a1"), new Piece(Color.White, PieceType.Rook));
        board.SetPiece(Square.FromName("a8"), new Piece(Color.Black, PieceType.Knight));
        Position position = CreatePosition(board, Color.White);
        Move move = new(Square.FromName("a1"), Square.FromName("a8"));

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Null(nextPosition.Board.GetPiece(Square.FromName("a1")));
        Assert.Equal(new Piece(Color.White, PieceType.Rook), nextPosition.Board.GetPiece(Square.FromName("a8")));
    }

    [Fact]
    public void Apply_SwitchesSideToMove()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b1"), new Piece(Color.White, PieceType.Knight));
        Position position = CreatePosition(board, Color.White);
        Move move = new(Square.FromName("b1"), Square.FromName("c3"));

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Equal(Color.Black, nextPosition.SideToMove);
    }

    [Fact]
    public void Apply_AfterBlackMove_IncrementsFullmoveNumber()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b8"), new Piece(Color.Black, PieceType.Knight));
        Position position = CreatePosition(board, Color.Black, fullmoveNumber: 1);
        Move move = new(Square.FromName("b8"), Square.FromName("c6"));

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Equal(2, nextPosition.FullmoveNumber);
    }

    [Fact]
    public void Apply_AfterWhiteMove_KeepsFullmoveNumber()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b1"), new Piece(Color.White, PieceType.Knight));
        Position position = CreatePosition(board, Color.White, fullmoveNumber: 4);
        Move move = new(Square.FromName("b1"), Square.FromName("c3"));

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Equal(4, nextPosition.FullmoveNumber);
    }

    [Fact]
    public void Apply_ClearsEnPassantTarget()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b1"), new Piece(Color.White, PieceType.Knight));
        Position position = CreatePosition(board, Color.White, enPassantTarget: Square.FromName("e3"));
        Move move = new(Square.FromName("b1"), Square.FromName("c3"));

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Null(nextPosition.EnPassantTarget);
    }

    [Fact]
    public void Apply_PreservesCastlingRightsAndHalfmoveClockForBasicMoveApplication()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b1"), new Piece(Color.White, PieceType.Knight));
        Position position = CreatePosition(
            board,
            Color.White,
            castlingRights: CastlingRights.WhiteKingside,
            halfmoveClock: 12);
        Move move = new(Square.FromName("b1"), Square.FromName("c3"));

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Equal(CastlingRights.WhiteKingside, nextPosition.CastlingRights);
        Assert.Equal(12, nextPosition.HalfmoveClock);
    }

    [Fact]
    public void Apply_WhenSourceSquareIsEmpty_ThrowsInvalidOperationException()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        Position position = CreatePosition(board, Color.White);
        Move move = new(Square.FromName("b1"), Square.FromName("c3"));

        Assert.Throws<InvalidOperationException>(() => MoveApplier.Apply(position, move));
    }

    [Fact]
    public void Apply_ForWhitePawnPromotion_PlacesPromotedPiece()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e7"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);
        Move move = new(Square.FromName("e7"), Square.FromName("e8"), PieceType.Queen);

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Null(nextPosition.Board.GetPiece(Square.FromName("e7")));
        Assert.Equal(new Piece(Color.White, PieceType.Queen), nextPosition.Board.GetPiece(Square.FromName("e8")));
    }

    [Fact]
    public void Apply_ForBlackPawnPromotion_PlacesPromotedPiece()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e2"), new Piece(Color.Black, PieceType.Pawn));
        Position position = CreatePosition(board, Color.Black);
        Move move = new(Square.FromName("e2"), Square.FromName("e1"), PieceType.Knight);

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Null(nextPosition.Board.GetPiece(Square.FromName("e2")));
        Assert.Equal(new Piece(Color.Black, PieceType.Knight), nextPosition.Board.GetPiece(Square.FromName("e1")));
    }

    [Fact]
    public void Apply_ForPromotionCapture_ReplacesTargetWithPromotedPiece()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e7"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("d8"), new Piece(Color.Black, PieceType.Rook));
        Position position = CreatePosition(board, Color.White);
        Move move = new(Square.FromName("e7"), Square.FromName("d8"), PieceType.Queen);

        Position nextPosition = MoveApplier.Apply(position, move);

        Assert.Null(nextPosition.Board.GetPiece(Square.FromName("e7")));
        Assert.Equal(new Piece(Color.White, PieceType.Queen), nextPosition.Board.GetPiece(Square.FromName("d8")));
    }

    [Fact]
    public void Apply_ForPromotion_DoesNotMutateOriginalPosition()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e7"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);
        Move move = new(Square.FromName("e7"), Square.FromName("e8"), PieceType.Queen);

        MoveApplier.Apply(position, move);

        Assert.Equal(new Piece(Color.White, PieceType.Pawn), position.Board.GetPiece(Square.FromName("e7")));
        Assert.Null(position.Board.GetPiece(Square.FromName("e8")));
    }

    private static Position CreatePosition(
        ChessBoard board,
        Color sideToMove,
        CastlingRights castlingRights = CastlingRights.None,
        Square? enPassantTarget = null,
        int halfmoveClock = 0,
        int fullmoveNumber = 1)
    {
        return new Position(
            board,
            sideToMove,
            castlingRights,
            enPassantTarget,
            halfmoveClock,
            fullmoveNumber);
    }
}
