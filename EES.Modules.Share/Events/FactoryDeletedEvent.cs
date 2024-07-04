using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Events
{
    /// <summary>
    /// 删除工厂事件
    /// </summary>
    public class FactoryDeletedEvent : EventBase
    {

        /// <summary>
        /// 
        /// </summary>
        public FactoryDeletedEvent():base() { }

        /// <summary>
        ///  删除工厂事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorName"></param>
        public FactoryDeletedEvent(long id, long operatorId, string operatorName):base()
        {
            Id = id;
            OperatorId = operatorId;
            OperatorName = operatorName;
        }


        /// <summary>
        /// 工厂id
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
