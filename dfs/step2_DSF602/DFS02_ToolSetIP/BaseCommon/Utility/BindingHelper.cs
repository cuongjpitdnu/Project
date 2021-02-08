using System.Collections.Generic;
using System.Windows.Forms;

namespace BaseCommon.Utility
{
    public class DataBinding<T>
    {
        public string Display { get; set; }
        public T Value { get; set; }
    }

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
            cboBinding.SelectedValue = selectValue;
        }
    }
}
