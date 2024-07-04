using EES.Infrastructure.Modules;
using EES.Modules.Ums.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Data
{
    /// <summary>
    /// 模块初始化
    /// </summary>
    public class ModuleInitializer : IModuleInitializer
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            //注入各种 service ，仓储          
        }
    }
}
