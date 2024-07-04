using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Commands
{
    /// <summary>
    /// 创建车间数据
    /// </summary>
    public class CreateWorkshopDataCmd : CommandBase
    {
        /// <summary>
        /// 创建车间数据
        /// </summary>
        /// <param name="id">车间id</param>
        /// <param name="name">车间名称</param>
        /// <param name="code">车间编码</param>
        /// <param name="parentId">车间所属工厂id</param>
        public CreateWorkshopDataCmd(long id, string name, string code,long parentId) : base()
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
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 车间所属的工厂id
        /// </summary>
        public long ParentId { get; set; }
       
    }
}
