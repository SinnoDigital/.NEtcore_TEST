using EES.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EES.Infrastructure.Commons
{
    /// <summary>
    /// 查询操作返回值对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResponse<T>
    {
        protected QueryResponse() { }

        /// <summary>
        /// 请求响应码，0 为正常
        /// </summary>
        public virtual int Code { get; protected set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public virtual bool Status => Code == 0;

        /// <summary>
        /// 响应信息
        /// </summary>
        public virtual string? Message { get; protected set; }

        /// <summary>
        /// 业务数据
        /// </summary>
        public T? Data { get; protected set; }


        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static QueryResponse<T> Success(T data, string message = "ok") => new()
        {
            Code = 0,
            Message = message,
            Data = data
        };

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static QueryResponse<T> Fail(int code, string message, T? data = default) => new()
        {
            Code = code,
            Message = message,
            Data = data
        };


        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static QueryResponse<T> Fail(BusinessError error, T? data = default) => new()
        {
            Message = error.ToString(),
            Code = (int)error,
            Data = data
        };


        /// <summary>
        /// 实例一个响应结果
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="info"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static QueryResponse<T> Instance<TEnum>(TEnum info, T? data = default) where TEnum : Enum
        {
            var message = info.ToString();
            return new QueryResponse<T>
            {
                Code = (int)Enum.Parse(typeof(TEnum), message),
                Message = message,
                Data = data
            };
        }
    }
}
