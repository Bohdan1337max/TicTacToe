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
    public int PointerCoordinateX { get; set; }
    public int PointerCoordinateY { get; set; }
}