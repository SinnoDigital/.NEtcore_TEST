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
	/// 用户信息
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "ums_user")]
	public partial class UmsUser {

		/// <summary>
		/// 数据的唯一ID
		/// </summary>
		[JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
		public long Id { get; set; }

		/// <summary>
		/// 用户账户
		/// </summary>
		[JsonProperty, Column(Name = "account", StringLength = 64)]
		public string Account { get; set; }

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
		/// 部门ID
		/// </summary>
		[JsonProperty, Column(Name = "departmen_id")]
		public long DepartmenId { get; set; }

		/// <summary>
		/// 用户照片的url
		/// </summary>
		[JsonProperty, Column(Name = "image_url", StringLength = 512)]
		public string ImageUrl { get; set; }

		/// <summary>
		/// 用户系统语言标识
		/// </summary>
		[JsonProperty, Column(Name = "language")]
		public int Language { get; set; }

		/// <summary>
		/// 用户邮箱
		/// </summary>
		[JsonProperty, Column(Name = "mail", StringLength = 128)]
		public string Mail { get; set; }

		/// <summary>
		/// 用户名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 128)]
		public string Name { get; set; }

		/// <summary>
		/// 用户密码密文
		/// </summary>
		[JsonProperty, Column(Name = "password", StringLength = 128)]
		public string Password { get; set; }

		/// <summary>
		/// 用户电话
		/// </summary>
		[JsonProperty, Column(Name = "phone", StringLength = 64)]
		public string Phone { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 512)]
		public string Remark { get; set; }

		/// <summary>
		/// 密码盐
		/// </summary>
		[JsonProperty, Column(Name = "salt", StringLength = 128)]
		public string Salt { get; set; }

		/// <summary>
		/// 用户签名的url
		/// </summary>
		[JsonProperty, Column(Name = "signature_url", StringLength = 512)]
		public string SignatureUrl { get; set; }

		/// <summary>
		/// 用户状态
		/// </summary>
		[JsonProperty, Column(Name = "state")]
		public int State { get; set; }

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
