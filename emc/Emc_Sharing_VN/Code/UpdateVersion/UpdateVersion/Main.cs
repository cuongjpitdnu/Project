using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateVersion
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            frmUpdateVersion updateForm = new frmUpdateVersion();
            updateForm.ShowDialog();
        }

    }
}
