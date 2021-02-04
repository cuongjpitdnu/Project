using System;
using System.Drawing;
using GP40Main.Core;
using GP40Main.Services.Navigation;
using static GP40Main.Core.AppConst;

namespace GP40Main.Services.Dialog
{
    public partial class ColorPicker : BaseUserControl
    {
        public ColorPicker(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            this.SetDefaultUI();
        }

        private void ColorPicker_Load(object sender, EventArgs e)
        {
            materialColorPicker1.Value = this.GetParameters().GetValue<Color>();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close(materialColorPicker1.Value);
        }
    }
}
