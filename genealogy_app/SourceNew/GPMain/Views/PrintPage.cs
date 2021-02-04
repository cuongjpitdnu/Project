using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Windows.Data;
using Xceed.Document.NET;
using SkiaSharp;
using Font = System.Drawing.Font;
using GPMain.Common;
using GP40DrawTree;
using GPModels;
using GPMain.Views.Tree;
using GP40Common;
using System.Runtime.InteropServices;
using GPMain.Core;
using static GP40Common.clsConst;

namespace GPMain.Views
{
    public partial class PrintPage : Form
    {
        bool _TreeMode;
        public PrintPage(bool treeMode)
        {
            InitializeComponent();
            _TreeMode = treeMode;
        }

        DrawSimpleTree drawSimpleTree;
        DrawTreeExpand drawExpandTree;
        int ppi = 300;

        int Cvt_MM_To_Pixel(float mSize)
        {
            var numPixel = mSize * (ppi / 25.4);
            return (int)Math.Ceiling(numPixel);
        }

        private void cbzoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void PrintPage_Load(object sender, EventArgs e)
        {
            PrinterSettings printerSettings = new PrinterSettings();
            var listPrinter = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            cbprint.Items.AddRange(listPrinter.ToArray());
            listPrinter.ForEach(x =>
            {
                printerSettings.PrinterName = x;
                cbprint.SelectedIndex = printerSettings.IsDefaultPrinter ? listPrinter.IndexOf(x) : cbprint.SelectedIndex;
            });
            cbsizePage.SelectedIndex = 0;
            var pageInfo = typeof(GPConst.PageSize).GetFields().Select(x => (GPConst.PageInfo)x.GetValue(x.Name)).ToArray()[cbsizePage.SelectedIndex];
            var themeDB = AppManager.DBManager.GetTable<ThemeConfig>().FirstOrDefault() ?? new ThemeConfig();
            var configTree = themeDB.ToConfig();
            int height = Cvt_MM_To_Pixel(pageInfo.Height);
            int width = Cvt_MM_To_Pixel(pageInfo.Width);
            clsConst.ENUM_MEMBER_TEMPLATE emTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
            var rootMember = RootMember;
            drawSimpleTree = new DrawSimpleTree(configTree);
            drawSimpleTree.Config.DataMember = rootMember.CreateDataTree(emTemplate, drawSimpleTree.Config.MaxLevelInFamily);
            drawSimpleTree.DrawToPrint = true;
            drawSimpleTree.MaxHeight = height;
            drawSimpleTree.MaxWidth = width;
            drawSimpleTree.Draw(rootMember.Id, emTemplate);
            drawSimpleTree.Tree.Dock = DockStyle.Fill;

            drawExpandTree = new DrawTreeExpand(configTree);
            drawExpandTree.Config.DataMember = rootMember.CreateDataTree(emTemplate, drawExpandTree.Config.MaxLevelInFamily);
            drawExpandTree.DrawToPrint = true;
            drawExpandTree.MaxHeight = height;
            drawExpandTree.MaxWidth = width;
            drawExpandTree.Draw(rootMember.Id, emTemplate);
            drawExpandTree.Tree.Dock = DockStyle.Fill;

            if (_TreeMode)
            {
                panelDocument.Controls.Add(drawExpandTree.Tree);
            }
            else
            {
                panelDocument.Controls.Add(drawSimpleTree.Tree);
            }
        }

        private TMember RootMember
        {
            get
            {
                var objFamily = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                var objRootData = !string.IsNullOrEmpty(objFamily.RootId) ? AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == objFamily.RootId) : null;
                if (objRootData == null)
                {
                    objRootData = new TMember();
                }
                return objRootData;
            }
        }

        private void PrintDocument_Event(object sender, PrintPageEventArgs e)
        {

        }

        private void cbzoom_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbsizePage_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (_TreeMode)
            {
                if (drawExpandTree != null)
                {
                    int.TryParse(cbdpi.Text, out ppi);
                    clsConst.ENUM_MEMBER_TEMPLATE emTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
                    var rootMember = RootMember;
                    var pageInfo = typeof(GPConst.PageSize).GetFields().Select(x => (GPConst.PageInfo)x.GetValue(x.Name)).ToArray()[cbsizePage.SelectedIndex];
                    int height = Cvt_MM_To_Pixel(pageInfo.Height);
                    int width = Cvt_MM_To_Pixel(pageInfo.Width);
                    drawExpandTree.MaxHeight = height;
                    drawExpandTree.MaxWidth = width;
                    drawExpandTree.Config.DataMember = rootMember.CreateDataTree(emTemplate, drawExpandTree.Config.MaxLevelInFamily);
                    drawExpandTree.DrawFirst = true;
                    drawExpandTree.Draw(rootMember.Id, emTemplate);
                }
            }
            else if (drawSimpleTree != null)
            {
                int.TryParse(cbdpi.Text, out ppi);
                clsConst.ENUM_MEMBER_TEMPLATE emTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
                var rootMember = RootMember;
                var pageInfo = typeof(GPConst.PageSize).GetFields().Select(x => (GPConst.PageInfo)x.GetValue(x.Name)).ToArray()[cbsizePage.SelectedIndex];
                int height = Cvt_MM_To_Pixel(pageInfo.Height);
                int width = Cvt_MM_To_Pixel(pageInfo.Width);
                drawSimpleTree.MaxHeight = height;
                drawSimpleTree.MaxWidth = width;
                drawSimpleTree.Config.DataMember = rootMember.CreateDataTree(emTemplate, drawSimpleTree.Config.MaxLevelInFamily);
                drawSimpleTree.DrawFirst = true;
                drawSimpleTree.Draw(rootMember.Id, emTemplate);
            }
        }
        void DrawBitmapToCanvas(SKCanvas canvas, Bitmap bitmapInput, int width, int height)
        {
            using (SKBitmap sKBitmap = SKBitmap.Decode("frame.jpg").Resize(new SKSizeI(width, height), SKFilterQuality.High))
            {
                canvas.DrawBitmap(sKBitmap, 0, 0);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var pageInfo = typeof(GPConst.PageSize).GetFields().Select(x => (GPConst.PageInfo)x.GetValue(x.Name)).ToArray()[cbsizePage.SelectedIndex];
            int.TryParse(cbdpi.Text, out ppi);
            try
            {
                SKRect bounds = new SKRect(0, 0, Cvt_MM_To_Pixel(pageInfo.Width), Cvt_MM_To_Pixel(pageInfo.Height));
                SKBitmap sKBitmap = new SKBitmap((int)bounds.Right, (int)bounds.Height);
                using (SKCanvas bitmapCanvas = new SKCanvas(sKBitmap))
                {
                    if (_TreeMode)
                    {
                        if (drawExpandTree != null)
                        {
                            drawExpandTree.DrawTree(bitmapCanvas, false, null, true);
                        }
                    }
                    else if (drawSimpleTree != null)
                    {
                        drawSimpleTree.DrawTree(bitmapCanvas, false, null, true);
                    }
                }
                using (Bitmap bmp = ExportBitMap(sKBitmap))
                {
                    bmp.SetResolution(ppi, ppi);
                    bmp.Save("1.png");
                    Print(cbprint.Text, bmp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Cursor = Cursors.Default;
        }

        private Bitmap ExportBitMap(SKBitmap sKBitmap)
        {
            Bitmap bmp = new Bitmap(sKBitmap.Width, sKBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            var intp = bitmapData.Scan0;
            var dataBuffer = sKBitmap.Bytes;
            Marshal.Copy(dataBuffer, 0, intp, dataBuffer.Length);
            bmp.UnlockBits(bitmapData);
            return bmp;
        }

        private void Print(string printer, Bitmap image)
        {
            var printerSettings = new PrinterSettings
            {
                PrinterName = printer,
            };
            var pageSettings = new PageSettings(printerSettings);
            pageSettings.PaperSize = printerSettings.PaperSizes.Cast<PaperSize>().FirstOrDefault(v => v.PaperName == cbsizePage.Text);
            pageSettings.PrinterResolution = new PrinterResolution() { Kind = PrinterResolutionKind.Custom, X = ppi, Y = ppi };
            var printDocument = new PrintDocument()
            {
                PrinterSettings = printerSettings,
                DefaultPageSettings = pageSettings,
                PrintController = new StandardPrintController()
            };
            printDocument.PrintPage += (sender, e) =>
            {
                Graphics grp = e.Graphics;
                grp.DrawImage(image, 0, 0);
                image.Dispose();
            };
            printDocument.DocumentName = Guid.NewGuid().ToString();
            printDocument.Print();
        }
        private void panelDocument_SizeChanged(object sender, EventArgs e)
        {

        }

        private void panelDocument_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPrint_MouseHover(object sender, EventArgs e)
        {
            btnPrint.Cursor = Cursors.Hand;
        }

        private void btnPrint_MouseLeave(object sender, EventArgs e)
        {
            btnPrint.Cursor = Cursors.Default;
        }
    }
}
