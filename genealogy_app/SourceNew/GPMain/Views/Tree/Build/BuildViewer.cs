using GPMain.Common;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Properties;
using GPMain.Views.Member;
using GPModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GPMain.Views.Tree.Build
{
    public partial class BuildViewer : BaseUserControl
    {
        #region Khai báo sự kiện
        public event EventHandler<ParamRequest> MemberEdit;
        public event EventHandler<ExTMember> MemberSelect;
        public event EventHandler<ExTMember> DeleteRelationComplete;
        public event EventHandler<ExTMember> ReloadMember;
        #endregion

        #region Khai báo biến
        private int _CenterX { get; set; }
        private int _CenterY { get; set; }
        private int _SpaceX { get; set; } = 10;
        private int _SpaceY { get; set; } = 50;
        private int _SpaceChildWifeY { get; set; } = 10;
        private ExTMember member { get; set; }

        private Color BorderColor = Color.Red;
        private Color ColorLeave = Color.Blue;
        private Color ColorHover = Color.Red;
        private int FontSize = 9;
        private int NumChild = 0;
        private Pen pen = new Pen(Color.Blue, 1);
        FontFamily fontFamily;
        Font font;
        #endregion

        #region Khai báo control
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

        private Panel upBorderMemberSelect;
        private Panel downBorderMemberSelect;
        private Panel leftBorderMemberSelect;
        private Panel rightBorderMemberSelect;

        Label lblrelationship;
        Label Relationship;
        Label lblnumberChild;
        Label lbllistChild;
        #endregion

        #region Khai báo class helper
        MemberHelper memberHelper = new MemberHelper();
        MemberRelationHelper memberRelation = new MemberRelationHelper();
        #endregion
        
        public BuildViewer(ExTMember objMember = null) : base()
        {
            InitializeComponent();

            fontFamily = new FontFamily(AppConst.FontName);
            Control[] controls = new Control[8];

            this.SuspendLayout();
            font = new Font(fontFamily, FontSize, FontStyle.Regular);

            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            member = objMember ?? new ExTMember() { Name = AppConst.NameDefaul.Other };
            cardMain = new MemberCardForBuildTree();
            cardFather = new MemberCardForBuildTree(null, 0.5f);
            cardMother = new MemberCardForBuildTree(null, 0.5f);
            cardSpouse = new MemberCardForBuildTree();

            pnlSpouse.MinimumSize = new Size(MemberCardForBuildTree.WidthSize, 150);

            pnlSpouse.SizeChanged += (sender, e) =>
            {
                pnlMainMember.Size = pnlSpouse.Size;
                pnlContainerChild.Location = new Point(pnlContainerChild.Location.X, pnlSpouse.Location.Y + pnlSpouse.Height + 20);
            };


            pnlMainMember.MinimumSize = new Size(MemberCardForBuildTree.WidthSize, 150);

            InitPanelMaineMember();

            InitSelectRectangle();

            InitEventMemberCard();

            _CenterX = this.Width / 2;
            _CenterY = cardMain.Height / 2 + cardFather.Height + _SpaceY + 10;

            controls[0] = cardMain;
            controls[1] = cardSpouse;
            controls[2] = cardFather;
            controls[3] = cardMother;
            controls[4] = upBorderMemberSelect;
            controls[5] = downBorderMemberSelect;
            controls[6] = leftBorderMemberSelect;
            controls[7] = rightBorderMemberSelect;

            this.Controls.AddRange(controls);
            this.ResumeLayout();

            this.Paint += OnPaint;
            this.SizeChanged += SizeChanged_Event;
            // UpdateInfo(member);
        }


        private void InitPanelMaineMember()
        {
            /*********************Panel Person*********************/
            lblrelationship = new Label();
            Relationship = new Label();
            lblnumberChild = new Label();
            lbllistChild = new Label();

            pnlMainMember.SuspendLayout();
            pnlMainMember.Size = pnlSpouse.Size;
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

            InitEventButtonAddChildren();

            btnCancelRelationship.Cursor = Cursors.Hand;
            btnCancelRelationship.Click += DeleteRelation;

            Control[] listControlPanelPersion = new Control[4];
            listControlPanelPersion[0] = lblrelationship;
            listControlPanelPersion[1] = Relationship;
            listControlPanelPersion[2] = lblnumberChild;
            listControlPanelPersion[3] = lbllistChild;
            pnlMainMember.Controls.AddRange(listControlPanelPersion);
            Application.DoEvents();
            pnlMainMember.ResumeLayout();
            pnlMainMember.Padding = new Padding(3);
            /***********************************************************/
            lbllistspouse.MouseHover += (sender, e) =>
            {
                lbllistspouse.ForeColor = ColorHover;
            };
            lbllistspouse.MouseLeave += (sender, e) =>
            {
                lbllistspouse.ForeColor = ColorLeave;
            };
            lbllistspouse.Click += ShowListSpouse_Event;

            InitEventButtonAddSpouse();

            InitEventButtonListSpouse();
        }
        private void InitEventButtonAddChildren()
        {
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
                ExTMember exTMember = new ExTMember()
                {
                    Name = AppConst.NameDefaul.Child
                };
                MemberEdit_Event(sender, exTMember);
            };
        }
        private void InitEventButtonAddSpouse()
        {
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
                ExTMember exTMember = new ExTMember()
                {
                    Name = AppConst.NameDefaul.Husban
                };
                if (cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male))
                {
                    exTMember.Name = AppConst.NameDefaul.Wife;
                }
                MemberEdit_Event(sender, exTMember);
            };
        }
        private void InitEventButtonListSpouse()
        {
            btnlistSpouse.MouseHover += (sender, e) =>
            {
                btnlistSpouse.BackgroundImage = Resources.listcheck;
            };
            btnlistSpouse.MouseLeave += (sender, e) =>
            {
                btnlistSpouse.BackgroundImage = Resources.list3;
            };
            btnlistSpouse.Click += ShowListSpouse_Event;
        }
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
                        GetParent(cardSelect.Member, out ExTMember father, out ExTMember mother);
                        cardFather.Member = father;
                        cardMother.Member = mother;
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
                        GetParent(cardSelect.Member, out ExTMember father, out ExTMember mother);
                        cardFather.Member = father;
                        cardMother.Member = mother;
                        this.Refresh();
                    }));
                }
            };
        }
        private void ShowListSpouse_Event(object sender, EventArgs e)
        {
            string gender = AppConst.Gender.Male;
            if (cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male))
            {
                gender = AppConst.Gender.Female;
            }
            if (AppManager.Navigation.ShowDialogWithParam<ListMemberRelation, RelationParam>(
                new RelationParam()
                {
                    Member = cardSelect.Member,
                    RelationFilter = cardSelect.Member.GenderShow.Equals(AppConst.Gender.Male) ? GPConst.Relation.PREFIX_WIFE : GPConst.Relation.PREFIX_HUSBAND,
                    Gender = gender
                },
                ModeForm.None,
                new StatusBarColor() { BackColor = AppConst.StatusBarBackColor, ForeColor = AppConst.StatusBarForeColor }).Result == DialogResult.OK)
            {
                RequestReloadListMember(sender, cardSelect.Member);
            }
        }
        private void ContextClickEnd_Event(object sender, TMember member)
        {
            if (member != null)
            {
                RequestReloadListMember(sender, cardSelect.Member);
            }
        }

        private void RequestReloadListMember(object sender, ExTMember mem)
        {
            if (ReloadMember != null)
            {
                ReloadMember?.Invoke(sender, mem);
            }
        }

        private void DeleteRelation(object sender, EventArgs e)
        {
            if ((cardSelect.Member.ListSPOUSE == null || cardSelect.Member.ListSPOUSE.Count == 0) && (cardSelect.Member.ListCHILDREN == null || cardSelect.Member.ListCHILDREN.Count == 0)) return;
            string info = $"Quan hệ vợ chồng giữa {cardSelect.Member.Name} ";
            if (!(cardSelect.Member.ListSPOUSE == null || cardSelect.Member.ListSPOUSE.Count == 0))
            {
                info += $"và {(cardSelect == cardMain ? cardSpouse.Member.Name : cardMain.Member.Name)} ";
            }
            if (!(cardSelect.Member.ListCHILDREN == null || cardSelect.Member.ListCHILDREN.Count == 0))
            {
                info += "và các con sẽ bị hủy.\n";
            }
            info += "Bạn có chắc chắn thực hiện thao tác này?";

            //if (MessageBox.Show(info, "Hủy quan hệ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            if (AppManager.Dialog.Confirm(info))
            {
                if (memberRelation.DeleteAllRelation(cardSelect.Member) && DeleteRelationComplete != null)
                {
                    DeleteRelationComplete?.Invoke(sender, cardSelect.Member);
                }
            }
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

            pnlContainerChild.Size = new Size(cardSpouse.Location.X + cardSpouse.Width - cardMain.Location.X, pnlContainerChild.Height);

            UpdateChild(cardSelect.Member);
            UpdateSpouse(cardSelect.Member);
            SetPositionBorderMemberSelect();
        }

        public void UpdateInfo(ExTMember exTMember, bool reload = false)
        {
            if (exTMember == null)
            {
                exTMember = new ExTMember() { Name = "Thêm thành viên" };
            }
            else
            {
                exTMember = memberHelper.GetExTMemberByID(exTMember.Id);
                if (exTMember == null)
                {
                    exTMember = new ExTMember() { Name = "Thêm thành viên" };
                }
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
                ExTMember memberFather = null;
                ExTMember memberMother = null;
                if (exTMember.ListPARENT == null || exTMember.ListPARENT.Count == 0)
                {
                    memberFather = new ExTMember() { Name = AppConst.NameDefaul.Father };
                    memberMother = new ExTMember() { Name = AppConst.NameDefaul.Mother };
                }
                else
                {
                    var temp = memberHelper.GetExTMemberByID(exTMember.ListPARENT[0]);
                    if (temp != null)
                    {
                        if (temp.GenderShow.Equals(AppConst.Gender.Male))
                        {
                            memberFather = temp;
                        }
                        else
                        {
                            memberMother = temp;
                        }
                    }
                    try
                    {
                        if (memberFather == null)
                        {
                            memberFather = exTMember.ListPARENT.Count > 1 ? (memberHelper.GetExTMemberByID(exTMember.ListPARENT[1])) : (new ExTMember() { Name = AppConst.NameDefaul.Father });
                        }
                        else
                        {
                            memberMother = exTMember.ListPARENT.Count > 1 ? (memberHelper.GetExTMemberByID(exTMember.ListPARENT[1])) : (new ExTMember() { Name = AppConst.NameDefaul.Mother });
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

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
        }

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

                NumChild = member.ListCHILDREN.Count; //member == cardMain.Member ? (cardMain.Member.ListCHILDREN != null ? cardMain.Member.ListCHILDREN.Count : 0) : (cardSpouse.Member.ListCHILDREN != null ? cardSpouse.Member.ListCHILDREN.Count : 0);

                lblnumberChild.Text = $"Có {NumChild} người con.";
                Application.DoEvents();
                pnlContainerChild.Visible = lbllistChild.Visible = NumChild > 0;

                var listChild = member.ListCHILDREN;
                int width = 0;

                flpChild.Controls.Clear();
                flpChild.SuspendLayout();
                MemberCardForBuildTree[] arrCard = new MemberCardForBuildTree[listChild.Count];
                List<ExTMember> lstChildMember = memberHelper.FindMember("", "", "").Where(x => listChild.Any(m => m == x.Id)).OrderBy(o => o.LevelInFamilyForShow).ToList();
                for (int i = 0; i < lstChildMember.Count; i++)
                {
                    var mem = lstChildMember[i];
                    MemberCardForBuildTree newCard = new MemberCardForBuildTree(mem, 0.6f)
                    {
                        Member = mem,
                        Anchor = AnchorStyles.None
                    };
                    width = newCard.Width;
                    newCard.SelectMember += ChildSelect_Event;
                    newCard.EditInfoMember += MemberEdit_Event;
                    newCard.RequestReload += ContextClickEnd_Event;
                    arrCard[i] = newCard;
                };
                flpChild.Controls.AddRange(arrCard);
                flpChild.ResumeLayout();
                flpChild.Left = pnlContainerChild.Width > flpChild.Width + 1 ? (pnlContainerChild.Width - flpChild.Width) / 2 : 1;
                flpChild.Top = (pnlContainerChild.Height - (!pnlContainerChild.HorizontalScroll.Visible ? AppConst.ScrollHeight : AppConst.ScrollHeight) - flpChild.Height) / 2;
                pnlContainerChild.BorderStyle = pnlContainerChild.HorizontalScroll.Visible ? BorderStyle.FixedSingle : BorderStyle.None;
            }));
        }

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

                List<Label> arrLabel = new List<Label>();

                flpSpouse.Controls.Clear();
                flpSpouse.SuspendLayout();
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
                        arrLabel.Add(lbl);
                    }
                };
                flpSpouse.Controls.AddRange(arrLabel.ToArray());
                flpSpouse.ResumeLayout();
            }));
        }

        private void ChildSelect_Event(object sender, ExTMember member)
        {
            UpdateInfo(member);
        }

        private void SizeChanged_Event(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                this.SuspendLayout();
                _CenterX = this.Width / 2;
                _CenterY = cardMain.Height / 2 + cardFather.Height + _SpaceY + 10;
                CreatePositionCard();

                pnlSpouse.MaximumSize = new Size(pnlSpouse.Width, this.Height - pnlContainerChild.Height - 40 - pnlSpouse.Location.Y);//Kích thước tối đa của panel Spouse
                //flpSpouse.MaximumSize = new Size(cardMain.Width - 60, pnlSpouse.Height - 90);
                //pnlContainerChild.Location = new Point(pnlContainerChild.Location.X, pnlSpouse.Location.Y + pnlSpouse.Height + 20);
                this.ResumeLayout();
            }));
        }

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
        public void MemberSelect_Event(object sender, ExTMember member)
        {
            if (MemberSelect != null)
            {
                MemberSelect?.Invoke(sender, member);
            }
        }

        public void GetParent(ExTMember mem, out ExTMember father, out ExTMember mother)
        {
            ExTMember memberFather = null;
            ExTMember memberMother = null;
            if (mem.ListPARENT == null || mem.ListPARENT.Count == 0)
            {
                memberFather = new ExTMember() { Name = AppConst.NameDefaul.Father };
                memberMother = new ExTMember() { Name = AppConst.NameDefaul.Mother };
            }
            else
            {
                var temp = memberHelper.GetExTMemberByID(mem.ListPARENT[0]);

                if (temp.GenderShow.Equals(AppConst.Gender.Male))
                {
                    memberFather = temp;
                }
                else
                {
                    memberMother = temp;
                }
                try
                {
                    if (memberFather == null)
                    {
                        memberFather = mem.ListPARENT.Count > 1 ? memberHelper.GetExTMemberByID(mem.ListPARENT[1]) : new ExTMember() { Name = AppConst.NameDefaul.Father };
                    }
                    else
                    {
                        memberMother = mem.ListPARENT.Count > 1 ? memberHelper.GetExTMemberByID(mem.ListPARENT[1]) : new ExTMember() { Name = AppConst.NameDefaul.Mother };
                    }
                }
                catch
                {
                }
            }
            father = memberFather;
            mother = memberMother;
        }

        private void SetPositionBorderMemberSelect()
        {
            cardSelect = cardSelect ?? cardMain;
            upBorderMemberSelect.Location = new Point(cardSelect.Location.X - 4, cardSelect.Location.Y - 4);
            downBorderMemberSelect.Location = new Point(cardSelect.Location.X - 4, cardSelect.Location.Y + cardSelect.Height + 2);
            leftBorderMemberSelect.Location = new Point(cardSelect.Location.X - 4, cardSelect.Location.Y - 2);
            rightBorderMemberSelect.Location = new Point(cardSelect.Location.X + cardSelect.Width + 2, cardSelect.Location.Y - 2);
        }
    }
}
