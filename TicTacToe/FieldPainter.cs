using System;
 
 namespace TicTacToe;
 public class FieldPainter 
 {
     public void FieldCreate(int[,] gameField)
     {
         for (int i = 0; i <= 2; i++)
         {
             for (int j = 0; j <= 2; j++)
             {
                 if(gameField[i,j] == 1 )
                     Console.Write('X');
                 if (gameField[i,j] == 2)
                     Console.Write('O');
                 if(gameField[i,j] == 0)
                     Console.Write(' ');
             }
             Console.Write('|');
             Console.WriteLine();
             if(i == 2)
                 Console.WriteLine("---");
         }
     }
     /*public void SectorCreate()
     {
         for (int i = 0; i <= _sectorSize; i++)
         {
             if (i == 0 || i == _sectorSize)
             {
                 var iterator = 0;
                 while (iterator != _sectorSize)
                 {
                     Console.Write("#");
                     iterator++;
                 }
                 Console.WriteLine();
                 continue;
             }
             
             for (int j = 0; j < _sectorSize; j++)
             {
                 if (j == 0 || j == _sectorSize-1)
                     Console.Write('#');
                 Console.Write(' ');
                
             }
             Console.WriteLine();
             
         }
     }*/
  
 }