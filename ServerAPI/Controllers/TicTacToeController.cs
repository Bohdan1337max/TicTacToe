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
    public IActionResult AddPlayerPost(Player player)
    {
        
        var startGameInfo = new StartGameInfo() {GameState = new GameState
            {
                GameField = new GameSigns[9]
            }
        };
        switch (ServerGame.Players.Count)
        {
            case 0:
                player.Id = 1;
                player.Sign = GameSigns.X;
                ServerGame.Players.Add(player);
                startGameInfo.PlayerSign = player.Sign;
                return Ok(startGameInfo);
            case 1:
                player.Id = 2;
                player.Sign = GameSigns.O;
                startGameInfo.PlayerSign = player.Sign;
                ServerGame.Players.Add(player);
                return Ok(startGameInfo);
            default:
                return BadRequest("Room is Full");
        }
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