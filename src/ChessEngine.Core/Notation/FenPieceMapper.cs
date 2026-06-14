using ChessEngine.Core.Enums;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Notation;

/// <summary>
/// Provides piece conversion logic for FEN notation.
/// </summary>
/// <remarks>
/// FEN uses uppercase letters for White pieces, such as P or K, and lowercase
/// letters for Black pieces, such as p or k.
/// </remarks>
public static class FenPieceMapper
{
    /// <summary>
    /// Converts a FEN piece character to a chess piece.
    /// </summary>
    public static Piece ToPiece(char fenChar)
    {
        return fenChar switch
        {
            'P' => new Piece(Color.White, PieceType.Pawn),
            'N' => new Piece(Color.White, PieceType.Knight),
            'B' => new Piece(Color.White, PieceType.Bishop),
            'R' => new Piece(Color.White, PieceType.Rook),
            'Q' => new Piece(Color.White, PieceType.Queen),
            'K' => new Piece(Color.White, PieceType.King),
            'p' => new Piece(Color.Black, PieceType.Pawn),
            'n' => new Piece(Color.Black, PieceType.Knight),
            'b' => new Piece(Color.Black, PieceType.Bishop),
            'r' => new Piece(Color.Black, PieceType.Rook),
            'q' => new Piece(Color.Black, PieceType.Queen),
            'k' => new Piece(Color.Black, PieceType.King),
            _ => throw new ArgumentException($"'{fenChar}' is not a valid FEN piece character.", nameof(fenChar))
        };
    }

    /// <summary>
    /// Converts a chess piece to its FEN piece character.
    /// </summary>
    public static char ToFenChar(Piece piece)
    {
        return piece switch
        {
            { Color: Color.White, Type: PieceType.Pawn } => 'P',
            { Color: Color.White, Type: PieceType.Knight } => 'N',
            { Color: Color.White, Type: PieceType.Bishop } => 'B',
            { Color: Color.White, Type: PieceType.Rook } => 'R',
            { Color: Color.White, Type: PieceType.Queen } => 'Q',
            { Color: Color.White, Type: PieceType.King } => 'K',
            { Color: Color.Black, Type: PieceType.Pawn } => 'p',
            { Color: Color.Black, Type: PieceType.Knight } => 'n',
            { Color: Color.Black, Type: PieceType.Bishop } => 'b',
            { Color: Color.Black, Type: PieceType.Rook } => 'r',
            { Color: Color.Black, Type: PieceType.Queen } => 'q',
            { Color: Color.Black, Type: PieceType.King } => 'k',
            _ => throw new ArgumentException("The piece cannot be converted to a FEN character.", nameof(piece))
        };
    }
}
