using System.Windows.Forms;

namespace GP40Main.Utility
{
    /// <summary>
    /// Meno        : Support UI/UX
    /// Create by   : 2020.07.28 AKB Nguyễn Thanh Tùng
    /// </summary>
    public static class UIHelper
    {
        public static void FocusLast(this TextBoxBase textBox)
        {
            if (textBox == null) return;
            textBox.Focus();
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
        }

        public static void FocusAndSelected(this TextBoxBase textBox)
        {
            if (textBox == null) return;
            textBox.Focus();
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
        }
    }
}
