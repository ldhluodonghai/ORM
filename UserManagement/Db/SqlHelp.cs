using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace UserManagement.Db
{
    public class SqlHelp
    {
        public SqlHelp() { } //0.空构造函数

        //1.读取appsetting.json文件中的数据库连接字符串
        private static string ConnectionString
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) //这行还有个写法是：.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); //optional=true表示文件是必须的，reloadOnChange=true表示当文件变更时自动更新配置
                var strConnection = builder.Build().GetSection("DbConnectionStr")["SqlServerStr"];
                return strConnection;
            }
        }

        //2.执行sql语句返回dataset
        public DataSet Query(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        //3.执行sql语句，返回受影响的行数
        public int ExecuteSql(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {

                    try
                    {
                        connection.Open();



                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public int Insert<T>(string SQLString, T entity) where T : class, new()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {

                    try
                    {
                        connection.Open();
                        /*var t = Assembly
                                 .GetExecutingAssembly()
                                 .GetReferencedAssemblies()
                                 .Select(x => Assembly.Load(x))
                                  .SelectMany(x => x.GetTypes()).First(x => x.FullName == typeName);*/
                        Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "\\" + "Model" + ".dll");                    
                        Type[] types = assembly.GetTypes();
                        Type modelType = typeof(T);
                        
                        string modelName = modelType.Name;
                        //Console.WriteLine(modelName);
                        //Console.WriteLine();
                        foreach (Type type in types)
                        {
                            Console.WriteLine(type.Name);
                            if (type.Name == modelName)
                           {         
                                FieldInfo[] fieldInfos = type.GetFields();
                                foreach (FieldInfo fieldInfo in fieldInfos)
                                {
                                    var nameType = fieldInfo.FieldType.Name;
                                    var name = fieldInfo.Name;
                                    var v = fieldInfo.GetValue(entity);
                                    cmd.Parameters.AddWithValue("@name", name);
                                }
                            }
                        }
                        //Type bean = entity.GetType();
                        //Assembly assembly = typeof(T).Assembly;


                        //FieldInfo[] fieldInfos = bean.GetRuntimeFields().Where(f => f.IsPublic).ToArray();

                        /* FieldInfo[] fieldInfos = bean.GetFields();
                         foreach (FieldInfo fieldInfo in fieldInfos)
                         {
                             var nameType = fieldInfo.FieldType.Name;
                             var name = fieldInfo.Name;
                             var v = fieldInfo.GetValue(entity);
                             cmd.Parameters.AddWithValue("@name", name);
                         }*/
                        /*cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@name", name);*/
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public int ExecteSql<T>(string SQLString, T entity) where T : class, new()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        Type ts = entity.GetType();
                        PropertyInfo[] propertyInfos = ts.GetProperties();
                        foreach (PropertyInfo propertyInfo in propertyInfos)
                        {
                            //Type fieldType = fi.FieldType;
                            string name = propertyInfo.Name;
                            string parm = "@" + name;
                            object? v = propertyInfo.GetValue(entity);
                            cmd.Parameters.AddWithValue(parm, v);                           
                        }
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
    }
}
