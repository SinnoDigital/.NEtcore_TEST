using EES.Infrastructure.Attributes;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Modules.Ums.Commands.Department;
using EES.Modules.Ums.Commands.Role;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Controllers
{

    /// <summary>
    /// UMS-角色管理
    /// </summary>
    [ApiController]
    [Route("api/ums/[controller]")]
    [ApiExplorerSettings(GroupName = "ums")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;

        private readonly IRoleService _roleService;

        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="roleService"></param>
        /// <param name="mediator"></param>
        public RoleController(ILogger<RoleController> logger, IRoleService roleService, IMediator mediator)
        {
            _logger = logger;
            _roleService = roleService;
            _mediator = mediator;
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [RateLimit]
        [Idempotent(Flag ="role")]
        [AuthorizationRequired("ums_role_create")]
        public async Task<ApiResponseBase> CreateAsync([FromBody] CreateRoleCmd cmd)
        {

            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("modify")]
        [RateLimit]
        [AuthorizationRequired("ums_role_modify")]
        public async Task<ApiResponseBase> ModifyAsync(ModifyRoleCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [RateLimit]
        [AuthorizationRequired("ums_role_delete")]
        public async Task<ApiResponseBase> DeleteAsync(DeleteRoleCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [Route("get")]
        [HttpGet]
        public async Task<ApiResponseBase<RoleDto>> GetDepartmentAsync([FromQuery] long id)
        {
            var res = await _roleService.GetRoleAsync(id);

            return res.Status ? ApiResponseBase<RoleDto>.Success(data: res.Data)
                             : ApiResponseBase<RoleDto>.Fail(res.Code, res.Message);
        }


        /// <summary>
        ///  获取角色列表
        /// </summary>
        /// <param name="roleName">角色名关键字</param>
        /// <param name="pageSize">每页数据大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isGetTotalCount">是否返回总数量</param>
        /// <returns></returns>
        [Route("getList")]
        [HttpGet]
        public async Task<ApiResponseBase<PaginationModel<RoleListDto>>> GetUserListAsync(string roleName, int pageSize, int pageIndex, bool isGetTotalCount = true)
        {
            var res = await _roleService.GetRolesAsync(roleName, pageSize, pageIndex, isGetTotalCount);

            return res.Status ? ApiResponseBase<PaginationModel<RoleListDto>>.Success(data: res.Data)
                            : ApiResponseBase<PaginationModel<RoleListDto>>.Fail(res.Code, res.Message);
        }
    }
}
