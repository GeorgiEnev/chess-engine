namespace ChessEngine.Core.Geometry;

/// <summary>
/// Provides reusable direction sets for board movement and attack detection.
/// </summary>
internal static class DirectionSets
{
    /// <summary>
    /// Represents the L-shaped directions used by knights.
    /// </summary>
    public static readonly Direction[] Knight =
    [
        new(1, 2),
        new(2, 1),
        new(2, -1),
        new(1, -2),
        new(-1, -2),
        new(-2, -1),
        new(-2, 1),
        new(-1, 2)
    ];

    /// <summary>
    /// Represents the diagonal directions used by bishops.
    /// </summary>
    public static readonly Direction[] Bishop =
    [
        new(1, 1),
        new(1, -1),
        new(-1, -1),
        new(-1, 1)
    ];

    /// <summary>
    /// Represents the straight directions used by rooks.
    /// </summary>
    public static readonly Direction[] Rook =
    [
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0)
    ];

    /// <summary>
    /// Represents the adjacent directions used by kings.
    /// </summary>
    public static readonly Direction[] King =
    [
        new(0, 1),
        new(1, 1),
        new(1, 0),
        new(1, -1),
        new(0, -1),
        new(-1, -1),
        new(-1, 0),
        new(-1, 1)
    ];
}
