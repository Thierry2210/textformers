using RedacaoAPI.Models;
using System.Collections.Generic;

namespace RedacaoAPI.Interfaces
{
    public interface IPlanRepository
    {
        public Plan? GetById(int id);
        public IEnumerable<Plan> GetAll();
        public void Create(Plan plan);
        public void Update(Plan plan);
        public void SoftDelete(int id);
    }
}
