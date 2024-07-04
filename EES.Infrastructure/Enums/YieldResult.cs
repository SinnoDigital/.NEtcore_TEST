using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 平衡结论
    /// </summary>
    public enum YieldResult
    {
        未处理 = 0,
        符合平衡 = 1,
        不符合平衡 = 2,
        部分符合平衡 = 3
    }
}
