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
    /// 权限资源查询服务
    /// </summary>
    public interface IResourceService : ITransientDependency
    {
        /// <summary>
        /// 获取菜单详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QueryResponse<MenuDto>> GetMenuAsync(long id);


        /// <summary>
        /// 以树状结构返回指定资源以及其全部子资源信息
        /// </summary>
        /// <param name="menuType"></param>
        /// <param name="rootId"></param>
        /// <returns></returns>
        Task<QueryResponse<IEnumerable<TreeItem<MenuDto>>>> GetMenuTreeAsync(MenuType menuType, long rootId = 0);

        /// <summary>
        /// 获取功能权限资源详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QueryResponse<FunctionDto>> GetFunctionAsync(long id);


        /// <summary>
        /// 获取功能权限资源树
        /// </summary>
        /// <returns></returns>
        Task<QueryResponse<IEnumerable<TreeItem<FunctionDto>>>> GetFunctionTreeAsync();


        /// <summary>
        /// 获取数据权限资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QueryResponse<DataDto>> GetDataAsync(long id);


        /// <summary>
        /// 获取功能权限资源树
        /// </summary>
        /// <returns></returns>
        Task<QueryResponse<AllDataDto>> GetDataTreeAsync();
    }
}
