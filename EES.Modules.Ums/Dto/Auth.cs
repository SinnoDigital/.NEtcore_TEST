using EES.Infrastructure.Auth;
using EES.Infrastructure.Data;
using EES.Infrastructure.Enums;
using EES.Modules.Ums.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Dto
{
    /// <summary>
    /// jwt token 
    /// </summary>
    public class TokenDto
    { 
        /// <summary>
        /// 授权的token
        /// </summary>
        public string Authorization { get; set; }

    }
  
    /// <summary>
    /// 登录参数
    /// </summary>
    public class LoginParamDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
