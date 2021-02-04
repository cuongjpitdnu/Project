using GP40DrawTree;
using GPMain.Views.Controls;
using SkiaSharp.Views.Desktop;

namespace GPMain.Views.MenuItems
{
    public partial class LineItem : ItemBase
    {
        public LineItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();

            // Set Event
            cbChildLineColor.ColorControl.onColorChanged += (e) => ChangeData_Event();
            cbSpouseLineColor.ColorControl.onColorChanged += (e) => ChangeData_Event();
            cbSelectMember.ColorControl.onColorChanged += (e) => ChangeData_Event();
        }

        protected override void SetConfig()
        {
            base.SetConfig();

            Config.ChildLineColor = ColorDrawHelper.FromColor(cbChildLineColor.Color);
            Config.SpouseLineColor = ColorDrawHelper.FromColor(cbSpouseLineColor.Color);
            Config.SelectedMemberColor = ColorDrawHelper.FromColor(cbSelectMember.Color);
        }

        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();

            cbChildLineColor.Color = Config.ChildLineColor.ToDrawingColor();
            cbSpouseLineColor.Color = Config.SpouseLineColor.ToDrawingColor();
            cbSelectMember.Color = Config.SelectedMemberColor.ToDrawingColor();
        }
    }
}