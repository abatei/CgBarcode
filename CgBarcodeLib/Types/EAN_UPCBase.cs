using System;
using System.Collections.Generic;
using System.Text;

namespace CgBarcodeLib.Types
{
    /// <summary>
    /// EAN-13、EAN-8、UPC-A、UPC-E的共同基类
    /// </summary>
    public class EAN_UPCBase : BarcodeBase
    {
        protected static string[] SetA = { "0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011" };
        protected static string[] SetB = { "0100111", "0110011", "0011011", "0100001", "0011101", "0111001", "0000101", "0010001", "0001001", "0010111" };
        protected static string[] SetC = { "1110010", "1100110", "1101100", "1000010", "1011100", "1001110", "1010000", "1000100", "1001000", "1110100" };
        protected static string[] EAN_Pattern = { "aaaaaa", "aababb", "aabbab", "aabbba", "abaabb", "abbaab", "abbbaa", "ababab", "ababba", "abbaba" };
        protected static Dictionary<string, string> CountryCodes;

        public string Country { get; private set; }

        /// <summary>
        /// 修正原始编码，仅用于EAN-13、EAN-8编码
        /// </summary>
        /// <param name="EAN_len">只能是13或8，分别代表EAN-13和EAN-8</param>
        protected void AmendEAN(int EAN_len)
        {
            //判断编码所对应的国家
            if (CountryCodes == null)
            {
                Init_CountryCodes();
            }
            string threedigitCode = rawData.Substring(0, 3);
            if (CountryCodes.ContainsKey(threedigitCode))
            {
                Country = CountryCodes[threedigitCode];
            }
            else
            {
                error.Add("前缀码不正确，未找到相应国家！");
            }
            AmendLength(EAN_len);
        }

        /// <summary>
        /// 修正原始编码，仅用于ITF-14编码
        /// </summary>
        /// <param name="ITF_len"></param>
        protected void AmendITF()
        {
            //判断编码所对应的国家
            if (CountryCodes == null)
            {
                Init_CountryCodes();
            }
            string threedigitCode = rawData.Substring(1, 3);
            if (CountryCodes.ContainsKey(threedigitCode))
            {
                Country = CountryCodes[threedigitCode];
            }
            else
            {
                error.Add("前缀码不正确，未找到相应国家！");
            }
            AmendLength(14);
        }

        private static void AddCountryCodeRange(int startNum, int endNum, string countryDescription)
        {
            for (int i = startNum; i <= endNum; i++)
            {
                CountryCodes.Add(i.ToString("00"), countryDescription);
            }
        }

        protected static void Init_CountryCodes()
        {
            CountryCodes = new Dictionary<string, string>();
            AddCountryCodeRange(0, 19, "美国");
            AddCountryCodeRange(20, 29, "IN STORE");
            AddCountryCodeRange(30, 39, "美国");
            AddCountryCodeRange(40, 49, "Used to issue restricted circulation numbers within a geographic region (MO defined)");
            AddCountryCodeRange(50, 59, "美国 GS1 US 保留，未来使用");
            AddCountryCodeRange(60, 99, "美国");
            AddCountryCodeRange(100, 139, "美国");
            AddCountryCodeRange(200, 299, "Used to issue GS1 restricted circulation number within a geographic region (MO defined)");
            AddCountryCodeRange(300, 379, "法国");
            AddCountryCodeRange(380, 380, "保加利亚");
            AddCountryCodeRange(383, 383, "斯洛文尼亚");
            AddCountryCodeRange(385, 385, "克罗地亚");
            AddCountryCodeRange(387, 387, "波斯尼亚-黑塞哥维那");
            AddCountryCodeRange(389, 389, "黑山共和国");
            AddCountryCodeRange(400, 440, "德国");
            AddCountryCodeRange(450, 459, "日本");
            AddCountryCodeRange(460, 469, "俄罗斯");
            AddCountryCodeRange(470, 470, "吉尔吉斯斯坦");
            AddCountryCodeRange(471, 471, "中国台湾");
            AddCountryCodeRange(474, 474, "爱沙尼亚");
            AddCountryCodeRange(475, 475, "拉脱维亚");
            AddCountryCodeRange(476, 476, "阿塞拜疆");
            AddCountryCodeRange(477, 477, "立陶宛");
            AddCountryCodeRange(478, 478, "乌兹别克斯坦");
            AddCountryCodeRange(479, 479, "斯里兰卡");
            AddCountryCodeRange(480, 480, "菲律宾");
            AddCountryCodeRange(481, 481, "白俄罗斯");
            AddCountryCodeRange(482, 482, "乌克兰");
            AddCountryCodeRange(483, 483, "土库曼斯坦");
            AddCountryCodeRange(484, 484, "摩尔多瓦");
            AddCountryCodeRange(485, 485, "亚美尼亚");
            AddCountryCodeRange(486, 486, "乔治亚");
            AddCountryCodeRange(487, 487, "哈萨克斯坦");
            AddCountryCodeRange(488, 488, "塔吉克斯坦");
            AddCountryCodeRange(489, 489, "中国香港");
            AddCountryCodeRange(490, 499, "日本");
            AddCountryCodeRange(500, 509, "英国");
            AddCountryCodeRange(520, 521, "希腊");
            AddCountryCodeRange(528, 528, "黎巴嫩");
            AddCountryCodeRange(529, 529, "塞浦路斯");
            AddCountryCodeRange(530, 530, "阿尔巴尼亚");
            AddCountryCodeRange(531, 531, "马其顿");
            AddCountryCodeRange(535, 535, "马尔他");
            AddCountryCodeRange(539, 539, "爱尔兰");
            AddCountryCodeRange(540, 549, "比利时、卢森堡");
            AddCountryCodeRange(560, 560, "葡萄牙");
            AddCountryCodeRange(569, 569, "冰岛");
            AddCountryCodeRange(570, 579, "丹麦");
            AddCountryCodeRange(590, 590, "波兰");
            AddCountryCodeRange(594, 594, "罗马尼亚");
            AddCountryCodeRange(599, 599, "匈牙利");
            AddCountryCodeRange(600, 601, "南非");
            AddCountryCodeRange(603, 603, "加纳");
            AddCountryCodeRange(604, 604, "塞内加尔");
            AddCountryCodeRange(608, 608, "巴林");
            AddCountryCodeRange(609, 609, "毛里求斯");
            AddCountryCodeRange(611, 611, "摩洛哥");
            AddCountryCodeRange(613, 613, "阿尔及利亚");
            AddCountryCodeRange(615, 615, "尼日利亚");
            AddCountryCodeRange(616, 616, "肯尼亚");
            AddCountryCodeRange(618, 618, "科特迪瓦");
            AddCountryCodeRange(619, 619, "突尼斯");
            AddCountryCodeRange(620, 620, "坦桑尼亚");
            AddCountryCodeRange(621, 621, "叙利亚");
            AddCountryCodeRange(622, 622, "埃及");
            AddCountryCodeRange(623, 623, "文莱");
            AddCountryCodeRange(624, 624, "利比亚");
            AddCountryCodeRange(625, 625, "约旦");
            AddCountryCodeRange(626, 626, "伊朗");
            AddCountryCodeRange(627, 627, "科威特");
            AddCountryCodeRange(628, 628, "沙特阿拉伯");
            AddCountryCodeRange(629, 629, "阿拉伯联合酋长国");
            AddCountryCodeRange(640, 649, "芬兰");
            AddCountryCodeRange(690, 699, "中国");
            AddCountryCodeRange(700, 709, "挪威");
            AddCountryCodeRange(729, 729, "以色列");
            AddCountryCodeRange(730, 739, "瑞典");
            AddCountryCodeRange(740, 740, "危地马拉");
            AddCountryCodeRange(741, 741, "萨尔瓦多");
            AddCountryCodeRange(742, 742, "洪都拉斯");
            AddCountryCodeRange(743, 743, "尼加拉瓜");
            AddCountryCodeRange(744, 744, "哥斯达黎加");
            AddCountryCodeRange(745, 745, "巴拿马");
            AddCountryCodeRange(746, 746, "多米尼加");
            AddCountryCodeRange(750, 750, "墨西哥");
            AddCountryCodeRange(754, 755, "加拿大");
            AddCountryCodeRange(759, 759, "委内瑞拉");
            AddCountryCodeRange(760, 769, "瑞士");
            AddCountryCodeRange(770, 771, "哥伦比亚");
            AddCountryCodeRange(773, 773, "乌拉圭");
            AddCountryCodeRange(775, 775, "秘鲁");
            AddCountryCodeRange(777, 777, "波利维亚");
            AddCountryCodeRange(778, 779, "阿根廷");
            AddCountryCodeRange(780, 780, "智利");
            AddCountryCodeRange(784, 784, "巴拉圭");
            AddCountryCodeRange(786, 786, "厄瓜多尔");
            AddCountryCodeRange(789, 790, "巴西");
            AddCountryCodeRange(800, 839, "意大利");
            AddCountryCodeRange(840, 849, "西班牙");
            AddCountryCodeRange(850, 850, "古巴");
            AddCountryCodeRange(858, 858, "斯洛伐克");
            AddCountryCodeRange(859, 859, "捷克");
            AddCountryCodeRange(860, 860, "塞尔维亚和黑山");
            AddCountryCodeRange(865, 865, "蒙古");
            AddCountryCodeRange(867, 867, "朝鲜");
            AddCountryCodeRange(868, 869, "土尔其");
            AddCountryCodeRange(870, 879, "荷兰");
            AddCountryCodeRange(880, 880, "韩国");
            AddCountryCodeRange(884, 884, "柬埔寨");
            AddCountryCodeRange(885, 885, "泰国");
            AddCountryCodeRange(888, 888, "新加坡");
            AddCountryCodeRange(890, 890, "印度");
            AddCountryCodeRange(893, 893, "越南");
            AddCountryCodeRange(896, 896, "巴基斯坦");
            AddCountryCodeRange(899, 899, "印度尼西亚");
            AddCountryCodeRange(900, 919, "奥地利");
            AddCountryCodeRange(930, 939, "澳大利亚");
            AddCountryCodeRange(940, 949, "新西兰");
            AddCountryCodeRange(950, 950, "国际物品编码协会总部");
            AddCountryCodeRange(951, 951, "EPC 全球特殊应用");
            AddCountryCodeRange(955, 955, "马来西亚");
            AddCountryCodeRange(958, 958, "中国澳门");
            AddCountryCodeRange(960, 961, "GS1 UK OFFICE: GTIN-8 ALLOCATIONS");
            AddCountryCodeRange(962, 969, "GS1 GLOBAL OFFICE: GTIN-8 ALLOCATIONS");
            AddCountryCodeRange(977, 977, "系列出版物 (ISSN)");
            AddCountryCodeRange(978, 979, "图书 (ISBN)");
            AddCountryCodeRange(980, 980, "退款收据");
            AddCountryCodeRange(981, 984, "GS1 COUPON IDENTIFICATION FOR COMMON CURRENCY AREAS");
            AddCountryCodeRange(990, 999, "GS1 COUPON IDENTIFICATION");
        }
    }
}
