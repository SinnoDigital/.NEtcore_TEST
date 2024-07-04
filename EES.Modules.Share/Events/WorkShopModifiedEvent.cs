using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Events
{
    /// <summary>
    /// 修改车间信息事件
    /// </summary>
    public class WorkShopModifiedEvent : EventBase
    {
        public WorkShopModifiedEvent() : base() { }

        /// <summary>
        /// 修改车间信息事件
        /// </summary>
        /// <param name="id">车间id</param>
        /// <param name="name">车间名称</param>
        /// <param name="code">车间编码</param>
        /// <param name="parentId">车间所属工厂id</param>
        /// <param name="operatorId"></param>
        /// <param name="operatorName"></param>
        public WorkShopModifiedEvent(long id, string name, string code, long parentId, long operatorId, string operatorName) : base()
        {
            Id = id;
            Name = name;
            Code = code;
            ParentId = parentId;
            OperatorId = operatorId;
            OperatorName = operatorName;
        }

        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 车间所属的工厂id
        /// </summary>
        public long ParentId { get; set; }

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
