using EES.Infrastructure.Attributes;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Infrastructure.Enums;
using EES.Modules.Ums.Commands.Data;
using EES.Modules.Ums.Commands.Funtion;
using EES.Modules.Ums.Commands.Menu;
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
    /// UMS-资源管理(菜单，功能权限，数据权限)
    /// </summary>
    [ApiController]
    [Route("api/ums/[controller]")]
    [ApiExplorerSettings(GroupName = "ums")]
    [Authorize]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;

        private readonly IMediator _mediator;

        private readonly IResourceService _resourceService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="resourceService"></param>
        public ResourceController(ILogger<ResourceController> logger, IMediator mediator, IResourceService resourceService)
        {
            _logger = logger;
            _mediator = mediator;
            _resourceService = resourceService;
        }


        /// <summary>
        /// 添加菜单资源
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("createMenu")]
        [HttpPost]
        [RateLimit]
        [Idempotent(Flag ="menu")]
        public async Task<ApiResponseBase> CreateMenuAsync(CreateMenuCmd cmd)
        {

            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改菜单资源信息
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("modifyMenu")]
        [HttpPost]
        [RateLimit]
        public async Task<ApiResponseBase> ModifyMenuAsync(ModifyMenuCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 删除菜单资源
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("deleteMenu")]
        [HttpPost]
        [RateLimit]
        public async Task<ApiResponseBase> DeleteMenuAsync(DeleteMenuCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 获取资源详情
        /// </summary>
        /// <param name="id">资源id</param>
        /// <returns></returns>
        [Route("getMenu")]
        [HttpGet]
        [RateLimit]
        public async Task<ApiResponseBase<MenuDto>> GetMenuAsync([FromQuery] long id)
        {
            var res = await _resourceService.GetMenuAsync(id);

            return res.Status ? ApiResponseBase<MenuDto>.Success(data: res.Data)
                              : ApiResponseBase<MenuDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 以树状结构返回指定资源以及其全部子资源信息
        /// </summary>
        /// <param name="rootId">根节点(菜单)id，获取全部则写0</param>
        /// <param name="menuType">菜单类型 1:PC  2:PDA</param>
        /// <returns></returns>
        [Route("getMenuTree")]
        [HttpGet]
        public async Task<ApiResponseBase<IEnumerable<TreeItem<MenuDto>>>> GetMenuTreeAsync([FromQuery] MenuType menuType, long rootId = 0)
        {
            var res = await _resourceService.GetMenuTreeAsync(menuType, rootId);

            return res.Status ? ApiResponseBase<IEnumerable<TreeItem<MenuDto>>>.Success(data: res.Data)
                             : ApiResponseBase<IEnumerable<TreeItem<MenuDto>>>.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 创建功能权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("createFunc")]
        [HttpPost]
        [RateLimit]
        [Idempotent(Flag = "func")]
        public async Task<ApiResponseBase> CreateFunctionAsync(CreateFuntionCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改功能权限数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("modifyFunc")]
        [HttpPost]
        [RateLimit]
        public async Task<ApiResponseBase> ModifyFunctionAsync(ModifyFunctionCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 删除功能权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("deleteFunc")]
        [HttpPost]
        [RateLimit]
        public async Task<ApiResponseBase> DeleteFunctionAsync(DeleteFunctionCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        ///  获取功能权限资源详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getFunc")]
        [HttpGet]
        public async Task<ApiResponseBase<FunctionDto>> GetFunctionAsync(long id)
        { 
            var res= await _resourceService.GetFunctionAsync(id);

            return res.Status ? ApiResponseBase<FunctionDto>.Success(data: res.Data)
                         : ApiResponseBase<FunctionDto>.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 获取功能权限资源树
        /// </summary>
        /// <returns></returns>
        [Route("getFuncTree")]
        [HttpGet]
        public async Task<ApiResponseBase<IEnumerable<TreeItem<FunctionDto>>>> GetFunctionTreeAsync()
        {
            var res = await _resourceService.GetFunctionTreeAsync();

            return res.Status ? ApiResponseBase<IEnumerable<TreeItem<FunctionDto>>>.Success(data: res.Data)
                             : ApiResponseBase<IEnumerable<TreeItem<FunctionDto>>>.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 创建物料类型数据权限资源
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("createMaterialData")]
        [HttpPost]
        [RateLimit]
        [Idempotent(Flag = "Material")]
        public async Task<ApiResponseBase> CreateMaterialDataAsync(CreateMaterialDataCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 修改物料类型数据权限资源
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("modifyMaterialData")]
        [HttpPost]
        [RateLimit]
        public async Task<ApiResponseBase> ModifyMaterialDataAsync(ModifyMaterialDataCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 删除物料类型数据权限资源
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [Route("deleteMaterialData")]
        [HttpPost]
        [RateLimit]
        public async Task<ApiResponseBase> DeleteMaterialDataAsync(DeleteMaterialDataCmd cmd)
        {
            var res = await _mediator.Send(cmd);

            return res.Status ? ApiResponseBase.Success()
                : ApiResponseBase.Fail(res.Code, res.Message);
        }


        /// <summary>
        /// 获取数据权限资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getData")]
        [HttpGet]
        public async Task<ApiResponseBase<DataDto>> GetDataAsync(long id)
        {
            var res = await _resourceService.GetDataAsync(id);

            return res.Status ? ApiResponseBase<DataDto>.Success(data: res.Data)
                         : ApiResponseBase<DataDto>.Fail(res.Code, res.Message);
        }

        /// <summary>
        /// 获取数据权限资源树
        /// </summary>
        /// <returns></returns>
        [Route("getDataTree")]
        [HttpGet]
        public async Task<ApiResponseBase<AllDataDto>> GetDataTreeAsync()
        {
            var res = await _resourceService.GetDataTreeAsync();

            return res.Status ? ApiResponseBase<AllDataDto>.Success(data: res.Data)
                         : ApiResponseBase<AllDataDto>.Fail(res.Code, res.Message);
        }
    }
}
