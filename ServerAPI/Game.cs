namespace ServerAP;

public class Game
{
    private readonly int[,] _field = new int[3, 3]
    {
        // 0  1  2
        {7, 8, 9}, //0
        {4, 5, 6}, //1
        {1, 2, 3} //2
    };
    
    
    public static List<Player> Players = new();
    public TurnInfo TurnInfo { get; set; } = null!;
    public GameState GameState { get; set; }
    public GameSigns Winner { get; set; }
    public GameSigns[,] GameField = new GameSigns[3, 3];
    public GameSigns CurrentSign = GameSigns.X;
    public bool IsGameEnd { get; set; }
    public bool CanPlayerMakeTurn { get; set; }
    
    
    public void MakeTurn()
    {
        
        GameField[TurnInfo.X, TurnInfo.Y] = CurrentSign;
        if (FindWinCombination(TurnInfo.X, TurnInfo.Y))
        {
            Winner = CurrentSign;
            IsGameEnd = true;
            return;
        }
        CheckIsFieldFull();
    }

    public void ChangeGameSign()
    {
        CurrentSign = TurnInfo.PlayerSign == GameSigns.X ? GameSigns.O : GameSigns.X;
    }

    public (bool isTurnValid,string errorMessage) ValidateTurn()
    {
        var errorMessage = "";
        var isTurnValid = true;
        
        if (TurnInfo.PlayerSign != CurrentSign)
        {
            errorMessage = "Now it's the other player's turn";
            isTurnValid = false;
            return (isTurnValid,errorMessage);
        }

        if (GameField[TurnInfo.X, TurnInfo.Y] != GameSigns.Empty)
        {
            errorMessage = "Cell is full";
            isTurnValid = false;
        }

        return (isTurnValid, errorMessage);
    }
    
    
    
    
    private bool FindWinCombination(int x, int y)
    {
        if (FindVerticalWinCombination(x, y) >= 2)
            return true;
        if (FindHorizontalWinCombination(x, y) >= 2)
            return true;
        return FindDiagonalWinCombination(x, y) >= 2;
    }
    

    public GameState GameStateCollect()
    {
        GameState = new GameState()
        {
            GameField = GameField.Cast<GameSigns>().ToArray(),
            Winner = Winner,
            IsGameEnd = IsGameEnd,
            CanPlayerMakeTurn = CanPlayerMakeTurn
            
        };
        //?
        return GameState;
    }
    private int FindVerticalWinCombination(int x, int y)
    {
        var sign = GameField[x, y];
        int winCombination = 0;

        //Optimize method
        switch (y)
        {
            case 0:
            {
                while (y < 2)
                {
                    if (sign == GameField[x, y + 1])
                    {
                        winCombination++;
                        y++;
                    }
                    else
                    {
                        y++;
                    }
                }

                return winCombination;
            }
            case 1:
            {
                if (sign == GameField[x, y + 1])
                {
                    winCombination++;
                }

                if (sign == GameField[x, y - 1])
                {
                    winCombination++;
                }

                return winCombination;
            }
            case 2:
            {
                while (y > 0)
                {
                    if (sign == GameField[x, y - 1])
                    {
                        winCombination++;
                        y--;
                    }
                    else
                    {
                        y--;
                    }
                }

                return winCombination;
            }
            default:
                return winCombination;
        }
    }

    private int FindHorizontalWinCombination(int x, int y)
    {
        var sign = GameField[x, y];
        int winCombination = 0;

        switch (x)
        {
            case 0:
            {
                while (x < 2)
                {
                    if (sign == GameField[x + 1, y])
                    {
                        winCombination++;
                        x++;
                    }
                    else
                    {
                        x++;
                    }
                }

                return winCombination;
            }
            case 1:
            {
                if (sign == GameField[x + 1, y])
                {
                    winCombination++;
                }

                if (sign == GameField[x + 1, y])
                {
                    winCombination++;
                }

                return winCombination;
            }
            case 2:
            {
                while (x > 0)
                {
                    if (sign == GameField[x - 1, y])
                    {
                        winCombination++;
                        x--;
                    }
                    else
                    {
                        x--;
                    }
                }

                return winCombination;
            }
            default:
                return winCombination;
        }
    }

    private int FindDiagonalWinCombination(int x, int y)
    {
        var sign = GameField[x, y];

        if (sign == GameField[2, 0] && sign == GameField[0, 2] && sign == GameField[1, 1])
            return 2;
        if (sign == GameField[0, 0] && sign == GameField[2, 2] && sign == GameField[1, 1])
            return 2;
        return 0;
    }

    private void CheckIsFieldFull()
    {
        var emptySpaceCounter = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (GameField[i, j] == GameSigns.Empty)
                    emptySpaceCounter++;
            }
        }

        if (emptySpaceCounter != 0)
        {
            return;
        }

        IsGameEnd = true;
        Winner = GameSigns.Empty;
    }
}