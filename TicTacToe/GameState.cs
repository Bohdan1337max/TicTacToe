namespace TicTacToe;

public class GameState
{
    public GameSigns[] GameField { get; set; } = null!;
    public GameSigns CurrentSign { get; set; }
    public int PointCoordinateX { get; set; }
    public int PointCoordinateY { get; set; }
}