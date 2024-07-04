using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Dto
{
    /// <summary>
    /// 角色模型
    /// </summary>
    public class ShortRoleInfoDto
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get;  set; }
    }

    /// <summary>
    /// 角色列表Dto
    /// </summary>
    public class RoleListDto
    {
        /// <summary>
        /// 角色id
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
    /// 角色详情Dto
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// 角色id
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
