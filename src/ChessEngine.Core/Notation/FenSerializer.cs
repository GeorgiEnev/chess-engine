using System.Text;
using ChessEngine.Core.Enums;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Notation;

/// <summary>
/// Provides serialization logic for standard FEN notation.
/// </summary>
public static class FenSerializer
{
    /// <summary>
    /// Creates the standard FEN representation of the given chess position.
    /// </summary>
    public static string Serialize(Position position)
    {
        return string.Join(
            ' ',
            SerializePiecePlacement(position),
            SerializeSideToMove(position),
            SerializeCastlingRights(position),
            SerializeEnPassantTarget(position),
            position.HalfmoveClock,
            position.FullmoveNumber);
    }

    private static string SerializePiecePlacement(Position position)
    {
        StringBuilder builder = new();

        for (int rank = 7; rank >= 0; rank--)
        {
            if (rank < 7)
            {
                builder.Append('/');
            }

            AppendRank(position, rank, builder);
        }

        return builder.ToString();
    }

    // Writes one rank, replacing consecutive empty squares with a number.
    private static void AppendRank(Position position, int rank, StringBuilder builder)
    {
        int emptySquares = 0;

        for (int file = 0; file < 8; file++)
        {
            Piece? piece = position.Board.GetPiece(Square.FromFileRank(file, rank));

            if (piece is null)
            {
                emptySquares++;
                continue;
            }

            if (emptySquares > 0)
            {
                builder.Append(emptySquares);
                emptySquares = 0;
            }

            builder.Append(FenPieceMapper.ToFenChar(piece.Value));
        }

        if (emptySquares > 0)
        {
            builder.Append(emptySquares);
        }
    }

    private static string SerializeSideToMove(Position position)
    {
        return position.SideToMove == Color.White ? "w" : "b";
    }

    private static string SerializeCastlingRights(Position position)
    {
        if (position.CastlingRights == CastlingRights.None)
        {
            return "-";
        }

        StringBuilder builder = new();

        if (position.CastlingRights.HasFlag(CastlingRights.WhiteKingside))
        {
            builder.Append('K');
        }

        if (position.CastlingRights.HasFlag(CastlingRights.WhiteQueenside))
        {
            builder.Append('Q');
        }

        if (position.CastlingRights.HasFlag(CastlingRights.BlackKingside))
        {
            builder.Append('k');
        }

        if (position.CastlingRights.HasFlag(CastlingRights.BlackQueenside))
        {
            builder.Append('q');
        }

        return builder.Length == 0 ? "-" : builder.ToString();
    }

    private static string SerializeEnPassantTarget(Position position)
    {
        return position.EnPassantTarget?.ToString() ?? "-";
    }
}
