using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CgBarcodeLib;

namespace CgBarcode
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Barcode barcode;
        TYPE encodedType = TYPE.EAN13;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Drawing.Text.InstalledFontCollection font = new System.Drawing.Text.InstalledFontCollection();
            System.Drawing.FontFamily[] sysFonts = font.Families;
            foreach (var f in sysFonts)
            {
                cbFontStyle.Items.Add(f.Name);
            }
            cbFontStyle.Text = "Microsoft Sans Serif";
        }

        //条码类型组合框选中项目改变时
        private void CbBarcodeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (e.AddedItems[0] as ComboBoxItem).Content as string;
            UpdateUI(text);
            if (tbContent != null)
            {
                tbContent.Text = "";
            }
            switch (text)
            {
                case "EAN-13":
                    encodedType = TYPE.EAN13;
                    this.Resources["TipString"] = "请输入 12 或 13 位数字";
                    break;
                case "EAN-8":
                    encodedType = TYPE.EAN8;
                    this.Resources["TipString"] = "请输入 7 或 8 位数字";
                    break;
                case "UPC-A":
                    encodedType = TYPE.UPCA;
                    this.Resources["TipString"] = "请输入 11 或 12 位数字";
                    break;
                case "UPC-E":
                    encodedType = TYPE.UPCE;
                    this.Resources["TipString"] = "请输入 7、8 位数字的 UPC-E 码或 11 或 12 位数字的 UPC-A 码";
                    break;
                case "交叉25码":
                    encodedType = TYPE.Interleaved2of5;
                    this.Resources["TipString"] = "请输入数字";
                    break;
                case "ITF-14":
                    encodedType = TYPE.ITF14;
                    this.Resources["TipString"] = "请输入 13或 14 位数字";
                    break;
                case "Code 128 Auto":
                    encodedType = TYPE.CODE128;
                    this.Resources["TipString"] = "可输入大小写英文字母、数字、标点符号及控制字符";
                    break;
                case "Code 128 A":
                    encodedType = TYPE.CODE128A;
                    this.Resources["TipString"] = "可输入大写英文字母、数字、标点符号及控制字符";
                    break;
                case "Code 128 B":
                    encodedType = TYPE.CODE128B;
                    this.Resources["TipString"] = "可输入大小写英文字母、数字、标点符号";
                    break;
                case "Code 128 C":
                    encodedType = TYPE.CODE128C;
                    this.Resources["TipString"] = "可输入数字";
                    break;
                case "GS1-128":
                    encodedType = TYPE.GS1_128;
                    this.Resources["TipString"] = "AI请使用()括起";
                    break;
            }
        }

        //用于在条码类型变化时，增加和减少UI控件
        private void UpdateUI(string text)
        {
            switch (text)
            {
                case "EAN-13":
                case "EAN-8":
                case "UPC-A":
                case "UPC-E":
                    spTextSpace.Visibility = Visibility.Collapsed;
                    tbITF.Visibility = Visibility.Collapsed;
                    spITF.Visibility = Visibility.Collapsed;
                    break;
                case "ITF-14":
                    spTextSpace.Visibility = Visibility.Visible;
                    tbITF.Visibility = Visibility.Visible;
                    spITF.Visibility = Visibility.Visible;
                    break;
                case "交叉25码":
                case "Code 128 Auto":
                case "Code 128 A":
                case "Code 128 B":
                case "Code 128 C":
                case "GS1-128":
                    spTextSpace.Visibility = Visibility.Visible;
                    tbITF.Visibility = Visibility.Collapsed;
                    spITF.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void CbFontStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (barcode != null)
            {
                barcode.LabelFontStyle = cbFontStyle.Text;
                PaintBarCode();
            }
        }

        public BitmapImage BitmapToBitmapImage(Bitmap bmp)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }

        private void BtnDraw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                barcode = new Barcode(tbContent.Text, encodedType);
                barcode.ModuleWidth = int.Parse(tbModuleWidth.Text);
                barcode.Height = int.Parse(tbHeight.Text);
                barcode.LabelFontSize = int.Parse(tbFontSize.Text);
                barcode.FontSpace = int.Parse(tbFontSpace.Text);
                PaintBarCode();
            }
            catch (Exception ex)
            {
                tbMessage.Text = ex.Message;
                return;
            }
            tbMessage.Clear();
            switch (encodedType)
            {
                case TYPE.EAN13:
                case TYPE.EAN8:
                    if (barcode.Country != null)
                    {
                        tbMessage.AppendText("编码所属国家：" + barcode.Country);
                    }
                    break;
            }
            foreach (var s in barcode.Errors)
            {
                tbMessage.AppendText(s + "\r\n");
            }
        }

        private void PaintBarCode()
        {
            Bitmap bmp = barcode.GetBitmap();
            imgContainer.Source = BitmapToBitmapImage(bmp);
        }

        #region 数字调节按钮事件方法
        private void BtnFontSizeAdd_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(tbFontSize.Text) + 1;
            if (i <= 72)
            {
                tbFontSize.Text = i.ToString();
            }
            if (barcode != null)
            {
                barcode.LabelFontSize = i;
                PaintBarCode();
            }
        }

        private void BtnFontSizeSub_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(tbFontSize.Text) - 1;
            if (i >= 8)
            {
                tbFontSize.Text = i.ToString();
            }
            if (barcode != null)
            {
                barcode.LabelFontSize = i;
                PaintBarCode();
            }
        }

        private void BtnFontSpaceAdd_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(tbFontSpace.Text) + 1;
            tbFontSpace.Text = i.ToString();
            if (barcode != null)
            {
                barcode.FontSpace = i;
                PaintBarCode();
            }
        }

        private void BtnFontSpaceSub_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(tbFontSpace.Text) - 1;
            if (i >= 0)
            {
                tbFontSpace.Text = i.ToString();
            }
            if (barcode != null)
            {
                barcode.FontSpace = i;
                PaintBarCode();
            }
        }

        private void BtnWidthAdd_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(tbModuleWidth.Text) + 1;
            tbModuleWidth.Text = i.ToString();
            if (barcode != null)
            {
                barcode.ModuleWidth = i;
                PaintBarCode();
            }
        }

        private void BtnWidthSub_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(tbModuleWidth.Text) - 1;
            if (i >= 1)
            {
                tbModuleWidth.Text = i.ToString();
            }
            if (barcode != null)
            {
                barcode.ModuleWidth = i;
                PaintBarCode();
            }
        }

        #endregion
    }
}
