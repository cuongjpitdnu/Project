using GPMain.Common.Navigation;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GPMain.Common.Interface
{
    public interface INavigation
    {
        void RunApp<TFormTheme>() where TFormTheme : Form;

        NavigationResult ShowDialog<TPage>(ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl;
        TResult ShowDialog<TPage, TResult>(ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl;

        NavigationResult ShowDialogWithParam<TPage>(NavigationParameters parameters = null, ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl;
        NavigationResult ShowDialogWithParam<TPage, TParam>(TParam parameters, ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl;

        TResult ShowDialogWithParam<TPage, TResult>(NavigationParameters parameters = null, ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl;
        TResult ShowDialogWithParam<TPage, TParam, TResult>(TParam parameters, ModeForm mode = ModeForm.None, StatusBarColor statusBarColor = null) where TPage : BaseUserControl;

        void NextMenu(Type typeView, TabPage tabPage);

    }
}
