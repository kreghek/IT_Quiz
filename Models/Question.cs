namespace IT_Quiz.Models;

public sealed record Question(string Id, string Text, IReadOnlyList<Answer> Answers);