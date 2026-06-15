using ChessEngine.Core.Board;
using ChessEngine.Core.Enums;
using ChessEngine.Core.Positions;
using ChessEngine.Core.ValueObjects;

namespace ChessEngine.Core.Notation;

/// <summary>
/// Provides parsing logic for standard FEN notation.
/// </summary>
public static class FenParser
{
    /// <summary>
    /// Creates a chess position from a FEN string.
    /// </summary>
    public static Position Parse(string fen)
    {
        ArgumentNullException.ThrowIfNull(fen);

        string[] fields = fen.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (fields.Length != 6)
        {
            throw new ArgumentException("FEN must contain exactly 6 fields.", nameof(fen));
        }

        ChessBoard board = ParsePiecePlacement(fields[0], fen);
        Color sideToMove = ParseSideToMove(fields[1], fen);
        CastlingRights castlingRights = ParseCastlingRights(fields[2], fen);
        Square? enPassantTarget = ParseEnPassantTarget(fields[3], fen);
        int halfmoveClock = ParseHalfmoveClock(fields[4], fen);
        int fullmoveNumber = ParseFullmoveNumber(fields[5], fen);

        return new Position(
            board,
            sideToMove,
            castlingRights,
            enPassantTarget,
            halfmoveClock,
            fullmoveNumber);
    }

    private static ChessBoard ParsePiecePlacement(string piecePlacement, string fen)
    {
        string[] ranks = piecePlacement.Split('/');

        if (ranks.Length != 8)
        {
            throw new ArgumentException("FEN piece placement must contain 8 ranks.", nameof(fen));
        }

        ChessBoard board = ChessBoard.CreateEmpty();

        for (int fenRank = 0; fenRank < 8; fenRank++)
        {
            int rank = 7 - fenRank;
            ParseRank(ranks[fenRank], rank, board, fen);
        }

        return board;
    }

    private static void ParseRank(string fenRank, int rank, ChessBoard board, string fen)
    {
        int file = 0;
        bool previousSymbolWasDigit = false;

        foreach (char symbol in fenRank)
        {
            if (symbol is >= '1' and <= '8')
            {
                if (previousSymbolWasDigit)
                {
                    throw new ArgumentException("FEN rank cannot contain consecutive empty-square numbers.", nameof(fen));
                }

                file += symbol - '0';
                previousSymbolWasDigit = true;

                if (file > 8)
                {
                    throw new ArgumentException("FEN rank contains more than 8 squares.", nameof(fen));
                }

                continue;
            }

            previousSymbolWasDigit = false;

            if (file >= 8)
            {
                throw new ArgumentException("FEN rank contains more than 8 squares.", nameof(fen));
            }

            Piece piece = FenPieceMapper.ToPiece(symbol);
            board.SetPiece(Square.FromFileRank(file, rank), piece);
            file++;
        }

        if (file != 8)
        {
            throw new ArgumentException("FEN rank must contain exactly 8 squares.", nameof(fen));
        }
    }

    private static Color ParseSideToMove(string sideToMove, string fen)
    {
        return sideToMove switch
        {
            "w" => Color.White,
            "b" => Color.Black,
            _ => throw new ArgumentException("FEN side to move must be w or b.", nameof(fen))
        };
    }

    private static CastlingRights ParseCastlingRights(string castlingField, string fen)
    {
        if (castlingField == "-")
        {
            return CastlingRights.None;
        }

        if (castlingField.Contains('-'))
        {
            throw new ArgumentException("FEN castling rights cannot combine '-' with castling flags.", nameof(fen));
        }

        CastlingRights rights = CastlingRights.None;

        foreach (char symbol in castlingField)
        {
            CastlingRights right = symbol switch
            {
                'K' => CastlingRights.WhiteKingside,
                'Q' => CastlingRights.WhiteQueenside,
                'k' => CastlingRights.BlackKingside,
                'q' => CastlingRights.BlackQueenside,
                _ => throw new ArgumentException("FEN castling rights contain an invalid flag.", nameof(fen))
            };

            if (rights.HasFlag(right))
            {
                throw new ArgumentException("FEN castling rights contain a duplicate flag.", nameof(fen));
            }

            rights |= right;
        }

        return rights;
    }

    private static Square? ParseEnPassantTarget(string enPassantField, string fen)
    {
        if (enPassantField == "-")
        {
            return null;
        }

        try
        {
            Square target = Square.FromName(enPassantField);

            if (target.Rank is not 2 and not 5)
            {
                throw new ArgumentException("FEN en passant target must be on rank 3 or rank 6.", nameof(fen));
            }

            return target;
        }
        catch (ArgumentException exception)
        {
            throw new ArgumentException("FEN en passant target must be '-' or a valid square name.", nameof(fen), exception);
        }
    }

    private static int ParseHalfmoveClock(string halfmoveField, string fen)
    {
        if (!int.TryParse(halfmoveField, out int halfmoveClock) || halfmoveClock < 0)
        {
            throw new ArgumentException("FEN halfmove clock must be a non-negative number.", nameof(fen));
        }

        return halfmoveClock;
    }

    private static int ParseFullmoveNumber(string fullmoveField, string fen)
    {
        if (!int.TryParse(fullmoveField, out int fullmoveNumber) || fullmoveNumber < 1)
        {
            throw new ArgumentException("FEN fullmove number must be at least 1.", nameof(fen));
        }

        return fullmoveNumber;
    }
}
