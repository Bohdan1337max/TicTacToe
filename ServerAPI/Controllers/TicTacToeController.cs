using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;

[ApiController]
[Route("TicTacToe")]
public class TicTacToeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetGameState()
    {
        return Ok(ServerGame.GameState);
    }

    [HttpPost("PostGS")]
    public IActionResult PostGameState(GameState gameState)
    {
        ServerGame.GameState = gameState;
        return Ok(gameState);
    }

    [HttpPost("AddPlayer")]
    public IActionResult AddPlayer(Player player)
    {
        var startGameInfo = new StartGameInfo()
        {
            GameState = new GameState
            {
                GameField = new GameSigns[9],
                TurnSign = ServerGame.CurrentTurnSign
            }
        };
        var playerSign = ServerGame.Players.Count switch
        {
            0 => GameSigns.X,
            1 => GameSigns.O,
            _ => GameSigns.Empty
        };
        if (playerSign == GameSigns.Empty)
            return BadRequest("Server is full");
        player.Id = (int) playerSign;
        player.Sign = playerSign;
        startGameInfo.PlayerSign = player.Sign;
        ServerGame.Players.Add(player);
        return Ok(startGameInfo);
    }

    [HttpGet("PlayersGet")]
    public IActionResult GetPlayers()
    {
        return Ok(ServerGame.Players);
    }

    [HttpGet("IsGameReady")]
    public IActionResult IsGameReady()
    {
        return Ok(ServerGame.Players.Count == 2);
    }
}