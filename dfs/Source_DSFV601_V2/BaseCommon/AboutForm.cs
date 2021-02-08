using System;
using System.IO;
using System.Windows.Forms;

namespace BaseCommon
{
    public partial class AboutForm : BaseForm
    {

        public AboutForm()
        {
            InitializeComponent();
        }

        private void btnResetKey_Click(object sender, EventArgs e)
        {
            var rs = ComfirmMsg("Do you want to import new key? If yes, you must be restart App!");

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
                            this.DialogResult = DialogResult.OK;
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
            var rs = ComfirmMsg("Do you want to reset App? If yes, you must be restart App!");

            if (rs)
            {
                clsCommon.writeKeyToRegistry("");
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
