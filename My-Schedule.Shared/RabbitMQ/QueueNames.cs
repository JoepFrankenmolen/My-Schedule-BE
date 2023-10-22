namespace My_Schedule.Shared.RabbitMQ
{
    public static class QueueNames
    {
        public static class Users
        {
            public const string UserBlocked = "user_blocked";
            public const string UserBanned = "user_banned";
        }

        public static class Tokens
        {
            public const string TokenStatusCreated = "token_status_created";
        }
    }
}
