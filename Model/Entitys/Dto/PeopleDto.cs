using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys.Dto
{
    public class PeopleDto
    {
        public string PeopleName { get; set; }



        public PeopleDto(string peopleName)
        {
            PeopleName = peopleName;
        }

        public PeopleDto()
        {
        }
    }
}
