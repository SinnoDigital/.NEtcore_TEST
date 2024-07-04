using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Auth
{
    public class AuthMenuItem
    {
        /// <summary>
        /// id
        /// </summary>

        public long Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 上级菜单
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 同级别下显示排序
        /// </summary>
        public int SortNumber { get; set; }
        /// <summary>
        /// 菜单路由
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType Type { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 菜单的英文名称
        /// </summary>
        public string EnglishName { get; set; }
    }
}
