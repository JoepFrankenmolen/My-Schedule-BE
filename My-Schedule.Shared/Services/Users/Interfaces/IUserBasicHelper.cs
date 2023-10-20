using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserBasicHelper
    {
        Task<IUserBasic> GetUserBasicById(Guid id);
    }
}