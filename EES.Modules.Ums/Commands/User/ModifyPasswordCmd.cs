using EES.Infrastructure.Bus;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.User
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class ModifyPasswordCmd : CommandBase
    {

        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
    }
}
