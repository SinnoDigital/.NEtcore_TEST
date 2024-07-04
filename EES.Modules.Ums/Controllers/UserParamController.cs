using EES.Infrastructure.Attributes;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Modules.Ums.Commands.SystemParams;
using EES.Modules.Ums.Commands.UserParams;
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
    /// UMS-用户参数
    /// </summary>
    [ApiController]
    [Route("api/ums/[controller]")]
    [ApiExplorerSettings(GroupName = "ums")]
    [Authorize]
    public class UserParamController : ControllerBase
    {
        private readonly ILogger<SystemParamController> _logger;

        private readonly IParamService _paramService;

        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="paramService"></param>
        /// <param name="mediator"></param>
        public UserParamController(ILogger<SystemParamController> logger, IParamService paramService, IMediator mediator)
        {
            _logger = logger;
            _paramService = paramService;
            _mediator = mediator;
        }

        /// <summary>
        /// 创建用户参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [RateLimit]
        [Idempotent(Flag ="userParam")]
        public async Task<ApiResponseBase> CreateUserParamAsync(CreateUserParamCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改用户参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("modify")]
        [HttpPost]
        public async Task<ApiResponseBase> ModifySystemParamAsync(ModifyUserParamCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 删除用户参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [RateLimit]
        public async Task<ApiResponseBase> DeleteSystemParamAsync(DeleteUserParamCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 根据ID获取用户参数信息
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        [Route("getById")]
        [HttpGet]
        public async Task<ApiResponseBase<UserParamDto>> GetUserParamAsync(long id)
        {
            var res = await _paramService.GetUserParamAsync(id);

            return res.Status ? ApiResponseBase<UserParamDto>.Success(data: res.Data)
                           : ApiResponseBase<UserParamDto>.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 根据类型和编码获取当前用户的用户参数信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("get")]
        [HttpGet]
        public async Task<ApiResponseBase<UserParamDto>> GetUserParamAsync(string type, string code)
        {
            var res = await _paramService.GetUserParamAsync(type, code);

            return res.Status ? ApiResponseBase<UserParamDto>.Success(data: res.Data)
                           : ApiResponseBase<UserParamDto>.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 获取用户全部的用户参数信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="isGetTotalCount">是否返回总数量</param>
        /// <returns></returns>
        [Route("getList")]
        [HttpGet]
        public async Task<ApiResponseBase<PaginationModel<UserParamDto>>> GetUserParamsAsync(int pageIndex, int PageSize, bool isGetTotalCount = true)
        {
            var res = await _paramService.GetUserParamsAsync(pageIndex, PageSize, isGetTotalCount);

            return res.Status ? ApiResponseBase<PaginationModel<UserParamDto>>.Success(data: res.Data)
                            : ApiResponseBase<PaginationModel<UserParamDto>>.Fail(res.Code, res.Message);
        }
    }
}
