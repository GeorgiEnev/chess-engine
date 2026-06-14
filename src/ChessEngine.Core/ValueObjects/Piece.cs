using ChessEngine.Core.Enums;

namespace ChessEngine.Core.ValueObjects;

/// <summary>
/// Represents a chess piece by combining its color and type.
/// </summary>
/// <param name="Color">The side the piece belongs to.</param>
/// <param name="Type">The type of the chess piece.</param>
public readonly record struct Piece(Color Color, PieceType Type);
