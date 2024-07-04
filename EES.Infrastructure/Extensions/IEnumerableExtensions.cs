using EES.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 列表生成树形节点
        /// </summary>
        /// <typeparam name="T">集合对象的类型</typeparam>
        /// <typeparam name="K">父节点的类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="idSelector">主键ID</param>
        /// <param name="parentIdSelector">父节点</param>
        /// <param name="rootId">根节点</param>
        /// <returns>列表生成树形节点</returns>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> idSelector,
            Func<T, K> parentIdSelector,
            K rootId = default) where T : class where K : struct
        {
            foreach (var c in collection.Where(u =>
            {
                var selector = parentIdSelector(u);
                return rootId.Equals(selector);
            }))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(idSelector, parentIdSelector, idSelector(c))
                };
            }
        }    
    }

}
