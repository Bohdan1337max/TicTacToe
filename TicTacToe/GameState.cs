using System.Text.Json.Serialization;

namespace TicTacToe;

public class GameState
{
    
    public GameSigns[] GameField { get; set; } = null!;
    public int X { get; set; }
    public int Y { get; set; }
    public GameSigns TurnSign { get; set; }
}