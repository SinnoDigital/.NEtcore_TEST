using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands.Role;
using EES.Modules.Ums.Entities;
using MediatR;
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
    /// 角色处理
    /// </summary>
    public class RoleCommandHandler : ServerBase,
                                    IRequestHandler<CheckRoleIdsCmd, CommandResponse>,
                                    IRequestHandler<CreateRoleCmd, CommandResponse>,
                                    IRequestHandler<ModifyRoleCmd,CommandResponse>,
                                    IRequestHandler<DeleteRoleCmd,CommandResponse>
    {
        private readonly IRepository<Role> _roleRepository;

        private readonly IRepository<Entities.Data> _dataRepository;

        private readonly IRepository<Menu> _menuRepository;

        private readonly IRepository<Function> _functionRepository;

        private readonly IRepository<UserRoles> _userRolesRepository;

        private readonly ILogger<RoleCommandHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleRepository"></param>
        /// <param name="dataRepository"></param>
        /// <param name="menuRepository"></param>
        /// <param name="functionRepository"></param>
        /// <param name="userRolesRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public RoleCommandHandler(IRepository<Role> roleRepository, IRepository<Entities.Data> dataRepository, IRepository<Menu> menuRepository, IRepository<Function> functionRepository, IRepository<UserRoles> userRolesRepository, ILogger<RoleCommandHandler> logger, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _roleRepository = roleRepository;
            _dataRepository = dataRepository;
            _menuRepository = menuRepository;
            _functionRepository = functionRepository;
            _userRolesRepository = userRolesRepository;
            _logger = logger;
        }

        /// <summary>
        /// 检查角色id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(CheckRoleIdsCmd request, CancellationToken cancellationToken)
        {

            if (request.RoleIds == null || !request.RoleIds.Any())
            {
                return CommandResponse.Success(data: false);
            }

            var allRoleIds = await _roleRepository.Query().Select(x => x.Id).ToListAsync(cancellationToken: cancellationToken);

            return CommandResponse.Success(data: request.RoleIds.All(allRoleIds.Contains));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(CreateRoleCmd request, CancellationToken cancellationToken)
        {
            if (await CheckRoleNameExistsAsync(request.Name))
            {
                return CommandResponse.Fail(BusinessError.角色名重复);
            }
            // PC菜单权限、PDA菜单权限跟MFS菜单权限最少要有一个
            if (! await CheckMenuAsync(request.PcMenus, MenuType.PC) && !await CheckMenuAsync(request.PdaMenus, MenuType.PDA) && !await CheckMenuAsync(request.PdaMenus, MenuType.MFS))
            {
                return CommandResponse.Fail(BusinessError.请最少给角色分配一项菜单权限);
            }

            if (!await CheckDataAsync(request.Datas))
            {
                return CommandResponse.Fail(BusinessError.请最少给角色分配一项数据权限);
            }

            if (!await CheckFunctionAsync(request.Functions))
            {
                return CommandResponse.Fail(BusinessError.请最少给角色分配一项功能权限);
            }

            var role = new Role(request.Name, request.Description, request.Datas, request.Functions, request.PcMenus, request.PdaMenus, request.MfsMenus, Accessor.Id, Accessor.Name);

            _roleRepository.Add(role);

            return CommandResponse.Success(role);
        }


        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(ModifyRoleCmd request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.Query()
                           .Include(x => x.RoleMenus)
                           .Include(x => x.RoleDatas)
                           .Include(x => x.RoleFunctions)
                           .AsSplitQuery()
                           .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (role is null)
            {
                return CommandResponse.Fail(BusinessError.角色不存在);
            }

            if (await CheckRoleNameExistsAsync(request.Name,role.Id))
            {
                return CommandResponse.Fail(BusinessError.角色名重复);
            }

            // PC菜单权限、PDA菜单权限跟MFS菜单权限最少要有一个
            if (!await CheckMenuAsync(request.PcMenus, MenuType.PC) && !await CheckMenuAsync(request.PdaMenus, MenuType.PDA) && !await CheckMenuAsync(request.PdaMenus, MenuType.MFS))
            {
                return CommandResponse.Fail(BusinessError.请最少给角色分配一项菜单权限);
            }

            if (!await CheckDataAsync(request.Datas))
            {
                return CommandResponse.Fail(BusinessError.请最少给角色分配一项数据权限);
            }

            if (!await CheckFunctionAsync(request.Functions))
            {
                return CommandResponse.Fail(BusinessError.请最少给角色分配一项功能权限);
            }

            role.Modify(request.Name, request.Description, request.Datas, request.Functions, request.PcMenus, request.PdaMenus, request.MfsMenus, Accessor.Id, Accessor.Name);

            return CommandResponse.Success(role);
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(DeleteRoleCmd request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.Query()
                           .Include(x => x.RoleMenus)
                           .Include(x => x.RoleDatas)
                           .Include(x => x.RoleFunctions)
                           .AsSplitQuery()
                           .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (role is null)
            {
                return CommandResponse.Fail(BusinessError.角色不存在);
            }

            if (await _userRolesRepository.NoTrackingQuery().AnyAsync(t =>t.RoleId==role.Id, cancellationToken: cancellationToken))
            {
                return CommandResponse.Fail(BusinessError.角色正在被使用);
            }

            role.ExcuteDelete();

            _roleRepository.Delete(role);

            return CommandResponse.Success(role);
        }


        /// <summary>
        /// 检查角色名是否存在
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="excludedRoleId">被排除，不参与检查的角色的id</param>
        /// <returns></returns>
        private async Task<bool> CheckRoleNameExistsAsync(string roleName, long excludedRoleId = 0)
        {
            return await _roleRepository.Query().AnyAsync(t => t.Name == roleName && t.Id != excludedRoleId);
        }

        /// <summary>
        /// 校验菜单id
        /// </summary>
        /// <param name="menuIds"></param>
        /// <param name="menuType"></param>
        /// <returns></returns>
        private async Task<bool> CheckMenuAsync(IEnumerable<long> menuIds, MenuType menuType)
        {
            var allMenuIds = await _menuRepository.NoTrackingQuery().Where(t => t.Type == menuType).Select(x => x.Id).ToListAsync();

            return menuIds != null && menuIds.Any() && menuIds.All(allMenuIds.Contains);
        }

        /// <summary>
        /// 校验数据权限id
        /// </summary>
        /// <param name="dataIds"></param>
        /// <returns></returns>
        private async Task<bool> CheckDataAsync(IEnumerable<long> dataIds)
        {
            var allDataIds = await _dataRepository.NoTrackingQuery().Select(x => x.Id).ToListAsync();

            return dataIds != null && dataIds.Any() && dataIds.All(allDataIds.Contains);
        }

        /// <summary>
        /// 校验功能数据id
        /// </summary>
        /// <param name="functionIds"></param>
        /// <returns></returns>
        private async Task<bool> CheckFunctionAsync(IEnumerable<long> functionIds)
        {
            var allFunctionIds = await _functionRepository.NoTrackingQuery().Select(x => x.Id).ToListAsync();

            return functionIds != null && functionIds.Any() && functionIds.All(allFunctionIds.Contains);
        }

       
    }
}
