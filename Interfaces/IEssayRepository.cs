using RedacaoAPI.Models;

namespace RedacaoAPI.Interfaces
{
    public interface IEssayRepository
    {
        public Essay? GetById(Guid essayId);
        public void Create(Essay essay);
        public void SoftDelete(Guid essayId);
        public List<Essay> GetByUserId(Guid userId);
    }
}
