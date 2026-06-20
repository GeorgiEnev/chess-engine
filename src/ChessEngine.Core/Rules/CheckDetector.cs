using ChessEngine.Core.Attacks;
using ChessEngine.Core.Enums;
using ChessEngine.Core.Extensions;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Rules;

/// <summary>
/// Provides check detection logic for chess positions.
/// </summary>
public static class CheckDetector
{
    /// <summary>
    /// Returns true when the king of the given color is attacked by the opponent.
    /// </summary>
    /// <param name="position">The position to inspect.</param>
    /// <param name="color">The color whose king is being checked.</param>
    public static bool IsInCheck(Position position, Color color)
    {
        ArgumentNullException.ThrowIfNull(position);

        Square kingSquare = FindKingSquare(position, color);
        Color opponentColor = color.Opposite();

        return AttackDetector.IsSquareAttacked(position, kingSquare, opponentColor);
    }

    // Finds the king square for the color being tested for attack.
    private static Square FindKingSquare(Position position, Color kingColor)
    {
        for (int index = 0; index < 64; index++)
        {
            Square square = new(index);
            Piece? piece = position.Board.GetPiece(square);

            if (piece is { Type: PieceType.King, Color: var color } && color == kingColor)
            {
                return square;
            }
        }

        throw new InvalidOperationException("Position does not contain a king for the requested color.");
    }
}
