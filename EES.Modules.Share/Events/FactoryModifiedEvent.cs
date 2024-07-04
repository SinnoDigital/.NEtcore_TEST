using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Events
{
    /// <summary>
    /// 修改工厂信息事件
    /// </summary>
    public class FactoryModifiedEvent : EventBase
    {
        public FactoryModifiedEvent():base() { }

        /// <summary>
        /// 修改工厂信息事件
        /// </summary>
        /// <param name="id">工厂id</param>
        /// <param name="name">工厂名字</param>
        /// <param name="code">工厂编码</param>
        /// <param name="operatorId"></param>
        /// <param name="operatorName"></param>
        /// <param name="parentId">上级id.工厂没有上级，则默认传0</param>     
        public FactoryModifiedEvent(long id, string name, string code, long operatorId, string operatorName,long parentId = 0) : base()
        {
            Id = id;
            Name = name;
            Code = code;
            ParentId = parentId;
            OperatorId = operatorId;
            OperatorName = operatorName;
        }


        /// <summary>
        /// 工厂id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工厂名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 上级id.工厂没有上级，则默认传0
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
