using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSF602.View.ControlLayout
{
    public class NumericTextBox : TextBox
    {
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            //// only allow one decimal point
            //if ((e.KeyChar == '.') && (this.Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }
    }
}
