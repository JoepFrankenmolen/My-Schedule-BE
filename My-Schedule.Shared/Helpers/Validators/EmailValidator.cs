namespace My_Schedule.Shared.Helpers.Validators
{
    public class EmailValidator
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var emailAddress = new System.Net.Mail.MailAddress(email);
                return emailAddress.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}