using EES.Infrastructure.Data;
using EES.Infrastructure.Modules;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static readonly IModuleConfigurationManager _modulesConfig = new ModuleConfigurationManager();

        /// <summary>
        /// 加载模块信息
        /// </summary>
        /// <param name="services"></param>
        /// <returns>将模块信息绑定至GlobalConfiguration</returns>
        public static IServiceCollection AddModules(this IServiceCollection services)
        {
            foreach (var module in _modulesConfig.GetModules())
            {

                module.Assembly = Assembly.Load(new AssemblyName(module.Id));

                GlobalConfiguration.Modules.Add(module);
            }

            return services;
        }

        public static IServiceCollection AddCustomizedController(this IServiceCollection services, IList<ModuleInfo> modules)
        {
            var mvcBuilder = services.AddControllers();

            //foreach (var module in modules.Where(x => !x.IsBundledWithHost))
            //{
            //    AddApplicationPart(mvcBuilder, module.Assembly);
            //}

            return services;
        }

        /// <summary>
        /// 将未被引用的程序集加载到ApplicationParts
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <param name="assembly"></param>
        private static void AddApplicationPart(IMvcBuilder mvcBuilder, Assembly assembly)
        {
            var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
            foreach (var part in partFactory.GetApplicationParts(assembly))
            {
                mvcBuilder.PartManager.ApplicationParts.Add(part);
            }

            var relatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(assembly, throwOnError: false);
            foreach (var relatedAssembly in relatedAssemblies)
            {
                partFactory = ApplicationPartFactory.GetApplicationPartFactory(relatedAssembly);
                foreach (var part in partFactory.GetApplicationParts(relatedAssembly))
                {
                    mvcBuilder.PartManager.ApplicationParts.Add(part);
                }
            }
        }
        /// <summary>
        /// 尝试从模块的文件夹里面加载模块的dll
        /// </summary>
        /// <param name="moduleFolderPath">模块文件夹路径</param>
        /// <param name="module">模块信息</param>
        /// <param name="binariesFolderName">模块dll文件所在位置</param>
        /// <exception cref="Exception"></exception>
        /// <remarks>这个方法是为了防止模块需要被集成进来，但是WebHost没有引用这个模块。即webhost的bin文件夹下面没有这个模块的dll文件。
        /// 但是在.net 7的项目里面，模块编译后，dll的默认输出文件夹应该是"bin\Debug\net7.0"或者"bin\Release\net7.0"，也跟用户设置的输出位置有关系，需要手动指定</remarks>
        private static void TryLoadModuleAssembly(string moduleFolderPath, ModuleInfo module, string binariesFolderName = @"bin\Debug\net7.0")
        {
            //const string binariesFolderName = "bin";
            var binariesFolderPath = Path.Combine(moduleFolderPath, binariesFolderName);
            var binariesFolder = new DirectoryInfo(binariesFolderPath);

            if (Directory.Exists(binariesFolderPath))
            {
                foreach (var file in binariesFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly;
                    try
                    {
                        assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                    }
                    catch (FileLoadException)
                    {
                        // Get loaded assembly. This assembly might be loaded
                        assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                        if (assembly == null)
                        {
                            throw;
                        }

                        string loadedAssemblyVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                        string tryToLoadAssemblyVersion = FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;

                        // Or log the exception somewhere and don't add the module to list so that it will not be initialized
                        if (tryToLoadAssemblyVersion != loadedAssemblyVersion)
                        {
                            throw new Exception($"Cannot load {file.FullName} {tryToLoadAssemblyVersion} because {assembly.Location} {loadedAssemblyVersion} has been loaded");
                        }
                    }

                    if (Path.GetFileNameWithoutExtension(assembly.ManifestModule.Name) == module.Id)
                    {
                        module.Assembly = assembly;
                    }
                }
            }
        }
    }
}
