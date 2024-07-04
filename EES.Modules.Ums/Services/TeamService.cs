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
    /// 工作组服务
    /// </summary>
    public class TeamService:ServerBase, ITeamService
    {
        private readonly IRepository<Team> _teamRepository;

        private readonly IRepository<User> _userRepository;

         private readonly IRepository<TeamMembers> _teamMembersRepository;

         private  readonly IMapper _mapper;

        private readonly ILogger<TeamService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="teamMembersRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public TeamService(IRepository<Team> teamRepository, IRepository<User> userRepository, IRepository<TeamMembers> teamMembersRepository, IMapper mapper, ILogger<TeamService> logger,IMediatorHandler mediatorHandler):base(mediatorHandler)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _teamMembersRepository = teamMembersRepository;
        }

        /// <summary>
        /// 查询工作组详细数据
        /// </summary>
        /// <param name="id">工作组id</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<QueryResponse<TeamDto>> GetTeamAsync(long id)
        {
            var team = await _teamRepository.NoTrackingQuery().FirstOrDefaultAsync(t => t.Id == id);

            if (team is null)
            {
                return QueryResponse<TeamDto>.Fail(BusinessError.工作组不存在);
            }
            var dto= _mapper.Map<TeamDto>(team);

            var userIds=await _teamMembersRepository.NoTrackingQuery().Where(x=>x.TeamId==team.Id).Select(x=>x.UserId).ToListAsync();

            var users = await _userRepository.NoTrackingQuery().Include(x => x.Department).Include(x => x.Roles).Where(t => userIds.Contains(t.Id)).ToListAsync();

            dto.Users = _mapper.Map<IEnumerable<TeamUserDto>>(users);

            return QueryResponse<TeamDto>.Success(dto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public async Task<QueryResponse<PaginationModel<TeamListDto>>> GetTeamsAsync(TeamListQueryParams queryParams)
        {

            if (queryParams.FactoryIds is null || !queryParams.FactoryIds.Any())
            {
                return QueryResponse<PaginationModel<TeamListDto>>.Fail(BusinessError.请最少选择一个工厂);
            }

            var (totalCount, teams) =await _teamRepository.NoTrackingQuery()
                       .Where(t => queryParams.FactoryIds.Contains(t.FactoryId))
                       .WhereIf(!string.IsNullOrWhiteSpace(queryParams.TeamName), t => t.Name.Contains(queryParams.TeamName))
                       .WhereIf(!string.IsNullOrWhiteSpace(queryParams.AreaCode), t => t.AreaCode.Contains(queryParams.AreaCode))
                       .OrderBy(t => t.Id).GetPagingInTupleAsync(queryParams.PageIndex, queryParams.PageSize, queryParams.IsGetTotalCount);


            var pagingModel = new PaginationModel<TeamListDto>
            {
                PageIndex = queryParams.PageIndex,
                PageSize = queryParams.PageSize,
                TotalCount = totalCount,
                Data = _mapper.Map<IEnumerable<TeamListDto>>(teams)
            };

            return QueryResponse<PaginationModel<TeamListDto>>.Success(pagingModel);

        }
    }
}
