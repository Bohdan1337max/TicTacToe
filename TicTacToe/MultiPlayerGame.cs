using System;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe;
public enum GameSigns
{
    Empty,
    O,
    X,
         
}
public class MultiPlayerGame
{
    private readonly int[,] _field = new int[3, 3]
    { // 0  1  2
        {7, 8, 9}, //0
        {4, 5, 6}, //1
        {1, 2, 3}  //2
    };

    public GameSigns Winner { get; set; }
    public GameSigns[,] GameField = new GameSigns[3, 3];
    public GameSigns CurrentSign = GameSigns.X;
    public bool CanPlayerMakeTurn { get; set; }
    public bool IsGameEnd;
    private readonly InputHandler _inputHandler;
    private readonly Services _services;

    public MultiPlayerGame(Services services, InputHandler inputHandler)
    {
        _services = services;
        _inputHandler = inputHandler;
    }

    public GameSigns SignFromServer { get; set; }

   
    public async Task MakeTurn(int x, int y)
    {
        if (GameField[x, y] != GameSigns.Empty)
            return;
        
        if (!CanPlayerMakeTurn)
        {
            Console.WriteLine("now it's the other player's turn");
            await _services.WaitForTurn( this);
        }
        
        await _services.ServerMakeTurn(this);
        
    }
    
    
    public TurnInfo TurnInfoCollector()
    {
        return new TurnInfo()
        {
            X = _inputHandler.X,
            Y = _inputHandler.Y,
            PlayerSign = CurrentSign
        };
    }
    
    private bool IsNowMyTurn()
    {
        return CurrentSign == SignFromServer;
    }
    

    public void ShowEndGameNotification(GameSigns winner)
    {
        Console.WriteLine(
            winner == GameSigns.Empty ? "DRAW!! Game field is full" : $"Congratulations {winner} WIN!!!!!");
    }
    
    public void ShowWelcomeNotification()
    {
        Console.WriteLine("Welcome! Red# is your pointer.You can use arrows for navigate on the field!");
    }
}