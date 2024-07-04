using EES.Infrastructure.Bus;
using EES.Infrastructure.Enums;
using EES.Modules.Ums.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.User
{
    /// <summary>
    /// 创建用户
    /// </summary>
    public class CreateUserCmd : CommandBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateUserCmd() : base()
        {

        }

        /// <summary>
        /// 用户账户
        /// </summary>
        [Required]
        public string Account { get; set; }
        /// <summary>
        /// 用户密码密文
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [Required]
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
        [Required]
        public long DepartmenId { get; set; }

        /// <summary>
        /// 用户系统语言标识
        /// </summary>
        public Language Language { get; set; } = Language.简体中文;

        /// <summary>
        /// 分配给用户的角色列表
        /// </summary>
        [Required]
        public IEnumerable<long> RoleIds { get; set; }
    }
}
