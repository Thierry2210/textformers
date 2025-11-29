using Microsoft.EntityFrameworkCore;
using RedacaoAPI.Interfaces;
using RedacaoAPI.Models;

namespace RedacaoAPI.Repositories
{
    public class UserRepository : BaseRepositoryGuid<User>, IUserRepository
    {

        public UserRepository(RedacaoDbContext context): base(context)
        {
        }

        public User? GetByEmail(string email)
        {
            return _context.Users
                .Include(user => user.Plan)
                .FirstOrDefault(user => user.Email == email && user.DeletedAt == null);
        }

        public User? GetByIdWithPlan(Guid id)
        {
            return _context.Users
                .Include(user => user.Plan)
                .FirstOrDefault(user => user.Id == id && user.DeletedAt == null);
        }
    }
}
