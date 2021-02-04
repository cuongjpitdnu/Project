using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GP40Common
{
    public partial class NumericTB : TextBox
    {
        public NumericTB()
        {
            InitializeComponent();
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }

        public int Value
        {
            get {
                return clsCommon.ConvertToInt(this.Text);
            }
            set {
                this.Text = "";
                if (clsCommon.IsNumber(value))
                {
                    this.Text = clsCommon.ConvertToInt(value).ToString();
                }
            }
        }
    }
}
