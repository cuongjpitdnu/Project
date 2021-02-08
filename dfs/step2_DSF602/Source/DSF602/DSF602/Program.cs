using System;
using System.Windows.Forms;

namespace DSF602
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppManager.Configure();
            AppManager.UserLogin = AppManager.ShowDialog<View.LoginForm>() as Model.MUser;

            if (AppManager.UserLogin == null)
            {
                Application.Exit();
                return;
            }

            AppManager.Start<View.MainForm>();
        }
    }
}
