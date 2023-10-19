using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class JwtTokenOptions
    {
        public string Audience
        {
            get;
            set;
        }
        public string SecurityKey
        { 
            get; 
            set;
        }
        public string Issuer
        {
            get;
            set;
        }
    }
}
