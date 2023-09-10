using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;

[ApiController]
[Route("TicTacToe")]
public class TicTacToeController : ControllerBase
{
    private Game _game;
    
    public TicTacToeController(GameDispenser game)
    {
        _game = game.DispenseGame();
    }
    
    [HttpGet]
    public IActionResult GetGameState()
    {
        return Ok(_game.GameState);
    }

    [HttpPost("PostGS")]
    public IActionResult PostGameState(GameState gameState)
    {
        _game.GameState = gameState;
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
            }
        };
        var playerSign = Game.Players.Count switch
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
        Game.Players.Add(player);
        if (player.Sign == _game.CurrentSign)
            startGameInfo.GameState.CanPlayerMakeTurn = true;
        return Ok(startGameInfo);
    }

    [HttpGet("PlayersGet")]
    public IActionResult GetPlayers()
    {
        return Ok(Game.Players);
    }

    [HttpGet("IsGameReady")]
    public IActionResult IsGameReady()
    {
        return Ok(Game.Players.Count == 2);
    }
}