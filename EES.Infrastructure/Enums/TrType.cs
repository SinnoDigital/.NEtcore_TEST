using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 工艺类型
    /// </summary>
    public enum TrType
    {
        未定义 = 0,
        称量 = 1,
        制作 = 2,
        灌装 = 4,
        包装 = 8,
        灌包一体灌 = 16,
        灌包一体包 = 32,
        CIP = 64,
        消毒 = 128,
    }
}
