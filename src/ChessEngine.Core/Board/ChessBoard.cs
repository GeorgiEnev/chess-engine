using ChessEngine.Core.Enums;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Board;

/// <summary>
/// Represents a chess board as 64 squares that can each hold one piece or be empty.
/// </summary>
/// <remarks>
/// This class is responsible only for piece placement. It does not decide whose
/// turn it is, whether a move is legal, or any other game-state rule.
/// </remarks>
public class ChessBoard
{
    private readonly Piece?[] squares;

    private ChessBoard()
    {
        squares = new Piece?[64];
    }

    /// <summary>
    /// Creates a separate board with the same piece placement.
    /// </summary>
    public ChessBoard Copy()
    {
        ChessBoard copy = CreateEmpty();

        for (int index = 0; index < 64; index++)
        {
            Square square = new(index);
            Piece? piece = GetPiece(square);

            if (piece is not null)
            {
                copy.SetPiece(square, piece.Value);
            }
        }

        return copy;
    }

    /// <summary>
    /// Creates an empty board.
    /// </summary>
    public static ChessBoard CreateEmpty()
    {
        return new ChessBoard();
    }

    /// <summary>
    /// Creates a board with the standard chess starting piece placement.
    /// </summary>
    public static ChessBoard CreateStartingBoard()
    {
        ChessBoard board = CreateEmpty();

        board.SetBackRank(Color.White, rank: 0);
        board.SetPawnRank(Color.White, rank: 1);

        board.SetBackRank(Color.Black, rank: 7);
        board.SetPawnRank(Color.Black, rank: 6);

        return board;
    }

    /// <summary>
    /// Gets the piece on a square, or null when the square is empty.
    /// </summary>
    public Piece? GetPiece(Square square)
    {
        return squares[square.Index];
    }

    /// <summary>
    /// Places a piece on a square, replacing any piece already there.
    /// </summary>
    public void SetPiece(Square square, Piece piece)
    {
        squares[square.Index] = piece;
    }

    /// <summary>
    /// Clears a square by removing any piece placed there.
    /// </summary>
    public void RemovePiece(Square square)
    {
        squares[square.Index] = null;
    }

    /// <summary>
    /// Returns true when a square has no piece.
    /// </summary>
    public bool IsEmpty(Square square)
    {
        return GetPiece(square) is null;
    }

    // Places the non-pawn pieces in their standard order on the first or eighth rank.
    private void SetBackRank(Color color, int rank)
    {
        SetPiece(Square.FromFileRank(0, rank), new Piece(color, PieceType.Rook));
        SetPiece(Square.FromFileRank(1, rank), new Piece(color, PieceType.Knight));
        SetPiece(Square.FromFileRank(2, rank), new Piece(color, PieceType.Bishop));
        SetPiece(Square.FromFileRank(3, rank), new Piece(color, PieceType.Queen));
        SetPiece(Square.FromFileRank(4, rank), new Piece(color, PieceType.King));
        SetPiece(Square.FromFileRank(5, rank), new Piece(color, PieceType.Bishop));
        SetPiece(Square.FromFileRank(6, rank), new Piece(color, PieceType.Knight));
        SetPiece(Square.FromFileRank(7, rank), new Piece(color, PieceType.Rook));
    }

    // Places one pawn on each file of the given pawn rank.
    private void SetPawnRank(Color color, int rank)
    {
        for (int file = 0; file < 8; file++)
        {
            SetPiece(Square.FromFileRank(file, rank), new Piece(color, PieceType.Pawn));
        }
    }
}
