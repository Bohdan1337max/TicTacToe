using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;
[ApiController]
[Route("LongPolling")]

public class LongPollingController : ControllerBase
{
    private static PollingHandler handler = new ();
    

    [HttpGet("Poll")]
    public async Task<IActionResult> LongPoll()
    {
        
        while (!handler.Notified)
        {
            await Task.Delay(1000);
        }
        
        
        return Ok(handler.Consume());
    }

    
    [HttpPost( "PostGameState")]
    public IActionResult SendGameState(GameState gameState)
    {
        if (gameState.TurnSign != TicTacToeController.CurrentTurnSign)
            return BadRequest("now it's the other player's turn");
        
        handler.Notify(gameState);
        TicTacToeController.CurrentTurnSign = gameState.TurnSign == GameSigns.X ? GameSigns.O : GameSigns.X;
        
        return Ok(gameState);
    }
    
}





