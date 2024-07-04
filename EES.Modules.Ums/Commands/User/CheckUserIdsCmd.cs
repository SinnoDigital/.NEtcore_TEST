using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.User
{
    /// <summary>
    /// 检查用户id
    /// </summary>
    public class CheckUserIdsCmd : CommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckUserIdsCmd() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userIds"></param>
        public CheckUserIdsCmd(IEnumerable<long> userIds):base()
        {
            UserIds = userIds;
        }

        public IEnumerable<long> UserIds { get; set; }
    }
}
