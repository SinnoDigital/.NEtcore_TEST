using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Md5Encryption
    {
        /// <summary>
        /// 普通md5，默认UFT8编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">编码类型，默认未null，则使用UTF-8编码</param>
        /// <returns></returns>
        public static string Md5(this string str, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;

            var result = MD5.HashData(encoding.GetBytes(str));
            var strResult = BitConverter.ToString(result);
            string result3 = strResult.Replace("-", "");
            return result3;
        }

        /// <summary>
        /// 十六进制MD5
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Md5By16Binary(this string str, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;

            var data = MD5.HashData(encoding.GetBytes(str));
            StringBuilder builder = new();
            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串 
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("X2"));
            }
            string result4 = builder.ToString();
            return result4;
        }

    }
}
