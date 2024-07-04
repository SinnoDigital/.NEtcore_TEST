﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using EES.Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace EES.Modules.Ums.Entities
{
    /// <summary>
    /// 角色功能权限表
    /// </summary>
    public partial class RoleFunctions: EntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="functionId"></param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        public RoleFunctions(long roleId, long functionId, long createUserId, string createUserName):base(createUserId, createUserName)
        {
            RoleId = roleId;
            FunctionId = functionId;
        }



        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; private set; }
        /// <summary>
        /// 功能id
        /// </summary>
        public long FunctionId { get; private set; }

        /// <summary>
        /// 角色
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// 创建角色功能权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="functionIds">功能id集合</param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        /// <returns></returns>
        public static IEnumerable<RoleFunctions> GenerateRoleFunctions(long roleId, IEnumerable<long> functionIds, long createUserId, string createUserName)
        {
            foreach (var item in functionIds)
            {
                yield return new RoleFunctions(roleId, item, createUserId, createUserName);
            }
        }


    }
}