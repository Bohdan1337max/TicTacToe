using System;
using System.Threading.Tasks;

namespace TicTacToe;

internal static class Program
{
    static async Task Main(string[] args)
    {
        InputHandler inputHandler = new InputHandler();
        FieldPainter fieldPainter = new();
        Console.WriteLine("Do you want play on your computer or on the server?");
        Console.WriteLine("1: On the Server");
        Console.WriteLine("2: On yor computer");


        switch (HandleInput())
        {
            case 1:
                await StartMultiPlayerGame(fieldPainter, inputHandler);
                break;
            case 2:
                StartSinglePlayerGame(fieldPainter, inputHandler, args);
                break;
        }
        
    }

    private static async Task StartMultiPlayerGame(FieldPainter fieldPainter, InputHandler inputHandler)
    {
        var services = new Services();
        MultiPlayerGame multiPlayerGame = new MultiPlayerGame(services, inputHandler);

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

            fieldPainter.PaintGameField(multiPlayerGame.GameField, inputHandler.X, inputHandler.Y);

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

    private static void StartSinglePlayerGame(FieldPainter fieldPainter, InputHandler inputHandler, string[] args)
    {
        var game = new Game();
        GetGameParams(args, game, fieldPainter);
        game.ShowWelcomeNotification();
        while (!game.IsGameEnd)
        {
            fieldPainter.PaintGameField(game.GameField, inputHandler.X, inputHandler.Y);
            var key = Console.ReadKey(true);
            inputHandler.Handle(key);

            if (key.Key == ConsoleKey.Enter)
            {
                game.MakeTurn(inputHandler.X, inputHandler.Y);
            }

            fieldPainter.PaintGameField(game.GameField, inputHandler.X, inputHandler.Y);
        }

        game.ShowEndGameNotification(game.Winner);
    }

    private static void GetGameParams(string[] args, Game game, FieldPainter fieldPainter)
    {
        string colorX;
        string colorO;
        if (args.Length < 1)
            return;

        if (args[0] == "O")
        {
            game.currentSign = GameSigns.O;
        }

        if (args[0] == "X")
        {
            game.currentSign = GameSigns.X;
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


    private static (bool IsParsed, ConsoleColor color) CheckColor(string color)
    {
        return Enum.TryParse(color, out ConsoleColor parsedColor) ? (true, parsedColor) : (false, ConsoleColor.Black);
    }


    private static bool ValidateInput(string? turnInput)
    {
        if (int.TryParse(turnInput, out var correctInput))
        {
            if (correctInput is >= 1 and <= 2)
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