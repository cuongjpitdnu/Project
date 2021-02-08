using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BaseCommon.ControlTemplate;

namespace BaseCommon.Core
{
    public partial class BaseForm : Form
    {
        private List<btn> _listButton = new List<btn>();

        public object Params { get; set; }
        public BaseForm FormParent { get; set; }
        public object ResultData { get; set; }

        [Description("Esc To Close"), Category("Custom")]
        public bool EscToClose { get; set; }
        
        public BaseForm()
        {
            InitializeComponent();

            this.BackColor = Color.AliceBlue;
        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            GetAllButtonBaseControl(this);
            SetLanguageControl();
        }

        protected virtual void SetLanguageControl()
        {
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (this.EscToClose && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            if (_listButton.Count == 0)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            var objButton = _listButton.FirstOrDefault(i => i.Shortcut == keyData);

            if (objButton != null)
            {
                objButton.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GetAllButtonBaseControl(Control c)
        {
            foreach (Control control in c.Controls)
            {
                if (control.GetType() == typeof(btn))
                {
                    _listButton.Add((btn)control);
                }

                if (control.GetType() == typeof(Panel)
                    || control.GetType() == typeof(GroupBox)
                    || control.GetType() == typeof(TabControl)
                    || control.GetType() == typeof(SplitContainer)
                    || control.GetType() == typeof(FlowLayoutPanel)
                    || control.GetType() == typeof(TableLayoutPanel))
                {
                    GetAllButtonBaseControl(control);
                }
            }
        }

        public bool ComfirmMsg(string strMsg, string strTitle = "")
        {
            if (this.InvokeRequired)
            {
                return (bool)this.Invoke(
                  new Func<bool>(() => MessageBox.Show(this, strMsg, strTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                );
            } else
            {
                return MessageBox.Show(this, strMsg, strTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK;
            }
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

        public void UpdateSafeControl(Control control, Action<Control> action)
        {
            if (control == null || action == null)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke((MethodInvoker)(() => { action(control); }));
            }
            else
            {
                action(control);
            }
        }

        public void UpdateChildControl(Control control, string keyChildControl, Action<Control> action)
        {
            var objChildControl = control != null && control.Controls.ContainsKey(keyChildControl)
                                  ? control.Controls[keyChildControl] : null;
            this.UpdateSafeControl(objChildControl, action);
        }

        public void SetModeWaiting(bool isWaiting = true)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)(() => { this.SetModeWaiting(isWaiting); }));
            } else
            {
                this.Cursor = isWaiting ? Cursors.WaitCursor : Cursors.Default;
                this.Enabled = !isWaiting;
                this.UseWaitCursor = isWaiting;
                //Application.UseWaitCursor = isWaiting;
                if (!isWaiting)
                {
                    this.Focus();
                    this.BringToFront();
                    this.Activate();
                }
            }
        }
    }
}
