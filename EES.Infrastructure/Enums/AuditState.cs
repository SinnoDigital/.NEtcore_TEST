using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditState
    {
        未定义 = 0,
        编辑中 = 1,
        提交审核 = 2,
        审核批准 = 4,
        已驳回 = 8,
        已反审 = 16
    }
}
