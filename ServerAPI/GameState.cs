using System.Text.Json.Serialization;

namespace ServerAP;
public enum GameSigns
{
    Empty = 0,
    O = 2,
    X = 1,
}
public class GameState
{
    public GameSigns[] GameField { get; set; } = null!;
    public int X { get; set; }
    public int Y { get; set; }
    public GameSigns TurnSign { get; set; }
}