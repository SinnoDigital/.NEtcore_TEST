using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Events
{
    /// <summary>
    /// 区域修改事件
    /// </summary>
    public class AreaModifyEvent : EventBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AreaModifyEvent() : base() { }

        /// <summary>
        /// 区域修改事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="areaName"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorName"></param>
        public AreaModifyEvent(long id, string areaName, long operatorId, string operatorName) : base()
        {
            Id = id;
            AreaName = areaName;
            OperatorId = operatorId;
            OperatorName = operatorName;
        }


        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }


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
