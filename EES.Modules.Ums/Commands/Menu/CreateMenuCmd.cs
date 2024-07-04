using EES.Infrastructure.Bus;
using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Menu
{
    /// <summary>
    /// 创建菜单资源
    /// </summary>
    public class CreateMenuCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateMenuCmd() : base() { }
      
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 上级菜单
        /// </summary>
        [Required]
        public long ParentId { get; set; }
        /// <summary>
        /// 同级别下显示排序
        /// </summary>
        public int SortNumber { get; set; }
        /// <summary>
        /// 菜单路由
        /// </summary>
        [Required]
        public string Route { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        [Required]
        public MenuType Type { get; set ; }


        /// <summary>
        /// 菜单的类别(导航分类 or 具体的页面)
        /// </summary>
        [Required]
        public MenuCategory Category { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get;  set; }

        /// <summary>
        /// 菜单的英文名称
        /// </summary>
        public string EnglishName { get;  set; }
    }
}
