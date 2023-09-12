namespace ServerAP;

public class GameDispenser
{
    private readonly Game _game = new ();
    
    public Game DispenseGame()
    {
        return _game;
    }
    
}