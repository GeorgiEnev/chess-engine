namespace ChessEngine.Core.Positions;

/// <summary>
/// Represents the castling options that are still available in a chess position.
/// </summary>
[Flags]
public enum CastlingRights
{
    None = 0,
    WhiteKingside = 1,
    WhiteQueenside = 2,
    BlackKingside = 4,
    BlackQueenside = 8,
    All = WhiteKingside | WhiteQueenside | BlackKingside | BlackQueenside
}
