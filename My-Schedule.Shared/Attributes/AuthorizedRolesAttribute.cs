using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizedRolesAttribute : Attribute
    {
        public UserRoleType[] Roles { get; }

        /// <summary>
        /// Set which roles are allowed to reach this API endpoint.
        /// Can accept multilbe roles. When null or not specified anyone can access the endpoint.
        /// MasterAdmin has access to everything.
        /// </summary>
        /// <param name="roles"> Must be an array or null.</param>
        public AuthorizedRolesAttribute(params UserRoleType[] roles)
        {
            Roles = roles ?? throw new ArgumentNullException(nameof(roles));
            AddMasterAdminToRoles();
        }

        private void AddMasterAdminToRoles()
        {
            if (!Roles.Contains(UserRoleType.MasterAdmin))
            {
                Roles.Append(UserRoleType.MasterAdmin);
            }
        }
    }
}
