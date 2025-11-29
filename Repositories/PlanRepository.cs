using RedacaoAPI.Interfaces;
using RedacaoAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RedacaoAPI.Repositories
{
    public class PlanRepository : BaseRepositoryInt<Plan>, IPlanRepository
    {
        private readonly RedacaoDbContext _context;

        public PlanRepository(RedacaoDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Plan> GetAll()
        {
            return _context.Set<Plan>()
                           .Where(p => p.DeletedAt == null)
                           .ToList();
        }
    }
}
