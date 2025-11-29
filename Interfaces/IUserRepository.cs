using RedacaoAPI.Models;

namespace RedacaoAPI.Interfaces
{
    public interface IUserRepository
    {
        public User? GetByEmail(string email);
        public User? GetByIdWithPlan(Guid id);
        public User? GetById(Guid id);
        public void Create(User user);
        public void Update(User user);
        public void SoftDelete(Guid id);
    }
}
