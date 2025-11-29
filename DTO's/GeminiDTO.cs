namespace RedacaoAPI.DTO_s
{
    public class GeminiDTO
    {
        public class GeminiResponseDTO
        {
            public int Score { get; set; }
            public List<string>? Feedback { get; set; }
            public string? CorrectedContent { get; set; }
        }
    }
}
