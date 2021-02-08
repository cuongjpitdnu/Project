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
    public partial class ErrorMessage : BaseForm
    {

        private List<string> _lstMsg = new List<string>();

        public ErrorMessage()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        public void AppendErrorMsg(string objStrBuilder)
        {
            if (!_lstMsg.Contains(objStrBuilder))
            {
                _lstMsg.Add(objStrBuilder);
                this.UpdateSafeControl(txtMsgError, (c) =>
                {
                    ((TextBox)c).AppendText(objStrBuilder + Environment.NewLine);
                });
            }
            //if (txtMsgError.InvokeRequired)
            //{
            //    txtMsgError.BeginInvoke(new Action(() =>
            //    {
            //        txtMsgError.AppendText(objStrBuilder + Environment.NewLine);
            //    }));
            //} else
            //{

            //}
            
        }

        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(this, "ERROR_MESSAGE_TITLE");
            LanguageHelper.SetValueOf(lblMsgError, "ERROR_MESSAGE_HEADER");
        }

        private void ErrorMessage_Load(object sender, EventArgs e)
        {
            var strErrorMsg = this.Params as StringBuilder;
            if (strErrorMsg == null)
            {
                return;
            }
            if (strErrorMsg.Length != 0)
            {
                AppendErrorMsg(strErrorMsg.ToString().Trim());
            }
            this.TopMost = true;
        }

        private void ErrorMessage_FormClosed(object sender, FormClosedEventArgs e)
        {
           //MainForm._frmMsgError = null;
        }
    }
}
