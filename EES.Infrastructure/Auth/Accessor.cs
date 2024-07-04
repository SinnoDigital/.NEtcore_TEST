using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Auth
{
    /// <summary>
    /// 当前请求的用户信息
    /// </summary>
    public class Accessor
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// 当前用户的功能权限
        /// </summary>
        public IEnumerable<AuthFunctionItem> AuthFunctions { get; set; }

        /// <summary>
        /// 当前用户的数据权限信息
        /// </summary>
        public AuthDataModel AuthData { get; set; }


        public static Accessor Anonymous()
        {
            return new Accessor
            {
                Id = 0,
                Name = string.Empty,
                AuthData = null,
                AuthFunctions = null
            };
        }

    }


}
