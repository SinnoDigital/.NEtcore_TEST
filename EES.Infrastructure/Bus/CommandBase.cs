using EES.Infrastructure.Commons;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EES.Infrastructure.Bus
{
    /// <summary>
    /// Command基类
    /// </summary>
    public abstract class CommandBase : IRequest<CommandResponse>
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        protected DateTime Timestamp { get; init; }

        protected CommandBase()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public string SerializeCommand() =>JsonConvert.SerializeObject(this);

        /// <summary>
        /// 获取命令名称
        /// </summary>
        /// <returns></returns>
        public string GetCommandName() => this.GetGenericTypeName();

        /// <summary>
        /// 获取命令id
        /// </summary>
        /// <returns></returns>
        public virtual long GetCommandId() => this.Timestamp.ToUnixTimestamp();
    }
}
