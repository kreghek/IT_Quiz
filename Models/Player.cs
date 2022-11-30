namespace IT_Quiz.Models;

public sealed class Player
{
    private readonly Lobby _lobby;

    public Player(Lobby lobby)
    {
        _lobby = lobby;
    }

    public int HP { get; set; } = 10;
}