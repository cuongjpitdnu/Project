using GPMain.Common;
using GPModels;
using System;
using System.Collections.Generic;
using GPMain.Properties;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPMain.Views.Member;
using GPMain.Common.Helper;
using GPConst;

namespace GPMain.Views.Tree.Build
{
    /// <summary>
    /// Meno: Draw Build Tree
    /// Create by: Nguyễn Văn Hải
    /// </summary>
    public class BuildTree : Panel
    {
        public event EventHandler<ParamRequest> MemberEdit;
        public event EventHandler<ExTMember> MemberSelect;
        public event EventHandler<ExTMember> DeleteRelationComplete;
        public event EventHandler<ExTMember> ReloadMember;

        private int _CenterX { get; set; }
        private int _CenterY { get; set; }
        private int _SpaceX { get; set; } = 10;
        private int _SpaceY { get; set; } = 50;
        private int _SpaceChildWifeY { get; set; } = 10;
        private ExTMember member { get; set; }
        public MemberCardForBuildTree cardMain { get; set; }
        public MemberCardForBuildTree cardSpouse { get; set; }
        public MemberCardForBuildTree cardFather { get; set; }
        public MemberCardForBuildTree cardMother { get; set; }

        private MemberCardForBuildTree _cardSelect;
        private MemberCardForBuildTree cardSelect
        {
            get { return _cardSelect; }
            set
            {
                _cardSelect = value;
                btnCancelRelationship.Visible = !((_cardSelect.Member.ListSPOUSE == null || _cardSelect.Member.ListSPOUSE.Count == 0) && (_cardSelect.Member.ListCHILDREN == null || _cardSelect.Member.ListCHILDREN.Count == 0));
            }
        }

        private Panel pnlSpouse;
        private Panel pnlMainMember;

        Label lblinfospouse;
        Label lbllistspouse;
        Button btnaddSpouse;
        FlowLayoutPanel flpSpouse;

        Label lblrelationship;
        Label Relationship;
        Label lblnumberChild;
        Label lbllistChild;
        Panel pnlContainerChild;
        FlowLayoutPanel flpChild;
        Button btnaddChild;
        Button btnCancelRelationship;

        Button btnlistSpouse;

        private Panel upBorderMemberSelect;
        private Panel downBorderMemberSelect;
        private Panel leftBorderMemberSelect;
        private Panel rightBorderMemberSelect;

        private Color BorderColor = Color.Red;
        private Color ColorLeave = Color.Blue;
        private Color ColorHover = Color.Red;
        private int FontSize = 9;
        private int NumChild = 0;
        private Pen pen = new Pen(Color.Blue, 1);
        FontFamily fontFamily;
        Font font;
        ToolTip toolTipDeleteAllRelation;
        string infoToolTip = "";

        float flpSpouseItemHeight = 0;
        float flpSpouseMaxHeight = 0;
        int flpSpouseMinHeight = 50;
        float flpSpouseHeight = 0;

        MemberHelper memberHelper = new MemberHelper();
        MemberRelationHelper memberRelation = new MemberRelationHelper();
        public BuildTree(ExTMember objMember = null) : base()
        {
            DoubleBuffered = true;
            fontFamily = new FontFamily(AppConst.FontName);
            Control[] controls = new Control[11];

            toolTipDeleteAllRelation = new ToolTip()
            {
                IsBalloon = true
            };

            this.SuspendLayout();
            font = new Font(fontFamily, FontSize, FontStyle.Regular);

            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            member = objMember == null ? new ExTMember() { Name = AppConst.NameDefaul.Other } : objMember;
            cardMain = new MemberCardForBuildTree();
            cardFather = new MemberCardForBuildTree(null, 0.5f);
            cardMother = new MemberCardForBuildTree(null, 0.5f);
            cardSpouse = new MemberCardForBuildTree();
            InitPanelListChildren();

            InitPanelSpouse();

            InitPanelMaineMember();

            InitSelectRectangle();

            _CenterX = this.Width / 2;
            _CenterY = cardMain.Height / 2 + cardFather.Height + _SpaceY + 10;

            InitEventMemberCard();

            flpSpouseMaxHeight = (this.Height - pnlContainerChild.Height - 20 - _SpaceChildWifeY - pnlSpouse.Location.Y) - flpSpouse.Location.Y - btnaddSpouse.Height - 1 - pnlSpouse.Padding.Bottom * 2;

            controls[0] = cardMain;
            controls[1] = cardSpouse;
            controls[2] = cardFather;
            controls[3] = cardMother;
            controls[4] = upBorderMemberSelect;
            controls[5] = downBorderMemberSelect;
            controls[6] = leftBorderMemberSelect;
            controls[7] = rightBorderMemberSelect;
            controls[8] = pnlMainMember;
            controls[9] = pnlSpouse;
            controls[10] = pnlContainerChild;

            this.Controls.AddRange(controls);
            this.ResumeLayout();
            FormLoad();

        }
        private void FormLoad()
        {
            Thread.Sleep(50);
            this.Paint += OnPaint;
            this.SizeChanged += SizeChanged_Event;
        }

        //Khởi tạo panel danh sách con
        private void InitPanelListChildren()
        {
            pnlContainerChild = new Panel();
            flpChild = new FlowLayoutPanel();
            flpChild.SuspendLayout();
            pnlContainerChild.SuspendLayout();

            pnlContainerChild.LocationChanged += (sender, e) =>
            {
                //pnlSpouse.Size = new Size(pnlSpouse.Width, pnlContainerChild.Location.Y - pnlSpouse.Location.Y - _SpaceChildWifeY);
                //pnlMainMember.Size = new Size(pnlMainMember.Width, pnlSpouse.Height);
            };

            flpChild.AutoSize = true;
            flpChild.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpChild.MinimumSize = new Size(100, 100);
            flpChild.FlowDirection = FlowDirection.LeftToRight;
            flpChild.WrapContents = false;

            pnlContainerChild.Size = new Size(800, 120);
            pnlContainerChild.BackColor = Color.Transparent;
            pnlContainerChild.BorderStyle = BorderStyle.FixedSingle;
            pnlContainerChild.ForeColor = Color.LightGray;
            pnlContainerChild.AutoScroll = true;
            pnlContainerChild.Controls.Add(flpChild);

            pnlContainerChild.LocationChanged += (sender, e) =>
            {
                pnlSpouse.Size = new Size(pnlSpouse.Width, pnlContainerChild.Location.Y - pnlSpouse.Location.Y - _SpaceChildWifeY);
                pnlMainMember.Size = new Size(pnlMainMember.Width, pnlSpouse.Height);
            };

            flpChild.ResumeLayout();
            pnlContainerChild.ResumeLayout();
            pnlContainerChild.Visible = false;
        }

        //Khởi tạo Panel thông tin vợ/ chồng của thành viên chính
        private void InitPanelSpouse()
        {
            pnlSpouse = new Panel();
            lblinfospouse = new Label();
            lbllistspouse = new Label();
            flpSpouse = new FlowLayoutPanel();
            btnaddSpouse = new Button();
            btnlistSpouse = new Button();

            pnlSpouse.SuspendLayout();
            pnlSpouse.Size = new Size(MemberCardForBuildTree.WidthSize, 150);
            pnlSpouse.BorderStyle = BorderStyle.FixedSingle;
            pnlSpouse.BackColor = Color.Transparent;

            lblinfospouse.AutoSize = true;
            lblinfospouse.Font = font;
            lblinfospouse.ForeColor = Color.Black;
            lblinfospouse.Location = new Point(lblinfospouse.Location.X, 5);

            btnaddSpouse.Size = new Size(150, 40);
            btnaddSpouse.FlatStyle = FlatStyle.Flat;
            btnaddSpouse.Location = new Point(1, pnlSpouse.Height - btnaddSpouse.Height);
            btnaddSpouse.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnaddSpouse.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnaddSpouse.Padding = new Padding(0);
            btnaddSpouse.Font = new Font(fontFamily, 12, FontStyle.Regular);
            btnaddSpouse.ForeColor = ColorLeave;
            btnaddSpouse.Image = Resources.add_user;
            btnaddSpouse.TextAlign = ContentAlignment.MiddleLeft;
            btnaddSpouse.ImageAlign = ContentAlignment.MiddleLeft;
            btnaddSpouse.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnaddSpouse.Padding = new Padding(0);
            btnaddSpouse.Cursor = Cursors.Hand;
            btnaddSpouse.MouseHover += (sender, e) =>
            {
                btnaddSpouse.Image = Resources.add_user2;
                btnaddSpouse.ForeColor = Color.Red;
            };
            btnaddSpouse.MouseLeave += (sender, e) =>
            {
                btnaddSpouse.Image = Resources.add_user;
                btnaddSpouse.ForeColor = Color.Blue;
            };
            btnaddSpouse.Click += (sender, e) =>
            {
                ExTMember exTMember = new ExTMember();
                exTMember.Name = AppConst.NameDefaul.Husban;
                if (cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male))
                {
                    exTMember.Name = AppConst.NameDefaul.Wife;
                }
                MemberEdit_Event(sender, exTMember);
            };

            flpSpouse.WrapContents = false;
            flpSpouse.Location = new Point(flpSpouse.Location.X, lblinfospouse.Location.Y + 20);
            flpSpouse.Size = new Size(cardMain.Width - 30, btnaddSpouse.Location.Y - flpSpouse.Location.Y - 1);
            flpSpouse.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            flpSpouse.AutoScroll = true;
            flpSpouse.Padding = new Padding(10, 0, 0, 5);
            flpSpouse.FlowDirection = FlowDirection.TopDown;
            flpSpouse.WrapContents = false;

            btnlistSpouse.Size = new Size(32, 32);
            btnlistSpouse.FlatStyle = FlatStyle.Flat;
            btnlistSpouse.FlatAppearance.BorderSize = 0;
            btnlistSpouse.Location = new Point(pnlSpouse.Width - btnlistSpouse.Width - 5, pnlSpouse.Height - btnlistSpouse.Height - 1);
            btnlistSpouse.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnlistSpouse.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnlistSpouse.BackColor = Color.Transparent;
            btnlistSpouse.Cursor = Cursors.Hand;
            btnlistSpouse.BackgroundImage = Resources.list3;
            btnlistSpouse.BackgroundImageLayout = ImageLayout.Zoom;
            btnlistSpouse.MouseHover += (sender, e) =>
            {
                btnlistSpouse.BackgroundImage = Resources.listcheck;
            };
            btnlistSpouse.MouseLeave += (sender, e) =>
            {
                btnlistSpouse.BackgroundImage = Resources.list3;
            };
            btnlistSpouse.Click += (sender, e) =>
            {
                string gender = AppConst.Gender.Male;
                if (cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male))
                {
                    gender = AppConst.Gender.Female;
                }
                if (AppManager.Navigation.ShowDialogWithParam<ListMemberRelation, RelationParam>(new RelationParam() { Member = cardSelect.Member, RelationFilter = cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male) ? GPConst.Relation.PREFIX_WIFE : GPConst.Relation.PREFIX_HUSBAND, Gender = gender }, ModeForm.None, new StatusBarColor() { BackColor = AppConst.StatusBarBackColor, ForeColor = AppConst.StatusBarForeColor }).Result == DialogResult.OK)
                    RequestReloadListMember(sender, cardSelect.Member);
            };

            lbllistspouse.AutoSize = true;
            lbllistspouse.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lbllistspouse.Font = font;
            lbllistspouse.ForeColor = ColorLeave;
            lbllistspouse.Text = "Danh sách";
            lbllistspouse.Cursor = Cursors.Hand;
            lbllistspouse.Location = new Point(btnlistSpouse.Location.X - 60, btnlistSpouse.Location.Y + btnlistSpouse.Height - 15);
            lbllistspouse.MouseHover += (sender, e) =>
            {
                lbllistspouse.ForeColor = ColorHover;
            };
            lbllistspouse.MouseLeave += (sender, e) =>
            {
                lbllistspouse.ForeColor = ColorLeave;
            };
            lbllistspouse.Click += (sender, e) =>
            {
                if (AppManager.Navigation.ShowDialogWithParam<ListMemberRelation, RelationParam>(new RelationParam() { Member = cardSelect.Member, RelationFilter = cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male) ? GPConst.Relation.PREFIX_WIFE : GPConst.Relation.PREFIX_HUSBAND }, ModeForm.None, new StatusBarColor() { BackColor = AppConst.StatusBarBackColor, ForeColor = AppConst.StatusBarForeColor }).Result == DialogResult.OK)
                    RequestReloadListMember(sender, cardSelect.Member);
            };

            Control[] listControlPanelSpouse = new Control[5];
            listControlPanelSpouse[0] = lblinfospouse;
            listControlPanelSpouse[1] = lbllistspouse;
            listControlPanelSpouse[2] = flpSpouse;
            listControlPanelSpouse[3] = btnaddSpouse;
            listControlPanelSpouse[4] = btnlistSpouse;
            pnlSpouse.Controls.AddRange(listControlPanelSpouse);
            pnlSpouse.ResumeLayout();
            pnlSpouse.Padding = new Padding(3);
        }
        //Khỏi tạo Panel thông tin thành viên chính
        private void InitPanelMaineMember()
        {
            pnlMainMember = new Panel();
            lblrelationship = new Label();
            Relationship = new Label();
            lblnumberChild = new Label();
            lbllistChild = new Label();
            btnaddChild = new Button();
            btnCancelRelationship = new Button();

            pnlMainMember.SuspendLayout();
            pnlMainMember.Size = new Size(MemberCardForBuildTree.WidthSize, 150);
            pnlMainMember.BackColor = Color.Transparent;
            pnlMainMember.BorderStyle = BorderStyle.FixedSingle;

            lblrelationship.AutoSize = true;
            lblrelationship.Font = font;
            lblrelationship.ForeColor = Color.Black;
            lblrelationship.Text = "Tình trạng hôn nhân :";
            lblrelationship.Location = new Point(lblrelationship.Location.X, 5);

            Relationship.Font = font;
            Relationship.ForeColor = Color.Black;
            Relationship.Location = new Point(lblrelationship.Location.X + lblrelationship.Width + 30, 8);

            lblnumberChild.AutoSize = true;
            lblnumberChild.BackColor = Color.Transparent;
            lblnumberChild.Font = font;
            lblnumberChild.ForeColor = Color.Black;
            lblnumberChild.Location = new Point(lblnumberChild.Location.X, lblrelationship.Location.Y + 20);

            lbllistChild.AutoSize = true;
            lbllistChild.BackColor = Color.Transparent;
            lbllistChild.Font = new Font(fontFamily, 10, FontStyle.Italic);
            lbllistChild.ForeColor = ColorLeave;
            lbllistChild.Text = "Danh sách các con.";
            lbllistChild.Cursor = Cursors.Hand;
            lbllistChild.Visible = NumChild > 0;
            lbllistChild.Location = new Point(lblnumberChild.Location.X + 100, lblnumberChild.Location.Y);
            lbllistChild.MouseHover += (sender, e) =>
            {
                lbllistChild.ForeColor = ColorHover;
                lbllistChild.Font = new Font(lbllistChild.Font.Name, lbllistChild.Font.SizeInPoints, FontStyle.Underline | FontStyle.Italic);
            };
            lbllistChild.MouseLeave += (sender, e) =>
            {
                lbllistChild.ForeColor = ColorLeave;
                lbllistChild.Font = new Font(lbllistChild.Font.Name, lbllistChild.Font.SizeInPoints, FontStyle.Italic);
            };
            lbllistChild.Click += (sender, e) =>
            {
                if (AppManager.Navigation.ShowDialogWithParam<ListMemberRelation, RelationParam>(new RelationParam() { Member = cardSelect.Member, RelationFilter = GPConst.Relation.PREFIX_CHILD }, ModeForm.None, new StatusBarColor() { BackColor = AppConst.StatusBarBackColor, ForeColor = AppConst.StatusBarForeColor }).Result == DialogResult.OK)
                    RequestReloadListMember(sender, cardSelect.Member);
            };

            btnaddChild.Size = new Size(150, 40);
            btnaddChild.FlatStyle = FlatStyle.Flat;
            btnaddChild.Location = new Point(btnaddChild.Location.X + 1, pnlMainMember.Height - btnaddChild.Height);
            btnaddChild.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnaddChild.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnaddChild.Font = new Font(fontFamily, 12, FontStyle.Regular);
            btnaddChild.ForeColor = ColorLeave;
            btnaddChild.Text = "Thêm con";
            btnaddChild.Cursor = Cursors.Hand;
            btnaddChild.Image = Resources.add_user;
            btnaddChild.ImageAlign = ContentAlignment.MiddleLeft;
            btnaddChild.TextAlign = ContentAlignment.MiddleLeft;
            btnaddChild.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnaddChild.MouseHover += (sender, e) =>
            {
                btnaddChild.Image = Resources.add_user2;
                btnaddChild.ForeColor = Color.Red;
            };
            btnaddChild.MouseLeave += (sender, e) =>
            {
                btnaddChild.Image = Resources.add_user;
                btnaddChild.ForeColor = Color.Blue;
            };

            btnaddChild.Click += (sender, e) =>
            {
                ExTMember exTMember = new ExTMember();
                exTMember.Name = AppConst.NameDefaul.Child;
                MemberEdit_Event(sender, exTMember);
            };


            btnCancelRelationship.Size = new Size(24, 24);
            btnCancelRelationship.Cursor = Cursors.Hand;
            btnCancelRelationship.BackgroundImage = Resources.cancel;
            btnCancelRelationship.BackgroundImageLayout = ImageLayout.Stretch;
            btnCancelRelationship.Left = pnlMainMember.Width - btnCancelRelationship.Width - 5;
            btnCancelRelationship.Top = 3;
            btnCancelRelationship.Click += DeleteRelation;

            Control[] listControlPanelPersion = new Control[6];
            listControlPanelPersion[0] = lblrelationship;
            listControlPanelPersion[1] = Relationship;
            listControlPanelPersion[2] = lblnumberChild;
            listControlPanelPersion[3] = lbllistChild;
            listControlPanelPersion[4] = btnaddChild;
            listControlPanelPersion[5] = btnCancelRelationship;
            pnlMainMember.Controls.AddRange(listControlPanelPersion);
            Application.DoEvents();
            pnlMainMember.ResumeLayout();
            pnlMainMember.Padding = new Padding(3);
        }
        //Khởi tạo border chọn thẻ
        private void InitSelectRectangle()
        {
            upBorderMemberSelect = new Panel()
            {
                Size = new Size(cardMain.Width + 8, 2),
                BackColor = BorderColor
            };
            downBorderMemberSelect = new Panel()
            {
                Size = new Size(cardMain.Width + 8, 2),
                BackColor = BorderColor
            };
            leftBorderMemberSelect = new Panel()
            {
                Size = new Size(2, cardMain.Height + 6),
                BackColor = BorderColor
            };
            rightBorderMemberSelect = new Panel()
            {
                Size = new Size(2, cardMain.Height + 6),
                BackColor = BorderColor
            };

            upBorderMemberSelect.BringToFront();
            downBorderMemberSelect.BringToFront();
            leftBorderMemberSelect.BringToFront();
            rightBorderMemberSelect.BringToFront();
        }
        //Khởi tạo sự kiện cho các thẻ thành viên
        private void InitEventMemberCard()
        {
            cardMain.EditInfoMember += MemberEdit_Event;
            cardFather.EditInfoMember += MemberEdit_Event;
            cardMother.EditInfoMember += MemberEdit_Event;
            cardSpouse.EditInfoMember += MemberEdit_Event;

            cardMain.RequestReload += ContextClickEnd_Event;
            cardFather.RequestReload += ContextClickEnd_Event;
            cardMother.RequestReload += ContextClickEnd_Event;
            cardSpouse.RequestReload += ContextClickEnd_Event;

            cardMain.SelectMember += (sender, e) =>
            {
                if (cardSelect != cardMain)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        cardSelect = cardMain;

                        pnlMainMember.Location = new Point(cardMain.Location.X, pnlMainMember.Location.Y);
                        pnlSpouse.Location = new Point(cardSpouse.Location.X, pnlSpouse.Location.Y);
                        UpdateChild(cardSelect.Member);
                        UpdateSpouse(cardSelect.Member);
                        SetPositionBorderMemberSelect();
                        memberHelper.GetParent(cardSelect.Member, out ExTMember father, out ExTMember mother);
                        cardFather.Member = father;
                        cardMother.Member = mother;
                        SetMessageDeleteRelation();
                        this.Refresh();
                    }));
                }
            };
            cardFather.SelectMember += MemberSelect_Event;
            cardMother.SelectMember += MemberSelect_Event;
            cardSpouse.SelectMember += (sender, e) =>
            {
                if (cardSelect != cardSpouse)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        cardSelect = cardSpouse;

                        pnlSpouse.Location = new Point(cardMain.Location.X, pnlMainMember.Location.Y);
                        pnlMainMember.Location = new Point(cardSpouse.Location.X, pnlSpouse.Location.Y);
                        UpdateChild(cardSelect.Member);
                        UpdateSpouse(cardSelect.Member);
                        SetPositionBorderMemberSelect();
                        memberHelper.GetParent(cardSelect.Member, out ExTMember father, out ExTMember mother);
                        cardFather.Member = father;
                        cardMother.Member = mother;
                        SetMessageDeleteRelation();
                        this.Refresh();
                    }));
                }
            };
        }

        //private void ShowListSpouse_Event(object sender, EventArgs e)
        //{
        //    string gender = AppConst.Gender.Male;
        //    if (cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male))
        //    {
        //        gender = AppConst.Gender.Female;
        //    }
        //    DialogResult dialogResult = AppManager.Navigation.ShowDialogWithParam<ListMemberRelation, RelationParam>(
        //       new RelationParam()
        //       {
        //           Member = cardSelect.Member,
        //           RelationFilter = cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male) ? GPConst.Relation.PREFIX_WIFE : GPConst.Relation.PREFIX_HUSBAND,
        //           Gender = gender
        //       },
        //       ModeForm.None,
        //       new StatusBarColor() { BackColor = AppConst.StatusBarBackColor, ForeColor = AppConst.StatusBarForeColor }).Result;
        //    if (dialogResult == DialogResult.OK)
        //    {
        //        RequestReloadListMember(sender, AppManager.MenuMemberBuffer.MemberCurrent);
        //    }
        //}

        //Sự kiện cho menu context
        private void ContextClickEnd_Event(object sender, TMember member)
        {
            if (member != null)
            {
                RequestReloadListMember(sender, memberHelper.TMemberToExTMember(member));
            }
        }

        //Sự kiện yêu cầu reload
        private void RequestReloadListMember(object sender, ExTMember mem)
        {
            if (ReloadMember != null)
            {
                ReloadMember?.Invoke(sender, mem);
            }
        }

        //Sự kiện cho nút xóa toàn bộ quan hệ
        private void DeleteRelation(object sender, EventArgs e)
        {
            string mess = infoToolTip + " sẽ bị hủy.\nBạn có chắc chắn thực hiện thao tác này?";
            if (AppManager.Dialog.Confirm(mess))
            {
                if (memberRelation.DeleteAllRelation(cardSelect.Member) && DeleteRelationComplete != null)
                {
                    DeleteRelationComplete?.Invoke(sender, cardSelect.Member);
                }
            }
        }

        private void SetMessageDeleteRelation()
        {
            cardSelect = cardSelect ?? cardMain;
            if ((cardSelect.Member.ListSPOUSE == null || cardSelect.Member.ListSPOUSE.Count == 0) && (cardSelect.Member.ListCHILDREN == null || cardSelect.Member.ListCHILDREN.Count == 0)) return;
            string info = $"Quan hệ vợ chồng giữa {cardSelect.Member.Name} ";
            if (!(cardSelect.Member.ListSPOUSE == null || cardSelect.Member.ListSPOUSE.Count == 0))
            {
                info += $"và {(cardSelect == cardMain ? cardSpouse.Member.Name : cardMain.Member.Name)} ";
            }
            if (!(cardSelect.Member.ListCHILDREN == null || cardSelect.Member.ListCHILDREN.Count == 0))
            {
                info += "và các con";
            }

            infoToolTip = info;
            toolTipDeleteAllRelation.SetToolTip(btnCancelRelationship, $"Hủy {infoToolTip.ToLower()}");
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            this.SuspendLayout();
            Graphics grp = e.Graphics;
            if (cardSelect == null)
            {
                cardSelect = cardMain;
            }

            PointF pp11 = new PointF(cardFather.Location.X + cardFather.Width / 2, cardFather.Location.Y + cardFather.Height);
            PointF pp21 = new PointF(cardMother.Location.X + cardMother.Width / 2, cardMother.Location.Y + cardMother.Height);
            PointF pp31 = new PointF(cardSelect.Location.X + cardSelect.Width / 2, cardSelect.Location.Y);
            PointF pp12 = new PointF(pp11.X, pp11.Y + (pp31.Y - pp11.Y) / 3);
            PointF pp22 = new PointF(pp21.X, pp21.Y + (pp31.Y - pp21.Y) / 3);
            PointF pp32 = new PointF(pp31.X, pp31.Y - (pp31.Y - pp11.Y) / 3);
            PointF ppMid1 = new PointF(pp12.X + (pp22.X - pp12.X) / 2, pp12.Y);
            PointF ppMid2 = new PointF(ppMid1.X, ppMid1.Y + (pp31.Y - pp11.Y) / 3);

            grp.DrawLines(pen, new PointF[6] { pp11, pp12, ppMid1, ppMid2, pp32, pp31 });
            grp.DrawLines(pen, new PointF[3] { pp21, pp22, ppMid1 });
            this.ResumeLayout();
        }

        //Khởi tạo vị trí của các thẻ thành viên
        public void CreatePositionCard()
        {
            cardMain.Location = new Point(_CenterX - cardMain.Width - _SpaceX, _CenterY - cardMain.Height / 2);
            cardSpouse.Location = new Point(_CenterX + _SpaceX, cardMain.Location.Y);
            cardFather.Location = new Point(_CenterX - cardFather.Width - _SpaceX, cardMain.Location.Y - cardFather.Height - _SpaceY);
            cardMother.Location = new Point(_CenterX + _SpaceX, cardFather.Location.Y);
            if (cardSelect == null)
            {
                cardSelect = cardMain;
            }
            pnlMainMember.Location = new Point(cardSelect.Location.X, cardSelect.Location.Y + cardSelect.Height + 20);
            pnlSpouse.Location = new Point(cardSelect.Member.GenderShow == null || cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male) ? cardSpouse.Location.X : cardMain.Location.X, pnlMainMember.Location.Y);
            pnlContainerChild.Location = new Point(cardMain.Location.X, pnlMainMember.Location.Y + pnlMainMember.Height + 20);
            pnlContainerChild.Size = new Size(cardSpouse.Location.X + cardSpouse.Width - cardMain.Location.X, pnlContainerChild.Height);

            SetPositionBorderMemberSelect();
        }
        //Cập nhật thông tin cho các thẻ
        public void UpdateInfo(ExTMember tMember, bool reload = false)
        {
            ExTMember exTMember = memberHelper.GetExTMemberByID(tMember == null ? "" : (tMember.Id ?? ""));

            if (exTMember == null)
            {
                exTMember = new ExTMember() { Name = "Thêm thành viên" };
            }

            try
            {
                ExTMember memberSpouse = null;
                if (exTMember.ListSPOUSE == null || exTMember.ListSPOUSE.Count == 0)
                {
                    memberSpouse = new ExTMember() { Name = "Thêm thành viên" };
                }
                else
                {
                    memberSpouse = memberHelper.GetExTMemberByID(exTMember.ListSPOUSE[0]);
                }

                if (exTMember.GenderShow == null || exTMember.GenderShow.Equals(AppConst.Gender.Male))
                {
                    member = exTMember;
                    cardSelect = cardMain;
                    pnlMainMember.Location = new Point(cardMain.Location.X, pnlMainMember.Location.Y);
                    pnlSpouse.Location = new Point(cardSpouse.Location.X, pnlSpouse.Location.Y);
                }
                else
                {
                    member = memberSpouse;
                    memberSpouse = exTMember;
                    cardSelect = cardSpouse;
                    pnlSpouse.Location = new Point(cardMain.Location.X, pnlMainMember.Location.Y);
                    pnlMainMember.Location = new Point(cardSpouse.Location.X, pnlSpouse.Location.Y);
                }

                memberHelper.GetParent(cardSelect == cardMain ? member : memberSpouse, out ExTMember memberFather, out ExTMember memberMother);

                member.Name = (typeof(AppConst.NameDefaul)).GetFields().Select(v => v.GetValue(v.Name)).ToArray().Contains(member.Name) ? AppConst.NameDefaul.Husban : member.Name;
                memberSpouse.Name = (typeof(AppConst.NameDefaul)).GetFields().Select(v => v.GetValue(v.Name)).ToArray().Contains(memberSpouse.Name) ? AppConst.NameDefaul.Wife : memberSpouse.Name;
                cardFather.Member = memberFather;
                cardMother.Member = memberMother;
                cardMain.Member = member;
                cardSpouse.Member = memberSpouse;

                if (cardSelect.Member.GenderShow == null || cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male))
                {
                    pnlMainMember.Visible = member.Name != AppConst.NameDefaul.Husban;
                    pnlSpouse.Visible = memberSpouse.Name != AppConst.NameDefaul.Wife;
                }
                else
                {
                    pnlSpouse.Visible = member.Name != AppConst.NameDefaul.Husban;
                    pnlMainMember.Visible = memberSpouse.Name != AppConst.NameDefaul.Wife;
                }

                ExTMember memberChild = null;

                if (cardSelect.Member.ListCHILDREN == null || cardSelect.Member.ListCHILDREN.Count == 0)
                {
                    memberChild = new ExTMember() { Name = AppConst.NameDefaul.Child };
                }
                else
                {
                    memberChild = memberHelper.GetExTMemberByID(cardSelect.Member.ListCHILDREN[0]);
                }
                UpdateChild(cardSelect.Member);
                UpdateSpouse(cardSelect.Member);
                SetPositionBorderMemberSelect();
                this.Refresh();
            }
            catch (Exception ex) { }
            SetMessageDeleteRelation();
        }
        //Cập nhật thông tin danh sách các con của thành viên
        private void UpdateChild(ExTMember Member)
        {
            this.BeginInvoke(new Action(() =>
            {
                var member = memberHelper.GetExTMemberByID(Member.Id);
                if (member == null) return;
                member.ListSPOUSE = member.ListSPOUSE ?? new List<string>();
                member.ListCHILDREN = member.ListCHILDREN ?? new List<string>();

                Relationship.Text = member.ListSPOUSE.Count == 0 ? "Độc thân" : "Đã kết hôn";
                Application.DoEvents();

                NumChild = member.ListCHILDREN.Count;

                lblnumberChild.Text = $"Có {NumChild} người con.";
                Application.DoEvents();
                pnlContainerChild.Visible = lbllistChild.Visible = NumChild > 0;

                var listChild = member.ListCHILDREN;
                int width = 0;

                flpChild.Controls.Clear();
                flpChild.SuspendLayout();
                MemberCardForBuildTree[] arrCard = new MemberCardForBuildTree[listChild.Count];
                List<ExTMember> lstChildMember = memberHelper.FindMember("", "", "").Where(x => listChild.Any(m => m == x.Id)).OrderBy(o => o.LevelInFamilyForShow).ToList();
                int cnt = 0;
                foreach (var mem in lstChildMember)
                {
                    MemberCardForBuildTree newCard = new MemberCardForBuildTree(mem, 0.6f)
                    {
                        Member = mem,
                        Anchor = AnchorStyles.None
                    };
                    width = newCard.Width;
                    newCard.SelectMember += ChildSelect_Event;
                    newCard.EditInfoMember += MemberEdit_Event;
                    newCard.RequestReload += ContextClickEnd_Event;
                    arrCard[cnt] = newCard;
                    cnt++;
                };
                flpChild.Controls.AddRange(arrCard);
                flpChild.ResumeLayout();
                flpChild.Left = pnlContainerChild.Width > flpChild.Width + 1 ? (pnlContainerChild.Width - flpChild.Width) / 2 : 1;
                flpChild.Top = (pnlContainerChild.Height - (!pnlContainerChild.HorizontalScroll.Visible ? AppConst.ScrollHeight : AppConst.ScrollHeight) - flpChild.Height) / 2;
                pnlContainerChild.BorderStyle = pnlContainerChild.HorizontalScroll.Visible ? BorderStyle.FixedSingle : BorderStyle.None;
            }));
        }
        //Cập nhật thông tin vợ/ chồng của thành viên
        private void UpdateSpouse(ExTMember Member)
        {
            this.BeginInvoke(new Action(() =>
            {
                var member = memberHelper.GetExTMemberByID(Member.Id);
                if (member == null) return;
                if (member.GenderShow == null || member.GenderShow.Equals(AppConst.Gender.Male))
                {
                    lblinfospouse.Text = $"Thành viên là vợ của ông {member.Name}";
                    btnaddSpouse.Tag = btnaddSpouse.Text = "Thêm vợ";
                }
                else
                {
                    lblinfospouse.Text = $"Thành viên là chồng của bà {member.Name}";
                    btnaddSpouse.Tag = btnaddSpouse.Text = "Thêm chồng";
                }
                if (member.ListSPOUSE == null) member.ListSPOUSE = new List<string>();
                var listSpouse = member.ListSPOUSE;

                Label[] arrLabel = new Label[listSpouse.Count];

                flpSpouse.Controls.Clear();
                flpSpouse.SuspendLayout();
                int cnt = 0;
                foreach (var spouse in listSpouse)
                {
                    var memberTemp = memberHelper.GetExTMemberByID(spouse);
                    if (memberTemp != null)
                    {
                        Label lbl = new Label()
                        {
                            Font = new Font(fontFamily, 10, FontStyle.Regular),
                            ForeColor = ColorLeave,
                            Text = $"● {(memberTemp.Name)}",
                            Tag = spouse,
                            AutoSize = true,
                            Cursor = Cursors.Hand
                        };

                        lbl.MouseHover += (sender, e) =>
                        {
                            lbl.ForeColor = ColorHover;
                            lbl.Font = new Font(lbl.Font.Name, lbl.Font.SizeInPoints, FontStyle.Underline);
                        };
                        lbl.MouseLeave += (sender, e) =>
                        {
                            lbl.ForeColor = ColorLeave;
                            lbl.Font = new Font(lbl.Font.Name, lbl.Font.SizeInPoints, FontStyle.Regular);
                        };
                        lbl.Click += (sender, e) =>
                        {
                            Label label = sender as Label;
                            if (member.GenderShow.Equals(AppConst.Gender.Male))
                            {
                                cardSpouse.Member = memberHelper.GetExTMemberByID(label.Tag.ToString());
                            }
                            else
                            {
                                cardMain.Member = memberHelper.GetExTMemberByID(label.Tag.ToString());
                            }
                        };
                        arrLabel[cnt] = lbl;
                        cnt++;
                    }
                };
                flpSpouse.Controls.AddRange(arrLabel);
                flpSpouse.ResumeLayout();

                if (arrLabel.Length > 0)
                {
                    flpSpouseItemHeight = arrLabel[0].Height;
                }

                InitialPanelSpouseSize();
            }));
        }
        //Khởi tạo kích thước panel vợ/ chồng của thành viên đang được chọn
        private void InitialPanelSpouseSize()
        {
            flpSpouseHeight = flpSpouse.Controls.Count * flpSpouseItemHeight;

            if (flpSpouseHeight > flpSpouseMaxHeight)
            {
                flpSpouseHeight = flpSpouseMaxHeight;
            }

            flpSpouseHeight = flpSpouseHeight < flpSpouseMinHeight ? flpSpouseMinHeight : flpSpouseHeight;

            float temp = lblinfospouse.Height + btnaddSpouse.Height + pnlSpouse.Padding.Bottom * 2 + 21 + flpSpouseHeight;

            pnlContainerChild.Location = new Point(pnlContainerChild.Location.X, pnlSpouse.Location.Y + (int)temp + _SpaceChildWifeY);
        }
        //Sự kiện chọn thẻ con
        private void ChildSelect_Event(object sender, ExTMember member)
        {
            UpdateInfo(member);
        }
        //Sự kiện kích thước thay đổi
        private void SizeChanged_Event(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                _CenterX = this.Width / 2;
                _CenterY = cardMain.Height / 2 + cardFather.Height + _SpaceY + 10;
                CreatePositionCard();

                flpSpouseMaxHeight = (this.Height - pnlContainerChild.Height - 20 - _SpaceChildWifeY - pnlSpouse.Location.Y) - flpSpouse.Location.Y - btnaddSpouse.Height - 1 - pnlSpouse.Padding.Bottom * 2;

                InitialPanelSpouseSize();
            }));
        }
        //Sự kiện edit thông tin thành viên
        public void MemberEdit_Event(object sender, ExTMember member)
        {
            if (MemberEdit != null)
            {
                if (((cardSelect.Member.Name == AppConst.NameDefaul.Husban) || (cardSelect.Member.Name == AppConst.NameDefaul.Wife)) && member.Name != cardSelect.Member.Name) return;
                ParamRequest paramRequest = new ParamRequest()
                {
                    Mode = (typeof(AppConst.NameDefaul)).GetFields().Select(v => v.GetValue(v.Name)).ToArray().Contains(member.Name) ? ModeForm.New : ModeForm.Edit,
                    MainMember = cardSelect.Member,
                    SpouseMember = cardSelect == cardMain ? cardSpouse.Member : cardMain.Member,
                    RelationMember = member
                };
                MemberEdit?.Invoke(sender, paramRequest);
            }
        }
        //Sự kiện chọn thẻ thành viên
        public void MemberSelect_Event(object sender, ExTMember member)
        {
            if (MemberSelect != null)
            {
                MemberSelect?.Invoke(sender, member);
            }
        }
        //Cập nhật vị trí cho border chọn thẻ thành viên
        private void SetPositionBorderMemberSelect()
        {
            cardSelect = cardSelect ?? cardMain;
            upBorderMemberSelect.Location = new Point(cardSelect.Location.X - 4, cardSelect.Location.Y - 4);
            downBorderMemberSelect.Location = new Point(cardSelect.Location.X - 4, cardSelect.Location.Y + cardSelect.Height + 2);
            leftBorderMemberSelect.Location = new Point(cardSelect.Location.X - 4, cardSelect.Location.Y - 2);
            rightBorderMemberSelect.Location = new Point(cardSelect.Location.X + cardSelect.Width + 2, cardSelect.Location.Y - 2);
        }
    }

    public class ParamRequest
    {
        public ExTMember MainMember { get; set; }
        public ExTMember SpouseMember { get; set; }
        public ExTMember RelationMember { get; set; }
        public ModeForm Mode { get; set; }
    }
}
