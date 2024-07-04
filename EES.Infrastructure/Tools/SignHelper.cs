using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EES.Infrastructure.Tools
{
    /// <summary>
    /// 签名制作帮助类
    /// </summary>
    public class SignHelper
    {

        /// <summary>
        ///  将请求参数和action的幂等性标记按照既定规则进行处理
        /// </summary>
        /// <param name="actionArguments"></param>
        /// <param name="attrFlag"></param>
        /// <returns></returns>
        public static string GetGetIdempotentKeyFlag(IDictionary<string, object?> actionArguments, string attrFlag)
        {
            var paramStr = GetParamsStr(actionArguments);

            if (!string.IsNullOrWhiteSpace(attrFlag))
            {
                paramStr += attrFlag;
            }

            return paramStr.ToLower().Md5().ToLower();
        }


        /// <summary>
        /// 将参数按照指定规则
        /// </summary>
        /// <param name="keyValues"></param>
        /// <param name="comparer">排序规则，默认为null，则使用StringComparer.Ordinal进行排序</param>
        /// <param name="connectKeyValueChar">连接key 和value的字符  示例 id=1</param>
        /// <param name="splitChar">连接两个kv 的字符  id=1&name=tom </param>
        /// <param name="format">对value进行json序列化时的格式规则</param>
        /// <returns></returns>
        public static string GetParamsStr(IDictionary<string, object?> keyValues, StringComparer comparer = null, char connectKeyValueChar = '=', char splitChar = '&', Formatting format = Formatting.None)
        {
            if (keyValues is null || keyValues.Count==0)
            {
                return string.Empty;
            }

            comparer ??= StringComparer.Ordinal;

            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, object?> sortedParams = new SortedDictionary<string, object?>(keyValues, comparer);

            var sb = new StringBuilder();

            foreach (var item in sortedParams)
            {
                var json = JsonConvert.SerializeObject(item.Value, format);

                sb.Append(item.Key).Append(connectKeyValueChar).Append(json).Append(splitChar);
            }

            return sb.ToString().TrimEnd(splitChar);
        }
    }
}
