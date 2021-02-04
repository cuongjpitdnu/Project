namespace GPModels
{
    public class ThemeConfig : BaseModel
    {
        public string DisplayName { get; set; }
        public bool IsDefault { get; set; }

        public string BackgroudColor { get; set; } = "#fab509";
        public string SelectedMemberColor { get; set; } = "#328fa8";
        public string ChildLineColor { get; set; } = "#3f2d3b";
        public string SpouseLineColor { get; set; } = "#3f2d3b";
        public string TextColor { get; set; } = "#dd800b";
        public string BorderColor { get; set; } = "#3f2d3b";
        public string MaleBackColor { get; set; } = "#9c1f2d";
        public string FeMaleBackColor { get; set; } = "#743042";
        public string UnknowBackColor { get; set; } = "#743042";
        public int MemberVerticalSpace { get; set; } = 30;
        public int MemberHorizonSpace { get; set; } = 30;
        public int NumberFrame { get; set; } = 1;
        public bool ShowImage { get; set; } = true;
        public bool ShowBirthDayDefaul { get; set; } = true;
        public bool ShowDeathDayLunarCalendar { get; set; } = true;
        public bool ShowFamilyLevel { get; set; } = true;
        public bool ShowGender { get; set; } = false;
        public string TypeTextShow { get; set; } = "Normal";
    }
}
