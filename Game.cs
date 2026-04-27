using System.Linq;

public class Game
{
    public string[] Board = new string[9];
    public string CurrentPlayer = "X";

    public void Reset()
    {
        for (int i = 0; i < 9; i++)
            Board[i] = "";
    }

    public bool MakeMove(int index)
    {
        if (Board[index] == "")
        {
            Board[index] = CurrentPlayer;
            CurrentPlayer = CurrentPlayer == "X" ? "O" : "X";
            return true;
        }
        return false;
    }

    public string CheckWinner()
    {
        int[,] w = {
            {0,1,2},{3,4,5},{6,7,8},
            {0,3,6},{1,4,7},{2,5,8},
            {0,4,8},{2,4,6}
        };

        for (int i = 0; i < 8; i++)
        {
            if (Board[w[i, 0]] != "" &&
                Board[w[i, 0]] == Board[w[i, 1]] &&
                Board[w[i, 1]] == Board[w[i, 2]])
                return Board[w[i, 0]];
        }

        return Board.All(x => x != "") ? "Empate" : null;
    }
}