
using ServerAP.Controllers;

namespace ServerAP;

public class PollingHandler
{
    public bool Notified { get; private set; }
    
    public void Notify(TurnInfo turnInfo)
    {
        Notified = true;
        Game.TurnInfo = turnInfo;
       
    }
    public GameState Consume()
    {
        Notified = false;
        return Game.GameState;
    }
}