using System;
using System.Linq;
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
    //add http client in constructor
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = {new JsonStringEnumConverter()},
        PropertyNameCaseInsensitive = true
    };

    public async Task JoinToTheGame(MultiPlayerGame multiPlayerGame)
    {
        const string baseUrl = "http://localhost:5213/TicTacToe/AddPlayer";
        using var client = new HttpClient();
        var player = new Player();
        var content = JsonContent.Create(player);
        var response = await client.PostAsync(baseUrl, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var startGameInfo = JsonSerializer.Deserialize<StartGameInfo>(responseContent, _options);
        
        if (startGameInfo != null)
        {
            Console.WriteLine($"Welcome your player sign is {startGameInfo.PlayerSign}");
            multiPlayerGame.CurrentSign = startGameInfo.PlayerSign;
            multiPlayerGame.CanPlayerMakeTurn = startGameInfo.GameState.CanPlayerMakeTurn;
            multiPlayerGame.GameField = ConvertTo2DArray(startGameInfo.GameState.GameField);
        }
    }

    public async Task<bool> IsGameStarted()
    {
        const string baseUrl = "http://localhost:5213/TicTacToe/IsGameReady";
        using var client = new HttpClient();
        var response = await client.GetAsync(baseUrl);
        var responseContent = await response.Content.ReadAsStringAsync();
        var isGameStarted = JsonSerializer.Deserialize<bool>(responseContent, options: _options);
        return isGameStarted;
    }

    public async Task ServerMakeTurn(MultiPlayerGame multiPlayerGame)
    {
        try
        {
            const string baseUrl = "http://localhost:5213/GameController/MakeTurn";
            using var client = new HttpClient();
            var turnInfo = multiPlayerGame.TurnInfoCollector();
            var jsonContent = JsonSerializer.Serialize(turnInfo, options: _options);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(baseUrl, content);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var gameStateFromServer = JsonSerializer.Deserialize<GameState>(responseContentString, options: _options);
            
            if (gameStateFromServer != null)
            {
                //make separate method or constructor but how field painter in actual gameStateFromServer??!
                multiPlayerGame.IsGameEnd = gameStateFromServer.IsGameEnd;
                multiPlayerGame.GameField = ConvertTo2DArray(gameStateFromServer.GameField);
                multiPlayerGame.CanPlayerMakeTurn = gameStateFromServer.CanPlayerMakeTurn;
                multiPlayerGame.Winner = gameStateFromServer.Winner;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Request error: " + ex.Message);
        }
    }
    
    public async Task WaitForTurn(MultiPlayerGame multiPlayerGame)
    {
        try
        {
            const string baseUrl = "http://localhost:5213/GameController/WaitForTurn";
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            var response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                DeserializeGameStateFromJson(responseContent,multiPlayerGame);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    private void DeserializeGameStateFromJson(string responseContent, MultiPlayerGame multiPlayerGame )
    {
        var gameState = JsonSerializer.Deserialize<GameState>(responseContent, options: _options);
        if (gameState == null) return;
        
        multiPlayerGame.GameField = ConvertTo2DArray(gameState.GameField);
        multiPlayerGame.Winner = gameState.Winner;
        multiPlayerGame.IsGameEnd = gameState.IsGameEnd;
        multiPlayerGame.CanPlayerMakeTurn = gameState.CanPlayerMakeTurn;
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