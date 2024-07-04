using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Role
{
    /// <summary>
    /// 检查角色Id的合法性
    /// </summary>
    public class CheckRoleIdsCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckRoleIdsCmd() : base() { }

        /// <summary>
        /// 角色Id集合
        /// </summary>
        public IEnumerable<long> RoleIds { get; set; }
    }
}
