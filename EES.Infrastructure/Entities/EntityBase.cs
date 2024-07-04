using EES.Infrastructure.Bus;
using EES.Infrastructure.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Entities
{
    /// <summary>
    /// 业务实体基类
    /// </summary>
    public abstract class EntityBase
    {

        /// <summary>
        /// CreateTime
        /// </summary>
        /// <param name="createUserName">创建者名称</param>
        /// <param name="createUserId">创建者id</param>
        /// <param name="timeAlignment">时间对齐 CreateTime和UpdateTime是否保持一致</param>
        protected EntityBase(long createUserId = default, string createUserName = "", bool timeAlignment = false)
        {
            CreateUserId = createUserId;
            CreateTime = DateTime.Now;
            CreateUserName = string.IsNullOrEmpty(createUserName) ? string.Empty : createUserName;

            UpdateUserId = default;
            UpdateUserName = "";
            UpdateTime = timeAlignment ? CreateTime : GetDefaultDateTime();
        }

        /// <summary>
        /// id
        /// </summary>
        public long Id { get; init; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建者Id
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 最后一次更新数据的用户Id
        /// </summary>
        public long UpdateUserId { get; set; }

        /// <summary>
        /// 最后一次更新用户的名称
        /// </summary>
        public string UpdateUserName { get; set; }


        /// <summary>
        /// 领域事件
        /// </summary>
        private List<EventBase>? _domainEvents;

        /// <summary>
        /// 获取领域事件
        /// </summary>
        public IReadOnlyCollection<EventBase> GetDomainEvents() => _domainEvents?.AsReadOnly();


        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="eventItem"></param>
        public void AddDomainEvent(EventBase eventItem)
        {
            _domainEvents ??= new();
            _domainEvents.Add(eventItem);
        }

        /// <summary>
        /// 移除领域事件
        /// </summary>
        /// <param name="eventItem"></param>
        public void RemoveDomainEvent(EventBase eventItem) => _domainEvents?.Remove(eventItem);


        /// <summary>
        /// 清空领域事件
        /// </summary>
        public void ClearDomainEvents() => _domainEvents?.Clear();

        /// <summary>
        /// 更改数据后记录更改信息
        /// </summary>
        /// <param name="updateUserId"></param>
        /// <param name="updateUserName"></param>
        protected virtual void UpdateRecord(long updateUserId = default, string updateUserName = "")
        {
            this.UpdateTime = DateTime.Now;
            this.UpdateUserName = string.IsNullOrEmpty(updateUserName) ? string.Empty : updateUserName;
            this.UpdateUserId = updateUserId;
        }

        /// <summary>
        /// 获取一个系统的时间默认值
        /// </summary>
        /// <returns></returns>
        protected static DateTime GetDefaultDateTime()
        {
            return DateTimeHelper.GetDefaultTime();
        }
    }
}
