using EES.Infrastructure.Cache;
using EES.Modules.Share.Events;
using EES.Modules.Ums.Entities;
using EES.Modules.Ums.Services;
using FreeRedis;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.EventHandlers
{
    /// <summary>
    /// 用户事件处理
    /// </summary>
    public class UserEventHandler : INotificationHandler<AccountDisabledEvent>
    {
        private readonly RedisClient _redisClient;

        private readonly ILogger<UserEventHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserEventHandler(RedisClient redisClient, ILogger<UserEventHandler> logger)
        {
            _redisClient = redisClient ?? throw new ArgumentNullException(nameof(redisClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 用户封禁处理
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(AccountDisabledEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("用户封禁事件触发，开始清理缓存. User:{userId}", notification.UserId);

            try
            {
                var pcTokenKey = CacheKeyProvider.GetTokenCacheKey("pc", notification.UserId.ToString());

                if (await _redisClient.ExistsAsync(pcTokenKey))
                {
                   await _redisClient.DelAsync(pcTokenKey);
                }

                var pdaTokenKey= CacheKeyProvider.GetTokenCacheKey("pda", notification.UserId.ToString());

                if (await _redisClient.ExistsAsync(pdaTokenKey))
                {
                    await _redisClient.DelAsync(pdaTokenKey);
                }

                var authKey= CacheKeyProvider.GetAuthCacheKey(notification.UserId.ToString());

                if (await _redisClient.ExistsAsync(authKey))
                {
                    await _redisClient.DelAsync(authKey);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "清理用户缓存异常");
            }


           
        }
    }
}
