using EES.Infrastructure.Bus;
using EES.Modules.Ums.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Team
{
    /// <summary>
    /// 新增工作组
    /// </summary>
    public class CreateTeamCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateTeamCmd() : base() { }

        /// <summary>
        /// 工作组编码
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 工作组名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 班组类型
        /// </summary>
        [Required]
        public TeamType Type { get; set; }
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Required]
        public string FactoryCode { get; set; }
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Required]
        public string FactoryName { get; set; }
        /// <summary>
        /// 工厂id
        /// </summary>
        [Required]
        public long FactoryId { get; set; }
        /// <summary>
        /// 工作区域编码
        /// </summary>
       
        public string AreaCode { get; set; }
        /// <summary>
        /// 工作区域名称
        /// </summary>
       
        public string AreaName { get; set; }
        /// <summary>
        /// 工作区域id
        /// </summary>
        
        public long AreaId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 工作组成员
        /// </summary>
        [Required]
        public IEnumerable<long> UserIds { get; set; }
    }
}
