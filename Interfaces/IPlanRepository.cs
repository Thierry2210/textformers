using RedacaoAPI.Models;

namespace RedacaoAPI.Interfaces
{
    public interface IPlanRepository
    {
        public Plan? GetById(int id);
        public void Create(Plan plan);
        public void Update(Plan plan);
        public void SoftDelete(int id);
    }
}