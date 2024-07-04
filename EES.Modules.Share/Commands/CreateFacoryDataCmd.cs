using EES.Infrastructure.Bus;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Share.Commands
{
    /// <summary>
    /// 创建工厂数据
    /// </summary>
    public class CreateFacoryDataCmd : CommandBase
    {
        /// <summary>
        /// 创建工厂数据
        /// </summary>
        /// <param name="id">工厂id</param>
        /// <param name="name">工厂名字</param>
        /// <param name="code">工厂编码</param>
        /// <param name="parentId">上级id.工厂没有上级，则默认传0</param>
        public CreateFacoryDataCmd(long id, string name, string code, long parentId = 0) : base()
        {
            Id = id;
            Name = name;
            Code = code;
            ParentId = parentId;
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
    }
}
