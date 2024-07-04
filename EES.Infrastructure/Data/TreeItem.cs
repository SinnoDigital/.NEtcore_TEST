using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Data
{
    /// <summary>
    /// 树形结构模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeItem<T>
    {
        /// <summary>
        /// 当前节点
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
}
