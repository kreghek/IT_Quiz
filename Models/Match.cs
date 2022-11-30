using System.Text.Json.Serialization;

namespace IT_Quiz.Models;

public sealed class Match
{
    private IReadOnlyList<Lobby> _lobbies;

    public IReadOnlyCollection<Player> Players => _lobbies.Select(x => new Player(x)).ToArray();

    public Match(string matchId, IReadOnlyList<Lobby> lobbies, string startQuestionId)
    {
        MatchId = matchId;

        _lobbies = lobbies;

        CurrentStage = new Stage(Guid.NewGuid().ToString(), startQuestionId, Players);

        _stages.Add(CurrentStage);

        State = "InProgress";
    }

    public string MatchId { get; }

    [JsonIgnore]
    public string CurrentQuestionId => CurrentStage.QuestionId;

    [JsonIgnore]
    public Stage CurrentStage { get; private set; }

    public string State { get; set; }

    private readonly IList<Stage> _stages = new List<Stage>();

    public IReadOnlyList<Stage> Stages => _stages.ToArray();

    public Task RegisterAnswer(string username, string answerId)
    {
        CurrentStage.RegisterAnswer(CurrentStage.Players.Single(x=>x.Lobby.User == username), answerId);

        return Task.CompletedTask;
    }

    public void CompleteCurrentStage(string? questionId)
    {
        if (questionId is null)
        {
            State = "Finish";
            return;
        }

        CurrentStage = new Stage(Guid.NewGuid().ToString(), questionId, Players);

        _stages.Add(CurrentStage);
    }
}