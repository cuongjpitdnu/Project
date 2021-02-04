using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using MaterialSkin;
using MaterialSkin.Controls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GPMain.Common
{
    public class BaseForm : MaterialForm
    {
        [Browsable(true)]
        public string TitleBar { get; set; }

        private Color _TitleBarColor = Color.Transparent;

        [Browsable(true)]
        public Color ForeColorStatusBar
        {
            get => _TitleBarColor;
            set
            {
                _TitleBarColor = value;
                Refresh();
            }
        }

        [Browsable(true)]
        public string MsgComfirmExit { get; set; }

        public BaseForm FormParent { get; set; }
        public NavigationResult ResultData { get; set; }

        public bool ComfirmWhenExit { get; set; } = true;

        public MaterialSkinManager materialSkinManager;

        public BaseForm()
        {
            if (materialSkinManager == null)
                materialSkinManager = MaterialSkinManager.Instance;

            materialSkinManager.Theme = UIHelper.IsLightMode() ? MaterialSkinManager.Themes.LIGHT : MaterialSkinManager.Themes.DARK;

            if (materialSkinManager.Theme == MaterialSkinManager.Themes.LIGHT)
            {
                materialSkinManager.ColorScheme = new ColorScheme(
                    Color.FromArgb(13, 63, 103),
                    Color.FromArgb(13, 63, 103),
                    Color.FromArgb(13, 63, 103),
                    Color.FromArgb(104, 38, 54),
                    Color.White
                );
            }
            else
            {
                materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Indigo200, TextShade.WHITE);
            }

            TitleBar = string.IsNullOrWhiteSpace(TitleBar)
                       ? string.Format(AppConst.FormatFormTitleBase, AppManager.AppName, AppManager.AppVersion, AppManager.AppCreateDate.ToString())
                       : TitleBar.Trim();

            this.Load += (sender, e) => AppManager.MinimizeFootprint();

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

            this.TitleBar = !string.IsNullOrEmpty(userControl.TitleBar) ? $"{AppConst.TitleBarFisrt}{userControl.TitleBar}" : this.TitleBar;
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
                TitleBar, this.Font, ForeColorStatusBar == Color.Transparent ? SkinManager.ColorScheme.TextBrush : new SolidBrush(ForeColorStatusBar),
                new Rectangle(3, 0, Width, 24), new StringFormat { LineAlignment = StringAlignment.Center }
                         );
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Name = "BaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }
    }
}
