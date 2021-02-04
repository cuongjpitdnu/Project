using GP40DrawTree;
using GP40Main.Services.Navigation;
using MaterialSkin;
using MaterialSkin.Controls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GP40Main.Core
{
    public class BaseForm : MaterialForm
    {
        [Browsable(true)]
        public string TitleBar { get; set; }

        [Browsable(true)]
        public string MsgComfirmExit { get; set; }

        public BaseForm FormParent { get; set; }
        public NavigationResult ResultData { get; set; }

        public bool ComfirmWhenExit { get; set; } = true;

        protected readonly MaterialSkinManager materialSkinManager;

        public BaseForm()
        {
            materialSkinManager = MaterialSkinManager.Instance;
            //materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = AppManager.IsLightMode() ? MaterialSkinManager.Themes.LIGHT : MaterialSkinManager.Themes.DARK;
            //materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Indigo200, TextShade.WHITE);

            if (materialSkinManager.Theme == MaterialSkinManager.Themes.LIGHT)
            {
                //materialSkinManager.ColorScheme = new ColorScheme(Primary.Brown500, Primary.Brown500, Primary.Brown300, Accent.Blue400, TextShade.WHITE);
                materialSkinManager.ColorScheme = new ColorScheme(ColorDrawHelper.FromHtmlToColor("#0d3f67")
                    , ColorDrawHelper.FromHtmlToColor("#0d3f67"), ColorDrawHelper.FromHtmlToColor("#0d3f67"), Color.FromArgb(104, 38, 54), Color.White);
            }
            else
            {
                materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Indigo200, TextShade.WHITE);
            }

            TitleBar = string.IsNullOrWhiteSpace(TitleBar) ? string.Format(AppConst.FormatFormTitleBase, AppManager.AppName, AppManager.AppVersion) : TitleBar.Trim();

            this.Load += (sender, e) =>
            {
                Icon = Properties.Resources.icon_app;
            };

            this.FormClosing += (sender, e) =>
            {
                if (ComfirmWhenExit && !string.IsNullOrEmpty(MsgComfirmExit))
                {
                    e.Cancel = !AppManager.Dialog.Confirm(MsgComfirmExit);
                }
            };

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);
        }

        public virtual void AddPage(BaseUserControl userControl)
        {
            if (userControl == null)
            {
                return;
            }

            this.TitleBar = !string.IsNullOrEmpty(userControl.TitleBar) ? userControl.TitleBar : this.TitleBar;
            this.MinimizeBox = userControl.MinimizeBox;
            this.MaximizeBox = userControl.MaximizeBox;
            this.StartPosition = userControl.StartPosition;
            this.WindowState = userControl.WindowState;
            this.AcceptButton = userControl.AcceptButton;
            this.CancelButton = userControl.CancelButton;
            this.Sizable = userControl.Sizable;
            this.MsgComfirmExit = userControl.ComfirmMsgExit;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            // Draw Text Title Bar
            g.DrawString(
                TitleBar, this.Font, SkinManager.ColorScheme.TextBrush,
                new Rectangle(3, 0, Width, 24), new StringFormat { LineAlignment = StringAlignment.Center }
            );
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // BaseForm
            //
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Name = "BaseForm";
            this.ResumeLayout(false);

        }
    }
}
