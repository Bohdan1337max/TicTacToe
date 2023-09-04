using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;
[ApiController]
[Route("GameController")]

public class LongPollingController : ControllerBase
{
    private Game _game;
    private PollingHandler _handler;
    
    public LongPollingController(GameDispenser game)
    {
        _game = game.DispenseGame();
        _handler = new PollingHandler(_game);
    }
    
    [HttpGet("WaitForTurn")]
    public async Task<IActionResult> WaitingForTurn()
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
        if (!_game.IfCanPlayerMakeTurn())
        {
            _game.CanPlayerMakeTurn = false;
            return BadRequest("Now it's the other player's turn");
        }
//Make turb from game!
         _game.MakeTurn(turnInfo.X,turnInfo.Y);
        _game.ChangeGameSign();
        _handler.Notify(turnInfo);
        
        turnInfo.PlayerSign = _game.CurrentSign;
        
        return Ok(_game.GameState);
    }
    
}