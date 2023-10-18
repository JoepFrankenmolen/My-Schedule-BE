using My_Schedule.Shared.Models.Users.UserInterfaces;

namespace My_Schedule.Shared.Helpers.Validators
{
    public static class UserValidator
    {
        public static bool IsValidUser(IUserStatus user, bool mustEmailBeConfirmed = true)
        {
            // If user is null or has too many access failed attempts, or is blocked/banned, return false.
            if (user == null || user.IsBlocked || user.IsBanned)
            {
                return false;
            }

            // If mustEmailBeConfirmed is true and email is not confirmed, return false.
            if (mustEmailBeConfirmed && !user.IsEmailConfirmed)
            {
                return false;
            }

            // If none of the above conditions are met, the user is considered valid.
            return true;
        }
    }
}