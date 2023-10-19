using Microsoft.AspNetCore.Authorization;
using UserManagement.ServiceDev;

namespace Usermanger.AuthorizationSelf
{
    public class CustomAuthorizationHandler:AuthorizationHandler<CustomAuthorizationRequirement>
    {
        private readonly UserRoleService userroleService;
        private readonly RoleService roleService;

        public CustomAuthorizationHandler(UserRoleService userroleService, RoleService roleService)
        {
            this.userroleService = userroleService;
            this.roleService = roleService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {

            /* System.Security.Claims.ClaimsPrincipal user = context.User;
             var id = user.FindFirst("id").Value;
             Model.Entitys.UserRoleRelation userRoleRelation = userroleService.Find(id);
             Guid roleId = userRoleRelation.RoleId;
             Model.Entitys.Role role = roleService.Find(roleId);
             string? name = role.Name;
             if (name.Equals(requirement.RequiredRole))
             {
                 context.Succeed(requirement); // 授权通过
             }
             else
             {
                 context.Fail(); // 授权失败
             }

             return Task.CompletedTask;*/

            /*foreach (var requiredRole in requirement.RequiredRole)
            {
                if (context.User.IsInRole(requiredRole))
                {
                    context.Succeed(requirement); // 授权通过
                    return Task.CompletedTask;
                }
            }

            context.Fail(); // 授权失败
            return Task.CompletedTask;*/

       
            // 检查 JWT 是否有效
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // 检查 JWT 中是否包含角色信息
            var rolesClaim = context.User.FindFirst("Role");
            if (rolesClaim == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // 检查用户的角色是否满足资源所需的角色
            var userRoles = rolesClaim.Value.Split(',');
            if (userRoles.Any(role => requirement.RequiredRole.Contains(role)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
