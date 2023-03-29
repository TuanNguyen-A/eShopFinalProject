using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.Common
{
    public class ApplicationUtils
    {
        public static JwtSecurityToken ReadJwtToken(string authorization)
        {
            var jwtToken = authorization.Substring(0, 7).ToLower() == "bearer " ?
                authorization.ToString().Substring("bearer ".Length) : authorization;
            var tokenValidator = new JwtSecurityTokenHandler();
            return tokenValidator.ReadJwtToken(jwtToken);
        }
    }
}
