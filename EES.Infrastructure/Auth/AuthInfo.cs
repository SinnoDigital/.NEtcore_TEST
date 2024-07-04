using EES.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Auth
{
    /// <summary>
    /// 用户的权限信息
    /// </summary>
    public class AuthInfo
    {
        /// <summary>
        /// 当前用户的基本信息
        /// </summary>
        public LoginUser User { get; set; }

        /// <summary>
        /// PC菜单权限
        /// </summary>
        public IEnumerable<TreeItem<AuthMenuItem>> AuthPcMenus { get; set; }

        /// <summary>
        /// Pda菜单权限
        /// </summary>
        public IEnumerable<TreeItem<AuthMenuItem>> AuthPdaMenus { get; set; }

        /// <summary>
        /// Mfs菜单权限
        /// </summary>
        public IEnumerable<TreeItem<AuthMenuItem>> AuthMfsMenus { get; set; }

        /// <summary>
        /// 当前用户的功能权限
        /// </summary>
        public IEnumerable<AuthFunctionItem> AuthFunctions { get; set; }

        /// <summary>
        /// 当前用户的数据权限信息
        /// </summary>
        public AuthDataModel AuthData { get; set; }
    }
}
