using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Auth
{
    public class LoginUser
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
        /// 角色名(角色1,角色2,角色3)
        /// </summary>
        public string RoleNames { get; set; }

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
        /// 用户系统语言标识
        /// </summary>
        public Language Language { get; set; }
    }
}
