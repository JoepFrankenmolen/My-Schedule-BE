using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;

namespace My_Schedule.Shared.Services.Users
{
    public interface IUserHelper
    {
        Task<IUserBasic> GetUserBasicById(Guid id);
    }
}