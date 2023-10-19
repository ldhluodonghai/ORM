using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Model.Entitys;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlTypes;
using UserManagement.Db;

namespace UserManagement.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        //public readonly IBaseService baseService;
        private readonly SqlHelp sqlHelp;
        //SqlHelper.Query("")
        public TestController(SqlHelp sqlHelper)
        {
            this.sqlHelp = sqlHelper;
        }
        [HttpGet]
        public void Get()
        {
            var v = sqlHelp.Query("select * from t_user");
            Console.WriteLine(v);
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
            sqlHelp.ExecteSql(sqlstr,role);
        }
    }
}
