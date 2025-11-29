using Microsoft.EntityFrameworkCore;
using RedacaoAPI.Models;

namespace RedacaoAPI.Repositories
{
    public class BaseTimeRepository<T> where T : BaseTimeEntity
    {
        protected readonly RedacaoDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseTimeRepository(RedacaoDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
    }

    public class BaseRepositoryGuid<T> : BaseTimeRepository<T> where T : BaseEntityGuid
    {
        public BaseRepositoryGuid(RedacaoDbContext context) : base(context) { }

        public T? GetById(Guid id)
        {
            return _dbSet.FirstOrDefault(entity => entity.Id == id && entity.DeletedAt == null);
        }

        public void SoftDelete(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.DeletedAt = DateTime.UtcNow.AddHours(-3);
                Update(entity);
            }
        }
    }

    public class BaseRepositoryInt<T> : BaseTimeRepository<T> where T : BaseEntityInt
    {
        public BaseRepositoryInt(RedacaoDbContext context) : base(context) { }

        public T? GetById(int id)
        {
            return _dbSet.FirstOrDefault(entity => entity.Id == id && entity.DeletedAt == null);
        }

        public void SoftDelete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.DeletedAt = DateTime.UtcNow.AddHours(-3);
                Update(entity);
            }
        }
    }
}
