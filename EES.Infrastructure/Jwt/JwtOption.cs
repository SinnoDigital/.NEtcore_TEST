using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Jwt
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtOption
    {
        /// <summary>
        /// 受众
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 安全密钥
        /// </summary>
        public string SecurityKey { get; set; }
        /// <summary>
        /// 签发方
        /// </summary>
        public string Issuer { get; set; }



        /// <summary>
        /// 获取默认的Options
        /// </summary>
        /// <returns></returns>
        public static JwtOption GetDefaultOptions()
        {
            return new JwtOption
            {
                Audience = "Client",
                Issuer = "Enrich",
                SecurityKey = "VIXDzXgr8Bao8Ae8vs9y4gryNiWM8RC2O1i8yvUYCgRI7rHa7xJZqa9bzYFwog5x1iQ7l3L0YxaYSc7AluLR",
            };
        }

        public static TokenValidationParameters GetDefaultTokenValidationParameters(JwtOption option=null)
        {
            option ??= GetDefaultOptions();
         
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = option.Issuer,
                ValidAudience = option.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(option.SecurityKey)),
                ClockSkew = TimeSpan.FromSeconds(10)
            };
        }
    }
}
