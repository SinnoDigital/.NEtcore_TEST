using AutoMapper;
using EES.Infrastructure.Auth;
using EES.Infrastructure.Data;
using EES.Infrastructure.Tools;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Data
{
    /// <summary>
    /// automapper配置
    /// </summary>
    public class AutoMapperConfig : Profile
    {
        /// <summary>
        /// 配置
        /// </summary>
        public AutoMapperConfig()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.Roles, y => y.MapFrom(src => src.Roles ?? null));

            CreateMap<Role, ShortRoleInfoDto>();


            CreateMap<User, UserListDto>()
                .ForMember(x => x.DepartmentName, y => y.MapFrom(src => src.Department == null ? string.Empty : src.Department.Name))
                 .ForMember(x => x.DepartmentCode, y => y.MapFrom(src => src.Department == null ? string.Empty : src.Department.Code))
                 .ForMember(x => x.QRCode, y => y.MapFrom(src => src.Password == null ? string.Empty : src.Password))
                 .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    :Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")))
                 .ForMember(x => x.RoleNames, y => y.MapFrom(src => src.Roles == null ? string.Empty : string.Join(",", src.Roles.Select(x => x.Name))));


            CreateMap<Department, DepartmentDto>()
                .ForMember(x => x.ParentName, y => y.Ignore())
                .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<Menu, MenuDto>()
                 .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<UserParam, UserParamDto>()
                 .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")));


            CreateMap<Function, FunctionDto>()
                 .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")));


            CreateMap<Function, FunctionTreeDto>();


            CreateMap<Entities.Data, DataDto>()
                 .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<Entities.Data, ShortDataDto>();

            CreateMap<Team, TeamDto>()
                 .ForMember(x => x.Users, y => y.Ignore())
                 .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<Team, TeamListDto>()
                 .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")));

            /*
             由于没有开启 EF CORE的Lazy Load模式，在使用实体的导航属性进行字段映射时，请务必保证查询时，实体已经将导航属性显式include进来。
             */
            CreateMap<User, TeamUserDto>()
                .ForMember(x => x.DepartmentName, y => y.MapFrom(src => src.Department.Name))
                .ForMember(x => x.RoleNames, y => y.MapFrom(src => string.Join(",", src.Roles.Select(x => x.Name))));


            _ = CreateMap<Role, RoleDto>()
                 .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")))
                 .ForMember(x => x.Datas, y => y.Ignore())
                 .ForMember(x => x.Functions, y => y.Ignore())
                 .ForMember(x => x.PcMenus, y => y.Ignore())
                  .ForMember(x => x.PdaMenus, y =>y.Ignore());

            CreateMap<Role, RoleListDto>()
                  .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<SystemParam, SystemParamDto>()
                .ForMember(x => x.UpdateTime, y => y.MapFrom(
                    src => DateTimeHelper.IsDefaultTime(src.UpdateTime) ? ""
                    : Convert.ToDateTime(src.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<SystemParam, SystemParamModel>();

            CreateMap<User, LoginUser>()
                  .ForMember(x => x.DepartmentName, y => y.MapFrom(src => src.Department == null ? string.Empty : src.Department.Name))
                  .ForMember(x => x.DepartmentCode, y => y.MapFrom(src => src.Department == null ? string.Empty : src.Department.Code))
                  .ForMember(x => x.RoleNames, y => y.MapFrom(src => src.Roles == null ? string.Empty : string.Join(",", src.Roles.Select(x => x.Name))));

            CreateMap<Menu, AuthMenuItem>();

            CreateMap<Entities.Data, AuthDataItem>()
                .ForMember(x => x.Name, y => y.MapFrom(src => src.ObjectName))
                .ForMember(x => x.Code, y => y.MapFrom(src => src.ObjectCode))
                .ForMember(x => x.Id, y => y.MapFrom(src => src.ObjectId));

            CreateMap<Function, AuthFunctionItem>()
                .ForMember(x => x.Identifier, y => y.MapFrom(src => src.Identifier.ToLower())); //全部转为小写
        }
    }
}
