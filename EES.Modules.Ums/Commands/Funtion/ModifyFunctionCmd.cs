using EES.Infrastructure.Bus;
using EES.Infrastructure.Enums;
using EES.Modules.Ums.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Funtion
{
    /// <summary>
    /// 
    /// </summary>
    public class ModifyFunctionCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ModifyFunctionCmd() : base() { }

        /// <summary>
        /// Ids
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 标识 1: 分类标识   2：功能
        /// </summary>
        public FunctionType Type { get; set; }
        /// <summary>
        /// 父级id 没有父级默认为0
        /// </summary>
        [Required]
        public long ParentId { get; set; }
        /// <summary>
        /// 功能说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 功能权限标识符
        /// </summary>
        public string Identifier { get; set; }
    }
}
