using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public  class User
    {
        
        public Guid Id { get; private set; }
        public long JWTVersion { get;  set; } = 0;
        public string? Name { get; set; }
        public String Created { get; private set; } 
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
        public int IsEnable { get; set; }=0;
        public User() 
        {
            Id = Guid.NewGuid();
            string v = DateTime.Now.ToString();
            Created = v;
        }
        public User(
            long jwtVersion,string name,  string? phoneNumber, string password)
        {
            JWTVersion = jwtVersion;
            Name = name;
          
            string v = DateTime.Now.ToString();
            Created = v;
            PhoneNumber = phoneNumber;
            Password = password;
           
        }
    }
}
