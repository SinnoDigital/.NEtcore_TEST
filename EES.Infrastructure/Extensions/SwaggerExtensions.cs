using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Swagger扩展
    /// </summary>
    public static class SwaggerExtensions
    {

        private readonly static string _endPointUrlFormat = @"/swagger/{0}/swagger.json";

        /// <summary>
        /// 注册Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddKnife4jSwagger(this IServiceCollection services,
            IConfiguration configuration, string section = "Swagger"
            )
        {
            SwaggerOption options = new();
            configuration.GetSection(section).Bind(options);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(options.Name, options.Info);
                foreach (var file in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
                {
                    c.IncludeXmlComments(file, true);
                }

                c.AddServer(new OpenApiServer()
                {
                    Url = "",
                    Description = "vvv"
                });
                c.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction?.ControllerName + "-" + controllerAction?.ActionName;
                });

                /* 这两个搭配使用，是的枚举类型的参数能正确显示注释*/
                c.SchemaFilter<EnumSchemaFilter>();
                c.UseInlineDefinitionsForEnums();

            });

            services.AddRouting(o => o.LowercaseUrls = true);

            return services;
        }



        /// <summary>
        /// 根据配置的模块加载Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置</param>
        /// <param name="xmlNames">配置的模块的xml文件名称集合</param>
        /// <param name="section">配置项</param>
        /// <returns></returns>
        public static IServiceCollection AddKnife4jSwagger(this IServiceCollection services,
            IConfiguration configuration, IEnumerable<string> xmlNames, string section = "Swagger"
            )
        {
            List<SwaggerOption> options = new();
            configuration.GetSection(section).Bind(options);



            services.AddSwaggerGen(c =>
            {
                foreach (var option in options)
                {
                    c.SwaggerDoc(option.Name, option.Info);
                }


                foreach (var filePath in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
                {
                    var fileFullName = (filePath.Split('\\'))[^1]; //xxx.xml

                    if (xmlNames == null || fileFullName.Contains("WebHost") || fileFullName.Contains("Share") || fileFullName.Contains("Infrastructure") || xmlNames.Any(t => fileFullName.Contains(t)))
                    {
                        c.IncludeXmlComments(filePath, true);
                    }
                }

                c.AddServer(new OpenApiServer()
                {
                    Url = "",
                    Description = "vvv"
                });
                c.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction?.ControllerName + "-" + controllerAction?.ActionName;
                });


                /* 这两个搭配使用，是的枚举类型的参数能正确显示注释*/
                c.SchemaFilter<EnumSchemaFilter>();
                c.UseInlineDefinitionsForEnums();

            });

            services.AddRouting(o => o.LowercaseUrls = true);
            services.AddRouting(o => o.SuppressCheckForUnhandledSecurityMetadata = true);
            return services;
        }



        /// <summary>
        /// 使用UseKnife4jUI
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseKnife4jSwagger(this IApplicationBuilder app, IConfiguration configuration, string section = "Swagger")
        {

            List<SwaggerOption> options = new();
            configuration.GetSection(section).Bind(options);
            // 添加Swagger有关中间件
            app.UseSwagger();

            app.UseKnife4UI(c =>
            {
                c.RoutePrefix = ""; // serve the UI at root

                foreach (var option in options)
                {
                    string endPointUel = string.IsNullOrWhiteSpace(option.EndPointUrl)
                                       ? string.Format(_endPointUrlFormat, option.Name)
                                       : option.EndPointUrl;
                    c.SwaggerEndpoint(endPointUel, option.Info.Title);
                }

            });
            return app;
        }


        /// <summary>
        /// 让不在特定程序集的controller在API文档不显示(实际控制器已经加载至程序内)
        /// </summary>
        public class ActionHidingConvention : IActionModelConvention
        {
            private readonly IEnumerable<Assembly> assemblys;

            public ActionHidingConvention(IEnumerable<Assembly> assemblys)
            {
                this.assemblys = assemblys;
            }

            public void Apply(ActionModel action)
            {
                var name = action.Controller.ControllerType.Assembly.GetName().Name;

                if (!name!.Contains("WebHost") && !assemblys.Any(t => t.GetName().Name == name))
                {
                    action.ApiExplorer.IsVisible = false;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        internal class SwaggerOption
        {
            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public OpenApiInfo Info { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string EndPointUrl { get; set; }
        }



        /// <summary>
        /// Swagger文档枚举字段显示枚举属性和枚举值,以及枚举描述
        /// </summary>
        public class EnumSchemaFilter : ISchemaFilter
        {
            /// <summary>
            /// 实现接口
            /// </summary>
            /// <param name="model"></param>
            /// <param name="context"></param>

            public void Apply(OpenApiSchema model, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    model.Enum.Clear();
                    Enum.GetNames(context.Type)
                        .ToList()
                        .ForEach(name =>
                        {
                            Enum e = (Enum)Enum.Parse(context.Type, name);
                            model.Enum.Add(new OpenApiString($"<br>{name} : {Convert.ToInt64(Enum.Parse(context.Type, name))} "));
                        });
                }
            }

        }

    }
}
