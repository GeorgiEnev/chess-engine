using ChessEngine.Core.Enums;

namespace ChessEngine.Core.ValueObjects;

/// <summary>
/// Represents a chess move from one square to another, with an optional promotion piece type.
/// </summary>
/// <param name="From">The square the moving piece starts on.</param>
/// <param name="To">The square the moving piece lands on.</param>
/// <param name="Promotion">The piece type a pawn promotes to, or null for a non-promotion move.</param>
public readonly record struct Move(Square From, Square To, PieceType? Promotion = null);
