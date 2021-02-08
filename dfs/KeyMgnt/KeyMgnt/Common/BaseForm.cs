using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KeyMgnt.Common
{
    public class BaseForm : Form
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string CURRENT_USER;

        #region Binding Views

        public void BindingDataGridView<T>(DataGridView dgv, List<T> data)
        {
            dgv.DataSource = data;
        }

        #endregion

        #region Dialog

        public bool ShowMsg(MessageBoxIcon type, string strMsg, string strTitle = "")
        {
            MessageBox.Show(strMsg, strTitle, MessageBoxButtons.OK, type);

            if (type == MessageBoxIcon.Error || type == MessageBoxIcon.Warning)
            {
                return false;
            }

            return true;
        }

        public bool ComfirmMsg(string strMsg, string strTitle = "")
        {
            return MessageBox.Show(strMsg, strTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK;
        }

        public bool ComfirmMsgErr(string strMsg, string strTitle = "")
        {
            return MessageBox.Show(strMsg, strTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        public SaveFileDialog SaveExcelDialog(string defaultFileName = "")
        {
            return new SaveFileDialog
            {
                DefaultExt = "xlsx",
                Filter = "Excel Workbook (*.xls, *.xlsx)|*.xls;*.xlsx",
                AddExtension = true,
                RestoreDirectory = true,
                Title = "Save as",
                FileName = defaultFileName,
                InitialDirectory = @"D:\",
            };
        }

        #endregion

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            this.SuspendLayout();
            // 
            // BaseForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BaseForm";
            this.Load += new System.EventHandler(this.BaseForm_Load);
            this.ResumeLayout(false);

        }

        private void BaseForm_Load(object sender, System.EventArgs e)
        {
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;

            this.BackColor = Color.LightSteelBlue;
        }
    }
}
