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
     private FileHandler _fileHandler = new FileHandler();
     private Pixel[,] _field = new Pixel[FieldWidth,FieldHeight];
     private const int FieldWidth = 31;
     private const int FieldHeight = 25;
     private const int CellHeight = 7;
     private const int CellWidth = 9;
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
     //TODO Add Pointer on the field,feel build Buffer method,
     public void BuildBufferField(GameSigns[,] gameField, int x,int y)
     {
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
         
         int X = FindIndex(x,y).X;
         int Y = FindIndex(x, y).Y;
         _field[X, Y] = pointer;


     }
     private (int X, int Y) FindIndex(int x,int y)
     {
         return (x + 1 + CellWidth * x,y + 1 + CellHeight * y);
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
                     _field = InsertSign(_field, _o, x, y);
                 if (gameField[j, i] == GameSigns.X)
                     _field = InsertSign(_field, _x, x, y);
             }
         }
     }

     private void IndicateVerticalLines()
     {
         for (int i = 0; i < FieldHeight; i++)
         {
             _field[0, i]._char = '#';
             
             _field[10, i]._char = '#';

             _field[20, i]._char = '#';

             _field[30, i]._char = '#';
         }
         
     }

     private void IndicateHorizontalLines()
     {
         for (int i = 0; i < FieldWidth; i++)
         {
             _field[i, 0]._char = '#';

             _field[i, 8]._char = '#';

             _field[i, 16]._char = '#';

             _field[i, 24]._char = '#';
         }
     }

     private Pixel[,] InsertSign(Pixel[,] field, int[,] signPictureArray,int x,int y)
     {
         int iteratorX = 0;
         int iteratorY = 0;
         for (int i = y; i < y + CellHeight ; i++)
         {
             for (int j = x; j < x + CellWidth ; j++)
             {
                 if (signPictureArray[iteratorY,iteratorX] == 1)
                 {
                     field[j, i]._char = '#';
                 }

                 if (iteratorX == CellWidth - 1)
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