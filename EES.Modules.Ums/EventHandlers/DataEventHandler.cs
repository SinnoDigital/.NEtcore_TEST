using EES.Infrastructure.Bus;
using EES.Infrastructure.DataBase.Repository;
using EES.Infrastructure.Service;
using EES.Modules.Share.Events;
using EES.Modules.Ums.CommandHandlers;
using EES.Modules.Ums.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.EventHandlers
{
    /// <summary>
    /// 权限数据事件处理
    /// </summary>
    public class DataEventHandler :
                                  INotificationHandler<FactoryDeletedEvent>,
                                  INotificationHandler<FactoryModifiedEvent>,
                                  INotificationHandler<StoreDeletedEvent>,
                                  INotificationHandler<StoreModifiedEvent>,
                                  INotificationHandler<WorkShopDeletedEvent>,
                                  INotificationHandler<WorkShopModifiedEvent>
    {

        private readonly IRepository<Entities.Data> _dataRepository;

        private readonly IRepository<RoleDatas> _roleDatasRepository;

        private ILogger<DataEventHandler> _logger;

        /// <summary>
        /// 权限数据事件处理
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public DataEventHandler(IServiceProvider provider, ILogger<DataEventHandler> logger, IMediatorHandler mediatorHandler)
        {
            var scope = provider.CreateScope();

            _dataRepository = scope.ServiceProvider.GetService<IRepository<Entities.Data>>()!;

            _roleDatasRepository = scope.ServiceProvider.GetService<IRepository<Entities.RoleDatas>>()!;
            _logger = logger;
        }

        /// <summary>
        /// 工厂数据修改事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(FactoryModifiedEvent notification, CancellationToken cancellationToken)
        {
            var data = await _dataRepository.Query().FirstOrDefaultAsync(t => t.Category == Enums.DataCategory.工厂 && t.ObjectId == notification.Id, cancellationToken: cancellationToken);

            if (data is null)
            {
                _logger.LogInformation(message: "未查询到工厂的数据,Event:{event}", notification.SerializeEvent());
                return;
            }

            data.Modify(notification.Id, notification.Code, notification.Name, notification.ParentId, notification.OperatorId, notification.OperatorName);

            await _dataRepository.SaveChangesAsync();
        }

        /// <summary>
        /// 工厂删除事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(FactoryDeletedEvent notification, CancellationToken cancellationToken)
        {
            /*
                 删除权限模块的工厂数据时，需要将 RoleDatas表的数据一起删除。

                 这里演示使用导航属性进行全部删除。
                 
                 同一个SaveChangs操作，默认是在同一个事务内
             */
            var data = await _dataRepository.Query().Include(x => x.RoleDatas).FirstOrDefaultAsync(t => t.Category == Enums.DataCategory.工厂 && t.ObjectId == notification.Id, cancellationToken: cancellationToken);

            if (data is null)
            {
                _logger.LogInformation(message: "未查询到工厂的数据,Event:{event}", notification.SerializeEvent());
                return;
            }

            data.RoleDatas.Clear();

            _dataRepository.Delete(data);

            await _dataRepository.SaveChangesAsync(); // 一起保存
        }

        /// <summary>
        /// 仓库删除事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(StoreDeletedEvent notification, CancellationToken cancellationToken)
        {

            var store = await _dataRepository.Query().Include(x => x.RoleDatas).FirstOrDefaultAsync(t => t.Category == Enums.DataCategory.仓库 && t.ObjectId == notification.Id, cancellationToken: cancellationToken);

            if (store is null)
            {
                _logger.LogInformation(message: "未查询到仓库的数据,Event:{event}", notification.SerializeEvent());
                return;
            }

            store.RoleDatas.Clear();

            _dataRepository.Delete(store);

            await _dataRepository.SaveChangesAsync();
        }

        /// <summary>
        /// 仓库修改事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(StoreModifiedEvent notification, CancellationToken cancellationToken)
        {
            var store = await _dataRepository.Query().FirstOrDefaultAsync(t => t.Category == Enums.DataCategory.仓库 && t.ObjectId == notification.Id, cancellationToken: cancellationToken);

            if (store is null)
            {
                _logger.LogInformation(message: "未查询到仓库的数据,Event:{event}", notification.SerializeEvent());
                return;
            }

            store.Modify(notification.Id, notification.Code, notification.Name, notification.ParentId, notification.OperatorId, notification.OperatorName);

            await _dataRepository.SaveChangesAsync();
        }

        /// <summary>
        /// 车间删除事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(WorkShopDeletedEvent notification, CancellationToken cancellationToken)
        {
            var workshop = await _dataRepository.Query().Include(x => x.RoleDatas).FirstOrDefaultAsync(t => t.Category == Enums.DataCategory.车间 && t.ObjectId == notification.Id, cancellationToken: cancellationToken);

            if (workshop is null)
            {
                _logger.LogInformation(message: "未查询到车间的数据,Event:{event}", notification.SerializeEvent());
                return;
            }

            workshop.RoleDatas.Clear();

            _dataRepository.Delete(workshop);

            await _dataRepository.SaveChangesAsync(); // 一起保存
        }

        /// <summary>
        /// 车间信息修改事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(WorkShopModifiedEvent notification, CancellationToken cancellationToken)
        {
            var workshop = await _dataRepository.Query().FirstOrDefaultAsync(t => t.Category == Enums.DataCategory.车间 && t.ObjectId == notification.Id, cancellationToken: cancellationToken);

            if (workshop is null)
            {
                _logger.LogInformation(message: "未查询到车间的数据,Event:{event}", notification.SerializeEvent());
                return;
            }

            workshop.Modify(notification.Id, notification.Code, notification.Name, notification.ParentId, notification.OperatorId, notification.OperatorName);

            await _dataRepository.SaveChangesAsync();
        }
    }
}
