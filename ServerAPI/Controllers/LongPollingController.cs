using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ServerAP.Controllers;

public class LongPollingController : ControllerBase
{
    private static PollingHandler handler = new PollingHandler();
    
//make post and end longPoll
    [HttpGet("Poll")]
    public async Task<IActionResult> LongPoll()
    {
        
        while (!handler.Notified)
        {
            await Task.Delay(1000);
        }
        
        
        return Ok(handler.Consume());
        
    }

    [HttpPost( "Post")]

    public IActionResult SendData( GameState gameState)
    {
        handler.Notify(gameState);
        return Ok();
    }


}