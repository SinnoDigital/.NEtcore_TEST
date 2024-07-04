using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EES.Infrastructure.Jwt
{
    /// <summary>
    /// Claims模型
    /// </summary>
    public class ClaimsModel
    {

        static ClaimsModel()
        {
            Id = "id";
            Platform = "platform";
        }


        /// <summary>
        /// Id
        /// </summary>
        public static string Id { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public static string Platform { get; set; }
    }
}
