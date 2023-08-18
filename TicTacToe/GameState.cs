using System.Text.Json.Serialization;

namespace TicTacToe;

public class GameState
{
    
    public GameSigns[] GameField { get; set; } = null!;
    public GameSigns TurnSign { get; set; }
}