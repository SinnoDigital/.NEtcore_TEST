using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Infrastructure.Enums;
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
    /// 系统参数和用户参数查询接口
    /// </summary>
    public interface IParamService : ITransientDependency
    {
        /// <summary>
        /// 根据ID获取当前用户的用户参数信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QueryResponse<UserParamDto>> GetUserParamAsync(long id);

        /// <summary>
        /// 根据类型和编码获取当前用户的用户参数信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<QueryResponse<UserParamDto>> GetUserParamAsync(string type, string code);


        /// <summary>
        /// 获取用户全部的用户参数信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="isGetTotalCount">是否返回总数量的信息</param>
        /// <returns></returns>
        Task<QueryResponse<PaginationModel<UserParamDto>>> GetUserParamsAsync(int pageIndex, int PageSize, bool isGetTotalCount = true);



        /// <summary>
        /// 根据ID获取系统参数信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QueryResponse<SystemParamDto>> GetSystemParamAsync(long id);

        /// <summary>
        /// 根据模块和编码获系统参数信息
        /// </summary>
        /// <param name="module"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<QueryResponse<SystemParamDto>> GetSystemParamAsync(SystemModule module, string code);


        /// <summary>
        /// 获取全部的系统参数(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="isGetTotalCount">是否返回总数量的信息</param>
        /// <returns></returns>
        Task<QueryResponse<PaginationModel<SystemParamDto>>> GetSystemParamsAsync(int pageIndex, int PageSize, bool isGetTotalCount = true);

    }
}
