using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.SystemParams
{
    /// <summary>
    /// 删除系统参数
    /// </summary>
    public class DeleteSystemParamCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteSystemParamCmd() : base() { }

        /// <summary>
        /// Ids
        /// </summary>
        public long Id { get; set; }
    }
}
