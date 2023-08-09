using System;
 
 namespace TicTacToe;
 public enum GameSigns
 {
     Empty,
     O,
     X,
         
 }
 
 
 // Зробити сервер для чепухи 
 // 3 окремих процесів
 // http client
 // like API
 //Зробити сервер і окремо термінал
 
 //Записувати історію матчів в файл
 
 public class FieldPainter
 {
     private readonly FileHandler _fileHandler = new FileHandler();
     public Pixel[,] Field;
     public ConsoleColor ColorX { get; set;}
     public ConsoleColor ColorO { get; set; }
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
         Field = new Pixel[FieldWidth, FieldHeight];

     }
     
     public void PaintGameField(GameSigns[,] gameField,int x,int y)
     {
         Console.Clear();
         BuildBufferField(gameField,x,y);
         for (int i = 0; i < FieldHeight; i++)
         {
             for (int j = 0; j < FieldWidth; j++)
             {
                 Console.BackgroundColor = Field[j, i]._color;
                 Console.Write(Field[j, i]._char);

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
         Array.Clear(Field);   
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
         Field[xPointerIndex, yPointerIndex] = pointer;


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
                     Field = InsertSign(Field, _fileHandler.GetSignPicture(GameSigns.O), x, y,ColorO);
                 if (gameField[j, i] == GameSigns.X)    
                     Field = InsertSign(Field, _fileHandler.GetSignPicture(GameSigns.X), x, y,ColorX);
             }
         }
     }

     private void IndicateVerticalLines()
     {
         for (int i = 0; i < FieldHeight; i++)
         {
             Field[0, i]._char = '#';
             
             Field[1 + _cellWidth, i]._char = '#';

             Field[2 + 2 * _cellWidth , i]._char = '#';

             Field[3 + 3 * _cellWidth , i]._char = '#';
         }
         
     }

     private void IndicateHorizontalLines()
     {
         for (int i = 0; i < FieldWidth; i++)
         {
             Field[i, 0]._char = '#';

             Field[i, 1 + _cellHeight]._char = '#';

             Field[i, 2 + 2 * _cellHeight]._char = '#';

             Field[i, 3 + 3 * _cellHeight]._char = '#';
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
     
     // Чи стрінг це просто массив чарів?
     
     private Pixel[,] InsertSign(Pixel[,] field, string[] signPictureArray,int x,int y, ConsoleColor color)
     {
         int iteratorX = 0;
         int iteratorY = 0;
         
         for (int i = y + 1; i < y + 1 + signPictureArray.Length ; i++) // + 1 = empty space between boarder and sign
         {
             for (int j = x + 2; j < x + 2 + signPictureArray[iteratorY].Length; j++)
             {
                 field[j, i]._char = signPictureArray[iteratorY][iteratorX];
                 field[j, i]._color = color;
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