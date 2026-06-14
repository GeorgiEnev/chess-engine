using ChessEngine.Core.Enums;

namespace ChessEngine.Core.ValueObjects;

public readonly record struct Piece(Color Color, PieceType Type);
