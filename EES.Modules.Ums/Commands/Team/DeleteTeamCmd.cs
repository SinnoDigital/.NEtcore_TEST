using EES.Infrastructure.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Modules.Ums.Commands.Team
{
    /// <summary>
    /// 删除工作组
    /// </summary>
    public class DeleteTeamCmd : CommandBase
    {

        /// <summary>
        /// 
        /// </summary>
        public DeleteTeamCmd() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public DeleteTeamCmd(long id):base()
        {
            Id = id;
        }



        /// <summary>
        /// Team Ids
        /// </summary>
        public long Id { get; set; }
    }
}
