namespace IT_Quiz.Models;

public sealed record Lobby(string User)
{
    public string? MatchId{get; set;}
};