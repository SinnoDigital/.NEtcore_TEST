using EES.Infrastructure.Bus;
using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.SystemParams
{
    /// <summary>
    /// 修改系统参数
    /// </summary>
    public class ModifySystemParamCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ModifySystemParamCmd() : base() { }

        /// <summary>
        /// Ids
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public SystemModule Module { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; } = true;
        /// <summary>
        /// 参数编码
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [Required]
        public string DefalutValue { get; set; }
        /// <summary>
        /// 设定值
        /// </summary>
        [Required]
        public string SetValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
