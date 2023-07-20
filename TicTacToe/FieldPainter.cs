﻿using System;
 
 namespace TicTacToe;
 public enum GameSigns
 {
     Empty,
     O,
     X,
         
 }

 public enum FieldDye
 {
     EmptySpace,
     Sign
     
 }
 
 public class FieldPainter
 {
     private readonly FileHandler _fileHandler = new FileHandler();
     private Pixel[,] _field;
     private static int FieldWidth { get; set; }
     private static int FieldHeight { get; set; }
     private int _cellHeight;
     private int _cellWidth;
     private readonly int[,] _x = new int[,]
     {
         {0, 0, 0, 0, 0, 0, 0, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 0, 1, 0, 1, 0, 0, 0},
         {0, 0, 0, 0, 1, 0, 0, 0, 0},
         {0, 0, 0, 1, 0, 1, 0, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 0, 0, 0, 0, 0, 0, 0}
     };

     private readonly int[,] _o = new int[,]
     {
         {0, 0, 0, 0, 0, 0, 0, 0, 0},
         {0, 0, 0, 1, 1, 1, 0, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 0, 1, 1, 1, 0, 0, 0},
         {0, 0, 0, 0, 0, 0, 0, 0, 0}
     };

     /*
     public static void PaintField (GameSigns[,] gameField)
     {
         for (int i = 0; i <= 2; i++)
         {
             for (int j = 0; j <= 2; j++)
             {
                 if(gameField[i,j] == GameSigns.X)
                     Console.Write('X');
                 if (gameField[i,j] == GameSigns.O)
                     Console.Write('O');
                 if(gameField[i,j] == GameSigns.Empty)
                     Console.Write(' ');
             }
             Console.Write('|');
             Console.WriteLine();
             if(i == 2)
                 Console.WriteLine("---");
         }
     }
     */

     public void ReadFile()
     {
         _fileHandler.FindCellSize();
         _cellHeight = _fileHandler.CellHeight + 2; // 2 = emptySpace in cell on the top
         _cellWidth = _fileHandler.CellWidth + 4; // 4 = emptySpace in cell on the sides
         FieldWidth = _cellWidth * 3 + 4;
         FieldHeight = _cellHeight * 3 + 4;
         _field = new Pixel[FieldWidth, FieldHeight];

     }
     
     //Clear whole console
     public void PaintGameField(GameSigns[,] gameField,int x,int y)
     {
         Console.Clear();
         BuildBufferField(gameField,x,y);
         for (int i = 0; i < FieldHeight; i++)
         {
             for (int j = 0; j < FieldWidth; j++)
             {
                 Console.BackgroundColor = _field[j, i]._color;
                 Console.Write(_field[j, i]._char);

                 if(j == FieldWidth - 1)
                     Console.WriteLine();
                 
             }
         }
         
     }
     /*
     /0  1  2
     {7, 8, 9}, //0
     {4, 5, 6}, //1
     {1, 2, 3}  //2
     */
     
     public void BuildBufferField(GameSigns[,] gameField, int x,int y)
     {
         ReadFile();
         Array.Clear(_field);   
         IndicateHorizontalLines();
         IndicateVerticalLines();
         HandlePointer(x,y);
         AddSignOnField(gameField);
     }

    
     private void HandlePointer(int x,int y)
     {
         var pointer = new Pixel
         {
             _char = '*',
             _color = ConsoleColor.DarkRed
         };
         
         int xPointerIndex = FindIndex(x,y).X;
         int yPointerIndex = FindIndex(x, y).Y;
         _field[xPointerIndex, yPointerIndex] = pointer;


     }
     private (int X, int Y) FindIndex(int x,int y)
     {
         return (x + 1 + _cellWidth * x,y + 1 + _cellHeight * y);
     }

     public void AddSignOnField(GameSigns[,] gameField)

     {
         for (int i = 0; i < 3; i++)
         {
             for (int j = 0; j < 3; j++)
             {
                 int x = FindIndex(j, i).X;
                 int y = FindIndex(j, i).Y;

                 if (gameField[j, i] == GameSigns.O)
                     _field = InsertSign(_field, _fileHandler.GetSignPicture(GameSigns.O), x, y);
                 if (gameField[j, i] == GameSigns.X)
                     _field = InsertSign(_field, _fileHandler.GetSignPicture(GameSigns.X), x, y);
             }
         }
     }

     private void IndicateVerticalLines()
     {
         for (int i = 0; i < FieldHeight; i++)
         {
             _field[0, i]._char = '#';
             
             _field[1 + _cellWidth, i]._char = '#';

             _field[2 + 2 * _cellWidth , i]._char = '#';

             _field[3 + 3 * _cellWidth , i]._char = '#';
         }
         
     }

     private void IndicateHorizontalLines()
     {
         for (int i = 0; i < FieldWidth; i++)
         {
             _field[i, 0]._char = '#';

             _field[i, 1 + _cellHeight]._char = '#';

             _field[i, 2 + 2 * _cellHeight]._char = '#';

             _field[i, 3 + 3 * _cellHeight]._char = '#';
         }
     }

     private Pixel[,] InsertSign(Pixel[,] field, int[,] signPictureArray,int x,int y)
     {
         int iteratorX = 0;
         int iteratorY = 0;
         for (int i = y; i < y + _cellHeight ; i++)
         {
             for (int j = x; j < x + _cellWidth ; j++)
             {
                 if (signPictureArray[iteratorY,iteratorX] == 1)
                 {
                     field[j, i]._char = '#';
                 }

                 if (iteratorX == _cellWidth - 1)
                 {
                     iteratorX = 0;
                     continue;
                 }

                 iteratorX++;
                
             }
             
             iteratorY++;
         }

         return field;

     }
     private Pixel[,] InsertSign(Pixel[,] field, string[] signPictureArray,int x,int y)
     {
         int iteratorX = 0;
         int iteratorY = 0;
         
         for (int i = y + 1; i < y + 1 + signPictureArray.Length ; i++) // + 1 = empty space between boarder and sign
         {
             for (int j = x + 2; j < x + 2 + signPictureArray[iteratorY].Length; j++)
             {
                 field[j, i]._char = signPictureArray[iteratorY][iteratorX];
                 if (iteratorX == signPictureArray[iteratorY].Length - 1)
                 {
                     iteratorX = 0;
                     continue;
                 }
                 iteratorX++;
             }
             iteratorY++;
         }
         return field;
     }
     
 }