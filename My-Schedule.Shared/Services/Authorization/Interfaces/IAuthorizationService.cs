using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Authorization.Interfaces
{
    public interface IAuthorizationService
    {
        Task<IUser> AuthorizeRequest(HttpRequest request);
    }
}