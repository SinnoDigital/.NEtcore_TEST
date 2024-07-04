using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Data
{
    /// <summary>
    /// 删除物料类型数据
    /// </summary>
    public class DeleteMaterialDataCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteMaterialDataCmd():base() { }


        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
    }
}
