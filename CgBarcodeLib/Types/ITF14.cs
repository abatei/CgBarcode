using System;

namespace CgBarcodeLib.Types
{
    class ITF14 : EAN_UPCBase, IBarcode
    {
        public ITF14(string input)
        {
            if (!CheckNumericOnly(input))
            {
                throw new ArgumentException("参数 input 错误，ITF-14编码中不能存在非数字字符！");
            }
            if (input.Length < 12)
            {
                throw new ArgumentException("参数 input 错误，ITF-14编码最少要包括 13 位数字！");
            }
            rawData = input;
            AmendITF();
        }
        public string GetEncode()
        {
            Interleaved2of5 i25 = new Interleaved2of5(rawData);
            return i25.GetEncode();
        }
    }
}
