using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 来源
    /// </summary>
    public enum Source
    {
        未定义 = 0,
        手工创建 = 1,
        ERP导入 = 2,
        Excel导入 = 4,
        系统创建 = 8,
    }
}
