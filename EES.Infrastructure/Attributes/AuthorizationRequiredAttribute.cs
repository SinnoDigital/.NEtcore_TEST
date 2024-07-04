using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Attributes
{
    /// <summary>
    /// 权限特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthorizationRequiredAttribute : Attribute
    {
        public string Permission { get; }

        public AuthorizationRequiredAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
