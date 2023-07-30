using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicTacToe;

class Program
{ 
    static async Task Main(string[] args)
    {
        Game game = new Game();
        FieldPainter fieldPainter = new();
        InputHandler inputHandler = new InputHandler();
        //using var client = new HttpClient();

        game.ShowWelcomeNotification();
        GetGameParams(args, game,fieldPainter);
        
        while (!game.IsGameEnd)
        {
            // var numpadTurnInput = HandleInput();
            fieldPainter.PaintGameField(game.GameField, inputHandler.X, inputHandler.Y);
            var key = Console.ReadKey(true);
            inputHandler.Handle(key);

            if (key.Key == ConsoleKey.Enter)
            {
                game.MakeTurn(inputHandler.X, inputHandler.Y);
            }

            
            await MakePost(game.GameField, inputHandler.X, inputHandler.Y, game.currentSign);
            
            fieldPainter.PaintGameField(game.GameField, inputHandler.X, inputHandler.Y);
              
        }

        game.ShowEndGameNotification(game.Winner);
    }
    
    
    static async Task MakePost(GameSigns[,] gameField, int pointerX, int pointerY, GameSigns currentSign)
    {
        using var client = new HttpClient();
        GameSigns[] gameField2D = gameField.Cast<GameSigns>().ToArray();
        var obj = new
        {
            GameFild = gameField2D,
            PointCoordinateX = pointerX,
            PointCoordinateY = pointerY,
            CurrentSign = currentSign
        };
        var content = JsonContent.Create(obj);
        var result = await client.PostAsync("http://localhost:5213/TicTacToe",content);
        string resultContent = await result.Content.ReadAsStringAsync();
        Console.WriteLine(resultContent);

    }


    public static void GetGameParams(string[] args, Game game, FieldPainter fieldPainter)
    {
        //add dependency sing and color in the console
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