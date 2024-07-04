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
	/// 工作组信息
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "ums_team")]
	public partial class UmsTeam {

		/// <summary>
		/// 数据的唯一ID
		/// </summary>
		[JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
		public long Id { get; set; }

		/// <summary>
		/// 工作区域编码
		/// </summary>
		[JsonProperty, Column(Name = "area_code", StringLength = 128)]
		public string AreaCode { get; set; }

		/// <summary>
		/// 工作区域id
		/// </summary>
		[JsonProperty, Column(Name = "area_id")]
		public long AreaId { get; set; }

		/// <summary>
		/// 工作区域名称
		/// </summary>
		[JsonProperty, Column(Name = "area_name", StringLength = 128)]
		public string AreaName { get; set; }

		/// <summary>
		/// 工作组编码
		/// </summary>
		[JsonProperty, Column(Name = "code", StringLength = 128)]
		public string Code { get; set; }

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
		/// 描述
		/// </summary>
		[JsonProperty, Column(Name = "description", StringLength = 512)]
		public string Description { get; set; }

		/// <summary>
		/// 工厂编码
		/// </summary>
		[JsonProperty, Column(Name = "factory_code", StringLength = 128)]
		public string FactoryCode { get; set; }

		/// <summary>
		/// 工厂id
		/// </summary>
		[JsonProperty, Column(Name = "factory_id")]
		public long FactoryId { get; set; }

		/// <summary>
		/// 工厂名称
		/// </summary>
		[JsonProperty, Column(Name = "factory_name", StringLength = 128)]
		public string FactoryName { get; set; }

		/// <summary>
		/// 工作组名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 128)]
		public string Name { get; set; }

		/// <summary>
		/// 班组类型:  1车间  2仓库  3质量  4其他
		/// </summary>
		[JsonProperty, Column(Name = "type")]
		public int Type { get; set; }

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

	}

}
