using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseCommon.Core
{
    public partial class BaseUserControl : UserControl
    {
        public BaseUserControl()
        {
            InitializeComponent();
        }

        private void BaseUserControl_Load(object sender, EventArgs e)
        {
            SetLanguageControl();
        }

        protected virtual void SetLanguageControl()
        {
        }

        public void SetModeWaiting(bool isWaiting = true)
        {
            this.Cursor = isWaiting ? Cursors.WaitCursor : Cursors.Default;
            this.Enabled = !isWaiting;
            this.UseWaitCursor = isWaiting;
            Application.UseWaitCursor = isWaiting;
        }

        public bool ShowMsg(MessageBoxIcon type, string strMsg, string strTitle = "")
        {
            MessageBox.Show(strMsg, strTitle, MessageBoxButtons.OK, type);

            if (type == MessageBoxIcon.Error || type == MessageBoxIcon.Warning)
            {
                return false;
            }

            return true;
        }

    }
}
