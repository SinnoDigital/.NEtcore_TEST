using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Service;
using EES.Modules.Share.Commands;
using EES.Modules.Ums.Commands.Data;
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
    /// 数据权限处理
    /// </summary>
    public class DataCommandHandler : ServerBase,
                                    IRequestHandler<CreateFacoryDataCmd, CommandResponse>,
                                    IRequestHandler<CreateStoreDataCmd, CommandResponse>,
                                    IRequestHandler<CreateWorkshopDataCmd, CommandResponse>,
                                    IRequestHandler<CreateMaterialDataCmd,CommandResponse>,
                                    IRequestHandler<DeleteMaterialDataCmd, CommandResponse>
    {

        private readonly IRepository<Entities.Data> _dataRepository;

        private ILogger<DataCommandHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRepository"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public DataCommandHandler(IRepository<Entities.Data> dataRepository, ILogger<DataCommandHandler> logger, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _dataRepository = dataRepository;
            _logger = logger;
        }

        /// <summary>
        /// 添加仓库数据权限
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(CreateStoreDataCmd request, CancellationToken cancellationToken)
        {
            var storeData=new Entities.Data(Enums.DataCategory.仓库,request.Id,request.Code,request.Name,request.ParentId,Accessor.Id,Accessor.Name);

            await _dataRepository.AddRangeAsync(storeData);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 添加车间数据权限
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(CreateWorkshopDataCmd request, CancellationToken cancellationToken)
        {
            var workshopData = new Entities.Data(Enums.DataCategory.车间, request.Id, request.Code, request.Name, request.ParentId, Accessor.Id, Accessor.Name);

            await _dataRepository.AddRangeAsync(workshopData);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 添加工厂数据权限
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommandResponse> Handle(CreateFacoryDataCmd request, CancellationToken cancellationToken)
        {
            var factoryData = new Entities.Data(Enums.DataCategory.工厂, request.Id, request.Code, request.Name, request.ParentId, Accessor.Id, Accessor.Name);

            await _dataRepository.AddAsync(factoryData);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 创建物料类型数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(CreateMaterialDataCmd request, CancellationToken cancellationToken)
        {
            var data = new Entities.Data(Enums.DataCategory.物料类型,(long)request.MaterialType, request.Code, request.Name, 0, Accessor.Id, Accessor.Name);

            await _dataRepository.AddAsync(data);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 修改物料类型数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(ModifyMaterialDataCmd request, CancellationToken cancellationToken)
        {
            var data = await _dataRepository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (data is null)
            {
                return CommandResponse.Fail(Infrastructure.Enums.BusinessError.物料类型资源不存在);
            }

            data.Modify((long)request.MaterialType, request.Code, request.Name, 0, Accessor.Id, Accessor.Name);

            return CommandResponse.Success();
        }

        /// <summary>
        /// 删除物料类型数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(DeleteMaterialDataCmd request, CancellationToken cancellationToken)
        {
            var data = await _dataRepository.Query().FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

            if (data is null)
            {
                return CommandResponse.Fail(Infrastructure.Enums.BusinessError.物料类型资源不存在);
            }

            await _dataRepository.DeleteAsync(data);

            return CommandResponse.Success();
        }
    }
}
