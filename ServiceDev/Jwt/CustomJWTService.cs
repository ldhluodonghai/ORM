using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.Entitys;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.ServiceDev.Jwt
{
    public class CustomJWTService : ICustomJWTService
    {
        private readonly JwtTokenOptions jwtTokenOptions;
        private readonly RoleService roleService;
        private readonly UserRoleService userRoleService;
       

        public CustomJWTService(IOptionsMonitor<JwtTokenOptions> jwtTokenOptions,RoleService roleService, UserRoleService userRoleService)
        {
            this.jwtTokenOptions = jwtTokenOptions.CurrentValue;
            this.roleService = roleService;
            this.userRoleService = userRoleService;
        }

        public string GetJwtToken(User user)
        {

            UserRoleRelation userRoleRelation = userRoleService.FindAny(user.Id);
            Role role = roleService.Find(userRoleRelation.RoleId);
            string roleName = role.Name;

            //payload
            var claims = new Claim[]
                {
                    
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name",user.Name),
                    new Claim("PhoneNumber", user.PhoneNumber),
                    new Claim(ClaimTypes.Role,roleName),
                    new Claim("IsEnable", user.IsEnable.ToString()),
                    new Claim(ClaimTypes.Version,user.JWTVersion.ToString())

                };
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF-LDH"));
            //Nuget引入：Microsoft.IdentityModel.Tokens
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOptions.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Nuget引入：System.IdentityModel.Tokens.Jwt
            var token = new JwtSecurityToken(
                issuer: jwtTokenOptions.Issuer,
                audience: jwtTokenOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
             );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
