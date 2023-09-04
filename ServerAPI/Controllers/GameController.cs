using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;
[ApiController]
[Route("GameController")]

public class GameController : ControllerBase
{
    private PollingHandler _handler;
    private Game _game;

    public GameController(GameDispenser gameDispenser)
    {
        _game = gameDispenser.DispenseGame();
        _handler = new PollingHandler( _game);
    }
    
    [HttpGet("WaitForTurn")]
    public async Task<IActionResult> WaitForTurn()
    {
        
        while (!_handler.Notified)
        {
            await Task.Delay(1000);
        }
        return Ok(_handler.Consume());
    }
    
    [HttpPost( "MakeTurn")]
    public IActionResult MakeTurn(TurnInfo turnInfo)
    {
        if (_game.IfCanPlayerMakeTurn())
        {
            _game.CanPlayerMakeTurn = false;
            return BadRequest("now it's the other player's turn");
        }
        _game.ChangeGameSign();
        _handler.Notify(turnInfo);
        
        return Ok(_game.GameState);
    }
    
}