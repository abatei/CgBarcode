using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgBarcodeLib;

namespace CgBarcodeLib.Types
{
    class Interleaved2of5 : BarcodeBase, IBarcode
    {
        private readonly string[] I25_Code = { "NNWWN", "WNNNW", "NWNNW", "WWNNN", "NNWNW", "WNWNN", "NWWNN", "NNNWW", "WNNWN", "NWNWN" };

        public Interleaved2of5(string input)
        {
            if (!CheckNumericOnly(input))
            {
                throw new ArgumentException("参数 input  错误，UPCA编码中不能存在非数字字符！");
            }
            //如果输入数据长度为偶数，则前面补0
            if (input.Length % 2 == 1)
            {
                rawData = "0" + input;
            }
            else
            {
                rawData = input;
            }
        }
        public string GetEncode()
        {
            string result = "NNNN"; //加入起始符
            StringBuilder patternMixed = new StringBuilder();
            for (int i = 0; i < rawData.Length; i += 2)
            {
                string patternBars = I25_Code[Int32.Parse(rawData[i].ToString())];
                string patternSpaces = I25_Code[Int32.Parse(rawData[i + 1].ToString())];
                //首先将编码两两交叉融合
                for (int j = 0; j < 5; j++)
                {
                    patternMixed.Append(patternBars[j]);
                    patternMixed.Append(patternSpaces[j]);
                }
            }
            result += patternMixed.ToString();
            //加入终止符
            result += "WNN";
            return result;
        }
    }
}
