using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.User
{
    /// <summary>
    /// 重置用户密码
    /// </summary>
    public class ResetPasswordCmd : CommandBase
    {

        /// <summary>
        /// 
        /// </summary>
        public ResetPasswordCmd():base() { }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string Password { get; set; }
    }
}
