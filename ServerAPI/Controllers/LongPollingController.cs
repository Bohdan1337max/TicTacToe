using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;
[ApiController]
[Route("GameController")]

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
    public IActionResult SendGameState(TurnInfo turnInfo)
    {
        if (turnInfo.PlayerSign != Game.CurrentSign)
            return BadRequest("now it's the other player's turn");
        
        Game.CurrentSign = turnInfo.PlayerSign == GameSigns.X ? GameSigns.O : GameSigns.X;
        Handler.Notify(turnInfo);
        turnInfo.PlayerSign = Game.CurrentSign;
        
        return Ok(turnInfo);
    }
    
}