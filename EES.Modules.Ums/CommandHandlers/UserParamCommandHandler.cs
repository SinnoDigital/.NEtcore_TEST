using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands.UserParams;
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
    /// 用户参数处理
    /// </summary>
    public class UserParamCommandHandler : ServerBase,
                                         IRequestHandler<CreateUserParamCmd, CommandResponse>,
                                         IRequestHandler<ModifyUserParamCmd, CommandResponse>,
                                         IRequestHandler<DeleteUserParamCmd,CommandResponse>
    {

        private readonly IRepository<Entities.UserParam> _repository;

        private ILogger<UserParamCommandHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public UserParamCommandHandler(IRepository<UserParam> repository, ILogger<UserParamCommandHandler> logger, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// 新增用户参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(CreateUserParamCmd request, CancellationToken cancellationToken)
        {
            var userParam = await _repository.Query().FirstOrDefaultAsync(t => t.UserId == Accessor.Id && t.Type == request.Type && t.Code == request.Code, cancellationToken: cancellationToken);

            if (userParam is null)
            {
                userParam = new(Accessor.Id, request.Type, request.IsEnable, request.Code, request.Name, request.Description, request.Unit, request.ValueType, request.DefalutValue, request.SetValue, request.Remark, Accessor.Id, Accessor.Name);

                await _repository.AddAsync(userParam);
            }
            else
            {
                userParam.Modify(request.Type,request.IsEnable, request.Code, request.Name, request.Description, request.Unit, request.ValueType, request.DefalutValue, request.SetValue, request.Remark, Accessor.Id, Accessor.Name);
            }


            return CommandResponse.Success();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(ModifyUserParamCmd request, CancellationToken cancellationToken)
        {
            var userParam = await _repository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (userParam is null)
            {
                return CommandResponse.Fail(Infrastructure.Enums.BusinessError.参数不存在);
            }

            if (Accessor.Id != userParam.UserId)
            {
                return CommandResponse.Fail(Infrastructure.Enums.BusinessError.无权操作此数据);
            }

            userParam.Modify(request.Type,request.IsEnable, request.Code, request.Name, request.Description, request.Unit, request.ValueType, request.DefalutValue, request.SetValue, request.Remark, Accessor.Id, Accessor.Name);

            return CommandResponse.Success();
        }


        /// <summary>
        /// 删除用户参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(DeleteUserParamCmd request, CancellationToken cancellationToken)
        {
            var userParam = await _repository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (userParam is null)
            {
                return CommandResponse.Fail(Infrastructure.Enums.BusinessError.参数不存在);
            }

            if (Accessor.Id != userParam.UserId)
            {
                return CommandResponse.Fail(Infrastructure.Enums.BusinessError.无权操作此数据);
            }

            await _repository.DeleteAsync(userParam);

            return CommandResponse.Success();
        }
    }
}
