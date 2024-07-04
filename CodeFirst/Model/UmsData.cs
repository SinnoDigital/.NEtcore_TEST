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
	/// 数据权限资源表- 数据来自pem模块
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "ums_data")]
	public partial class UmsData {

		[JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
		public long Id { get; set; }

		/// <summary>
		/// 1 工厂车间  2：仓库  3：物料类型   4:车间
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
		/// 对应数据的编码
		/// </summary>
		[JsonProperty, Column(Name = "object_code", StringLength = 128)]
		public string ObjectCode { get; set; }

		/// <summary>
		/// 对应的数据id
		/// </summary>
		[JsonProperty, Column(Name = "object_id")]
		public long ObjectId { get; set; }

		/// <summary>
		/// 数据显示名字
		/// </summary>
		[JsonProperty, Column(Name = "object_name", StringLength = 128)]
		public string ObjectName { get; set; }

		/// <summary>
		/// 所属上级的id。 对应到object_id。工厂的上级id为0，车间，仓库的上级id填写所属工厂的id。
		/// </summary>
		[JsonProperty, Column(Name = "parent_id")]
		public long ParentId { get; set; }

		/// <summary>
		/// 最后一次更新时间
		/// </summary>
		[JsonProperty, Column(Name = "update_time", DbType = "datetime2")]
		public DateTime UpdateTime { get; set; }

		/// <summary>
		/// 最后一次更新数据的用户Id
		/// </summary>
		[JsonProperty, Column(Name = "update_user_id")]
		public long UpdateUserId { get; set; }

		/// <summary>
		/// 最后一次更新用户的名称
		/// </summary>
		[JsonProperty, Column(Name = "update_user_name", StringLength = 128)]
		public string UpdateUserName { get; set; }

        /// <summary>
        /// 对应车间的类型
        /// </summary>
        [JsonProperty, Column(Name = "object_type")]
        public int ObjectType { get; set; } = 0;

    }

}
