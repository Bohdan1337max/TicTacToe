using System;
namespace TicTacToe;

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        FieldPainter fieldPainter = new ();
        while (!game.IsGameEnd)
        {
            int playerTurn = Convert.ToInt32(Console.ReadLine());
            game.DefineSign(playerTurn);
            game.FindWinCombination(playerTurn);
            fieldPainter.FieldCreate(game.GameField);
            game.TurnIncrease();
            
        }



    }
}