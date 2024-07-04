using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EES.Infrastructure.Modules
{
    public class ModuleInfo
    {
        /// <summary>
        /// 模块ID(程序集名称)
        /// </summary>
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [JsonPropertyName("version")]
        public required string Version { get; set; }

        /// <summary>
        /// 程序集
        /// </summary>
        [JsonIgnore]
        public Assembly? Assembly { get; set; }
    }
}
