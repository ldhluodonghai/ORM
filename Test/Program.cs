
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestConsole
{
    /*    public interface IUserProcessor
        {
            void RegUser(Student stident);
        }
        public class UserProcess : IUserProcessor
        {
            public void RegUser(Student student)
            {
                Console.WriteLine(student.age);
            }
        }
        public class UserProcessorDecorator : IUserProcessor
        {
            private IUserProcessor _userProcessor { get; set; }
            public UserProcessorDecorator(IUserProcessor userProcessor)
            {
                this._userProcessor = userProcessor;
            }
             public void RegUser(Student stident)
            {
                Console.WriteLine("11111");
                this._userProcessor.RegUser(stident);
            }
        }


        public class Student
        {
            public string name { get; set; }
            public int age { get; set; }
            public string className { get; set; }

        }
        //教师类

       public class Teacher
        {
            public string name { get; set; }
            public int age { get; set; }
            public string sex { get; set; }
        }
        public class LDHAttribute: Attribute
        {
            public string name { get; set; }
        }
        public class Progrm
        {
             static void Main(string[] args)
            {
                //给学生类赋值
                Student student = new Student
                {
                    name = "张三",
                    age = 20,
                    className = "六"
                };
                //泛型+反射方式
                Teacher teacher = ConvertModel<Teacher, Student>(student);
                Console.WriteLine(teacher.name);//张三
                Console.WriteLine(teacher.age);//20
                Console.WriteLine(teacher.sex);

                //JSON序列化方式
                teacher = null;
                string json = JsonConvert.SerializeObject(student);
                Console.WriteLine(json);
                teacher = JsonConvert.DeserializeObject<Teacher>(json);
                Console.WriteLine(teacher.name);//张三
                Console.WriteLine(teacher.age); //20
                Console.WriteLine(teacher.sex);


            }

            //泛型+反射

            private static T ConvertModel<T, P>(P pModel)
            {
                T ret = System.Activator.CreateInstance<T>();
                Type type = typeof(T);
                type.IsDefined(typeof(T), false);
                List<PropertyInfo> p_pis = pModel.GetType().GetProperties().ToList();
                PropertyInfo[] t_pis = typeof(T).GetProperties();

                foreach (PropertyInfo pi in t_pis)
                {
                    //可写入数据
                    if (pi.CanWrite)
                    {
                        //忽略大小写
                        var name = p_pis.Find(s => s.Name.ToLower() == pi.Name.ToLower());
                        if (name != null && pi.PropertyType.Name == name.PropertyType.Name)
                        {
                            pi.SetValue(ret, name.GetValue(pModel, null), null);
                        }

                    }
                }
                return ret;
            }
            */
    /*class Program
            {
                static async Task Main(string[] args)
                {
                    // 假设有多个视频文件需要上传
                    string[] videoFiles = { "video1.mp4", "video2.mp4", "video3.mp4" };

                    // 创建一个任务数组用于存储上传任务
                    var uploadTasks = new Task[videoFiles.Length];

                    // 启动多个上传任务
                    for (int i = 0; i < videoFiles.Length; i++)
                    {
                        int index = i; // 需要在闭包中使用
                        uploadTasks[i] = Task.Run(() => UploadVideo(videoFiles[index]));
                    }

                    // 等待所有上传任务完成
                    await Task.WhenAll(uploadTasks);

                    Console.WriteLine("所有视频上传完成！");
                }

                static void UploadVideo(string videoFileName)
                {
                    // 模拟上传视频的操作，可以根据实际需要替换为真正的上传逻辑
                    Console.WriteLine($"开始上传视频 {videoFileName}...");
                    // 模拟上传过程
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine($"上传进度：{i * 10}%");
                        Task.Delay(500).Wait();
                    }
                    Console.WriteLine($"视频 {videoFileName} 上传完成！");
                }
            }*//*
        }

            public void RegUser(Student stident)
            {
                _userProcessor.RegUser(stident);
            }
        }*/

    /*using System;
    using System.IO;
    using System.Threading.Tasks;

    namespace MultiThreadedVideoUpload
    {
        class Program
        {
            static async Task Main(string[] args)
            {
                string videoFilePath = "video.mp4";

                // 启动多个上传任务
                var uploadTasks = new Task[3]; // 三个并发上传任务

                for (int i = 0; i < uploadTasks.Length; i++)
                {
                    uploadTasks[i] = UploadVideoAsync(videoFilePath, i + 1);
                }

                // 等待所有上传任务完成
                await Task.WhenAll(uploadTasks);

                Console.WriteLine("所有上传任务已完成！");
            }

            static async Task UploadVideoAsync(string videoFilePath, int taskId)
            {
                try
                {
                    // 模拟上传视频的操作，可以根据实际需要替换为真正的上传逻辑
                    Console.WriteLine($"任务 {taskId}: 开始上传视频...");

                    using (var fileStream = new FileStream(videoFilePath, FileMode.Open))
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            // 这里可以调用上传方法，将 buffer 中的数据上传到服务器
                            // 模拟上传过程
                            await Task.Delay(100);
                        }
                    }

                    Console.WriteLine($"任务 {taskId}: 视频上传完成！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"任务 {taskId}: 上传失败 - {ex.Message}");
                }
            }
        }
    }
    */
    class My
    { 
        public int Add(int x, int y)
        {
            return x + y;
        }
        public int Sub(int x, int y) { return x - y; }
    }
    delegate int MyDel(int id,int c); 
    class Send
    {
        public event EventHandler CountedDozen;
        public void DoCount()
        {

            for(int i = 0; i < 100; i++)
            {
                if (i % 12 == 0 && CountedDozen != null)
                {
                    CountedDozen(this, null);
                }
            }
        }
    }
    class Sub
    {
        public int DozenCount { get; set; }
        public Sub(Send send)
        {
            DozenCount = 0;
            send.CountedDozen += Increment;
        }
        void Increment(object s, EventArgs e)
        {
            DozenCount++;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /* My my = new My();
            MyDel myDel = new MyDel(my.Add);
            myDel+=my.Sub;
            myDel+=my.Add;
            myDel+=my.Add;
            myDel+=my.Add;
            Console.WriteLine(myDel);
            Console.WriteLine(myDel(1,2));Console.WriteLine(myDel(1,2));
            MyDel mydel1 =delegate(int a,int b)
            {
                return a + b;
            };
            MyDel mydel2 =(int a,int b)=>
            {
                return a + b;
            }; 
            MyDel mydel3 =( a, b)=>
            {
                return a + b;
            };
            MyDel mydel4 =( a, b)=> a + b;
            Console.WriteLine();*/

            Send send = new Send();
            Sub sub = new Sub(send);
            send.DoCount();
            Console.WriteLine(sub.DozenCount);
        }
    }

}