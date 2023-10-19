using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Role
    {
       
        public Guid Id { get;private set; }
        public string? Name { get; set; }

        public Role()
        {
            Id = Guid.NewGuid();
        }

        public Role( string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
