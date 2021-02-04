using System;
using System.Windows.Forms;

namespace GPMain
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                AppManager.Init();
                AppManager.RunAppIfLogin();
            }
            catch (Exception ex)
            {
                AppManager.LoggerApp?.Error(typeof(Program), ex);
            }
        }
    }
}
