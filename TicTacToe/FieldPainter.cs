using System;
 
 namespace TicTacToe;
 public enum GameSigns
 {
     Empty,
     O,
     X,
         
 }
 public class FieldPainter 
 {
     
     public void PaintField (GameSigns[,] gameField)
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
     
     
     
  
 }