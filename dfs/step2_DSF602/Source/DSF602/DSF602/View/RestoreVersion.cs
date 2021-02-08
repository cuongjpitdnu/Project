using BaseCommon;
using BaseCommon.Core;
using DSF602.Language;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DSF602.View.UpdateVersion;
using static DSF602.View.MainForm;
using DSF602.Model;

namespace DSF602.View
{
    public partial class RestoreVersion : BaseForm
    {
        public string backupPath = Application.StartupPath + @"\Backup"; 
        public const string FILE_SETUP_UPDATE = "ToolUpdate.exe";

        public RestoreVersion()
        {
            InitializeComponent();
            lblHeader.Text = LanguageHelper.GetValueOf("RESTORE_LBL_HEADER");
            InitForm();
        }

        private void InitForm()
        {
            if (!Directory.Exists(backupPath))
            {
                return;
            }
            // Get all subdirectories
            string[] subdirectoryEntries = Directory.GetDirectories(backupPath);
            foreach (var subdirectory in subdirectoryEntries)
            {
                var dt = Directory.GetCreationTime(subdirectory).ToString(clsConst.cstrDateFomatShow1);

                lstbVersion.Items.Add(Path.GetFileName(subdirectory) + "\t" + dt);
            }

            lstbVersion.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (lstbVersion.GetItemText(lstbVersion.SelectedItem) == "")
            {
                return;
            }
            
        
            try
            {
                string versionNameFolder = lstbVersion.GetItemText(lstbVersion.SelectedItem);
                var index = versionNameFolder.IndexOf("\t");
                string versionName = versionNameFolder.Substring(0, index);

                var dataMsg = new MsgData
                {
                    KeyCheck = versionName,
                    Title = LanguageHelper.GetValueOf("UPDATE_TITLE_SENDER"),
                    Message = LanguageHelper.GetValueOf("UPDATE_MESSAGE_SENDER"),
                };

                var parentForm = this.FormParent as UpdateVersion;
                var mainForm = parentForm.FormParent as MainForm;
                mainForm.IsStop = true;
                this.Hide();
                parentForm.Hide();
                mainForm.MainForm_FormClosed(dataMsg, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
            }
        }

        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(this, "RESTOREVERSION_TITLE");
            LanguageHelper.SetValueOf(btnCancel, "RESTORE_BTN_CLOSE");
            LanguageHelper.SetValueOf(btnRestore, "RESTORE_BTN_AGREE");
        }
    }
}
