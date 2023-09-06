using System;
using System.Threading.Tasks;

namespace TicTacToe;

internal static class Program
{
    static async Task Main(string[] args)
    {
        InputHandler inputHandler = new InputHandler();
        FieldPainter fieldPainter = new();
        await StartMultiPlayerGame(fieldPainter, inputHandler);
    }

    private static async Task StartMultiPlayerGame( FieldPainter fieldPainter,InputHandler inputHandler)
    {
        var services = new Services();
        MultiPlayerGame multiPlayerGame = new MultiPlayerGame(services , inputHandler);
        
        await services.JoinToTheGame(multiPlayerGame);
        
        while (!await services.IsGameStarted())
        {
            await Task.Delay(2000);
            Console.WriteLine("Waiting for second player"); 
        }
        
        while (!multiPlayerGame.IsGameEnd)
        {
            fieldPainter.PaintGameField(multiPlayerGame.GameField, inputHandler.X, inputHandler.Y);
            
            await multiPlayerGame.CheckCurrentTurn();
            
            var key = Console.ReadKey(true);
            inputHandler.Handle(key);

            if (key.Key == ConsoleKey.Enter)
            {
                await multiPlayerGame.MakeTurn(inputHandler.X, inputHandler.Y);
            }
            fieldPainter.PaintGameField(multiPlayerGame.GameField, inputHandler.X, inputHandler.Y);
        }

        multiPlayerGame.ShowEndGameNotification(multiPlayerGame.Winner);
        
    }

    private static void StartSinglePlayerGame()
    {
        
    }
    
    private static void GetGameParams(string[] args, MultiPlayerGame multiPlayerGame, FieldPainter fieldPainter)
    {
        string colorX;
        string colorO;
        if (args.Length < 1)
            return;

        if (args[0] == "O")
        {
            multiPlayerGame.CurrentSign = GameSigns.O;
        }

        if (args[0] == "X")
        {
            multiPlayerGame.CurrentSign = GameSigns.X;
           
        }
        
        switch (args.Length)
        {
            case 2:
            {
                colorX = args[1];
                if (CheckColor(colorX).IsParsed)
                {
                    fieldPainter.ColorX = CheckColor(colorX).color;
                }
                else
                {
                    Console.WriteLine("You choose wrong Color");
                }

                break;
            }
            case 3:
            {
                colorX = args[1];
                colorO = args[2];
            
                if (CheckColor(colorX).IsParsed && CheckColor(colorO).IsParsed)
                {
                    fieldPainter.ColorX = CheckColor(colorX).color;
                    fieldPainter.ColorO = CheckColor(colorO).color;
                }
                else
                {
                    Console.WriteLine("You choose wrong color");
                }

                break;
            }
        }
    }

    private static (bool IsParsed ,ConsoleColor color) CheckColor(string color)
    {
        return Enum.TryParse(color, out ConsoleColor parsedColor) ? (true, parsedColor) : (false, ConsoleColor.Black);
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