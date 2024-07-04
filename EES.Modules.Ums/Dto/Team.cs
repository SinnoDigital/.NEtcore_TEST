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
    /// 工作组模型
    /// </summary>
    public class TeamDto
    {
        /// <summary>
        /// Ids
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工作组编码
        /// </summary>
        public string Code { get;  set; }
        /// <summary>
        /// 工作组名称
        /// </summary>
        public string Name { get;  set; }
        /// <summary>
        /// 班组类型
        /// </summary>
        public TeamType Type { get;  set; }
        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get;  set; }
        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get;  set; }
        /// <summary>
        /// 工厂id
        /// </summary>
        public long FactoryId { get;  set; }
        /// <summary>
        /// 工作区域编码
        /// </summary>
        public string AreaCode { get;  set; }
        /// <summary>
        /// 工作区域名称
        /// </summary>
        public string AreaName { get;  set; }
        /// <summary>
        /// 工作区域id
        /// </summary>
        public long AreaId { get;  set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get;  set; }

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
        /// 组员
        /// </summary>
        public IEnumerable<TeamUserDto> Users { get; set; }
    }

    /// <summary>
    /// 工作组列表数据模型
    /// </summary>
    public class TeamListDto
    {
        /// <summary>
        /// Ids
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工作组编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 工作组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 班组类型
        /// </summary>
        public TeamType Type { get; set; }
        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }
        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }
        /// <summary>
        /// 工厂id
        /// </summary>
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
    /// 工作组的用户模型
    /// </summary>
    public class TeamUserDto
    { 

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户账户
        /// </summary>
        public string Account { get; set; }

        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 角色名(角色1,角色2)
        /// </summary>
        public string RoleNames { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserState State { get;  set; }
    }

    /// <summary>
    /// 工作组列表查询参数模型
    /// </summary>
    public class TeamListQueryParams
    { 
        /// <summary>
        /// 所属工厂
        /// </summary>
        public IEnumerable<long> FactoryIds { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 工作组名称
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 是否需要返回总数量
        /// </summary>
        public bool IsGetTotalCount { get; set; } = true;
    }
}
