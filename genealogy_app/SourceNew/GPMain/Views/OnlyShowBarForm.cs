using GPMain.Common;
using System.Drawing;
using System.Windows.Forms;

namespace GPMain.Views
{
    public partial class OnlyShowBarForm : BaseForm
    {
        public OnlyShowBarForm() : base()
        {
            InitializeComponent();

            //this.SetDefaultUI();
            this.Padding = new Padding(this.Padding.Left, STATUS_BAR_HEIGHT, this.Padding.Right, this.Padding.Bottom);
        }

        public override void AddPage(BaseUserControl userControl)
        {
            base.AddPage(userControl);
           
            if (userControl == null)
            {
                return;
            }
            this.Text = userControl.TitleBar;
            this.SuspendLayout();
            this.Size = new Size(userControl.Size.Width, userControl.Height + STATUS_BAR_HEIGHT);
            this.Controls.Add(userControl);
            userControl.Dock = DockStyle.Fill;
            this.PerformLayout();
            this.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            // Draw Border
            using (var borderPen = new Pen(Color.FromArgb(219, 228, 236), 1))
            {
                g.DrawLine(borderPen, new Point(0, STATUS_BAR_HEIGHT), new Point(0, Height - 2));
                g.DrawLine(borderPen, new Point(Width - 1, STATUS_BAR_HEIGHT), new Point(Width - 1, Height - 2));
                g.DrawLine(borderPen, new Point(0, Height - 1), new Point(Width - 1, Height - 1));
            }
        }
    }
}