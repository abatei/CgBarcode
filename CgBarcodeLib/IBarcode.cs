using System;
using System.Collections.Generic;
using System.Text;

namespace CgBarcodeLib
{
    interface IBarcode
    {
        string RawData { get; }
        List<string> Errors { get; }
        string GetEncode();
    }
}
