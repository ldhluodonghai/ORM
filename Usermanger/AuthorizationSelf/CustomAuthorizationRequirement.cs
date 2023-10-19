using Microsoft.AspNetCore.Authorization;

namespace Usermanger.AuthorizationSelf
{
    public class CustomAuthorizationRequirement : IAuthorizationRequirement
    {
        public List<string> RequiredRole { get; private set; }

        public CustomAuthorizationRequirement(List<string> requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}
