using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 基础物料枚举
    /// </summary>
    public class BaseMaterialEnums
    {
        /// <summary>
        /// 储存方式
        /// </summary>
        public enum StorageMode
        {
            /// <summary>
            /// 默认
            /// </summary>
            [Description("默认")]
            默认 = 0,
            /// <summary>
            /// 常温
            /// </summary>
            [Description("常温")]
            常温 = 1,
            /// <summary>
            /// 冷藏
            /// </summary>
            [Description("冷藏")]
            冷藏 = 2,
            /// <summary>
            /// 冷冻
            /// </summary>
            [Description("冷冻")]
            冷冻 = 3
        }
        /// <summary>
        /// 数据来源
        /// </summary>
        public enum DataSources
        {
            /// <summary>
            /// 手动创建
            /// </summary>
            [Description("手动创建")]
            CreateManually = 0,
            /// <summary>
            /// 第三方同步
            /// </summary>
            [Description("第三方同步")]
            ThirdPartySync = 1,
            /// <summary>
            /// Excel导入
            /// </summary>
            [Description("Excel导入")]
            ExcelInport = 2
        }
    }
}
