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
    /// 组织部门查询接口
    /// </summary>
    public interface IDepartmentService : ITransientDependency
    {
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        Task<QueryResponse<DepartmentDto>> GetDepartmentAsync(long departmentId);

        /// <summary>
        /// 以树状结构返回指定组织部门以及其全部子部门信息
        /// </summary>
        /// <param name="rootId">部门Id</param>
        /// <returns></returns>
        Task<QueryResponse<IEnumerable<TreeItem<DepartmentDto>>>> GetDepartmentTreeAsync(long rootId = 0);
    }
}
