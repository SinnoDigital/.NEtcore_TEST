using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 打印状态
    /// </summary>
    public enum BisPrintState
    {
        None = 0,
        未打印 = 1,
        打印中 = 2,
        已打印 = 4
    }
}
