using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Models.Users.UserInterfaces;

namespace My_Schedule.Shared.Interfaces.Context
{
    public interface IUserSettingsContext : IContextBase, IUserContext
    {
        DbSet<IUserSettings> UserSettings { get; set; }
    }
}