using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Dto
{
    /// <summary>
    /// 用户参数模型
    /// </summary>
    public class UserParamDto
    {

        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

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
        /// 修改前的值
        /// </summary>
        public string BeforeValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
     
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 最后一次更新用户的名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }

    /// <summary>
    /// 系统参数模型
    /// </summary>
    public class SystemParamDto
    {

        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public SystemModule Module { get;  set; }
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
        /// 默认值
        /// </summary>
        public string DefalutValue { get; set; }
        /// <summary>
        /// 设定值
        /// </summary>
        public string SetValue { get; set; }
        /// <summary>
        /// 修改前的值
        /// </summary>
        public string BeforeValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 最后一次更新用户的名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
