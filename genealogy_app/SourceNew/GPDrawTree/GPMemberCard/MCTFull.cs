namespace GPMemberCard
{
    public partial class MemberCardTemplFull : MCBase
    {
        public MemberCardTemplFull() : base()
        {
            InitializeComponent();
            CardType = MemberCardType.Full;
        }

        public override MCData ToMCData()
        {
            var rst = base.ToMCData();

            rst.MCDataChildren.Add(new MCDataChild()
            {
                Name = nameof(picImage),
                Position = picImage.Location,
                Size = picFrame.Size,
            });

            return rst;
        }
    }
}