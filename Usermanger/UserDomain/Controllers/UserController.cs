using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.Entitys;
using Model.Entitys.Dto;
using Model.Entitys.Model;
using Model.@enum;
using Model.Result;
using Service;
using System.Security.Claims;
using UserManagement.ServiceDev;
using UserManagement.ServiceDev.Jwt;
using Usermanger.AuthorizationSelf;

namespace Usermanger.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        private readonly ICustomJWTService customJWTService;
        private readonly UserRoleService userRoleService;
        private readonly RoleService roleService;
        private readonly CustomAuthorizationRequirement requirement;

        public UserController(UserService userService, ICustomJWTService customJWTService, 
            UserRoleService userRoleService, RoleService roleService)
        {
            this.userService = userService;
            this.customJWTService = customJWTService;
            this.userRoleService = userRoleService;
            this.roleService = roleService;
            
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultApi> Login(string username, string pwd)
        {
            User user =await userService.FindAsync(username);
           
            user.Password = pwd;
            if(user == null|| user.Password != pwd)
            {
                return  ResultHelper.Error("用户名或密码错误");
            }
            else
            {
                user.JWTVersion++;
                userService.EditByEntity(user);
                return ResultHelper.Success(customJWTService.GetJwtToken(user));
            }
        }
        /// <summary>
        /// 
        /// 注册
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultApi Signup(UserDto userDto)
        {
            User user = new User();
            if (userDto.PhoneNumber != null && userDto.Name!= null && userDto.Password != null)
            {               
                user.Name=userDto.Name; user.Password=userDto.Password;user.PhoneNumber = userDto.PhoneNumber;              
            }

            Console.WriteLine(user);
            int v = userService.Create(user);
            if (v >= 0)
            {
                return ResultHelper.Success(v);
            }
            else
            {
                return ResultHelper.Error("注册失败");
            }
        } 

        [HttpPut]
        [Authorize(Roles ="罗")]
        public ResultApi AddUserRole(string[] role )
        {
            string userId = this.User.FindFirst("id")!.Value;
            int i = 0;
            foreach (string roleName in role)
            {                
                Role enumRole = roleService.Find(roleName);
                if (enumRole != null)
                {
                    UserRoleRelation userRoleRelation = new UserRoleRelation();
                    userRoleRelation.UserId =new Guid(userId);
                    userRoleRelation.RoleId = enumRole.Id;
                    int ur = userRoleService.Create(userRoleRelation);
                    i += ur;
                }
                else
                {
                    return ResultHelper.Error("添加失败");
                }
            }                                                     
            return ResultHelper.Success(i);               
        }
        [HttpPut]
        [Authorize]
        public ResultApi Edit(UserDto userDto)
        {

            User user = userService.Find(userDto.Name);
            user.Password = userDto.Password;
            user.PhoneNumber = userDto.PhoneNumber;
            int v = userService.EditByEntity(user);
            if (v > 0)
            {
                return ResultHelper.Success(v);
            }
            else
            {
                return ResultHelper.Error("修改失败");
            }
            
        }
        [HttpDelete]
        [Authorize(Roles ="书记")]
        public ResultApi DeleteRole(string roleName) 
        {
            Role role = roleService.Find(roleName);
            UserRoleRelation userRoleRelation = userRoleService.FindAny(role.Id);
            int v = userRoleService.Delete(userRoleRelation.Id);
            if (v > 0)
            {
                return ResultHelper.Success(v);
            }
            else
            {
                return ResultHelper.Error("删除失败");
            }
        }
        /// <summary>
        /// 给其他人添加角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
   
        public ResultApi AddOtherRole(string name,string[] role)
        {
            if (name.IsNullOrEmpty() || role.IsNullOrEmpty())
            {
                return ResultHelper.Error("请输入值");
            }
            User user = userService.Find(name);          
            foreach(string roleName in role)
            {
                UserRoleRelation userRole = new UserRoleRelation();
                userRole.UserId = user.Id;
                Role role1 = roleService.Find(roleName);
                userRole.RoleId= role1.Id;
            }
            UserModel model = new UserModel();
            model.Name = name;
            model.Phones = user.PhoneNumber;
            model.Role = role;                 
            return ResultHelper.Success(model);           
        }
        //查看当前用户角色有用那些资源
        [HttpPost]
        public ResultApi GetAllPost(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                //string v = this.User.FindFirstValue("id");
                var value = this.User.FindFirst("id");
                Guid guid = new Guid(value!.Value);
                IList<Post> posts = userService.GetPosts(guid);
                return ResultHelper.Success(posts);
            }
            else
            {
                User user = userService.Find(name);
                IList<Post> posts = userService.GetPosts(user.Id);
                return ResultHelper.Success(posts);
            }
        }
        //撤销用户登录状态
        [Authorize(Roles ="系统管理员")]
        [HttpPut]
        public ResultApi DismissUser(string name) 
        {
            User user = userService.Find(name);
            user.JWTVersion++;
            int v = userService.EditByEntity(user);
            if (v > 0)
            {

                return ResultHelper.Success(v);
            }
            else
            {
                return ResultHelper.Error("用户撤销失败");
            }

        }

    }
}
