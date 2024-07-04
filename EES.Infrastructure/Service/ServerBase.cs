using EES.Infrastructure.Auth;
using EES.Infrastructure.Bus;
using EES.Infrastructure.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Service
{
    public class ServerBase
    {
        private readonly IMediatorHandler _mediatorHandler;

        /// <summary>
        /// 当前请求用户信息
        /// </summary>
        public static Accessor Accessor => HttpAccessor.Accessor;

        public ServerBase(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task PublishEventAsync<T>(T @event) where T : EventBase
        {
            await _mediatorHandler.PublishEventAsync(@event);
        }

        public async Task<CommandResponse> SendCommandAsync<T>(T command) where T : CommandBase
        {
            return await _mediatorHandler.SendCommandAsync(command);
        }


    }
}
