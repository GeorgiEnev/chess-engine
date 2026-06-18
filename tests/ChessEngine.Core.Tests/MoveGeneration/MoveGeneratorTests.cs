using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.MoveGeneration;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.MoveGeneration;

public sealed class MoveGeneratorTests
{
    [Fact]
    public void GenerateMoves_ForStartingPosition_GeneratesWhitePawnAndKnightMoves()
    {
        Position position = Position.CreateStartingPosition();

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(20, moves.Count);
        Assert.Contains(new Move(Square.FromName("e2"), Square.FromName("e3")), moves);
        Assert.Contains(new Move(Square.FromName("e2"), Square.FromName("e4")), moves);
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
        board.SetPiece(Square.FromName("f5"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

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

    [Fact]
    public void GenerateMoves_ForBishopInCenter_GeneratesDiagonalMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Bishop));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(13, moves.Count);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("e5")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("h8")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("a1")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("a7")), moves);
    }

    [Fact]
    public void GenerateMoves_ForRookInCenter_GeneratesStraightMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Rook));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(14, moves.Count);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("d8")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("h4")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("d1")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("a4")), moves);
    }

    [Fact]
    public void GenerateMoves_ForQueenInCenter_GeneratesDiagonalAndStraightMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Queen));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(27, moves.Count);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("h8")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("d8")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("a1")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("a4")), moves);
    }

    [Fact]
    public void GenerateMoves_ForSlidingPieceWithFriendlyBlocker_StopsBeforeFriendlyPiece()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Rook));
        board.SetPiece(Square.FromName("d6"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("d5")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("d4"), Square.FromName("d6")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("d4"), Square.FromName("d7")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("d4"), Square.FromName("d8")), moves);
    }

    [Fact]
    public void GenerateMoves_ForSlidingPieceWithEnemyBlocker_CapturesAndStops()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Rook));
        board.SetPiece(Square.FromName("d6"), new Piece(Color.Black, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("d5")), moves);
        Assert.Contains(new Move(Square.FromName("d4"), Square.FromName("d6")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("d4"), Square.FromName("d7")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("d4"), Square.FromName("d8")), moves);
    }

    [Fact]
    public void GenerateMoves_ForWhitePawnOnStartingRank_GeneratesOneAndTwoSquareForwardMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e2"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(2, moves.Count);
        Assert.Contains(new Move(Square.FromName("e2"), Square.FromName("e3")), moves);
        Assert.Contains(new Move(Square.FromName("e2"), Square.FromName("e4")), moves);
    }

    [Fact]
    public void GenerateMoves_ForBlackPawnOnStartingRank_GeneratesOneAndTwoSquareForwardMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e7"), new Piece(Color.Black, PieceType.Pawn));
        Position position = CreatePosition(board, Color.Black);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(2, moves.Count);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("e6")), moves);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("e5")), moves);
    }

    [Fact]
    public void GenerateMoves_ForPawnWithBlockedForwardSquare_GeneratesNoForwardMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e2"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("e3"), new Piece(Color.Black, PieceType.Knight));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Empty(moves);
    }

    [Fact]
    public void GenerateMoves_ForPawnWithBlockedTwoSquarePath_GeneratesOnlyOneSquareForwardMove()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e2"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("e4"), new Piece(Color.Black, PieceType.Knight));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Single(moves);
        Assert.Contains(new Move(Square.FromName("e2"), Square.FromName("e3")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("e2"), Square.FromName("e4")), moves);
    }

    [Fact]
    public void GenerateMoves_ForPawnPastStartingRank_GeneratesOnlyOneSquareForwardMove()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e3"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Single(moves);
        Assert.Contains(new Move(Square.FromName("e3"), Square.FromName("e4")), moves);
    }

    [Fact]
    public void GenerateMoves_ForWhitePawnWithEnemyDiagonalTargets_GeneratesCaptureMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e4"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("d5"), new Piece(Color.Black, PieceType.Knight));
        board.SetPiece(Square.FromName("f5"), new Piece(Color.Black, PieceType.Bishop));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(3, moves.Count);
        Assert.Contains(new Move(Square.FromName("e4"), Square.FromName("e5")), moves);
        Assert.Contains(new Move(Square.FromName("e4"), Square.FromName("d5")), moves);
        Assert.Contains(new Move(Square.FromName("e4"), Square.FromName("f5")), moves);
    }

    [Fact]
    public void GenerateMoves_ForBlackPawnWithEnemyDiagonalTargets_GeneratesCaptureMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e5"), new Piece(Color.Black, PieceType.Pawn));
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Knight));
        board.SetPiece(Square.FromName("f4"), new Piece(Color.White, PieceType.Bishop));
        Position position = CreatePosition(board, Color.Black);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(3, moves.Count);
        Assert.Contains(new Move(Square.FromName("e5"), Square.FromName("e4")), moves);
        Assert.Contains(new Move(Square.FromName("e5"), Square.FromName("d4")), moves);
        Assert.Contains(new Move(Square.FromName("e5"), Square.FromName("f4")), moves);
    }

    [Fact]
    public void GenerateMoves_ForPawnWithFriendlyDiagonalTargets_DoesNotGenerateCaptureMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e4"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("d5"), new Piece(Color.White, PieceType.Knight));
        board.SetPiece(Square.FromName("f5"), new Piece(Color.White, PieceType.Bishop));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.DoesNotContain(new Move(Square.FromName("e4"), Square.FromName("d5")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("e4"), Square.FromName("f5")), moves);
    }

    [Fact]
    public void GenerateMoves_ForPawnWithEmptyDiagonalTargets_DoesNotGenerateCaptureMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e4"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Single(moves);
        Assert.Contains(new Move(Square.FromName("e4"), Square.FromName("e5")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("e4"), Square.FromName("d5")), moves);
        Assert.DoesNotContain(new Move(Square.FromName("e4"), Square.FromName("f5")), moves);
    }

    [Fact]
    public void GenerateMoves_ForPawnOnBoardEdge_GeneratesOnlyCapturesInsideBoard()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("a4"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("b5"), new Piece(Color.Black, PieceType.Knight));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(2, moves.Count);
        Assert.Contains(new Move(Square.FromName("a4"), Square.FromName("a5")), moves);
        Assert.Contains(new Move(Square.FromName("a4"), Square.FromName("b5")), moves);
    }

    [Fact]
    public void GenerateMoves_ForWhitePawnMovingToLastRank_GeneratesPromotionMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e7"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(4, moves.Count);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("e8"), PieceType.Queen), moves);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("e8"), PieceType.Rook), moves);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("e8"), PieceType.Bishop), moves);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("e8"), PieceType.Knight), moves);
        Assert.DoesNotContain(new Move(Square.FromName("e7"), Square.FromName("e8")), moves);
    }

    [Fact]
    public void GenerateMoves_ForBlackPawnMovingToLastRank_GeneratesPromotionMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e2"), new Piece(Color.Black, PieceType.Pawn));
        Position position = CreatePosition(board, Color.Black);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(4, moves.Count);
        Assert.Contains(new Move(Square.FromName("e2"), Square.FromName("e1"), PieceType.Queen), moves);
        Assert.Contains(new Move(Square.FromName("e2"), Square.FromName("e1"), PieceType.Rook), moves);
        Assert.Contains(new Move(Square.FromName("e2"), Square.FromName("e1"), PieceType.Bishop), moves);
        Assert.Contains(new Move(Square.FromName("e2"), Square.FromName("e1"), PieceType.Knight), moves);
        Assert.DoesNotContain(new Move(Square.FromName("e2"), Square.FromName("e1")), moves);
    }

    [Fact]
    public void GenerateMoves_ForWhitePawnCapturingOnLastRank_GeneratesPromotionCaptureMoves()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e7"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("d8"), new Piece(Color.Black, PieceType.Rook));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(8, moves.Count);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("e8"), PieceType.Queen), moves);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("d8"), PieceType.Queen), moves);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("d8"), PieceType.Rook), moves);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("d8"), PieceType.Bishop), moves);
        Assert.Contains(new Move(Square.FromName("e7"), Square.FromName("d8"), PieceType.Knight), moves);
        Assert.DoesNotContain(new Move(Square.FromName("e7"), Square.FromName("d8")), moves);
    }

    [Fact]
    public void GenerateMoves_ForPromotion_DoesNotGenerateKingOrPawnPromotions()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e7"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.DoesNotContain(new Move(Square.FromName("e7"), Square.FromName("e8"), PieceType.King), moves);
        Assert.DoesNotContain(new Move(Square.FromName("e7"), Square.FromName("e8"), PieceType.Pawn), moves);
    }

    [Fact]
    public void GenerateMoves_ForWhitePawnWithEnPassantTarget_GeneratesEnPassantMove()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e5"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("d5"), new Piece(Color.Black, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White, Square.FromName("d6"));

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(2, moves.Count);
        Assert.Contains(new Move(Square.FromName("e5"), Square.FromName("e6")), moves);
        Assert.Contains(new Move(Square.FromName("e5"), Square.FromName("d6")), moves);
    }

    [Fact]
    public void GenerateMoves_ForBlackPawnWithEnPassantTarget_GeneratesEnPassantMove()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e4"), new Piece(Color.Black, PieceType.Pawn));
        board.SetPiece(Square.FromName("d4"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.Black, Square.FromName("d3"));

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.Equal(2, moves.Count);
        Assert.Contains(new Move(Square.FromName("e4"), Square.FromName("e3")), moves);
        Assert.Contains(new Move(Square.FromName("e4"), Square.FromName("d3")), moves);
    }

    [Fact]
    public void GenerateMoves_ForPawnWithoutEnPassantTarget_DoesNotGenerateEnPassantMove()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e5"), new Piece(Color.White, PieceType.Pawn));
        board.SetPiece(Square.FromName("d5"), new Piece(Color.Black, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White);

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.DoesNotContain(new Move(Square.FromName("e5"), Square.FromName("d6")), moves);
    }

    [Fact]
    public void GenerateMoves_ForPawnWithInvalidEnPassantTarget_DoesNotGenerateEnPassantMove()
    {
        ChessBoard board = ChessBoard.CreateEmpty();
        board.SetPiece(Square.FromName("e5"), new Piece(Color.White, PieceType.Pawn));
        Position position = CreatePosition(board, Color.White, Square.FromName("g6"));

        IReadOnlyList<Move> moves = MoveGenerator.GenerateMoves(position);

        Assert.DoesNotContain(new Move(Square.FromName("e5"), Square.FromName("g6")), moves);
    }

    private static Position CreatePosition(ChessBoard board, Color sideToMove)
    {
        return CreatePosition(board, sideToMove, enPassantTarget: null);
    }

    private static Position CreatePosition(ChessBoard board, Color sideToMove, Square? enPassantTarget)
    {
        return new Position(
            board,
            sideToMove,
            CastlingRights.None,
            enPassantTarget,
            halfmoveClock: 0,
            fullmoveNumber: 1);
    }
}
