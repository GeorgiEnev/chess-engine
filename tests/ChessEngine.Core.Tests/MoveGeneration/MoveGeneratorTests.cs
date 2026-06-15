using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.MoveGeneration;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.MoveGeneration;

public sealed class MoveGeneratorTests
{
    [Fact]
    public void GenerateMoves_ForStartingPosition_GeneratesWhiteKnightMoves()
    {
        Position position = Position.CreateStartingPosition();

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(4, moves.Count);
        Assert.Contains(new Move(Square.FromName("b1"), Square.FromName("a3")), moves);
        Assert.Contains(new Move(Square.FromName("b1"), Square.FromName("c3")), moves);
        Assert.Contains(new Move(Square.FromName("g1"), Square.FromName("f3")), moves);
        Assert.Contains(new Move(Square.FromName("g1"), Square.FromName("h3")), moves);
    }

    [Fact]
    public void GenerateMoves_ForKnightInCenter_GeneratesEightMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Knight));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(8, moves.Count);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("e6")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("f5")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("f3")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("e2")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("c2")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("b3")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("b5")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("c6")), moves);
    }

    [Fact]
    public void GenerateMoves_ForKnightInCorner_GeneratesOnlyMovesInsideBoard()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("a1"), new Piece(Color.White, PieceType.Knight));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(2, moves.Count);
        Assert.Contains(new Move(Square.FromName("a1"), Square.FromName("b3")), moves);
        Assert.Contains(new Move(Square.FromName("a1"), Square.FromName("c2")), moves);
    }

    [Fact]
    public void GenerateMoves_ForKnightWithFriendlyTargets_SkipsFriendlyOccupiedSquares()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Knight));
        board.SetPiece(Square.FromName("e6"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("f5"), new Piece(Color.White, PieceType.Bishop));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(6, moves.Count);
        Assert.DoesNotContain(new Move(Square.FromName("d4"), Square.FromName("e6")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("d4"), Square.FromName("f5")), moves);
    }

    [Fact]
    public void GenerateMoves_ForKnightWithEnemyTargets_IncludesCaptures()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Knight));
        board.SetPiece(Square.FromName("e6"), new Piece(Color.Black, PieceType.Pawn));
        board.SetPiece(Square.FromName("f5"), new Piece(Color.Black, PieceType.Bishop));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(8, moves.Count);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("e6")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("f5")), moves);
    }

    [Fact]
    public void GenerateMoves_UsesSideToMove()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("b1"), new Piece(Color.White, PieceType.Knight));
        board.SetPiece(Square.FromName("g8"), new Piece(Color.Black, PieceType.Knight));
        Position position = CreatePosition(board, Color.Black);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(3, moves.Count);
        Assert.All(moves, move => Assert.Equal(Square.FromName("g8"), move.From));
        Assert.Contains(new Move(Square.FromName("g8"), Square.FromName("e7")), moves);
        Assert.Contains(new Move(Square.FromName("g8"), Square.FromName("f6")), moves);
        Assert.Contains(new Move(Square.FromName("g8"), Square.FromName("h6")), moves);
    }

    private static Position CreatePosition(ChessBoard board, Color sideToMove)
    {
        return new Position(
            board,
            sideToMove,
            CastlingRights.None,
            enPassantTarget: null,
            halfmoveClock: 0,
            fullmoveNumber: 1);
    }
}
