using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Auth
{
    public class AuthFunctionItem

    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标识 1: 分类标识   2：功能
        /// </summary>
        public FunctionType Type { get; set; }

        /// <summary>
        /// 功能说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 功能权限标识符
        /// </summary>
        public string Identifier { get; set; }
    }
}
