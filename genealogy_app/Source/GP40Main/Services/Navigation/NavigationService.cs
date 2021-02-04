using GP40Main.Core;
using GP40Main.Themes;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GP40Main.Core.AppConst;

namespace GP40Main.Services.Navigation
{
    public class NavigationService
    {
        private bool isRunning = false;

        public NavigationResult ShowDialog<TPage>(ModeForm mode) where TPage : BaseUserControl
        {
            return ShowDialog<TPage>(null, mode);
        }

        public TResult ShowDialog<TPage, TResult>(ModeForm mode) where TPage : BaseUserControl
        {
            return ShowDialog<TPage, TResult>(null, mode);
        }

        public TResult ShowDialog<TPage, TResult>(NavigationParameters parameters = null, ModeForm mode = ModeForm.None) where TPage : BaseUserControl
        {
            return ShowDialog<TPage>(parameters, mode).GetValue<TResult>();
        }

        public NavigationResult ShowDialog<TPage>(NavigationParameters parameters = null, ModeForm mode = ModeForm.None) where TPage : BaseUserControl
        {
            try
            {
                var userControl = (TPage)Activator.CreateInstance(typeof(TPage), new object[] { parameters, mode });

                if (userControl == null)
                {
                    return new NavigationResult();
                }

                using (BaseForm frmStart = new OnlyShowBarForm())
                {
                    frmStart.AddPage(userControl);
                    frmStart.ShowDialog();
                    return frmStart.ResultData ?? new NavigationResult();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task NextMenuAsync<TPage>(TabPage tabPage) where TPage : BaseUserControl
        {
            var userControl = (TPage)Activator.CreateInstance(typeof(TPage), new object[] { null, ModeForm.None });

            if (tabPage != null && userControl != null)
            {
                tabPage.Controls.Clear();
                tabPage.Controls.Add(userControl);
                userControl.Dock = DockStyle.Fill;
            }

            return Task.CompletedTask;
        }

        public Task NextMenuAsync(Type typeView, TabPage tabPage)
        {
            var obj = Activator.CreateInstance(typeView, new object[] { null, ModeForm.None });

            if (tabPage != null && obj != null && obj is BaseUserControl userControl)
            {
                tabPage.SuspendLayout();
                tabPage.Controls.Clear();
                tabPage.Controls.Add(userControl);
                userControl.Dock = DockStyle.Fill;
                tabPage.PerformLayout();
                tabPage.ResumeLayout(true);
                Application.DoEvents();
            }

            return Task.CompletedTask;
        }

        public void RunApp<TFormTheme>() where TFormTheme : BaseForm
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
    }
}
