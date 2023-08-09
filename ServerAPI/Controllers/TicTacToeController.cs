using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;
[ApiController]
[Route("TicTacToe")]

public class TicTacToeController : ControllerBase
{
    
    public static List<GameState> GameStates = new ();
    public static List<Player> Players = new ();
    public static GameSigns CurrentTurnSign { get; set; } = GameSigns.X;


    [HttpGet]
    public IActionResult GetGameState()
    {
        return Ok(GameStates);
    }

    [HttpPost("PostGS")]
    public IActionResult PostGameState(GameState gameState)
    {
        GameStates.Add(gameState);
        return Ok(gameState);
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
                return Ok(GameSigns.X);
            case 1:
                player.Id = 2;
                player.Sign = GameSigns.O;
                Players.Add(player);
                return Ok(GameSigns.O);
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