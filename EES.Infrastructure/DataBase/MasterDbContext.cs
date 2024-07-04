using EES.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EES.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace EES.Infrastructure.DataBase
{
    /// <summary>
    /// 业务数据库上下文
    /// </summary>
    public class MasterDbContext : DbContext
    {

        public static readonly ILoggerFactory MasterLoggerFactory = LoggerFactory.Create(builder => 
        {
            builder.AddConsole();
            builder.AddFilter((category, level) =>
              category == DbLoggerCategory.Database.Command.Name
              && level == LogLevel.Information);
            builder.AddNLog();
        });

        /// <summary>
        /// 当前事务
        /// </summary>
        private IDbContextTransaction? _currentTransaction;

        /// <summary>
        /// 获取当前事务
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        /// <summary>
        /// 当前DbContext是否开启了事务
        /// </summary>
        public bool HasActiveTransaction => _currentTransaction != null;


        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Type> typeToRegisters = new();
            foreach (var module in GlobalConfiguration.Modules)
            {
                typeToRegisters.AddRange(module.Assembly.DefinedTypes.Select(t => t.AsType()));
            }

            RegisterEntities(modelBuilder, typeToRegisters);

            base.OnModelCreating(modelBuilder);

            RegisterCustomMappings(modelBuilder, typeToRegisters);


        }


        /// <summary>
        /// 注册各个模块的实体
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="typeToRegisters"></param>
        private static void RegisterEntities(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var entityTypes = typeToRegisters.Where(x => x.GetTypeInfo().IsSubclassOf(typeof(EntityBase)) && !x.GetTypeInfo().IsAbstract);
            foreach (var type in entityTypes)
            {
                modelBuilder.Entity(type);
            }
        }


        /// <summary>
        /// 反射执行模块里面的ModelBuilder
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="typeToRegisters"></param>
        private static void RegisterCustomMappings(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var customModelBuilderTypes = typeToRegisters.Where(x => typeof(ICustomModelBuilder).IsAssignableFrom(x));
            foreach (var builderType in customModelBuilderTypes)
            {
                if (builderType != null && builderType != typeof(ICustomModelBuilder))
                {
                    var builder = (ICustomModelBuilder)Activator.CreateInstance(builderType)!;
                    builder.Build(modelBuilder);
                }
            }
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="isolation"></param>
        /// <returns></returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(isolation);

            return _currentTransaction;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
