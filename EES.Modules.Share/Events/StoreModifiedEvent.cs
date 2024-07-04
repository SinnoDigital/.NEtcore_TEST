using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Events
{
    /// <summary>
    /// 仓库信息修改事件
    /// </summary>
    public class StoreModifiedEvent : EventBase
    {
        /// <summary>
        /// 
        /// </summary>
        public StoreModifiedEvent() : base() { }

        /// <summary>
        /// 仓库信息修改事件
        /// </summary>
        /// <param name="id">仓库id</param>
        /// <param name="name">仓库名字</param>
        /// <param name="code">仓库编码</param>
        /// <param name="parentId">仓库所属工厂的id</param>
        /// <param name="operatorId"></param>
        /// <param name="operatorName"></param>
        public StoreModifiedEvent(long id, string name, string code, long parentId, long operatorId, string operatorName) : base()
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
        /// 上级id. 仓库所属工厂的id
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
