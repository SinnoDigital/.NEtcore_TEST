using EES.Infrastructure.Auth;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Enums;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Controllers
{
    /// <summary>
    /// UMS-授权中心
    /// </summary>
    [ApiController]
    [Route("api/ums/[controller]")]
    [ApiExplorerSettings(GroupName = "ums")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly IAuthService _authService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authService"></param>
        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// Pc端登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("pcLogin")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponseBase<TokenDto>> PcLoginAsync(LoginParamDto dto)
        {

            var res = await _authService.LoginAsync(dto.Account, dto.Password);

            return res.Status ? ApiResponseBase<TokenDto>.Success(data: res.Data)
                             : ApiResponseBase<TokenDto>.Fail(res.Code, res.Message);
        }
        /// <summary>
        /// MFS端登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("mfsLogin")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponseBase<TokenDto>> MfsLoginAsync(LoginParamDto dto)
        {

            var res = await _authService.LoginAsync(dto.Account, dto.Password,"mfs");

            return res.Status ? ApiResponseBase<TokenDto>.Success(data: res.Data)
                             : ApiResponseBase<TokenDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// pda端登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("pdaLogin")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponseBase<TokenDto>> PdaLoginAsync(LoginParamDto dto)
        {

            var res = await _authService.LoginAsync(dto.Account, dto.Password, "pda");

            return res.Status ? ApiResponseBase<TokenDto>.Success(data: res.Data)
                             : ApiResponseBase<TokenDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// pse端登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("pseLogin")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponseBase<TokenDto>> PseLoginAsync(LoginParamDto dto)
        {

            var res = await _authService.LoginAsync(dto.Account, dto.Password, "pse");

            return res.Status ? ApiResponseBase<TokenDto>.Success(data: res.Data)
                             : ApiResponseBase<TokenDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// pad端登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("padLogin")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponseBase<TokenDto>> PadLoginAsync(LoginParamDto dto)
        {

            var res = await _authService.LoginAsync(dto.Account, dto.Password, "pad");

            return res.Status ? ApiResponseBase<TokenDto>.Success(data: res.Data)
                             : ApiResponseBase<TokenDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 获取登录用户的资源信息
        /// </summary>
        /// <returns></returns>
        [Route("get")]
        [HttpGet]
        public async Task<ApiResponseBase<AuthInfo>> GetAuthAsync()
        { 
            var res= await _authService.GetAuthAsync();

            return res.Status ? ApiResponseBase<AuthInfo>.Success(data: res.Data)
                           : ApiResponseBase<AuthInfo>.Fail(res.Code, res.Message);
        }

    }
}
