using System.Text.Json.Serialization;

namespace ServerAP;
public enum GameSigns
{
    Empty,
    O,
    X,
}
public class GameState
{
    public GameSigns[] GameField { get; set; } = null!;
    public GameSigns TurnSign { get; set; }
}