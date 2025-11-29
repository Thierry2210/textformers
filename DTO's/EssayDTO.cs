namespace RedacaoAPI.DTOs
{
    public class EssayDTO
    {
        public class EssayCreateDTO
        {
            public string Title { get; set; }
            public string Content { get; set; }
        }

        public class EssayResponseDTO
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string CorrectedContent { get; set; }
            public int Score { get; set; }
            public string Feedback { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class queryDTO
        {
            public Guid UserId { get; set; }
            public Guid EssayId { get; set; }
        }
    }
}
