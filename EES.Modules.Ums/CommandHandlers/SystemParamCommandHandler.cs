using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands.SystemParams;
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
    /// 
    /// </summary>
    public class SystemParamCommandHandler : ServerBase,
                                           IRequestHandler<CreateSystemParamCmd, CommandResponse>,
                                           IRequestHandler<ModifySystemParamCmd, CommandResponse>,
                                           IRequestHandler<DeleteSystemParamCmd, CommandResponse>
    {
        private readonly IRepository<Entities.SystemParam> _repository;

        private ILogger<SystemParamCommandHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public SystemParamCommandHandler(IRepository<SystemParam> repository, ILogger<SystemParamCommandHandler> logger, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// 新增系统参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(CreateSystemParamCmd request, CancellationToken cancellationToken)
        {
            var systemParam = await _repository.Query().FirstOrDefaultAsync(t => t.Module == request.Module && t.Code == request.Code, cancellationToken: cancellationToken);

            if (systemParam is null)
            {
                systemParam = new(request.Module, request.IsEnable, request.Code, request.Name, request.Description, request.DefalutValue, request.SetValue, request.Remark, Accessor.Id, Accessor.Name);

                _repository.Add(systemParam);
            }
            else
            {
                systemParam.Modify(request.Module, request.IsEnable, request.Code, request.Name, request.Description, request.DefalutValue, request.SetValue, request.Remark, Accessor.Id, Accessor.Name);
            }

            return CommandResponse.Success();
        }

        /// <summary>
        /// 修改系统参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(ModifySystemParamCmd request, CancellationToken cancellationToken)
        {
            var systemParam = await _repository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (systemParam is null)
            {
                return CommandResponse.Fail(Infrastructure.Enums.BusinessError.参数不存在);
            }

            systemParam.Modify(request.Module, request.IsEnable, request.Code, request.Name, request.Description, request.DefalutValue, request.SetValue, request.Remark, Accessor.Id, Accessor.Name);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 删除系统参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(DeleteSystemParamCmd request, CancellationToken cancellationToken)
        {
            await _repository.Query().Where(t => t.Id == request.Id).ExecuteDeleteAsync(cancellationToken);

            return CommandResponse.Success();
        }
    }
}
