using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Entities
{
    /// <summary>
    /// 日志模块实体基类
    /// </summary>
    public abstract class LogEntityBase
    {
       
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; init; }


        /// <summary>
        /// 数据记录时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
