using GPMain.Common.Navigation;
using System;
using System.Drawing;

namespace GPMain.Common.Dialog
{
    public partial class ColorPicker : BaseUserControl
    {
        public ColorPicker(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
        }

        private void ColorPicker_Load(object sender, EventArgs e)
        {
            materialColorPicker1.Value = this.Params.GetValue<Color>();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close(materialColorPicker1.Value);
        }
    }
}
