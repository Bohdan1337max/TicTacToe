namespace ServerAP;

public class ServerGame
{
    public static GameState GameState { get; set; }
    public static List<Player> Players = new();
    public static GameSigns CurrentTurnSign { get; set; } = GameSigns.X;
}