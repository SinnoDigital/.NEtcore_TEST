using EES.Infrastructure.DataBase;
using EES.Modules.Ums.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Data
{
    /// <summary>
    /// UMS模块的模型构建
    /// </summary>
    public class UmsModelBuilder : ICustomModelBuilder
    {
        /// <summary>
        /// 模型绑定
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void Build(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Entities.Data>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_data__3213E83F33327F39");

                entity.ToTable("ums_data", tb => tb.HasComment("数据权限资源表- 数据来自pem模块"));

                entity.HasMany(x => x.RoleDatas).WithOne(x => x.Data).HasForeignKey(x => x.DataId);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Category)
                    .HasComment("1 工厂车间  2：仓库  3：物料类型   4:车间")
                    .HasColumnName("category");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.ObjectCode)
                    
                    .HasMaxLength(64)
                    .HasComment("对应数据的编码")
                    .HasColumnName("object_code");
                entity.Property(e => e.ObjectId)
                    .HasComment("对应的数据id")
                    .HasColumnName("object_id");
                entity.Property(e => e.ObjectName)
                    
                    .HasMaxLength(64)
                    .HasComment("数据显示名字")
                    .HasColumnName("object_name");
                entity.Property(e => e.ParentId)
                    .HasComment("所属上级的id。 对应到object_id。工厂的上级id为0，车间，仓库的上级id填写所属工厂的id。")
                    .HasColumnName("parent_id");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次更新时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次更新数据的用户Id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次更新用户的名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_depa__3213E83F637AF943");

                entity.ToTable("ums_department", tb => tb.HasComment("部门信息"));

                entity.Property(e => e.Id)
                    .HasComment("数据的唯一ID")
                    .HasColumnName("id");
                entity.Property(e => e.Code)
                    
                    .HasMaxLength(64)
                    .HasComment("代码")
                    .HasColumnName("code");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.Description)
                    
                    .HasMaxLength(255)
                    .HasComment("描述")
                    .HasColumnName("description");
                entity.Property(e => e.ImageUrl)
                    
                    .HasMaxLength(255)
                    .HasComment("图标的url")
                    .HasColumnName("image_url");
                entity.Property(e => e.Name)
                    
                    .HasMaxLength(64)
                    .HasComment("名称")
                    .HasColumnName("name");
                entity.Property(e => e.ParentId)
                    .HasComment("数据的父级ID")
                    .HasColumnName("parent_id");
                entity.Property(e => e.SortNumber)
                    .HasComment("序号")
                    .HasColumnName("sort_number");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修改时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<Function>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_func__3213E83F5A146FFE");

                entity.ToTable("ums_function", tb => tb.HasComment("功能资源表"));

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.Description)
                    
                    .HasMaxLength(64)
                    .HasComment("功能说明")
                    .HasColumnName("description");
                entity.Property(e => e.Identifier)
                    
                    .HasMaxLength(64)
                    .HasComment("功能权限标识符")
                    .HasColumnName("identifier");
                entity.Property(e => e.Name)
                    
                    .HasMaxLength(64)
                    .HasComment("名字")
                    .HasColumnName("name");
                entity.Property(e => e.ParentId)
                    .HasComment("父级id")
                    .HasColumnName("parent_id");
                entity.Property(e => e.Type)
                    .HasComment("标识 1: 分类标识   2：功能")
                    .HasColumnName("type");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_menu__3213E83F4E5F56F5");

                entity.ToTable("ums_menu", tb => tb.HasComment("系统菜单资源表"));

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Category)
                    .HasComment("菜单的类别：1 导航分类 2：页面")
                    .HasColumnName("category");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.Description)
                    
                    .HasMaxLength(255)
                    .HasComment("备注说明")
                    .HasColumnName("description");
                entity.Property(e => e.EnglishName)
                    
                    .HasMaxLength(64)
                    .HasComment("菜单的英文名称")
                    .HasColumnName("english_name");
                entity.Property(e => e.Icon)
                    
                    .HasMaxLength(64)
                    .HasComment("图标")
                    .HasColumnName("icon");
                entity.Property(e => e.Name)
                    
                    .HasMaxLength(64)
                    .HasComment("菜单名称")
                    .HasColumnName("name");
                entity.Property(e => e.ParentId)
                    .HasComment("上级菜单")
                    .HasColumnName("parent_id");
                entity.Property(e => e.Route)
                    
                    .HasMaxLength(64)
                    .HasComment("菜单路由")
                    .HasColumnName("route");
                entity.Property(e => e.SortNumber)
                    .HasComment("同级别下显示排序")
                    .HasColumnName("sort_number");
                entity.Property(e => e.Type)
                    .HasComment("菜单类型: 1 PC菜单， 2：PDA菜单 ")
                    .HasColumnName("type");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_role__3213E83F026CD04D");

                entity.ToTable("ums_role", tb => tb.HasComment("角色的对象集合"));

                entity.HasMany(x => x.RoleMenus).WithOne(x => x.Role).HasForeignKey(e => e.RoleId);

                entity.HasMany(x => x.RoleDatas).WithOne(x => x.Role).HasForeignKey(e => e.RoleId);

                entity.HasMany(x => x.RoleFunctions).WithOne(x => x.Role).HasForeignKey(e => e.RoleId);

                entity.Property(e => e.Id)
                    .HasComment("数据的唯一ID")
                    .HasColumnName("id");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.Description)
                    
                    .HasMaxLength(255)
                    .HasComment("描述")
                    .HasColumnName("description");
                entity.Property(e => e.Name)
                    
                    .HasMaxLength(64)
                    .HasComment("名称")
                    .HasColumnName("name");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修改时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<RoleDatas>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_role__3213E83FD5C4D540");

                entity.ToTable("ums_role_datas", tb => tb.HasComment("角色数据权限"));

                entity.HasIndex(e => e.DataId, "data_id");

                entity.HasIndex(e => e.RoleId, "role_id");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.DataId)
                    .HasComment("数据权限id")
                    .HasColumnName("data_id");
                entity.Property(e => e.RoleId)
                    .HasComment("角色id")
                    .HasColumnName("role_id");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<RoleFunctions>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_role__3213E83FBD4E8CDC");

                entity.ToTable("ums_role_functions", tb => tb.HasComment("角色功能权限表"));

                entity.HasIndex(e => e.FunctionId, "function_id");

                entity.HasIndex(e => e.RoleId, "role_id");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.FunctionId)
                    .HasComment("功能id")
                    .HasColumnName("function_id");
                entity.Property(e => e.RoleId)
                    .HasComment("角色id")
                    .HasColumnName("role_id");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<RoleMenus>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_role__3213E83FFE12C368");

                entity.ToTable("ums_role_menus", tb => tb.HasComment("角色菜单权限表"));

                entity.HasIndex(e => e.MenuId, "menu_id");

                entity.HasIndex(e => e.RoleId, "role_id");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.MenuId)
                    .HasComment("菜单id")
                    .HasColumnName("menu_id");
                entity.Property(e => e.MenuType)
                    .HasComment("菜单类型: 1 PC菜单， 2：PDA菜单， 3：MFS菜单 ")
                    .HasColumnName("menu_type");
                entity.Property(e => e.RoleId)
                    .HasComment("角色id")
                    .HasColumnName("role_id");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<SystemParam>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_syst__3213E83FC2078344");

                entity.ToTable("ums_system_param", tb => tb.HasComment("系统参数信息"));

                entity.Property(e => e.Id)
                    .HasComment("主键id")
                    .HasColumnName("id");
                entity.Property(e => e.BeforeValue)
                    
                    .HasMaxLength(2048)
                    .HasComment("修改前的值")
                    .HasColumnName("before_value");
                entity.Property(e => e.Code)
                    
                    .HasMaxLength(64)
                    .HasComment("参数编码")
                    .HasColumnName("code");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.DefalutValue)
                    
                    .HasMaxLength(2048)
                    .HasComment("默认值")
                    .HasColumnName("defalut_value");
                entity.Property(e => e.Description)
                    
                    .HasMaxLength(255)
                    .HasComment("描述")
                    .HasColumnName("description");
                entity.Property(e => e.IsEnable)
                    .HasComment("是否启用")
                    .HasColumnName("is_enable");
                entity.Property(e => e.Module)
                    .HasComment("所属模块")
                    .HasColumnName("module");
                entity.Property(e => e.Name)
                    
                    .HasMaxLength(64)
                    .HasComment("名称")
                    .HasColumnName("name");
                entity.Property(e => e.Remark)
                    
                    .HasMaxLength(255)
                    .HasComment("备注")
                    .HasColumnName("remark");
                entity.Property(e => e.SetValue)
                    
                    .HasMaxLength(2048)
                    .HasComment("设定值")
                    .HasColumnName("set_value");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_team__3213E83F97CBFDC3");

                entity.ToTable("ums_team", tb => tb.HasComment("工作组信息"));

                entity.Property(e => e.Id)
                    .HasComment("数据的唯一ID")
                    .HasColumnName("id");
                entity.Property(e => e.AreaCode)
                    
                    .HasMaxLength(64)
                    .HasComment("工作区域编码")
                    .HasColumnName("area_code");
                entity.Property(e => e.AreaId)
                    .HasComment("工作区域id")
                    .HasColumnName("area_id");
                entity.Property(e => e.AreaName)
                    
                    .HasMaxLength(64)
                    .HasComment("工作区域名称")
                    .HasColumnName("area_name");
                entity.Property(e => e.Code)
                    
                    .HasMaxLength(64)
                    .HasComment("工作组编码")
                    .HasColumnName("code");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.Description)
                    
                    .HasMaxLength(255)
                    .HasComment("描述")
                    .HasColumnName("description");
                entity.Property(e => e.FactoryCode)
                    
                    .HasMaxLength(64)
                    .HasComment("工厂编码")
                    .HasColumnName("factory_code");
                entity.Property(e => e.FactoryId)
                    .HasComment("工厂id")
                    .HasColumnName("factory_id");
                entity.Property(e => e.FactoryName)
                    
                    .HasMaxLength(64)
                    .HasComment("工厂名称")
                    .HasColumnName("factory_name");
                entity.Property(e => e.Name)
                    
                    .HasMaxLength(64)
                    .HasComment("工作组名称")
                    .HasColumnName("name");
                entity.Property(e => e.Type)
                    .HasComment("班组类型:  1车间  2仓库  3质量  4其他")
                    .HasColumnName("type");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修改时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<TeamMembers>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_team__3213E83F0A7297FB");

                entity.ToTable("ums_team_members", tb => tb.HasComment("组员"));

                entity.Property(e => e.Id)
                    .HasComment("数据的唯一ID")
                    .HasColumnName("id");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.TeamId)
                    .HasComment("所属班组id")
                    .HasColumnName("team_id");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修改时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
                entity.Property(e => e.UserId)
                    .HasComment("用户ID")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_user__3213E83FE170279D");

                entity.ToTable("ums_user", tb => tb.HasComment("用户信息"));

                entity.HasIndex(e => e.Account, "account").IsUnique();

                entity.HasMany(x => x.Roles)
                    .WithMany(x => x.Users)
                    .UsingEntity<UserRoles>
                     (
                         j => j.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId),
                         j => j.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId)
                       );

                entity.HasOne(x => x.Department).WithMany(x => x.Users).HasForeignKey(x => x.DepartmenId);


                entity.Property(e => e.Id)
                    .HasComment("数据的唯一ID")
                    .HasColumnName("id");
                entity.Property(e => e.Account)
                    
                    .HasMaxLength(32)
                    .HasComment("用户账户")
                    .HasColumnName("account");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.DepartmenId)
                    .HasComment("部门ID")
                    .HasColumnName("departmen_id");
                entity.Property(e => e.ImageUrl)
                    
                    .HasMaxLength(255)
                    .HasComment("用户照片的url")
                    .HasColumnName("image_url");
                entity.Property(e => e.Language)
                    .HasComment("用户系统语言标识")
                    .HasColumnName("language");
                entity.Property(e => e.Mail)
                    
                    .HasMaxLength(64)
                    .HasComment("用户邮箱")
                    .HasColumnName("mail");
                entity.Property(e => e.Name)
                    
                    .HasMaxLength(64)
                    .HasComment("用户名称")
                    .HasColumnName("name");
                entity.Property(e => e.Password)
                    
                    .HasMaxLength(64)
                    .HasComment("用户密码密文")
                    .HasColumnName("password");
                entity.Property(e => e.Phone)
                    
                    .HasMaxLength(32)
                    .HasComment("用户电话")
                    .HasColumnName("phone");
                entity.Property(e => e.Remark)
                    
                    .HasMaxLength(255)
                    .HasComment("备注")
                    .HasColumnName("remark");
                entity.Property(e => e.Salt)
                    
                    .HasMaxLength(64)
                    .HasComment("密码盐")
                    .HasColumnName("salt");
                entity.Property(e => e.SignatureUrl)
                    
                    .HasMaxLength(255)
                    .HasComment("用户签名的url")
                    .HasColumnName("signature_url");
                entity.Property(e => e.State)
                    .HasComment("用户状态")
                    .HasColumnName("state");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
            });

            modelBuilder.Entity<UserParam>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_user__3213E83F0B63681B");

                entity.ToTable("ums_user_param", tb => tb.HasComment("用户参数信息"));

                entity.Property(e => e.Id)
                    .HasComment("主键id")
                    .HasColumnName("id");
                entity.Property(e => e.BeforeValue)
                    
                    .HasMaxLength(2048)
                    .HasComment("修改前的值")
                    .HasColumnName("before_value");
                entity.Property(e => e.Code)
                    
                    .HasMaxLength(64)
                    .HasComment("参数编码")
                    .HasColumnName("code");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.DefalutValue)
                    
                    .HasMaxLength(2048)
                    .HasComment("默认值")
                    .HasColumnName("defalut_value");
                entity.Property(e => e.Description)
                    
                    .HasMaxLength(255)
                    .HasComment("描述")
                    .HasColumnName("description");
                entity.Property(e => e.IsEnable)
                    .HasComment("是否启用")
                    .HasColumnName("is_enable");
                entity.Property(e => e.Name)
                    
                    .HasMaxLength(64)
                    .HasComment("名称")
                    .HasColumnName("name");
                entity.Property(e => e.Remark)
                    
                    .HasMaxLength(255)
                    .HasComment("备注")
                    .HasColumnName("remark");
                entity.Property(e => e.SetValue)
                    
                    .HasMaxLength(2048)
                    .HasComment("设定值")
                    .HasColumnName("set_value");
                entity.Property(e => e.Type)
                    
                    .HasMaxLength(64)
                    .HasComment("参数类型")
                    .HasColumnName("type");
                entity.Property(e => e.Unit)
                    
                    .HasMaxLength(32)
                    .HasComment("参数单位")
                    .HasColumnName("unit");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
                entity.Property(e => e.UserId)
                    .HasComment("用户id")
                    .HasColumnName("user_id");
                entity.Property(e => e.ValueType)
                    
                    .HasMaxLength(255)
                    .HasComment("值类型")
                    .HasColumnName("value_type");
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ums_user__3213E83F5ABAB020");

                entity.ToTable("ums_user_roles", tb => tb.HasComment("用户 - 角色关系表"));

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateTime)
                    .HasComment("创建时间")
                    .HasColumnName("create_time");
                entity.Property(e => e.CreateUserId)
                    .HasComment("创建者id")
                    .HasColumnName("create_user_id");
                entity.Property(e => e.CreateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("创建者名称")
                    .HasColumnName("create_user_name");
                entity.Property(e => e.RoleId)
                    .HasComment("角色id")
                    .HasColumnName("role_id");
                entity.Property(e => e.UpdateTime)
                    .HasComment("最后一次修时间")
                    .HasColumnName("update_time");
                entity.Property(e => e.UpdateUserId)
                    .HasComment("最后一次修改者id")
                    .HasColumnName("update_user_id");
                entity.Property(e => e.UpdateUserName)
                    
                    .HasMaxLength(64)
                    .HasComment("最后一次修改者名称")
                    .HasColumnName("update_user_name");
                entity.Property(e => e.UserId)
                    .HasComment("用户id")
                    .HasColumnName("user_id");
            });
        }
    }
}
