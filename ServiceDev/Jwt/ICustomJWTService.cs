using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.ServiceDev.Jwt
{
    public interface ICustomJWTService
    {
        string GetJwtToken(User user);
    }
}
