using System.Text.Json.Serialization;

namespace IT_Quiz.Models;

public sealed class Match
{
    private IReadOnlyList<Lobby> _lobbies;

    private Stage _currentStage;

    public Match(string matchId, IReadOnlyList<Lobby> lobbies, string startQuestionId)
    {
        MatchId = matchId;

        _lobbies = lobbies;

        _currentStage = new Stage(Guid.NewGuid().ToString(), startQuestionId,
            lobbies.Select(x => new Player(x)).ToArray());

        State = "InProgress";
    }

    public string MatchId { get; }

    public string CurrentQuestionId => _currentStage.QuestionId;

    [JsonIgnore]
    public Stage CurrentStage { get; private set; }

    public string State { get; set; }

    public Task RegisterAnswer(string username, string answerId)
    {
        _currentStage.RegisterAnswer(_currentStage.Players.Single(x=>x.Lobby.User == username), answerId);

        return Task.CompletedTask;
    }

    public void CompleteCurrentStage(string? questionId)
    {
        if (questionId is null)
        {
            State = "Finish";
            return;
        }

        CurrentStage = new Stage(Guid.NewGuid().ToString(), questionId,
            _lobbies.Select(x => new Player(x)).ToArray());
    }
}