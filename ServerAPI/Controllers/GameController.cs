using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;
[ApiController]
[Route("GameController")]

public class GameController : ControllerBase
{
    private readonly Game _game;
    private readonly PollingHandler _handler;

    public GameController(GameDispenser gameDispenser,PollingHandler pollingHandler)
    {
        _game = gameDispenser.DispenseGame();
        _handler = pollingHandler;
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
    
    //Make Normal Turn Flow
    [HttpPost( "MakeTurn")]
    public  IActionResult MakeTurn(TurnInfo turnInfo)
    {
        
        _game.TurnInfo = turnInfo;
        
        var turnValidator = _game.ValidateTurn();
        if (!turnValidator.isTurnValid)
        {
            return BadRequest(turnValidator.errorMessage);
        }
        
        _game.MakeTurn();
        _game.ChangeGameSign();
        _game.GameStateCollect();
        
        _handler.Notify(turnInfo);
        
        return Ok(_game.GameState);
    }
    
}