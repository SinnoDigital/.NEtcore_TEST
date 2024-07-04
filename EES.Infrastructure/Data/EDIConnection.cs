using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Data
{
    /// <summary>
    /// EDI连接配置
    /// </summary>
    public class EDIConnection
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string EDIAddress { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
}
