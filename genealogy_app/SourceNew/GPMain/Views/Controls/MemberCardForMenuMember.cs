using GP40Common;
using GPConst;
using GPMain.Common.Helper;
using GPMain.Properties;
using GPModels;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPMain.Views.Controls
{
    /// <summary>
    /// Memo: Template Member Card for Menu Member
    /// Create by Nguyễn Văn Hải
    /// </summary>
    public class MemberCardForMenuMember : MaterialSkin.Controls.MaterialCard
    {
        private ContextMenuStripData<TMember> _contextMember;

        private static MaterialSkinManager materialSkinmanager = MaterialSkinManager.Instance;

        private int UserHeigh = AppManager.MenuMemberBuffer.UserMemberHeight;
        private int UserWidth = AppManager.MenuMemberBuffer.UserMemberWidth;

        #region Public Event

        public event EventHandler<ExTMember> MemberMouseClicked;
        public event EventHandler<TMember> RequestReload;

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
                var objMember = AppManager.MenuMemberBuffer.ListAllMember[MemberInfo.Id];
                if (objMember == null)
                {
                    objMember = MemberInfo;
                }
                using (MemberHelper memberHelper = new MemberHelper())
                {
                    var rootMember = memberHelper.RootTMember;
                    bool isRootMember = false;
                    if (rootMember != null)
                    {
                        isRootMember = rootMember.Id == member.Id;
                    }
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrAddRootFamilyMember, (rootMember == null || objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveRootFamilyMember, (objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrAddFamilyHead, (objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveFamilyHead, (objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                    _contextMember.Show(UIHelper.GetCursorPosition(), objMember);
                }
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
            if (RectGender.Contains(mousePosition))
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
                this.BackColor = MemberInfo.IsDeath ? Color.LightGray : Color.White;
            }
        }

        private void MouseMove_Event(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            if (!bMouseHover)
            {
                MemberMouseHover_Event(sender, GetMember());
            }
        }

        #endregion Public Event

        private bool bMouseHover = false;
        private Rectangle RectGender = new Rectangle(194, 50, 32, 32);
        private Point mousePosition = new Point(0, 0);
        private SolidBrush brush = new SolidBrush(Color.Black);
        private Font font = materialSkinmanager.getFontByType(MaterialSkinManager.fontType.Body1);
        private string rank;
        public string name;
        private string birtdaySun;
        private string deadday;
        private Bitmap bmpRoot = null;
        private Bitmap bmpLeader = null;
        private Bitmap bmpGender = null;

        private ExTMember _MemberInfo;

        public MemberCardForMenuMember(ExTMember member, string RootID, bool FamilyHead) : base()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);
            _contextMember = ContextMenuStripManager.CreateForMember();
            _contextMember.ItemClickEnd += (sender, mem) =>
            {
                if (RequestReload != null)
                {
                    RequestReload?.Invoke(sender, mem);
                }
            };
            MemberInfo = member;
            if (MemberInfo.Id == RootID)
            {
                bmpRoot = Resources.root;
            }
            if (FamilyHead)
            {
                bmpLeader = Resources.familyHead;
            }
            this.Size = new Size(UserWidth, UserHeigh);
            this.BackColor = MemberInfo.IsDeath ? Color.LightGray : Color.White;
            rank = $"Đời: {(MemberInfo.LevelInFamily >= 0 ? MemberInfo.LevelInFamilyForShow : "")}";
            name = MemberInfo.Name;
            birtdaySun = $"Ngày Sinh: {MemberInfo.BirthdayShow}";
            deadday = $"Ngày Mất: {(MemberInfo.IsDeath ? MemberInfo.DeadDayLunarShow : "")}";

            switch (MemberInfo.Gender)
            {
                case (int)EmGender.Male: bmpGender = Resources.male2; break;
                case (int)EmGender.FeMale: bmpGender = Resources.female2; break;
                case (int)EmGender.Unknown: bmpGender = Resources.unknown; break;
            }

            this.MouseHover += (sender, e) => { MemberMouseHover_Event(sender, GetMember()); };
            this.MouseLeave += (sender, e) => { MemberMouseLeave_Event(sender, GetMember()); };
            this.MouseClick += (sender, e) => { MemberMouseClick_Event(sender, e, GetMember()); };
            this.DoubleClick += (sender, e) => { MemberSelect_Event(sender, GetMember()); };
            this.MouseMove += MouseMove_Event;
            Paint += new PaintEventHandler(paintControl);
            this.Margin = new Padding(5);
        }

        private ExTMember GetMember()
        {
            return MemberInfo;
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.BackColor = IsSelected ? Color.FromArgb(235, 183, 52) : MemberInfo.IsDeath ? Color.LightGray : Color.White;
            }
        }

        public ExTMember MemberInfo { get => _MemberInfo; set => _MemberInfo = value; }

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

            g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.Clear(Parent.BackColor);

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
                g.DrawImage(bmpLeader, new Rectangle(UserWidth - 20, 25, 16, 16));
            }

            if (bmpRoot != null)
            {
                g.DrawImage(bmpRoot, new Rectangle(UserWidth - 20, 5, 16, 16));
            }

            if (bmpGender != null)
            {
                g.DrawImage(bmpGender, new Rectangle(UserWidth - 32, 50, 32, 32));
            }
        }
    }
}
