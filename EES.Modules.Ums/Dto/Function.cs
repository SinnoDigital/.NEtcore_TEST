using EES.Infrastructure.Enums;
using EES.Modules.Ums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Dto
{
    /// <summary>
    /// 功能数据
    /// </summary>
    public class FunctionDto
    {

        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// 标识 1: 分类标识   2：功能
        /// </summary>
        public FunctionType Type { get;  set; }
    
        /// <summary>
        /// 功能说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 功能权限标识符
        /// </summary>
        public string Identifier { get; set; }
    
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

        /// <summary>
        /// 上级id
        /// </summary>
        public long ParentId { get; set; }
    }

    /// <summary>
    /// 功能权限资源树状结构
    /// </summary>
    public class FunctionTreeDto
    {

        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标识 1: 分类标识   2：功能
        /// </summary>
        public FunctionType Type { get; set; }

        /// <summary>
        /// 功能权限标识符
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public long ParentId { get; set; }
    }
}
