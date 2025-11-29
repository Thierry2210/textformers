using RedacaoAPI.Interfaces;
using RedacaoAPI.Models;

namespace RedacaoAPI.Repositories
{
    public class PlanRepository : BaseRepositoryInt<Plan>, IPlanRepository
    {
        public PlanRepository(RedacaoDbContext context) : base(context) { }
    }
}
