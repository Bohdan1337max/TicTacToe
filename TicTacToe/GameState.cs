namespace TicTacToe;

public class GameState
{
    public GameSigns[] GameField { get; set; } = null!;
    public int PointerCoordinateX { get; set; }
    public int PointerCoordinateY { get; set; }
}