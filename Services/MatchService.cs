using IT_Quiz.Models;

namespace IT_Quiz.Services;

public sealed class QuestionService
{
    private readonly IReadOnlyList<Question> _questions;
    private readonly Random _rnd = new Random();

    public QuestionService()
    {
        _questions = new []
        {
         new Question("q1", "q1", new []{new Answer("a1", "a1", true), new Answer("a2", "a2", false), new Answer("a3", "a3", false), new Answer("a4", "a4", false)}),
         new Question("q2", "q2", new []{new Answer("a1", "a1", false), new Answer("a2", "a2", true), new Answer("a3", "a3", false), new Answer("a4", "a4", false)}),
         new Question("q3", "q3", new []{new Answer("a1", "a1", false), new Answer("a2", "a2", false), new Answer("a3", "a3", true), new Answer("a4", "a4", false)}),
         new Question("q4", "q4", new []{new Answer("a1", "a1", false), new Answer("a2", "a2", false), new Answer("a3", "a3", false), new Answer("a4", "a4", true)}),
        };
    }
    
    public Task<Question> RollQuestionAsync()
    {
        var index = _rnd.Next(0, _questions.Count);
        return Task.FromResult(_questions[index]);
    }

    public Task<Question> GetQuestionAsync(string id)
    {
        return Task.FromResult(_questions.Single(x => x.Id == id));
    }
}

public sealed class MatchService
{
    private readonly QuestionService _questionService;
    private readonly IList<Lobby> _lobbies;
    private readonly IList<Match> _matches;

    public MatchService(QuestionService questionService)
    {
        _questionService = questionService;
        _lobbies = new List<Lobby>();
        _matches = new List<Match>();
    }

    public async Task RegisterLobbyAsync(string userName)
    {
        var lobby = new Lobby(userName);

        _lobbies.Add(lobby);

        Console.WriteLine("lobby added");

        var otherLobbies = _lobbies.Where(x=>x.User != userName);
        if (otherLobbies.Any())
        {
            var enemyLobby = otherLobbies.First();

            var matchId = Guid.NewGuid().ToString();
            var startQuestion = await _questionService.RollQuestionAsync();
            
            _matches.Add(new Match(matchId, new[]{ lobby, enemyLobby }, startQuestion.Id));
            lobby.MatchId = matchId;
            enemyLobby.MatchId=matchId;
        }
    }

    public Task<Lobby> GetLobbyStateAsync(string userName)
    {
        Console.WriteLine($"{_lobbies.Count} lobbies are registered.");

        foreach(var item in _lobbies)
        {
            Console.WriteLine($"User: {item.User}");
        }

        Console.WriteLine($"Lobby with {userName} requested");

        var lobby = _lobbies.SingleOrDefault(x=>x.User == userName);

        if (lobby is null)
        {
            throw new InvalidOperationException();   
        }

        return Task.FromResult(lobby);
    }

    public Task<Match> GetMatchAsync(string matchId)
    {
        var match = _matches.SingleOrDefault(x=>x.MatchId == matchId);

        if (match is null)
        {
            throw new InvalidOperationException();   
        }

        return Task.FromResult(match);
    }

    public async Task ReceiveAnswerAsync(string username, string matchId, string answerId)
    {
        var lobby = _lobbies.Single(x => x.User == username);
        var match = _matches.Single(x => x.MatchId == lobby.MatchId);

        await match.RegisterAnswer(username, answerId);

        var currentStage = match.CurrentStage;

        if (currentStage.State == "Complete")
        {
            await CalcDamage(match, currentStage);

            var deadPlayer = match.CurrentStage.Players.SingleOrDefault(x => x.HP <= 0);

            if (deadPlayer is null)
            {
                var nextQuestion = await _questionService.RollQuestionAsync();
                match.CompleteCurrentStage(nextQuestion.Id);
            }
            else
            {
                match.CompleteCurrentStage(null);
            }
        }
    }

    private async Task CalcDamage(Match match, Stage currentStage)
    {
        var question = await _questionService.GetQuestionAsync(currentStage.QuestionId);

        var answerCorrectness = match.Players.Select(x => new 
        { 
            Player = x,
            IsCorrent = question.Answers.Single(ans => ans.Id == currentStage.Answers.Single(a => a.Item1 == x).answerid).IsCorrect }
        );

        if (answerCorrectness.All(x => x.IsCorrent) || answerCorrectness.All(x => !x.IsCorrent))
        {
            // Do nothing
        }
        else
        {
            var attackerPlayer = answerCorrectness.Single(x => x.IsCorrent);
            var damagedPlayer = answerCorrectness.Single(x => !x.IsCorrent);
            damagedPlayer.Player.HP-= attackerPlayer.Player.Dmg;
        }
    }
}