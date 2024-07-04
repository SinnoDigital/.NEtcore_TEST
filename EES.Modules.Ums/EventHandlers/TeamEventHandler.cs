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
    /// 工作组的事件处理
    /// </summary>
    public class TeamEventHandler :
                                  INotificationHandler<FactoryModifiedEvent>,
                                  INotificationHandler<StoreModifiedEvent>,
                                  INotificationHandler<WorkShopModifiedEvent>
    {
        private readonly IRepository<Entities.Team> _teamRepository;

        private ILogger<TeamEventHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        /// <param name="mediatorHandler"></param>
        public TeamEventHandler(IServiceProvider provider, ILogger<TeamEventHandler> logger, IMediatorHandler mediatorHandler) 
        {
            var scope = provider.CreateScope();

            _teamRepository = scope.ServiceProvider.GetService<IRepository<Entities.Team>>()!;
            _logger = logger;
        }

        /// <summary>
        /// 修改工作组的工厂数据
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(FactoryModifiedEvent notification, CancellationToken cancellationToken)
        {
            var @event = notification.SerializeEvent();

            _logger.LogInformation("TeamEventHandler Receive FactoryModified Event! Event:{event}", @event);

            var teams = await _teamRepository.Query().Where(t => t.FactoryId == notification.Id).ToListAsync(cancellationToken: cancellationToken);

            if (!teams.Any())
            {
                _logger.LogInformation("Can't find any factory in team. Factory Ids:{id}", notification.Id);
                return;
            }

            teams.ForEach(t => t.ModifyFactoryInfo(notification.Code, notification.Name));

            await _teamRepository.SaveChangesAsync();
        }

        /// <summary>
        /// 修改工作组内的车间信息
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(StoreModifiedEvent notification, CancellationToken cancellationToken)
        {

            var @event = notification.SerializeEvent();

            _logger.LogInformation("TeamEventHandler Receive StoreModified Event! Event:{event}", @event);

            var teams = await _teamRepository.Query().Where(t => t.Type == Enums.TeamType.仓库 && t.AreaId == notification.Id).ToListAsync(cancellationToken: cancellationToken);

            if (!teams.Any()) 
            {
                _logger.LogInformation("Can't find any store in team. Store id:{id}", notification.Id);
                return;
            }

            teams.ForEach(t => t.ModifiyAreaInfo(notification.Code, notification.Name));

            await _teamRepository.SaveChangesAsync();
        }

        /// <summary>
        /// 修改工作组的车间信息
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async  Task Handle(WorkShopModifiedEvent notification, CancellationToken cancellationToken)
        {
            var @event = notification.SerializeEvent();

            _logger.LogInformation("TeamEventHandler receive WorkShopModified Event! Event:{event}", @event);

            var teams = await _teamRepository.Query().Where(t => t.Type == Enums.TeamType.车间 && t.AreaId == notification.Id).ToListAsync(cancellationToken: cancellationToken);

            if (!teams.Any())
            {
                _logger.LogInformation("Can't find any workshop in team. Workshop id:{id}", notification.Id);
                return;
            }

            teams.ForEach(t => t.ModifiyAreaInfo(notification.Code, notification.Name));

            await _teamRepository.SaveChangesAsync();
        }
    }
}
