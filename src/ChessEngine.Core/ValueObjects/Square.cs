namespace ChessEngine.Core.ValueObjects;

/// <summary>
/// Represents one square on a chess board.
/// </summary>
public readonly record struct Square
{
    /// <summary>
    /// Gets the zero-based square index, where a1 is 0 and h8 is 63.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets the zero-based file, where a is 0 and h is 7.
    /// </summary>
    public int File => Index % 8;

    /// <summary>
    /// Gets the zero-based rank, where rank 1 is 0 and rank 8 is 7.
    /// </summary>
    public int Rank => Index / 8;

    /// <summary>
    /// Creates a square from a zero-based square index.
    /// </summary>
    /// <param name="index">The square index from 0 to 63.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="index"/> is outside the valid board range.
    /// </exception>
    public Square(int index)
    {
        if (index is < 0 or > 63)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Square index must be between 0 and 63.");
        }

        Index = index;
    }

    /// <summary>
    /// Creates a square from zero-based file and rank coordinates.
    /// </summary>
    /// <param name="file">The file from 0 to 7, where a is 0 and h is 7.</param>
    /// <param name="rank">The rank from 0 to 7, where rank 1 is 0 and rank 8 is 7.</param>
    /// <returns>The square at the given file and rank.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="file"/> or <paramref name="rank"/> is outside the valid board range.
    /// </exception>
    public static Square FromFileRank(int file, int rank)
    {
        if (file is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(file), "File must be between 0 and 7.");
        }

        if (rank is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(rank), "Rank must be between 0 and 7.");
        }

        return new Square(rank * 8 + file);
    }

    /// <summary>
    /// Converts the square to algebraic coordinate notation, such as a1, e4, or h8.
    /// </summary>
    /// <returns>The square name in algebraic coordinate notation.</returns>
    public override string ToString()
    {
        char fileName = (char)('a' + File);
        char rankName = (char)('1' + Rank);

        return $"{fileName}{rankName}";
    }
}
