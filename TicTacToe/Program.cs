using System;
namespace TicTacToe;
//TODO Clear console after turn
class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        FieldPainter fieldPainter = new ();
        game.ShowWelcomeNotification();
        while (!game.IsGameEnd)
        {
            
            var numpadTurnInput = HandleInput();
            game.MakeTurn(numpadTurnInput);
            fieldPainter.PaintField(game.GameField);
            
        }
        game.ShowEndGameNotification(game.Winner);
    }

    private static bool ValidateInput(string? turnInput)
    {
        if (int.TryParse(turnInput, out var correctInput))
        {
            if (correctInput is >= 1 and <= 9)
            {
                return true;
            }
            
        }

        return false;
    }

    private static int HandleInput()
    {
        
        while (true)
        {
            string? turnInput = Console.ReadLine();
            if (ValidateInput(turnInput))
            {
                return Convert.ToInt32(turnInput);
            }

            Console.WriteLine("You choose wrong number.Try again!");

        }
    }
}