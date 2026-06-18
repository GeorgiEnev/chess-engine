namespace ChessEngine.Core.Geometry;

/// <summary>
/// Provides shared board-coordinate helper logic.
/// </summary>
internal static class BoardGeometry
{
    /// <summary>
    /// Returns true when the file and rank are inside the 8x8 chess board.
    /// </summary>
    public static bool IsInsideBoard(int file, int rank)
    {
        return file is >= 0 and <= 7 && rank is >= 0 and <= 7;
    }
}
