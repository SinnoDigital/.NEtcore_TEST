using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace LinCms.Core.Entities {

	/// <summary>
	/// 系统菜单资源表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "ums_menu")]
	public partial class UmsMenu {

		[JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
		public long Id { get; set; }

		/// <summary>
		/// 菜单的类别：1 导航分类 2：页面
		/// </summary>
		[JsonProperty, Column(Name = "category")]
		public int Category { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty, Column(Name = "create_time", DbType = "datetime2")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 创建者id
		/// </summary>
		[JsonProperty, Column(Name = "create_user_id")]
		public long CreateUserId { get; set; }

		/// <summary>
		/// 创建者名称
		/// </summary>
		[JsonProperty, Column(Name = "create_user_name", StringLength = 128)]
		public string CreateUserName { get; set; }

		/// <summary>
		/// 备注说明
		/// </summary>
		[JsonProperty, Column(Name = "description", StringLength = 512)]
		public string Description { get; set; }

		/// <summary>
		/// 菜单的英文名称
		/// </summary>
		[JsonProperty, Column(Name = "english_name", StringLength = 128)]
		public string EnglishName { get; set; }

		/// <summary>
		/// 图标
		/// </summary>
		[JsonProperty, Column(Name = "icon", StringLength = 128)]
		public string Icon { get; set; }

		/// <summary>
		/// 菜单名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 128)]
		public string Name { get; set; }

		/// <summary>
		/// 上级菜单
		/// </summary>
		[JsonProperty, Column(Name = "parent_id")]
		public long ParentId { get; set; }

		/// <summary>
		/// 菜单路由
		/// </summary>
		[JsonProperty, Column(Name = "route", StringLength = 128)]
		public string Route { get; set; }

		/// <summary>
		/// 同级别下显示排序
		/// </summary>
		[JsonProperty, Column(Name = "sort_number")]
		public int SortNumber { get; set; }

		/// <summary>
		/// 菜单类型: 1 PC菜单， 2：PDA菜单 
		/// </summary>
		[JsonProperty, Column(Name = "type")]
		public int Type { get; set; }

		/// <summary>
		/// 最后一次修时间
		/// </summary>
		[JsonProperty, Column(Name = "update_time", DbType = "datetime2")]
		public DateTime UpdateTime { get; set; }

		/// <summary>
		/// 最后一次修改者id
		/// </summary>
		[JsonProperty, Column(Name = "update_user_id")]
		public long UpdateUserId { get; set; }

		/// <summary>
		/// 最后一次修改者名称
		/// </summary>
		[JsonProperty, Column(Name = "update_user_name", StringLength = 128)]
		public string UpdateUserName { get; set; }

	}

}
