using ChessEngine.Core.Notation;

namespace ChessEngine.Core.Tests.Notation;

public sealed class FenRoundTripTests
{
    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
    [InlineData("8/8/8/8/8/8/8/8 b - - 12 34")]
    [InlineData("8/8/8/8/8/8/8/8 w Kq - 0 1")]
    [InlineData("8/8/8/8/8/8/8/8 b - e3 0 3")]
    [InlineData("8/8/8/8/3Q3k/8/8/8 w - - 17 42")]
    public void ParseThenSerialize_ReturnsOriginalFen(string fen)
    {
        string serializedFen = FenSerializer.Serialize(FenParser.Parse(fen));

        Assert.Equal(fen, serializedFen);
    }
}
