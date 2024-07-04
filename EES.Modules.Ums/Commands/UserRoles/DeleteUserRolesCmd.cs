using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.UserRoles
{
    /// <summary>
    /// 删除用户，清空其绑定关系
    /// </summary>
    public class DeleteUserRolesCmd : CommandBase
    {

        /// <summary>
        /// 删除用户，清空其绑定关系
        /// </summary>
        public DeleteUserRolesCmd() : base() { }

        /// <summary>
        /// 删除用户，清空其绑定关系
        /// </summary>
        /// <param name="userId"></param>
        public DeleteUserRolesCmd(long userId) : base()
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
