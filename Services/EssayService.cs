using RedacaoAPI.Interfaces;
using RedacaoAPI.Models;
using RedacaoAPI.Repositories;
using static RedacaoAPI.DTOs.EssayDTO;

namespace RedacaoAPI.Services
{
    public class EssayService : IEssayService
    {
        private readonly IEssayRepository _essayRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGeminiService _geminiService;

        public EssayService(IEssayRepository essayRepository, IGeminiService geminiService, IUserRepository userRepository)
        {
            _essayRepository = essayRepository;
            _userRepository = userRepository;
            _geminiService = geminiService;
        }

        public async Task<EssayResponseDTO> CreateEssayAsync(Guid userId, EssayCreateDTO essayDTO)
        {

            if (string.IsNullOrWhiteSpace(essayDTO.Content))
                throw new Exception("O conteúdo da redação não pode ser vazio.");

            User? user = _userRepository.GetByIdWithPlan(userId);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            ValidateMonthlyCount(user);

            var correction = await _geminiService.CorrigirRedacaoAsync(essayDTO.Title, essayDTO.Content);

            Essay essay = new Essay
            {
                UserId = userId,
                Title = essayDTO.Title,
                Content = essayDTO.Content,
                CorrectedContent = correction.correctedContent,
                Score = correction.score,
                Feedback = correction.feedback
            };

            EssayResponseDTO response = new EssayResponseDTO
            {
                Id = essay.Id,
                Title = essayDTO.Title,
                Content = essayDTO.Content,
                CorrectedContent = correction.correctedContent,
                Score = correction.score,
                Feedback = correction.feedback,
                CreatedAt = essay.CreatedAt
            };

            _essayRepository.Create(essay);
            _userRepository.Update(user);

            return response;
        }

        public List<Essay> GetEssaysByUser(Guid userId)
        {
            return _essayRepository.GetByUserId(userId);
        }

        public Essay GetEssayById(Guid essayId, Guid userId)
        {
            Essay? essay = _essayRepository.GetById(essayId);

            if (essay == null || essay.UserId != userId)
                throw new Exception("Redação não encontrada.");

            return essay;
        }

        public void SoftDelete(Guid essayId, Guid userId)
        {
            Essay? essay = _essayRepository.GetById(essayId);

            if (essay == null || essay.UserId != userId)
                throw new Exception("Redação não encontrada ou não pertence ao usuário.");

            if (essay.DeletedAt != null)
                throw new Exception("Essa redação já foi excluída.");

            _essayRepository.SoftDelete(essayId);
        }

        private void ValidateMonthlyCount(User user)
        {
            ResetIfNeeded(user);

            if (user.Plan == null)
                throw new Exception("Plano do usuário não encontrado.");

            if (user.MonthlyEssayCount >= user.Plan.MaxCorrections)
                throw new Exception("Limite mensal de redações atingido.");

            user.MonthlyEssayCount++;
        }

        private void ResetIfNeeded(User user)
        {
            var now = DateTime.UtcNow.AddHours(-3);

            bool newMonth = user.LastEssayReset.Month != now.Month || user.LastEssayReset.Year != now.Year;

            if (newMonth)
            {
                user.MonthlyEssayCount = 0;
                user.LastEssayReset = now;
            }
        }
    }
}
