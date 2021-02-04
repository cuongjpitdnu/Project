using System.Collections.Generic;
using System.Windows.Forms;

namespace GPMain.Common.Helper
{

    /// <summary>
    /// Meno        : Type data binding common
    /// Create by   : AKB Nguyễn Thanh Tùng
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataBinding<T>
    {
        public string Display { get; set; }
        public T Value { get; set; }
    }

    /// <summary>
    /// Meno        : Support binding data to control
    /// Create by   : AKB Nguyễn Thanh Tùng
    /// </summary>
    public class BindingHelper
    {
        public static void Combobox<T>(ComboBox cboBinding, List<DataBinding<T>> dataBindings, T selectValue = default(T))
        {
            if (cboBinding == null)
            {
                return;
            }

            cboBinding.DisplayMember = "Display";
            cboBinding.ValueMember = "Value";
            cboBinding.DataSource = dataBindings;

            if (selectValue != null && !selectValue.Equals(default(T)))
            {
                cboBinding.SelectedValue = selectValue;
            }
            else
            {
                cboBinding.SelectedIndex = -1;
            }
        }

        public static void Combobox<T>(ComboBox cboBinding, List<T> dataBindings, string DisplayMember, string ValueMember, T selectValue = default(T))
        {
            if (cboBinding == null)
            {
                return;
            }

            cboBinding.DisplayMember = DisplayMember;
            cboBinding.ValueMember = ValueMember;
            cboBinding.DataSource = dataBindings;

            if (selectValue != null && !selectValue.Equals(default(T)))
            {
                cboBinding.SelectedValue = selectValue;
            }
            else
            {
                cboBinding.SelectedIndex = -1;
            }
        }

        public static void BindingDataGrid<T>(DataGridView dgv, List<T> dataBinding)
        {
            var source = new BindingSource(dataBinding, null);
            dgv.DataSource = source;
        }
    }
}
