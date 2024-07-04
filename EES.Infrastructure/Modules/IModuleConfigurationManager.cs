using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Modules
{
    /// <summary>
    /// 模块配置管理
    /// </summary>
    public interface IModuleConfigurationManager
    {
        /// <summary>
        /// 获取所有模块
        /// </summary>
        /// <returns></returns>
        IEnumerable<ModuleInfo> GetModules();
    }
}
