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
	/// 组员
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "ums_team_members")]
	public partial class UmsTeamMembers {

		/// <summary>
		/// 数据的唯一ID
		/// </summary>
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
		/// 所属班组id
		/// </summary>
		[JsonProperty, Column(Name = "team_id")]
		public long TeamId { get; set; }

		/// <summary>
		/// 最后一次修改时间
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

		/// <summary>
		/// 用户ID
		/// </summary>
		[JsonProperty, Column(Name = "user_id")]
		public long UserId { get; set; }

	}

}
