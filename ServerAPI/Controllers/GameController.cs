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
    
    [HttpPost( "MakeTurn")]
    public  IActionResult MakeTurn(TurnInfo turnInfo)
    {
        var (isValid, errorMessage) = _game.ValidateTurn(turnInfo.X,turnInfo.Y,turnInfo.PlayerSign);
        
        if (!isValid)
        {
            return BadRequest(errorMessage);
        }
        _game.MakeTurn(turnInfo.X,turnInfo.Y);
        
        _game.ChangeGameSign(turnInfo.PlayerSign);

        _game.CanPlayerMakeTurn = true;
        
        var gameState = _game.GameStateCollect();
        
        _handler.Notify(gameState);

        while (_handler.Notified)
        {
            _game.CanPlayerMakeTurn = false;
        }
        
        gameState = _game.GameStateCollect();
        
        return Ok(gameState);
    }
    
}