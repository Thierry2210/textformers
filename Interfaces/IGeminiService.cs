namespace RedacaoAPI.Interfaces
{
    public interface IGeminiService
    {
        Task<(string correctedContent, int score, string feedback)> CorrigirRedacaoAsync(string title, string content);
    }
}