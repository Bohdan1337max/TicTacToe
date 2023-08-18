using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe;

public class Services
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = {new JsonStringEnumConverter()},
        PropertyNameCaseInsensitive = true
    };

    public async Task JoinToTheGame(Game game)
    {
        const string baseUrl = "http://localhost:5213/TicTacToe/AddPlayer";
        using var client = new HttpClient();
        Player player = new Player();
        var content = JsonContent.Create(player);
        var response = await client.PostAsync(baseUrl, content);
        string responseContent = await response.Content.ReadAsStringAsync();
        var startGameInfo = JsonSerializer.Deserialize<StartGameInfo>(responseContent, _options);
        if (startGameInfo != null)
        {
            game.CurrentSign = startGameInfo.PlayerSign;
            game.GameField = ConvertTo2DArray(startGameInfo.GameState.GameField);
        }
    }

    public async Task<bool> IsGameStarted()
    {
        const string baseUrl = "http://localhost:5213/TicTacToe/IsGameReady";
        using var client = new HttpClient();
        var response = await client.GetAsync(baseUrl);
        string responseContent = await response.Content.ReadAsStringAsync();
        return responseContent == "true";
    }

    public async Task PostGameState(Game game)
    {
        try
        {
            const string baseUrl = "http://localhost:5213/LongPolling/PostGameState";
            using var client = new HttpClient();
            var gameState = game.GameStateCollector();
            var jsonContent = JsonSerializer.Serialize(gameState, options: _options);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(baseUrl, content);
            string responseContentString = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<GameState>(responseContentString, options: _options);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Request error: " + ex.Message);
        }
    }


    public async Task GetGameState(Game game)
    {
        try
        {
            const string baseUrl = "http://localhost:5213/LongPolling/Poll";
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            var response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                DeserializeGameStateFromJson(responseContent, game);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    private void DeserializeGameStateFromJson(string responseContent, Game game)
    {
        var gameState = JsonSerializer.Deserialize<GameState>(responseContent, options: _options);
        if (gameState == null) return;
        game.CurrentSign = gameState.TurnSign;
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
}