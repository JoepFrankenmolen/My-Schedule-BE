using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;

namespace My_Schedule.Shared.Services.Users.Users
{
    public static class UserCheckService
    {
        public static async Task<bool> CheckIfEmailExists(string email, IUserContext context)
        {
            return await context.Users.AnyAsync(n => n.Email == email);
        }
    }
}