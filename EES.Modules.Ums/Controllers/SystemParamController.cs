using EES.Infrastructure.Attributes;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Infrastructure.Enums;
using EES.Modules.Ums.Commands.SystemParams;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Entities;
using EES.Modules.Ums.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Controllers
{
    /// <summary>
    /// UMS-系统参数
    /// </summary>
    [ApiController]
    [Route("api/ums/[controller]")]
    [ApiExplorerSettings(GroupName = "ums")]
    [Authorize]
    public class SystemParamController : ControllerBase
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
        public SystemParamController(ILogger<SystemParamController> logger, IParamService paramService, IMediator mediator)
        {
            _logger = logger;
            _paramService = paramService;
            _mediator = mediator;
        }

        /// <summary>
        /// 创建系统参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [RateLimit]
        [Idempotent(Flag ="sysParam")]
        [AuthorizationRequired("ums_sysparam_create")]
        public async Task<ApiResponseBase> CreateSystemParamAsync(CreateSystemParamCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改系统参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("modify")]
        [HttpPost]
        [RateLimit]
        [AuthorizationRequired("ums_sysparam_modify")]
        public async Task<ApiResponseBase> ModifySystemParamAsync(ModifySystemParamCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 删除系统参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [RateLimit]
        [AuthorizationRequired("ums_sysparam_delete")]
        public async Task<ApiResponseBase> DeleteSystemParamAsync(DeleteSystemParamCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 根据ID获取系统参数信息
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        [Route("getById")]
        [HttpGet]
        public async Task<ApiResponseBase<SystemParamDto>> GetSystemParamByIdAsync(long id)
        { 
            var res= await _paramService.GetSystemParamAsync(id);

            return res.Status ? ApiResponseBase<SystemParamDto>.Success(data: res.Data)
                           : ApiResponseBase<SystemParamDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 根据模块和编码获系统参数信息
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        [Route("get")]
        [HttpGet]
        public async Task<ApiResponseBase<SystemParamDto>> GetSystemParamAsync(SystemModule module, string code)
        {
            var res = await _paramService.GetSystemParamAsync(module, code);

            return res.Status ? ApiResponseBase<SystemParamDto>.Success(data: res.Data)
                           : ApiResponseBase<SystemParamDto>.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 分页形式获取系统参数列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="PageSize">每页的数量</param>
        /// <param name="isGetTotalCount">是否返回总数量</param>
        /// <returns></returns>
        [Route("getList")]
        [HttpGet]
       public async Task<ApiResponseBase<PaginationModel<SystemParamDto>>> GetSystemParamsAsync(int pageIndex, int PageSize, bool isGetTotalCount = true)
        {
            var res = await _paramService.GetSystemParamsAsync(pageIndex, PageSize, isGetTotalCount);

            return res.Status ? ApiResponseBase<PaginationModel<SystemParamDto>>.Success(data: res.Data)
                            : ApiResponseBase<PaginationModel<SystemParamDto>>.Fail(res.Code, res.Message);
        }
    }
}
