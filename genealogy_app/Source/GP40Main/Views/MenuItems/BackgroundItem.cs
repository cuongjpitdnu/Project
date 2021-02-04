using GP40DrawTree;
using GP40Main.Themes.Controls;
using SkiaSharp.Views.Desktop;

namespace GP40Main.Views.MenuItems
{
    public partial class BackgroundItem : ItemBase
    {
        public BackgroundItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();

            // Set Event
            cbBackgroudColor.ColorControl.onColorChanged += (e) => ChangeData_Event();
            cbMaleBackColor.ColorControl.onColorChanged += (e) => ChangeData_Event();
            cbFeMaleBackColor.ColorControl.onColorChanged += (e) => ChangeData_Event();
        }

        protected override void SetConfig()
        {
            base.SetConfig();

            Config.BackgroudColor = ColorDrawHelper.FromColor(cbBackgroudColor.Color);
            Config.MaleBackColor = ColorDrawHelper.FromColor(cbMaleBackColor.Color);
            Config.FeMaleBackColor = ColorDrawHelper.FromColor(cbFeMaleBackColor.Color);
        }

        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();

            cbBackgroudColor.Color = Config.BackgroudColor.ToDrawingColor();
            cbMaleBackColor.Color = Config.MaleBackColor.ToDrawingColor();
            cbFeMaleBackColor.Color = Config.FeMaleBackColor.ToDrawingColor();
        }
    }
}