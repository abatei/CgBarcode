using System;
using System.Collections.Generic;
using System.Text;

namespace CgBarcodeLib.Types
{
    class Code128 : BarcodeBase, IBarcode
    {
        protected Symbols symbols = new Symbols();
        public CharSet SetID { get; private set; }

        public Code128() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="setID">字符集</param>
        /// <param name="input">原始字符串</param>
        public Code128(string input, CharSet setID = CharSet.Auto)
        {
            SetID = setID;

            if (input.Length < 1 || input.Length > 48)
            {
                throw new ArgumentException("原始字符串中的字符个数必须在 1~48 之间！");
            }

            if (setID == CharSet.C)
            {
                if (!CheckNumericOnly(input))
                {
                    throw new ArgumentException("Code 128C 编码必须全部为数字！");
                }
                if (input.Length % 2 != 0)
                {
                    throw new ArgumentException("Code 128C 编码只能表示长度为偶数的数字！");
                }
            }
            else if (setID == CharSet.A || setID == CharSet.B) //字符集A、B
            {
                //判断是否存在非法字符 
                foreach (char c in input)
                {
                    if (symbols[setID, c] == -1)
                    {
                        string[] arr = { "Auto", "A", "B", "C" };
                        string setStr = arr[(int)setID];
                        throw new ArgumentException("输入字符串中的某些字符并在于字符集" + setStr + "中！");
                    }
                }
            }
            else //Code 128Auto
            {

            }
            rawData = input;
            SetID = setID;
        }
        public virtual string GetEncode()
        {
            List<int> result = new List<int>();
            if (SetID != CharSet.Auto) //Code 128A、Code 128B、Code 128C
            {
                //加入起始符             
                result.Add(symbols[SetID, Convert.ToChar(207 + (int)SetID)]);
                if (SetID == CharSet.C) //Code 128C
                {
                    for (int i = 0; i < rawData.Length; i += 2)
                    {
                        result.Add(symbols[CharSet.C, rawData.Substring(i, 2)]);
                    }
                }
                else if (SetID == CharSet.A || SetID == CharSet.B) //Code 128A、Code 128B
                {
                    foreach (char c in rawData)
                    {
                        result.Add(symbols[SetID, c]);
                    }
                }
            }
            else// Code 128Auto
            {
                result = GetAutoEnccodeNumSet(rawData);
            }
            AddCheckSumAndStop(result);
            Errors.Add(symbols.NumSetToDescribe(result));
            return NumSetToEncode(result);
        }

        /// <summary>
        /// 计算由符号字符值组成的编码数组，此数组可很方便地转换为图形编码。
        /// </summary>
        /// <returns></returns>
        protected List<int> GetAutoEnccodeNumSet(string raw)
        {
            CharSet mode;
            List<int> result = new List<int>();
            //首先判断起始字符集
            int index = 0;
            int digitLen = GetDigitLen(raw);
            //起始字符为连续4个以上数字，或字符串为长度为2的数字组成
            if ((raw.Length == 2 && GetDigitLen(raw) == 2) || digitLen >= 4)
            {
                mode = CharSet.C;
                //加入起始字符 START_C
                result.Add(symbols[mode, Symbols.START_C]);
                //加入后面的数字
                for (int i = 0; i < digitLen - 1; i += 2)
                {
                    result.Add(symbols[mode, raw.Substring(i, 2)]);
                    index += 2;
                }
            }
            else
            {
                mode = GetModeAorB(raw);
                //加入起始字符
                result.Add(symbols[mode, Convert.ToChar(207 + (int)mode)]);
            }
            //加入后续字符            
            while (index < raw.Length)
            {   /* 如果在字符集 A 或 B 中同时产生 4 位或更多的数字型字符：
                 * b)如果数字型数据符个数为奇数，则在第一个数字之后插入字符集 C 字符以便转换为字符集 C
                 * a)如果数字型数据符个数为偶数，则在第一个数字之前插入字符集 C 字符以便转换为字符集 C */
                int len = GetDigitLen(raw.Substring(index));
                if (len >= 4)
                {
                    if (len % 2 == 1) //加入第一个数字字符，使用原来的字符集
                    {
                        result.Add(symbols[mode, raw[index]]);
                        index++;
                        len--;
                    }
                    //转换为字符集 C
                    result.Add(symbols[CharSet.A, Symbols.CODE_C]);
                    mode = CharSet.C;
                    //加入剩余数字字符，此时字符个数为偶数
                    for (int i = 0; i < len; i += 2)
                    {
                        result.Add(symbols[CharSet.C, raw.Substring(index + i, 2)]);
                    }
                    index += len;
                    continue;
                }

                if (mode == CharSet.C)
                {   //此时刚处理完连续 4 个以上数字，需转换为字符集A或B
                    mode = GetModeAorB(raw.Substring(index));
                    result.Add(symbols[CharSet.C, Convert.ToChar(204 + (int)mode)]);
                }
                else if (mode == CharSet.B)
                {   /* 当在字符集B中且ASCII控制字符在数据中出现时：
                     * a) 如果在该字符之后，小写字母键盘字符在另一个控制字符出现之前出现在数据中，
                     *    则在该控制字符之前插入转换符；
                     * b) 否则，在控制字符之前插入 CODE A 字符以转换为字符集 A。*/
                    if (IsControl(raw[index]))
                    {
                        if (GetFirstCharType(raw.Substring(index + 1)) == CharType.Lower)
                        {   //插入转换符
                            result.Add(symbols[mode, Symbols.SHIFT]);
                        }
                        else
                        {
                            mode = CharSet.A;
                            result.Add(symbols[CharSet.C, Symbols.CODE_A]);
                        }
                    }
                }
                else if (mode == CharSet.A)
                {
                    /* 当在字符集 A 中且小写字母键盘字符在数据中出现时：
                     * a) 如果在该字符之后，控制字符在另一个小写字母键盘字符出现之前出现在数据中，
                     *    则在该小写字母键盘字符之前插入转换符；
                     * b) 否则，在小写字母键盘字符之前插入 CODE B 字符以转换为字符集 B。*/
                    if (IsLowerKey(raw[index]))
                    {
                        if (GetFirstCharType(raw.Substring(index + 1)) == CharType.Control)
                        {   //插入转换符
                            result.Add(symbols[mode, Symbols.SHIFT]);
                        }
                        else
                        {
                            mode = CharSet.B;
                            result.Add(symbols[CharSet.C, Symbols.CODE_B]);
                        }
                    }
                }

                //最后是正常插入每一个字符
                result.Add(symbols[mode, raw[index]]);
                index++;
            }
            return result;
        }

        /// <summary>
        /// 计算并加入校验符，并在最后加入终止符
        /// </summary>
        /// <param name="result"></param>
        protected void AddCheckSumAndStop(List<int> result)
        {
            int sum = result[0];
            for (int i = 1; i < result.Count; i++)
            {
                sum += i * result[i];
            }
            result.Add(sum % 103);
            result.Add(symbols[SetID, Symbols.STOP]);
        }

        /// <summary>
        /// 将数字数组转换为图形编码
        /// </summary>
        /// <returns>返回图形编码</returns>
        protected string NumSetToEncode(List<int> result)
        {
            StringBuilder encode = new StringBuilder();
            foreach (int i in result)
            {
                encode.Append(symbols[i]);
            }
            return encode.ToString();
        }

        /// <summary>
        /// 给定一个字符串，判断从第一个字符开始，一共有几个连续的数字字符
        /// </summary>
        /// <param name="data">给定的字符串</param>
        /// <returns>返回连续的数字字符长度</returns>
        protected int GetDigitLen(string data)
        {
            int digitLen = 0;
            while (digitLen < data.Length && char.IsDigit(data[digitLen]))
            {
                digitLen++;
            }
            return digitLen;
        }

        /// <summary>
        /// 给定一个字符串，判断应当使用字符集A还是使用字符集B。
        /// 使用时请注意，不要传入开头为连续4个以上数字字符的字符串
        /// </summary>
        /// <param name="data">给定的字符串</param>
        /// <returns>返回A或B</returns>
        private CharSet GetModeAorB(string data)
        {
            bool hasLower = false;
            bool hasControl = false;
            foreach (char c in data)
            {
                if (char.IsLower(c))
                {
                    hasLower = true;
                }
                else if (IsControl(c))
                {
                    hasControl = true;
                }
                if (hasControl && !hasLower) //控制字符先于小写字母出现，使用字符集A
                {
                    return CharSet.A;
                }
            }
            return CharSet.B;
        }

        private enum CharType { Lower, Control, None };
        /// <summary>
        /// 判断一个字符串中是先出现小写字母键盘字符，还是先出现控制字符
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private CharType GetFirstCharType(string data)
        {
            foreach (char c in data)
            {
                if (IsLowerKey(c))
                {
                    return CharType.Lower;
                }
                else if (IsControl(c))
                {
                    return CharType.Control;
                }
            }
            return CharType.None;
        }

        private bool IsControl(char c)
        {
            if (c >= 0 && c <= 31)
            {
                return true;
            }
            return false;
        }

        private bool IsLowerKey(char c)
        {
            if (c >= 96 && c <= 127)
            {
                return true;
            }
            return false;
        }
    }
}
