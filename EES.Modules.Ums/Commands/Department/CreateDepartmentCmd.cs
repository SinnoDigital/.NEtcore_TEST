using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Department
{
    /// <summary>
    /// 新建部门
    /// </summary>
    public class CreateDepartmentCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateDepartmentCmd() : base()
        {

        }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        [Required]
        public long ParentId { get; set; } = 0;
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
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } 
        /// <summary>
        /// 图标的url
        /// </summary>
        public string ImageUrl { get; set; } 
    }
}
