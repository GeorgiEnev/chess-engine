using ChessEngine.Core.Enums;
using ChessEngine.Core.Geometry;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Attacks;

/// <summary>
/// Provides attack detection logic for chess positions.
/// </summary>
public static class AttackDetector
{
    /// <summary>
    /// Returns true when the square is attacked by at least one piece of the given color.
    /// </summary>
    /// <param name="position">The position to inspect.</param>
    /// <param name="square">The square being tested for attacks.</param>
    /// <param name="attackingColor">The color of the attacking side.</param>
    public static bool IsSquareAttacked(Position position, Square square, Color attackingColor)
    {
        ArgumentNullException.ThrowIfNull(position);

        return IsAttackedByPawn(position, square, attackingColor)
            || IsAttackedByKnight(position, square, attackingColor);
    }

    // Checks the possible squares from which a pawn could attack the target square.
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

    // Checks the possible squares from which a knight could attack the target square.
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
}
