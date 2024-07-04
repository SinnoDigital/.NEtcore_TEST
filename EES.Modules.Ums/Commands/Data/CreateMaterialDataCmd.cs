using EES.Infrastructure.Bus;
using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Data
{
    /// <summary>
    /// 创建物料类型数据
    /// </summary>
    public class CreateMaterialDataCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateMaterialDataCmd():base() { }


        /// <summary>
        /// 物料类型
        /// </summary>
        public MaterialType  MaterialType { get; set; }


        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
