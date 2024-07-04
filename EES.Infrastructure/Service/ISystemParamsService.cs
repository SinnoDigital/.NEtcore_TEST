using EES.Infrastructure.Data;
using EES.Infrastructure.Enums;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Service
{
    /// <summary>
    /// 系统参数查询服务
    /// </summary>
    public interface ISystemParamsService : ITransientDependency
    {

        /// <summary>
        /// 获取模块下的全部配置
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="isIncludeDisabled">是否包含已经被禁用的</param>
        /// <returns></returns>
        Task<IEnumerable<SystemParamModel>> GetSystemParamModelsAsync(SystemModule module, bool isIncludeDisabled = false);

        /// <summary>
        /// 查询模块下特定的配置信息
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        Task<SystemParamModel> GetSystemParamModelAsync(SystemModule module, string code);
    }
}
