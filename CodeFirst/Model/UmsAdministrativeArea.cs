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
	/// 行政区域
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "ums_administrative_area")]
	public partial class UmsAdministrativeArea {

		/// <summary>
		/// ID
		/// </summary>
		[JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
		public long Id { get; set; }

		/// <summary>
		/// 行政区域编号
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
		[JsonProperty, Column(Name = "create_user_name", StringLength = 64)]
		public string CreateUserName { get; set; }

		/// <summary>
		/// 所属工厂编号
		/// </summary>
		[JsonProperty, Column(Name = "factory_code", StringLength = 128)]
		public string FactoryCode { get; set; }

		/// <summary>
		/// 所属工厂id
		/// </summary>
		[JsonProperty, Column(Name = "factory_id")]
		public long FactoryId { get; set; }

		/// <summary>
		/// 所属工厂名称
		/// </summary>
		[JsonProperty, Column(Name = "factory_name", StringLength = 64)]
		public string FactoryName { get; set; }

		/// <summary>
		/// 当前行政区域编制人数
		/// </summary>
		[JsonProperty, Column(Name = "headcount")]
		public int Headcount { get; set; }

		/// <summary>
		/// 是否启用
		/// </summary>
		[JsonProperty, Column(Name = "is_enable")]
		public bool IsEnable { get; set; }

		/// <summary>
		/// 行政区域名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 64)]
		public string Name { get; set; }

		/// <summary>
		/// 序号
		/// </summary>
		[JsonProperty, Column(Name = "number")]
		public int Number { get; set; }

		/// <summary>
		/// 当前行政区域已启用人数
		/// </summary>
		[JsonProperty, Column(Name = "real_number")]
		public int RealNumber { get; set; }

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
		[JsonProperty, Column(Name = "update_user_name", StringLength = 64)]
		public string UpdateUserName { get; set; }

	}

}
