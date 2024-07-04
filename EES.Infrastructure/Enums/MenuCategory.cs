using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Enums
{
    /// <summary>
    /// 菜单的类别
    /// </summary>
    public enum MenuCategory
    {

        /// <summary>
        /// 未知
        /// </summary>
        None = 0,
        /// <summary>
        /// 导航分类
        /// </summary>
        Navigation = 1,

        /// <summary>
        /// 具体的页面
        /// </summary>
        Page = 2,
    }
}
