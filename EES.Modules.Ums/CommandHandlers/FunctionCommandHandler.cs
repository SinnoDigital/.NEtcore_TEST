using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Service;
using EES.Modules.Ums.Commands.Funtion;
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
    /// 功能权限处理
    /// </summary>
    public class FunctionCommandHandler : ServerBase,
                                        IRequestHandler<CreateFuntionCmd, CommandResponse>,
                                        IRequestHandler<ModifyFunctionCmd, CommandResponse>,
                                        IRequestHandler<DeleteFunctionCmd,CommandResponse>
                                    
    {
        private readonly IRepository<Entities.Function> _functionRepository;

        private readonly IRepository<RoleFunctions> _roleFunctionsrepository;

        private ILogger<FunctionCommandHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionRepository"></param>
        /// <param name="roleFunctionsrepository"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public FunctionCommandHandler(IRepository<Function> functionRepository, IRepository<RoleFunctions> roleFunctionsrepository,ILogger<FunctionCommandHandler> logger,IMediatorHandler mediatorHandler):base(mediatorHandler)
        {
            _functionRepository = functionRepository;
            _logger = logger;
            _roleFunctionsrepository= roleFunctionsrepository;
        }

        /// <summary>
        /// 新建功能权限数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(CreateFuntionCmd request, CancellationToken cancellationToken)
        {
            if (! await CheckParentFunctionExistsAsync(request.ParentId))
            {
                return CommandResponse.Fail(BusinessError.上级功能资源不存在);
            }

            var function = new Function(request.Name, request.Type, request.ParentId, request.Description, request.Identifier, Accessor.Id, Accessor.Name);

            await _functionRepository.AddAsync(function);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(ModifyFunctionCmd request, CancellationToken cancellationToken)
        {
            var function = await _functionRepository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (function is null)
            {
                return CommandResponse.Fail(BusinessError.功能资源不存在);
            }

            if (!await CheckParentFunctionExistsAsync(request.ParentId))
            {
                return CommandResponse.Fail(BusinessError.上级功能资源不存在);
            }

            function.Modify(request.Name, request.Type, request.ParentId, request.Description, request.Identifier, Accessor.Id, Accessor.Name);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 删除数据权限
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(DeleteFunctionCmd request, CancellationToken cancellationToken)
        {
            var function = await _functionRepository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (function is null)
            {
                return CommandResponse.Fail(BusinessError.功能资源不存在);
            }

            var isHasChild = await _functionRepository.Query().AnyAsync(t => t.ParentId == function.Id, cancellationToken: cancellationToken);

            if (isHasChild)
            {
                return CommandResponse.Fail(BusinessError.请先删除资源下的子资源);
            }

            var isUsed = await _roleFunctionsrepository.Query().AnyAsync(t => t.FunctionId == function.Id, cancellationToken: cancellationToken);

            if (isUsed)
            {
                return CommandResponse.Fail(BusinessError.资源正在被使用);
            }

            _functionRepository.Delete(function);

            return CommandResponse.Success();
        }



        /// <summary>
        /// 检查上级是否存在
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private async Task<bool> CheckParentFunctionExistsAsync(long parentId)
        {
            return parentId == 0 || await _functionRepository.Query().AnyAsync(t => t.Id == parentId);
        }
    }
}
