﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using EES.Infrastructure.Entities;
using EES.Infrastructure.Enums;
using EES.Modules.Ums.Commands.User;
using System;
using System.Collections.Generic;

namespace EES.Modules.Ums.Entities
{
    /// <summary>
    /// 角色的对象集合
    /// </summary>
    public partial class Role : EntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="dataIds"></param>
        /// <param name="functionIds"></param>
        /// <param name="pcMenuIds"></param>
        /// <param name="pdaMenuIds"></param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        public Role(string name, string description, IEnumerable<long> dataIds, IEnumerable<long> functionIds, IEnumerable<long> pcMenuIds, IEnumerable<long> pdaMenuIds, IEnumerable<long> mfsMenuIds, long createUserId, string createUserName) : base(createUserId, createUserName)
        {
            Name = name;
            Description = string.IsNullOrEmpty(description) ? string.Empty : description;

            AddRoleDatas(dataIds, createUserId, createUserName);
            AddRoleFunctions(functionIds, createUserId, createUserName);
            AddRoleMenus(pcMenuIds, pdaMenuIds, mfsMenuIds, createUserId, createUserName);
        }

        /// <summary>
        /// 
        /// </summary>
        protected Role() { }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 用户集合
        /// </summary>
        public virtual List<User> Users { get; set; }

        /// <summary>
        /// 角色数据权限
        /// </summary>
        public virtual List<RoleDatas> RoleDatas { get; set; }

        /// <summary>
        /// 角色功能权限
        /// </summary>
        public virtual List<RoleFunctions> RoleFunctions { get; set; }

        /// <summary>
        /// 角色菜单权限
        /// </summary>
        public virtual List<RoleMenus> RoleMenus { get; set; }


        /// <summary>
        /// 角色修改
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="dataIds"></param>
        /// <param name="functionIds"></param>
        /// <param name="pcMenuIds"></param>
        /// <param name="pdaMenuIds"></param>
        /// <param name="updateUserId"></param>
        /// <param name="updateUserName"></param>
        public void Modify(string name, string description, IEnumerable<long> dataIds, IEnumerable<long> functionIds, IEnumerable<long> pcMenuIds, IEnumerable<long> pdaMenuIds, IEnumerable<long> mfsMenuIds, long updateUserId, string updateUserName)
        {
            Name = name;
            Description = string.IsNullOrEmpty(description) ? string.Empty : description;

            UpdateRecord(updateUserId, updateUserName);

            AddRoleDatas(dataIds, updateUserId, updateUserName);
            AddRoleFunctions(functionIds, updateUserId, updateUserName);
            AddRoleMenus(pcMenuIds, pdaMenuIds, mfsMenuIds, updateUserId, updateUserName);
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        public void ExcuteDelete()
        {
            /*
                删除角色时，将角色数据一并删除
             */

            if (RoleDatas is not null)
            {
                RoleDatas.Clear();
            }

            if (RoleFunctions is not null)
            {
                RoleFunctions.Clear();
            }

            if (RoleMenus is not null)
            {
                RoleMenus.Clear();
            }
        }



        /// <summary>
        /// 添加角色数据权限
        /// </summary>
        /// <param name="dataIds"></param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        private void AddRoleDatas(IEnumerable<long> dataIds, long createUserId, string createUserName)
        {
            if (RoleDatas is not null)
            {
                RoleDatas.Clear();
            }
            else
            {
                RoleDatas = new();
            }

            RoleDatas.AddRange(Entities.RoleDatas.GenerateRoleDatas(Id, dataIds, createUserId, createUserName));
        }

        /// <summary>
        /// 添加角色功能权限
        /// </summary>
        /// <param name="functionIds"></param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        private void AddRoleFunctions(IEnumerable<long> functionIds, long createUserId, string createUserName)
        {
            if (RoleFunctions is not null)
            {
                RoleFunctions.Clear();
            }
            else
            {
                RoleFunctions = new();
            }


            RoleFunctions.AddRange(Entities.RoleFunctions.GenerateRoleFunctions(Id, functionIds, createUserId, createUserName));
        }

        /// <summary>
        /// 添加角色菜单权限
        /// </summary>
        /// <param name="pcMenuIds"></param>
        /// <param name="pdaMenuIds"></param>
        /// <param name="mfsMenuIds"></param>
        /// <param name="createUserId"></param>
        /// <param name="createUserName"></param>
        private void AddRoleMenus(IEnumerable<long> pcMenuIds, IEnumerable<long> pdaMenuIds, IEnumerable<long> mfsMenuIds, long createUserId, string createUserName)
        {
            if (RoleMenus is not null)
            {
                RoleMenus.Clear();
            }
            else
            {               
                RoleMenus = new();
            }

            if (pcMenuIds!=null && pcMenuIds.Any())
            {
                RoleMenus.AddRange(Entities.RoleMenus.GenerateRoleMenus(Id, pcMenuIds, MenuType.PC, createUserId, createUserName));
            }

            if (pdaMenuIds != null && pdaMenuIds.Any())
            {
                RoleMenus.AddRange(Entities.RoleMenus.GenerateRoleMenus(Id, pdaMenuIds, MenuType.PDA, createUserId, createUserName));
            }

            if (mfsMenuIds != null && mfsMenuIds.Any())
            {
                RoleMenus.AddRange(Entities.RoleMenus.GenerateRoleMenus(Id, mfsMenuIds, MenuType.MFS, createUserId, createUserName));
            }
        }
    }
}