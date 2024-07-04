using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Data
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="TEntity">数据对象</typeparam>
    public class PaginationModel<TEntity> where TEntity : class
    {
        /// <summary>
        /// 页码，从1开始
        /// </summary>
        public int PageIndex { get; set; }     

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 查询结果集总数量 。如果该值为负数，则标识查询时无需返回总数量
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 当前页数据集合
        /// </summary>
        public IEnumerable<TEntity> Data { get; set; }
    }
}
