using BaseCommon.Core;
using DSF602;
using DSF602.Language;
using DSF602.View;
using System;
using System.IO;
using System.Windows.Forms;

namespace BaseCommon
{
    public partial class LicenseForm : BaseForm
    {
        private MainForm _MainForm;

        public LicenseForm(MainForm mainForm)
        {
            this._MainForm = mainForm;
            InitializeComponent();

            txtlMachineCode.Text = string.Join(clsConst.KEY_CHAR_JOIN, clsCommon.GetMachineCode());
            txtKey.Focus();
        }

        private void BtnLoadFile_Click(object sender, EventArgs e)
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
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                txtKey.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtKey.Text))
            {
                ShowMsg(MessageBoxIcon.Warning, LanguageHelper.GetValueOf("MSG_KEY_BLANLK"));
                txtKey.Focus();
                return;
            }

            if (!clsCommon.ValidateMachineAndDevices(txtKey.Text))
            {
                ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_ACTIVATE_ERR"));
                txtKey.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
            _MainForm?.Show();
        }


        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(this, "LICENSE_TITLE");
            LanguageHelper.SetValueOf(lblContent1, "LICENSE_LBL_CONTENT1");
            LanguageHelper.SetValueOf(lblContent2, "LICENSE_LBL_CONTENT2");
            LanguageHelper.SetValueOf(lblMachineCode, "LICENSE_LBL_MACHINECODE");
            LanguageHelper.SetValueOf(lblKey, "LICENSE_LBL_KEY");
            LanguageHelper.SetValueOf(btnLoadFile, "LICENSE_BTN_LOAD_FILE");
            LanguageHelper.SetValueOf(btnActivate, "LICENSE_BTN_ACTIVE");

            this.Refresh();
        }
    }
}
