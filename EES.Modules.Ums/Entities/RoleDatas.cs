﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using EES.Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace EES.Modules.Ums.Entities
{
    /// <summary>
    /// 角色数据权限
    /// </summary>
    public partial class RoleDatas : EntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="dataId"></param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        public RoleDatas(long roleId, long dataId, long createUserId, string createUserName) : base(createUserId, createUserName)
        {
            RoleId = roleId;
            DataId = dataId;
        }


        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; private set; }
        /// <summary>
        /// 数据权限id
        /// </summary>
        public long DataId { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        public virtual Entities.Data Data { get;  set; }

        /// <summary>
        /// 角色
        /// </summary>
        public virtual Role Role { get;  set; }


        /// <summary>
        /// 创建角色数据权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="dataIds">数据id集合</param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        /// <returns></returns>
        public static IEnumerable<RoleDatas> GenerateRoleDatas(long roleId, IEnumerable<long> dataIds, long createUserId, string createUserName)
        {
            foreach (var dataId in dataIds)
            {
                yield return new RoleDatas(roleId, dataId, createUserId, createUserName);
            }
        }

    }
}