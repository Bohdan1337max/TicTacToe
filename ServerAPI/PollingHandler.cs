
using ServerAP.Controllers;

namespace ServerAP;

public class PollingHandler
{
    public bool Notified { get; private set; }
    private readonly Game _game;

    public PollingHandler(GameDispenser gameDispenser)
    {
        _game = gameDispenser.DispenseGame();

    }
    public void Notify(GameState gameState)
    {
        Notified = true;
        _game.GameState = gameState;
    }
    public GameState Consume()
    {
        Notified = false;
        return _game.GameState;
    }
}