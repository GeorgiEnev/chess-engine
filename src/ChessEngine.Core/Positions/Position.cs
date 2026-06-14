using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Positions;

/// <summary>
/// Represents the complete state of a chess position.
/// </summary>
/// <remarks>
/// A position includes the board plus the state needed to interpret the rules,
/// such as side to move, castling rights, en passant target, and move clocks.
/// </remarks>
public sealed class Position
{
    /// <summary>
    /// Gets the board piece placement for this position.
    /// </summary>
    public ChessBoard Board { get; }

    /// <summary>
    /// Gets the side whose turn it is to move.
    /// </summary>
    public Color SideToMove { get; }

    /// <summary>
    /// Gets the castling options still available in this position.
    /// </summary>
    public CastlingRights CastlingRights { get; }

    /// <summary>
    /// Gets the en passant target square, or null when no en passant capture is available.
    /// </summary>
    public Square? EnPassantTarget { get; }

    /// <summary>
    /// Tracks the number of halfmoves since the last pawn move or capture.
    /// </summary>
    public int HalfmoveClock { get; }

    /// <summary>
    /// Tracks the chess move number, starting at 1 and increasing after each Black move.
    /// </summary>
    public int FullmoveNumber { get; }

    public Position(
        ChessBoard board,
        Color sideToMove,
        CastlingRights castlingRights,
        Square? enPassantTarget,
        int halfmoveClock,
        int fullmoveNumber)
    {
        if (halfmoveClock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(halfmoveClock), "Halfmove clock cannot be negative.");
        }

        if (fullmoveNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(fullmoveNumber), "Fullmove number must be at least 1.");
        }

        Board = board;
        SideToMove = sideToMove;
        CastlingRights = castlingRights;
        EnPassantTarget = enPassantTarget;
        HalfmoveClock = halfmoveClock;
        FullmoveNumber = fullmoveNumber;
    }

    /// <summary>
    /// Creates the standard initial chess position.
    /// </summary>
    public static Position CreateStartingPosition()
    {
        return new Position(
            ChessBoard.CreateStartingBoard(),
            Color.White,
            CastlingRights.All,
            enPassantTarget: null,
            halfmoveClock: 0,
            fullmoveNumber: 1);
    }
}
