using ChessEngine.Core.Enums;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.MoveGeneration;

/// <summary>
/// Provides move generation logic for chess positions.
/// </summary>
public static class MoveGenerator
{
    private static readonly (int FileOffset, int RankOffset)[] KnightOffsets =
    [
        (1, 2),
        (2, 1),
        (2, -1),
        (1, -2),
        (-1, -2),
        (-2, -1),
        (-2, 1),
        (-1, 2)
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

            if (piece.Value.Type == PieceType.Knight)
            {
                AddKnightMoves(position, from, piece.Value.Color, moves);
            }
        }

        return moves;
    }

    // Knights move by fixed file/rank offsets and can jump over nearby pieces.
    private static void AddKnightMoves(Position position, Square from, Color movingColor, List<Move> moves)
    {
        foreach ((int fileOffset, int rankOffset) in KnightOffsets)
        {
            int targetFile = from.File + fileOffset;
            int targetRank = from.Rank + rankOffset;

            if (!IsInsideBoard(targetFile, targetRank))
            {
                continue;
            }

            Square to = Square.FromFileRank(targetFile, targetRank);

            if (IsFriendlyPiece(position, to, movingColor))
            {
                continue;
            }

            moves.Add(new Move(from, to));
        }
    }

    private static bool IsInsideBoard(int file, int rank)
    {
        return file is >= 0 and <= 7 && rank is >= 0 and <= 7;
    }

    private static bool IsFriendlyPiece(Position position, Square square, Color movingColor)
    {
        Piece? targetPiece = position.Board.GetPiece(square);

        return targetPiece is not null && targetPiece.Value.Color == movingColor;
    }
}
