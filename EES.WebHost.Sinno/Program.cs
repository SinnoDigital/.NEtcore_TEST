using EES.Infrastructure.Bus;
using EES.Infrastructure.Data;
using EES.Infrastructure.DataBase;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Middleware;
using EES.Infrastructure.Modules;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using NLog.Web;
using System.Text.Json;
using static Microsoft.Extensions.DependencyInjection.SwaggerExtensions;
using Microsoft.Extensions.FileProviders;
using FreeRedis;
using EES.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading.Channels;
using EES.Infrastructure.Entities;
using EES.Infrastructure.Service;
using Microsoft.Extensions.Logging;

namespace EES.WebHost.Sinno
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseShutdownTimeout(TimeSpan.FromSeconds(10));

            //启用NLog,并且屏蔽系统框架里面一些无关紧要的输出日志
            builder.Logging.ClearProviders().AddConsole().SetMinimumLevel(LogLevel.Information).AddNLog("NLog.config");

            var logger = NLog.LogManager.GetCurrentClassLogger();

            ConfigureService(builder);

            var app = builder.Build();

            var appLeftTime = app.Services.GetService(typeof(IHostApplicationLifetime)) as IHostApplicationLifetime;

            appLeftTime?.ApplicationStopping.Register(() =>
            {

                logger.Info("收到ShutDown指令，程序即将关闭....");
                Console.WriteLine("程序即将ShutDown，请等待程序继续执行未完成的任务,过程约需要5-10秒");
                NLog.LogManager.Shutdown();

                Thread.Sleep(5 * 1000); //延迟五秒关闭
            });


            Configure(app, builder.Configuration, builder.Environment);

        }

        /// <summary>
        /// 设置相关配置，完成服务注册
        /// </summary>
        /// <param name="builder"></param>
        static void ConfigureService(WebApplicationBuilder builder)
        {
            builder.Services.AddJwt(); //Jwt服务注入

            GlobalConfiguration.WebRootPath = builder.Environment.WebRootPath;
            GlobalConfiguration.ContentRootPath = builder.Environment.ContentRootPath;
            var masterConnStr = builder.Configuration.GetConnectionString("MasterConnection");

            var logConnStr = builder.Configuration.GetConnectionString("LogConnection");

            var DbType = builder.Configuration.GetConnectionString("DbType");
            if (DbType == "MySql")
            {
                //注入主业务数据库的DbContext
                builder.Services.AddDbContext<MasterDbContext>(options =>
                {
                    options.UseMySql(masterConnStr, ServerVersion.AutoDetect(masterConnStr));

                    if (builder.Environment.IsDevelopment())
                    {
                        options.EnableSensitiveDataLogging();
                    }

                    options.UseLoggerFactory(MasterDbContext.MasterLoggerFactory);

                });

                //注入日志数据库的DbContext
                builder.Services.AddDbContext<LogDbContext>(options =>
                {
                    options.UseMySql(logConnStr, ServerVersion.AutoDetect(logConnStr));
                });
            }
            else if (DbType == "SqlServer")
            {
                builder.Services.AddDbContext<MasterDbContext>(options =>
                {
                    options.UseSqlServer(masterConnStr);

                    if (builder.Environment.IsDevelopment())
                    {
                        options.EnableSensitiveDataLogging();
                    }
                    options.UseLoggerFactory(MasterDbContext.MasterLoggerFactory);
                });

                //注入日志数据库的DbContext
                builder.Services.AddDbContext<LogDbContext>(options =>
                {
                    options.UseSqlServer(logConnStr);
                });
            }
            //EDI
            builder.Services.Configure<EDIConnection>(builder.Configuration.GetSection("EDIConnectionStrings"));

            var redisConnStr = builder.Configuration.GetConnectionString("RedisConnection");

            builder.Services.AddSingleton(provider =>
            {
                return new RedisClient(redisConnStr); //redis 配置
            });


            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            builder.Services.AddModules(); //加载模块信息

            var moduleAssemblys = GlobalConfiguration.Modules.Select(x => x.Assembly);

            if (!moduleAssemblys.Any())
            {
                ArgumentException.ThrowIfNullOrEmpty(nameof(moduleAssemblys));
            }

            builder.Services.AddSingleton(_ => Channel.CreateBounded<LogEntityBase>(new BoundedChannelOptions(1024)
            {
                FullMode = BoundedChannelFullMode.Wait
            })); //注入日志信息管道,只能是单例注入

            builder.Services.AddSingleton<ILogDispatchProvider, LogChannelProvider>();

            builder.Services.AddHostedService<LogHandlerService>(); //注入日志处理后台服务

            builder.Services.AddControllers(c =>
            {
                c.Conventions.Add(new ActionHidingConvention(moduleAssemblys)); //注册模块的Controller， 处理模块的api文档
                c.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                c.Filters.Add<GlobalAuthorizationFilter>();
                c.Filters.Add<IdempotentAsyncFilter>(5);
                c.Filters.Add<RequestRateLimitFilter>(2);
            })
            .AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
            });

            builder.Services.AddHttpClient();

            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>)); //注入仓储基类

            builder.Services.AddScoped<IMediator, Mediator>();
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
            builder.Services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssemblies(moduleAssemblys.ToArray()!);
                config.NotificationPublisher = new ParallelNoWaitPublisher();
            }); //扫描模块程序集，注册所有的CommandHandler和 EventHandler
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>)); //注册Mediator的事务管道


            builder.Services.AddInterfaceInject(moduleAssemblys.ToArray()!);
            //遍历模块的程序集，反射执行模块的IModuleInitializer接口的ConfigureServices方法，注入模块内需要注入的内容。
            foreach (var module in GlobalConfiguration.Modules)
            {
                var moduleInitializerType = module.Assembly!.GetTypes()
                   .FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
                if ((moduleInitializerType != null) && (moduleInitializerType != typeof(IModuleInitializer)))
                {
                    var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType)!;
                    builder.Services.AddSingleton(typeof(IModuleInitializer), moduleInitializer);
                    moduleInitializer.ConfigureServices(builder.Services);
                }
            }

            //使用knife4j
            builder.Services.AddKnife4jSwagger(builder.Configuration, GlobalConfiguration.Modules.Select(x => x.Id));


            builder.Services.AddAutoMapper(moduleAssemblys.ToArray()); //使用AutoMapper.扫描程序集，加载所有模块里继承了ProFile的类

            // 配置跨域处理，允许所有来源
            builder.Services.AddCors(options =>
            options.AddPolicy("DefaultCors",
            p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            JsonSerializerSettings setting = new()
            {
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                //日期类型默认格式化处理
                setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //空值处理
                setting.NullValueHandling = NullValueHandling.Include;

                return setting;
            });


        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        static void Configure(WebApplication app, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (app.Environment.IsDevelopment()) //正式环境应该屏蔽掉swagger页面
            {
                app.UseKnife4jSwagger(configuration);
            }

            app.UseMiddleware<LogMiddleWare>();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {

                FileProvider = new PhysicalFileProvider(Path.Combine(environment.ContentRootPath, "Files")),
                RequestPath = "/static",
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    c.Context.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
                }
            });


            app.UseRouting();

            // 允许所有跨域
            app.UseCors("DefaultCors");


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<AccessorMiddleWare>();

            // 使用验证中间件
            app.UseMiddleware<ValidationMiddleWare>();

            app.MapControllers();

            var moduleInitializers = app.Services.GetServices<IModuleInitializer>();
            foreach (var moduleInitializer in moduleInitializers)
            {
                moduleInitializer.Configure(app, environment); //执行各个模块内部的配置
            }

            app.Run();
        }

    }
}