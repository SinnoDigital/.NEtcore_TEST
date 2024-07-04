using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Role
{
    /// <summary>
    /// 修改角色
    /// </summary>
    public class ModifyRoleCmd : CommandBase
    {

        /// <summary>
        /// 
        /// </summary>
        public ModifyRoleCmd() : base() { }


        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 角色的PC菜单权限列表
        /// </summary>
        public IEnumerable<long> PcMenus { get; set; }

        /// <summary>
        /// 角色的Pda菜单权限列表
        /// </summary>
        public IEnumerable<long> PdaMenus { get; set; }

        /// <summary>
        /// 角色的Mfs菜单权限列表
        /// </summary>
        public IEnumerable<long> MfsMenus { get; set; }

        /// <summary>
        /// 功能权限列表
        /// </summary>
        public IEnumerable<long> Functions { get; set; }

        /// <summary>
        /// 数据权限列表
        /// </summary>
        public IEnumerable<long> Datas { get; set; }
    }
}
