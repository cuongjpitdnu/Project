using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GP40Main.Models;
using System.Drawing.Drawing2D;
using static GP40Main.Core.AppConst;
using MaterialSkin;
using GP40Main.Properties;
using GP40Main.Core;
using GP40Main.Utility;

namespace GP40Main.Themes.Controls
{
    public partial class MenuMember : UserControl
    {

        public override Color BackColor {
            get => base.BackColor;
            set {
                base.BackColor = value;

                if (MainLayout != null) MainLayout.BackColor = value;
                if (panelfilter != null) panelfilter.BackColor = value;
                if (flpMember != null) flpMember.BackColor = value;
            }
        }

        public EventHandler<InfoFilter> ChangeFilter;

        private void ChangeFilter_Event(object sender, InfoFilter filter)
        {

            if (sender is Button)
            {
                Button btn = sender as Button;

                if (btn == btnnextpage || btn == btnprepage)
                {
                    page = btn == btnnextpage ? (page < Maxpage ? page++ : page) : (page > 0 ? page-- : page);
                    ChangePage();
                }
                else
                {
                    buttonPage = btn;
                    foreach (Control ctl in panelfilter.Controls)
                    {
                        ctl.ForeColor = ctl == buttonPage ? Color.Red : Color.Black;
                    }
                }
            }
            else
            {
                page = 0;
                buttonPage = btnpage1;
                foreach (Control ctl in panelfilter.Controls)
                {
                    ctl.ForeColor = ctl == buttonPage ? Color.Red : Color.Black;
                }
                ChangePage();
            }
            if (ChangeFilter != null)
            {
                this.Invoke(ChangeFilter, new object[] { sender, filter });
            }
        }

        public EventHandler<EventArgs> PanelMemberSizeChanged;

        private void PanelMemberSizeChanged_Event(object sender, EventArgs e)
        {
        }

        public int NumberMemberInpage = 5;
        string sGender = "";
        string sLiveOrDie = "";
        Button buttonPage;

        private InfoFilter GetFilter()
        {
            sGender = cmbGender.SelectedValue.ToString();
            sLiveOrDie = cmbLiveOrDie.SelectedValue.ToString();
            return new InfoFilter()
            {
                Gender = sGender,
                LiveorDie = sLiveOrDie,
                KeyWord = txtKeyword.Text,
                Page = page,
                ButtonSelect = buttonPage
            };
        }

        public MenuMember()
        {
            InitializeComponent();
            LoadConfig();
        }

        private void LoadConfig()
        {

            this.SetDefaultUI();
            this.Load += (sender, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    BindingHelper.Combobox(cmbGender, GenerateData.GetListGender());
                    BindingHelper.Combobox(cmbLiveOrDie, GenerateData.GetListMemberStatus());
                }));

                this.BeginInvoke(new Action(() =>
                {
                    this.Add(new UserMember(new ExTMember(), string.Empty));
                }));
            };

            //cmbGender.SelectedIndexChanged += (sender, e) => { ChangeFilter_Event(sender, GetFilter()); };
            //cmbLiveOrDie.SelectedIndexChanged += (sender, e) => { ChangeFilter_Event(sender, GetFilter()); };
            //btnpage1.Click += (sender, e) => { ChangeFilter_Event(sender, GetFilter()); };
            //btnpage2.Click += (sender, e) => { ChangeFilter_Event(sender, GetFilter()); };
            //btnpage3.Click += (sender, e) => { ChangeFilter_Event(sender, GetFilter()); };
            //btnpage4.Click += (sender, e) => { ChangeFilter_Event(sender, GetFilter()); };
            //btnnextpage.Click += (sender, e) => { ChangeFilter_Event(sender, GetFilter()); };
            //btnprepage.Click += (sender, e) => { ChangeFilter_Event(sender, GetFilter()); };
            //txtKeyword.TextChanged += (sender, e) => { ChangeFilter_Event(sender, GetFilter()); };
            //flpMember.SizeChanged += (sender, e) => { PanelMemberSizeChanged_Event(sender, e); };
            //buttonPage = btnpage1;
        }

        void MemberMouseClick_Event(object sender, ExTMember e)
        {
            var um = sender as UserMember;

            if (sender == null || flpMember.Controls.Count == 0)
            {
                return;
            }
            foreach (UserMember c in flpMember.Controls)
            {
                c.IsSelected = c.memberinfo.Id == e.Id;
            }
        }

        private void Add(UserMember member)
        {
            if (member != null)
            {
                member.Margin = new Padding(3);
                member.MemberMouseClicked += MemberMouseClick_Event;
                member.Width = flpMember.Width - 6;
                flpMember.Controls.Add(member);            }
        }

        public void AddRange(UserMember[] listmember)
        {
            if (listmember != null)
            {
                if (listmember.Length > 0)
                {
                    foreach (UserMember um in listmember)
                    {
                        um.MemberMouseClicked += MemberMouseClick_Event;
                    }
                    this.SuspendLayout();
                    flpMember.Controls.AddRange(listmember);
                    flpMember.BackgroundImage = null;
                }
                else
                {
                    //flpMember.BackgroundImage = Resources.nodata_available2;
                }
            }
        }
        public void Clear()
        {
            flpMember.Controls.Clear();
        }

        int page = 0;
        private int _Maxpage = 0;
        public int Maxpage
        {
            get { return _Maxpage; }
            set
            {
                if (_Maxpage != value)
                {
                    _Maxpage = value;
                    btnnextpage.Visible = !(page == Maxpage);
                    btnprepage.Visible = !(page == 0);
                }
            }
        }

        public int Element
        {
            get => _Element;
            set
            {
                _Element = value;
                btnpage1.Visible = _Element > 0;
                btnpage2.Visible = _Element > NumberMemberInpage;
                btnpage3.Visible = _Element > NumberMemberInpage * 2;
                btnpage4.Visible = _Element > NumberMemberInpage * 3;

                int wFlowLayoutzPanel = 0;
                foreach (Control ctl in flpPage.Controls)
                    wFlowLayoutzPanel += ctl.Visible ? ctl.Width + 2 : 0;
                flpPage.Width = wFlowLayoutzPanel;
                flpPage.Left = (panelfilter.Width - flpPage.Width) / 2;
            }
        }

        public int TotalMember
        {
            get => _TotalMember;
            set
            {
                _TotalMember = value;
                lbltotalmember.Text = _TotalMember + "";
            }
        }
        private int _Element;

        private int _TotalMember = 0;

        private void btnnextpage_Click(object sender, EventArgs e)
        {

        }

        private void btnprepage_Click(object sender, EventArgs e)
        {

        }

        private void ChangePage()
        {
            btnpage1.Tag = btnpage2.Tag = btnpage3.Tag = btnpage4.Tag = page;
            btnpage1.Text = (page * NumberMemberInpage + 1).ToString();
            btnpage2.Text = (page * NumberMemberInpage + 2).ToString();
            btnpage3.Text = (page * NumberMemberInpage + 3).ToString();
            btnpage4.Text = (page * NumberMemberInpage + 4).ToString();
            if (Maxpage == 0)
            {
                btnprepage.Visible = btnnextpage.Visible = false;
            }
            else
            {
                btnprepage.Visible = page > 0;
                btnnextpage.Visible = page < Maxpage & Element > 0;
            }
        }
    }

    public class UserMember : MaterialSkin.Controls.MaterialCard
    {
        private ContextMenuStripData<TMember> _contextMember;

        static MaterialSkinManager materialSkinmanager = MaterialSkinManager.Instance;

        private static int _Heigh = 85;
        public static int Heigh
        {
            get { return _Heigh; }
        }

        #region Public Event

        public event EventHandler<ExTMember> MemberMouseClicked;

        private void MemberMouseClick_Event(object sender, MouseEventArgs e, ExTMember member)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsSelected = true;
                if (MemberMouseClicked != null & IsSelected)
                {
                    this.BeginInvoke(MemberMouseClicked, new object[] { sender, member });
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                _contextMember.Show(AppManager.GetCursorPosition(), memberinfo);
            }
        }

        public EventHandler<ExTMember> MemberDoubleClick;
        private void MemberSelect_Event(object sender, ExTMember member)
        {
            if (MemberDoubleClick != null)
            {
                this.BeginInvoke(MemberDoubleClick, new object[] { sender, member });
            }
        }

        public EventHandler<ExTMember> MemberEdit;
        private void MemberEdit_Event(object sender, ExTMember member)
        {
            if (MemberEdit != null)
            {
                this.BeginInvoke(MemberEdit, new object[] { sender, member });
            }
        }

        public EventHandler<ExTMember> MemberMouseHover;
        private void MemberMouseHover_Event(object sender, ExTMember member)
        {
            if (RectGender.Contains(MousePosition))
            {
                bMouseHover = true;
                if (MemberEdit != null)
                {
                    this.BeginInvoke(MemberMouseHover, new object[] { sender, member });
                }
            }
            this.Cursor = Cursors.Hand;
            this.BackColor = Color.FromArgb(250, 222, 222);
        }

        public EventHandler<ExTMember> MemberMouseLeave;
        private void MemberMouseLeave_Event(object sender, ExTMember member)
        {
            bMouseHover = false;
            this.Cursor = Cursors.Default;
            if (MemberMouseLeave != null)
            {
                this.BeginInvoke(MemberMouseLeave, new object[] { sender, member });
            }
            if (IsSelected)
            {
                this.BackColor = Color.FromArgb(235, 183, 52);
            }
            else
            {
                this.BackColor = memberinfo.IsDeath ? Color.LightGray : Color.White;
            }
        }
        private void MouseMove_Event(object sender, MouseEventArgs e)
        {
            MousePosition = e.Location;
            if (!bMouseHover)
            {
                MemberMouseHover_Event(sender, GetMember());
            }
        }

        #endregion
        bool bMouseHover = false;
        Rectangle RectGender = new Rectangle(194, 50, 32, 32);
        Point MousePosition = new Point(0, 0);
        SolidBrush brush = new SolidBrush(Color.Black);
        Font font = materialSkinmanager.getFontByType(MaterialSkinManager.fontType.Subtitle1);
        string rank;
        string name;
        string birtdaySun;
        string deadday;
        Bitmap bmpRoot = null;
        Bitmap bmpLeader = null;
        Bitmap bmpGender = null;

        public ExTMember memberinfo;

        public UserMember(ExTMember member, string RootID) : base()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);
            _contextMember = ContextMenuStripManager.CreateForMember();
            memberinfo = member;
            if (memberinfo.Id == RootID)
            {
                bmpRoot = Resources.root;
            }
            this.Size = new Size(230, _Heigh);
            this.BackColor = memberinfo.IsDeath ? Color.LightGray : Color.White;
            rank = "Đời: " + memberinfo.ChildLevelInFamily;
            name = memberinfo.Name;
            birtdaySun = "Ngày Sinh: " + memberinfo.BirthdayShow;
            deadday = "Ngày Mất: " + memberinfo.DeadDaySunShow;// chuyen sang enum
            switch (memberinfo.Gender)
            {
                case (int)GenderMember.Male: bmpGender = Resources.male2; break;
                case (int)GenderMember.Female: bmpGender = Resources.female32; break;
                case (int)GenderMember.Unknown: bmpGender = Resources.unknown; break;
            }

            this.MouseHover += (sender, e) => { MemberMouseHover_Event(sender, GetMember()); };
            this.MouseLeave += (sender, e) => { MemberMouseLeave_Event(sender, GetMember()); };
            this.MouseClick += (sender, e) => { MemberMouseClick_Event(sender, e, GetMember()); };
            this.DoubleClick += (sender, e) => { MemberSelect_Event(sender, GetMember()); };
            this.MouseMove += MouseMove_Event;
            Paint += new PaintEventHandler(paintControl);
        }
        private ExTMember GetMember()
        {
            return memberinfo;
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.BackColor = IsSelected ? Color.FromArgb(235, 183, 52) : memberinfo.IsDeath ? Color.LightGray : Color.White;
            }
        }
        public static GraphicsPath CreateRoundRect(float x, float y, float width, float height, float radius)
        {
            var gp = new GraphicsPath();
            gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90);
            gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp.AddArc(x, y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            return gp;
        }
        public static GraphicsPath CreateRoundRect(RectangleF rect, float radius)
        {
            return CreateRoundRect(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }
        private void paintControl(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent.BackColor);

            // card rectangle path
            //RectangleF cardRectF = new RectangleF(ClientRectangle.Location, ClientRectangle.Size);
            //cardRectF.X -= 0.5f;
            //cardRectF.Y -= 0.5f;
            //GraphicsPath cardPath = CreateRoundRect(cardRectF, 6);
            // Draw card
            //using (SolidBrush normalBrush = new SolidBrush(BackColor))
            //{
            //    g.FillPath(normalBrush, cardPath);
            //}
            g.DrawString(name, font, brush, new PointF(3, 3));
            g.DrawString(rank, font, brush, new PointF(3, 23));
            g.DrawString(birtdaySun, font, brush, new PointF(3, 43));
            g.DrawString(deadday, font, brush, new PointF(3, 63));

            if (bmpLeader != null)
            {
                g.DrawImage(bmpLeader, new Rectangle(185, 5, 16, 16));
            }

            if (bmpRoot != null)
            {
                g.DrawImage(bmpRoot, new Rectangle(210, 5, 16, 16));
            }

            if (bmpGender != null)
            {
                g.DrawImage(bmpGender, new Rectangle(194, 50, 32, 32));
            }
        }
    }

    public class InfoFilter
    {
        private string _Gender = "";
        private string _LiveorDie = "";
        private string _KeyWord = "";
        public int Page { get; set; }
        public Button ButtonSelect { get; set; }
        public string Gender { get => _Gender; set => _Gender = value; }
        public string LiveorDie { get => _LiveorDie; set => _LiveorDie = value; }
        public string KeyWord { get => _KeyWord; set => _KeyWord = value; }
    }
}
