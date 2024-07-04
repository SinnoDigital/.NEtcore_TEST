using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 物料类型
    /// </summary>
    public enum MaterialType
    {
        未定义 = 0,
        原料 = 1,
        包材 = 2,
        制造半成品 = 4,
        灌装半成品 = 8,
        成品 = 16,
        辅料 = 32,
        备品备件 = 64
    }
}
