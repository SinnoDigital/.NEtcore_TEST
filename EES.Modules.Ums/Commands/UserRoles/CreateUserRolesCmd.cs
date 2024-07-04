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
    /// 创建用户-角色绑定关系
    /// </summary>
    public class CreateUserRolesCmd : CommandBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        public CreateUserRolesCmd(long userId, IEnumerable<long> roleIds) : base()
        {
            UserId = userId;
            RoleIds = roleIds;
        }

        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        [Required]
        public IEnumerable<long> RoleIds { get; set; }

    }
}
