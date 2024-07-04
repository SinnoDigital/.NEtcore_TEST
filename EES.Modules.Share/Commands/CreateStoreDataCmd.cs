using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Commands
{
    /// <summary>
    /// 仓库数据创建
    /// </summary>
    public class CreateStoreDataCmd : CommandBase
    {
        /// <summary>
        /// 仓库数据创建
        /// </summary>
        /// <param name="id">仓库id</param>
        /// <param name="name">仓库名字</param>
        /// <param name="code">仓库编码</param>
        /// <param name="parentId">仓库所属工厂的id</param>
        public CreateStoreDataCmd(long id, string name, string code, long parentId) : base()
        {
            Id = id;
            Name = name;
            Code = code;
            ParentId = parentId;    
        }


        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public  string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public  string Code { get; set; }

        /// <summary>
        /// 上级id. 仓库所属工厂的id
        /// </summary>
        public long ParentId { get; set; }
    }
}
