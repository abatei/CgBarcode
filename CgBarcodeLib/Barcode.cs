using CgBarcodeLib.Types;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CgBarcodeLib
{
    public enum TYPE : int { UPCA, UPCE, EAN13, EAN8, Interleaved2of5, ITF14, CODE128, CODE128A, CODE128B, CODE128C, GS1_128 };

    public class Barcode
    {
        public Barcode(string data, TYPE iType)
        {
            this.RawData = data;
            this.Encoded_Type = iType;
            switch (iType)
            {
                case TYPE.EAN13:
                    ibarcode = new EAN13(data);
                    country = ((EAN_UPCBase)ibarcode).Country;
                    break;
                case TYPE.EAN8:
                    ibarcode = new EAN8(data);
                    country = ((EAN_UPCBase)ibarcode).Country;
                    break;
                case TYPE.UPCA:
                    ibarcode = new UPCA(data);
                    break;
                case TYPE.UPCE:
                    ibarcode = new UPCE(data);
                    break;
                case TYPE.Interleaved2of5:
                    ibarcode = new Interleaved2of5(data);
                    break;
                case TYPE.ITF14:
                    ibarcode = new ITF14(data);
                    break;
                case TYPE.CODE128A:
                    ibarcode = new Code128(data, CharSet.A);
                    break;
                case TYPE.CODE128B:
                    ibarcode = new Code128(data, CharSet.B);
                    break;
                case TYPE.CODE128C:
                    ibarcode = new Code128(data, CharSet.C);
                    break;
                case TYPE.CODE128:
                    ibarcode = new Code128(data, CharSet.Auto);
                    break;
                case TYPE.GS1_128:
                    ibarcode = new GS1_128(data);
                    break;
            }
            RawData = ibarcode.RawData;
            EncodeValue = ibarcode.GetEncode();
            SetFontSize(RawData);
        }

        private IBarcode ibarcode;
        public string EncodeValue { get; set; }

        #region 属性
        public string RawData { get; set; }
        public TYPE Encoded_Type { get; private set; } = TYPE.EAN13;
        private string country;
        public string Country
        {
            get { return country; }
        }
        public Color ForeColor { get; set; } = Color.Black;
        public Color BackColor { get; set; } = Color.White;

        private Font font = new Font("Microsoft Sans Serif", 15);
        private int fontWidth; //文字的宽度，以数字 8 为准
        private int fontHeight; //文字的高度，以数字8为准

        /// <summary>
        /// 供人识别字体名称，字符串表示
        /// </summary>
        public string LabelFontStyle
        {
            get => font.Name;
            set
            {
                font = new Font(value, LabelFontSize);
                SetFontSize(RawData);
            }
        }
        /// <summary>
        /// 供人识别字体大小
        /// </summary>
        public int LabelFontSize
        {
            get => (int)font.Size;
            set
            {
                font = new Font(LabelFontStyle, value);
                SetFontSize(RawData);
            }
        }
        /// <summary>
        /// 标签显示文字的字间距，仅对交叉25码、ITF-14、Code128、GS1-128条码有效
        /// </summary>
        public int FontSpace { get; set; } = 5;
        /// <summary>
        /// 条码高度，不包含文字部分
        /// </summary>
        public int Height { get; set; } = 150;
        /// <summary>
        /// 线条的宽窄比,默认为 2.5，仅用于交叉25码和ITF-14码
        /// </summary>
        public float WNRate { get; set; } = 2.5f;
        /// <summary>
        /// ITF-14 保护框宽度是窄条宽度的几倍，最小为2倍，此处默认值为3倍
        /// </summary>
        public int BearerbarWidth { get; set; } = 3;

        /// <summary>
        /// 一个模块的宽度，也就是最小条或空的宽度，以像素为单位，默认值为 5
        /// </summary>
        public int ModuleWidth { get; set; } = 3;

        /// <summary>
        /// //左侧空白区和右侧空白区占用多少个模块宽度
        /// </summary>
        public int SpaceWidth { get; set; } = 10;
        public bool IncludeLabel { get; set; }

        public List<string> Errors
        {
            get
            {
                return this.ibarcode?.Errors;
            }
        }
        #endregion

        #region 绘制相关代码
        public Bitmap GetBitmap()
        {
            switch (Encoded_Type)
            {
                case TYPE.EAN13:
                    return Draw_EAN13();
                case TYPE.EAN8:
                    return Draw_EAN8();
                case TYPE.UPCA:
                    return Draw_UPCA();
                case TYPE.UPCE:
                    return Draw_UPCE();
                case TYPE.Interleaved2of5:
                    return DrawInterleaved2of5Bar();
                case TYPE.ITF14:
                    return DrawITF14();
                case TYPE.CODE128A:
                case TYPE.CODE128B:
                case TYPE.CODE128C:
                case TYPE.CODE128:
                case TYPE.GS1_128:
                    return DrawCode128();
            }
            return null;
        }

        private Bitmap DrawCode128()
        {
            //绘制背景
            Bitmap bmp = GetBlankBitmap();
            Graphics g = Graphics.FromImage(bmp);
            //绘制条码
            int width = EncodeValue.Length * ModuleWidth + SpaceWidth * ModuleWidth * 2;
            Pen bp = new Pen(ForeColor, ModuleWidth);
            for (int i = 0; i < EncodeValue.Length; i++)
            {
                int x = i * ModuleWidth + SpaceWidth * ModuleWidth;
                if (EncodeValue[i] == '1')
                {
                    g.DrawLine(bp, x, 5, x, Height + 5);
                }
            }
            //绘制文字
            DrawLabelText(g, width, Height + 5);
            return bmp;
        }

        private Bitmap DrawInterleaved2of5Bar()
        {
            int wpWidth = (int)(ModuleWidth * WNRate); //宽线宽度
            int left = SpaceWidth * ModuleWidth;
            //绘制背景
            SolidBrush sbBg = new SolidBrush(BackColor);
            int height = Height + 5 + fontHeight;//计算背景图片的高度
            //计算背景图片的宽度
            int width = left * 2;
            foreach (char c in EncodeValue)
            {
                if (c == 'N')
                {
                    width += ModuleWidth;
                }
                else
                {
                    width += wpWidth;
                }
            }
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            Rectangle rect = new Rectangle(0, 0, width, height);
            g.FillRectangle(sbBg, rect);

            //绘制条码
            SolidBrush sb = new SolidBrush(ForeColor);
            for (int i = 0; i < EncodeValue.Length; i++)
            {
                if (EncodeValue[i] == 'N')
                {
                    if (i % 2 == 0)
                    {
                        g.FillRectangle(sb, left, 5, ModuleWidth, Height); //画窄线
                    }
                    left += ModuleWidth;
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        g.FillRectangle(sb, left, 5, wpWidth, Height);//画宽线
                    }
                    left += wpWidth;
                }
            }
            //绘制文字
            DrawLabelText(g, width, Height + 5);
            return bmp;
        }

        private Bitmap DrawITF14()
        {
            if (SpaceWidth < 10) SpaceWidth = 10;
            int wpWidth = (int)(ModuleWidth * WNRate); //宽线宽度
            int left = (SpaceWidth + BearerbarWidth) * ModuleWidth;
            //绘制背景
            SolidBrush sbBg = new SolidBrush(BackColor);
            //计算背景图片的宽度
            int width = left * 2;
            foreach (char c in EncodeValue)
            {
                if (c == 'N')
                {
                    width += ModuleWidth;
                }
                else
                {
                    width += wpWidth;
                }
            }
            int bearerW = BearerbarWidth * ModuleWidth; //保护框宽度，像素单位
            int height = Height + bearerW * 2;
            Bitmap bmp = new Bitmap(width, height + fontHeight);
            Graphics g = Graphics.FromImage(bmp);
            Rectangle rect = new Rectangle(0, 0, width, height + fontHeight);
            g.FillRectangle(sbBg, rect);
            //绘制保护框
            Pen p = new Pen(ForeColor, bearerW);
            g.DrawRectangle(p, 0 + bearerW / 2, 0 + bearerW / 2, width - bearerW, height - bearerW);
            //绘制条码
            SolidBrush sb = new SolidBrush(ForeColor);
            for (int i = 0; i < EncodeValue.Length; i++)
            {
                if (EncodeValue[i] == 'N')
                {
                    if (i % 2 == 0)
                    {
                        g.FillRectangle(sb, left, bearerW, ModuleWidth, Height); //画窄线
                    }
                    left += ModuleWidth;
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        g.FillRectangle(sb, left, bearerW, wpWidth, Height);//画宽线
                    }
                    left += wpWidth;
                }
            }
            //绘制文字
            DrawLabelText(g, width, bearerW * 2 + Height);
            return bmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘制的画布</param>
        /// <param name="gWidth">条码所占空间（包含左右侧空白区）的宽度</param>
        /// <param name="top">绘制起始点的 y 坐标</param>
        private void DrawLabelText(Graphics g, int width, int top)
        {
            int textWidth = RawData.Length * fontWidth + FontSpace * (RawData.Length - 1);
            int left = (width - textWidth) / 2;
            SolidBrush sb = new SolidBrush(ForeColor);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            //绘制文字
            for (int i = 0; i < RawData.Length; i++)
            {
                Rectangle rect = new Rectangle(left, top, fontWidth, fontHeight);
                g.DrawString(RawData[i].ToString(), font, sb, rect, sf);
                left += fontWidth + FontSpace;
            }
        }

        private Bitmap Draw_EAN13()
        {
            //绘制背景
            Bitmap bmp = GetBlankBitmap();
            Graphics g = Graphics.FromImage(bmp);
            //绘制条码
            Pen bp = new Pen(ForeColor, ModuleWidth);
            for (int i = 0; i < EncodeValue.Length; i++)
            {
                int h = Height;
                if (i <= 3 || (i >= 46 && i <= 48) || i >= 92)
                {
                    h = (int)(Height + LabelFontSize / 2);
                }
                int x = i * ModuleWidth + SpaceWidth * ModuleWidth;
                if (EncodeValue[i] == '1')
                {
                    g.DrawLine(bp, x, 5, x, h + 5);
                }
            }

            //绘制文字            
            SolidBrush sb = new SolidBrush(ForeColor);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            int top = Height + 5;
            //首先绘制前置码
            int spaceWidth = SpaceWidth * ModuleWidth; //左右空白区宽度
            Rectangle rect = new Rectangle(0, top, spaceWidth, fontHeight + 5);
            g.DrawString(RawData[0].ToString(), font, sb, rect, sf);
            //绘制左侧数据符
            for (int i = 1; i <= 6; i++)
            {
                rect = new Rectangle(spaceWidth + ((i - 1) * 7 + 3) * ModuleWidth,
                    top, 7 * ModuleWidth, fontHeight + 5);
                g.DrawString(RawData[i].ToString(), font, sb, rect, sf);
            }
            //绘制右侧数据符
            for (int i = 7; i <= 12; i++)
            {
                rect = new Rectangle(spaceWidth + ((i - 1) * 7 + 7) * ModuleWidth,
                    top, 7 * ModuleWidth, fontHeight + 5);
                g.DrawString(RawData[i].ToString(), font, sb, rect, sf);
            }
            return bmp;
        }

        private Bitmap Draw_EAN8()
        {
            Bitmap bmp = GetBlankBitmap();
            Graphics g = Graphics.FromImage(bmp);
            //绘制条码
            Pen bp = new Pen(ForeColor, ModuleWidth);
            for (int i = 0; i < EncodeValue.Length; i++)
            {
                int h = Height;
                if (i <= 3 || (i >= 31 && i <= 35) || i >= 64)
                {
                    h = (int)(Height + LabelFontSize / 2 + 3);
                }
                int x = i * ModuleWidth + SpaceWidth * ModuleWidth;
                if (EncodeValue[i] == '1')
                {
                    g.DrawLine(bp, x, 5, x, h + 5);
                }
            }

            //绘制文字            
            SolidBrush sb = new SolidBrush(ForeColor);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            int top = Height + 5;
            int spaceWidth = SpaceWidth * ModuleWidth; //左右空白区宽度
            Rectangle rect;
            //绘制左侧数据符
            for (int i = 0; i <= 3; i++)
            {
                rect = new Rectangle(spaceWidth + (i * 7 + 3) * ModuleWidth,
                    top, 7 * ModuleWidth, fontHeight + 5);
                g.DrawString(RawData[i].ToString(), font, sb, rect, sf);
            }
            //绘制右侧数据符
            for (int i = 4; i <= 7; i++)
            {
                rect = new Rectangle(spaceWidth + (i * 7 + 7) * ModuleWidth,
                    top, 7 * ModuleWidth, fontHeight + 5);
                g.DrawString(RawData[i].ToString(), font, sb, rect, sf);
            }
            return bmp;
        }

        private Bitmap Draw_UPCA()
        {
            //绘制背景
            Bitmap bmp = GetBlankBitmap();
            Graphics g = Graphics.FromImage(bmp);
            //绘制条码
            Pen bp = new Pen(ForeColor, ModuleWidth);
            for (int i = 0; i < EncodeValue.Length; i++)
            {
                int h = Height;
                if (i <= 10 || (i >= 46 && i <= 48) || i >= 84)
                {
                    h = (int)(Height + LabelFontSize / 2);
                }
                int x = i * ModuleWidth + SpaceWidth * ModuleWidth;
                if (EncodeValue[i] == '1')
                {
                    g.DrawLine(bp, x, 5, x, h + 5);
                }
            }

            //绘制文字            
            SolidBrush sb = new SolidBrush(ForeColor);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            int top = Height + 5;
            int sideTop = top + (int)(LabelFontSize * 0.2);
            Font sideFont = new Font(LabelFontStyle, (int)(LabelFontSize / 1.2)); //两侧小字体
            //首先绘制最左边编码
            int spaceWidth = SpaceWidth * ModuleWidth; //左右空白区宽度
            Rectangle rect = new Rectangle(0, sideTop, spaceWidth, fontHeight + 5);
            g.DrawString(RawData[0].ToString(), sideFont, sb, rect, sf);
            //绘制左侧数据符
            for (int i = 1; i <= 5; i++)
            {
                rect = new Rectangle(spaceWidth + ((i - 1) * 7 + 10) * ModuleWidth,
                    top, 7 * ModuleWidth, fontHeight + 5);
                g.DrawString(RawData[i].ToString(), font, sb, rect, sf);
            }
            //绘制右侧数据符
            for (int i = 6; i <= 10; i++)
            {
                rect = new Rectangle(spaceWidth + ((i - 1) * 7 + 13) * ModuleWidth,
                    top, 7 * ModuleWidth, fontHeight + 5);
                g.DrawString(RawData[i].ToString(), font, sb, rect, sf);
            }
            //绘制最右侧校验字符
            rect = new Rectangle(spaceWidth + 95 * ModuleWidth,
                    sideTop, spaceWidth, fontHeight + 5);
            g.DrawString(RawData[11].ToString(), sideFont, sb, rect, sf);

            return bmp;
        }

        private Bitmap Draw_UPCE()
        {
            Bitmap bmp = GetBlankBitmap();
            Graphics g = Graphics.FromImage(bmp);
            //绘制条码
            Pen bp = new Pen(ForeColor, ModuleWidth);
            for (int i = 0; i < EncodeValue.Length; i++)
            {
                int h = Height;
                if (i <= 2 || i >= 45)
                {
                    h = (int)(Height + LabelFontSize / 2);
                }
                int x = i * ModuleWidth + SpaceWidth * ModuleWidth;
                if (EncodeValue[i] == '1')
                {
                    g.DrawLine(bp, x, 5, x, h + 5);
                }
            }

            //绘制文字            
            SolidBrush sb = new SolidBrush(ForeColor);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            int top = Height + 5;
            int sideTop = top + (int)(LabelFontSize * 0.2);
            Font sideFont = new Font(LabelFontStyle, (int)(LabelFontSize / 1.2)); //两侧小字体
            //首先绘制最左边编码
            int spaceWidth = SpaceWidth * ModuleWidth; //左右空白区宽度
            Rectangle rect = new Rectangle(0, sideTop, spaceWidth, fontHeight + 5);
            g.DrawString(RawData[0].ToString(), sideFont, sb, rect, sf);
            //绘制数据符
            for (int i = 1; i <= 6; i++)
            {
                rect = new Rectangle(spaceWidth + ((i - 1) * 7 + 3) * ModuleWidth,
                    top, 7 * ModuleWidth, fontHeight + 5);
                g.DrawString(RawData[i].ToString(), font, sb, rect, sf);
            }
            //绘制最右侧校验字符
            rect = new Rectangle(spaceWidth + 51 * ModuleWidth,
                    sideTop, spaceWidth, fontHeight + 5);
            g.DrawString(RawData[7].ToString(), sideFont, sb, rect, sf);
            return bmp;
        }

        /// <summary>
        /// 计算绘制条码所需空间尺寸，并生成Bitmap位图，填充背景色后返回。
        /// 交叉25和ITF-14码不使用此方法
        /// </summary>
        /// <returns>返回绘制条码所需的位图</returns>
        private Bitmap GetBlankBitmap()
        {
            int height = Height + 5 + fontHeight;//计算背景图片的高度
            int width = (SpaceWidth * 2 + EncodeValue.Length) * ModuleWidth;//计算背景图片的宽度
            Bitmap bmp = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(bmp);
            SolidBrush sb = new SolidBrush(BackColor);
            g.FillRectangle(sb, 0, 0, width, height);
            return bmp;
        }

        private SizeF MeasureFontSize(string str)
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            SizeF size = g.MeasureString(str, font);
            size.Width = size.Width / str.Length;
            return size;
        }

        private void SetFontSize(string str)
        {
            SizeF s = MeasureFontSize(str);
            fontWidth = (int)s.Width; //(int)s.Width;
            fontHeight = (int)s.Height;
        }
        #endregion
    }
}
