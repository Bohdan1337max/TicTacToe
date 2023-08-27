
using ServerAP.Controllers;

namespace ServerAP;

public class PollingHandler
{
    public bool Notified { get; private set; }
    
    public void Notify(TurnInfo turnInfo)
    {
        Notified = true;
        ServerGame.TurnInfo = turnInfo;
       
    }
    public GameState Consume()
    {
        Notified = false;
        return ServerGame.GameState;
    }
}