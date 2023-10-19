using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using Model.Result;
using Service;
using UserManagement.ServiceDev;

namespace Usermanger.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly GroupService groupService;
        private readonly UserService userService;
        private readonly UserGroupService userGroupService;

        public GroupController(GroupService groupService, UserService userService, UserGroupService userGroupService)
        {
            this.groupService = groupService;
            this.userService = userService;
            this.userGroupService = userGroupService;
        }

        [HttpPut]
        public ResultApi AddGroup(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ResultHelper.Error("请输入岗位名称");
            }
            Group group = new Group();
            group.Name = name;
            int v = groupService.Create(group);
            return ResultHelper.Success(v);

        }
        [HttpPost]
        public ResultApi AddUserGroup(string userName,String groupName) 
        {
            if (string.IsNullOrEmpty(userName)&& string.IsNullOrEmpty(groupName))
            {
                return ResultHelper.Error("请检查输入");
            }
            User user = userService.Find(userName);
            Group group = groupService.Find(groupName);
            UserGroupRelation relation = new UserGroupRelation();
            relation.UserId = user.Id;
            relation.GroupId = group.Id;
            int v = userGroupService.Create(relation);
            return ResultHelper.Success(v);
        }
       
        
    }
}
