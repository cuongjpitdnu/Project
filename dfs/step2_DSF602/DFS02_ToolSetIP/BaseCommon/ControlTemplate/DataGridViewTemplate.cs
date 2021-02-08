using System.Windows.Forms;

namespace BaseCommon.ControlTemplate
{
    public partial class dgv : DataGridView
    {
        public dgv()
        {
            InitializeComponent();

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.RowHeadersVisible = false;
            this.RowTemplate.Height = 30;
            this.ColumnHeadersHeight = 30;
            this.EnableHeadersVisualStyles = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.ReadOnly = true;
            this.MultiSelect = false;
            this.AutoGenerateColumns = false;

            try
            {
                typeof(DataGridView).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.SetProperty,
                    null,
                    this,
                    new object[] { true }
                );
            }
            catch
            {
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            base.OnDataBindingComplete(e);
            this.ClearSelection();
            this.CurrentCell = null;
        }

        public T GetSelectedData<T>() where T : class
        {
            if (this.SelectedRows.Count < 1)
            {
                return null;
            }

            return this.SelectedRows[0].DataBoundItem as T;
        }
    }
}
