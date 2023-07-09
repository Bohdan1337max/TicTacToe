using System;

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
    private GameSigns _currentSign = GameSigns.X;
    public bool IsGameEnd;

    public void MakeTurn(int numpadTurnInput)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_field[i, j] == numpadTurnInput)
                {
                    if (GameField[i, j] != GameSigns.Empty)
                        return;
                    
                    GameField[i, j] = _currentSign;
                    Console.Clear();
                    if (FindWinCombination(numpadTurnInput))
                    {
                        Winner = _currentSign;
                        IsGameEnd = true;
                        return;
                    }

                    ChangeGameSign();
                    CheckIsFieldComplete();
                }
            }
        }
    }

    private void ChangeGameSign()
    {
        _currentSign = _currentSign switch
        {
            GameSigns.X => GameSigns.O,
            GameSigns.O => GameSigns.X,
            _ => _currentSign
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

    private int FindVerticalWinCombination(int i, int j)
    {
        var sign = GameField[i, j];
        int winCombination = 0;

        switch (i)
        {
            case 0:
            {
                while (i < 2)
                {
                    if (sign == GameField[i + 1, j])
                    {
                        winCombination++;
                        i++;
                    }
                    else
                    {
                        i++;
                    }
                }

                return winCombination;
            }
            case 1:
            {
                if (sign == GameField[i + 1, j])
                {
                    winCombination++;
                }

                if (sign == GameField[i - 1, j])
                {
                    winCombination++;
                }

                return winCombination;
            }
            case 2:
            {
                while (i > 0)
                {
                    if (sign == GameField[i - 1, j])
                    {
                        winCombination++;
                        i--;
                    }
                    else
                    {
                        i--;
                    }
                }

                return winCombination;
            }
            default:
                return winCombination;
        }
    }

    private int FindHorizontalWinCombination(int i, int j)
    {
        var sign = GameField[i, j];
        int winCombination = 0;

        switch (j)
        {
            case 0:
            {
                while (j < 2)
                {
                    if (sign == GameField[i, j + 1])
                    {
                        winCombination++;
                        j++;
                    }
                    else
                    {
                        j++;
                    }
                }

                return winCombination;
            }
            case 1:
            {
                if (sign == GameField[i, j + 1])
                {
                    winCombination++;
                }

                if (sign == GameField[i, j - 1])
                {
                    winCombination++;
                }

                return winCombination;
            }
            case 2:
            {
                while (j > 0)
                {
                    if (sign == GameField[i, j - 1])
                    {
                        winCombination++;
                        j--;
                    }
                    else
                    {
                        j--;
                    }
                }

                return winCombination;
            }
            default:
                return winCombination;
        }
    }

    private int FindDiagonalWinCombination(int i, int j)
    {
        var sign = GameField[i, j];

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
        Console.WriteLine("Choose your number on numpad it's will be your turn");
    }
}