using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;
[ApiController]
[Route("TicTacToe")]

public class TicTacToeController : ControllerBase
{
    
    public static List<GameState> GameStates = new List<GameState>();

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
    
}