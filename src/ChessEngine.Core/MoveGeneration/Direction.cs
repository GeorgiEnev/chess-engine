namespace ChessEngine.Core.MoveGeneration;

/// <summary>
/// Represents a file and rank offset used to move from one square toward another.
/// </summary>
/// <param name="FileOffset">The horizontal offset applied to a square file.</param>
/// <param name="RankOffset">The vertical offset applied to a square rank.</param>
internal readonly record struct Direction(int FileOffset, int RankOffset);
