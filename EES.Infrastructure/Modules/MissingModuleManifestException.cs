using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Modules
{
    /// <summary>
    /// 模块缺失异常
    /// </summary>
    public sealed class MissingModuleManifestException : Exception
    {
        public required string ModuleName { get; set; }

        public MissingModuleManifestException()
        {

        }

        public MissingModuleManifestException(string message)
            : base(message)
        {

        }

        public MissingModuleManifestException(string message, string moduleName)
            : this(message)
        {
            ModuleName = moduleName;
        }
    }
}
