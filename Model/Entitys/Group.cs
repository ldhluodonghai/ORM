using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Group
    {
        public Guid Id { get;private set; }
        public string? Name { get; set; }

        public Group() {
            Id = Guid.NewGuid();
        }
        public Group(string name)
        {
            this.Name = name;
            Id = Guid.NewGuid();
        }
    }
}
