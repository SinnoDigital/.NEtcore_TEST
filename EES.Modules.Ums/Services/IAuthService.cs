using EES.Infrastructure.Auth;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Services
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public interface IAuthService : ITransientDependency
    {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">密码</param>
        /// <param name="platform">平台</param>
        /// <returns></returns>
        Task<QueryResponse<TokenDto>> LoginAsync(string account, string password,string platform="pc");

        /// <summary>
        /// 用户登录加密
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">密码</param>
        /// <param name="platform">平台</param>
        /// <returns></returns>
        Task<QueryResponse<TokenDto>> LoginPwdAsync(string account, string password, string platform = "pc");


        /// <summary>
        /// 获取当前用户的资源
        /// </summary>
        /// <returns></returns>
        Task<QueryResponse<AuthInfo>> GetAuthAsync();

    }
}
