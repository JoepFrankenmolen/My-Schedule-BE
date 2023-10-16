using Microsoft.Extensions.Primitives;
using My_Schedule.Shared.Models.Helpers;

namespace My_Schedule.Shared.Helpers
{
    public static class HttpContextHelper
    {
        /// <summary>
        /// Retrieves the bearer token from a httpRequest
        /// </summary>
        /// <param name="request"> The httpRequest a bearer token must be retrieved from. </param>
        /// <returns> A string with the bearer token already processed. Retuns null if no bearer token found. </returns>
        public static string GetBearerToken(HttpRequest request)
        {
            string authorizationHeader = request.Headers["Authorization"];
            if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }
            return null;
        }

        public static HttpContextDetailsDTO GetContextDetails(HttpContext context)
        {
            var request = context.Request;

            return new HttpContextDetailsDTO
            {
                IPAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = request.Headers["User-Agent"].ToString()
            };
        }
    }
}