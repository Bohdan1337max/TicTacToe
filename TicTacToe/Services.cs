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
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };


    public Services()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://localhost:5213/");
    }

    public async Task JoinToTheGame(MultiPlayerGame multiPlayerGame)
    {
        var player = new Player();
        var content = JsonContent.Create(player);
        var response = await _client.PostAsync("TicTacToe/AddPlayer", content);
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
        var response = await _client.GetAsync("TicTacToe/IsGameReady");
        var responseContent = await response.Content.ReadAsStringAsync();
        var isGameStarted = JsonSerializer.Deserialize<bool>(responseContent, options: _options);
        return isGameStarted;
    }

    public async Task ServerMakeTurn(MultiPlayerGame multiPlayerGame)
    {
        try
        {
            var turnInfo = multiPlayerGame.CollectTurnInfo();
            var jsonContent = JsonSerializer.Serialize(turnInfo, options: _options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("GameController/MakeTurn", content);
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
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(Timeout.Infinite);
            //_client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            var response = await _client.GetAsync("GameController/WaitForTurn", cts.Token);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cts.Token);
                DeserializeGameStateFromJson(responseContent, multiPlayerGame);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    private void DeserializeGameStateFromJson(string responseContent, MultiPlayerGame multiPlayerGame)
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