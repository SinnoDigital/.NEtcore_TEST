using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands.Team;
using EES.Modules.Ums.Commands.User;
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
    /// 工作组处理
    /// </summary>
    public class TeamCommandHandler : ServerBase,
                                    IRequestHandler<CreateTeamCmd, CommandResponse>,
                                    IRequestHandler<ModifyTeamCmd, CommandResponse>,
                                    IRequestHandler<DeleteTeamCmd, CommandResponse>
    {
        private readonly IRepository<Entities.Team> _teamRepository;

        private readonly IRepository<Entities.TeamMembers> _membersRepository;

        private ILogger<TeamCommandHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamRepository"></param>
        /// <param name="membersRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public TeamCommandHandler(IRepository<Team> teamRepository, IRepository<TeamMembers> membersRepository, ILogger<TeamCommandHandler> logger, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _teamRepository = teamRepository;
            _membersRepository = membersRepository;
            _logger = logger;
        }


        /// <summary>
        /// 新增工作组
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(CreateTeamCmd request, CancellationToken cancellationToken)
        {
            if (await CheckCodeExistAsync(request.Code))
            {
                return CommandResponse.Fail(BusinessError.工作组编码重复);
            }

            if (await CheckNameExistAsync(request.Name))
            {
                return CommandResponse.Fail(BusinessError.工作组名称重复);
            }

            var checkUserCmd = new CheckUserIdsCmd(request.UserIds);

            var checkUserRes = await SendCommandAsync(checkUserCmd);

            if (checkUserRes != null && checkUserRes.Status)
            {
                var res = bool.Parse(checkUserRes.Data.ToString());

                if (!res)
                {
                    return CommandResponse.Fail(BusinessError.用户数据错误);
                }
            }
            else
            {
                CommandResponse.Fail(BusinessError.服务器异常);
            }

            var team = new Team(request.Code, request.Name, request.Type, request.FactoryCode, request.FactoryName, request.FactoryId, request.AreaCode, request.AreaName, request.AreaId, request.Description, Accessor.Id, Accessor.Name);

            _teamRepository.Add(team);

            await _teamRepository.SaveChangesAsync();

            await DealTeamMembersAsync(team.Id, request.UserIds);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 修改工作组
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(ModifyTeamCmd request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (team is null)
            {
                return CommandResponse.Fail(BusinessError.工作组不存在);
            }

            if (await CheckCodeExistAsync(request.Code, team.Id))
            {
                return CommandResponse.Fail(BusinessError.工作组编码重复);
            }

            if (await CheckNameExistAsync(request.Name, team.Id))
            {
                return CommandResponse.Fail(BusinessError.工作组名称重复);
            }

            var checkUserCmd = new CheckUserIdsCmd(request.UserIds);

            var checkUserRes = await SendCommandAsync(checkUserCmd);

            if (checkUserRes != null && checkUserRes.Status)
            {
                var res = bool.Parse(checkUserRes.Data.ToString());

                if (!res)
                {
                    return CommandResponse.Fail(BusinessError.用户数据错误);
                }
            }
            else
            {
                CommandResponse.Fail(BusinessError.服务器异常);
            }

            team.Modify(request.Code, request.Name, request.Type, request.FactoryCode, request.FactoryName, request.FactoryId, request.AreaCode, request.AreaName, request.AreaId, request.Description, Accessor.Id, Accessor.Name);

            await DealTeamMembersAsync(team.Id, request.UserIds); //处理组员信息

            return CommandResponse.Success();
        }

        /// <summary>
        /// 删除工作组
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(DeleteTeamCmd request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (team is null)
            {
                return CommandResponse.Fail(BusinessError.工作组不存在);
            }

            await ClearTeamMembersAsync(team.Id);

            await _teamRepository.DeleteAsync(team);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 检查编码是否存在
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="excludedDepartmentId">被排除，不参与检查的部门的id</param>
        /// <returns></returns>
        private async Task<bool> CheckCodeExistAsync(string code, long excludedDepartmentId = 0)
        {
            return string.IsNullOrEmpty(code) || await _teamRepository.Query().AnyAsync(t => t.Code == code && t.Id != excludedDepartmentId);
        }

        /// <summary>
        /// 检查工作组名称是否存在
        /// </summary>
        /// <param name="name">工作组名称</param>
        /// <param name="excludedDepartmentId">被排除，不参与检查的部门的id</param>
        /// <returns></returns>
        private async Task<bool> CheckNameExistAsync(string name, long excludedDepartmentId = 0)
        {
            return string.IsNullOrEmpty(name) || await _teamRepository.Query().AnyAsync(t => t.Name == name && t.Id != excludedDepartmentId);
        }


        /// <summary>
        /// 删除所有组员
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private async Task ClearTeamMembersAsync(long teamId)
        {
            await _membersRepository.Query().Where(t => t.TeamId == teamId).ExecuteDeleteAsync();
        }

        /// <summary>
        /// 组员处理
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        private async Task DealTeamMembersAsync(long teamId, IEnumerable<long> userIds)
        {
            await ClearTeamMembersAsync(teamId); //添加之前先默认把组里面之前的组员全部清空掉。免得去挨个对比。

            var members = TeamMembers.GenerateUserRoles(teamId, userIds, Accessor.Id, Accessor.Name);

            await _membersRepository.AddRangeAsync(members);
        }


    }
}
