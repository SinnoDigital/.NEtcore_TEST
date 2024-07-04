using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Jwt
{
    public class JwtTokenProvider
    {
        private static readonly JwtOption _options;

        private static readonly JwtSecurityTokenHandler _tokenHandler;

        private static readonly TokenValidationParameters _validationParameters;

        static JwtTokenProvider()
        {
            _options = JwtOption.GetDefaultOptions();
            _validationParameters = JwtOption.GetDefaultTokenValidationParameters(_options);
            _tokenHandler = new JwtSecurityTokenHandler();
        }


        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="expMins"></param>
        /// <returns></returns>
        private static string Generate(CurrentUser user, int expMins)
        {
            //存入Token的信息
            var claims = new[] {
                new Claim(ClaimsModel.Id,user.Id),
                new Claim(ClaimsModel.Platform,user.Platform),
            };
            //加密key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //生成Token
            JwtSecurityToken security = new(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expMins),
                signingCredentials: creds);
            //将Token信息转换为字符串
            var token = _tokenHandler.WriteToken(security);

            return token;
        }



        public static string GenerateToken(string id, string platform, int expMins = 60)
        {

            CurrentUser user = new()
            {
                Id = id,
                Platform = platform,
            };

            return Generate(user, expMins);
        }

        public static DateTime GetExpirationTime(string token, bool getLocalTime = true)
        {
            try
            {
                JwtSecurityToken jwt = _tokenHandler.ReadJwtToken(token);

                return getLocalTime ? TimeZoneInfo.ConvertTimeFromUtc(jwt.ValidTo, TimeZoneInfo.Local)
                                   : jwt.ValidTo;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static bool TryGetExpirationTime(string jwtToken, out DateTime expirationTime, bool getLocalTime = true)
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = _tokenHandler.ValidateToken(jwtToken, _validationParameters, out SecurityToken validatedToken);
                JwtSecurityToken jwtSecurityToken = validatedToken as JwtSecurityToken;

                var time = jwtSecurityToken!.ValidTo; // 获取过期时间

                expirationTime = getLocalTime ? TimeZoneInfo.ConvertTimeFromUtc(time, TimeZoneInfo.Local)
                                   : time;

                return true;
            }
            catch (Exception)
            {
                expirationTime = DateTime.MinValue;
                return false;
            }
        }

        public static bool TryGetExpirationTimestamp(string jwtToken, out int? exp)
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = _tokenHandler.ValidateToken(jwtToken, _validationParameters, out SecurityToken validatedToken);
                JwtSecurityToken jwtSecurityToken = validatedToken as JwtSecurityToken;

                exp = jwtSecurityToken!.Payload.Exp;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                exp = 0;
                return false;

            }
        }

        /// <summary>
        /// 获取jwt的Claim信息
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <param name="platform"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool TryGetClaim(string jwtToken, out string? platform, out long? userId)
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = _tokenHandler.ValidateToken(jwtToken, _validationParameters, out SecurityToken validatedToken);
                JwtSecurityToken jwtSecurityToken = validatedToken as JwtSecurityToken;

                var claims = jwtSecurityToken?.Claims;

                platform = claims?.FirstOrDefault(t => t.Type == "platform")?.Value;

                var _userId = claims?.FirstOrDefault(t => t.Type == "id")?.Value;

                if (string.IsNullOrWhiteSpace(_userId))
                {
                    userId = 0;
                }
                else
                {
                    userId = long.Parse(_userId);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex is not SecurityTokenExpiredException) //有时候匿名访问也会带上之前的token，这里把这个异常过滤掉
                {
                    Console.WriteLine(ex);
                }             
                platform = string.Empty;
                userId = 0;
                return false;
            }
        }
    }
}
