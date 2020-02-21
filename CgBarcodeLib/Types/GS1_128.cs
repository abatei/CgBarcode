/*******************************************************************************************
 * 作者：陈广
 * 开始日期：2019 年 9 月 2 日
 * 网址：www.iotxfd.cn
 * 内容：GS1-128 条码类，参照国家标准 GB/T 15425 — 2014 进行编写。
 * GS1-128 使用 Code 128 编码，此处仅支持使用 Code 128 Auto 表示 GS1-128
 * 在输入 GS1-128 编码时，请使用小括号括住 AI 编号，此处会自动将其解析为合适的图形编码
 * 在搞了一轮后突然发现完全不能复用 Code 128 的逻辑进行编码，因为 FNC1 有可能被安插在
 * Code-C 中一个编码所对应的两个数字的中间，杯具啊！只能重写。
 * 稍感安慰的是，本 GS1-128 代码仅用于 AI 的编码，而 AI 所能包含的字符不包括控制字符，所以不
 * 会使用到字符集 A，降低了程序的复杂度。
 * (02)66901234000049(17)050101(37)10(10)ABC
 *******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace CgBarcodeLib.Types
{
    class GS1_128 : Code128, IBarcode
    {
        public GS1_128(string input)
        {
            if (input.Length < 1 || input.Length > 48)
            {
                throw new ArgumentException("原始字符串中的字符个数必须在 1~48 之间！");
            }
            rawData = input;
            ParseInput();
        }

        const int FNC1 = 102;
        AI ais = new AI();
        List<KeyValuePair<string, string>> aiSet = new List<KeyValuePair<string, string>>();
        CharSet mode = CharSet.Auto;

        public override string GetEncode()
        {
            mode = CharSet.Auto; //借此标记为未加入起始符
            List<int> result = new List<int>();
            string raw = "";
            for (int i = 0; i < aiSet.Count; i++)
            {
                raw += aiSet[i].Key + aiSet[i].Value;
                // AI 为固定长度，则继续连接下一个 AI
                // AI 为可变长度，需加入 FNC1，但如果已到结尾，则无需加入。
                if ((i < aiSet.Count - 1) && ais.IsFixed(aiSet[i].Key))
                {
                    continue;
                }
                result.AddRange(EncodeSection(raw));
                raw = "";
                if (i < aiSet.Count - 1)
                {
                    result.Add(FNC1);
                }
            }
            AddCheckSumAndStop(result);
            Errors.Add(symbols.NumSetToDescribe(result));
            return NumSetToEncode(result);
        }

        /// <summary>
        /// 对单个片段进行编码
        /// </summary>
        /// <param name="raw">编码字符串片段</param>
        /// <returns></returns>
        private List<int> EncodeSection(string raw)
        {
            List<int> result = new List<int>();
            int index = 0;
            while (index < raw.Length)
            {   /* 如果在字符集 B 中同时产生 4 位或更多的数字型字符：
                 * b)如果数字型数据符个数为奇数，则在第一个数字之后插入字符集 C 字符以便转换为字符集 C
                 * a)如果数字型数据符个数为偶数，则在第一个数字之前插入字符集 C 字符以便转换为字符集 C */
                int digitLen = GetDigitLen(raw.Substring(index));
                if (digitLen >= 4)
                {
                    if (mode == CharSet.Auto) //表明此时为第一个片段
                    {
                        mode = CharSet.C;
                        //加入起始字符 START_C
                        result.Add(symbols[mode, Symbols.START_C]);
                        result.Add(FNC1);
                    }
                    else if (mode == CharSet.B)
                    {
                        if (digitLen % 2 == 1) //加入第一个数字字符，使用原来的 B 字符集
                        {
                            result.Add(symbols[mode, raw[index]]);
                            index++;
                            digitLen--;
                        }
                        //转换为字符集 C
                        result.Add(symbols[CharSet.B, Symbols.CODE_C]);
                        mode = CharSet.C;
                    }
                    //加入剩余数字字符，此时字符个数为偶数
                    int i;
                    for (i = 0; i < digitLen - 1; i += 2)
                    {
                        result.Add(symbols[CharSet.C, raw.Substring(index + i, 2)]);
                    }
                    index += i;
                    continue;
                }

                if (mode == CharSet.C)
                {
                    //如果有两个连续数字，需先使用字符集C表示
                    if (digitLen >= 2)
                    {
                        result.Add(symbols[CharSet.C, raw.Substring(index, 2)]);
                        index += 2;
                    }
                    //此时刚处理完连续 4 个以上数字，需转换为字符集A或B。
                    //如果字符集 C 中数字为奇数个，也会走到这一步
                    mode = CharSet.B;
                    result.Add(symbols[CharSet.C, Symbols.CODE_B]);
                }
                //最后是正常插入每一个字符
                result.Add(symbols[mode, raw[index]]);
                index++;
            }
            return result;
        }
        /// <summary>
        /// 将每个 AI 编码及其对应的数据编码部分分离出来，以键值对的形式存入 List
        /// </summary>
        /// <returns></returns>
        private void ParseInput()
        {
            if (rawData[0] != '(')
            {
                throw new ArgumentException("GS1-128 编码必须以小括号括起的 AI 代码开始！");
            }
            aiSet.Clear();
            int left = -1; //左括号位置索引
            int right = -1; //右括号位置索引
            StringBuilder ai_Num = new StringBuilder();
            StringBuilder ai_Data = new StringBuilder();
            for (int i = 0; i < rawData.Length; i++)
            {
                if (rawData[i] == '(')
                {
                    left = i;
                    right = -1;
                    if (ai_Num.Length != 0)
                    {
                        aiSet.Add(new KeyValuePair<string, string>(ai_Num.ToString(), ai_Data.ToString()));
                    }
                    ai_Num.Clear();
                    ai_Data.Clear();
                    continue;
                }
                else if (rawData[i] == ')')
                {
                    right = i;
                    left = -1;
                    continue;
                }
                if (left != -1 && i > left)
                {
                    ai_Num.Append(rawData[i]);
                }
                if (right != -1 && i > right)
                {
                    ai_Data.Append(rawData[i]);
                }
            }
            aiSet.Add(new KeyValuePair<string, string>(ai_Num.ToString(), ai_Data.ToString()));
            //检查 AI 合法性
            foreach (var pair in aiSet)
            {
                if (!CheckNumericOnly(pair.Key))
                {
                    throw new ArgumentException("小括号中的 AI 代码必须为数字！");
                }
                AI_Struct ai_struct = ais[pair.Key];
                CheckAIData(ai_struct, pair.Value);
                //加入提示
                Errors.Add("(" + ai_struct.AI_Num + ")：" + ai_struct.Name);
            }
        }

        /// <summary>
        /// 检查 AI 数据编码部分是否合法
        /// </summary>
        /// <param name="ai"></param>
        /// <param name="ai_data"></param>
        public void CheckAIData(AI_Struct ai, string ai_data)
        {
            if (ai_data.Length < ai.MinLen || ai_data.Length > ai.MaxLen)
            {
                throw new ArgumentException("AI：" + ai + "的数据编码部分长度不正确！");
            }
            if (ai.Comprise == CharType.Number)
            {
                if (!CheckNumericOnly(ai_data))
                {
                    throw new ArgumentException("AI：" + ai + "的数据编码部分只能为数字！");
                }
            }
            else
            {
                foreach (char c in ai_data)
                {
                    if (!CheckChar(c))
                    {
                        throw new ArgumentException("AI：" + ai + "的数据编码部分存在非法字符！");
                    }
                }
            }
        }

        /// <summary>
        /// 判断一个字符是否是 AI 中的合法字符。
        /// 参考 GBT 16986-2018 规范中的附录 B 的表 B.1
        /// </summary>
        /// <param name="c">字符</param>
        /// <returns>合法则返回 true，否则返回 false</returns>
        public bool CheckChar(char c)
        {
            if ((c >= 37 && c <= 63) || (c >= 65 && c <= 90) ||
                (c >= 97 && c <= 122) || c == 33 || c == 34 || c == 95)
            {
                return true;
            }
            return false;
        }
    }
}
