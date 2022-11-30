namespace IT_Quiz.Models;

public sealed class Player
{
    public Lobby Lobby { get; }

    public Player(Lobby lobby)
    {
        Lobby = lobby;
    }

    public int HP { get; set; } = 10;
}