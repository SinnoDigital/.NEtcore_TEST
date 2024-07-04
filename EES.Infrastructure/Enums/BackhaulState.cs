using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 回传状态流程
    /// </summary>
    public enum BackhaulState
    {
        产出确认中 = 0,
        待出锅 = 1,
        待录入 = 2,
        待回传 = 3,
        已回传 = 4
    }
}
