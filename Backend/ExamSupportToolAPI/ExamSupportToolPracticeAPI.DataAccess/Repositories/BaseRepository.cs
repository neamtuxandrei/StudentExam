using ExamSupportToolAPI.Abstractions;
using ExamSupportToolAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamSupportToolAPI.DataAccess.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : User
    {
        private readonly ExaminationSessionDbContext _dbContext;
        internal DbSet<T> dbSet;
        public BaseRepository(ExaminationSessionDbContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = _dbContext.Set<T>();
        }
        public void Add(T toAdd)
        {
            dbSet.Add(toAdd);
        }

        public void AddRange(ICollection<T> toAdd)
        {
            dbSet.AddRange(toAdd);
        }
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public T GetById(Guid id)
        {
            return dbSet.First(i => i.Id == id);
        }

        public T? GetByIdFirstOrDefault(Guid id)
        {
            return dbSet.FirstOrDefault(i => i.Id == id);
        }

        public async Task<T> GetByEmail(string email)
        {
            return await dbSet.FirstAsync(i => i.Email == email);
        }

        public async Task<T?> GetByEmailFirstOrDefault(string email)
        {
            return await dbSet.FirstOrDefaultAsync(i => i.Email == email);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
