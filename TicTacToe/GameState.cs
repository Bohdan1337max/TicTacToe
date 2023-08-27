using System.Text.Json.Serialization;

namespace TicTacToe;

public class GameState
{
    public GameSigns[] GameField { get; set; } = null!;
    public GameSigns Winner { get; set; }
    public bool IsGameEnd { get; set; }
    public bool CanPlayerMakeTurn { get; set; }
    
}