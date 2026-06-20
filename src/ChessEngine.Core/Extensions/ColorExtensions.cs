using ChessEngine.Core.Enums;

namespace ChessEngine.Core.Extensions;

/// <summary>
/// Provides extension methods for chess colors.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Returns the opposite chess color.
    /// </summary>
    public static Color Opposite(this Color color)
    {
        return color == Color.White ? Color.Black : Color.White;
    }
}
