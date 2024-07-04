using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuDto
    {
        /// <summary>
        /// id
        /// </summary>

        public long Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get;  set; }
        /// <summary>
        /// 上级菜单
        /// </summary>
        public long ParentId { get;  set; }
        /// <summary>
        /// 同级别下显示排序
        /// </summary>
        public int SortNumber { get;  set; }
        /// <summary>
        /// 菜单路由
        /// </summary>
        public string Route { get;  set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType Type { get; set; }


        /// <summary>
        /// 菜单的类别(导航分类 or 具体的页面)
        /// </summary>
        public MenuCategory Category { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Description { get;  set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 菜单的英文名称
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建者Id
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 最后一次更新用户的名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
