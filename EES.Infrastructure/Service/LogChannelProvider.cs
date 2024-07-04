using EES.Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace EES.Infrastructure.Service
{
    /// <summary>
    /// 日志管道，记录程序日志
    /// </summary>
    public class LogChannelProvider : ILogDispatchProvider
    {
        private readonly Channel<LogEntityBase> _logChannel;

        private readonly ILogger<LogChannelProvider> _logger;

        public LogChannelProvider(Channel<LogEntityBase> logChannel, ILogger<LogChannelProvider> logger)
        {
            _logChannel = logChannel;
            _logger = logger;
        }

        public void Publish(LogEntityBase message)
        {
            if (!_logChannel.Writer.TryWrite(message))
            {
                _logger.LogInformation("尝试写入消息失败,message:{message}", JsonConvert.SerializeObject(message));
            } 
        }

        public async Task PublishAsync(LogEntityBase message)
        {
            await _logChannel.Writer.WriteAsync(message); 
                     
        }
    }
}
