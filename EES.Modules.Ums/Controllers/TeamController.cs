using EES.Infrastructure.Attributes;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Modules.Ums.Commands.Team;
using EES.Modules.Ums.Commands.User;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Entities;
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
    /// UMS-工作组管理
    /// </summary>
    [ApiController]
    [Route("api/ums/[controller]")]
    [ApiExplorerSettings(GroupName = "ums")]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        private readonly ILogger<TeamController> _logger;

        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamService"></param>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public TeamController(ITeamService teamService, ILogger<TeamController> logger, IMediator mediator)
        {
            _teamService = teamService;
            _logger = logger;
            _mediator = mediator;
        }


        /// <summary>
        /// 创建工作组
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [RateLimit]
        [Idempotent(Flag ="team")]
        [AuthorizationRequired("ums_team_create")]
        public async Task<ApiResponseBase> CreateAsync(CreateTeamCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改工作组信息
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("modify")]
        [RateLimit]
        [AuthorizationRequired("ums_team_modify")]
        public async Task<ApiResponseBase> ModifyAsync(ModifyTeamCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 删除工作组
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        [RateLimit]
        [AuthorizationRequired("ums_team_delete")]
        public async Task<ApiResponseBase> DeleteAsync(DeleteTeamCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 查询工作组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("get")]
        [HttpGet]
        public async Task<ApiResponseBase<TeamDto>> GetTeamAsync(long id)
        {
            var res = await _teamService.GetTeamAsync(id);

            return res.Status ? ApiResponseBase<TeamDto>.Success(data: res.Data)
                              : ApiResponseBase<TeamDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 查询工作组列表
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        [Route("getlist")]
        [HttpPost]
        public async Task<ApiResponseBase<PaginationModel<TeamListDto>>> GetTeamListAsync(TeamListQueryParams queryParams)
        {
            var res = await _teamService.GetTeamsAsync(queryParams);

            return res.Status ? ApiResponseBase<PaginationModel<TeamListDto>>.Success(data: res.Data)
                            : ApiResponseBase<PaginationModel<TeamListDto>>.Fail(res.Code, res.Message);
        }
    }
}
