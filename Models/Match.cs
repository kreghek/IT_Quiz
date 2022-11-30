namespace IT_Quiz.Models;

public sealed class Match
{
    private IReadOnlyList<Lobby> _lobbies;
    
    private IReadOnlyCollection<Stage>

    public Match(string matchId, IReadOnlyList<Lobby> lobbies, string startQuestionId)
    {
        MatchId = matchId;

        _lobbies = lobbies;

        CurrentQuestionId = startQuestionId;
    }

    public string MatchId { get; }

    public string CurrentQuestionId { get; }
}