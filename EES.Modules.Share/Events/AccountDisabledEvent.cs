using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Events
{
    /// <summary>
    /// 员工账号被禁用事件
    /// </summary>
    public class AccountDisabledEvent : EventBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public AccountDisabledEvent(long userId) : base()
        {
            UserId=userId;
        }


        /// <summary>
        /// 被禁用的员工账号id
        /// </summary>
        public long UserId { get; protected set; }
    }
}
