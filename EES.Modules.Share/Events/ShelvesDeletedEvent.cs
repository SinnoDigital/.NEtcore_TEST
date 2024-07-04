using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Wms.Events
{
    /// <summary>
    /// 货架删除事件
    /// </summary>
    public class ShelvesDeletedEvent : EventBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ShelvesDeletedEvent() : base() { }

        /// <summary>
        /// 货架删除事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorName"></param>
        public ShelvesDeletedEvent(long id, long operatorId, string operatorName) : base()
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
