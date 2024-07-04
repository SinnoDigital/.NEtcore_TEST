using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Auth
{
    public class AuthDataItem
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
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 父级id (若是车间，则该字段的值为所属的工厂的id, 以此类推)
        /// </summary>
        public long ParentId { get; set; }
    }
}
