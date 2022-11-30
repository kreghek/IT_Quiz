using IT_Quiz.Models;
using IT_Quiz.Services;
using Microsoft.AspNetCore.Mvc;

namespace IT_Quiz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController : ControllerBase
{
    private readonly MatchService _matchService;
    private readonly QuestionService _questionService;

    public MatchController(MatchService matchService, QuestionService questionService)
    {
        _matchService = matchService;
        _questionService = questionService;
    }

    [HttpPost("register")]
    public IActionResult RegisterLobby(string userName) 
    { 
        _matchService.RegisterLobbyAsync(userName).Wait();

        return Ok();
    }

    [HttpGet("lobby-state")]
    public async Task<ActionResult<Lobby>> GetLobbyState(string username)
    {
        var lobby = await _matchService.GetLobbyStateAsync(username);

        return lobby;
    }

    [HttpGet("match")]
    public async Task<ActionResult<Match>> GetMatch(string id)
    {
        var match = await _matchService.GetMatchAsync(id);

        return match;
    }

    [HttpGet("question")]
    public async Task<ActionResult<Question>> GetQuestion(string id)
    {
        var question = await _questionService.GetQuestionAsync(id);
        return question;
    }

    [HttpPost("answer")]
    public async Task<IActionResult> SendAnswer(string username, string matchId, string answerId)
    {
        await _matchService.ReceiveAnswerAsync(username, matchId, answerId);
        return Ok();
    }
}