﻿namespace My_Schedule.Shared.Services.Tokens.Interfaces
{
    public interface ITokenSessionValidator
    {
        Task<bool> isValidSession(Guid sessionId);
    }
}