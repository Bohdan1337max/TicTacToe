using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;
[ApiController]
[Route("TicTacToe")]

public class TicTacToeController : ControllerBase
{
    
    public static List<GameState> GameStates = new ();
    public static List<Player> Players = new ();

    [HttpGet]
    public IActionResult GetGameState()
    {
        return Ok(GameStates);
    }

    [HttpPost]
    public IActionResult PostGameState(GameState gameState)
    {
        GameStates.Add(gameState);
        
        return Ok();
    }

    [HttpPost("AddPlayer")]
    public IActionResult AddPlayerPost(Player player)
    {
        switch (Players.Count)
        {
            case 0:
                player.Id = 1;
                player.Sign = GameSigns.X;
                Players.Add(player);
                return Ok(player);
            case 1:
                player.Id = 2;
                player.Sign = GameSigns.O;
                Players.Add(player);
                return Ok("Last player connected");
            default:
                return NotFound("Room is Full");
        }
    }

    [HttpGet("PlayersGet")]
    public IActionResult GetPlayers()
    {
        return Ok(Players);
    }
    
}