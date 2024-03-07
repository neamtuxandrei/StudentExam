using ExamSupportToolAPI.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ExamSupportToolAPI.Data
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RemoveUser(Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId.ToString());
            if (user != null)
            {
                _dbContext.Users.Remove(user);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
