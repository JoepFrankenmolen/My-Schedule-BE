using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;

namespace My_Schedule.Shared.Services.Authorization.Interfaces
{
    public interface IAuthorizationService
    {
        Task<IUser> AuthorizeRequest(HttpRequest request);
    }
}