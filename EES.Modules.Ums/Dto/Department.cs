using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Dto
{
    /// <summary>
    /// 部门信息
    /// </summary>
    public class DepartmentDto
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 上级部门名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int SortNumber { get; set; }

        /// <summary>
        /// 代码编码(通常由用户指定，以关联到其他系统)
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
        /// 图标的url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }


        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改者名称
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
