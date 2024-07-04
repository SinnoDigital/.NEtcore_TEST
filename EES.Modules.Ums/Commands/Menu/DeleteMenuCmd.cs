using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Menu
{
    /// <summary>
    /// 删除资源
    /// </summary>
    public class DeleteMenuCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteMenuCmd() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public DeleteMenuCmd(long id) : base()
        {
            Id = id;
        }

        /// <summary>
        /// 资源id
        /// </summary>
        public long Id { get; set; }
    }
}
