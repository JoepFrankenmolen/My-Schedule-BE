namespace My_Schedule.Shared.Helpers.Validators
{
    public static class UserValidator
    {
        public static bool IsValidUser(IUserValidationFields user, int maxAttempts, bool mustEmailBeConfirmed = true)
        {
            // If user is null or has too many access failed attempts, or is blocked/banned, return false.
            if (user == null || user.AccessFailedCount >= maxAttempts || user.IsBlocked || user.IsBanned)
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