using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Dao.ESModel
{
    public class People
    {
        public int Id { get; set; }
        public string PeopleName { get; set; }
        public string Group { get; set; }
        public string Sex { get; set; }
        public string Borth {  get; set; }
        public int Age { get ; set; }
        public string Addr { get; set; }


        public People(int id, string peopleName, string group, string sex, string borth, int age, string addr)
        {
            Id = id;
            PeopleName = peopleName;
            Group = group;
            Sex = sex;
            Borth = borth;
            Age = age;
            Addr = addr;
        }

        public People()
        {
        }
    }
}
