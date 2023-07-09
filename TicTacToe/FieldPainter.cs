using System;
 
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
     private FieldDye[,] _field = new FieldDye[FieldHeight,FieldWidth];
     private const int FieldWidth = 31;
     private const int FieldHeight = 25;
     private const int CellHeight = 7;
     private const int CellWidth = 9;

     int[,] X = new int[,]
     {
         {0, 0, 0, 0, 0, 0, 0, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 0, 1, 0, 1, 0, 0, 0},
         {0, 0, 0, 0, 1, 0, 0, 0, 0},
         {0, 0, 0, 1, 0, 1, 0, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 0, 0, 0, 0, 0, 0, 0}
     };
     int[,] O = new int[,]
     {
         {0, 0, 0, 0, 0, 0, 0, 0, 0},
         {0, 0, 0, 1, 1, 1, 0, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 1, 0, 0, 0, 1, 0, 0},
         {0, 0, 0, 1, 1, 1, 0, 0, 0},
         {0, 0, 0, 0, 0, 0, 0, 0, 0}
     };

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
     
     
     public void PaintGameField(GameSigns[,] gameField)
     {
         IndicateHorizontalLines();
         IndicateVerticalLines();
         
         for (int i = 0; i < 3; i++)
         {
             for (int j = 0; j < 3; j++)
             {
                 int x = FindIndex(i,j).X;
                 int y = FindIndex(i, j).Y;
                 
                 if (gameField[i, j] == GameSigns.O)
                     _field = InsertSign(_field, O,x,y);
                 if (gameField[i, j] == GameSigns.X)
                     _field = InsertSign(_field, X, x, y);


             }
         }
         for (int i = 0; i < FieldHeight; i++)
         {
             for (int j = 0; j < FieldWidth; j++)
             {
                 if (_field[i, j] == FieldDye.Sign)
                 {
                     Console.Write("#");
                 }
                 else
                 {
                     Console.Write(' ');
                 }
                 if(j == FieldWidth - 1)
                     Console.WriteLine();

             }
         }
         
     }
//TODO
     /*
     /0  1  2
     {7, 8, 9}, //0
     {4, 5, 6}, //1
     {1, 2, 3}  //2
     */
     private (int X, int Y) FindIndex(int x,int y)
     {
         return (x + 1 + CellHeight * x,y + 1 + CellWidth * y);
     }
     
  
     private void IndicateHorizontalLines()
     {
         for (int i = 0; i < FieldWidth; i++)
         {
             _field[0, i] = FieldDye.Sign;
             
             _field[8, i] = FieldDye.Sign;

             _field[16, i] = FieldDye.Sign;

             _field[24, i] = FieldDye.Sign;
         }
         
     }

     private void IndicateVerticalLines()
     {
         for (int i = 0; i < FieldHeight; i++)
         {
             _field[i, 0] = FieldDye.Sign;

             _field[i, 10] = FieldDye.Sign;

             _field[i, 20] = FieldDye.Sign;

             _field[i, 30] = FieldDye.Sign;
         }
     }

     private FieldDye[,] InsertSign(FieldDye[,] field, int[,] signPictureArray,int x,int y)
     {
         int iteratorX = 0;
         int iteratorY = 0;
         for (int i = x; i < x + CellHeight; i++)
         {
             for (int j = y; j < y + CellWidth ; j++)
             {
                 if (signPictureArray[iteratorX,iteratorY] == 1)
                 {
                     field[i, j] = FieldDye.Sign;
                 }

                 if (iteratorY == CellWidth - 1)
                 {
                     iteratorY = 0;
                     continue;
                 }

                 iteratorY++;
                
             }
             
             iteratorX++;
         }

         return field;

     }




 }