using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 条码打印类型
    /// </summary>
    public enum BisPrintType
    {
        None = 0,
        条码打印 = 1,
        组成条码=2,
        条码复制=4,
        区间补打=8
    }
}
