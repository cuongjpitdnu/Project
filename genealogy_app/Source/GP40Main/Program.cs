using GP40Main.Core;
using System;
using System.Windows.Forms;

namespace GP40Main
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppManager.Init();
            AppManager.RunAppIfLogin();
        }
    }
}
