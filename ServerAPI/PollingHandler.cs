
using ServerAP.Controllers;

namespace ServerAP;

public class PollingHandler
{
    public bool Notified { get; private set; }
    
    
    public void Notify(GameState gameState)
    {
        Notified = true;
        ServerGame.GameState = gameState;
       
    }
    public GameState Consume()
    {
        Notified = false;
        return ServerGame.GameState;
    }
}