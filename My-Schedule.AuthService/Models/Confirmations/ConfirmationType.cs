namespace My_Schedule.AuthService.Models.Confirmations
{
    public enum ConfirmationType
    {
        EmailConfirmation = 0,
        LoginVerification = 1,
        PasswordReset = 2,
        UnBlock = 3,
    }
}