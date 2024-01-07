using Microsoft.AspNetCore.Antiforgery;

namespace My_Schedule.Shared.Middleware
{
    public class DevelopmentMiddleware : IMiddleware
    {
        private readonly IAntiforgery _antiforgery;
        private readonly string accessToken;

/*        // broken for now
        public DevelopmentMiddleware(IAntiforgery antiforgery, TokenDevelopmentGenerator tokenDevelopmentGenerator)
        {
            _antiforgery = antiforgery ?? throw new ArgumentNullException(nameof(antiforgery));
            accessToken = tokenDevelopmentGenerator.GenerateToken().Result; // .result used bc of async not availbe in constructor.
        }*/

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    Console.WriteLine("filled bearer token");
                    context.Request.Headers["Authorization"] = "Bearer " + accessToken;
                }

                var antiForgeryToken = _antiforgery.GetAndStoreTokens(context);
                if (!context.Request.Headers.ContainsKey(antiForgeryToken.HeaderName))
                {
                    Console.WriteLine("filled csfr token");
                    context.Request.Headers[antiForgeryToken.HeaderName] = antiForgeryToken.RequestToken;
                }
            }
            catch
            {
                context.Response.StatusCode = 404; // bad request
                return;
            }
            await next.Invoke(context);
        }
    }
}