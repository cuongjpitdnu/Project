using System.ComponentModel;
using System.Windows.Forms;

namespace GPMain.Views.Controls
{
    public partial class DataGridViewIconColumn : DataGridViewButtonColumn
    {
        public DataGridViewIconColumn()
        {
            InitializeComponent();
        }

        public DataGridViewIconColumn(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}
