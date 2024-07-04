using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Auth
{
    public class AuthDataModel
    {
        /// <summary>
        /// 拥有权限的工厂集合
        /// </summary>
        public IEnumerable<AuthDataItem> Factories { get; set; }

        /// <summary>
        /// 拥有权限的车间集合
        /// </summary>
        public IEnumerable<AuthDataItem> Workshops { get; set; }

        /// <summary>
        /// 拥有权限的仓库集合
        /// </summary>
        public IEnumerable<AuthDataItem> Stories { get; set; }

        /// <summary>
        /// 拥有权限的物料类型集合
        /// </summary>
        public IEnumerable<AuthDataItem> Materials { get; set; }
    }
}
