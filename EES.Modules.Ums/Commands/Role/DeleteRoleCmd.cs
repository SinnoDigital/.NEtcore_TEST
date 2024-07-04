using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Role
{
    /// <summary>
    /// 删除角色
    /// </summary>
    public class DeleteRoleCmd : CommandBase
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public long Id { get; set; }
    }
}
