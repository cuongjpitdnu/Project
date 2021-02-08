using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseCommon.Core
{
    public class AppCore
    {
        public static void Start<TForm>() where TForm : BaseForm
        {
            var frmStart = Activator.CreateInstance<TForm>();

            if (frmStart == null || frmStart.IsDisposed)
            {
                Application.Exit();
                return;
            }

            Application.Run(frmStart);
        }

        public static object ShowDialog<TForm>(object objParams = null, BaseForm parentForm = null) where TForm : BaseForm
        {
            using (var frmStart = Activator.CreateInstance<TForm>())
            {
                frmStart.Params = objParams;
                frmStart.FormParent = parentForm;
                frmStart.ShowDialog();

                return frmStart.ResultData;
            }
        }

        public static Task<object> ShowDialogAsync<TForm>(object objParams = null, BaseForm parentForm = null) where TForm : BaseForm
        {
            return Task.Run(() => ShowDialog<TForm>(objParams, parentForm));
        }
    }
}
