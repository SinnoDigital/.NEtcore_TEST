﻿using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace LinCms.Core.Entities {

	/// <summary>
	/// 部门信息
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "ums_department")]
	public partial class UmsDepartment {

		/// <summary>
		/// 数据的唯一ID
		/// </summary>
		[JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
		public long Id { get; set; }

		/// <summary>
		/// 代码
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
		/// 图标的url
		/// </summary>
		[JsonProperty, Column(Name = "image_url", StringLength = 512)]
		public string ImageUrl { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 128)]
		public string Name { get; set; }

		/// <summary>
		/// 数据的父级ID
		/// </summary>
		[JsonProperty, Column(Name = "parent_id")]
		public long ParentId { get; set; }

		/// <summary>
		/// 序号
		/// </summary>
		[JsonProperty, Column(Name = "sort_number")]
		public int SortNumber { get; set; }

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
		/// 最后一次修者名称
		/// </summary>
		[JsonProperty, Column(Name = "update_user_name", StringLength = 128)]
		public string UpdateUserName { get; set; }

	}

}
