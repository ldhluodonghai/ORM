using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys.Model
{
    public class UserModel
    {
        public string Name { get; set; } 

        public string Phones { get; set; }

        public string[] Role { get; set; }
        public UserModel() { }

        public UserModel(string name, string phones, string[] role)
        {
            Name = name;
            Phones = phones;
            Role = role;
        }
    }
}
