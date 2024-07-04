using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands.Menu;
using EES.Modules.Ums.Entities;
using MediatR;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.CommandHandlers
{
    /// <summary>
    /// 菜单资源处理
    /// </summary>
    public class MenuCommandHandler : ServerBase,
                                      IRequestHandler<CreateMenuCmd, CommandResponse>,
                                      IRequestHandler<ModifyMenuCmd, CommandResponse>,
                                      IRequestHandler<DeleteMenuCmd, CommandResponse>
    {
        private readonly IRepository<Menu> _menuRepository;

        private readonly IRepository<RoleMenus> _roleMenusRepository;

        private ILogger<MenuCommandHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuRepository"></param>
        /// <param name="roleMenusRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public MenuCommandHandler(IRepository<Menu> menuRepository, IRepository<RoleMenus> roleMenusRepository, ILogger<MenuCommandHandler> logger, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _menuRepository = menuRepository;
            _logger = logger;
            _roleMenusRepository= roleMenusRepository;
        }

        /// <summary>
        /// 新增资源
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(CreateMenuCmd request, CancellationToken cancellationToken)
        {
            var isParentExists = await CheckParentMenuExistsAsync(request.ParentId);

            if (!isParentExists)
            {
                return CommandResponse.Fail(BusinessError.上级资源不存在);
            }

            var isRouteExists = await CheckRouteExistsAsync(request.Route, request.Type);
            if (isRouteExists)
            {
                return CommandResponse.Fail(BusinessError.资源路由重复);
            }

            var menu = new Menu(request.Name, request.ParentId, request.SortNumber, request.Route, request.Type,
                request.Category, request.Description,request.Icon,request.EnglishName, Accessor.Id, Accessor.Name);

            _menuRepository.Add(menu);

            return CommandResponse.Success(menu);
        }


        /// <summary>
        /// 修改资源信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(ModifyMenuCmd request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (menu is null)
            {
                return CommandResponse.Fail(BusinessError.资源不存在);
            }

            var isParentExists = await CheckParentMenuExistsAsync(request.ParentId);

            if (!isParentExists)
            {
                return CommandResponse.Fail(BusinessError.上级资源不存在);
            }

            var isRouteExists = await CheckRouteExistsAsync(request.Route, menu.Type, menu.Id);
            if (isRouteExists)
            {
                return CommandResponse.Fail(BusinessError.资源路由重复);
            }

            menu.Modify(request.Name, request.ParentId, request.SortNumber, request.Route, request.Description, request.Icon, request.EnglishName,request.Type,request.Category, Accessor.Id, Accessor.Name);

            return CommandResponse.Success(menu);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(DeleteMenuCmd request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (menu is null)
            {
                return CommandResponse.Fail(BusinessError.资源不存在);
            }

            var isHasChild = await _menuRepository.Query().AnyAsync(t => t.ParentId == menu.Id, cancellationToken: cancellationToken);

            if (isHasChild)
            {
                return CommandResponse.Fail(BusinessError.请先删除资源下的子资源);
            }

            var isUesd = await _roleMenusRepository.Query().AnyAsync(t => t.MenuId == menu.Id, cancellationToken: cancellationToken);

            if (isUesd)
            {
                return CommandResponse.Fail(BusinessError.资源正在被使用);
            }

            _menuRepository.Delete(menu);

            return CommandResponse.Success(menu);
        }

        /// <summary>
        /// 检查上级资源是否存在
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private async Task<bool> CheckParentMenuExistsAsync(long parentId)
        {
            return parentId == 0 || await _menuRepository.Query().AnyAsync(t => t.Id == parentId);
        }

        /// <summary>
        /// 检查路由是否存在
        /// </summary>
        /// <param name="route"></param>
        /// <param name="type"></param>
        /// <param name="excludedMenuId"></param>
        /// <returns></returns>
        private async Task<bool> CheckRouteExistsAsync(string route, MenuType type, long excludedMenuId = 0)
        {
            return await _menuRepository.Query().AnyAsync(t => t.Type == type && t.Route == route && t.Id != excludedMenuId);
        }
    }
}
