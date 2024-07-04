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
    /// 删除组织
    /// </summary>
    public class DeleteDepartmentCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteDepartmentCmd() : base() { }

        /// <summary>
        /// 部门id
        /// </summary>
        [Required]
        public long DepartmentId { get; set; }
    }
}
