/* 作者：陈广
 * 开始日期：2019 年 8 月 16 日
 * 网址：www.iotxfd.cn
 * 内容：应用标识符 application identifier 类，参照最新国际标准 GBT16986-2018 编写
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgBarcodeLib.Types;

namespace CgBarcodeLib
{
    /// <summary>
    /// 应用标识符数据编码部分组成结构
    /// </summary>
    public enum CharType
    {
        Number, //只能是数字
        NumberAndLetter //数字和字母混合
    };
    public class AI_Struct
    {
        /// <summary>
        /// 应用标识符，不包含数据编码部分
        /// </summary>
        public string AI_Num { get; set; }
        /// <summary>
        /// 应用标识符名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注，更为详细的描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 应用标识符数据编码部分最小长度，如果是固定长度，则 MinLen 和 MaxLen 值相等。
        /// </summary>
        public int MinLen { get; set; }
        /// <summary>
        /// 应用标识符数据编码部分最大长度，如果是固定长度，则 MinLen 和 MaxLen 值相等。
        /// </summary>
        public int MaxLen { get; set; }
        /// <summary>
        /// 应用标识符数据编码部分是纯数字组成还是由数字和字母组成
        /// </summary>
        public CharType Comprise { get; set; }

        public AI_Struct(string aiNum, string name, int minLen, int maxLen, CharType comprise)
        {
            AI_Num = aiNum;
            Name = name;
            MinLen = minLen;
            MaxLen = maxLen;
            Comprise = comprise;
        }
    }

    /// <summary>
    /// 尾数不带 n 的 AI
    /// </summary>
    public class AI
    {
        private static Dictionary<string, AI_Struct> ai_set = new Dictionary<string, AI_Struct>()
        {
            { "00", new AI_Struct("00", "系列货运包装箱代码（SSCC）", 18, 18, CharType.Number) },
            { "01", new AI_Struct("01", "全球贸易项目代码（GTIN）", 14, 14, CharType.Number) },
            { "02", new AI_Struct("02", "物流单元内贸易项目", 14, 14, CharType.Number) },
            { "10", new AI_Struct("10", "批号", 1, 18, CharType.NumberAndLetter) },
            { "11", new AI_Struct("11", "生产日期", 6, 6, CharType.Number) },
            { "12", new AI_Struct("12", "付款截止日期", 6, 6, CharType.Number) },
            { "13", new AI_Struct("13", "包装日期", 6, 6, CharType.Number) },
            { "15", new AI_Struct("15", "保质期", 6, 6, CharType.Number) },
            { "16", new AI_Struct("16", "销售截止日期", 6, 6, CharType.Number) },
            { "17", new AI_Struct("17", "有效期", 6, 6, CharType.Number) },
            { "20", new AI_Struct("20", "内部产品变体", 2, 2, CharType.Number) },
            { "21", new AI_Struct("21", "系列号", 1, 18, CharType.NumberAndLetter) },
            { "22", new AI_Struct("22", " 消费品变体", 1, 18, CharType.NumberAndLetter) },
            { "240", new AI_Struct("240", "附加产品标识", 1, 27, CharType.NumberAndLetter) },
            { "241", new AI_Struct("241", "客户方代码", 1, 27, CharType.NumberAndLetter) },
            { "242", new AI_Struct("242", "定制产品变体代码", 1, 3, CharType.Number) },
            { "243", new AI_Struct("243", "包装组件代码", 1, 17, CharType.NumberAndLetter) },
            { "250", new AI_Struct("250", "二级系列号", 1, 27, CharType.NumberAndLetter) },
            { "251", new AI_Struct("251", "源实体参考代码", 1, 27, CharType.NumberAndLetter) },
            { "253", new AI_Struct("253", "全球文件类型代码", 1, 14, CharType.Number) },
            { "254", new AI_Struct("254", "全球参与方位置码扩展部分代码", 1, 17, CharType.NumberAndLetter) },
            { "255", new AI_Struct("255", "全球优惠券代码", 13, 25, CharType.Number) },
            { "30", new AI_Struct("30", "变量贸易项目中项目数量", 1, 6, CharType.Number) },
            { "37", new AI_Struct("37", "物流单元内贸易项目数量", 1, 8, CharType.Number) },
            { "400",  new AI_Struct("400", "客户订单代码", 1, 30, CharType.NumberAndLetter) },
            { "401",  new AI_Struct("401", "全球货物托运标识代码", 1, 30, CharType.NumberAndLetter) },
            { "402",  new AI_Struct("402", "全球货物托运标识代码", 17, 17, CharType.NumberAndLetter) },
            { "403",  new AI_Struct("403", "路径代码", 1, 30, CharType.NumberAndLetter) },
            { "410",  new AI_Struct("410", "交货地全球位置码", 13, 13, CharType.Number) },
            { "411",  new AI_Struct("411", "受票方全球位置码", 13, 13, CharType.Number) },
            { "412",  new AI_Struct("412", "供货方全球位置码", 13, 13, CharType.Number) },
            { "413",  new AI_Struct("413", "货物最终目的地全球位置码", 13, 13, CharType.Number) },
            { "414",  new AI_Struct("414", "标识物理位置的全球位置码", 13, 13, CharType.Number) },
            { "415",  new AI_Struct("415", "开票方全球位置码", 13, 13, CharType.Number) },
            { "416",  new AI_Struct("416", "产品或服务所在地全球位置码", 13, 13, CharType.Number) },
            { "420",  new AI_Struct("420", "交货地邮政编码", 1, 20, CharType.NumberAndLetter) },
            { "421",  new AI_Struct("421", "含 ISO 国家(地区)代码的交货地邮政编码", 3, 12, CharType.NumberAndLetter) },
            { "422",  new AI_Struct("422", "贸易项目原产国(地区)代码", 3, 3, CharType.Number) },
            { "423",  new AI_Struct("423", "贸易项目初始加工国家(地区)代码", 3, 15, CharType.Number) },
            { "424",  new AI_Struct("424", "贸易项目加工国家(地区)代码", 3, 3, CharType.Number) },
            { "425",  new AI_Struct("425", "贸易项目拆分国家(地区)代码", 3, 15, CharType.Number) },
            { "426",  new AI_Struct("426", "全程加工贸易项目的国家(地区)代码", 3, 3, CharType.Number) },
            { "427",  new AI_Struct("427", "贸易项目原产地国家(地区)行政区划代码", 1, 3, CharType.NumberAndLetter) },
            { "7001",  new AI_Struct("7001", "北约物资代码", 13, 13, CharType.Number) },
            { "7002",  new AI_Struct("7002", "胴体肉与分割产品分类", 1, 30, CharType.NumberAndLetter) },
            { "7003",  new AI_Struct("7003", "产品有效日期和时间", 10, 10, CharType.Number) },
            { "7004",  new AI_Struct("7004", "活性值", 1, 4, CharType.NumberAndLetter) },
            { "7005",  new AI_Struct("7005", "捕捞区域代码", 1, 12, CharType.NumberAndLetter) },
            { "7006",  new AI_Struct("7006", "首次冷冻日期", 6, 6, CharType.Number) },
            { "7007",  new AI_Struct("7007", "收获日期", 12, 12, CharType.Number) },
            { "7008",  new AI_Struct("7008", "渔业品种代码", 1, 13, CharType.NumberAndLetter) },
            { "7009",  new AI_Struct("7009", "渔具类型代码", 1, 10, CharType.NumberAndLetter) },
            { "7010",  new AI_Struct("7010", "生产方法代码", 1, 2, CharType.NumberAndLetter) },
            { "7020",  new AI_Struct("7020", "翻新批次标识代码", 1, 20, CharType.NumberAndLetter) },
            { "7021",  new AI_Struct("7021", "功能状态代码", 1, 20, CharType.NumberAndLetter) },
            { "7022",  new AI_Struct("7022", "调整状态代码", 1, 20, CharType.NumberAndLetter) },
            { "7023",  new AI_Struct("7023", "组合件全球单个资产代码", 1, 30, CharType.NumberAndLetter) },
            { "8001", new AI_Struct("8001", "卷状产品可变属性值", 14, 14, CharType.Number) },
            { "8002", new AI_Struct("8002", "蜂窝移动电话", 1, 20, CharType.NumberAndLetter) },
            { "8003", new AI_Struct("8003", "全球可回收资产代码", 14, 30, CharType.NumberAndLetter) },
            { "8004", new AI_Struct("8004", "全球单个资产代码", 1, 30, CharType.NumberAndLetter) },
            { "8005", new AI_Struct("8005", "变量贸易项目单价", 6, 6, CharType.Number) },
            { "8006", new AI_Struct("8006", "贸易项目组件标识代码", 18, 18, CharType.Number) },
            { "8007", new AI_Struct("8007", "国际银行账号", 1, 34, CharType.Number) },
            { "8008", new AI_Struct("8008", "产品生产日期与时间", 12, 12, CharType.Number) },
            { "8010", new AI_Struct("8010", "组件/部件代码", 1, 30, CharType.NumberAndLetter) },
            { "8011", new AI_Struct("8011", "组件/部件序列号", 1, 12, CharType.Number) },
            { "8012", new AI_Struct("8012", "软件版本号", 1, 20, CharType.NumberAndLetter) },
            { "8013", new AI_Struct("8013", "全球型号代码", 1, 30, CharType.NumberAndLetter) },
            { "8017", new AI_Struct("8017", "全球服务关系提供方代码", 18, 18, CharType.Number) },
            { "8018", new AI_Struct("8018", "全球服务关系接受方代码", 18, 18, CharType.Number) },
            { "8019", new AI_Struct("8019", "服务关系事项代码", 1, 10, CharType.Number) },
            { "8020", new AI_Struct("8020", "付款单参考代码",1, 25, CharType.NumberAndLetter) },
            { "8111", new AI_Struct("8111", "优惠券忠诚度积分", 4, 4, CharType.Number) },
            { "8200", new AI_Struct("8200", "扩展包装 URL", 1, 70, CharType.NumberAndLetter) },
            { "90", new AI_Struct("90", "贸易伙伴之间相互约定的信息", 1, 30, CharType.NumberAndLetter) },
            { "91", new AI_Struct("91", "公司内部信息", 1, 90, CharType.NumberAndLetter) },
            { "92", new AI_Struct("92", "公司内部信息", 1, 90, CharType.NumberAndLetter) },
            { "93", new AI_Struct("93", "公司内部信息", 1, 90, CharType.NumberAndLetter) },
            { "94", new AI_Struct("94", "公司内部信息", 1, 90, CharType.NumberAndLetter) },
            { "95", new AI_Struct("95", "公司内部信息", 1, 90, CharType.NumberAndLetter) },
            { "96", new AI_Struct("96", "公司内部信息", 1, 90, CharType.NumberAndLetter) },
            { "97", new AI_Struct("97", "公司内部信息", 1, 90, CharType.NumberAndLetter) },
            { "98", new AI_Struct("98", "公司内部信息", 1, 90, CharType.NumberAndLetter) },
            { "99", new AI_Struct("99", "公司内部信息", 1, 90, CharType.NumberAndLetter) }
        };

        /// <summary>
        /// 尾数带 n 的 AI
        /// </summary>
        private static Dictionary<string, AI_Struct> ain_set = new Dictionary<string, AI_Struct>()
        {
          { "310", new AI_Struct("310n", "净重（千克）", 1, 6, CharType.Number) },
          { "311", new AI_Struct("311n", "长度或第一尺寸（米）", 1, 6, CharType.Number) },
          { "312", new AI_Struct("312n", "宽度、直径或第二尺寸（米）", 1, 6, CharType.Number) },
          { "313", new AI_Struct("313n", "深度、厚度、高度或第三尺寸（米）", 1, 6, CharType.Number) },
          { "314", new AI_Struct("314n", "面积（平方米）", 1, 6, CharType.Number) },
          { "315", new AI_Struct("315n", "净体积、净容积（升）", 1, 6, CharType.Number) },
          { "316", new AI_Struct("316n", "净体积、净容积（立方米）", 1, 6, CharType.Number) },
          { "320", new AI_Struct("320n", "净重（磅）", 1, 6, CharType.Number) },
          { "321", new AI_Struct("321n", "长度或第一尺寸（英寸）", 1, 6, CharType.Number) },
          { "322", new AI_Struct("322n", "长度或第一尺寸（英尺）", 1, 6, CharType.Number) },
          { "323", new AI_Struct("323n", "长度或第一尺寸（码）", 1, 6, CharType.Number) },
          { "324", new AI_Struct("324n", "宽度、直径或第二尺寸（英寸）", 1, 6, CharType.Number) },
          { "325", new AI_Struct("325n", "宽度、直径或第二尺寸（英尺）", 1, 6, CharType.Number) },
          { "326", new AI_Struct("326n", "宽度、直径或第二尺寸（码）", 1, 6, CharType.Number) },
          { "327", new AI_Struct("327n", "深度、厚度、高度或第三尺寸（英寸）", 1, 6, CharType.Number) },
          { "328", new AI_Struct("328n", "深度、厚度、高度或第三尺寸（英尺）", 1, 6, CharType.Number) },
          { "329", new AI_Struct("329n", "深度、厚度、高度或第三尺寸（码）", 1, 6, CharType.Number) },
          { "350", new AI_Struct("350n", "面积（平方英寸）", 1, 6, CharType.Number) },
          { "351", new AI_Struct("351n", "面积（平方英尺）", 1, 6, CharType.Number) },
          { "352", new AI_Struct("352n", "面积（平方码）", 1, 6, CharType.Number) },
          { "356", new AI_Struct("356n", "净重（英两）", 1, 6, CharType.Number) },
          { "357", new AI_Struct("357n", "净重或净体积、净容积（盎司）", 1, 6, CharType.Number) },
          { "360", new AI_Struct("360n", "净体积、净容积（夸脱）", 1, 6, CharType.Number) },
          { "361", new AI_Struct("361n", "净体积、净容积（加仑）", 1, 6, CharType.Number) },
          { "364", new AI_Struct("364n", "净体积、净容积（立方英寸）", 1, 6, CharType.Number) },
          { "365", new AI_Struct("365n", "净体积、净容积（立方英尺）", 1, 6, CharType.Number) },
          { "366", new AI_Struct("366n", "净体积、净容积（立方码）", 1, 6, CharType.Number) },
          { "330", new AI_Struct("330n", "物流重量（千克）", 1, 6, CharType.Number) },
          { "331", new AI_Struct("331n", "长度或第一尺寸（米）", 1, 6, CharType.Number) },
          { "332", new AI_Struct("332n", "宽度、直径或第二尺寸（米）", 1, 6, CharType.Number) },
          { "333", new AI_Struct("333n", "深度、厚度、高度或第三尺寸（米）", 1, 6, CharType.Number) },
          { "334", new AI_Struct("334n", "面积（平方米）", 1, 6, CharType.Number) },
          { "335", new AI_Struct("335n", "物流体积（升）", 1, 6, CharType.Number) },
          { "336", new AI_Struct("336n", "物流体积（立方米）", 1, 6, CharType.Number) },
          { "337", new AI_Struct("337n", "贸易项目的千克每平方米(kg/m2)数值", 1, 6, CharType.Number) },
          { "340", new AI_Struct("340n", "物流重量（磅）", 1, 6, CharType.Number) },
          { "341", new AI_Struct("341n", "长度或第一尺寸（英寸）", 1, 6, CharType.Number) },
          { "342", new AI_Struct("342n", "长度或第一尺寸（英尺）", 1, 6, CharType.Number) },
          { "343", new AI_Struct("343n", "长度或第一尺寸（码）", 1, 6, CharType.Number) },
          { "344", new AI_Struct("344n", "宽度、直径或第二尺寸（英寸）", 1, 6, CharType.Number) },
          { "345", new AI_Struct("345n", "宽度、直径或第二尺寸（英尺）", 1, 6, CharType.Number) },
          { "346", new AI_Struct("346n", "宽度、直径或第二尺寸（码）", 1, 6, CharType.Number) },
          { "347", new AI_Struct("347n", "深度、厚度、高度或第三尺寸（英寸）", 1, 6, CharType.Number) },
          { "348", new AI_Struct("348n", "深度、厚度、高度或第三尺寸（英尺）", 1, 6, CharType.Number) },
          { "349", new AI_Struct("349n", "深度、厚度、高度或第三尺寸（码）", 1, 6, CharType.Number) },
          { "353", new AI_Struct("353n", "面积（平方英寸）", 1, 6, CharType.Number) },
          { "354", new AI_Struct("354n", "面积（平方英尺）", 1, 6, CharType.Number) },
          { "355", new AI_Struct("355n", "面积（平方英码）", 1, 6, CharType.Number) },
          { "362", new AI_Struct("362n", "物流体积（夸脱）", 1, 6, CharType.Number) },
          { "363", new AI_Struct("363n", "物流体积（加仑）", 1, 6, CharType.Number) },
          { "367", new AI_Struct("367n", "物流体积（立方英寸）", 1, 6, CharType.Number) },
          { "368", new AI_Struct("368n", "物流体积（立方英尺）", 1, 6, CharType.Number) },
          { "369", new AI_Struct("369n", "物流体积（立方码）", 1, 6, CharType.Number) },
          { "390", new AI_Struct("390n", "应支付金额或优惠券价值", 1, 15, CharType.Number) },
          { "391", new AI_Struct("391n", "应支付金额或优惠券价值", 7, 21, CharType.Number) },
          { "392", new AI_Struct("392n", "变量贸易项目应支付金额", 1, 15, CharType.Number) },
          { "393", new AI_Struct("393n", "含 ISO 货币代码的变量贸易项目应支付金额", 7, 21, CharType.Number) },
          { "394", new AI_Struct("394n", "优惠券折扣百分比", 4, 4, CharType.Number) },
          { "703", new AI_Struct("703n", "含 ISO 国家(地区)代码的加工者核准号码", 3, 30, CharType.NumberAndLetter) }
        };

        /// <summary>
        /// 索引器，通过 AI 号返回 AI 信息
        /// </summary>
        /// <param name="ai_num"></param>
        /// <returns>查找成功则返回 AI 信息，失败返回 null</returns>
        public AI_Struct this[string ai_num]
        {
            get
            {
                if (ai_set.ContainsKey(ai_num))
                {
                    return ai_set[ai_num];
                }
                if (ai_num.Length == 4)
                {
                    string ai = ai_num.Substring(0, 3);
                    if (ain_set.ContainsKey(ai))
                    {
                        return ain_set[ai];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 判断指定 AI 数据编码部分是否为指定长度
        /// </summary>
        /// <param name="ai_num"></param>
        /// <returns>如果此 AI 为固定长度则返回 true，否则返回 false</returns>
        public bool IsFixed(string ai_num)
        {
            AI_Struct ai_struct = this[ai_num];
            if (ai_struct.MinLen == ai_struct.MaxLen)
            {
                return true;
            }
            return false;
        }
    }
}
