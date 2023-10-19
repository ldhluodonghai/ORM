using Dao.IDao;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Model.Entitys;
using MySql.Data.MySqlClient;
using Service;
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Unicode;
using UserManagement.Db;
using UserManagement.ServiceDev;

namespace UserManagement.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly RoleService roleService;
        private readonly UserRoleService userRoleService;
        private readonly UserService userService;
       
        //SqlHelper.Query("")
        public TestController(RoleService roleService, UserRoleService userRoleService, UserService userService)
        {
            this.roleService = roleService;
            this.userRoleService = userRoleService;
            this.userService = userService;
        }
        [HttpGet]
        public void Get()
        {
            /*var role = new Role("书记");
            var v = sqlHelper.Query("select * from t_user",role);
            Console.WriteLine(v);*/
        }

        [HttpPost]
        public void Post()
        {
            var role = new Role("书记");
            var name = role.Name;
            var id = role.Id;
            var sqlstr = "insert into t_role values(@id,@name)";

            using (MySqlConnection connection = new MySqlConnection("server=localhost;Database =quangangtong;uid=root;pwd=1152417278;charset=utf8;"))
            {
                using MySqlCommand cmd = new MySqlCommand(sqlstr, connection);

                connection.Open();

                /* MySqlParameter param = new MySqlParameter("@id", MySqlDbType.Binary);
                 param.Value = id;
                 cmd.Parameters.Add(param);
                 MySqlParameter param1 = new MySqlParameter("@name", MySqlDbType.VarChar);
                 param1.Value = name;
                 cmd.Parameters.Add(param1);*/
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                int rows = cmd.ExecuteNonQuery();
                /* using MySqlDataAdapter adapter = new MySqlDataAdapter(sqlstr, connection);
                     DataTable dt = new DataTable();
                     MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                     adapter.Fill(dt);
                     var row = dt.NewRow();
                     row[0] = id;
                     row[1] = name;
                     dt.Rows.Add(row);
                     adapter.Update(dt);*/
            }
        }
        [HttpPost]
        public void Put()
        {
            var role = new Role("块长");

            var sqlstr = "insert into t_role values(@id,@name)";
            //sqlHelper.ExecteSql(sqlstr, role);
        }
        [HttpPost]
        public void Put1()
        {
            User user = userService.Find("ldh");
            Role role = roleService.Find("罗");
            UserRoleRelation relation = new UserRoleRelation();
            relation.UserId = user.Id;
            relation.RoleId = role.Id;


            int v = userRoleService.Create(relation);
            Console.WriteLine(v);

        }
        [HttpPost]
        public void FindByname()
        {

            Role role = roleService.Find("人口专员");
            Console.WriteLine(role.Name + role.Id);

        }
        [HttpPost]
        public void FindById()
        {

            Role role = roleService.Find("人口专员");
            roleService.Find(role.Id);
            Console.WriteLine(role.Name + role.Id);

        }
        [HttpGet]
        public List<Role> Getlist()
        {
            List<Role> roles = roleService.GetAll();
            return roles;
        }

    }
}
