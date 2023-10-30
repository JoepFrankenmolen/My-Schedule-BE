namespace My_Schedule.Shared.RabbitMQ
{
    public static class QueueNames
    {
        public static class Users
        {
            public const string UserBlocked = "user_blocked";
            public const string UserBanned = "user_banned";
            public const string UserTokenRevocation = "user_token_revocation";
            public const string UserEmailConfirmation = "user_email_confirmation";
            public const string UserIdentityUpdate = "user_identity_update";
            public const string UserRoleUpdate = "user_role_update";
            public const string UserCreated = "user_created";
        }

        public static class UserAuthDetails
        {
            public const string TwoFactorEnabledUpdate = "two_factor_enabled_update";
            public const string SuccessfullLogin = "successfull_login";
            public const string FailedLoginAttempt = "failed_login_attempt";
            public const string UserAuthDetailCreated = "user_auth_detail_created";
        }

        public static class Tokens
        {
            public const string TokenStatusCreated = "token_status_created";
        }
    }
}