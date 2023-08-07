
using ServerAP.Controllers;

namespace ServerAP;

public class PollingHandler
{
    
    
    public bool Notified { get;  set; }
    
    
    public void Notify(GameState gameState)
    {
        Notified = true;
        TicTacToeController.GameStates.Add(gameState);
    }
    public GameState Consume()
    {
        Notified = false;

        var lastGameState = TicTacToeController.GameStates.LastOrDefault();
        return lastGameState ?? new GameState();
    }
}