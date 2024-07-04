using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands.UserRoles;
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
    /// 用户-角色绑定关系
    /// </summary>
    public class UserRolesCommandHandler : ServerBase,
                                          IRequestHandler<CreateUserRolesCmd, CommandResponse>,
                                          IRequestHandler<DeleteUserRolesCmd, CommandResponse>
    {
        private readonly IRepository<UserRoles> _userRolesRepository;

        private readonly ILogger<UserCommandHadler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRolesRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public UserRolesCommandHandler(IRepository<UserRoles> userRolesRepository, ILogger<UserCommandHadler> logger, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _userRolesRepository = userRolesRepository;
            _logger = logger;
        }

        /// <summary>
        /// 添加用户-角色绑定关系
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(CreateUserRolesCmd request, CancellationToken cancellationToken)
        {
            //需要先整个清除之前所有的绑定关系，再完整添加。
            var res = await _userRolesRepository.Query().Where(t => t.UserId == request.UserId).ExecuteDeleteAsync(cancellationToken: cancellationToken);

            var mappings = UserRoles.GenerateUserRoles(request.UserId, request.RoleIds, Accessor.Id, Accessor.Name);

            await _userRolesRepository.AddRangeAsync(mappings);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 删除用户的全部角色绑定关系
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(DeleteUserRolesCmd request, CancellationToken cancellationToken)
        {
            _ = await _userRolesRepository.Query().Where(t => t.UserId == request.UserId).ExecuteDeleteAsync(cancellationToken: cancellationToken);

            return CommandResponse.Success();
        }
    }
}
