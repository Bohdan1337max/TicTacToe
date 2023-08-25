using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;
[ApiController]
[Route("LongPolling")]

public class LongPollingController : ControllerBase
{
    private static readonly PollingHandler Handler = new ();

    [HttpGet("WaitForTurn")]
    public async Task<IActionResult> LongPoll()
    {
        
        while (!Handler.Notified)
        {
            await Task.Delay(1000);
        }
        return Ok(Handler.Consume());
    }
    
    [HttpPost( "MakeTurn")]
    public IActionResult SendGameState(GameState gameState)
    {
        if (gameState.TurnSign != ServerGame.CurrentTurnSign)
            return BadRequest("now it's the other player's turn");
        
        ServerGame.CurrentTurnSign = gameState.TurnSign == GameSigns.X ? GameSigns.O : GameSigns.X;
        Handler.Notify(gameState);
        gameState.TurnSign = ServerGame.CurrentTurnSign;
        
        return Ok(gameState);
    }
}