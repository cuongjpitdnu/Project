using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeaDSF601
{
    public class BaseForm : Form
    {
        public BaseForm()
        {
            System.ComponentModel.ComponentResourceManager resources =
            new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            this.Icon = Properties.Resources.appicon3;
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

        protected void onlyInputNumber(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

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
