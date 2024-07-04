using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Wms.Events
{
    /// <summary>
    /// 区域删除事件
    /// </summary>
    public class AreaDeletedEvent : EventBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AreaDeletedEvent() : base() { }

        /// <summary>
        /// 区域删除事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorName"></param>
        public AreaDeletedEvent(long id, long operatorId, string operatorName) : base()
        {
            Id = id;
            OperatorId = operatorId;
            OperatorName = operatorName;
        }


        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        /// 操作人id
        /// </summary>
        public long OperatorId { get; set; }

        /// <summary>
        /// 操作人名字
        /// </summary>
        public string OperatorName { get; set; }
    }
}
