using EES.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// 分页查询扩展
    /// </summary>
    public static class PagingQueryExtensions
    {
        /// <summary>
        /// 根据条件执行表达式分页的数据集合(不返回总数量)
        /// </summary>
        /// <param name="query"></param> 
        /// <param name="pageIndex">页码,最小为1</param>
        /// <param name="pageSize">每页大小(10-100)</param> 
        /// <returns></returns>
        public static IEnumerable<TEntity> GetPagingList<TEntity>(this IOrderedQueryable<TEntity> query, int pageIndex = 1, int pageSize = 10) where TEntity : class, new()
        {
            VerifyPagingParams(ref pageIndex, ref pageSize);
            return ToPageFun(query, pageIndex, pageSize).AsEnumerable();
        }

        /// <summary>
        /// 根据条件执行表达式分页的数据集合(不返回总数量)
        /// </summary>
        /// <param name="query"></param> 
        /// <param name="pageIndex">页码,最小为1</param>
        /// <param name="pageSize">每页大小，最小为10</param> 
        /// <returns></returns>
        public static async Task<IEnumerable<TEntity>> GetPagingListAsync<TEntity>(this IOrderedQueryable<TEntity> query, int pageIndex = 1, int pageSize = 10) where TEntity : class, new()
        {
            VerifyPagingParams(ref pageIndex, ref pageSize);
            return await ToPageFun(query, pageIndex, pageSize).ToListAsync();
        }


        /// <summary>
        /// 根据条件执行表达式分页
        /// </summary>
        /// <param name="query"></param> 
        /// <param name="pageIndex">页码,最小为1</param>
        /// <param name="pageSize">每页大小，最小为10</param> 
        /// <param name="isGetTotalCount">是否返回总数量</param>
        /// <returns></returns>
        public static async Task<PaginationModel<TEntity>> GetPagingAsync<TEntity>(this IOrderedQueryable<TEntity> query, int pageIndex = 1, int pageSize = 10, bool isGetTotalCount = true) where TEntity : class
        {
            VerifyPagingParams(ref pageIndex, ref pageSize);

            var totalCount = GetTotalCount(query, isGetTotalCount);

            return new PaginationModel<TEntity>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = await ToPageFun(query, pageIndex, pageSize).ToListAsync()
            };
        }

        /// <summary>
        /// 根据条件执行表达式分页
        /// </summary>
        /// <param name="query"></param> 
        /// <param name="pageIndex">页码,最小为1</param>
        /// <param name="pageSize">每页大小，最小为10</param> 
        /// <param name="isGetTotalCount">是否返回总数量</param>
        /// <returns></returns>
        public static PaginationModel<TEntity> GetPaging<TEntity>(this IOrderedQueryable<TEntity> query, int pageIndex = 1, int pageSize = 10, bool isGetTotalCount = true) where TEntity : class, new()
        {
            VerifyPagingParams(ref pageIndex, ref pageSize);

            var totalCount = GetTotalCount(query, isGetTotalCount);
           
            return new PaginationModel<TEntity>
            {
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Data = ToPageFun(query, pageIndex, pageSize).ToList()
            };
        }


        /// <summary>
        /// 以元组的形式返回分页查询信息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex">页码,最小为1</param>
        /// <param name="pageSize">每页大小，最小为10</param> 
        /// <param name="isGetTotalCount">是否返回总数量</param>
        /// <returns></returns>
        public static async Task<(int totalCount, IEnumerable<TEntity>)> GetPagingInTupleAsync<TEntity>(this IOrderedQueryable<TEntity> query, int pageIndex = 1, int pageSize = 10, bool isGetTotalCount = true) where TEntity : class
        {
            VerifyPagingParams(ref pageIndex, ref pageSize);

            var totalCount = GetTotalCount(query, isGetTotalCount);

            var list = await ToPageFun(query, pageIndex, pageSize).ToListAsync();

            return (totalCount, list);
        }

        /// <summary>
        /// 以元组的形式返回分页查询信息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex">页码,最小为1</param>
        /// <param name="pageSize">每页大小，最小为10</param> 
        /// <param name="isGetTotalCount">是否返回总数量</param>
        /// <returns></returns>
        public static (int totalCount, IEnumerable<TEntity>) GetPagingInTuple<TEntity>(this IOrderedQueryable<TEntity> query, int pageIndex = 1, int pageSize = 10, bool isGetTotalCount = true) where TEntity : class
        {
            VerifyPagingParams(ref pageIndex, ref pageSize);

            var totalCount = GetTotalCount(query, isGetTotalCount);

            var list =  ToPageFun(query, pageIndex, pageSize).ToList();

            return (totalCount, list);
        }


        /// <summary>
        /// 分页私有方法
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex">页码,最小为1</param>
        /// <param name="pageSize">每页大小,最小为10</param>
        /// <returns></returns>
        private static IQueryable<TEntity> ToPageFun<TEntity>(IOrderedQueryable<TEntity> query, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex!=0 && pageSize!=0)
            {
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return query;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private static void VerifyPagingParams(ref int pageIndex, ref int pageSize)
        {
            //if (pageIndex < 1)
            //{
            //    pageIndex = 1;
            //}

            //if (pageSize < 10)
            //{
            //    pageSize = 10;
            //}
        }

        /// <summary>
        /// 获取数据总数量
        /// </summary>
        /// <param name="query">查询表达式</param>
        /// <param name="isGetTotalCount">是否计算总数量</param>
        /// <returns></returns>
        private static int GetTotalCount<TEntity>(IOrderedQueryable<TEntity> query, bool isGetTotalCount = true)
        {
            return isGetTotalCount ? query.Count() : -1;
        }
    }
}
