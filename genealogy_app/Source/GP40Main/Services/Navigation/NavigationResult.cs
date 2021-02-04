using System.Windows.Forms;

namespace GP40Main.Services.Navigation
{
    public class NavigationResult : NavigationParameters
    {
        public DialogResult Result { get; set; } = DialogResult.None;

        public NavigationResult() : base() 
        { 

        }
        public NavigationResult(object defaultParameter) : base(defaultParameter)
        {

        }

        public NavigationResult(DialogResult dialogResult)
        {
            Result = dialogResult;
        }
    }
}
