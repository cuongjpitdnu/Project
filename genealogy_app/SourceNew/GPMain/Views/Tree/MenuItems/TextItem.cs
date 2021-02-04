using GP40DrawTree;
using GPMain.Views.Controls;
using SkiaSharp.Views.Desktop;

namespace GPMain.Views.MenuItems
{
    public partial class TextItem : ItemBase
    {
        public TextItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();
            cbTextColor.ColorControl.onColorChanged += (e) => ChangeData_Event();
        }

        protected override void SetConfig()
        {
            base.SetConfig();
            Config.TextColor = ColorDrawHelper.FromColor(cbTextColor.Color);
        }

        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();
            cbTextColor.Color = Config.TextColor.ToDrawingColor();
        }
    }
}