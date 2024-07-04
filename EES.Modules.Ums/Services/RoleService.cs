using AutoMapper;
using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Data;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Dto;
using EES.Modules.Ums.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Services
{
    /// <summary>
    /// 角色查询服务
    /// </summary>
    public class RoleService : ServerBase, IRoleService
    {
        private readonly IRepository<Role> _roleRepository;

        private readonly ILogger<RoleService> _logger;

        private readonly IRepository<Menu> _menuRepository;

        private readonly IRepository<Entities.Data> _dataRepository;

        private readonly IRepository<Function> _functionRepository;

        private readonly IMapper _mapper;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleRepository"></param>
        /// <param name="logger"></param>
        /// <param name="menuRepository"></param>
        /// <param name="dataRepository"></param>
        /// <param name="functionRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="mediatorHandler"></param>
        public RoleService(IRepository<Role> roleRepository, ILogger<RoleService> logger, IRepository<Menu> menuRepository, IRepository<Entities.Data> dataRepository, IRepository<Function> functionRepository, IMapper mapper, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _roleRepository = roleRepository;
            _logger = logger;
            _menuRepository = menuRepository;
            _dataRepository = dataRepository;
            _functionRepository = functionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 查询角色详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QueryResponse<RoleDto>> GetRoleAsync(long id)
        {
            var role = await _roleRepository.NoTrackingQuery()
                           .Include(x => x.RoleMenus)
                           .Include(x => x.RoleDatas)
                           .Include(x => x.RoleFunctions)
                           .AsSplitQuery()
                           .FirstOrDefaultAsync(x => x.Id == id);

            if (role is null)
            {
                return QueryResponse<RoleDto>.Fail(BusinessError.角色不存在);
            }

            var dto = _mapper.Map<RoleDto>(role);

            var menuIds = role.RoleMenus.Select(x => x.MenuId);

            /*
                为了便利前端展示树状结构，这里的数据需要进行一次筛选，只提供子叶点的数据
             */

            dto.PdaMenus = _menuRepository.NoTrackingQuery()
                           .Where(t => t.Type == MenuType.PDA && t.Category == MenuCategory.Page && menuIds.Contains(t.Id))
                           .Select(x => x.Id);

            dto.PcMenus = _menuRepository.NoTrackingQuery()
                         .Where(t => t.Type == MenuType.PC && t.Category == MenuCategory.Page && menuIds.Contains(t.Id))
                         .Select(x => x.Id);

            dto.MfsMenus = _menuRepository.NoTrackingQuery()
                       .Where(t => t.Type == MenuType.MFS && t.Category == MenuCategory.Page && menuIds.Contains(t.Id))
                       .Select(x => x.Id);

            var dataIds = role.RoleDatas.Select(d => d.DataId);

            dto.Datas = _dataRepository.NoTrackingQuery()
                        .Where(t => t.Category != Enums.DataCategory.工厂 && dataIds.Contains(t.Id))
                        .Select(d => d.Id);

            var funcIds = role.RoleFunctions.Select(f => f.FunctionId);

            dto.Functions = _functionRepository.NoTrackingQuery()
                          .Where(t => t.Type == FunctionType.功能权限 && funcIds.Contains(t.Id))
                          .Select(d => d.Id);

            return QueryResponse<RoleDto>.Success(dto);

        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isGetTotalCount"></param>
        /// <returns></returns>
        public async Task<QueryResponse<PaginationModel<RoleListDto>>> GetRolesAsync(string roleName, int pageSize, int pageIndex, bool isGetTotalCount = true)
        {
            var (totalCount, roles) = await _roleRepository.NoTrackingQuery()                       
                          .WhereIf(!string.IsNullOrWhiteSpace(roleName), t => t.Name.Contains(roleName))
                          .OrderBy(x => x.Id)
                          .GetPagingInTupleAsync(pageIndex, pageSize, isGetTotalCount);

            var pagingModel = new PaginationModel<RoleListDto>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = _mapper.Map<IEnumerable<RoleListDto>>(roles)
            };

            return QueryResponse<PaginationModel<RoleListDto>>.Success(pagingModel);
        }
    }
}
