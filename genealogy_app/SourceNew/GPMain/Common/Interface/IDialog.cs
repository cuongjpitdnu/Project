using GPMain.Core;
using System;
using System.Windows.Forms;

namespace GPMain.Common.Interface
{
    public interface IDialog
    {
        DialogResult Ok(string msg);
        DialogResult Error(string msg);
        DialogResult Warning(string msg);
        bool Confirm(string msg);

        void ShowProgressBar(Action<ProgressBarManager> action, string title = "Đang xử lý", string titleBar = "");

        string OpenFile(string filter = "", string title = "Save As");
        string SaveFile(string fileName = "", string filter = "", string title = "");
    }
}
