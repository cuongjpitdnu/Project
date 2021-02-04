using GPMain.Common;
using GPMain.Properties;
using GPModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GPMain.Views.Controls
{
    /// <summary>
    /// Meno: Create field binding data for menu member
    /// Create by Nguyễn Văn Hải
    /// </summary>
    public class MenuMemberBuffer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Dictionary<string, ExTMember> _ListAllMember = new Dictionary<string, ExTMember>();
        private Dictionary<string, ExTMember> _ListMember = new Dictionary<string, ExTMember>();
        //private UserMember[] _ArrUserMember;
        private string _RootID;
        private int _PageIndex = 1;
        private int _Page = 1;
        private int _MaxPage;
        private int _MaxPages;
        private int _TotalMember;
        private int _NumberMemberInPage;
        private int _NumberShowMember;
        private string _strTotalMember;

        private bool _ButtonNextVisible = true;
        private bool _ButtonPreVisible = true;
        private bool _MemberSelected = false;
        private Point _FrameSelectedLocation = new Point(0, 0);
        private Point _UpFrame;
        private Point _DownFrame;
        private Point _LeftFrame;
        private Point _RightFrame;
        private int _UserMemberWidth = 240;
        private int _UserMemberHeight = 85;

        private Color _ForeColor1 = Color.Black;
        private Color _ForeColor2 = Color.Black;
        private Color _ForeColor3 = Color.Black;
        private Color _ForeColor4 = Color.Black;

        private string _Text1 = "1";
        private string _Text2 = "2";
        private string _Text3 = "3";
        private string _Text4 = "4";
        private bool _Visible1 = true;
        private bool _Visible2 = true;
        private bool _Visible3 = true;
        private bool _Visible4 = true;

        public Color ForeColor1 { get => _ForeColor1; set { _ForeColor1 = value; OnPropertyChanged(nameof(ForeColor1)); } }
        public Color ForeColor2 { get => _ForeColor2; set { _ForeColor2 = value; OnPropertyChanged(nameof(ForeColor2)); } }
        public Color ForeColor3 { get => _ForeColor3; set { _ForeColor3 = value; OnPropertyChanged(nameof(ForeColor3)); } }
        public Color ForeColor4 { get => _ForeColor4; set { _ForeColor4 = value; OnPropertyChanged(nameof(ForeColor4)); } }
        public string Text1 { get => _Text1; set { _Text1 = value; OnPropertyChanged(nameof(Text1)); } }
        public string Text2 { get => _Text2; set { _Text2 = value; OnPropertyChanged(nameof(Text2)); } }
        public string Text3 { get => _Text3; set { _Text3 = value; OnPropertyChanged(nameof(Text3)); } }
        public string Text4 { get => _Text4; set { _Text4 = value; OnPropertyChanged(nameof(Text4)); } }
        public bool Visible1 { get => _Visible1; set { _Visible1 = value; OnPropertyChanged(nameof(Visible1)); } }
        public bool Visible2 { get => _Visible2; set { _Visible2 = value; OnPropertyChanged(nameof(Visible2)); } }
        public bool Visible3 { get => _Visible3; set { _Visible3 = value; OnPropertyChanged(nameof(Visible3)); } }
        public bool Visible4 { get => _Visible4; set { _Visible4 = value; OnPropertyChanged(nameof(Visible4)); } }

        public int PageIndex
        {
            get => _PageIndex;
            set
            {
                _PageIndex = value;
                ForeColor1 = _PageIndex == (Page - 1) * 4 + 1 ? Color.Red : Color.Black;
                ForeColor2 = _PageIndex == (Page - 1) * 4 + 2 ? Color.Red : Color.Black;
                ForeColor3 = _PageIndex == (Page - 1) * 4 + 3 ? Color.Red : Color.Black;
                ForeColor4 = _PageIndex == (Page - 1) * 4 + 4 ? Color.Red : Color.Black;
            }
        }
        public int Page
        {
            get => _Page;
            set
            {
                _Page = value;
                Text1 = ((_Page - 1) * 4 + 1).ToString();
                Text2 = ((_Page - 1) * 4 + 2).ToString();
                Text3 = ((_Page - 1) * 4 + 3).ToString();
                Text4 = ((_Page - 1) * 4 + 4).ToString();
                int Temp = TotalMember - (_Page - 1) * NumberShowMember * 4;
                NumberMemberInPage = Temp >= NumberShowMember * 4 ? NumberShowMember * 4 : Temp;
                ButtonNextVisible = Page < MaxPages && MaxPages > 1;
                ButtonPreVisible = Page > 1;
            }
        }
        public int MaxPages { get => _MaxPages; set => _MaxPages = value; }
        public int TotalMember
        {
            get => _TotalMember;
            set
            {
                _TotalMember = value;

                MaxPages = _TotalMember / (NumberShowMember == 0 ? 1 : NumberShowMember * 4) + ((_TotalMember % (NumberShowMember == 0 ? 1 : NumberShowMember * 4)) > 0 ? 1 : 0);
                Page = 1;
                MaxPage = _TotalMember / (NumberShowMember == 0 ? 1 : NumberShowMember) + (_TotalMember % (NumberShowMember == 0 ? 1 : NumberShowMember) != 0 ? 1 : 0);
                //OnPropertyChanged(nameof(TotalMember));
                StrTotalMember = string.Format(AppConst.FormatNumber, TotalMember);
            }
        }
        public int NumberMemberInPage
        {
            get => _NumberMemberInPage;
            set
            {
                _NumberMemberInPage = value;
                Visible1 = _NumberMemberInPage > 0;
                Visible2 = _NumberMemberInPage > NumberShowMember;
                Visible3 = _NumberMemberInPage > NumberShowMember * 2;
                Visible4 = _NumberMemberInPage > NumberShowMember * 3;
            }
        }
        public int NumberShowMember
        {
            get => _NumberShowMember;
            set
            {
                _NumberShowMember = value;
                MaxPages = _TotalMember / (_NumberShowMember == 0 ? 1 : _NumberShowMember * 4) + ((_TotalMember % (_NumberShowMember == 0 ? 1 : _NumberShowMember * 4)) > 0 ? 1 : 0);
                Page = 1;
            }
        }
        public bool ButtonNextVisible { get => _ButtonNextVisible; set { _ButtonNextVisible = value; OnPropertyChanged(nameof(ButtonNextVisible)); } }
        public bool ButtonPreVisible { get => _ButtonPreVisible; set { _ButtonPreVisible = value; OnPropertyChanged(nameof(ButtonPreVisible)); } }

        public Dictionary<string, ExTMember> ListAllMember { get => _ListAllMember; set => _ListAllMember = value; }
        public Dictionary<string, ExTMember> ListMember
        {
            get => _ListMember;
            set
            {
                _ListMember = value;
                TotalMember = _ListMember.Count;
                PageIndex = 1;
            }
        }

        public bool MemberSelected
        {
            get => _MemberSelected;
            set
            {
                _MemberSelected = value;
                OnPropertyChanged(nameof(MemberSelected));
            }
        }
        public Point FrameSelectedLocation
        {
            get => _FrameSelectedLocation;
            set
            {
                _FrameSelectedLocation = value;
                UpFrame = new Point(_FrameSelectedLocation.X - 3, _FrameSelectedLocation.Y - 4);
                DownFrame = new Point(_FrameSelectedLocation.X - 3, _FrameSelectedLocation.Y + UserMemberHeight + 2);
                LeftFrame = new Point(_FrameSelectedLocation.X - 3, _FrameSelectedLocation.Y - 4);
                RightFrame = new Point(_FrameSelectedLocation.X + UserMemberWidth + 3, _FrameSelectedLocation.Y - 4);
            }
        }

        public Point UpFrame
        {
            get => _UpFrame;
            set
            {
                _UpFrame = value;
                OnPropertyChanged(nameof(UpFrame));
            }
        }
        public Point DownFrame
        {
            get => _DownFrame;
            set
            {
                _DownFrame = value;
                OnPropertyChanged(nameof(DownFrame));
            }
        }
        public Point LeftFrame
        {
            get => _LeftFrame;
            set
            {
                _LeftFrame = value;
                OnPropertyChanged(nameof(LeftFrame));
            }
        }
        public Point RightFrame
        {
            get => _RightFrame;
            set
            {
                _RightFrame = value;
                OnPropertyChanged(nameof(RightFrame));
            }
        }
        public int UserMemberWidth { get => _UserMemberWidth; private set => _UserMemberWidth = value; }
        public int UserMemberHeight { get => _UserMemberHeight; private set => _UserMemberHeight = value; }
        public string RootID { get => _RootID; set => _RootID = value; }
        public ExTMember MemberCurrent { get; set; }
        public int MaxPage { get => _MaxPage; set => _MaxPage = value; }
        //public UserMember[] ArrUserMember { get => _ArrUserMember; set => _ArrUserMember = value; }
        public string StrTotalMember { get => _strTotalMember; set { _strTotalMember = value; OnPropertyChanged(nameof(StrTotalMember)); } }
    }
}
