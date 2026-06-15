using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.ValueObjects;

public sealed class SquareTests
{
    [Theory]
    [InlineData(0, 0, 0, "a1")]
    [InlineData(7, 7, 0, "h1")]
    [InlineData(28, 4, 3, "e4")]
    [InlineData(63, 7, 7, "h8")]
    public void Square_FromIndex_StoresIndexFileAndRank(int index, int expectedFile, int expectedRank, string expectedName)
    {
        Square square = new(index);

        Assert.Equal(index, square.Index);
        Assert.Equal(expectedFile, square.File);
        Assert.Equal(expectedRank, square.Rank);
        Assert.Equal(expectedName, square.ToString());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(64)]
    public void Square_WithInvalidIndex_Throws(int index)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Square(index));
    }

    [Theory]
    [InlineData(0, 0, 0, "a1")]
    [InlineData(4, 3, 28, "e4")]
    [InlineData(7, 7, 63, "h8")]
    public void FromFileRank_CreatesExpectedSquare(int file, int rank, int expectedIndex, string expectedName)
    {
        Square square = Square.FromFileRank(file, rank);

        Assert.Equal(expectedIndex, square.Index);
        Assert.Equal(expectedName, square.ToString());
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(8, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 8)]
    public void FromFileRank_WithInvalidFileOrRank_Throws(int file, int rank)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Square.FromFileRank(file, rank));
    }

    [Theory]
    [InlineData("a1", 0)]
    [InlineData("e4", 28)]
    [InlineData("h8", 63)]
    public void FromName_CreatesExpectedSquare(string name, int expectedIndex)
    {
        Square square = Square.FromName(name);

        Assert.Equal(expectedIndex, square.Index);
        Assert.Equal(name, square.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("e")]
    [InlineData("e44")]
    [InlineData("i4")]
    [InlineData("e9")]
    [InlineData("E4")]
    public void FromName_WithInvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => Square.FromName(name));
    }

    [Fact]
    public void FromName_WithNullName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Square.FromName(null!));
    }
}
