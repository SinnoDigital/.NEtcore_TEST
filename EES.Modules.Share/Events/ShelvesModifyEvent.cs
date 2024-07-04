using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Events
{
    /// <summary>
    /// 货架修改事件
    /// </summary>
    public class ShelvesModifyEvent : EventBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ShelvesModifyEvent() : base() { }

        /// <summary>
        /// 货架修改事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shelvesName"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorName"></param>
        public ShelvesModifyEvent(long id, string shelvesName, long operatorId, string operatorName) : base()
        {
            Id = id;
            ShelvesName = shelvesName;
            OperatorId = operatorId;
            OperatorName = operatorName;
        }


        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 货架名称
        /// </summary>
        public string ShelvesName { get; set; }


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
