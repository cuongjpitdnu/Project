using GPModels;
using SkiaSharp;
using System.Collections.Generic;

namespace GPTree
{
    public class TreeConfig
    {
        public string PathRoot { get; set; }
        public Dictionary<string, TMember> DataMember { get; set; }

        //public clsConst.ENUM_MEMBER_TEMPLATE TemplateMember { get; set; } = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
        public SKColor BackgroudColor { get; set; } = new SKColor(250, 191, 98);

        public SKColor SelectedMemberColor { get; set; } = SKColors.RoyalBlue;
        public SKColor ChildLineColor { get; set; } = SKColors.Blue;
        public SKColor SpouseLineColor { get; set; } = SKColors.Red;
        public SKColor TextColor { get; set; } = SKColors.Black;
        public SKColor BorderColor { get; set; } = SKColors.Blue;
        public SKColor MaleBackColor { get; set; } = SKColors.GhostWhite;
        public SKColor FeMaleBackColor { get; set; } = SKColors.LightPink;

        public int MemberVerticalSpace { get; set; } = 30;
        public int MemberHorizonSpace { get; set; } = 30;
        public int NumberFrame { get; set; } = 1;

        public float MinLeft { get; set; }
        public float MinTop { get; set; }
    }
}