using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.User
{
    /// <summary>
    /// 删除用户
    /// </summary>
    public class DeleteUserCmd : CommandBase
    {
        /// <summary>
        /// 删除用户
        /// </summary>
        public DeleteUserCmd() : base() { }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        public DeleteUserCmd(long userId) : base()
        {
            UserId = userId;
        }


        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public long UserId { get; set; }
    }
}
