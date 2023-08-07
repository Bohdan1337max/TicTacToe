using System;
using System.Linq;

namespace TicTacToe;

public class Game
{
    private readonly int[,] _field = new int[3, 3]
    { // 0  1  2
        {7, 8, 9}, //0
        {4, 5, 6}, //1
        {1, 2, 3}  //2
    };
   
    public GameSigns Winner; 
    public readonly GameSigns[,] GameField = new GameSigns[3, 3];
    public GameSigns currentSign = GameSigns.X;
    public bool IsGameEnd;

    public void MakeTurn(int numpadTurnInput)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_field[i, j] == numpadTurnInput)
                {
                    MakeTurn(i,j);
                }
            }
        }
    }

    public void MakeTurn(int x, int y)
    {

        if (GameField[x, y] != GameSigns.Empty)
            return;
        GameField[x, y] = currentSign;
        if (FindWinCombination(x, y))
        {
            Winner = currentSign;
            IsGameEnd = true;
            return;
        }
        
        ChangeGameSign();
        CheckIsFieldComplete();
    }

    private void ChangeGameSign()
    {
        currentSign = currentSign switch
        {
            GameSigns.X => GameSigns.O,
            GameSigns.O => GameSigns.X,
            _ => currentSign
        };
    }


    private bool FindWinCombination(int numpadTurnInput)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (numpadTurnInput == _field[i, j])
                {
                    if (FindVerticalWinCombination(i, j) >= 2)
                        return true;
                    if (FindHorizontalWinCombination(i, j) >= 2)
                        return true;
                    if (FindDiagonalWinCombination(i, j) >= 2)
                        return true;
                }
            }
        }

        return false;
    }

    public GameState GameStateCollector()
    {
        GameSigns[] gameField2D = GameField.Cast<GameSigns>().ToArray();
        return new GameState()
        {
            GameField = gameField2D,
        };

    }
    private bool FindWinCombination(int x, int y)
    {
        if (FindVerticalWinCombination(x, y) >= 2)
            return true;
        if (FindHorizontalWinCombination(x, y) >= 2)
            return true;
        return FindDiagonalWinCombination(x, y) >= 2;
    }

    private int FindVerticalWinCombination(int x, int y)
    {
        var sign = GameField[x, y];
        int winCombination = 0;

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
        var sign = GameField[x,y];
        int winCombination = 0;

        switch (x)
        {
            case 0:
            {
                while (x < 2)
                {
                    if (sign == GameField[ x + 1,y])
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
                if (sign == GameField[ x + 1,y])
                {
                    winCombination++;
                }

                if (sign == GameField[ x + 1, y])
                {
                    winCombination++;
                }

                return winCombination;
            }
            case 2:
            {
                while (x > 0)
                {
                    if (sign == GameField[ x - 1,y])
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

    public void ShowEndGameNotification(GameSigns winner)
    {
        Console.WriteLine(
            winner == GameSigns.Empty ? "DRAW!! Game field is full" : $"Congratulations {winner} WIN!!!!!");
    }

    private void CheckIsFieldComplete()
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
    public void ShowWelcomeNotification()
    {
        Console.WriteLine("Welcome! Red# is your pointer.You can use arrows for navigate on the field!");
    }
}