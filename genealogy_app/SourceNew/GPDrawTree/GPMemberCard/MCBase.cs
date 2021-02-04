using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GPMemberCard
{
    public enum MemberCardType
    {
        Full,
        Input,
        Short,
        Tall,
        VeryShort,
    }

    public class MCBase : UserControl
    {
        public MemberCardType CardType { get; protected set; }

        public MCBase()
        {
        }

        public virtual MCData ToMCData()
        {
            return new MCData() {
                CardType = this.CardType,
                CardSize = this.Size,
                MCDataChildren = new List<MCDataChild>(),
            };
        }
    }

    public class MCData
    {
        public MemberCardType CardType { get; set; }
        public Size CardSize { get; set; }
        public List<MCDataChild> MCDataChildren { get; set; }
    }

    public class MCDataChild
    {
        public string Name { get; set; }
        public Point Position { get; set; }
        public Size Size { get; set; }
    }
}