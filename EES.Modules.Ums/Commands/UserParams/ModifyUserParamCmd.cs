using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.UserParams
{
    /// <summary>
    /// 修改用户参数
    /// </summary>
    public class ModifyUserParamCmd : CommandBase
    {

        /// <summary>
        /// 
        /// </summary>
        public ModifyUserParamCmd() { }


        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 参数编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 参数单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 值类型
        /// </summary>
        public string ValueType { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefalutValue { get; set; }
        /// <summary>
        /// 设定值
        /// </summary>
        public string SetValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
