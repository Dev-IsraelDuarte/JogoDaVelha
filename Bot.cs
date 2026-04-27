using System;
using System.Linq;
using System.Collections.Generic;

public class Bot
{
    Random rnd = new Random();

    public int GetMove(string[] board, Difficulty difficulty, string bot, string player)
    {
        return difficulty switch
        {
            Difficulty.Easy => RandomMove(board),
            Difficulty.Medium => rnd.Next(2) == 0 ? RandomMove(board) : BestMove(board, bot, player),
            Difficulty.Hard => BestMove(board, bot, player),
            Difficulty.Impossible => MinimaxMove(board, bot, player),
            _ => RandomMove(board)
        };
    }

    private int RandomMove(string[] board)
    {
        var empty = board
            .Select((v, i) => new { v, i })
            .Where(x => x.v == "")
            .Select(x => x.i)
            .ToList();

        return empty[rnd.Next(empty.Count)];
    }

    private int BestMove(string[] board, string bot, string player)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[i] == "")
            {
                board[i] = bot;
                if (CheckWinner(board) == bot)
                {
                    board[i] = "";
                    return i;
                }
                board[i] = "";
            }
        }

        for (int i = 0; i < 9; i++)
        {
            if (board[i] == "")
            {
                board[i] = player;
                if (CheckWinner(board) == player)
                {
                    board[i] = "";
                    return i;
                }
                board[i] = "";
            }
        }

        return RandomMove(board);
    }

    private int MinimaxMove(string[] board, string bot, string player)
    {
        int bestScore = int.MinValue;
        int move = -1;

        for (int i = 0; i < 9; i++)
        {
            if (board[i] == "")
            {
                board[i] = bot;
                int score = Minimax(board, 0, false, bot, player);
                board[i] = "";

                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }

        return move;
    }

    private int Minimax(string[] board, int depth, bool isMax, string bot, string player)
    {
        string result = CheckWinner(board);

        if (result == bot) return 10 - depth;
        if (result == player) return depth - 10;
        if (board.All(x => x != "")) return 0;

        if (isMax)
        {
            int best = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == "")
                {
                    board[i] = bot;
                    best = Math.Max(best, Minimax(board, depth + 1, false, bot, player));
                    board[i] = "";
                }
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == "")
                {
                    board[i] = player;
                    best = Math.Min(best, Minimax(board, depth + 1, true, bot, player));
                    board[i] = "";
                }
            }
            return best;
        }
    }

    private string CheckWinner(string[] b)
    {
        int[,] w = {
            {0,1,2},{3,4,5},{6,7,8},
            {0,3,6},{1,4,7},{2,5,8},
            {0,4,8},{2,4,6}
        };

        for (int i = 0; i < 8; i++)
        {
            if (b[w[i, 0]] != "" &&
                b[w[i, 0]] == b[w[i, 1]] &&
                b[w[i, 1]] == b[w[i, 2]])
                return b[w[i, 0]];
        }

        return null;
    }
}