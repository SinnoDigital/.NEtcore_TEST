using EES.Infrastructure.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Bus
{
    /// <summary>
    /// Mediator接口
    /// </summary>
    public interface IMediatorHandler
    {
        /// <summary>
        /// 发送命令，将命令模型发布到中介者模块
        /// </summary>
        /// <typeparam name="TCommand"> 泛型 </typeparam>
        /// <param name="command"> 命令模型</param>
        /// <returns></returns>
        Task<CommandResponse> SendCommandAsync<TCommand>(TCommand command) where TCommand : CommandBase;

        /// <summary>
        /// 引发事件，通过总线，发布事件
        /// </summary>
        /// <typeparam name="TEvent"> 泛型</typeparam>
        /// <param name="event"> 事件模型</param>
        /// <returns></returns>
        Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : EventBase;
    }
}
