using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Dto
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户账户
        /// </summary>
        public string Account { get; set; }
             
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 用户电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 用户照片的url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 签名url
        /// </summary>
        public string SignatureUrl { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DepartmenId { get; set; }
  
        /// <summary>
        /// 用户状态
        /// </summary>
        public UserState State { get; set; }

        /// <summary>
        /// 角色名(角色1,角色2,角色3)
        /// </summary>
        public IEnumerable<ShortRoleInfoDto> Roles { get; set; }
    }

    /// <summary>
    /// 用户列表信息
    /// </summary>
    public class UserListDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DepartmenId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserState State { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 修改者名称
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 角色名(角色1,角色2,角色3)
        /// </summary>
        public string RoleNames { get; set; }
        /// <summary>
        /// 二维码内容
        /// </summary>
        public string QRCode { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class UserListQueryParam
    {
       // string account,string name, int pageIndex, int PageSize,bool isGetTotalCount

        /// <summary>
        /// 用户账号关键字
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户名称关键字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 是否包含总数量
        /// </summary>
        public bool IsGetTotalCount { get; set; }
    }
}
