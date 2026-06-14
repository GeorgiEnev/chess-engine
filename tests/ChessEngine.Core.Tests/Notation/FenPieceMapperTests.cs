using ChessEngine.Core.Enums;
using ChessEngine.Core.Notation;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Tests.Notation;

public sealed class FenPieceMapperTests
{
    [Theory]
    [InlineData('P', Color.White, PieceType.Pawn)]
    [InlineData('N', Color.White, PieceType.Knight)]
    [InlineData('B', Color.White, PieceType.Bishop)]
    [InlineData('R', Color.White, PieceType.Rook)]
    [InlineData('Q', Color.White, PieceType.Queen)]
    [InlineData('K', Color.White, PieceType.King)]
    [InlineData('p', Color.Black, PieceType.Pawn)]
    [InlineData('n', Color.Black, PieceType.Knight)]
    [InlineData('b', Color.Black, PieceType.Bishop)]
    [InlineData('r', Color.Black, PieceType.Rook)]
    [InlineData('q', Color.Black, PieceType.Queen)]
    [InlineData('k', Color.Black, PieceType.King)]
    public void ToPiece_ConvertsFenCharacterToPiece(char fenChar, Color expectedColor, PieceType expectedType)
    {
        Piece piece = FenPieceMapper.ToPiece(fenChar);

        Assert.Equal(new Piece(expectedColor, expectedType), piece);
    }

    [Theory]
    [InlineData(Color.White, PieceType.Pawn, 'P')]
    [InlineData(Color.White, PieceType.Knight, 'N')]
    [InlineData(Color.White, PieceType.Bishop, 'B')]
    [InlineData(Color.White, PieceType.Rook, 'R')]
    [InlineData(Color.White, PieceType.Queen, 'Q')]
    [InlineData(Color.White, PieceType.King, 'K')]
    [InlineData(Color.Black, PieceType.Pawn, 'p')]
    [InlineData(Color.Black, PieceType.Knight, 'n')]
    [InlineData(Color.Black, PieceType.Bishop, 'b')]
    [InlineData(Color.Black, PieceType.Rook, 'r')]
    [InlineData(Color.Black, PieceType.Queen, 'q')]
    [InlineData(Color.Black, PieceType.King, 'k')]
    public void ToFenChar_ConvertsPieceToFenCharacter(Color color, PieceType type, char expectedFenChar)
    {
        char fenChar = FenPieceMapper.ToFenChar(new Piece(color, type));

        Assert.Equal(expectedFenChar, fenChar);
    }

    [Theory]
    [InlineData('x')]
    [InlineData('1')]
    [InlineData(' ')]
    public void ToPiece_WithInvalidFenCharacter_Throws(char fenChar)
    {
        Assert.Throws<ArgumentException>(() => FenPieceMapper.ToPiece(fenChar));
    }
}
