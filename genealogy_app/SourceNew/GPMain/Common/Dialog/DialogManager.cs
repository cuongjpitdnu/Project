using GPMain.Common.Interface;
using GPMain.Common.Navigation;
using GPMain.Core;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GPMain.Common.Dialog
{
    public enum MessageIcon : int
    {
        Error,
        Warning,
        OK,
        Question,
    }

    public class DialogManager : IDialog
    {
        public DialogResult Show(string msg, MessageIcon icon)
        {
            var param = new NavigationParameters();
            param.Add(MessagePage.MSG_CONTENT_KEY, msg);
            param.Add(MessagePage.MSG_ICON_KEY, (int)icon);

            return AppManager.Navigation.ShowDialogWithParam<MessagePage>(param).Result;
        }

        public DialogResult Ok(string msg)
        {
            return Show(msg, MessageIcon.OK);
        }

        public DialogResult Error(string msg)
        {
#if DEBUG
            return Show(msg, MessageIcon.Error);
#else
            return Show("Có lỗi trong quá trình xử lý", MessageIcon.Error);
#endif
        }

        public DialogResult Warning(string msg)
        {
            return Show(msg, MessageIcon.Warning);
        }

        public bool Confirm(string msg)
        {
            return Show(msg, MessageIcon.Question) == DialogResult.OK;
        }

        public Color ColorPicker(Color color)
        {
            return AppManager.Navigation.ShowDialogWithParam<ColorPicker, Color, Color>(color);
        }

        public const string DIALOG_FILTER_ALL = "All File (*.*)|*.*";
        public const string DIALOG_FILTER_TEXT = "Text File (*.txt)|*.txt";
        public const string DIALOG_FILTER_EXCEL = "Excel File (*.xlsx)|*.xlsx";
        public const string DIALOG_FILTER_ZIP = "Zip Files|*.zip;*.rar";

        public string SaveFile(string fileName = "", string filter = DIALOG_FILTER_ALL, string title = "Save As")
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = filter;
                saveDialog.AddExtension = true;
                saveDialog.RestoreDirectory = true;
                saveDialog.Title = title;
                saveDialog.FileName = fileName;
                saveDialog.InitialDirectory = Path.GetPathRoot(Application.StartupPath);

                if (saveDialog.ShowDialog() != DialogResult.OK)
                {
                    return string.Empty;
                }

                return saveDialog.FileName;
            }
        }

        public string OpenFile(string filter = DIALOG_FILTER_ALL, string title = "Save As")
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = filter;
                openFileDialog.AddExtension = true;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = title;
                openFileDialog.InitialDirectory = Path.GetPathRoot(Application.StartupPath);
                openFileDialog.CheckFileExists = true;
                openFileDialog.AddExtension = true;

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return string.Empty;
                }

                return openFileDialog.FileName;
            }
        }

        public void ShowProgressBar(Action<ProgressBarManager> action, string title = "Đang xử lý", string titleBar = "")
        {
            using (var objProgressBar = new ProgressBarManager())
            {
                objProgressBar.fncCreateProgressBar(title);
               
                objProgressBar.ProcessThread = new Thread(() =>
                {
                    action?.Invoke(objProgressBar);
                    objProgressBar.CompleteProcess = true;
                });
                objProgressBar.fncStartProgressBar(titleBar);
            }
        }
    }
}