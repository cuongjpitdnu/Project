using GP40DrawTree;
using GPMain.Views.Controls;

namespace GPMain.Views.MenuItems
{
    public partial class SpaceItem : ItemBase
    {
        public SpaceItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();

            // Set Event
            sliderMemberHorizonSpace.onValueChanged += (e) => ChangeData_Event();
            sliderMemberVerticalSpace.onValueChanged += (e) => ChangeData_Event();
        }

        protected override void SetConfig()
        {
            base.SetConfig();

            Config.MemberHorizonSpace = sliderMemberHorizonSpace.Value;
            Config.MemberVerticalSpace = sliderMemberVerticalSpace.Value;
        }

        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();

            sliderMemberHorizonSpace.Value = Config.MemberHorizonSpace;
            sliderMemberVerticalSpace.Value = Config.MemberVerticalSpace;
        }
    }
}