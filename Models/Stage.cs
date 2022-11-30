namespace IT_Quiz.Models;

public sealed class Stage
{
    public string Id { get; }
    public string QuestionId { get; }
    public IReadOnlyCollection<Player> Players { get; }
    private IList<(Player, string answerid)> _answers;

    public IReadOnlyCollection<(Player, string answerid)> Answers => _answers.ToArray();

    public Stage(string Id, string QuestionId, IReadOnlyCollection<Player> players)
    {
        this.Id = Id;
        this.QuestionId = QuestionId;
        Players = players;
    }

    public string State
    {
        get
        {
            var allPlayerAnswered = Players.Select(x => new { x, AnswerDone = _answers.Any(a => a.Item1 == x) })
                .All(x => x.AnswerDone);
            if (allPlayerAnswered)
            {
                return "Complete";
            }
            else
            {
                return "WaitForAnswers";
            }
        }
    }

    public void RegisterAnswer(Player player, string answerId)
    {
        _answers.Add((player, answerId));
    }
}