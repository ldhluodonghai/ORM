using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys.Dto
{
    public class UserDto
    {
        public String Name { get; set; }    
        public String PhoneNumber { get; set; }
        public String Password { get; set; }
        public UserDto() { }

        public UserDto(string name, string phoneNumber, string password)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Password = password;
        }
    }
}
