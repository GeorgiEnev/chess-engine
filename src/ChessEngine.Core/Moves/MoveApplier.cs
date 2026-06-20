using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.Extensions;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Moves;

/// <summary>
/// Applies moves to chess positions.
/// </summary>
public static class MoveApplier
{
    /// <summary>
    /// Applies a move and returns the resulting position without modifying the original position.
    /// </summary>
    /// <param name="position">The position before the move is applied.</param>
    /// <param name="move">The move to apply.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the move source square does not contain a piece.
    /// </exception>
    public static Position Apply(Position position, Move move)
    {
        ArgumentNullException.ThrowIfNull(position);

        Piece movingPiece = position.Board.GetPiece(move.From)
            ?? throw new InvalidOperationException("Move source square does not contain a piece.");

        ChessBoard board = position.Board.Copy();

        board.RemovePiece(move.From);
        board.SetPiece(move.To, movingPiece);

        Color nextSideToMove = position.SideToMove.Opposite();
        int nextFullmoveNumber = position.SideToMove == Color.Black
            ? position.FullmoveNumber + 1
            : position.FullmoveNumber;

        return new Position(
            board,
            nextSideToMove,
            position.CastlingRights,
            enPassantTarget: null,
            position.HalfmoveClock,
            nextFullmoveNumber);
    }
}
