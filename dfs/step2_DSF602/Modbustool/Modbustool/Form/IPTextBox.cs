// iptb.IPTextBox
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace iptb
{
    public class IPTextBox : UserControl
    {
        private Panel panel1;

        private TextBox Box1;

        private Label label1;

        private TextBox Box2;

        private TextBox Box3;

        private TextBox Box4;

        private Label label2;

        private Label label3;

        private ToolTip toolTip1;

        private IContainer components;

        public string ToolTipText
        {
            get
            {
                return toolTip1.GetToolTip(Box1);
            }
            set
            {
                toolTip1.SetToolTip(Box1, value);
                toolTip1.SetToolTip(Box2, value);
                toolTip1.SetToolTip(Box3, value);
                toolTip1.SetToolTip(Box4, value);
                toolTip1.SetToolTip(label1, value);
                toolTip1.SetToolTip(label2, value);
                toolTip1.SetToolTip(label3, value);
            }
        }

        public override string Text
        {
            get
            {
                return Box1.Text + "." + Box2.Text + "." + Box3.Text + "." + Box4.Text;
            }
            set
            {
                if (value != "" && value != null)
                {
                    string[] array = new string[4];
                    array = value.ToString().Split(".".ToCharArray(), 4);
                    Box1.Text = array[0];
                    Box2.Text = array[1];
                    Box3.Text = array[2];
                    Box4.Text = array[3];
                }
                else
                {
                    Box1.Text = "";
                    Box2.Text = "";
                    Box3.Text = "";
                    Box4.Text = "";
                }
            }
        }

        public bool IsValid()
        {
            try
            {
                int num = int.Parse(Box1.Text);
                if (num < 0 || num > 255)
                {
                    return false;
                }
                num = int.Parse(Box2.Text);
                if (num < 0 || num > 255)
                {
                    return false;
                }
                num = int.Parse(Box3.Text);
                if (num < 0 || num > 255)
                {
                    return false;
                }
                num = int.Parse(Box4.Text);
                if (num < 0 || num > 255)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IPTextBox()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panel1 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            Box4 = new System.Windows.Forms.TextBox();
            Box3 = new System.Windows.Forms.TextBox();
            Box2 = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            Box1 = new System.Windows.Forms.TextBox();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            panel1.SuspendLayout();
            SuspendLayout();
            panel1.BackColor = System.Drawing.SystemColors.Window;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel1.Controls.AddRange(new System.Windows.Forms.Control[7]
            {
            label3,
            label2,
            Box4,
            Box3,
            Box2,
            label1,
            Box1
            });
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(128, 18);
            panel1.TabIndex = 0;
            panel1.EnabledChanged += new System.EventHandler(panel1_EnabledChanged);
            label3.Location = new System.Drawing.Point(24, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(8, 13);
            label3.TabIndex = 6;
            label3.Text = ".";
            label3.EnabledChanged += new System.EventHandler(label_EnabledChanged);
            label2.Location = new System.Drawing.Point(88, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(8, 13);
            label2.TabIndex = 5;
            label2.Text = ".";
            label2.EnabledChanged += new System.EventHandler(label_EnabledChanged);
            Box4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Box4.Location = new System.Drawing.Point(100, 0);
            Box4.MaxLength = 3;
            Box4.Name = "Box4";
            Box4.Size = new System.Drawing.Size(20, 13);
            Box4.TabIndex = 4;
            Box4.TabStop = false;
            Box4.Text = "";
            Box4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Box4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Box4_KeyPress);
            Box4.Enter += new System.EventHandler(Box_Enter);
            Box3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Box3.Location = new System.Drawing.Point(64, 0);
            Box3.MaxLength = 3;
            Box3.Name = "Box3";
            Box3.Size = new System.Drawing.Size(20, 13);
            Box3.TabIndex = 3;
            Box3.TabStop = false;
            Box3.Text = "";
            Box3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Box3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Box3_KeyPress);
            Box3.Enter += new System.EventHandler(Box_Enter);
            Box2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Box2.Location = new System.Drawing.Point(32, 0);
            Box2.MaxLength = 3;
            Box2.Name = "Box2";
            Box2.Size = new System.Drawing.Size(20, 13);
            Box2.TabIndex = 2;
            Box2.TabStop = false;
            Box2.Text = "";
            Box2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Box2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Box2_KeyPress);
            Box2.Enter += new System.EventHandler(Box_Enter);
            label1.Location = new System.Drawing.Point(56, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(8, 13);
            label1.TabIndex = 1;
            label1.Text = ".";
            label1.EnabledChanged += new System.EventHandler(label_EnabledChanged);
            Box1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Box1.Location = new System.Drawing.Point(4, 0);
            Box1.MaxLength = 3;
            Box1.Name = "Box1";
            Box1.Size = new System.Drawing.Size(20, 13);
            Box1.TabIndex = 1;
            Box1.Text = "";
            Box1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Box1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Box1_KeyPress);
            Box1.Enter += new System.EventHandler(Box_Enter);
            base.Controls.AddRange(new System.Windows.Forms.Control[1]
            {
            panel1
            });
            base.Name = "IPTextBox";
            base.Size = new System.Drawing.Size(128, 18);
            panel1.ResumeLayout(performLayout: false);
            ResumeLayout(performLayout: false);
        }

        private bool IsValid(string inString)
        {
            try
            {
                int num = int.Parse(inString);
                if (num >= 0 && num <= 255)
                {
                    return true;
                }
                MessageBox.Show("Must Be Between 0 and 255", "Out Of Range");
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void Box1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "." || char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                if (e.KeyChar.ToString() == ".")
                {
                    if (Box1.Text != "" && Box1.Text.Length != Box1.SelectionLength)
                    {
                        if (IsValid(Box1.Text))
                        {
                            Box2.Focus();
                        }
                        else
                        {
                            Box1.SelectAll();
                        }
                    }
                    e.Handled = true;
                }
                else if (Box1.SelectionLength != Box1.Text.Length && Box1.Text.Length == 2)
                {
                    if (e.KeyChar == '\b')
                    {
                        Box1.Text.Remove(Box1.Text.Length - 1, 1);
                    }
                    else if (!IsValid(Box1.Text + e.KeyChar.ToString()))
                    {
                        Box1.SelectAll();
                        e.Handled = true;
                    }
                    else
                    {
                        Box2.Focus();
                    }
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Box2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "." || char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                if (e.KeyChar.ToString() == ".")
                {
                    if (Box2.Text != "" && Box2.Text.Length != Box2.SelectionLength)
                    {
                        if (IsValid(Box1.Text))
                        {
                            Box3.Focus();
                        }
                        else
                        {
                            Box2.SelectAll();
                        }
                    }
                    e.Handled = true;
                }
                else if (Box2.SelectionLength != Box2.Text.Length)
                {
                    if (Box2.Text.Length == 2)
                    {
                        if (e.KeyChar == '\b')
                        {
                            Box2.Text.Remove(Box2.Text.Length - 1, 1);
                        }
                        else if (!IsValid(Box2.Text + e.KeyChar.ToString()))
                        {
                            Box2.SelectAll();
                            e.Handled = true;
                        }
                        else
                        {
                            Box3.Focus();
                        }
                    }
                }
                else if (Box2.Text.Length == 0 && e.KeyChar == '\b')
                {
                    Box1.Focus();
                    Box1.SelectionStart = Box1.Text.Length;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Box3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "." || char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                if (e.KeyChar.ToString() == ".")
                {
                    if (Box3.Text != "" && Box3.SelectionLength != Box3.Text.Length)
                    {
                        if (IsValid(Box1.Text))
                        {
                            Box4.Focus();
                        }
                        else
                        {
                            Box3.SelectAll();
                        }
                    }
                    e.Handled = true;
                }
                else if (Box3.SelectionLength != Box3.Text.Length)
                {
                    if (Box3.Text.Length == 2)
                    {
                        if (e.KeyChar == '\b')
                        {
                            Box3.Text.Remove(Box3.Text.Length - 1, 1);
                        }
                        else if (!IsValid(Box3.Text + e.KeyChar.ToString()))
                        {
                            Box3.SelectAll();
                            e.Handled = true;
                        }
                        else
                        {
                            Box4.Focus();
                        }
                    }
                }
                else if (Box3.Text.Length == 0 && e.KeyChar == '\b')
                {
                    Box2.Focus();
                    Box2.SelectionStart = Box2.Text.Length;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Box4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                if (Box4.SelectionLength != Box4.Text.Length)
                {
                    if (Box4.Text.Length == 2)
                    {
                        if (e.KeyChar == '\b')
                        {
                            Box4.Text.Remove(Box4.Text.Length - 1, 1);
                        }
                        else if (!IsValid(Box4.Text + e.KeyChar.ToString()))
                        {
                            Box4.SelectAll();
                            e.Handled = true;
                        }
                    }
                }
                else if (Box4.Text.Length == 0 && e.KeyChar == '\b')
                {
                    Box3.Focus();
                    Box3.SelectionStart = Box3.Text.Length;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Box_Enter(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void label_EnabledChanged(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (label.Enabled)
            {
                label.BackColor = SystemColors.Window;
            }
            else
            {
                label.BackColor = SystemColors.Control;
            }
        }

        private void panel1_EnabledChanged(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel.Enabled)
            {
                panel.BackColor = SystemColors.Window;
            }
            else
            {
                panel.BackColor = SystemColors.Control;
            }
        }
    }
}