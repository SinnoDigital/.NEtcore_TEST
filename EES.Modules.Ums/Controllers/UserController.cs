using EES.Infrastructure.Attributes;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands.User;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Services;
using MediatR;
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
    /// UMS-用户管理
    /// </summary>
    [ApiController]
    [Route("api/ums/[controller]")]
    [ApiExplorerSettings(GroupName = "ums")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly IMediator _mediator;

        private readonly IUserService _userService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="userService"></param>
        public UserController(ILogger<UserController> logger, IMediator mediator, IUserService userService)
        {
            _logger = logger;
            _mediator = mediator;
            _userService = userService;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("get")]
        [HttpGet]

        public async Task<ApiResponseBase<UserDto>> GetUserAsync([FromQuery] long id)
        {
            var res = await _userService.GetUserAsync(id);

            return res.Status ? ApiResponseBase<UserDto>.Success(data: res.Data)
                              : ApiResponseBase<UserDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [Route("getUsers")]
        [HttpPost]
        public async Task<ApiResponseBase<PaginationModel<UserListDto>>> GetUserListAsync([FromBody] UserListQueryParam queryParam)
        {
            var res = await _userService.GetUsersAsync(queryParam);

            return res.Status ? ApiResponseBase<PaginationModel<UserListDto>>.Success(data: res.Data)
                            : ApiResponseBase<PaginationModel<UserListDto>>.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [RateLimit]
        [Idempotent("user")]
        [AuthorizationRequired("ums_user_create")]

        public async Task<ApiResponseBase> CreateUserAsync(CreateUserCmd cmd)
        {

            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("modifyPwd")]
        [RateLimit]
        public async Task<ApiResponseBase> ModifyPasswordAsync(ModifyPasswordCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("modify")]
        [HttpPost]
        [RateLimit]
        [AuthorizationRequired("ums_user_modify")]
        public async Task<ApiResponseBase> ModifyInformationAsync(ModifyUserCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [RateLimit]
        [AuthorizationRequired("ums_user_delete")]
        public async Task<ApiResponseBase> DeleteUserAsync(DeleteUserCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("resetPwd")]
        [HttpPost]
        [RateLimit]
        [AuthorizationRequired("ums_user_reset")]
        public async Task<ApiResponseBase> ResetPasswordAsync(ResetPasswordCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

    }
}
