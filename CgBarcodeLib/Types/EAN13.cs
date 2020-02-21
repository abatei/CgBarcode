using System;
using System.Collections.Generic;
using System.Text;

namespace CgBarcodeLib.Types
{
    public class EAN13 : EAN_UPCBase, IBarcode
    {
        public EAN13(string input)
        {
            if (!CheckNumericOnly(input))
            {
                throw new ArgumentException("参数 input  错误，EAN-13编码中不能存在非数字字符！");
            }
            if (input.Length < 12)
            {
                throw new ArgumentException("参数 input  错误，EAN-13编码最少要包括 12 位数字！");
            }
            rawData = input;
            AmendEAN(13);
        }

        public string GetEncode()
        {   //第一位编码决定了使用哪一种模式
            string patterncode = EAN_Pattern[Int32.Parse(rawData[0].ToString())];
            string result = "101"; //加入起始符
            //加入左侧数据符
            int pos = 1;
            while (pos <= 6)
            {
                if (patterncode[pos - 1] == 'a')
                {
                    result += SetA[Int32.Parse(rawData[pos].ToString())];
                }
                else if (patterncode[pos - 1] == 'b')
                {
                    result += SetB[Int32.Parse(rawData[pos].ToString())];
                }
                pos++;
            }
            //加入中间分隔符
            result += "01010";
            //加入右侧数据符
            while (pos <= 12)
            {
                result += SetC[Int32.Parse(rawData[pos].ToString())];
                pos++;
            }
            //加入终止符
            result += "101";

            return result;
        }
    }
}
