
using Microsoft.Extensions.Configuration;
using Model.Entitys;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace UserManagement.Db
{
    public class ORMSqlHelper<T> where T : class,new()
    {
        public ORMSqlHelper() { } //0.空构造函数
        //1.读取appsetting.json文件中的数据库连接字符串直接手写。
        private static string ConnectionString
        {
            get
            {
                /*var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) //这行还有个写法是：.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); //optional=true表示文件是必须的，reloadOnChange=true表示当文件变更时自动更新配置
                var strConnection = builder.Build().GetSection("DbConnectionStr")["SqlServerStr"];*/
                return "server=localhost;Database =quangangtong;uid=root;pwd=1152417278;charset=utf8;";
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
        //直接反射获取实体
        public T FindByName(string name)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                T t = new T();
                Type type = t.GetType();
                string className = type.Name;
                string classname = className.ToLower();
                string table = "t_" + classname;
                string sql = $"select * from {table} where name='{name}'";
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(sql, connection);
                    command.Fill(ds, "ds");
                    DataTable dataTable = ds.Tables[0];
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    int length = propertyInfos.Length;
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {                       
                        for (int i = 0; i < length; i++)
                        {
                            string columnName = dataTable.Columns[i].ColumnName;                           
                            if (propertyInfo.Name.ToLower().Equals(columnName))
                            {
                                var value = dataTable.Rows[0][i];
                                if (columnName.Equals("id"))//可以去掉了一开始为了解决id类型现在数据库字段设置成了char（36）
                                {
                                    /*  byte[]? bytes = value as byte[];
                                      int v = bytes.Count();
                                      Guid guid = new Guid(bytes);*/
                                    //string? v = value.ToString();
                                    propertyInfo.SetValue(t, value);
                                    //Byte array for Guid must be exactly 16 bytes long. Arg_ParamName_Name”
                                }
                                else
                                {
                                    propertyInfo.SetValue(t, value);
                                }                             
                            }     
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                   throw new Exception(ex.Message);
                }
                return t; 
            }
        }
        //通过id查询
        [Obsolete]
        public T FindById(Guid id)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                T t = new T();
                Type type = t.GetType();
                string className = type.Name;
                string classname = className.ToLower();
                string table = "t_" + classname;                
                string sql = $"select * from {table} where id='{id}'";
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(sql, connection);
                    command.Fill(ds, "ds");
                    DataTable dataTable = ds.Tables[0];
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    int length = propertyInfos.Length;
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            string columnName = dataTable.Columns[i].ColumnName.ToLower();
                            if (propertyInfo.Name.ToLower().Equals(columnName))
                            {
                                var value = dataTable.Rows[0][i];                             
                                propertyInfo.SetValue(t, value);                               
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    throw new Exception(ex.Message);
                }
                return t;
            }
        }
        //优化所有id查询
        public T FindByAnyIds(Guid id)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                T t = new T();
                Type type = t.GetType();
                string className = type.Name;
                string classname = className.ToLower();
                PropertyInfo[] propertyInfos1 = type.GetProperties();
                connection.Open();
                foreach (PropertyInfo ps in propertyInfos1)
                {
                    string name = ps.Name.ToLower();
                    string table = "t_" + classname;
                    string sql = $"select * from {table} where {name}='{id}'";
                    DataSet ds = new DataSet();
                    try
                    {
                        //connection.Open();
                        MySqlDataAdapter command = new MySqlDataAdapter(sql, connection);
                        command.Fill(ds, "ds");
                        DataTable dataTable = ds.Tables[0];
                        DataRowCollection rows = dataTable.Rows;
                        int rowLength = 0;
                        foreach (DataRow row in rows)
                        {
                            rowLength++;
                        }
                        if (rowLength != 0)
                        {
                            int length1 = propertyInfos1.Length;                          
                            foreach (PropertyInfo propertyInfo in propertyInfos1)
                            {
                                for (int i = 0; i < length1; i++)
                                {
                                    string columnName = dataTable.Columns[i].ColumnName.ToLower();
                                    if (propertyInfo.Name.ToLower().Equals(columnName))
                                    {
                                        var value = dataTable.Rows[0][i];
                                        propertyInfo.SetValue(t, value);
                                    }
                                }
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        connection.Close();
                        throw new Exception(ex.Message);
                    }
                }
                //string table = "t_" + classname;
               // string sql = $"select * from {table} where id='{id}'";
                //DataSet ds = new DataSet();
                /*try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(sql, connection);
                    command.Fill(ds, "ds");
                    DataTable dataTable = ds.Tables[0];
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    int length = propertyInfos.Length;
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            string columnName = dataTable.Columns[i].ColumnName.ToLower();
                            if (propertyInfo.Name.ToLower().Equals(columnName))
                            {
                                var value = dataTable.Rows[0][i];
                                propertyInfo.SetValue(t, value);

                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    throw new Exception(ex.Message);
                }*/
                return t;
            }
        }
        public T FindByPhoneNumber(string phoneNumber)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                T t = new T();
                Type type = t.GetType();
                string className = type.Name;
                string classname = className.ToLower();
                string table = "t_" + classname;
                string sql = $"select * from {table} where phoneNumber='{phoneNumber}'";
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(sql, connection);
                    command.Fill(ds, "ds");
                    DataTable dataTable = ds.Tables[0];
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    int length = propertyInfos.Length;
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            string columnName = dataTable.Columns[i].ColumnName;
                            if (propertyInfo.Name.ToLower().Equals(columnName))
                            {
                                var value = dataTable.Rows[0][i];
                                propertyInfo.SetValue(t, value);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    throw new Exception(ex.Message);
                }
                return t;
            }
        }   
        //升级版
        /// <summary>
        /// 自定义传入实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SQLString"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  int ExecteSql(string SQLString, T entity) 
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
        /*  public static string GetModelValue(string FieldName, object obj)
          {
              try
              {
                  Type Ts = obj.GetType();
                  object o = Ts.GetProperty(FieldName).GetValue(obj, null);
                  if (null == o)
                      return null;
                  string Value = Convert.ToString(o);
                  if (string.IsNullOrEmpty(Value))
                      return null;
                  return Value;
              }
              catch (Exception ex)
              {
                  throw ex;
              }
              return null;
          }*/

       // public int Add(T entity) where T : class, new()
       //优化版
        public int Add(T entity) 
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                int i = 0;
                Type type = entity.GetType();
                string className = type.Name;
                string classname = className.ToLower();
                string table = "t_" + classname;
                PropertyInfo[] propertys = type.GetProperties();
                string sql = $"insert into {table} values(";
                StringBuilder stringBuilder = new StringBuilder(sql);                
                foreach(var ps  in propertys)
                {
                    i++;
                    string name = ps.Name;
                    string param = "@" + name + ",";
                    stringBuilder.Append(param);
                }
                string devSql = stringBuilder.ToString();
                string newSql = devSql.Remove(devSql.Length - 1, 1);
                string realSql = newSql + ")";
                //string sql = $"insert into {table} values({param},@name) ";
                using (MySqlCommand cmd = new MySqlCommand(realSql, connection))
                {
                    try
                    {
                        connection.Open();                       
                        foreach (PropertyInfo propertyInfo in propertys)
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
        //批量获取
        public List<T> GetList()
        {    
            using (MySqlConnection con = new MySqlConnection(ConnectionString))
            {
                List<T> list = new List<T>();             
                Type type = typeof(T);
                string className = type.Name;
                string classname = className.ToLower();
                string table = "t_" + classname;
                string sql = $"select * from {table}";
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(sql, con);
                    command.Fill(ds, "ds");
                    DataTable dataTable = ds.Tables[0];                   
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    int cloumns = dataTable.Columns.Count;
                    int rows = dataTable.Rows.Count;
                    for (int j = 0; j < rows; j++)
                    {
                        T t = new T();
                        foreach (PropertyInfo propertyInfo in propertyInfos)
                        {
                            for (int i = 0; i < cloumns; i++)
                            {
                                string columnName = dataTable.Columns[i].ColumnName;
                                if (propertyInfo.Name.ToLower().Equals(columnName.ToLower()))
                                {
                                    var value = dataTable.Rows[j][i];
                                    propertyInfo.SetValue(t, value);
                                }                                                         
                            }                          
                        }
                        list.Add(t);
                    }                                                              
                }
                catch (MySqlException e)
                {
                    con.Close();
                    throw new Exception(e.Message);
                }
                return list;
            }            
        }
        //异步
        public async Task<T> FindAsync(string name)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                T t = new T();
                Type type = t.GetType();
                string className = type.Name;
                string classname = className.ToLower();
                string table = "t_" + classname;
                string sql = $"select * from {table} where name='{name}'";
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(sql, connection);
                    command.Fill(ds, "ds");
                    DataTable dataTable = ds.Tables[0];
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    int length = propertyInfos.Length;
                    /*int[] a= {1,2,3};
                    IEnumerable<int> enumerable = a.Cast<int>();
                    IEnumerable<int> enumerable1 = a;*/
                    

                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            string columnName = dataTable.Columns[i].ColumnName.ToLower();
                            if (propertyInfo.Name.ToLower().Equals(columnName))
                            {
                                var value = dataTable.Rows[0][i];
                               /* if (columnName.ToLower().Equals("id"))//可以去掉了一开始为了解决id类型现在数据库字段设置成了char（36）
                                {
                                    *//*  byte[]? bytes = value as byte[];
                                      int v = bytes.Count();
                                      Guid guid = new Guid(bytes);*//*
                                    //string? v = value.ToString();
                                    propertyInfo.SetValue(t, value);
                                    //Byte array for Guid must be exactly 16 bytes long. Arg_ParamName_Name”
                                }
                                else
                                {
                                   
                                }*/
                                propertyInfo.SetValue(t, value);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    throw new Exception(ex.Message);
                }
                return t;
            }
        }

        public int Delete(Guid id)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                T t = new();
                Type type = t.GetType();
                string name = type.Name;
                string tableName ="t_" +name.ToLower();
                string sql = $"delete from {tableName} where id = {id} ";
                using(MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    try
                    {
                        conn.Open();               
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }


        public int Edit(T t)
        {
            using(MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                Type type = t.GetType();
                string table ="t_"+ type.Name.ToLower();
                //UPDATE 表名 SET column_name=value [,column_name2=value2,...] [WHEREcondition];
                PropertyInfo[] propertyInfos = type.GetProperties();
                string sql = $"update {table} set ";
                StringBuilder newSql = new StringBuilder(sql);
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {                   
                    string name = propertyInfo.Name.ToLower();
                    if (!name.Equals("name"))
                    {
                        if(!name.Equals("id"))
                        {
                            if(!name.Equals("created"))
                            {
                                string sqlsplit = name + "=@" + propertyInfo.Name.ToLower() + ",";
                                newSql.Append(sqlsplit);
                            }
                        }                        
                    }                   
                    //string sql = $"updat {table} set {name} = {v}";
                }
                string devSql = newSql.ToString();
                string sqlspilt = devSql.Remove(devSql.Length - 1, 1);

                StringBuilder realSql = new StringBuilder(sqlspilt);
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    string name = propertyInfo.Name.ToLower();
                    if (name.Equals("name"))
                    {
                        object? v = propertyInfo.GetValue(t);
                        string s = $" where name='{v}'";
                        realSql.Append(s);

                    }
                }
                string sqlnew = realSql.ToString();
                
                using (MySqlCommand cmd = new MySqlCommand(sqlnew, conn))
                {
                    try
                    {
                        conn.Open();
                        foreach(PropertyInfo propertyInfo in propertyInfos)
                        {
                            string name = propertyInfo.Name;
                            if (!name.Equals("Name"))
                            {
                                if(!name.Equals("Id"))
                                {
                                    if (!name.Equals("Created"))
                                    {
                                        string parm = "@" + name;
                                        string v1 = parm.ToLower();
                                        object? v = propertyInfo.GetValue(t);
                                        cmd.Parameters.AddWithValue(v1, v);
                                    }
                                }                               
                            }                           
                        }
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }                                         
            }           
        }


        public List<T> FindRolePostsById(Guid roleId)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                List<T> list = new List<T>();
                Type type = typeof(T);
                string tname = type.Name;
                string tableName = "t_" + tname.ToLower();
                string sql = $"select * from {tableName} where roleId = '{roleId}'";
                DataSet ds = new DataSet();
                try
                {
                    conn.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(sql, conn);
                    command.Fill(ds, "ds");
                    DataTable dataTable = ds.Tables[0];
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    int cloumns = dataTable.Columns.Count;
                    int rows = dataTable.Rows.Count;
                    for (int j = 0; j < rows; j++)
                    {
                        T t = new T();
                        foreach (PropertyInfo propertyInfo in propertyInfos)
                        {
                            for (int i = 0; i < cloumns; i++)
                            {
                                string columnName = dataTable.Columns[i].ColumnName;
                                if (propertyInfo.Name.ToLower().Equals(columnName.ToLower()))
                                {
                                    var value = dataTable.Rows[j][i];
                                    propertyInfo.SetValue(t, value);
                                }
                            }
                        }
                        list.Add(t);
                    }
                }
                catch (MySqlException e)
                {
                    conn.Close();
                    throw new Exception(e.Message);

                }
                
                return list;
            }
        }
    }
}
