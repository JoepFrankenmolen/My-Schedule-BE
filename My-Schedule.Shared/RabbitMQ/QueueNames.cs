namespace My_Schedule.Shared.RabbitMQ
{
    public static class QueueNames
    {
        public static class Users
        {
            public const string UserBlocked = "user.blocked";
            public const string UserBanned = "user.banned";
            public const string UserTokenRevocation = "user.token_revocation";
            public const string UserEmailConfirmation = "user.email_confirmation";
            public const string UserIdentityUpdate = "user.identity_update";
            public const string UserRoleUpdate = "user.role_update";
            public const string UserCreated = "user.created";
        }

        public static class UserAuthDetails
        {
            public const string TwoFactorEnabledUpdate = "userAuthDetail.two_factor_enabled_update";
            public const string SuccessfullLogin = "userAuthDetail.successfull_login";
            public const string FailedLoginAttempt = "userAuthDetail.failed_login_attempt";
            public const string UserAuthDetailCreated = "userAuthDetail.created";
        }

        public static class Tokens
        {
            public const string TokenStatusCreated = "token_status_created";
        }
    }
}