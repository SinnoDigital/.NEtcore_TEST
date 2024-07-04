using EES.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace EES.Infrastructure.Service
{
    /// <summary>
    /// 当前Http请求的用户信息
    /// <see cref="https://www.cnblogs.com/InCerry/p/Why-The-Design-HttpContextAccessor.html"/>
    /// </summary>
    public class HttpAccessor
    {
        private readonly static AsyncLocal<AccessorHolder> _asyncLocal = new();

        private readonly static AsyncLocal<TraceIdentifierHolder> _traceAsyncLocal = new();

        /// <summary>
        /// 当前请求用户的信息
        /// </summary>
        public static Accessor Accessor
        {
            get
            {
                return _asyncLocal.Value?.Accessor;
            }
            private set
            {
                var holder = _asyncLocal.Value;

                if (holder != null)
                {
                    holder.Accessor = null;
                }

                if (value != null)
                {
                    _asyncLocal.Value = new AccessorHolder { Accessor = value };
                }
            }
        }

        /// <summary>
        /// 当前请求的traceId
        /// </summary>
        public static string TraceIdentifier
        {
            get { return _traceAsyncLocal.Value?.TraceIdentifier; }

            private set
            {
                var holder = _traceAsyncLocal.Value;

                if (holder != null)
                {
                    holder.TraceIdentifier = string.Empty;
                }

                if (value != null)
                {
                    _traceAsyncLocal.Value = new TraceIdentifierHolder { TraceIdentifier = value };
                }
            }
        }

        public static void SetAccessor(Accessor accessor)
        {
            if (accessor != null)
            {
                Accessor = accessor;
            }
        }

        public static void SetTraceIdentifier(string traceIdentifier)
        {
            if (string.IsNullOrWhiteSpace(traceIdentifier))
            {
                traceIdentifier = string.Empty;
            }

            TraceIdentifier = traceIdentifier;
        }


        private class AccessorHolder
        {
            public Accessor Accessor;
        }

        private class TraceIdentifierHolder
        {
            public string TraceIdentifier { get; set; }
        }

    }

}
