using ChessEngine.Core.Enums;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.MoveGeneration;

/// <summary>
/// Provides move generation logic for chess positions.
/// </summary>
public static class MoveGenerator
{
    private static readonly Direction[] KnightDirections =
    [
        new(1, 2),
        new(2, 1),
        new(2, -1),
        new(1, -2),
        new(-1, -2),
        new(-2, -1),
        new(-2, 1),
        new(-1, 2)
    ];

    private static readonly Direction[] BishopDirections =
    [
        new(1, 1),
        new(1, -1),
        new(-1, -1),
        new(-1, 1)
    ];

    private static readonly Direction[] RookDirections =
    [
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0)
    ];

    /// <summary>
    /// Generates pseudo-legal moves for the side whose turn it is to move.
    /// </summary>
    /// <param name="position">The position to generate moves from.</param>
    /// <returns>The pseudo-legal moves available to the side to move.</returns>
    public static IReadOnlyList<Move> GenerateMoves(Position position)
    {
        ArgumentNullException.ThrowIfNull(position);

        List<Move> moves = [];

        for (int index = 0; index < 64; index++)
        {
            Square from = new(index);
            Piece? piece = position.Board.GetPiece(from);

            if (piece is null || piece.Value.Color != position.SideToMove)
            {
                continue;
            }

            switch (piece.Value.Type)
            {
                case PieceType.Pawn:
                    GeneratePawnMoves(position, from, piece.Value.Color, moves);
                    break;

                case PieceType.Knight:
                    GenerateKnightMoves(position, from, piece.Value.Color, moves);
                    break;

                case PieceType.Bishop:
                    GenerateSlidingMoves(position, from, piece.Value.Color, BishopDirections, moves);
                    break;

                case PieceType.Rook:
                    GenerateSlidingMoves(position, from, piece.Value.Color, RookDirections, moves);
                    break;

                // A queen combines bishop-style diagonal movement and rook-style straight movement.
                case PieceType.Queen:
                    GenerateSlidingMoves(position, from, piece.Value.Color, BishopDirections, moves);
                    GenerateSlidingMoves(position, from, piece.Value.Color, RookDirections, moves);
                    break;
            }
        }

        return moves;
    }

    // Generates pseudo-legal pawn moves by delegating each pawn rule to focused helpers.
    private static void GeneratePawnMoves(Position position, Square from, Color movingColor, List<Move> moves)
    {
        GeneratePawnForwardMoves(position, from, movingColor, moves);
    }

    // Generates pseudo-legal forward pawn moves, including the two-square move from the starting rank.
    private static void GeneratePawnForwardMoves(Position position, Square from, Color movingColor, List<Move> moves)
    {
        int forwardDirection = movingColor == Color.White ? 1 : -1;
        int startingRank = movingColor == Color.White ? 1 : 6;

        int oneStepRank = from.Rank + forwardDirection;

        if (!IsInsideBoard(from.File, oneStepRank))
        {
            return;
        }

        Square oneStep = Square.FromFileRank(from.File, oneStepRank);

        if (!position.Board.IsEmpty(oneStep))
        {
            return;
        }

        moves.Add(new Move(from, oneStep));

        if (from.Rank != startingRank)
        {
            return;
        }

        int twoStepRank = from.Rank + forwardDirection * 2;
        Square twoStep = Square.FromFileRank(from.File, twoStepRank);

        if (position.Board.IsEmpty(twoStep))
        {
            moves.Add(new Move(from, twoStep));
        }
    }

    // Generates pseudo-legal knight moves by checking each fixed L-shaped target square.
    private static void GenerateKnightMoves(Position position, Square from, Color movingColor, List<Move> moves)
    {
        foreach (Direction direction in KnightDirections)
        {
            int targetFile = from.File + direction.FileOffset;
            int targetRank = from.Rank + direction.RankOffset;

            if (!IsInsideBoard(targetFile, targetRank))
            {
                continue;
            }

            Square to = Square.FromFileRank(targetFile, targetRank);

            if (IsOccupiedByFriendlyPiece(position, to, movingColor))
            {
                continue;
            }

            moves.Add(new Move(from, to));
        }
    }

    // Generates pseudo-legal sliding moves for sliding pieces(bishop, rook, queen) 
    // by walking each direction until the board edge or a blocker is reached.
    private static void GenerateSlidingMoves(
        Position position,
        Square from,
        Color movingColor,
        IReadOnlyList<Direction> directions,
        List<Move> moves)
    {
        foreach (Direction direction in directions)
        {
            int targetFile = from.File + direction.FileOffset;
            int targetRank = from.Rank + direction.RankOffset;

            while (IsInsideBoard(targetFile, targetRank))
            {
                Square to = Square.FromFileRank(targetFile, targetRank);

                if (IsOccupiedByFriendlyPiece(position, to, movingColor))
                {
                    break;
                }

                moves.Add(new Move(from, to));

                if (IsOccupiedByEnemyPiece(position, to, movingColor))
                {
                    break;
                }

                targetFile += direction.FileOffset;
                targetRank += direction.RankOffset;
            }
        }
    }

    private static bool IsInsideBoard(int file, int rank)
    {
        return file is >= 0 and <= 7 && rank is >= 0 and <= 7;
    }

    private static bool IsOccupiedByFriendlyPiece(Position position, Square square, Color movingColor)
    {
        Piece? targetPiece = position.Board.GetPiece(square);

        return targetPiece is not null && targetPiece.Value.Color == movingColor;
    }

    private static bool IsOccupiedByEnemyPiece(Position position, Square square, Color movingColor)
    {
        Piece? targetPiece = position.Board.GetPiece(square);

        return targetPiece is not null && targetPiece.Value.Color != movingColor;
    }
}
