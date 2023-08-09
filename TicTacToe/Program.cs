using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TicTacToe;

internal static class Program
{
    static async Task Main(string[] args)
    {
        Game game = new Game();
        FieldPainter fieldPainter = new();
        InputHandler inputHandler = new InputHandler();
        //using var client = new HttpClient();

        game.ShowWelcomeNotification();
        GetGameParams(args, game, fieldPainter);
        //make waiting room
        var joined = await JoinToTheGame(game);
        if (!joined)
        {
            return;
        }
            
        while (!game.IsGameEnd)
        {

            // var numpadTurnInput = HandleInput();
            fieldPainter.PaintGameField(game.GameField, inputHandler.X, inputHandler.Y);
            var key = Console.ReadKey(true);
            inputHandler.Handle(key);

            if (key.Key == ConsoleKey.Enter)
            {
                game.MakeTurn(inputHandler.X, inputHandler.Y);
                await PostGameState(game);
                await GetGameState(game);
            }


            fieldPainter.PaintGameField(game.GameField, inputHandler.X, inputHandler.Y);

        }

        game.ShowEndGameNotification(game.Winner);
    }
    
    
    static async Task PostGameState(Game game)
    {
        try
        {
            const string baseUrl = "http://localhost:5213/LongPolling/PostGameState";
            using var client = new HttpClient();
            var gameState = game.GameStateCollector();
            //gameState.PointerCoordinateX = pointerX;
            //gameState.PointerCoordinateY = pointerY;
            
            var content = JsonContent.Create(gameState);
            var response = await client.PostAsync(baseUrl, content);
            string responseContent = await response.Content.ReadAsStringAsync();

        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Request error: " + ex.Message);
        }

    }

    static async Task GetGameState(Game game)
    {
        try
        {
            const string baseUrl = "http://localhost:5213/LongPolling/Poll";
            using var client = new HttpClient();
            var response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                DeserializeGameStateFromJson(responseContent,game);    
            }


        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        
    }
    


    private static void DeserializeGameStateFromJson(string responseContent, Game game)
    {

        var gameState = JsonSerializer.Deserialize<GameState>(responseContent);
        if (gameState == null) return;
        game.currentSign = gameState.TurnSign;
        game.GameField = ConvertTo2DArray(gameState.GameField);
        
    }


    private static GameSigns[,] ConvertTo2DArray(GameSigns[] array)
    {
        var gameField = new GameSigns[3, 3];
        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gameField[i, j] = array[index++];
            }
        }

        return gameField;
    }
    static async Task<bool> JoinToTheGame(Game game)
    {
        const string baseUrl = "http://localhost:5213/TicTacToe/AddPlayer";
        using var client = new HttpClient();
        Player player = new Player();
        var content = JsonContent.Create(player); 
        var response = await client.PostAsync(baseUrl,content);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return false;
        var definedPlayerSign = JsonSerializer.Deserialize<GameSigns>(responseContent);
        game.currentSign = definedPlayerSign;
        return true;


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