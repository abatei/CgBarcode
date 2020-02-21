using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CgBarcodeLib.Types
{
    public abstract class BarcodeBase
    {
        protected string rawData = "";
        protected List<string> error = new List<string>();

        public string RawData => rawData;
        public List<string> Errors => error;

        /// <summary>
        /// 修正原始编码长度
        /// </summary>
        /// <param name="len">表示编码标准长度</param>
        protected void AmendLength(int len)
        {
            if (rawData.Length > len)
            {
                error.Add("输入编码长度超过" + len.ToString() + "，被自动截断！");
                rawData = rawData.Substring(0, len);
            }
            //计算校验码是否正确
            char code = CheckDigit(rawData.Substring(0, len - 1));
            if (rawData.Length == len)
            {
                if (rawData[len - 1] != code)
                {
                    error.Add("校验码不正确，已自动修正！");
                }
            }
            rawData = rawData.Substring(0, len - 1) + code.ToString();
        }

        /// <summary>
        /// 根据给出的编码计算校验字符并返回，使用时必须确保编码全为数字，并且长度为编码长度减1。
        /// 如：EAN-13编码必须提供由12位数字组成的字符串
        /// </summary>
        /// <param name="data">由数字字符组成的编码</param>
        /// <returns>返回计算出的校验码字符串表示</returns>
        public static char CheckDigit(string data)
        {
            int even = 0;
            int odd = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (i % 2 == 0)
                    odd += Int32.Parse(data.Substring(data.Length - 1 - i, 1)) * 3;
                else
                    even += Int32.Parse(data.Substring(data.Length - 1 - i, 1));
            }

            int total = even + odd;
            int cs = total % 10;
            cs = 10 - cs;
            if (cs == 10)
            {
                cs = 0;
            }
            return (char)(cs + 48);
        }

        /// <summary>
        /// 检查字符串是否是全数字
        /// </summary>
        /// <param name="data">输入的字符串</param>
        /// <returns>如果是全数字则返回 true，否则返回 false</returns>
        internal static bool CheckNumericOnly(string data)
        {
            return Regex.IsMatch(data, @"^\d+$", RegexOptions.Compiled);
        }
    }
}
