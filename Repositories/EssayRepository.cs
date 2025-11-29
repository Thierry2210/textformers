using RedacaoAPI.Interfaces;
using RedacaoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RedacaoAPI.Repositories
{
    public class EssayRepository : BaseRepositoryGuid<Essay>, IEssayRepository
    {

    public EssayRepository(RedacaoDbContext context) : base(context){}

        public List<Essay> GetByUserId(Guid userId)
        {
            return _context.Essays
                .Where(essay => essay.UserId == userId && essay.DeletedAt == null)
                .OrderByDescending(essay => essay.CreatedAt)
                .ToList();
        }
    }
}
