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
	/// 用户参数信息
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "ums_user_param")]
	public partial class UmsUserParam {

		/// <summary>
		/// 主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
		public long Id { get; set; }

		/// <summary>
		/// 修改前的值
		/// </summary>
		[JsonProperty, Column(Name = "before_value", StringLength = -2)]
		public string BeforeValue { get; set; }

		/// <summary>
		/// 参数编码
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
		/// 默认值
		/// </summary>
		[JsonProperty, Column(Name = "defalut_value", StringLength = -2)]
		public string DefalutValue { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		[JsonProperty, Column(Name = "description", StringLength = 512)]
		public string Description { get; set; }

		/// <summary>
		/// 是否启用
		/// </summary>
		[JsonProperty, Column(Name = "is_enable")]
		public bool IsEnable { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 128)]
		public string Name { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 512)]
		public string Remark { get; set; }

		/// <summary>
		/// 设定值
		/// </summary>
		[JsonProperty, Column(Name = "set_value", StringLength = -2)]
		public string SetValue { get; set; }

		/// <summary>
		/// 参数类型
		/// </summary>
		[JsonProperty, Column(Name = "type", StringLength = 128)]
		public string Type { get; set; }

		/// <summary>
		/// 参数单位
		/// </summary>
		[JsonProperty, Column(Name = "unit", StringLength = 64)]
		public string Unit { get; set; }

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

		/// <summary>
		/// 用户id
		/// </summary>
		[JsonProperty, Column(Name = "user_id")]
		public long UserId { get; set; }

		/// <summary>
		/// 值类型
		/// </summary>
		[JsonProperty, Column(Name = "value_type", StringLength = 512)]
		public string ValueType { get; set; }

	}

}
