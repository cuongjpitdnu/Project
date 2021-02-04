using System;
using System.Windows.Forms;
using GP40Main.Core;
using GP40Main.Themes.Controls;

namespace GP40Main.Services.Dialog
{
    public partial class WaitForm : BaseUserControl
    {
        delegate void UpdateValue(int intProgress);
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

        public void fncUpdateProgressBar(int intProgress)
        {
            try
            {
                if (progressBarCommon.InvokeRequired)
                {
                    progressBarCommon.Invoke(new UpdateValue(fncUpdateProgressBar), new object[] { intProgress });
                } else
                {
                    Application.DoEvents();
                    progressBarCommon.Value = intProgress;
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(WaitForm), ex);
            }
        }
    }
}
