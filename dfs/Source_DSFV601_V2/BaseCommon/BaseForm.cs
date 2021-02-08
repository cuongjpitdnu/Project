using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseCommon
{
    public class BaseForm : Form
    {
        // Binding Combo
        protected const int SELECT_ALL = -1;
        protected const string SELECT_ALL_SHOW = "All";
        protected const string DISPLAY = "Display";
        protected const string VALUE = "Value";

        public BaseForm()
        {
            System.ComponentModel.ComponentResourceManager resources =
            new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            this.Icon = Properties.Resources.appicon;
            this.DoubleBuffered = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 67);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BaseForm";
            this.ResumeLayout(false);

        }
        
        #region Event Form

        protected void onlyInputNumber(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        protected void DGVDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            ClearSelectionDgv((DataGridView)sender);
        }

        #endregion Event Form

        #region Bindind Data

        protected bool BindingDataTableToComboBox(ComboBox cbo, DataTable tbl, string strDisplay, string strValue, object selectValue = null)
        {

            try
            {
                cbo.DisplayMember = strDisplay;
                cbo.ValueMember = strValue;
                cbo.DataSource = tbl;

                if (selectValue != null && cbo.Items.Count > 0)
                {
                    cbo.SelectedValue = selectValue;
                }

                return true;
            }
            catch
            {

            }

            return false;
        }

        protected void BindingCboTime(ComboBox cbo, int max)
        {
            var tblBinding = new DataTable();
            tblBinding.Columns.Add(DISPLAY, typeof(string));
            tblBinding.Columns.Add(VALUE, typeof(int));

            for (var i = 0; i < max; i++)
            {
                tblBinding.Rows.Add(new object[] { i.ToString().PadLeft(2, '0'), i });
            }

            BindingDataTableToComboBox(cbo, tblBinding, DISPLAY, VALUE, SELECT_ALL);
        }

        #endregion Bindind Data

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

        #endregion Dialog

        #region Protected Function

        protected void ClearSelectionDgv(DataGridView dgv)
        {
            dgv.ClearSelection();
            dgv.CurrentCell = null;
        }

        #endregion Protected Function

        #region delegate

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        protected delegate void SetTextCallback(string text, TextBox txtValue);
        protected void SetValueText(string text, TextBox txtValue)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (txtValue.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetValueText);
                this.Invoke(d, new object[] { text, txtValue });
            }
            else
            {
                txtValue.Text = text;
            }
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a Label control.
        protected delegate void SetLabelCallback(string text, Label lblValue);
        protected void SetValueLabel(string text, Label lblValue)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lblValue.InvokeRequired)
            {
                SetLabelCallback d = new SetLabelCallback(SetValueLabel);
                this.Invoke(d, new object[] { text, lblValue });
            }
            else
            {
                lblValue.Text = text;
            }
        }

        // This delegate enables asynchronous calls for setting
        protected delegate void SetGridCallback(object[] row, DataGridView grv);
        protected void SetGridValue(object[] row, DataGridView grv)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (grv.InvokeRequired)
            {
                SetGridCallback d = new SetGridCallback(SetGridValue);
                this.Invoke(d, new object[] { row, grv });
            }
            else
            {
                grv.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(grv)).BeginInit();
                grv.Rows.Add(row);
                ((System.ComponentModel.ISupportInitialize)(grv)).EndInit();
                grv.ResumeLayout(false);
                grv.PerformLayout();
            }
        }

        #endregion delegate
    }
}
