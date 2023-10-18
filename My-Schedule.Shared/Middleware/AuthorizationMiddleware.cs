using My_Schedule.Shared.Attributes;
using My_Schedule.Shared.Interfaces;
using My_Schedule.Shared.Models.Users.UserInterfaces;
using My_Schedule.Shared.Services.Authorization.Interfaces;

namespace My_Schedule.Shared.Middleware
{
    public class AuthorizationMiddleware : IMiddleware
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public AuthorizationMiddleware(IAuthorizationService authorizationService, IUserAuthenticationContext userAuthenticationContext)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _userAuthenticationContext = userAuthenticationContext ?? throw new ArgumentNullException(nameof(userAuthenticationContext));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var user = await _authorizationService.AuthorizeRequest(context.Request);

                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                _userAuthenticationContext.Install(context, user);

                if (!CheckRolesAuthorization(context, user))
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }
            }
            catch
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }
            await next.Invoke(context);
        }

        private bool CheckRolesAuthorization(HttpContext context, IUserRoles user)
        {
            var rolesRequiredAttribute = GetAuthorizedRolesAttribute(context);
            if (rolesRequiredAttribute == null)
                return true; // No AuthorizedRolesAttribute found which means no authorization is needed.

            // Get the required roles from the attribute
            var requiredRoles = rolesRequiredAttribute.Roles;

            // Check if the user has any of the required roles
            bool hasRequiredRole = user.Roles.Any(role => requiredRoles.Contains(role.Role));

            return hasRequiredRole;
        }

        private AuthorizedRolesAttribute? GetAuthorizedRolesAttribute(HttpContext context)
        {
            // Get the endpoint information
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            // Check if the endpoint has the RolesRequiredAttribute
            return endpoint.Metadata.GetMetadata<AuthorizedRolesAttribute>();
        }
    }
}