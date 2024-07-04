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
    /// 用户服务接口
    /// </summary>
    public interface IUserService: ITransientDependency
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<QueryResponse<UserDto>> GetUserAsync(long userId);


        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<QueryResponse<PaginationModel<UserListDto>>> GetUsersAsync(UserListQueryParam queryParam);
    }
}
