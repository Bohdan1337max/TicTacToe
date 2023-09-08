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
    
    
    public static readonly List<Player> Players = new();
    public GameState GameState { get; set; } = null!;
    private GameSigns Winner { get; set; }
    private readonly GameSigns[,] _gameField = new GameSigns[3, 3];
    public GameSigns CurrentSign = GameSigns.X;
    private bool IsGameEnd { get; set; }
    public bool CanPlayerMakeTurn { get; set; }
    
    
    public void MakeTurn(int x,int y)
    {
        
        _gameField[x, y] = CurrentSign;
        if (FindWinCombination(x, y))
        {
            Winner = CurrentSign;
            IsGameEnd = true;
            return;
        }
        CheckIsFieldFull();
    }

    public void ChangeGameSign(GameSigns playerSign)
    {
        CurrentSign = playerSign == GameSigns.X ? GameSigns.O : GameSigns.X;
    }

    public (bool isTurnValid,string errorMessage) ValidateTurn(int x,int y, GameSigns playerTurnSign)
    {
        var errorMessage = "";
        var isTurnValid = true;
        
        if (playerTurnSign != CurrentSign)
        {
            errorMessage = "Now it's the other player's turn";
            isTurnValid = false;
            return (isTurnValid,errorMessage);
        }

        if (_gameField[x, y] != GameSigns.Empty)
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
        return new GameState
        {
            GameField = _gameField.Cast<GameSigns>().ToArray(),
            Winner = Winner,
            IsGameEnd = IsGameEnd,
            CanPlayerMakeTurn = CanPlayerMakeTurn
            
        };
    }
    private int FindVerticalWinCombination(int x, int y)
    {
        var sign = _gameField[x, y];
        int winCombination = 0;

        //Optimize method
        switch (y)
        {
            case 0:
            {
                while (y < 2)
                {
                    if (sign == _gameField[x, y + 1])
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
                if (sign == _gameField[x, y + 1])
                {
                    winCombination++;
                }

                if (sign == _gameField[x, y - 1])
                {
                    winCombination++;
                }

                return winCombination;
            }
            case 2:
            {
                while (y > 0)
                {
                    if (sign == _gameField[x, y - 1])
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
        var sign = _gameField[x, y];
        int winCombination = 0;

        switch (x)
        {
            case 0:
            {
                while (x < 2)
                {
                    if (sign == _gameField[x + 1, y])
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
                if (sign == _gameField[x + 1, y])
                {
                    winCombination++;
                }

                if (sign == _gameField[x + 1, y])
                {
                    winCombination++;
                }

                return winCombination;
            }
            case 2:
            {
                while (x > 0)
                {
                    if (sign == _gameField[x - 1, y])
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
        var sign = _gameField[x, y];

        if (sign == _gameField[2, 0] && sign == _gameField[0, 2] && sign == _gameField[1, 1])
            return 2;
        if (sign == _gameField[0, 0] && sign == _gameField[2, 2] && sign == _gameField[1, 1])
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
                if (_gameField[i, j] == GameSigns.Empty)
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