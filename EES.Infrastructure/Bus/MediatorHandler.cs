using EES.Infrastructure.Commons;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Bus
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task PublishEventAsync<T>(T @event) where T : EventBase
        {
            await _mediator.Publish(@event);
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<CommandResponse> SendCommandAsync<T>(T command) where T : CommandBase
        {
            return await _mediator.Send(command);
        }
    }
}
