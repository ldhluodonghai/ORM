using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Result;
using UserManagement.ServiceDev;

namespace Usermanger.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class RoleController : ControllerBase
    {
        private readonly RoleService roleService;
        private readonly RolePostService rolePostService;
        private readonly PostService postService; 

        public RoleController(RoleService roleService, RolePostService rolePostService, PostService postService)
        {
            this.roleService = roleService;
            this.rolePostService = rolePostService;
            this.postService = postService;
        }
        [HttpPost]
        [Authorize(Roles="罗")]
        public ResultApi AddRole(string roleName)
        {
            Role role = new Role() { Name = roleName };
            int v = roleService.Create(role);
            if(v >= 0)
            {
                return ResultHelper.Success(v);
            }
            else
            {
                return ResultHelper.Error("添加失败");
            }
        }
        [HttpPost]
        [Authorize(Roles = "罗")]
        public ResultApi AddRolePost(string roleName,string postName) 
        {
            Role role = roleService.Find(roleName);
            Post post = postService.Find(postName);
            RolePostRelation relation = new RolePostRelation();
            relation.RoleId = role.Id;
            relation.PostId = post.Id;
            return ResultHelper.Success(relation);
        }

    }
}
