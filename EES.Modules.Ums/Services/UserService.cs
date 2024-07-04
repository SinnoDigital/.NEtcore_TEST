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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : ServerBase, IUserService
    {
        private readonly IRepository<User> _userRepository;

        private readonly ILogger<UserService> _logger;

        private readonly IRepository<Department> _departmenRepository;

        private readonly IRepository<Role> _roleRepository;

        private readonly IRepository<UserRoles> _userRolesRepository;

        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="departmenRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="userRolesRepository"></param>
        /// <param name="handler"></param>
        public UserService(IRepository<User> userRepository, ILogger<UserService> logger,
                           IMapper mapper,
                           IRepository<Department> departmenRepository,
                           IRepository<Role> roleRepository,
                           IRepository<UserRoles> userRolesRepository,
                           IMediatorHandler handler) : base(handler)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _departmenRepository = departmenRepository;
            _roleRepository = roleRepository;
            _userRolesRepository = userRolesRepository;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public async Task<QueryResponse<UserDto>> GetUserAsync(long userId)
        {
            var user = await _userRepository.NoTrackingQuery().Include(x => x.Roles).FirstOrDefaultAsync(t => t.Id == userId);

            if (user is null)
            {
                _logger.LogInformation("用户不存在！id：{id}",userId);
                return QueryResponse<UserDto>.Fail(BusinessError.用户不存在);
            }

            var dto = _mapper.Map<UserDto>(user);

            return QueryResponse<UserDto>.Success(dto);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public async Task<QueryResponse<PaginationModel<UserListDto>>> GetUsersAsync(UserListQueryParam queryParam)
        {
            var (totalCount, users) = await _userRepository.NoTrackingQuery()
                                     .Include(x => x.Department)
                                     .Include(x => x.Roles)
                                     .AsSplitQuery()
                                     .WhereIf(!string.IsNullOrWhiteSpace(queryParam.Account),t=>t.Account.Contains(queryParam.Account))
                                     .WhereIf(!string.IsNullOrWhiteSpace(queryParam.Name),t=>t.Name.Contains(queryParam.Name))
                                     .OrderBy(t => t.State)
                                     .ThenBy(x => x.CreateTime)                                    
                                     .GetPagingInTupleAsync(queryParam.PageIndex, queryParam.PageSize,queryParam.IsGetTotalCount);

            var pagingModel = new PaginationModel<UserListDto>
            {
                PageIndex = queryParam.PageIndex,
                PageSize = queryParam.PageSize,
                TotalCount = totalCount,
                Data = _mapper.Map<IEnumerable<UserListDto>>(users)
            };

            return QueryResponse<PaginationModel<UserListDto>>.Success(pagingModel);
        }
    }
}
