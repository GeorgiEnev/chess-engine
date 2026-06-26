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
        Piece pieceToPlace = move.Promotion is null
            ? movingPiece
            : new Piece(movingPiece.Color, move.Promotion.Value);

        if (IsEnPassantMove(position, move, movingPiece))
        {
            Square capturedPawnSquare = GetEnPassantCapturedPawnSquare(move, movingPiece.Color);
            board.RemovePiece(capturedPawnSquare);
        }

        board.RemovePiece(move.From);
        board.SetPiece(move.To, pieceToPlace);

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

    // Returns true when the move is an en passant capture in the current position.
    private static bool IsEnPassantMove(Position position, Move move, Piece movingPiece)
    {
        return movingPiece.Type == PieceType.Pawn
            && position.EnPassantTarget == move.To
            && move.From.File != move.To.File
            && position.Board.IsEmpty(move.To);
    }

    private static Square GetEnPassantCapturedPawnSquare(Move move, Color movingColor)
    {
        int capturedPawnRank = movingColor == Color.White
            ? move.To.Rank - 1
            : move.To.Rank + 1;

        return Square.FromFileRank(move.To.File, capturedPawnRank);
    }
}
