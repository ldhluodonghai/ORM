using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Entitys.Dto;
using Model.Result;
using UserManagement.ServiceDev;

namespace Usermanger.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles ="罗")]
    public class PostController : ControllerBase
    {
        private readonly PostService postService;
        private readonly RolePostService rolePostService;
        private readonly RoleService roleService;

        public PostController(PostService postService,RolePostService rolePostService,RoleService roleService)
        {
            this.postService = postService;
            this.rolePostService = rolePostService;
            this.roleService = roleService;
        }

        [HttpPost]
        public ResultApi AddPost(PostDto postDto)
        {
            Post post = new Post(Guid.NewGuid()) ;
            post.Name = postDto.Name;
            post.PostType = postDto.PostType;
            post.ParentId = postDto.ParentId;
            postDto.Url = postDto.Url;

            int v = postService.Create(post);
            if (v > 0)
            {
                return ResultHelper.Success(v);
            }
            else
            {
                return ResultHelper.Error("添加失败");
            }
            
        }
        //角色绑定资源模块
        [HttpPost]
        public ResultApi AddRolePost(string roleName, string[] PostName)
        {
            Role role = roleService.Find(roleName);
            int length = PostName.Length;
            int i = 0;
            foreach (var post in PostName)
            {
                var postRelation = postService.Find(post);
                RolePostRelation relation = new RolePostRelation(Guid.NewGuid());
                relation.RoleId = role.Id;
                relation.PostId = postRelation.Id;
                int v = rolePostService.Create(relation);
                i++;
            }
            if (length != i)
            {
                return ResultHelper.Error("授权失败");
            }
            return ResultHelper.Success(i);    
        }


    }
}
