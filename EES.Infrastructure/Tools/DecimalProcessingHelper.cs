using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.Tools
{
    /// <summary>
    /// 小数处理
    /// </summary>
    public static class DecimalProcessingHelper
    {
        /// <summary>
        /// 处理小数位
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static string ProcessStringAndNumber(string inputString, int decimalPlaces)
        {
            // 处理数值
            if (decimal.TryParse(inputString, out decimal inputValue))
            {
                // 检查是否存在小数位
                bool hasDecimal = inputValue % 1 != 0;

                if (hasDecimal)
                {
                    // 将数值向上取整到指定的小数位数
                    decimal roundedValue = Math.Ceiling(inputValue * Convert.ToDecimal(Math.Pow(10, decimalPlaces))) / Convert.ToDecimal(Math.Pow(10, decimalPlaces));

                    // 将数值格式化为字符串，保留指定的小数位数并采用四舍五入
                    string formattedNumber = roundedValue.ToString("N" + decimalPlaces);
                    return formattedNumber;
                }
                else
                {
                    // 如果没有小数位，则直接返回原始数值的字符串形式
                    return inputValue.ToString();
                }
            }
            else
            {
                return "无法解析输入的数值字符串。";
            }
        }
    }
}
