using GPMain.Views;
using GPMain.Common.Interface;
using System;
using System.Windows.Forms;
using MaterialSkin;
using System.Drawing;
using GPMain.Common.Helper;

namespace GPMain.Common.Navigation
{
    public class NavigationService : INavigation
    {
        private bool isRunning = false;

        public void RunApp<TFormTheme>() where TFormTheme : Form
        {
            if (isRunning)
            {
                return;
            }

            var frmStart = Activator.CreateInstance<TFormTheme>();

            if (frmStart == null || frmStart.IsDisposed)
            {
                Application.Exit();
                return;
            }

            Application.Run(frmStart);
            isRunning = true;
        }

        public void NextMenu(Type typeView, TabPage tabPage)
        {
            try
            {
                var obj = Activator.CreateInstance(typeView, new object[] { null, ModeForm.None });
                if (tabPage != null && obj != null && obj is BaseUserControl userControl)
                {
                    userControl.Dock = DockStyle.Fill;
                    tabPage.Controls.Clear();
                    tabPage.SuspendLayout();
                    tabPage.Controls.Add(userControl);
                    tabPage.PerformLayout();
                    tabPage.ResumeLayout(true);
                    userControl.ProcessAffterAddForm();
                }
            }
            catch (Exception ex)
            {
            }
            AppManager.MinimizeFootprint();
        }

        #region ShowDialog

        public NavigationResult ShowDialog<TPage>(ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl
        {
            return ShowDialogWithParam<TPage>(null, mode, statusBarColor);
        }

        public TResult ShowDialog<TPage, TResult>(ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl
        {
            return ShowDialogWithParam<TPage, TResult>(null, mode, statusBarColor);
        }

        #endregion ShowDialog

        #region ShowDialogWithParam

        public TResult ShowDialogWithParam<TPage, TResult>(NavigationParameters parameters = null, ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl
        {
            return ShowDialogWithParam<TPage>(parameters, mode, statusBarColor).GetValue<TResult>();
        }

        public NavigationResult ShowDialogWithParam<TPage>(NavigationParameters parameters = null, ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl
        {
            try
            {
                var userControl = (TPage)Activator.CreateInstance(typeof(TPage), new object[] { parameters, mode });

                if (userControl == null)
                {
                    return new NavigationResult();
                }
                using (var frmStart = new OnlyShowBarForm())
                {
                    if (statusBarColor != null)
                    {
                        frmStart.BackColorStatusBar = statusBarColor.BackColor;
                        frmStart.ForeColorStatusBar = statusBarColor.ForeColor;
                    }
                    frmStart.AddPage(userControl);
                    frmStart.ShowDialog();
                    return frmStart.ResultData ?? new NavigationResult();
                }
            }
            catch (Exception ex)
            {
                AppManager.LoggerApp.Error(typeof(NavigationService), ex);
                return new NavigationResult();
            }
            finally
            {
                AppManager.MinimizeFootprint();
            }
        }



        public NavigationResult ShowDialogWithParam<TPage, TParam>(TParam parameters, ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl
        {
            return ShowDialogWithParam<TPage>(new NavigationParameters(parameters), mode, statusBarColor);
        }

        public TResult ShowDialogWithParam<TPage, TParam, TResult>(TParam parameters, ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl
        {
            return ShowDialogWithParam<TPage, TResult>(new NavigationParameters(parameters), mode, statusBarColor);
        }

        #endregion ShowDialogWithParam
    }
}
