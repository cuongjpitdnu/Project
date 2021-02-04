using System;
using System.Windows.Forms;

namespace GPMain.Common.Dialog
{
    public partial class WaitForm : BaseUserControl
    {
        delegate void UpdateValue(int intProgress, int count = 0, int total = 0);
        delegate void CloseForm();

        public ProgressBarCommon ProgressBar => progressBarCommon;

        public string Title
        {
            get
            {
                return txtTitle.Text;
            }
            set
            {
                txtTitle.Text = value.Trim();
                Application.DoEvents();
            }
        }

        public int Maximum
        {
            get
            {
                return progressBarCommon.Maximum;
            }
            set
            {
                progressBarCommon.Maximum = value;
                Application.DoEvents();
            }
        }

        public WaitForm()
        {
            InitializeComponent();
            progressBarCommon.Minimum = 0;
            progressBarCommon.Maximum = 100;
        }

        public void fncUpdateProgressBar(int intProgress, int count = 0, int total = 0)
        {
            try
            {
                if (progressBarCommon.InvokeRequired)
                {
                    progressBarCommon.Invoke(new UpdateValue(fncUpdateProgressBar), new object[] { intProgress, count, total });
                }
                else
                {
                    txtTitle.Text = Title;
                    Application.DoEvents();
                    progressBarCommon.Value = intProgress;
                    Application.DoEvents();
                    lblcount.Text = count.ToString();
                    Application.DoEvents();
                    lblcount.Location = new System.Drawing.Point(lbledge.Location.X - lblcount.Width, lblcount.Location.Y);
                    Application.DoEvents();
                    lbltotal.Text = total.ToString();
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(WaitForm), ex);
            }
        }

        private void lblcount_Click(object sender, EventArgs e)
        {

        }

        private void WaitForm_Load(object sender, EventArgs e)
        {

        }
    }
}