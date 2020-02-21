using System;
using System.Collections.Generic;
using System.Text;

namespace CgBarcodeLib.Types
{
    public class EAN8 : EAN_UPCBase, IBarcode
    {
        public EAN8(string input)
        {
            if (!CheckNumericOnly(input))
            {
                throw new ArgumentException("参数 input 错误，EAN-8 编码中不能存在非数字字符！");
            }
            if (input.Length < 7)
            {
                throw new ArgumentException("参数 input 错误，EAN-8 编码最少要包括 7 位数字！");
            }
            rawData = input;
            AmendEAN(8);
        }

        public string GetEncode()
        {
            string result = "101"; //加入起始符
            //加入左侧数据符
            for (int i = 0; i < 4; i++)
            {
                result += SetA[Int32.Parse(rawData[i].ToString())];
            }
            //加入中间分隔符
            result += "01010";
            //加入右侧数据符
            for (int i = 4; i < 8; i++)
            {
                result += SetC[Int32.Parse(rawData[i].ToString())];
            }
            //加入终止符
            result += "101";

            return result;
        }
    }
}
