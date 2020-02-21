using System;
using System.Collections.Generic;
using System.Text;

namespace CgBarcodeLib.Types
{
    public enum CharSet : int { Auto = 0, A = 1, B = 2, C = 3 };

    /// <summary>
    /// 此类专门用于存放 Code 128 条码所使用字符及相应打印编码，并方便进行译码操作
    /// </summary>
    class Symbols
    {
        class Code1
        {
            public Code1(int id, char codeA, char codeB, string codeC, string encoding)
            {
                ID = id;
                CodeA = codeA;
                CodeB = codeB;
                CodeC = codeC;
                Encoding = encoding;
            }
            /// <summary>
            /// 符号字符值
            /// </summary>
            public int ID { get; set; }
            public char CodeA { get; set; }
            public char CodeB { get; set; }
            public string CodeC { get; set; }
            public string Encoding { get; set; }
        }

        class Code2
        {
            public Code2(int id, char codeA, char codeB, char codeC, string encoding)
            {
                ID = id;
                CodeA = codeA;
                CodeB = codeB;
                CodeC = codeC;
                Encoding = encoding;
            }
            /// <summary>
            /// 符号字符值
            /// </summary>
            public int ID { get; set; }
            public char CodeA { get; set; }
            public char CodeB { get; set; }
            public char CodeC { get; set; }
            public string Encoding { get; set; }
        }

        public static char FNC1 = Convert.ToChar(200);
        public static char FNC2 = Convert.ToChar(201);
        public static char FNC3 = Convert.ToChar(202);
        public static char FNC4 = Convert.ToChar(203);
        public static char SHIFT = Convert.ToChar(204);
        public static char CODE_A = Convert.ToChar(205);
        public static char CODE_B = Convert.ToChar(206);
        public static char CODE_C = Convert.ToChar(207);
        public static char START_A = Convert.ToChar(208);
        public static char START_B = Convert.ToChar(209);
        public static char START_C = Convert.ToChar(210);
        public static char STOP = Convert.ToChar(211);

        //编码表（ASCII码字符表）
        static Code1[] codeSet1 = new Code1[]
        {
            new Code1(0, ' ', ' ', "00", "11011001100"),
            new Code1(1, '!', '!', "01", "11001101100"),
            new Code1(2, '\"', '\"', "02", "11001100110"),
            new Code1(3, '#', '#', "03", "10010011000"),
            new Code1(4, '$', '$', "04", "10010001100"),
            new Code1(5, '%', '%', "05", "10001001100"),
            new Code1(6, '&', '&', "06", "10011001000"),
            new Code1(7, '\'', '\'', "07", "10011000100"),
            new Code1(8, '(', '(', "08", "10001100100"),
            new Code1(9, ')', ')', "09", "11001001000"),
            new Code1(10, '*', '*', "10", "11001000100" ),
            new Code1(11, '+', '+', "11", "11000100100" ),
            new Code1(12, ',', ',', "12", "10110011100" ),
            new Code1(13, '-', '-', "13", "10011011100" ),
            new Code1(14, '.', '.', "14", "10011001110" ),
            new Code1(15, '/', '/', "15", "10111001100" ),
            new Code1(16, '0', '0', "16", "10011101100" ),
            new Code1(17, '1', '1', "17", "10011100110" ),
            new Code1(18, '2', '2', "18", "11001110010" ),
            new Code1(19, '3', '3', "19", "11001011100" ),
            new Code1(20, '4', '4', "20", "11001001110" ),
            new Code1(21, '5', '5', "21", "11011100100" ),
            new Code1(22, '6', '6', "22", "11001110100" ),
            new Code1(23, '7', '7', "23", "11101101110" ),
            new Code1(24, '8', '8', "24", "11101001100" ),
            new Code1(25, '9', '9', "25", "11100101100" ),
            new Code1(26, ':', ':', "26", "11100100110" ),
            new Code1(27, ';', ';', "27", "11101100100" ),
            new Code1(28, '<', '<', "28", "11100110100" ),
            new Code1(29, '=', '=', "29", "11100110010" ),
            new Code1(30, '>', '>', "30", "11011011000" ),
            new Code1(31, '?', '?', "31", "11011000110" ),
            new Code1(32, '@', '@', "32", "11000110110" ),
            new Code1(33, 'A', 'A', "33", "10100011000" ),
            new Code1(34, 'B', 'B', "34", "10001011000" ),
            new Code1(35, 'C', 'C', "35", "10001000110" ),
            new Code1(36, 'D', 'D', "36", "10110001000" ),
            new Code1(37, 'E', 'E', "37", "10001101000" ),
            new Code1(38, 'F', 'F', "38", "10001100010" ),
            new Code1(39, 'G', 'G', "39", "11010001000" ),
            new Code1(40, 'H', 'H', "40", "11000101000" ),
            new Code1(41, 'I', 'I', "41", "11000100010" ),
            new Code1(42, 'J', 'J', "42", "10110111000" ),
            new Code1(43, 'K', 'K', "43", "10110001110" ),
            new Code1(44, 'L', 'L', "44", "10001101110" ),
            new Code1(45, 'M', 'M', "45", "10111011000" ),
            new Code1(46, 'N', 'N', "46", "10111000110" ),
            new Code1(47, 'O', 'O', "47", "10001110110" ),
            new Code1(48, 'P', 'P', "48", "11101110110" ),
            new Code1(49, 'Q', 'Q', "49", "11010001110" ),
            new Code1(50, 'R', 'R', "50", "11000101110" ),
            new Code1(51, 'S', 'S', "51", "11011101000" ),
            new Code1(52, 'T', 'T', "52", "11011100010" ),
            new Code1(53, 'U', 'U', "53", "11011101110" ),
            new Code1(54, 'V', 'V', "54", "11101011000" ),
            new Code1(55, 'W', 'W', "55", "11101000110" ),
            new Code1(56, 'X', 'X', "56", "11100010110" ),
            new Code1(57, 'Y', 'Y', "57", "11101101000" ),
            new Code1(58, 'Z', 'Z', "58", "11101100010" ),
            new Code1(59, '[', '[', "59", "11100011010" ),
            new Code1(60,'\\','\\', "60", "11101111010" ),
            new Code1(61, ']', ']', "61", "11001000010" ),
            new Code1(62, '^', '^', "62", "11110001010" ),
            new Code1(63, '_', '_', "63", "10100110000" ),
            new Code1(64, '\0', '`', "64", "10100001100"),
            new Code1(65, Convert.ToChar(1), 'a', "65", "10010110000"),
            new Code1(66, Convert.ToChar(2), 'b', "66", "10010000110"),
            new Code1(67, Convert.ToChar(3), 'c', "67", "10000101100"),
            new Code1(68, Convert.ToChar(4), 'd', "68", "10000100110"),
            new Code1(69, Convert.ToChar(5), 'e', "69", "10110010000"),
            new Code1(70, Convert.ToChar(6), 'f', "70", "10110000100"),
            new Code1(71, Convert.ToChar(7), 'g', "71", "10011010000"),
            new Code1(72, Convert.ToChar(8), 'h', "72", "10011000010"),
            new Code1(73, Convert.ToChar(9), 'i', "73", "10000110100"),
            new Code1(74, Convert.ToChar(10), 'j', "74", "10000110010"),
            new Code1(75, Convert.ToChar(11), 'k', "75", "11000010010"),
            new Code1(76, Convert.ToChar(12), 'l', "76", "11001010000"),
            new Code1(77, Convert.ToChar(13), 'm', "77", "11110111010"),
            new Code1(78, Convert.ToChar(14), 'n', "78", "11000010100"),
            new Code1(79, Convert.ToChar(15), 'o', "79", "10001111010"),
            new Code1(80, Convert.ToChar(16), 'p', "80", "10100111100"),
            new Code1(81, Convert.ToChar(17),'q', "81", "10010111100" ),
            new Code1(82, Convert.ToChar(18),'r', "82", "10010011110"),
            new Code1(83, Convert.ToChar(19),'s', "83", "10111100100"),
            new Code1(84, Convert.ToChar(20),'t', "84", "10011110100"),
            new Code1(85, Convert.ToChar(21),'u', "85", "10011110010"),
            new Code1(86, Convert.ToChar(22),'v', "86", "11110100100"),
            new Code1(87, Convert.ToChar(23),'w', "87", "11110010100"),
            new Code1(88, Convert.ToChar(24),'x', "88", "11110010010"),
            new Code1(89, Convert.ToChar(25),'y', "89", "11011011110"),
            new Code1(90, Convert.ToChar(26),'z', "90", "11011110110"),
            new Code1(91, Convert.ToChar(27),'{', "91", "11110110110"),
            new Code1(92, Convert.ToChar(28),'|', "92", "10101111000"),
            new Code1(93, Convert.ToChar(29),'}', "93", "10100011110"),
            new Code1(94, Convert.ToChar(30),'~', "94", "10001011110"),
            new Code1(95, Convert.ToChar(31), Convert.ToChar(127), "95", "10111101000"),
            new Code1(96, FNC3, FNC3, "96", "10111100010"),
            new Code1(97, FNC2, FNC2, "97", "11110101000"),
            new Code1(98, SHIFT, SHIFT, "98", "11110100010"),
            new Code1(99, CODE_C, CODE_C, "99", "10111011110"),
         };
        //编码表（非ASCII码字符表）
        static Code2[] codeSet2 = new Code2[]
        {
            new Code2(100, CODE_B, FNC4, CODE_B, "10111101110"),
            new Code2(101, FNC4, CODE_A, CODE_A, "11101011110" ),
            new Code2(102, FNC1, FNC1, FNC1, "11110101110"),
            new Code2(103, START_A, START_A, START_A, "11010000100"),
            new Code2(104, START_B, START_B, START_B, "11010010000"),
            new Code2(105, START_C, START_C, START_C, "11010011100"),
            new Code2(106, STOP, STOP, STOP, "1100011101011")
        };

        /// <summary>
        /// 索引器，通过一个字符查找相应的绘图编码
        /// </summary>
        /// <param name="setID">所使用的字符集，A、B、C三个值之一</param>
        /// <param name="rawChar">原始字符</param>
        /// <returns>返回符号字符值如果未找到，则返回 -1</returns>
        public int this[CharSet setID, char rawChar]
        {
            get
            {
                if (setID == CharSet.A)
                {
                    foreach (Code1 code in codeSet1)
                    {
                        if (code.CodeA == rawChar)
                        {
                            return code.ID;
                        }
                    }
                    foreach (Code2 code in codeSet2)
                    {
                        if (code.CodeA == rawChar)
                        {
                            return code.ID;
                        }
                    }
                }
                else if (setID == CharSet.B)
                {
                    foreach (Code1 code in codeSet1)
                    {
                        if (code.CodeB == rawChar)
                        {
                            return code.ID;
                        }
                    }
                    foreach (Code2 code in codeSet2)
                    {
                        if (code.CodeB == rawChar)
                        {
                            return code.ID;
                        }
                    }
                }
                else
                {
                    foreach (Code2 code in codeSet2)
                    {
                        if (code.CodeC == rawChar)
                        {
                            return code.ID;
                        }
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// 索引器，通过一个字符串查找相应的绘图编码，仅用于字符集C
        /// </summary>
        /// <param name="setID">所使用的字符集，只能使用C值</param>
        /// <param name="rawStr">原始字符串</param>
        /// <returns>返回符号字符值，如果未找到，则返回 -1</returns>
        public int this[CharSet setID, string rawStr]
        {
            get
            {
                if (setID != CharSet.C)
                {
                    throw new ArgumentException("参数 setID  错误，只能使用'C'值！");
                }

                foreach (Code1 code in codeSet1)
                {
                    if (code.CodeC == rawStr)
                    {
                        return code.ID;
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// 通过符号字符值查找相应的绘图编码
        /// </summary>
        /// <param name="id">符号字符值</param>
        /// <returns>返回译码后的字符串，如果未找到，则返回 null</returns>
        public string this[int id]
        {
            get
            {
                foreach (Code1 code in codeSet1)
                {
                    if (code.ID == id)
                    {
                        return code.Encoding;
                    }
                }
                foreach (Code2 code in codeSet2)
                {
                    if (code.ID == id)
                    {
                        return code.Encoding;
                    }
                }
                return null;
            }
        }

        StringBuilder describe;
        /// <summary>
        /// 本程序的Code 128图形编码是以整数数组的形式存放的，本方法用于
        /// 将整数数组转换为编码的字符串描述，以供学习
        /// </summary>
        /// <param name="result">整数数组的形式的图形编码</param>
        /// <returns></returns>
        public string NumSetToDescribe(List<int> result)
        {
            describe = new StringBuilder();
            CharSet mode = (CharSet)(result[0] - 102);

            AddOneCode(GetDescribe(mode, result[0]));
            for (int i = 1; i < result.Count;)
            {
                int code = result[i];
                AddOneCode(GetDescribe(mode, code));
                if (code == 99 && mode != CharSet.C)
                {
                    mode = CharSet.C;
                }
                else if (code == 100 && mode != CharSet.B)
                {
                    mode = CharSet.B;
                }
                else if (code == 101 && mode != CharSet.A)
                {
                    mode = CharSet.A;
                }
                else if (code == 98) //SHIFT转换字符
                {
                    if (mode == CharSet.A)
                    {
                        AddOneCode(GetDescribe(CharSet.B, result[++i]));
                    }
                    else if (mode == CharSet.B)
                    {
                        AddOneCode(GetDescribe(CharSet.A, result[++i]));
                    }
                }
                i++;
            }
            return describe.ToString();
        }

        private void AddOneCode(string code)
        {
            describe.Append('【');
            describe.Append(code);
            describe.Append('】');
        }

        //控制字符描述字符串
        static string[] CtrlString =
        {
            "NUL","SOH","STX","ETX","EOT","ENQ","ACK","BEL","BS",
            "HT","LF","VT","FF","CR","SO","SI","DLE","DC1","DC2",
            "DC3","DC4","NAK","SYN","ETB","CAN","EM","SUB","ESC",
            "FS","GS","RS"
        };

        static string[] AString = { "US", "FNC3", "FNC2", "SHIFT", "CODE C", "CODE B", "FNC4", "FNC1", "Start A", "Start B", "Start C", "Stop" };
        static string[] BString = { "DEL", "FNC3", "FNC2", "SHIFT", "CODE C", "FNC4", "CODE A", "FNC1", "Start A", "Start B", "Start C", "Stop" };
        static string[] CString = { "95", "96", "97", "98", "99", "CODE B", "CODE A", "FNC1", "Start A", "Start B", "Start C", "Stop" };

        /// <summary>
        /// 通过符号字符值查找相应的字符描述
        /// </summary>
        /// <param name="setID">所使用的字符集</param>
        /// <param name="id">符号字符值<</param>
        /// <returns></returns>
        private string GetDescribe(CharSet setID, int id)
        {
            if (id >= 0 && id <= 94) //codeSet1中的编码
            {
                if (setID == CharSet.C) //字符集C则直接返回id所对应的数字
                {
                    return id.ToString().PadLeft(2, '0');
                }
                else if (setID == CharSet.A && id >= 64) //控制字符
                {
                    return CtrlString[id - 64];
                }
                else
                {
                    if (id == 0) return "space"; //空格
                    foreach (Code1 code in codeSet1)
                    {
                        if (code.ID == id)
                        {
                            if (setID == CharSet.A)
                            {
                                return code.CodeA.ToString();
                            }
                            else if (setID == CharSet.B)
                            {
                                return code.CodeB.ToString();
                            }
                        }
                    }
                }
            }
            else
            {
                if (setID == CharSet.A)
                {
                    return AString[id - 95];
                }
                else if (setID == CharSet.B)
                {
                    return BString[id - 95];
                }
                else
                {
                    return CString[id - 95];
                }
            }
            return null;
        }
    }
}
