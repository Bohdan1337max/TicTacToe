
using ServerAP.Controllers;

namespace ServerAP;

public class PollingHandler
{
    public bool Notified { get; private set; }
    private Game _game;

    public PollingHandler(Game game)
    {
        _game = game;
    }
    public void Notify(TurnInfo turnInfo)
    {
        Notified = true;
        _game.TurnInfo = turnInfo;
       
    }
    public GameState Consume()
    {
        Notified = false;
        return _game.GameState;
    }
}