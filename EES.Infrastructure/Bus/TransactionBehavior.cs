using EES.Infrastructure.Commons;
using EES.Infrastructure.DataBase;
using EES.Infrastructure.Entities;
using EES.Infrastructure.Enums;
using EES.Infrastructure.Log;
using EES.Infrastructure.Service;
using EES.Infrastructure.Tools;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Bus
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
                                                          where TRequest : IRequest<TResponse> where TResponse : CommandResponse
    {

        private readonly MasterDbContext _context;

        private readonly IMediatorHandler _mediatorHandler;

        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        private readonly ILogDispatchProvider _logDispatchProvider;

        private readonly IWebHostEnvironment _environment;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionBehavior(MasterDbContext context, IMediatorHandler handler, ILogger<TransactionBehavior<TRequest, TResponse>> logger, ILogDispatchProvider logDispatchProvider, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediatorHandler = handler ?? throw new ArgumentNullException(nameof(handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logDispatchProvider = logDispatchProvider ?? throw new ArgumentNullException(nameof(logDispatchProvider));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = default(TResponse);

            try
            {
                if (_context.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _context.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await _context.BeginTransactionAsync();
                    response = await next();

                    if (response.Status)
                    {
                        var events = GetAllDomianEvents();

                        await _context.CommitTransactionAsync(transaction);

                        await PublishDomianEventsAsync(events);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, "sql 执行异常!");

                if (_environment.IsDevelopment())
                {
                    var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier;

                    //response = (TResponse)CommandResponse.Fail($"sql执行异常，错误信息:{ex.Message}.Trace Id:{traceId}", 999);
                    response = (TResponse)CommandResponse.Fail($"服务异常:{ex.InnerException?.Message ?? ex.Message}.Trace Id:{traceId}", 999);
                }
                else
                {
                    response = (TResponse)CommandResponse.Fail(BusinessError.操作失败);
                }

                ClearDomianEvents();

                var log = GetExceptionLog(ex);
                if (log != null)
                {
                    await _logDispatchProvider.PublishAsync(log); //记录异常日志
                }
            }

            return response;
        }


        /// <summary>
        /// 获取所有的领域事件
        /// </summary>
        /// <returns></returns>
        private IEnumerable<EventBase> GetAllDomianEvents()
        {
            var domainEntities = _context.ChangeTracker
              .Entries<EntityBase>()
              .Where(x => x.Entity.GetDomainEvents() != null && x.Entity.GetDomainEvents().Any());

            var domainEvents = domainEntities.SelectMany(x => x.Entity.GetDomainEvents()).ToList();

            domainEntities?.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

            return domainEvents;
        }

        /// <summary>
        /// 清空领域事件
        /// </summary>
        private void ClearDomianEvents()
        {
            var domainEntities = _context.ChangeTracker
             .Entries<EntityBase>()
             .Where(x => x.Entity.GetDomainEvents() != null && x.Entity.GetDomainEvents().Any());

            domainEntities?.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());
        }


        /// <summary>
        /// 发布领域事件
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public async Task PublishDomianEventsAsync(IEnumerable<EventBase> events)
        {
            if (events != null && events.Any())
            {
                foreach (var @event in events)
                {
                    await _mediatorHandler.PublishEventAsync(@event);
                }
            }
        }

        public ExceptionLog GetExceptionLog(Exception ex)
        {
            if (ex is null)
            {
                return null;
            }

            return new ExceptionLog
            {
                ExceptionType = ex.GetGenericTypeName(),
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Flag = Guid.NewGuid(),
                OccurredTime = DateTime.Now,
                Text = ExceptionHelper.GetCompleteErrorMessage(ex),
                TraceId = HttpAccessor.TraceIdentifier
            };
        }
    }
}
