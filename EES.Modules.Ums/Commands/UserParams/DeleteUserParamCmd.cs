using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.UserParams
{
    /// <summary>
    /// 删除用户参数
    /// </summary>
    public class DeleteUserParamCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteUserParamCmd():base() { }


        /// <summary>
        /// Ids
        /// </summary>
        public long Id { get; set; }
    }
}
