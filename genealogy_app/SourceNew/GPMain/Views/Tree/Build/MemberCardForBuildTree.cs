using GP40Common;
using GPMain.Common;
using GPMain.Common.Helper;
using GPMain.Views.Controls;
using GPModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPMain.Views.Tree.Build
{
    /// <summary>
    /// Meno: Member Card For Build Tree
    /// Create by: Nguyễn Văn Hải
    /// </summary>
    public class MemberCardForBuildTree : MaterialSkin.Controls.MaterialCard
    {
        public static int WidthSize = 350;
        public static int HeightSize = 150;
        private ContextMenuStripData<TMember> _contextMember;
        public event EventHandler<ExTMember> EditInfoMember;
        public event EventHandler<ExTMember> SelectMember;
        public event EventHandler<TMember> RequestReload;

        private PictureBox picAvarta;

        private Label NameMember;
        private Label lbldateofbirth;
        private Label DOBMember;
        private Label lblplaceofbirth;
        private Label POBMember;
        private Label lblhometown;
        private Label HomeTownMember;

        private Color Color1 = Color.Blue;
        private Color Color2 = Color.DarkBlue;
        private Font Font1;
        private Font Font2;
        private int FontSize1 = 14;
        private int FontSize2 = 10;
        public float Zoom = 1;
        FontFamily fontFamily;

        ToolTip toolTip;

        const string messEdit = "Chỉnh sửa thông tin thành viên";
        bool bEdit = false;
        string[] lstName;
        public MemberCardForBuildTree(ExTMember member = null, float zoom = 1) : base()
        {
            toolTip = new ToolTip()
            {
                IsBalloon = true,
                ToolTipIcon = ToolTipIcon.Info
            };
            fontFamily = new FontFamily(AppConst.FontName);
            DoubleBuffered = true;
            this.Margin = new Padding(3);
            Zoom = zoom;
            this.Size = new Size((int)(WidthSize * Zoom), (int)(HeightSize * Zoom));
            CreateFont();
            CreateControlInfoMember();
            _contextMember = ContextMenuStripManager.CreateForMember();
            _contextMember.ItemClickEnd += (sender, mem) =>
            {
                if (mem != null && RequestReload != null)
                {
                    RequestReload?.Invoke(sender, mem);
                }
            };

            Type type = typeof(AppConst.NameDefaul);
            lstName = type.GetFields().Select(v => v.GetValue(v.Name).ToString()).ToArray();



            Member = member ?? new ExTMember() { Name = AppConst.NameDefaul.Other };
        }

        private ExTMember _Member;
        public Image AvartaMember { get; set; }
        public ExTMember Member
        {
            get => _Member;
            set
            {
                _Member = value;
                if (_Member != null)
                {
                    bEdit = !string.IsNullOrEmpty(_Member.Name) && (!lstName.Contains(_Member.Name));
                    if (picAvarta != null)
                    {
                        toolTip.SetToolTip(picAvarta, bEdit ? messEdit : Member.Name);
                        toolTip.SetToolTip(NameMember, bEdit ? messEdit : Member.Name);
                    }

                    if (_Member.GenderShow == null)
                    {
                        Color1 = Color.Gray;
                        Color2 = Color.DarkGray;
                    }
                    else if (_Member.GenderShow.Equals(AppConst.Gender.Male))
                    {
                        Color1 = Color.Blue;
                        Color2 = Color.DarkBlue;
                    }
                    else if (_Member.GenderShow.Equals(AppConst.Gender.Female))
                    {
                        Color1 = Color.FromArgb(255, 0, 238);
                        Color2 = Color.FromArgb(148, 4, 138);
                    }
                    else
                    {
                        Color1 = Color.FromArgb(166, 52, 247);
                        Color2 = Color.FromArgb(94, 3, 158);
                    }

                    if (picAvarta != null)
                    {
                        string imgPath = Application.StartupPath + "\\" + AppConst.AvatarThumbnailPath + "\\" + string.Format(AppConst.FormatNameThumbnailTree, _Member.AvatarImg);
                        Image image = null;
                        if (File.Exists(imgPath))
                        {
                            using var fileStream = File.OpenRead(imgPath);
                            image = Image.FromStream(fileStream);
                        }
                        else
                        {
                            image = Properties.Resources.no_avata;
                        }
                        picAvarta.Image = image;
                    }

                    if (NameMember != null)
                    {
                        NameMember.Text = _Member.Name;
                        NameMember.ForeColor = Color1;
                    }
                    if (DOBMember != null)
                    {
                        DOBMember.Text = _Member.BirthdayShow;
                        lbldateofbirth.ForeColor = DOBMember.ForeColor = Color2;

                        lbldateofbirth.Location = new Point(NameMember.Location.X, NameMember.Location.Y + NameMember.Height + (Zoom == 1 ? 10 : 2));
                        DOBMember.Location = new Point(lbldateofbirth.Location.X + (lbldateofbirth.Visible ? 80 : 0), lbldateofbirth.Location.Y);
                    }
                    if (POBMember != null)
                    {
                        POBMember.Text = _Member.BirthPlace;
                        lblplaceofbirth.ForeColor = POBMember.ForeColor = Color2;

                        lblplaceofbirth.Location = new Point(lbldateofbirth.Location.X, lbldateofbirth.Location.Y + DOBMember.Height + (Zoom == 1 ? 10 : 2));
                        POBMember.Location = new Point(lblplaceofbirth.Location.X + (lblplaceofbirth.Visible ? 80 : 0), lblplaceofbirth.Location.Y);
                    }
                    if (HomeTownMember != null)
                    {
                        HomeTownMember.Text = _Member.HomeTown;
                        lblhometown.ForeColor = HomeTownMember.ForeColor = Color2;

                        lblhometown.Location = new Point(lblplaceofbirth.Location.X, lblplaceofbirth.Location.Y + POBMember.Height + (Zoom == 1 ? 10 : 2));
                        HomeTownMember.Location = new Point(lblhometown.Location.X + (lblhometown.Visible ? 80 : 0), lblhometown.Location.Y);
                    }
                }
            }
        }

        private void CreateFont()
        {
            Font1 = new Font(fontFamily, (int)(FontSize1 * Zoom * 1.5) > FontSize1 ? FontSize1 : (int)(FontSize1 * Zoom * 1.5), FontStyle.Bold);
            Font2 = new Font(fontFamily, (int)(FontSize2 * Zoom * 1.5) > FontSize2 ? FontSize2 : (int)(FontSize2 * Zoom * 1.5), FontStyle.Regular);
        }

        private void CreateControlInfoMember()
        {
            this.Controls.Clear();
            this.SuspendLayout();
            this.Cursor = Cursors.Hand;
            Member = Member ?? new ExTMember();
            if (Member.GenderShow == null || Member.GenderShow.Equals(AppConst.Gender.Male))
            {
                Color1 = Color.Blue;
                Color2 = Color.LightBlue;
            }
            else if (Member.GenderShow.Equals(AppConst.Gender.Female))
            {
                Color1 = Color.LightPink;
                Color2 = Color.DeepPink;
            }
            else
            {
                Color1 = Color.Violet;
                Color2 = Color.DarkViolet;
            }

            /*Tạo panel hình ảnh thành viên*/
            picAvarta = new PictureBox()
            {
                Cursor = Cursors.Hand,
                Size = new Size((int)(60 * Zoom), (int)(90 * Zoom)),//hình ảnh 2 x 3
                BackColor = Color.Transparent,
                Location = new Point(5, 5),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            string imgPath = Application.StartupPath + "\\" + AppConst.AvatarThumbnailPath + "\\" + string.Format(AppConst.FormatNameThumbnailTree, Member.AvatarImg);

            Image image = null;
            if (File.Exists(imgPath))
            {
                using var fileStream = File.OpenRead(imgPath);
                image = Image.FromStream(fileStream);
            }
            else
            {
                image = Properties.Resources.no_avata;
            }
            if (picAvarta.Image != null)
                picAvarta.Image.Dispose();
            picAvarta.Image = image;
            picAvarta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            /*Tạo label tên thành viên*/
            NameMember = new Label()
            {
                Font = Font1,
                AutoSize = true,
                Location = new Point(picAvarta.Location.X + picAvarta.Size.Width + 3, picAvarta.Location.Y),
                Cursor = Cursors.Hand,
                Text = Member.Name,
                ForeColor = Color1
            };
            NameMember.MaximumSize = new Size(this.Width - NameMember.Location.X, NameMember.Height * 2);

            /*Tạo label ngày sinh*/
            lbldateofbirth = new Label()
            {
                Cursor = Cursors.Hand,
                AutoSize = true,
                Visible = Zoom >= 0.8,
                ForeColor = Color2,
                Font = Font2,
                Text = "Ngày sinh: ",
                Location = new Point(NameMember.Location.X, NameMember.Location.Y + NameMember.Height + (Zoom == 1 ? 10 : 2))
            };

            DOBMember = new Label()
            {
                Cursor = Cursors.Hand,
                AutoSize = true,
                Text = Member.BirthdayShow,
                Font = Font2,
                Location = new Point(lbldateofbirth.Location.X + (lbldateofbirth.Visible ? 80 : 0), lbldateofbirth.Location.Y)
            };

            /*Tạo label nơi sinh*/
            lblplaceofbirth = new Label()
            {
                Cursor = Cursors.Hand,
                AutoSize = true,
                Visible = Zoom >= 0.8,
                ForeColor = Color2,
                Font = Font2,
                Text = "Nơi sinh: ",
                Location = new Point(lbldateofbirth.Location.X, lbldateofbirth.Location.Y + DOBMember.Height + (Zoom == 1 ? 10 : 2)),
            };

            POBMember = new Label()
            {
                Cursor = Cursors.Hand,
                AutoSize = true,
                Visible = Zoom > 0.6,
                ForeColor = Color2,
                Font = Font2,
                Location = new Point(lblplaceofbirth.Location.X + (lblplaceofbirth.Visible ? 80 : 0), lblplaceofbirth.Location.Y),
                Text = Member.BirthPlace
            };
            POBMember.MaximumSize = new Size(this.Width - POBMember.Location.X, POBMember.Height * 2);

            /*Tạo label quê quán*/
            lblhometown = new Label()
            {
                Cursor = Cursors.Hand,
                AutoSize = true,
                Visible = Zoom >= 0.8,
                ForeColor = Color2,
                Font = Font2,
                Text = "Quê quán: ",
                Location = new Point(lblplaceofbirth.Location.X, lblplaceofbirth.Location.Y + POBMember.Height + (Zoom == 1 ? 10 : 2))
            };

            HomeTownMember = new Label()
            {
                Cursor = Cursors.Hand,
                AutoSize = true,
                Visible = Zoom > 0.8,
                ForeColor = Color2,
                Font = Font2,
                Location = new Point(lblhometown.Location.X + (lblhometown.Visible ? 80 : 0), lblhometown.Location.Y),
                Text = Member.HomeTown
            };
            HomeTownMember.MaximumSize = new Size(this.Width - HomeTownMember.Location.X, lblhometown.Height * 2);

            Control[] controls = new Control[8];
            controls[0] = picAvarta;
            controls[1] = NameMember;
            controls[2] = lbldateofbirth;
            controls[3] = DOBMember;
            controls[4] = lblplaceofbirth;
            controls[5] = POBMember;
            controls[6] = lblhometown;
            controls[7] = HomeTownMember;

            this.Controls.AddRange(controls);
            this.ResumeLayout(false);
            SetEvent();
        }

        private void SetEvent()
        {
            picAvarta.MouseClick += MemberEdit_Event;
            NameMember.MouseClick += MemberEdit_Event;
            this.MouseClick += OnClick_Event;
            lbldateofbirth.MouseClick += OnClick_Event;
            lblhometown.MouseClick += OnClick_Event;
            lblplaceofbirth.MouseClick += OnClick_Event;
            DOBMember.MouseClick += OnClick_Event;
            POBMember.MouseClick += OnClick_Event;
            HomeTownMember.MouseClick += OnClick_Event;
        }

        private void MemberEdit_Event(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (EditInfoMember != null)
                {
                    EditInfoMember?.Invoke(sender, Member);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Member == null) return;
                if (string.IsNullOrEmpty(Member.Id)) return;
                var objMember = AppManager.MenuMemberBuffer.ListAllMember[Member.Id];

                _contextMember.EnaleItem(ContextMenuStripManager.cstrUpdateFamilyLevel, objMember != null);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrEditInfoMember, objMember != null);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrShowCurrentRelation, objMember != null);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrDeleteMember, objMember != null);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrAddRelation, objMember != null);

                if (objMember == null)
                {
                    objMember = Member;
                }

                using MemberHelper memberHelper = new MemberHelper();

                var rootMember = memberHelper.RootTMember;
                bool isRootMember = false;

                if (rootMember != null)
                {
                    isRootMember = rootMember.Id == Member.Id;
                }

                _contextMember.EnaleItem(ContextMenuStripManager.cstrAddRootFamilyMember, (rootMember == null || objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveRootFamilyMember, (objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrAddFamilyHead, (objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveFamilyHead, (objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                _contextMember.Show(UIHelper.GetCursorPosition(), objMember);
            }
        }
        private void OnClick_Event(object sender, MouseEventArgs e)
        {
            if (lstName.Contains(Member.Name)) return;
            if (e.Button == MouseButtons.Left)
            {
                if (SelectMember != null)
                {
                    SelectMember?.Invoke(sender, Member);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                var objMember = AppManager.MenuMemberBuffer.ListAllMember[Member.Id];
                _contextMember.EnaleItem(ContextMenuStripManager.cstrUpdateFamilyLevel, objMember != null);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrEditInfoMember, objMember != null);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrShowCurrentRelation, objMember != null);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrDeleteMember, objMember != null);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrAddRelation, objMember != null);
                if (objMember == null)
                {
                    objMember = Member;
                }
                using MemberHelper memberHelper = new MemberHelper();

                var rootMember = memberHelper.RootTMember;
                bool isRootMember = false;
                if (rootMember != null)
                {
                    isRootMember = rootMember.Id == Member.Id;
                }
                _contextMember.EnaleItem(ContextMenuStripManager.cstrAddRootFamilyMember, (rootMember == null || objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveRootFamilyMember, (objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrAddFamilyHead, (objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveFamilyHead, (objMember.InRootTree || isRootMember) && objMember.Gender == (int)clsConst.ENUM_GENDER.Male);
                _contextMember.Show(UIHelper.GetCursorPosition(), objMember);
            }
        }
    }
}
