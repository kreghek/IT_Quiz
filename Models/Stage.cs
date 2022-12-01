using System.Text.Json.Serialization;

namespace IT_Quiz.Models;

public sealed record PlayerAnswer(Player Player, string AnswerId);

public sealed class Stage
{
    public string Id { get; }
    public string QuestionId { get; }

    [JsonIgnore]
    public IReadOnlyCollection<Player> Players { get; }

    private IList<PlayerAnswer> _answers;

    [JsonIgnore]
    public IReadOnlyCollection<PlayerAnswer> Answers => _answers.ToArray();

    public Stage(string Id, string QuestionId, IReadOnlyCollection<Player> players)
    {
        this.Id = Id;
        this.QuestionId = QuestionId;
        Players = players;

        _answers = new List<PlayerAnswer>();
    }

    public string State
    {
        get
        {
            var allPlayerAnswered = Players
                .Select(x => new { x, AnswerDone = _answers.Any(a => a.Player == x) })
                .All(x => x.AnswerDone);
            
            return allPlayerAnswered ? "Complete" : "WaitForAnswers";
        }
    }

    public void RegisterAnswer(Player player, string answerId)
    {
        _answers.Add(new PlayerAnswer(player, answerId));
    }
}