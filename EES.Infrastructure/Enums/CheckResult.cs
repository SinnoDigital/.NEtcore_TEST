using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 质检结果
    /// </summary>
    public enum CheckResult
    {
        全部 = 0,
        未请检 = 1,
        已请检 = 2,
        全部合格 = 4,
        不合格 = 8,
        部分合格可出锅 = 16,
        特例放行 = 32
    }
}
