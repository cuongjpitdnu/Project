using MetroFramework.Controls;
using System.ComponentModel;
using System.Windows.Forms;

namespace GP40Main.Themes.Controls
{
    public partial class DataGridTemplate : MetroGrid
    {
        public DataGridTemplate()
        {
            InitializeComponent();
            InitControl();
        }

        public DataGridTemplate(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            InitControl();
        }

        private void InitControl()
        {
            ReadOnly = true;
            AutoGenerateColumns = false;
            RowHeadersVisible = false;

            if (RowHeadersDefaultCellStyle == null)
            {
                RowHeadersDefaultCellStyle = new DataGridViewCellStyle();
            }

            RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void DataGridTemplate_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ClearSelection();
        }
    }
}
