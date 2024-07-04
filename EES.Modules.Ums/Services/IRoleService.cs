using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Services
{
    /// <summary>
    /// 角色查询服务
    /// </summary>
    public interface IRoleService : ITransientDependency
    {
        /// <summary>
        /// 获取角色详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QueryResponse<RoleDto>> GetRoleAsync(long id);

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isGetTotalCount"></param>
        /// <returns></returns>
        Task<QueryResponse<PaginationModel<RoleListDto>>> GetRolesAsync(string roleName, int pageSize, int pageIndex, bool isGetTotalCount = true);
    }
}
