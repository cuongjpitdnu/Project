using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyMgnt.Common
{
    public partial class DataGridViewBase : DataGridView
    {
        public DataGridViewBase()
        {
            InitializeComponent();
            IntDataGridView();
        }

        public DataGridViewBase(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            IntDataGridView();
        }

        private void IntDataGridView()
        {
            this.AutoGenerateColumns = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.RowHeadersVisible = false;
            this.EnableHeadersVisualStyles = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.MultiSelect = false;
            this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 192, 128);
            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.ColumnHeadersHeight = 26;
            this.BackgroundColor = Color.Ivory;
        }
    }
}
