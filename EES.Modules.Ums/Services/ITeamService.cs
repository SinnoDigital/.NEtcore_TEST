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
    /// 工作组查询服务
    /// </summary>
    public interface ITeamService: ITransientDependency
    {
        /// <summary>
        /// 查询工作组详细数据
        /// </summary>
        /// <param name="id">工作组id</param>
        /// <returns></returns>
        Task<QueryResponse<TeamDto>> GetTeamAsync(long id);

        /// <summary>
        /// 工作组列表查询
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns></returns>
        Task<QueryResponse<PaginationModel<TeamListDto>>> GetTeamsAsync(TeamListQueryParams queryParams);
    }
}
