using System;

namespace TicTacToe;

public class Game
{
    private readonly int[,] _field = new int[3,3] { 
        {7,8,9},    //0
        {4,5,6},    //1
        {1,2,3} };  //2
    //   0 1 2
    private int[,] WinCombination = new int[,]
           {{7, 8, 9}, 
            {4, 5, 6},
            {1, 2, 3},
            {7, 8, 9},
            {8, 5, 2},
            {9, 6, 3},
            {7, 5, 3},
            {9, 5, 1}};

    public readonly int[,] GameField = new int[3, 3];
    private int _turnCount = 1;
    public bool IsGameEnd;

    public void DefineSign(int playerTurn)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_field[i, j] == playerTurn)
                {
                    if (_turnCount % 2 != 0)
                    {
                        GameField[i, j] = 1;
                        continue;
                    }
                    GameField[i, j] = 2;
                }
            }
        }
    }

    public void FindWinCombination(int playerTurn)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (playerTurn == _field[i, j])
                {
                    if (FindVerticalWinCombination(i, j) >= 2)
                        IsGameEnd = true;
                    if (FindHorizontalWinCombination(i, j) >= 2)
                        IsGameEnd = true;
                    if (FindDiagonalWinCombination(i, j) >= 2)
                        IsGameEnd = true;
                }
                    
            }
        }
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
                    if (sign == GameField[i , j+ 1])
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
                if (sign == GameField[i , j + 1])
                {
                    winCombination++;
                }

                if (sign == GameField[i , j - 1])
                {
                    winCombination++;
                }
                return winCombination;
            }
            case 2:
            {
                while (j > 0)
                {
                    if (sign == GameField[i , j - 1])
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
        
            if (sign == GameField[2, 0] && sign == GameField[0, 2])
                return 2;
            if (sign == GameField[0, 0] && sign == GameField[2, 2])
                return 2;

            return 0;
    }
    
    public void TurnIncrease()
    {
        _turnCount++;
    }

}