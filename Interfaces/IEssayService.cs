using RedacaoAPI.Models;
using static RedacaoAPI.DTOs.EssayDTO;

namespace RedacaoAPI.Interfaces
{
    public interface IEssayService
    {
        public Task<EssayResponseDTO> CreateEssayAsync(Guid userId, EssayCreateDTO essayDTO);
        public List<Essay> GetEssaysByUser(Guid userId);
        public Essay GetEssayById(Guid essayId, Guid userId);
        public void SoftDelete(Guid essayId, Guid userId);
    }
}