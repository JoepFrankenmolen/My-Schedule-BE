using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users
{
    public class UserBasicHelper : IUserBasicHelper
    {
        private readonly IUserBasicContext _dbContext;

        public UserBasicHelper(IUserBasicContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IUserBasic> GetUserBasicById(Guid id)
        {
            return await _dbContext.UserBasics
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}
