using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Funtion
{
    /// <summary>
    /// 删除功能资源
    /// </summary>
    public class DeleteFunctionCmd : CommandBase
    {
        /// <summary>
        /// 删除功能资源
        /// </summary>
        public DeleteFunctionCmd() : base() { }

        /// <summary>
        /// Ids
        /// </summary>
        public long Id { get; set; }
    }
}
