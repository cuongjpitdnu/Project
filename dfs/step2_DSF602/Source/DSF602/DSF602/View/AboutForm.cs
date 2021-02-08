using BaseCommon.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using DSF602.Language;

namespace DSF602.View
{
    public partial class AboutForm : BaseForm
    {
        public AboutForm()
        {
            InitializeComponent();
            this.Text = LanguageHelper.GetValueOf("ABOUT");
            tabSoft.Text = LanguageHelper.GetValueOf("SOFTWARE");
            tabCus.Text = LanguageHelper.GetValueOf("CUSTOMER");
            btnResetKey.Text = LanguageHelper.GetValueOf("ABOUT_RESETKEY");
            btnResetApp.Text = LanguageHelper.GetValueOf("ABOUT_RESETAPP");
            lbAlert.Text = LanguageHelper.GetValueOf("ABOUT_RESET_ALERT");
        }

        private void btnResetKey_Click(object sender, EventArgs e)
        {
            var rs = ComfirmMsg("Do you want to import new key? If OK, you must be restart App!");

            if (rs)
            {
                try
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        InitialDirectory = Application.StartupPath,
                        Title = "Browse Text Files",
                        CheckFileExists = true,
                        CheckPathExists = true,
                        DefaultExt = "txt",
                        Multiselect = false,
                        Filter = "txt files (*.txt)|*.txt",
                        FilterIndex = 2,
                        RestoreDirectory = true,
                        ReadOnlyChecked = true,
                    })
                    {
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            var fileName = openFileDialog.FileName;
                            var keyEncripted = File.ReadAllText(fileName);
                            clsCommon.writeKeyToRegistry(keyEncripted);
                            ShowMsg(MessageBoxIcon.Information, "Import key success!");
                            this.ResultData = true;
                            this.Close();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void btnResetApp_Click(object sender, EventArgs e)
        {
            var rs = ComfirmMsg("Do you want to reset App? If OK, you must be restart App!");

            if (rs)
            {
                clsCommon.writeKeyToRegistry("");
                this.ResultData = true;
                this.Close();
            }
        }
    }
}
