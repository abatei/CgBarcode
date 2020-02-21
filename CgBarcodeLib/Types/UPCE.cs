using System;

namespace CgBarcodeLib.Types
{
    public class UPCE : EAN_UPCBase, IBarcode
    {
        private string[] UPCE_Code_0 = { "bbbaaa", "bbabaa", "bbaaba", "bbaaab", "babbaa", "baabba", "baaabb", "bababa", "babaab", "baabab" };
        private string[] UPCE_Code_1 = { "aaabbb", "aababb", "aabbab", "aabbba", "abaabb", "abbaab", "abbbaa", "ababab", "ababba", "abbaba" };

        public UPCE(string input)
        {
            if (!CheckNumericOnly(input))
            {
                throw new ArgumentException("参数 input  错误，UPC编码中不能存在非数字字符！");
            }
            if (input.Length >= 7 && input.Length <= 8)
            {
                rawData = input;
                //计算校验码是否正确
                UPCA_Code = UPCE_to_UPCA(rawData);
                char code = UPCA_Code[11];
                if (rawData.Length == 8 && rawData[7] != code)
                {
                    error.Add("校验码不正确，已自动修正！");
                }
                rawData = rawData.Substring(0, 7) + code.ToString();
            }
            else if (input.Length >= 11 && input.Length <= 12)
            {
                rawData = input;
                AmendLength(12);
                UPCA_Code = RawData;
                rawData = UPCA.UPCA_to_UPCE(rawData);
            }
            error.Add("UPC-A编码：" + UPCA_Code);
        }

        public string GetEncode()
        {   //第一位编码决定了使用哪一组编码
            string[] patterns;
            if (rawData[0] == '0')
            {
                patterns = UPCE_Code_0;
            }
            else
            {
                patterns = UPCE_Code_1;
            }
            //校验码决定了使用哪一种模式
            string patternCode = patterns[Int32.Parse(rawData[7].ToString())];

            //加入起始符
            string result = "101";
            //加入数据符
            for (int pos = 1; pos <= 6; pos++)
            {
                if (patternCode[pos - 1] == 'a')
                {
                    result += SetA[Int32.Parse(rawData[pos].ToString())];
                }
                if (patternCode[pos - 1] == 'b')
                {
                    result += SetB[Int32.Parse(rawData[pos].ToString())];
                }
            }
            //加入终止符
            result += "010101";

            return result;
        }

        /// <summary>
        /// 此UPC-E码所对应的UPC-A码
        /// </summary>
        public string UPCA_Code { get; private set; }

        #region UPCE 转 UPCA
        /// <summary>
        /// 检查传入的UPCE码是否可以转换为UPCA码
        /// </summary>
        /// <param name="data">UPCE码，长度必须为7或8位的数字组成，第一位系统字符必须为0或1</param>
        public static void CheckUPCEFormat(string data)
        {
            if (!CheckNumericOnly(data))
            {
                throw new ArgumentException("参数 data 错误，UPCE 编码中不能存在非数字字符！");
            }
            if (data.Length < 7)
            {
                throw new ArgumentException("参数 data 错误，UPCE 编码最少要包括 7 位数字！");
            }
            if (data[0] != '0' && data[0] != '1')
            {
                throw new ArgumentException("参数 data  错误，UPCE 码第一个系统字符必须为'0'或'1'！");
            }
        }

        /// <summary>
        /// 将UPCE码转换为UPCA码
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回没有</returns>
        public static string UPCE_to_UPCA(string data)
        {
            CheckUPCEFormat(data);

            int pattern = int.Parse(data[6].ToString());
            string s1, s2, result;
            if (pattern >= 0 && pattern <= 2)
            {
                s1 = data.Substring(0, 3);
                s2 = data.Substring(3, 3);
                result = s1 + int.Parse(pattern.ToString()) + "0000" + s2;
            }
            else if (pattern == 3)
            {
                s1 = data.Substring(0, 4);
                s2 = data.Substring(4, 2);
                result = s1 + "00000" + s2;
            }
            else if (pattern == 4)
            {
                s1 = data.Substring(0, 5);
                s2 = data.Substring(5, 1);
                result = s1 + "00000" + s2;
            }
            else
            {
                s1 = data.Substring(0, 6);
                s2 = data.Substring(6, 1);
                result = s1 + "0000" + s2;
            }

            char code = CheckDigit(result);
            return result + code.ToString();
        }
        #endregion 
    }
}
