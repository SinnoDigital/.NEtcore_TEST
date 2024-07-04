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
	/// 功能资源表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "ums_function")]
	public partial class UmsFunction {

		[JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
		public long Id { get; set; }

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
		/// 功能说明
		/// </summary>
		[JsonProperty, Column(Name = "description", StringLength = 128)]
		public string Description { get; set; }

		/// <summary>
		/// 功能权限标识符
		/// </summary>
		[JsonProperty, Column(Name = "identifier", StringLength = 128)]
		public string Identifier { get; set; }

		/// <summary>
		/// 名字
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 128)]
		public string Name { get; set; }

		/// <summary>
		/// 父级id
		/// </summary>
		[JsonProperty, Column(Name = "parent_id")]
		public long ParentId { get; set; }

		/// <summary>
		/// 标识 1: 分类标识   2：功能
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
