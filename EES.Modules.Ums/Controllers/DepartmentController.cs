using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Modules.Ums.CommandHandlers;
using EES.Modules.Ums.Commands.Department;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Entities;
using EES.Modules.Ums.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using EES.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using EES.Infrastructure.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace EES.Modules.Ums.Controllers
{
    /// <summary>
    /// UMS-组织部门管理
    /// </summary>
    [ApiController]
    [Route("api/ums/[controller]")]
    [ApiExplorerSettings(GroupName ="ums")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;

        private readonly IDepartmentService _departmentService;

        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="departmentService"></param>
        /// <param name="mediator"></param>
        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentService departmentService, IMediator mediator)
        {
            _logger = logger;
            _departmentService = departmentService;
            _mediator = mediator;
        }

        /// <summary>
        /// 创建组织部门
        /// </summary>
        /// <param name="cmd">组织部门参数</param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [RateLimit]
        [Idempotent(Flag="department_create")]
        [AuthorizationRequired("ums_department_create")]
        public async Task<ApiResponseBase> CreateAsync([FromBody] CreateDepartmentCmd cmd)
        {   
            
            var res = await _mediator.Send(cmd);

            _logger.LogInformation("method：{method},result:{result}", "createDepartment",JsonConvert.SerializeObject(res));
         
            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改组织部门信息
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("modify")]
        [RateLimit]
        [AuthorizationRequired("ums_department_modify")]
        [Idempotent(Flag = "department_modify")]
        public async Task<ApiResponseBase> ModifyAsync(ModifyDepartmentCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [RateLimit]
        [AuthorizationRequired("ums_department_delete")]
        public async Task<ApiResponseBase> DeleteAsync(DeleteDepartmentCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                              : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 获取部门详情
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns></returns>
        [Route("get")]
        [HttpGet]
        public async Task<ApiResponseBase<DepartmentDto>> GetDepartmentAsync([FromQuery] long id)
        {
            var res = await _departmentService.GetDepartmentAsync(id);

            return res.Status ? ApiResponseBase<DepartmentDto>.Success(data: res.Data)
                             : ApiResponseBase<DepartmentDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        ///以树状结构返回指定组织部门以及其全部子部门信息
        /// </summary>
        /// <param name="rootId">根节点(部门)id，获取全部则写0</param>
        /// <returns></returns>
        [Route("getTree")]
        [HttpGet]
        public async Task<ApiResponseBase<IEnumerable<TreeItem<DepartmentDto>>>> GetDepartmentTreeAsync([FromQuery] long rootId = 0)
        {
            var res = await _departmentService.GetDepartmentTreeAsync(rootId);

            return res.Status ? ApiResponseBase<IEnumerable<TreeItem<DepartmentDto>>>.Success(data: res.Data)
                             : ApiResponseBase<IEnumerable<TreeItem<DepartmentDto>>>.Fail(res.Code, res.Message);
        }
    }
}
