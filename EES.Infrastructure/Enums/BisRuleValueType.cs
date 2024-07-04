using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 配置规则内容类型
    /// </summary>
    public enum BisRuleValueType
    {
        None=0,
        指定值=1,
        流水号=2,
        验证码=4,
        其他数据源=8
    }
}
