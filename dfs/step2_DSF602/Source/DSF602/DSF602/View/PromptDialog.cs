using BaseCommon.Core;
using DSF602.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSF602.View
{
    public partial class PromptDialog : BaseForm
    {
        public PromptDialog()
        {
            InitializeComponent();
            this.AcceptButton = btnOK;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string title = Params as string;
            lbTitle.Text = title;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string txt = txtInputValue.Text.Trim();
            if (string.IsNullOrEmpty(txt))
            {
                lbAlert.Text = LanguageHelper.GetValueOf("MSG_ERR_PASS_BLANK");
                lbAlert.Visible = true;
                return;
            }

            if (!txt.Equals(AppManager.UserLogin.Password))
            {
                lbAlert.Text = LanguageHelper.GetValueOf("MSG_PASS_NOT_SAME");
                lbAlert.Visible = true;
                return;
            }
            this.ResultData = txtInputValue.Text;
            this.Close();
        }
    }
}
