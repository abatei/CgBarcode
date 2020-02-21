using System;

namespace CgBarcodeLib.Types
{
    class UPCA : EAN_UPCBase, IBarcode
    {
        public UPCA(string input)
        {
            if (!CheckNumericOnly(input))
            {
                throw new ArgumentException("参数 input  错误，UPCA编码中不能存在非数字字符！");
            }
            if (input.Length < 11)
            {
                throw new ArgumentException("参数 input  错误，UPCA编码最少要包括 11 位数字！");
            }
            rawData = input;
            AmendLength(12);
        }

        public string GetEncode()
        {
            string result = "101"; //加入起始符
                                   //加入左侧数据符
            int pos = 0;
            while (pos <= 5)
            {
                result += SetA[Int32.Parse(rawData[pos].ToString())];
                pos++;
            }
            //加入中间分隔符
            result += "01010";
            //加入右侧数据符
            while (pos <= 11)
            {
                result += SetC[Int32.Parse(rawData[pos].ToString())];
                pos++;
            }
            //加入终止符
            result += "101";

            return result;
        }

        #region UPCA 转 UPCE
        /// <summary>
        /// 检查传入的UPCA码是否可以转换为UPCE码
        /// </summary>
        /// <param name="data">UPCA码，长度必须为11或12位的数字组成，第一位系统字符必须为0或1</param>
        /// <returns>可以转换则返回true，不能转换则返回false</returns>
        public static bool CheckFormatWithUPCA_toUPCE(string data)
        {
            if (!CheckNumericOnly(data))
            {
                throw new ArgumentException("参数 data 错误，UPCA 编码中不能存在非数字字符！");
            }
            if (data.Length < 11)
            {
                throw new ArgumentException("参数 data  错误，UPCA 编码最少要包括 11 位数字！");
            }
            if (data[0] != '0' && data[0] != '1')
            {
                throw new ArgumentException("参数 data  错误，只有第一个系统字符为'0'或'1'的 UPCA 码才能转换为 UPCE 码！");
            }

            string fCode = data.Substring(3, 3);
            string bCode = data.Substring(6, 2);
            if (fCode == "000" || fCode == "100" || fCode == "200")
            {
                if (bCode == "00")
                {
                    return true;
                }
            }

            fCode = data.Substring(4, 2);
            bCode = data.Substring(6, 3);
            if (fCode == "00")
            {
                if (bCode == "000")
                {
                    return true;
                }
            }

            fCode = data.Substring(5, 1);
            bCode = data.Substring(6, 4);
            if (fCode == "0")
            {
                if (bCode == "0000")
                {
                    return true;
                }
            }

            bCode = data.Substring(6, 5);
            if (bCode == "00005" || bCode == "00006" || bCode == "00007" ||
                bCode == "00008" || bCode == "00009")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 将UPCA码转换为UPCE码
        /// </summary>
        /// <param name="data">UPCA码，长度必须为11或12位的数字组成，第一位系统字符必须为0或1</param>
        /// <returns>成功则返回转换后的8位UPCE码，失败则返回null</returns>
        public static string UPCA_to_UPCE(string data)
        {
            if (!CheckFormatWithUPCA_toUPCE(data))
            {
                return null;
            }

            char code = CheckDigit(data.Substring(0, 11));//校验码
            string fCode = data.Substring(3, 3);
            string bCode = data.Substring(6, 2);
            if (fCode == "000" || fCode == "100" || fCode == "200")
            {
                if (bCode == "00")
                {
                    string s1 = data.Substring(0, 3);
                    string s2 = data.Substring(8, 3);
                    string s3 = data.Substring(3, 1);

                    return s1 + s2 + s3 + code.ToString();
                }
            }

            fCode = data.Substring(4, 2);
            bCode = data.Substring(6, 3);
            if (fCode == "00")
            {
                if (bCode == "000")
                {
                    string s1 = data.Substring(0, 4);
                    string s2 = data.Substring(9, 2);
                    return s1 + s2 + "3" + code;
                }
            }

            fCode = data.Substring(5, 1);
            bCode = data.Substring(6, 4);
            if (fCode == "0")
            {
                if (bCode == "0000")
                {
                    string s1 = data.Substring(0, 5);
                    string s2 = data.Substring(10, 1);
                    return s1 + s2 + "4" + code;
                }
            }

            bCode = data.Substring(6, 5);
            if (bCode == "00005" || bCode == "00006" || bCode == "00007" ||
                bCode == "00008" || bCode == "00009")
            {
                string s1 = data.Substring(0, 6);
                string s2 = data.Substring(10, 1);
                return s1 + s2 + code;
            }
            return null;
        }
        #endregion
    }
}
