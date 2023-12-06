namespace My_Schedule.Shared.Helpers.Validators
{
    public class PasswordValidator
    {
        /// <summary>
        /// Validates a password to ensure it meets specified complexity requirements.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <param name="minLength">The minimum length of the password (default is 8).</param>
        /// <param name="minUpperCase">The minimum number of uppercase characters (default is 1).</param>
        /// <param name="minLowerCase">The minimum number of lowercase characters (default is 1).</param>
        /// <param name="minDigits">The minimum number of digits (default is 1).</param>
        /// <param name="minSpecialChars">The minimum number of special characters (default is 1).</param>
        /// <returns>True if the password meets the specified requirements; otherwise, false.</returns>
        public static bool IsValidPassword(string password, int minLength = 8, int maxLength = 255, int minUpperCase = 1, int minLowerCase = 1, int minDigits = 1, int minSpecialChars = 1)
        {
            // Ensure the password meets the minimum or maximum length requirement.
            if (password.Length < minLength || password.Length > maxLength)
            {
                return false;
            }

            // Count the uppercase, lowercase, digits, and special characters in the password.
            int upperCaseCount = password.Count(char.IsUpper);
            int lowerCaseCount = password.Count(char.IsLower);
            int digitCount = password.Count(char.IsDigit);
            int specialCharCount = password.Count(c => !char.IsLetterOrDigit(c));

            // Check if each criteria is met.
            if (upperCaseCount < minUpperCase || lowerCaseCount < minLowerCase || digitCount < minDigits || specialCharCount < minSpecialChars)
            {
                return false;
            }

            return true;
        }
    }
}