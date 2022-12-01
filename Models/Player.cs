using System.Text.Json.Serialization;

namespace IT_Quiz.Models;

public sealed class Player
{
    [JsonIgnore]
    public Lobby Lobby { get; }

    public Player(Lobby lobby)
    {
        Lobby = lobby;
    }

    public int Hp { get; set; } = 10;
    public int Dmg => 4;
}