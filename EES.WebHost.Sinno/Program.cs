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

            //����NLog,��������ϵͳ�������һЩ�޹ؽ�Ҫ�������־
            builder.Logging.ClearProviders().AddConsole().SetMinimumLevel(LogLevel.Information).AddNLog("NLog.config");

            var logger = NLog.LogManager.GetCurrentClassLogger();

            ConfigureService(builder);

            var app = builder.Build();

            var appLeftTime = app.Services.GetService(typeof(IHostApplicationLifetime)) as IHostApplicationLifetime;

            appLeftTime?.ApplicationStopping.Register(() =>
            {

                logger.Info("�յ�ShutDownָ����򼴽��ر�....");
                Console.WriteLine("���򼴽�ShutDown����ȴ��������ִ��δ��ɵ�����,����Լ��Ҫ5-10��");
                NLog.LogManager.Shutdown();

                Thread.Sleep(5 * 1000); //�ӳ�����ر�
            });


            Configure(app, builder.Configuration, builder.Environment);

        }

        /// <summary>
        /// ����������ã���ɷ���ע��
        /// </summary>
        /// <param name="builder"></param>
        static void ConfigureService(WebApplicationBuilder builder)
        {
            builder.Services.AddJwt(); //Jwt����ע��

            GlobalConfiguration.WebRootPath = builder.Environment.WebRootPath;
            GlobalConfiguration.ContentRootPath = builder.Environment.ContentRootPath;
            var masterConnStr = builder.Configuration.GetConnectionString("MasterConnection");

            var logConnStr = builder.Configuration.GetConnectionString("LogConnection");

            var DbType = builder.Configuration.GetConnectionString("DbType");
            if (DbType == "MySql")
            {
                //ע����ҵ�����ݿ��DbContext
                builder.Services.AddDbContext<MasterDbContext>(options =>
                {
                    options.UseMySql(masterConnStr, ServerVersion.AutoDetect(masterConnStr));

                    if (builder.Environment.IsDevelopment())
                    {
                        options.EnableSensitiveDataLogging();
                    }

                    options.UseLoggerFactory(MasterDbContext.MasterLoggerFactory);

                });

                //ע����־���ݿ��DbContext
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

                //ע����־���ݿ��DbContext
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
                return new RedisClient(redisConnStr); //redis ����
            });


            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            builder.Services.AddModules(); //����ģ����Ϣ

            var moduleAssemblys = GlobalConfiguration.Modules.Select(x => x.Assembly);

            if (!moduleAssemblys.Any())
            {
                ArgumentException.ThrowIfNullOrEmpty(nameof(moduleAssemblys));
            }

            builder.Services.AddSingleton(_ => Channel.CreateBounded<LogEntityBase>(new BoundedChannelOptions(1024)
            {
                FullMode = BoundedChannelFullMode.Wait
            })); //ע����־��Ϣ�ܵ�,ֻ���ǵ���ע��

            builder.Services.AddSingleton<ILogDispatchProvider, LogChannelProvider>();

            builder.Services.AddHostedService<LogHandlerService>(); //ע����־�����̨����

            builder.Services.AddControllers(c =>
            {
                c.Conventions.Add(new ActionHidingConvention(moduleAssemblys)); //ע��ģ���Controller�� ����ģ���api�ĵ�
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
            builder.Services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>)); //ע��ִ�����

            builder.Services.AddScoped<IMediator, Mediator>();
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
            builder.Services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssemblies(moduleAssemblys.ToArray()!);
                config.NotificationPublisher = new ParallelNoWaitPublisher();
            }); //ɨ��ģ����򼯣�ע�����е�CommandHandler�� EventHandler
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>)); //ע��Mediator������ܵ�


            builder.Services.AddInterfaceInject(moduleAssemblys.ToArray()!);
            //����ģ��ĳ��򼯣�����ִ��ģ���IModuleInitializer�ӿڵ�ConfigureServices������ע��ģ������Ҫע������ݡ�
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

            //ʹ��knife4j
            builder.Services.AddKnife4jSwagger(builder.Configuration, GlobalConfiguration.Modules.Select(x => x.Id));


            builder.Services.AddAutoMapper(moduleAssemblys.ToArray()); //ʹ��AutoMapper.ɨ����򼯣���������ģ����̳���ProFile����

            // ���ÿ���������������Դ
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
                //��������Ĭ�ϸ�ʽ������
                setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //��ֵ����
                setting.NullValueHandling = NullValueHandling.Include;

                return setting;
            });


        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        static void Configure(WebApplication app, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (app.Environment.IsDevelopment()) //��ʽ����Ӧ�����ε�swaggerҳ��
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

            // �������п���
            app.UseCors("DefaultCors");


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<AccessorMiddleWare>();

            // ʹ����֤�м��
            app.UseMiddleware<ValidationMiddleWare>();

            app.MapControllers();

            var moduleInitializers = app.Services.GetServices<IModuleInitializer>();
            foreach (var moduleInitializer in moduleInitializers)
            {
                moduleInitializer.Configure(app, environment); //ִ�и���ģ���ڲ�������
            }

            app.Run();
        }

    }
}