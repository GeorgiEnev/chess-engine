using ChessEngine.Core.Enums;
using ChessEngine.Core.Geometry;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Attacks;

/// <summary>
/// Provides attack detection logic for chess positions.
/// </summary>
/// <remarks>
/// Attack detection checks whether a square is controlled by a side according
/// to piece attack patterns. It does not validate whether the attacking side's
/// position is legal or whether the attacking piece is allowed to move without
/// exposing its own king.
/// </remarks>
public static class AttackDetector
{
    /// <summary>
    /// Returns true when the square is controlled by at least one piece of the attacking color.
    /// </summary>
    /// <param name="position">The position to inspect.</param>
    /// <param name="square">The square being tested for attacks.</param>
    /// <param name="attackingColor">The color of the attacking side.</param>
    public static bool IsSquareAttacked(Position position, Square square, Color attackingColor)
    {
        ArgumentNullException.ThrowIfNull(position);

        return IsAttackedByPawn(position, square, attackingColor)
            || IsAttackedByKnight(position, square, attackingColor)
            || IsAttackedByKing(position, square, attackingColor)
            || IsAttackedDiagonally(position, square, attackingColor)
            || IsAttackedStraight(position, square, attackingColor);
    }

    // Returns true if a pawn of the attacking color attacks the target square.
    private static bool IsAttackedByPawn(Position position, Square target, Color attackingColor)
    {
        int attackerRank = attackingColor == Color.White
            ? target.Rank - 1
            : target.Rank + 1;

        for (int fileOffset = -1; fileOffset <= 1; fileOffset += 2)
        {
            int attackerFile = target.File + fileOffset;

            if (!BoardGeometry.IsInsideBoard(attackerFile, attackerRank))
            {
                continue;
            }

            Square attackerSquare = Square.FromFileRank(attackerFile, attackerRank);
            Piece? attacker = position.Board.GetPiece(attackerSquare);

            if (attacker is { Type: PieceType.Pawn, Color: var color } && color == attackingColor)
            {
                return true;
            }
        }

        return false;
    }

    // Returns true if a knight of the attacking color attacks the target square.
    private static bool IsAttackedByKnight(Position position, Square target, Color attackingColor)
    {
        foreach (Direction direction in DirectionSets.Knight)
        {
            int attackerFile = target.File + direction.FileOffset;
            int attackerRank = target.Rank + direction.RankOffset;

            if (!BoardGeometry.IsInsideBoard(attackerFile, attackerRank))
            {
                continue;
            }

            Square attackerSquare = Square.FromFileRank(attackerFile, attackerRank);
            Piece? attacker = position.Board.GetPiece(attackerSquare);

            if (attacker is { Type: PieceType.Knight, Color: var color } && color == attackingColor)
            {
                return true;
            }
        }

        return false;
    }

    // Returns true if a king of the attacking color attacks the target square.
    private static bool IsAttackedByKing(Position position, Square target, Color attackingColor)
    {
        foreach (Direction direction in DirectionSets.King)
        {
            int attackerFile = target.File + direction.FileOffset;
            int attackerRank = target.Rank + direction.RankOffset;

            if (!BoardGeometry.IsInsideBoard(attackerFile, attackerRank))
            {
                continue;
            }

            Square attackerSquare = Square.FromFileRank(attackerFile, attackerRank);
            Piece? attacker = position.Board.GetPiece(attackerSquare);

            if (attacker is { Type: PieceType.King, Color: var color } && color == attackingColor)
            {
                return true;
            }
        }

        return false;
    }

    // Returns true if a bishop or queen of the attacking color attacks the target square diagonally.
    private static bool IsAttackedDiagonally(Position position, Square target, Color attackingColor)
    {
        return IsAttackedBySlidingPiece(
            position,
            target,
            attackingColor,
            DirectionSets.Bishop,
            PieceType.Bishop);
    }

    // Returns true if a rook or queen of the attacking color attacks the target square on a file or rank.
    private static bool IsAttackedStraight(Position position, Square target, Color attackingColor)
    {
        return IsAttackedBySlidingPiece(
            position,
            target,
            attackingColor,
            DirectionSets.Rook,
            PieceType.Rook);
    }

    // Returns true if a sliding piece of the attacking color controls the target square along a clear line.
    private static bool IsAttackedBySlidingPiece(
        Position position,
        Square target,
        Color attackingColor,
        IReadOnlyList<Direction> directions,
        PieceType slidingPieceType)
    {
        foreach (Direction direction in directions)
        {
            int attackerFile = target.File + direction.FileOffset;
            int attackerRank = target.Rank + direction.RankOffset;

            while (BoardGeometry.IsInsideBoard(attackerFile, attackerRank))
            {
                Square attackerSquare = Square.FromFileRank(attackerFile, attackerRank);
                Piece? attacker = position.Board.GetPiece(attackerSquare);

                if (attacker is null)
                {
                    attackerFile += direction.FileOffset;
                    attackerRank += direction.RankOffset;
                    continue;
                }

                if (attacker.Value.Color == attackingColor
                    && (attacker.Value.Type == slidingPieceType || attacker.Value.Type == PieceType.Queen))
                {
                    return true;
                }

                break;
            }
        }

        return false;
    }
}
