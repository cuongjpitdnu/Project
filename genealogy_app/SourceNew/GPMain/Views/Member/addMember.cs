using GP40Common;
using GP40DrawTree;
using GPCommon;
using GPConst;
using GPMain.Common;
using GPMain.Common.Database;
using GPMain.Common.Dialog;
using GPMain.Common.FamilyEvent;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Views.Controls;
using GPMain.Views.Tree.Build;
using GPModels;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GPMain.Views.Member
{
    public partial class addMember : BaseUserControl
    {
        public bool bUpdateLevelInFamily = false;
        public const string cstrLabelTextUndefined = "Chưa rõ";
        private int _levelInFamily = 0;
        public string idSchoolSelected = "";
        public string idEventSelected = "";
        public string imgAvatarUrl = "";
        public List<ExTMemberJob> gLstMemberJob;
        public List<ExTMemberSchool> gLstMemberSchool;
        public List<ExTMemberEvent> gLstMemberEvent;
        public Dictionary<string, MaterialTextBox> _mappingControlSocial = new Dictionary<string, MaterialTextBox>();
        private bool bLoad = false;

        MemberCardTemplFull memberCardFull;
        MemberCardTemplInput memberCardInput;
        MemberCardTemplShort memberCardShort;
        MemberCardTemplTall memberCardTall;
        MemberCardTemplVeryShort memberCardVeryShort;

        string typeMember;
        TMember submember;
        TMember primaryMember;
        string relationType;
        string relationType2;
        string gender;

        bool inRootTree = false;
        bool spouseInRootTree = false;

        MemberHelper memberHelper = new MemberHelper();
        public addMember(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            TitleBar = mode == ModeForm.New ? "Thêm thành viên" : "Chỉnh sửa thông tin thành viên";

            InitializeComponent();

            SetBackColor();

            CreateTextboxSocialNetwork();

            BindingComboboxTypeTree();

            CreateTemplateCard();

            GetParameter();

            cbbackcolor.ColorControl.onColorChanged += (color) =>
            {
                ChangeTypeTree();
            };
            cbforecolor.ColorControl.onColorChanged += (color) =>
            {
                ChangeTypeTree();
            };

            BindingTableToCombo<MTypeName>(cmbTypeName, "TypeName");

            BindingTableToCombo<MReligion>(cmbReligion, "RelName");

            BindingTableToCombo<MNationality>(cmbNational, "NatName");

            var queryTypeName = AppManager.DBManager.GetTable<MTypeName>();
            var objTypeNameDefault = queryTypeName.FirstOrDefault(i => i.IsDefault == true);
            if (objTypeNameDefault != null)
            {
                cmbTypeName.SelectedValue = objTypeNameDefault.Id;
            }

            var queryReligion = AppManager.DBManager.GetTable<MReligion>();
            var objRReligionDefault = queryReligion.FirstOrDefault(i => i.IsDefault == true);
            if (objRReligionDefault != null)
            {
                cmbReligion.SelectedValue = objRReligionDefault.Id;
            }

            var queryNational = AppManager.DBManager.GetTable<MNationality>();
            var objNationalDefault = queryNational.FirstOrDefault(i => i.IsDefault == true);
            if (objNationalDefault != null)
            {
                cmbNational.SelectedValue = objNationalDefault.Id;
            }

            if (this.Mode == ModeForm.Edit)
            {
                InitialEditMember();
            }
            else if (this.Mode == ModeForm.New)
            {
                InitialNewMember();
            }

            ChangeTypeTree();
            bLoad = true;
        }

        private void SetBackColor()
        {
            this.BackColor = AppConst.PopupBackColor;
            tabPage1.BackColor = AppConst.PopupBackColor;
            tabPage2.BackColor = AppConst.PopupBackColor;
            tabPage3.BackColor = AppConst.PopupBackColor;
            tabPage4.BackColor = AppConst.PopupBackColor;
            tabPage5.BackColor = AppConst.PopupBackColor;
            tabPage6.BackColor = AppConst.PopupBackColor;
            pnlCard.BackColor = AppConst.PopupBackColor;
            gridEvent.BackgroundColor = AppConst.PopupBackColor;
            gridJob.BackgroundColor = AppConst.PopupBackColor;
            gridSchool.BackgroundColor = AppConst.PopupBackColor;
            txtNote.BackColor = AppConst.PopupBackColor;
        }

        private void CreateTextboxSocialNetwork()
        {
            var newY = tableLayoutPanel2.Location.Y + tableLayoutPanel2.Height;

            var lstSocialNetwork = AppManager.DBManager.GetTable<MSocialNetwork>().ToList();

            if (lstSocialNetwork != null)
            {
                var index = 0;
                foreach (var obj in lstSocialNetwork)
                {
                    var newLocation = new Point(txtAddress.Location.X, newY);
                    var txtAdd = CreateTextboxSocial("txtSocial" + (++index), obj.SocialName, newLocation, txtAddress.Size);
                    txtAdd.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                    _mappingControlSocial.Add(obj.SocialName, txtAdd);
                    tabPage1.Controls.Add(txtAdd);

                    newY = newY + txtAdd.Height + 8;
                }
            }
        }

        private void InitialEditMember()
        {
            var objParam = this.Params.GetValue<TMember>();
            ckFamilyHead.Enabled = ckRootMember.Enabled = objParam.InRootTree;
            var tblTMember = AppManager.DBManager.GetTable<TMember>();
            if (objParam.Id != "")
            {
                // check member exist
                var objMember = tblTMember.FirstOrDefault(i => i.Id == objParam.Id);
                if (objMember == null)
                {
                    return;
                }

                _levelInFamily = objMember.LevelInFamily;

                var rootMember = memberHelper.RootTMember;
                if (rootMember != null)
                {
                    ckRootMember.Checked = rootMember.Id == objMember.Id;
                }
                var lstFamilyHead = memberHelper.ListFamilyHead;
                ckFamilyHead.Checked = lstFamilyHead.Contains(objMember.Id);

                if (!string.IsNullOrEmpty(objMember.strBackColor))
                {
                    cbbackcolor.Color = ColorTranslator.FromHtml(objMember.strBackColor);
                    memberCardFull.BackColor = cbbackcolor.Color;
                }
                if (!string.IsNullOrEmpty(objMember.strForeColor))
                {
                    cbforecolor.Color = ColorTranslator.FromHtml(objMember.strForeColor);
                    memberCardFull.ForeColor = cbforecolor.Color;
                }
                ckUseDefaultColor.Checked = objMember.UseDefaultColor;
                ckStepChild.Checked = objMember.ShowStepChild;

                // binding data to form
                txtFullName.Text = objMember.Name.Trim();
                if (string.IsNullOrEmpty(objMember.ShowName))
                {
                    objMember.ShowName = memberHelper.CreateShowName(objMember.Name);
                }
                txtShowName.Text = objMember.ShowName.Trim();
                if (objMember.TypeName.Count > 0)
                {
                    cmbTypeName.SelectedValue = objMember.TypeName.First().Key ?? "";
                    txtCommonName.Text = objMember.TypeName.First().Value ?? "";
                }
                txtChildLevelInFamily.Text = (objMember.ChildLevelInFamily > 0) ? objMember.ChildLevelInFamily.ToString() : "";

                rdoMale.Checked = objMember.Gender == (int)EmGender.Male;
                rdoFemale.Checked = objMember.Gender == (int)EmGender.FeMale;
                rdoUnknown.Checked = objMember.Gender == (int)EmGender.Unknown;

                var strAvatarFolderThumbnail = Directory.GetCurrentDirectory() + AppConst.AvatarThumbnailPath;
                if (Directory.Exists(strAvatarFolderThumbnail))
                {
                    if (!string.IsNullOrEmpty(objMember.AvatarImg))
                    {
                        var filePath = strAvatarFolderThumbnail + objMember.AvatarImg;
                        var file = new FileInfo(filePath);

                        if (file.Exists)
                        {
                            using (var fileStream = File.OpenRead(filePath))
                            {
                                avatarImage.Image = Image.FromStream(fileStream);
                            }
                        }
                    }
                }

                txtHometown.Text = objMember.HomeTown?.Trim();
                cmbNational.SelectedValue = objMember.National ?? "";
                cmbReligion.SelectedValue = objMember.Religion ?? "";

                txtDateBDSun.Text = (objMember.Birthday.DaySun != -1) ? objMember.Birthday.DaySun.ToString().PadLeft(2, '0') : "";
                txtMonthBDSun.Text = (objMember.Birthday.MonthSun != -1) ? objMember.Birthday.MonthSun.ToString().PadLeft(2, '0') : "";
                txtYearBDSun.Text = (objMember.Birthday.YearSun != -1) ? objMember.Birthday.YearSun.ToString().PadLeft(2, '0') : "";
                txtDateBDLunar.Text = (objMember.Birthday.DayMoon != -1) ? objMember.Birthday.DayMoon.ToString().PadLeft(2, '0') : "";
                txtMonthBDLunar.Text = (objMember.Birthday.MonthMoon != -1) ? objMember.Birthday.MonthMoon.ToString().PadLeft(2, '0') : "";
                txtYearBDLunar.Text = (objMember.Birthday.YearMoon != -1) ? objMember.Birthday.YearMoon.ToString().PadLeft(2, '0') : "";
                chkLeapMonthBDLunar.Checked = objMember.IsLeapMonthDB;
                txtBirthPlace.Text = objMember.BirthPlace?.Trim();

                if (objMember.IsDeath == true)
                {
                    txtDateDDLunar.Text = (objMember.DeadDay.DayMoon != -1) ? objMember.DeadDay.DayMoon.ToString().PadLeft(2, '0') : "";
                    txtMonthDDLunar.Text = (objMember.DeadDay.MonthMoon != -1) ? objMember.DeadDay.MonthMoon.ToString().PadLeft(2, '0') : "";
                    txtYearDDLunar.Text = (objMember.DeadDay.YearMoon != -1) ? objMember.DeadDay.YearMoon.ToString().PadLeft(2, '0') : "";
                    chkLeapMonthDDLunar.Checked = objMember.IsLeapMonthDD;
                    txtDateDDSun.Text = (objMember.DeadDay.DaySun != -1) ? objMember.DeadDay.DaySun.ToString().PadLeft(2, '0') : "";
                    txtMonthDDSun.Text = (objMember.DeadDay.MonthSun != -1) ? objMember.DeadDay.MonthSun.ToString().PadLeft(2, '0') : "";
                    txtYearDDSun.Text = (objMember.DeadDay.YearSun != -1) ? objMember.DeadDay.YearSun.ToString().PadLeft(2, '0') : "";

                    txtDeadPlace.Text = objMember.DeadPlace != null ? objMember.DeadPlace.Trim() : "";
                    chkIsDeath.Checked = true;
                }

                txtAddress.Text = objMember.Contact.Address?.Trim();
                txtTel1.Text = objMember.Contact.Tel_1?.Trim();
                txtTel2.Text = objMember.Contact.Tel_2?.Trim();
                txtEmail1.Text = objMember.Contact.Email_1?.Trim();
                txtEmail2.Text = objMember.Contact.Email_2?.Trim();
                txtFax.Text = objMember.Contact.Fax?.Trim();
                txtWebsite.Text = objMember.Contact.Website?.Trim();

                if (objMember.Contact.SocialNetwork != null)
                {
                    foreach (var socialNetwork in objMember.Contact.SocialNetwork)
                    {
                        if (_mappingControlSocial.ContainsKey(socialNetwork.Key))
                        {
                            _mappingControlSocial[socialNetwork.Key].Text = socialNetwork.Value;
                        }
                    }
                }

                txtNote.Text = objMember.Contact.Note?.Trim();

                BindingMemberJobToDataGridView(objMember);

                BindingMemberSchoolToDataGridView(objMember);

                BindingMemberEventToDataGridView(objMember);
            }
        }

        private void BindingMemberJobToDataGridView(TMember objMember)
        {
            if (objMember.Job.Count > 0)
            {
                gLstMemberJob = objMember.Job.AsEnumerable().Select(i => new ExTMemberJob
                {
                    Id = i.Id,
                    CompanyName = i.CompanyName?.Trim(),
                    CompanyAddress = i.CompanyAddress?.Trim(),
                    Position = i.Position?.Trim(),
                    Job = i.Job?.Trim(),
                    StartDate = i.StartDate,
                    EndDate = i.EndDate,
                    TimeShow = ((i.StartDate != null) ? i.StartDate.ToDateSun() : "") + "~" + ((i.EndDate != null) ? i.EndDate.ToDateSun() : ""),
                }).ToList();

                BindingHelper.BindingDataGrid(gridJob, gLstMemberJob);
            }
        }

        private void BindingMemberSchoolToDataGridView(TMember objMember)
        {
            if (objMember.School.Count > 0)
            {
                gLstMemberSchool = objMember.School.AsEnumerable().Select(i => new ExTMemberSchool
                {
                    Id = i.Id,
                    SchoolName = i.SchoolName?.Trim(),
                    Description = i.Description?.Trim(),
                    StartDate = i.StartDate,
                    EndDate = i.EndDate,
                    TimeShow = ((i.StartDate != null) ? i.StartDate.ToDateSun() : "") + "~" + ((i.EndDate != null) ? i.EndDate.ToDateSun() : ""),
                }).ToList();
                BindingHelper.BindingDataGrid(gridSchool, gLstMemberSchool);
            }
        }

        private void BindingMemberEventToDataGridView(TMember objMember)
        {
            if (objMember.Event.Count > 0)
            {
                gLstMemberEvent = objMember.Event.AsEnumerable().Select(i => new ExTMemberEvent
                {
                    Id = i.Id,
                    EventName = i.EventName?.Trim(),
                    Location = i.Location?.Trim(),
                    Description = i.Description?.Trim(),
                    StartDate = i.StartDate,
                    EndDate = i.EndDate,
                    TimeShow = ((i.StartDate != null) ? i.StartDate.ToDateSun() : "") + "~" + ((i.EndDate != null) ? i.EndDate.ToDateSun() : ""),
                }).ToList();
                BindingHelper.BindingDataGrid(gridEvent, gLstMemberEvent);
            }
        }

        private void InitialNewMember()
        {
            if (this.Params != null)
            {
                gender = string.IsNullOrEmpty(gender) ? "" : gender;

                if (primaryMember != null)
                {
                    ckFamilyHead.Enabled = ckRootMember.Enabled = (typeMember.Equals(AppConst.NameDefaul.Child) || (primaryMember.InRootTree && typeMember.Equals(AppConst.NameDefaul.Father)));

                    _levelInFamily = primaryMember.LevelInFamily;

                    var tblMRelation = AppManager.DBManager.GetTable<MRelation>().FirstOrDefault(i => i.Id == relationType.ToString());
                    txtChildLevelInFamily.Text = "1";
                    if (tblMRelation.MainRelation.Contains(GPConst.Relation.PREFIX_CHILD))
                    {
                        _levelInFamily++;

                        inRootTree = primaryMember.InRootTree;

                        var tempMember = AppManager.MenuMemberBuffer.ListAllMember.GetValue(primaryMember?.Id ?? "", new ExTMember());
                        var tempSpouse = AppManager.MenuMemberBuffer.ListAllMember.GetValue(submember?.Id ?? "", new ExTMember());

                        if (tempMember.InRootTree)
                        {
                            txtChildLevelInFamily.Text = submember != null ? (submember.ListCHILDREN.Count + 1).ToString() : (primaryMember.ListCHILDREN.Count + 1).ToString();
                        }
                        else if (tempSpouse.InRootTree)
                        {
                            txtChildLevelInFamily.Text = tempSpouse != null ? (tempSpouse.ListCHILDREN.Count + 1).ToString() : (submember.ListCHILDREN.Count + 1).ToString();
                        }
                        else
                        {
                            if (tempMember.Gender == (int)GPConst.EmGender.Male)
                            {
                                txtChildLevelInFamily.Text = (tempSpouse.ListCHILDREN.Count + 1).ToString();
                            }
                            else if (tempSpouse.Gender == (int)GPConst.EmGender.Male)
                            {
                                txtChildLevelInFamily.Text = (tempMember.ListCHILDREN.Count + 1).ToString();
                            }
                            else
                            {
                                txtChildLevelInFamily.Text = "1";
                            }
                        }
                    }
                    else if (tblMRelation.MainRelation.Contains(GPConst.Relation.PREFIX_DAD) || tblMRelation.MainRelation.Contains(GPConst.Relation.PREFIX_MOM))
                    {
                        _levelInFamily--;
                        inRootTree = primaryMember.InRootTree;
                    }
                    else if (tblMRelation.MainRelation.Contains(GPConst.Relation.PREFIX_HUSBAND) || tblMRelation.MainRelation.Contains(GPConst.Relation.PREFIX_WIFE))
                    {
                        spouseInRootTree = primaryMember.SpouseInRootTree;
                    }
                }

                if (gender.Equals(AppConst.Gender.Male) || string.IsNullOrEmpty(gender)) rdoMale.Checked = true;
                else if (gender.Equals(AppConst.Gender.Female)) rdoFemale.Checked = true;
            }
        }

        private void CreateTemplateCard()
        {
            lblnote = new Label() { Text = "*Chọn thẻ để xem trước thông tin đối với từng loại thẻ.", ForeColor = Color.Red };
            lblnote.AutoSize = true;
            lblnote.Location = new Point(1, pnlCard.Height - lblnote.Height);

            pnlCard.Controls.Add(lblnote);

            memberCardFull = new MemberCardTemplFull();
            memberCardInput = new MemberCardTemplInput();
            memberCardShort = new MemberCardTemplShort();
            memberCardTall = new MemberCardTemplTall();
            memberCardVeryShort = new MemberCardTemplVeryShort();

            Font fontCardFull = memberCardFull.lblFullName.Font;
            memberCardFull.lblFullName.Font = new Font(fontCardFull.FontFamily, 14.25f);
            Font fontCardInput = memberCardInput.lblFullName.Font;
            memberCardInput.lblFullName.Font = new Font(fontCardInput.FontFamily, 14.25f);
            Font fontCardShort = memberCardShort.lblFullName.Font;
            memberCardShort.lblFullName.Font = new Font(fontCardShort.FontFamily, 14.25f);
            Font fontCardTall = memberCardTall.lblFullName.Font;
            memberCardTall.lblFullName.Font = new Font(fontCardTall.FontFamily, 14.25f);
            Font fontCardVeryShort = memberCardVeryShort.lblFullName.Font;
            memberCardVeryShort.lblFullName.Font = new Font(fontCardVeryShort.FontFamily, 14.25f);

            memberCardFull.Location = new Point(pnlCard.Width / 2 - memberCardFull.Width / 2, 5);
            memberCardInput.Location = new Point(pnlCard.Width / 2 - memberCardInput.Width / 2, 5);
            memberCardShort.Location = new Point(pnlCard.Width / 2 - memberCardShort.Width / 2, 5);
            memberCardTall.Location = new Point(pnlCard.Width / 2 - memberCardTall.Width / 2, 5);
            memberCardVeryShort.Location = new Point(pnlCard.Width / 2 - memberCardVeryShort.Width / 2, 5);

            pnlCard.Controls.Add(memberCardFull);
            if (cboTypeTree.Items.Count > 0)
            {
                string typeTree = this.Params.GetValue<string>("TypeTree");
                if (string.IsNullOrEmpty(typeTree))
                {
                    cboTypeTree.SelectedIndex = 0;
                }
                else
                {
                    cboTypeTree.Text = typeTree;
                }
            }
        }

        private void BindingComboboxTypeTree()
        {
            var arrTypeTree = clsConst.TEMPLATE_NAME;
            List<ModelBinding> dataSourceTypeTree = new List<ModelBinding>();
            int cnt = 0;
            foreach (var typeTree in arrTypeTree)
            {
                dataSourceTypeTree.Add(new ModelBinding() { Key = typeTree, Display = clsConst.TypeTreeDisplay[cnt++] });
            }
            cboTypeTree.DataSource = dataSourceTypeTree;
            cboTypeTree.ValueMember = "Key";
            cboTypeTree.DisplayMember = "Display";
        }

        private void GetParameter()
        {
            gender = this.Params.GetValue<string>("gender", AppConst.Gender.Male);
            typeMember = this.Params.GetValue<string>("type_member");
            submember = this.Params.GetValue<TMember>("sub_member");
            primaryMember = this.Params.GetValue<TMember>("primary_member");
            relationType = this.Params.GetValue<string>("relation_type");
            relationType2 = this.Params.GetValue<string>("relation_type2");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInfoMember()) return;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập tên thành viên!");
                    return;
                }

                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                TMember objMember;
                bool actionInsert = (this.Mode == ModeForm.Edit) ? false : true;

                var emGender = EmGender.Unknown;
                if (rdoMale.Checked == true)
                {
                    emGender = EmGender.Male;
                }
                if (rdoFemale.Checked == true)
                {
                    emGender = EmGender.FeMale;
                }

                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                if (actionInsert)
                {
                    objMember = new TMember();
                    objMember.Id = LiteDBManager.CreateNewId();
                }
                else
                {
                    var objParam = this.Params.GetValue<TMember>();
                    objMember = tblTMember.FirstOrDefault(i => i.Id == objParam.Id);
                    if (objMember == null)
                    {
                        return;
                    }
                }

                // avatar
                string oldImg = "";
                if (!string.IsNullOrEmpty(imgAvatarUrl))
                {
                    var objFile = new FileInfo(imgAvatarUrl);
                    if (objFile.Exists)
                    {
                        oldImg = objMember.AvatarImg;
                        objMember.AvatarImg = objMember.Id + objFile.Extension;
                    }
                }

                objMember.ShowName = txtShowName.Text.Trim();
                objMember.Name = txtFullName.Text.Trim();
                if (cmbTypeName.SelectedValue != null)
                {
                    if (objMember.TypeName.ContainsKey(cmbTypeName.SelectedValue.ToString()))
                    {
                        objMember.TypeName[cmbTypeName.SelectedValue.ToString()] = txtCommonName.Text.Trim();
                    }
                    else
                    {
                        objMember.TypeName.Add(cmbTypeName.SelectedValue.ToString(), txtCommonName.Text.Trim());
                    }
                }
                objMember.Gender = (int)emGender;
                int childLevelInFamily = 0;
                if (int.TryParse(txtChildLevelInFamily.Text.Trim(), out childLevelInFamily))
                {
                    if (objMember.ChildLevelInFamily != childLevelInFamily)
                    {
                        bUpdateLevelInFamily = true;
                    }
                }
                objMember.ChildLevelInFamily = childLevelInFamily;

                objMember.HomeTown = txtHometown.Text.Trim();
                objMember.National = (!string.IsNullOrEmpty(cmbNational.Text)) ? cmbNational.SelectedValue.ToString() : "";
                objMember.Religion = (!string.IsNullOrEmpty(cmbReligion.Text)) ? cmbReligion.SelectedValue.ToString() : "";

                objMember.Birthday.DaySun = ConvertHelper.CnvNullToInt(txtDateBDSun.Text.Trim());
                objMember.Birthday.MonthSun = ConvertHelper.CnvNullToInt(txtMonthBDSun.Text.Trim());
                objMember.Birthday.YearSun = ConvertHelper.CnvNullToInt(txtYearBDSun.Text.Trim());
                objMember.Birthday.DayMoon = ConvertHelper.CnvNullToInt(txtDateBDLunar.Text.Trim());
                objMember.Birthday.MonthMoon = ConvertHelper.CnvNullToInt(txtMonthBDLunar.Text.Trim());
                objMember.Birthday.YearMoon = ConvertHelper.CnvNullToInt(txtYearBDLunar.Text.Trim());
                objMember.IsLeapMonthDB = (chkLeapMonthBDLunar.Checked == true) ? true : false;
                objMember.BirthPlace = txtBirthPlace.Text.Trim();

                objMember.IsDeath = chkIsDeath.Checked;
                if (chkIsDeath.Checked == true)
                {
                    objMember.DeadDay.DaySun = ConvertHelper.CnvNullToInt(txtDateDDSun.Text.Trim());
                    objMember.DeadDay.MonthSun = ConvertHelper.CnvNullToInt(txtMonthDDSun.Text.Trim());
                    objMember.DeadDay.YearSun = ConvertHelper.CnvNullToInt(txtYearDDSun.Text.Trim());
                    objMember.DeadDay.DayMoon = ConvertHelper.CnvNullToInt(txtDateDDLunar.Text.Trim());
                    objMember.DeadDay.MonthMoon = ConvertHelper.CnvNullToInt(txtMonthDDLunar.Text.Trim());
                    objMember.DeadDay.YearMoon = ConvertHelper.CnvNullToInt(txtYearDDLunar.Text.Trim());
                    objMember.IsLeapMonthDD = (chkLeapMonthDDLunar.Checked == true) ? true : false;
                    objMember.DeadPlace = txtDeadPlace.Text.Trim();
                }

                objMember.Contact.Address = txtAddress.Text.Trim();
                objMember.Contact.Tel_1 = txtTel1.Text.Trim();
                objMember.Contact.Tel_2 = txtTel2.Text.Trim();
                objMember.Contact.Email_1 = txtEmail1.Text.Trim();
                objMember.Contact.Email_2 = txtEmail2.Text.Trim();
                objMember.Contact.Fax = txtFax.Text.Trim();
                objMember.Contact.Website = txtWebsite.Text.Trim();

                objMember.strBackColor = ColorTranslator.ToHtml(cbbackcolor.Color);
                objMember.strForeColor = ColorTranslator.ToHtml(cbforecolor.Color);
                objMember.UseDefaultColor = ckUseDefaultColor.Checked;
                objMember.ShowStepChild = ckStepChild.Checked;

                if (this.Mode == ModeForm.New)
                {
                    objMember.LevelInFamily = _levelInFamily;

                    var lstMember = tblTMember.ToList();
                    if (lstMember.Count == 0)
                    {
                        objMember.InRootTree = true;
                        objMember.RootID = objMember.Id;
                    }
                    else
                    {
                        objMember.InRootTree = inRootTree;
                        objMember.SpouseInRootTree = spouseInRootTree;
                        if (primaryMember != null)
                        {
                            objMember.RootID = string.IsNullOrEmpty(primaryMember.Id) ? objMember.Id : primaryMember.Id;
                        }
                        else
                        {
                            objMember.RootID = objMember.Id;
                        }
                    }
                }
                objMember = UpdateFamilyLevel(objMember);

                objMember = UpdateFamilyHead(objMember);

                objMember = UpdateMemberContact(objMember);

                objMember = UpdateMemberJob(objMember);

                objMember = UpdateMemberShool(objMember);

                objMember = UpdateMemberEvent(objMember);

                var rst = actionInsert ? tblTMember.InsertOne(objMember, false)
                                       : tblTMember.UpdateOne(i => i.Id == objMember.Id, objMember);

                if (!string.IsNullOrEmpty(imgAvatarUrl))
                {
                    var objFile = new FileInfo(imgAvatarUrl);
                    if (objFile.Exists)
                    {
                        // save avatar image
                        var strAvatarFolder = Directory.GetCurrentDirectory() + AppConst.AvatarFolderPath;
                        var strAvatarFolderRaw = Directory.GetCurrentDirectory() + AppConst.AvatarRawPath;
                        var strAvatarFolderThumbnail = Directory.GetCurrentDirectory() + AppConst.AvatarThumbnailPath;

                        // check if diẻctory folder backup empty
                        if (!Directory.Exists(strAvatarFolder))
                        {
                            Directory.CreateDirectory(strAvatarFolder);
                        }

                        if (!Directory.Exists(strAvatarFolderRaw))
                        {
                            Directory.CreateDirectory(strAvatarFolderRaw);
                        }

                        if (!Directory.Exists(strAvatarFolderThumbnail))
                        {
                            Directory.CreateDirectory(strAvatarFolderThumbnail);
                        }

                        // delete old img
                        if (!string.IsNullOrEmpty(oldImg))
                        {
                            var pathsDelete = new string[]
                            {
                                strAvatarFolderRaw + "\\" + oldImg,                                                         // Image Old
                                strAvatarFolderThumbnail + "\\" + oldImg,                                                   // Thumbnail Avatar
                                strAvatarFolderThumbnail + "\\" + string.Format(AppConst.FormatNameThumbnailTree, oldImg),  // Thumbnail Tree
                            };

                            foreach (var pathDelete in pathsDelete)
                            {
                                if (File.Exists(pathDelete))
                                {
                                    File.Delete(pathDelete);
                                }
                            }
                        }

                        // save raw image
                        var fileName = objMember.Id + objFile.Extension;
                        var destinationRaw = strAvatarFolderRaw + "\\" + fileName;
                        File.Copy(imgAvatarUrl, destinationRaw, true);

                        // save thumbnail image
                        var thumbnailImg = avatarImage.Image;
                        var destinationThumbnail = strAvatarFolderThumbnail + fileName;
                        FileHepler.SaveImage(thumbnailImg, destinationThumbnail);

                        // make thumbnail for tree
                        using (var imgThumbTree = FileHepler.ResizeImage(new Bitmap(imgAvatarUrl), 512))
                        {
                            var destinationThumbnailTree = strAvatarFolderThumbnail + String.Format(AppConst.FormatNameThumbnailTree, fileName);
                            FileHepler.SaveImage(imgThumbTree, destinationThumbnailTree);
                        }
                    }
                }

                if (rst == false)
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                if (actionInsert)
                {
                    if (primaryMember != null && relationType != null)
                    {
                        addMemberRelation(relationType, primaryMember, objMember);
                    }
                    //if (spousemember != null && relationType2 != null)
                    //{
                    //    addMemberRelation(relationType2, spousemember, objMember);
                    //}
                }
                this.Cursor = Cursors.Default;

                //if (bUpdateLevelInFamily)
                //{
                //    UpdateLevelInFamily();
                //}

                NavigationResult naviResult = new NavigationResult()
                {
                    Result = DialogResult.OK
                };

                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var objRelationType = tblMRelation.FirstOrDefault(i => i.Id == relationType);

                naviResult.Add("newmember", objMember);

                this.Close(naviResult);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private bool ValidateInfoMember()
        {
            bool validateOK = true;
            string messErr = ValidateContact();
            validateOK = string.IsNullOrEmpty(messErr);
            if (!validateOK)
            {
                AppManager.Dialog.Error(messErr);
            }
            return validateOK;
        }
        private string ValidateContact()
        {
            string phoneNumber1 = txtTel1.Text;
            string phoneNumber2 = txtTel2.Text;
            if (!string.IsNullOrEmpty(phoneNumber1) && (phoneNumber1.Length < 10))
            {
                txtTel1.FocusAndSelected();
                return "Số điện thoại không nhập hoặc ít nhất là 10 số.\nXin vui lòng nhập lại!";
            }

            if (!string.IsNullOrEmpty(phoneNumber2) && (phoneNumber2.Length < 10))
            {
                txtTel2.FocusAndSelected();
                return "Số điện thoại không nhập hoặc ít nhất là 10 số.\nXin vui lòng nhập lại!";
            }

            string messEmail1 = ValidateEmail(txtEmail1);
            if (!string.IsNullOrEmpty(messEmail1))
            {
                return messEmail1;
            }

            string messEmail2 = ValidateEmail(txtEmail2);
            if (!string.IsNullOrEmpty(messEmail2))
            {
                return messEmail2;
            }

            return "";
        }

        string ValidateEmail(MaterialTextBox textBoxEmail)
        {
            string email1 = textBoxEmail.Text;
            if (!string.IsNullOrEmpty(email1))
            {
                bool emailFail = false;
                string[] arrTemp = email1.Split('@');
                if (arrTemp.Length == 2)
                {
                    string strTemp = arrTemp[1];
                    string[] arrDomain = strTemp.Split('.');
                    if (arrDomain.Length < 2)
                    {
                        emailFail = true;
                    }
                    else if (arrDomain.Length == 2)
                    {
                        emailFail = !(arrDomain[1].ToLower().Equals("com") || arrDomain[1].ToLower().Equals("vn"));
                    }
                    else if (arrDomain.Length == 3)
                    {
                        emailFail = !(arrDomain[1].ToLower().Equals("com") || arrDomain[2].ToLower().Equals("vn"));
                    }
                }
                else
                {
                    emailFail = true;
                }
                if (emailFail)
                {
                    textBoxEmail.FocusAndSelected();
                    return "Sai định dạng email. Xin vui lòng nhập lại hoặc bỏ qua!";
                }
            }
            return "";
        }

        private TMember UpdateFamilyLevel(TMember member)
        {
            if (typeMember != null && typeMember.Equals(AppConst.NameDefaul.Child))
            {
                member.InRootTree = (primaryMember != null && primaryMember.InRootTree) || (submember != null && submember.InRootTree);
                if (primaryMember != null && primaryMember.InRootTree)
                {
                    member.LevelInFamilyOfFather = (!string.IsNullOrEmpty(primaryMember.LevelInFamilyOfFather) ? $"{primaryMember.LevelInFamilyOfFather}." : "") + $"{primaryMember.LevelInFamilyOfMother}";
                    int cntSpouse = primaryMember.ListSPOUSE.IndexOf(submember?.Id ?? "") + 1;
                    member.LevelInFamilyOfMother = $"[{(cntSpouse == 0 ? "_" : cntSpouse.ToString().PadLeft(2, '0'))}.{ member.ChildLevelInFamily.ToString().PadLeft(2, '0')}]";
                }
                else if (submember != null && submember.InRootTree)
                {
                    member.LevelInFamilyOfFather = (!string.IsNullOrEmpty(submember.LevelInFamilyOfFather) ? $"{submember.LevelInFamilyOfFather}." : "") + $"{submember.LevelInFamilyOfMother}";
                    int cntSpouse = submember.ListSPOUSE.IndexOf(primaryMember?.Id ?? "") + 1;
                    member.LevelInFamilyOfMother = $"[{(cntSpouse == 0 ? "_" : cntSpouse.ToString().PadLeft(2, '0'))}.{ member.ChildLevelInFamily.ToString().PadLeft(2, '0')}]";
                }
            }
            return member;
        }

        private TMember UpdateFamilyHead(TMember member)
        {
            if (ckRootMember.Checked)
            {
                memberHelper.AddRootMember(member.Id);
            }
            else
            {
                var rootMember = memberHelper.RootTMember;
                if (rootMember != null)
                {
                    if (member.Id == rootMember.Id)
                    {
                        memberHelper.RemoveRootMember(member.Id);
                    }
                }
            }
            if (ckFamilyHead.Checked)
            {
                memberHelper.AddFamilyHead(member.Id);
            }
            else
            {
                memberHelper.RemoveFamilyHead(member.Id);
            }
            return member;
        }

        private TMember UpdateMemberContact(TMember member)
        {
            foreach (var socialNetwork in _mappingControlSocial)
            {
                if (member.Contact.SocialNetwork.ContainsKey(socialNetwork.Key))
                {
                    member.Contact.SocialNetwork[socialNetwork.Key] = socialNetwork.Value.Text;
                }
                else
                {
                    member.Contact.SocialNetwork.Add(socialNetwork.Key, socialNetwork.Value.Text);
                }
            }
            member.Contact.Note = txtNote.Text.Trim();
            return member;
        }

        private TMember UpdateMemberJob(TMember member)
        {
            if (gLstMemberJob != null)
            {
                member.Job.Clear();
                foreach (var objJob in gLstMemberJob)
                {
                    var newObj = new TMemberJob();
                    newObj.Id = objJob.Id;
                    newObj.CompanyName = objJob.CompanyName;
                    newObj.CompanyAddress = objJob.CompanyAddress;
                    newObj.Position = objJob.Position;
                    newObj.Job = objJob.Job;
                    newObj.StartDate = objJob.StartDate;
                    newObj.EndDate = objJob.EndDate;
                    member.Job.Add(newObj);
                }
            }
            return member;
        }

        private TMember UpdateMemberShool(TMember member)
        {
            if (gLstMemberSchool != null)
            {
                member.School.Clear();
                foreach (var objSchool in gLstMemberSchool)
                {
                    var newObj = new TMemberSchool();
                    newObj.Id = objSchool.Id;
                    newObj.SchoolName = objSchool.SchoolName;
                    newObj.Description = objSchool.Description;
                    newObj.StartDate = objSchool.StartDate;
                    newObj.EndDate = objSchool.EndDate;
                    member.School.Add(newObj);
                }
            }
            return member;
        }

        private TMember UpdateMemberEvent(TMember member)
        {
            if (gLstMemberEvent != null)
            {
                member.Event.Clear();
                foreach (var objEvent in gLstMemberEvent)
                {
                    var newObj = new TMemberEvent();
                    newObj.EventName = objEvent.EventName;
                    newObj.Location = objEvent.Location;
                    newObj.Description = objEvent.Description;
                    newObj.StartDate = objEvent.StartDate;
                    newObj.EndDate = objEvent.EndDate;
                    member.Event.Add(newObj);
                }
            }
            return member;
        }

        private void UpdateLevelInFamily()
        {
            if (AppManager.Dialog.Confirm("Cập nhật lại thứ bậc?"))
            {
                if (memberHelper.RootTMember != null)
                {
                    AppManager.Dialog.ShowProgressBar(progressBar =>
                    {

                        memberHelper.UpdateLevelInFamily(progressBar);

                    }, "Đang cập nhật lại thứ bậc...", $"{AppConst.TitleBarFisrt}Cập nhật thứ bậc");
                }
            }
        }
        public void addMemberRelation(string idTypeRelation, TMember objMainMember, TMember objRelatedMember)
        {
            try
            {
                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();

                var objMemberRelation = new TMemberRelation();
                var objMemberRelatedRelation = new TMemberRelation();
                var objRelationType = tblMRelation.FirstOrDefault(i => i.Id == idTypeRelation.ToString());

                if (objRelationType == null)
                {
                    return;
                }

                var objNewMainMember = tblTMember.FirstOrDefault(i => i.Id == objMainMember.Id);
                if (objNewMainMember == null)
                {
                    return;
                }
                // Main member
                var objUserRelation = tblTMemberRelation.CreateQuery().Where(i => i.memberId == objMainMember.Id && i.relType == objRelationType.MainRelation);
                var countObjUserRelation = objUserRelation.Count();

                objMemberRelation.memberId = objNewMainMember.Id;
                objMemberRelation.relMemberId = objRelatedMember.Id;
                objMemberRelation.relType = objRelationType.MainRelation;
                objMemberRelation.roleOrder = (countObjUserRelation > 0) ? countObjUserRelation += 1 : 1;

                if (!tblTMemberRelation.InsertOne(objMemberRelation))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                // insert relation of tmember
                if (objRelationType.MainRelation.Contains(Relation.PREFIX_DAD) || objRelationType.MainRelation.Contains(Relation.PREFIX_MOM))
                {
                    objNewMainMember.ListPARENT.Add(objMemberRelation.relMemberId);
                }
                else if (objRelationType.MainRelation.Contains(Relation.PREFIX_HUSBAND) || objRelationType.MainRelation.Contains(Relation.PREFIX_WIFE))
                {
                    objNewMainMember.ListSPOUSE.Add(objMemberRelation.relMemberId);
                }
                else if (objRelationType.MainRelation.Contains(Relation.PREFIX_CHILD))
                {
                    objNewMainMember.ListCHILDREN.Add(objMemberRelation.relMemberId);
                }
                objNewMainMember.Relation.Add(objMemberRelation);
                // update data relation for main member
                if (!tblTMember.UpdateOne(i => i.Id == objNewMainMember.Id, objNewMainMember))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                // Relate member
                var objUserRelatedRelation = tblTMemberRelation.CreateQuery().Where(i => i.memberId == objRelatedMember.Id && i.relType == objRelationType.RelatedRelation);
                var countObjUseRelatedrRelation = objUserRelatedRelation.Count();

                objMemberRelatedRelation.memberId = objRelatedMember.Id;
                objMemberRelatedRelation.relMemberId = objNewMainMember.Id;
                objMemberRelatedRelation.relType = objRelationType.RelatedRelation;
                objMemberRelatedRelation.roleOrder = (countObjUseRelatedrRelation > 0) ? countObjUseRelatedrRelation += 1 : 1;

                if (!tblTMemberRelation.InsertOne(objMemberRelatedRelation))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                // insert relation of tmember
                if (objRelationType.RelatedRelation.Contains(Relation.PREFIX_DAD) || objRelationType.RelatedRelation.Contains(Relation.PREFIX_MOM))
                {
                    objRelatedMember.ListPARENT.Add(objMemberRelatedRelation.relMemberId);
                }
                else if (objRelationType.RelatedRelation.Contains(Relation.PREFIX_HUSBAND) || objRelationType.RelatedRelation.Contains(Relation.PREFIX_WIFE))
                {
                    objRelatedMember.ListSPOUSE.Add(objMemberRelatedRelation.relMemberId);
                }
                else if (objRelationType.RelatedRelation.Contains(Relation.PREFIX_CHILD))
                {
                    objRelatedMember.ListCHILDREN.Add(objMemberRelatedRelation.relMemberId);
                }
                objRelatedMember.Relation.Add(objMemberRelatedRelation);
                // update data relation for main member
                if (!tblTMember.UpdateOne(i => i.Id == objRelatedMember.Id, objRelatedMember))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private void chkIsDeath_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsDeath.Checked == true)
            {
                //txtDateDDSun.Enabled = true;
                //txtMonthDDSun.Enabled = true;
                //txtYearDDSun.Enabled = true;
                txtDateDDLunar.Enabled = true;
                txtMonthDDLunar.Enabled = true;
                txtYearDDLunar.Enabled = true;
                txtDeadPlace.Enabled = true;

                btnChooseCalendarDDSun.Enabled = true;
                chkLeapMonthDDLunar.Enabled = true;
            }

            if (chkIsDeath.Checked == false)
            {
                //txtDateDDSun.Enabled = false;
                //txtMonthDDSun.Enabled = false;
                //txtYearDDSun.Enabled = false;
                txtDateDDLunar.Enabled = false;
                txtMonthDDLunar.Enabled = false;
                txtYearDDLunar.Enabled = false;
                txtDeadPlace.Enabled = false;

                btnChooseCalendarDDSun.Enabled = false;
                chkLeapMonthDDLunar.Enabled = false;
            }
        }

        private void addMember_Load(object sender, EventArgs e)
        {

        }

        private void txtDateBDSun_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtMonthBDSun_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtYearBDSun_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtDateBDLunar_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtMonthBDLunar_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtYearBDLunar_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtDateDDSun_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtMonthDDSun_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtYearDDSun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtDateDDLunar_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtMonthDDLunar_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtYearDDLunar_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void btnJobClear_Click(object sender, EventArgs e)
        {
            txtCompanyName.Text = "";
            txtCompanyAddress.Text = "";
            txtPosition.Text = "";
            txtJob.Text = "";
            lblJobFrom.Text = cstrLabelTextUndefined;
            lblJobTo.Text = cstrLabelTextUndefined;
            gridJob.ClearSelection();
            idSchoolSelected = "";
        }

        private void btnSchoolClear_Click(object sender, EventArgs e)
        {
            txtSchoolName.Text = "";
            txtSchoolDescription.Text = "";
            lblSchoolFrom.Text = cstrLabelTextUndefined;
            lblSchoolTo.Text = cstrLabelTextUndefined;
            gridSchool.ClearSelection();
            idSchoolSelected = "";
        }

        private void btnEventClear_Click(object sender, EventArgs e)
        {
            txtEvent.Text = "";
            txtEventLocation.Text = "";
            txtEventDescription.Text = "";
            lblEventFrom.Text = cstrLabelTextUndefined;
            lblEventTo.Text = cstrLabelTextUndefined;
            gridEvent.ClearSelection();
            idEventSelected = "";
        }

        private void InsertOrUpdateList<T>(List<T> listData, T data) where T : BaseModel
        {
            var obj = listData.FirstOrDefault(i => i.Id == data.Id);

            if (obj == null)
            {
                listData.Add(obj);
            }
            else
            {
                obj = data;
            }
        }

        private void btnJobSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập tên công ty!");
                    return;
                }
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }
                var objTMemberJob = new ExTMemberJob();
                objTMemberJob.Id = LiteDBManager.CreateNewId();
                if (gLstMemberJob == null)
                {
                    gLstMemberJob = new List<ExTMemberJob>();
                }
                if (idSchoolSelected != "")
                {
                    objTMemberJob = gLstMemberJob.AsEnumerable().FirstOrDefault(i => i.Id == idSchoolSelected);
                    if (objTMemberJob == null)
                    {
                        objTMemberJob = new ExTMemberJob();
                    }
                    objTMemberJob.Id = ((objTMemberJob == null)) ? LiteDBManager.CreateNewId() : idSchoolSelected;
                }

                objTMemberJob.CompanyName = txtCompanyName.Text.Trim();
                objTMemberJob.CompanyAddress = txtCompanyAddress.Text.Trim();
                objTMemberJob.Position = txtPosition.Text.Trim();
                objTMemberJob.Job = txtJob.Text.Trim();
                objTMemberJob.StartDate = DateStringToVNDate(lblJobFrom.Text);
                objTMemberJob.EndDate = DateStringToVNDate(lblJobTo.Text);
                objTMemberJob.TimeShow = objTMemberJob.StartDate?.ToDateSun() + "~" + objTMemberJob.EndDate?.ToDateSun();
                if (idSchoolSelected != "")
                {
                    var objUpdate = gLstMemberJob.FirstOrDefault(i => i.Id == idSchoolSelected);
                    objUpdate = objTMemberJob;
                }
                else
                {
                    gLstMemberJob.Add(objTMemberJob);
                }

                BindingHelper.BindingDataGrid(gridJob, gLstMemberJob);

                // reset form
                idSchoolSelected = "";
                txtCompanyName.Text = "";
                txtCompanyAddress.Text = "";
                txtPosition.Text = "";
                txtJob.Text = "";
                lblJobFrom.Text = cstrLabelTextUndefined;
                lblJobTo.Text = cstrLabelTextUndefined;
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private void btnSchoolSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSchoolName.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập tên trường học!");
                    return;
                }
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }
                var objTMemberSchool = new ExTMemberSchool();
                objTMemberSchool.Id = LiteDBManager.CreateNewId();
                if (gLstMemberSchool == null)
                {
                    gLstMemberSchool = new List<ExTMemberSchool>();
                }
                if (idSchoolSelected != "")
                {
                    objTMemberSchool = gLstMemberSchool.AsEnumerable().FirstOrDefault(i => i.Id == idSchoolSelected);
                    if (objTMemberSchool == null)
                    {
                        objTMemberSchool = new ExTMemberSchool();
                    }
                    objTMemberSchool.Id = ((objTMemberSchool == null)) ? LiteDBManager.CreateNewId() : idSchoolSelected;
                }

                objTMemberSchool.SchoolName = txtSchoolName.Text.Trim();
                objTMemberSchool.Description = txtSchoolDescription.Text.Trim();
                var startDate = DateStringToVNDate(lblSchoolFrom.Text);
                objTMemberSchool.StartDate = startDate;
                var endDate = DateStringToVNDate(lblSchoolTo.Text);
                objTMemberSchool.EndDate = endDate;
                objTMemberSchool.TimeShow = objTMemberSchool.StartDate?.ToDateSun() + "~" + objTMemberSchool.EndDate?.ToDateSun();
                if (idSchoolSelected != "")
                {
                    var objUpdate = gLstMemberSchool.FirstOrDefault(i => i.Id == idSchoolSelected);
                    objUpdate = objTMemberSchool;
                }
                else
                {
                    gLstMemberSchool.Add(objTMemberSchool);
                }

                BindingHelper.BindingDataGrid(gridSchool, gLstMemberSchool);

                // reset form
                idSchoolSelected = "";
                txtSchoolName.Text = "";
                txtSchoolDescription.Text = "";
                lblSchoolFrom.Text = cstrLabelTextUndefined;
                lblSchoolTo.Text = cstrLabelTextUndefined;
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private void btnEventSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEvent.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập tên sự kiện!");
                    return;
                }
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }
                var objTMemberEvent = new ExTMemberEvent();
                objTMemberEvent.Id = LiteDBManager.CreateNewId();
                if (gLstMemberEvent == null)
                {
                    gLstMemberEvent = new List<ExTMemberEvent>();
                }
                if (idEventSelected != "")
                {
                    objTMemberEvent = gLstMemberEvent.AsEnumerable().FirstOrDefault(i => i.Id == idEventSelected);
                    if (objTMemberEvent == null)
                    {
                        objTMemberEvent = new ExTMemberEvent();
                    }
                    objTMemberEvent.Id = ((objTMemberEvent == null)) ? LiteDBManager.CreateNewId() : idEventSelected;
                }

                objTMemberEvent.EventName = txtEvent.Text.Trim();
                objTMemberEvent.Location = txtEventLocation.Text.Trim();
                objTMemberEvent.Description = txtEventDescription.Text.Trim();
                objTMemberEvent.StartDate = DateStringToVNDate(lblEventFrom.Text);
                objTMemberEvent.EndDate = DateStringToVNDate(lblEventTo.Text);

                objTMemberEvent.TimeShow = objTMemberEvent.StartDate?.ToDateSun() + "~" + objTMemberEvent.EndDate?.ToDateSun();
                if (idEventSelected != "")
                {
                    var objUpdate = gLstMemberEvent.FirstOrDefault(i => i.Id == idEventSelected);
                    objUpdate = objTMemberEvent;
                }
                else
                {
                    gLstMemberEvent.Add(objTMemberEvent);
                }
                BindingHelper.BindingDataGrid(gridEvent, gLstMemberEvent);

                // reset form
                idEventSelected = "";
                txtEvent.Text = "";
                txtEventLocation.Text = "";
                txtEventDescription.Text = "";
                lblEventFrom.Text = cstrLabelTextUndefined;
                lblEventTo.Text = cstrLabelTextUndefined;
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private void btnJobDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridJob.SelectedCells.Count > 0 && idSchoolSelected != "")
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                    }
                    var obj = gLstMemberJob.AsEnumerable().FirstOrDefault(i => i.Id == idSchoolSelected);
                    gLstMemberJob.Remove(obj);
                    BindingHelper.BindingDataGrid(gridJob, gLstMemberJob);
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private void btnSchoolDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridSchool.SelectedCells.Count > 0 && idSchoolSelected != "")
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }

                    var obj = gLstMemberSchool.AsEnumerable().FirstOrDefault(i => i.Id == idSchoolSelected);
                    gLstMemberSchool.Remove(obj);
                    BindingHelper.BindingDataGrid(gridSchool, gLstMemberSchool);
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private void btnEventDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEvent.SelectedCells.Count > 0 && idSchoolSelected != "")
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }
                    var obj = gLstMemberEvent.AsEnumerable().FirstOrDefault(i => i.Id == idSchoolSelected);
                    gLstMemberEvent.Remove(obj);
                    BindingHelper.BindingDataGrid(gridEvent, gLstMemberEvent);
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private void gridJob_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                var rowSelected = gridJob.Rows[e.RowIndex].DataBoundItem as TMemberJob;

                if (rowSelected != null)
                {
                    idSchoolSelected = rowSelected.Id;
                    txtCompanyName.Text = rowSelected.CompanyName;
                    txtCompanyAddress.Text = rowSelected.CompanyAddress;
                    txtPosition.Text = rowSelected.Position;
                    txtJob.Text = rowSelected.Job;
                    lblJobFrom.Text = rowSelected.StartDate?.ToDateSun();
                    lblJobTo.Text = rowSelected.EndDate?.ToDateSun();
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private void gridSchool_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                var rowSelected = gridSchool.Rows[e.RowIndex].DataBoundItem as TMemberSchool;

                if (rowSelected != null)
                {
                    idSchoolSelected = rowSelected.Id;
                    txtSchoolName.Text = rowSelected.SchoolName;
                    txtSchoolDescription.Text = rowSelected.Description;
                    lblSchoolFrom.Text = rowSelected.StartDate?.ToDateSun();
                    lblSchoolTo.Text = rowSelected.EndDate?.ToDateSun();
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private void gridEvent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                var rowSelected = gridEvent.Rows[e.RowIndex].DataBoundItem as TMemberEvent;

                if (rowSelected != null)
                {
                    idEventSelected = rowSelected.Id;
                    txtEvent.Text = rowSelected.EventName;
                    txtEventLocation.Text = rowSelected.Location;
                    txtEventDescription.Text = rowSelected.Description;
                    lblEventFrom.Text = rowSelected.StartDate?.ToDateSun();
                    lblEventTo.Text = rowSelected.EndDate?.ToDateSun();
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
            }
        }

        private MaterialTextBox CreateTextboxSocial(string NameTextbox, string Hint, Point Location, Size Size)
        {
            var component = new MaterialTextBox();
            component.BorderStyle = BorderStyle.None;
            component.Depth = 0;
            component.Font = new Font("Roboto", 12F);
            component.Hint = Hint;
            component.Location = Location;
            component.MaxLength = 50;
            component.MouseState = MouseState.OUT;
            component.Multiline = false;
            component.Name = NameTextbox;
            component.Size = Size;
            component.TabIndex = 5;
            component.Text = "";

            return component;
        }

        private void avatarImage_Click(object sender, EventArgs e)
        {
            var strPath = AppManager.Dialog.OpenFile("Image Files(*.JPG;*.PNG;)|*.JPG;*.PNG;");
            var file = !string.IsNullOrEmpty(strPath) ? new FileInfo(strPath) : null;

            if (file == null || !file.Exists)
            {
                return;
            }

            if (avatarImage.Image != null)
            {
                avatarImage.Image.Dispose();
                avatarImage.Image = null;
            }

            imgAvatarUrl = strPath;
            avatarImage.Image = FileHepler.ResizeImage(new Bitmap(imgAvatarUrl), avatarImage.Width, avatarImage.Height);
            ChangeTypeTree();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            bool bDay = int.TryParse(txtDateBDSun.Text, out int day);
            bool bMonth = int.TryParse(txtMonthBDSun.Text, out int month);
            bool bYear = int.TryParse(txtYearBDSun.Text, out int year);

            DateTime dateTemp = DateTime.Now;
            if (bDay && bMonth && bYear)
            {
                dateTemp = new DateTime(year, month, day);
            }
            CalendarVN calendarVN = new CalendarVN(dateTemp);

            if (calendarVN.ShowDialog() == DialogResult.OK)
            {
                var calendar = calendarVN.SelectLunarDate;

                DateTime dateTime = calendar.GetSolarDate(calendar.intLunarDay, calendar.intLunarMonth, calendar.intLunarYear);
                txtDateBDSun.Text = dateTime.Day.ToString().PadLeft(2, '0');
                txtMonthBDSun.Text = dateTime.Month.ToString().PadLeft(2, '0');
                txtYearBDSun.Text = dateTime.Year.ToString().PadLeft(2, '0');

                txtDateBDLunar.Text = calendar.intLunarDay.ToString().PadLeft(2, '0');
                txtMonthBDLunar.Text = calendar.intLunarMonth.ToString().PadLeft(2, '0');
                txtYearBDLunar.Text = calendar.intLunarYear.ToString().PadLeft(2, '0');
            }
        }

        private void btnChooseCalendarDDSun_Click(object sender, EventArgs e)
        {
            bool bDay = int.TryParse(txtDateDDSun.Text, out int day);
            bool bMonth = int.TryParse(txtMonthDDSun.Text, out int month);
            bool bYear = int.TryParse(txtYearDDSun.Text, out int year);

            DateTime dateTemp = DateTime.Now;
            if (bDay && bMonth && bYear)
            {
                dateTemp = new DateTime(year, month, day);
            }
            CalendarVN calendarVN = new CalendarVN(dateTemp);

            if (calendarVN.ShowDialog() == DialogResult.OK)
            {
                var calendar = calendarVN.SelectLunarDate;
                DateTime dateTime = calendar.GetSolarDate(calendar.intLunarDay, calendar.intLunarMonth, calendar.intLunarYear);

                txtDateDDSun.Text = dateTime.Day.ToString().PadLeft(2, '0');
                txtMonthDDSun.Text = dateTime.Month.ToString().PadLeft(2, '0');
                txtYearDDSun.Text = dateTime.Year.ToString().PadLeft(2, '0');

                txtDateDDLunar.Text = calendar.intLunarDay.ToString().PadLeft(2, '0');
                txtMonthDDLunar.Text = calendar.intLunarMonth.ToString().PadLeft(2, '0');
                txtYearDDLunar.Text = calendar.intLunarYear.ToString().PadLeft(2, '0');
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnJobCalendarFrom_Click(object sender, EventArgs e)
        {
            bool bDate = DateTime.TryParse(lblJobFrom.Text, out DateTime dateTemp);
            dateTemp = bDate ? dateTemp : DateTime.Now;
            CalendarVN frmCalendarVN = new CalendarVN(dateTemp);

            if (frmCalendarVN.ShowDialog() == DialogResult.OK)
            {
                var dateTime = frmCalendarVN.SelectSolarDate;
                lblJobFrom.Text = dateTime.ToString("dd/MM/yyyy");
            }
        }

        private void btnJobCalendarTo_Click(object sender, EventArgs e)
        {
            bool bDate = DateTime.TryParse(lblJobTo.Text, out DateTime dateTemp);
            dateTemp = bDate ? dateTemp : DateTime.Now;
            CalendarVN frmCalendarVN = new CalendarVN(dateTemp);

            if (frmCalendarVN.ShowDialog() == DialogResult.OK)
            {
                var dateTime = frmCalendarVN.SelectSolarDate;
                lblJobTo.Text = dateTime.ToString("dd/MM/yyyy");
            }
        }

        private void btnEducationCalendarFrom_Click(object sender, EventArgs e)
        {
            bool bDate = DateTime.TryParse(lblSchoolFrom.Text, out DateTime dateTemp);
            dateTemp = bDate ? dateTemp : DateTime.Now;
            CalendarVN frmCalendarVN = new CalendarVN(dateTemp);

            if (frmCalendarVN.ShowDialog() == DialogResult.OK)
            {
                var dateTime = frmCalendarVN.SelectSolarDate;
                lblSchoolFrom.Text = dateTime.ToString("dd/MM/yyyy");
            }
        }

        private void btnEducationCalendarTo_Click(object sender, EventArgs e)
        {
            bool bDate = DateTime.TryParse(lblSchoolTo.Text, out DateTime dateTemp);
            dateTemp = bDate ? dateTemp : DateTime.Now;
            CalendarVN frmCalendarVN = new CalendarVN(dateTemp);

            if (frmCalendarVN.ShowDialog() == DialogResult.OK)
            {
                var dateTime = frmCalendarVN.SelectSolarDate;
                lblSchoolTo.Text = dateTime.ToString("dd/MM/yyyy");
            }
        }

        private void btnEventCalendarFrom_Click(object sender, EventArgs e)
        {
            bool bDate = DateTime.TryParse(lblEventFrom.Text, out DateTime dateTemp);
            dateTemp = bDate ? dateTemp : DateTime.Now;
            CalendarVN frmCalendarVN = new CalendarVN(dateTemp);

            if (frmCalendarVN.ShowDialog() == DialogResult.OK)
            {
                var dateTime = frmCalendarVN.SelectSolarDate;
                lblEventFrom.Text = dateTime.ToString("dd/MM/yyyy");
            }
        }

        private void btnEventCalendarTo_Click(object sender, EventArgs e)
        {
            bool bDate = DateTime.TryParse(lblEventTo.Text, out DateTime dateTemp);
            dateTemp = bDate ? dateTemp : DateTime.Now;
            CalendarVN frmCalendarVN = new CalendarVN(dateTemp);

            if (frmCalendarVN.ShowDialog() == DialogResult.OK)
            {
                var dateTime = frmCalendarVN.SelectSolarDate;
                lblEventTo.Text = dateTime.ToString("dd/MM/yyyy");
            }
        }

        private void CreateShowName()
        {
            string fullName = txtFullName.Text;
            if (fullName.Length <= txtShowName.MaxLength)
            {
                txtShowName.Text = fullName;
            }
            else
            {
                string[] arrTemp = fullName.Split(' ');
                if (arrTemp.Length == 1)
                {
                    txtShowName.Text = fullName;
                }
                else
                {
                    string firstName = arrTemp[0];
                    string midName = "";
                    for (int i = 1; i < arrTemp.Length - 1; i++)
                    {
                        string text = arrTemp[i];
                        if (string.IsNullOrEmpty(text.Trim()))
                        {
                            continue;
                        }
                        midName += $"{text.Substring(0, 1).ToUpper()}.";
                    }
                    string lastName = arrTemp[arrTemp.Length - 1];
                    txtShowName.Text = $"{firstName} {midName.Substring(0, midName.Length - 1)} {lastName}";
                }
            }
        }


        private void txtFullName_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtFullName_TextChanged(object sender, EventArgs e)
        {
            if (bLoad)
            {
                CreateShowName();
            }
        }

        private void txtShowName_TextChanged(object sender, EventArgs e)
        {
            ChangeTypeTree();
        }

        private void ckStepChild_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTypeTree();
        }

        private void cboTypeTree_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTypeTree();
        }
        clsConst.ENUM_MEMBER_TEMPLATE cardCurrent;
        private void ChangeTypeTree()
        {
            var selected = cboTypeTree.SelectedValue?.ToString() + "";
            clsConst.ENUM_MEMBER_TEMPLATE emTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
            Enum.TryParse(selected, out emTemplate);

            if (cardCurrent == null || cardCurrent != emTemplate)
            {
                cardCurrent = emTemplate;
                pnlCard.Controls.Clear();
            }

            switch (emTemplate)
            {
                case clsConst.ENUM_MEMBER_TEMPLATE.MCTFull:
                    if (memberCardFull == null) break;
                    if (pnlCard.Controls.Count == 0)
                    {
                        pnlCard.Controls.Add(memberCardFull);
                    }
                    memberCardFull.BackColor = cbbackcolor.Color;
                    memberCardFull.ForeColor = cbforecolor.Color;
                    memberCardFull.lblFullName.Text = ckStepChild.Checked ? AppConst.StepChild : txtShowName.Text;
                    memberCardFull.picImage.Image = avatarImage.Image;
                    memberCardFull.lblBirthDate.Text = $"{NormalizeDate(txtDateBDSun.Text)}/{NormalizeDate(txtMonthBDSun.Text)}/{NormalizeDate(txtYearBDSun.Text)}";
                    memberCardFull.lblDeadDate.Text = $"{NormalizeDate(txtDateDDLunar.Text)}/{NormalizeDate(txtMonthDDLunar.Text)}/{NormalizeDate(txtYearDDLunar.Text)}";
                    memberCardFull.lblLevel.Visible = false;
                    break;
                case clsConst.ENUM_MEMBER_TEMPLATE.MCTInput:
                    if (memberCardInput == null) break;
                    if (pnlCard.Controls.Count == 0)
                    {
                        pnlCard.Controls.Add(memberCardInput);
                    }
                    memberCardInput.BackColor = cbbackcolor.Color;
                    memberCardInput.ForeColor = cbforecolor.Color;
                    memberCardInput.lblFullName.Text = ckStepChild.Checked ? AppConst.StepChild : txtShowName.Text;
                    memberCardInput.picImage.Image = avatarImage.Image;
                    memberCardInput.lblBirthDate.Text = $"{NormalizeDate(txtDateBDSun.Text)}/{NormalizeDate(txtMonthBDSun.Text)}/{NormalizeDate(txtYearBDSun.Text)}";
                    memberCardInput.lblDeadDate.Text = $"{NormalizeDate(txtDateDDLunar.Text)}/{NormalizeDate(txtMonthDDLunar.Text)}/{NormalizeDate(txtYearDDLunar.Text)}";
                    memberCardInput.lblHomeTown.Text = txtHometown.Text;
                    memberCardInput.lblLevel.Visible = false;
                    break;
                case clsConst.ENUM_MEMBER_TEMPLATE.MCTShort:
                    if (memberCardShort == null) break;
                    if (pnlCard.Controls.Count == 0)
                    {
                        pnlCard.Controls.Add(memberCardShort);
                    }
                    memberCardShort.BackColor = cbbackcolor.Color;
                    memberCardShort.ForeColor = cbforecolor.Color;
                    memberCardShort.lblFullName.Text = ckStepChild.Checked ? AppConst.StepChild : txtShowName.Text;
                    memberCardShort.lblBirthDate.Text = $"{NormalizeDate(txtDateBDSun.Text)}/{NormalizeDate(txtMonthBDSun.Text)}/{NormalizeDate(txtYearBDSun.Text)}";
                    memberCardShort.lblDeadDate.Text = $"{NormalizeDate(txtDateDDLunar.Text)}/{NormalizeDate(txtMonthDDLunar.Text)}/{NormalizeDate(txtYearDDLunar.Text)}";
                    memberCardShort.lblLevel.Visible = false;
                    break;
                case clsConst.ENUM_MEMBER_TEMPLATE.MCTTall:
                    if (memberCardTall == null) break;
                    if (pnlCard.Controls.Count == 0)
                    {
                        pnlCard.Controls.Add(memberCardTall);
                    }
                    memberCardTall.BackColor = cbbackcolor.Color;
                    memberCardTall.ForeColor = cbforecolor.Color;
                    memberCardTall.lblFullName.Text = ckStepChild.Checked ? AppConst.StepChild : txtShowName.Text;
                    memberCardTall.lblLevel.Visible = false;
                    break;
                case clsConst.ENUM_MEMBER_TEMPLATE.MCTVeryShort:
                    if (memberCardVeryShort == null) break;
                    if (pnlCard.Controls.Count == 0)
                    {
                        pnlCard.Controls.Add(memberCardVeryShort);
                    }
                    memberCardVeryShort.BackColor = cbbackcolor.Color;
                    memberCardVeryShort.ForeColor = cbforecolor.Color;
                    memberCardVeryShort.lblFullName.Text = ckStepChild.Checked ? AppConst.StepChild : txtShowName.Text;
                    memberCardVeryShort.lblLevel.Visible = false;
                    break;
            }

            pnlCard.Controls.Add(lblnote);
        }

        private string NormalizeDate(string date)
        {
            return string.IsNullOrEmpty(date) ? "_" : date.PadLeft(2, '0');
        }

        private VNDate DateStringToVNDate(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime dt))
            {
                LunarCalendar lunar = new LunarCalendar(dt);
                return new VNDate()
                {
                    DaySun = dt.Day,
                    MonthSun = dt.Month,
                    YearSun = dt.Year,
                    DayMoon = lunar.intLunarDay,
                    MonthMoon = lunar.intLunarMonth,
                    YearMoon = lunar.intLunarYear,
                    LeapMonth = lunar.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth
                };
            }
            else
            {
                return null;
            }
        }
        private void gridSchool_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtDateBDSun_TextChanged(object sender, EventArgs e)
        {
            MaterialTextBox textbox = sender as MaterialTextBox;

            if (textbox == txtDateBDSun && textbox.Text.Length == 2)
            {
                txtMonthBDSun.FocusAndSelected();
            }
            else if (textbox == txtMonthBDSun)
            {
                if (textbox.Text.Length == 2)
                {
                    txtYearBDSun.FocusAndSelected();
                }
            }
            else if (textbox == txtYearBDSun)
            {

            }

            if (txtYearBDSun.Text.Length < 4)
            {
                txtDateBDLunar.Text = "";
                txtMonthBDLunar.Text = "";
                txtYearBDLunar.Text = "";
                return;
            }
            if (ConvertDateSunToDateMoon(txtDateBDSun, txtMonthBDSun, txtYearBDSun, out LunarCalendar lunarCalendar))
            {
                txtDateBDLunar.Text = lunarCalendar.intLunarDay.ToString().PadLeft(2, '0');
                txtMonthBDLunar.Text = lunarCalendar.intLunarMonth.ToString().PadLeft(2, '0');
                txtYearBDLunar.Text = lunarCalendar.intLunarYear.ToString().PadLeft(2, '0');
                chkLeapMonthBDLunar.Checked = lunarCalendar.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth;
                chkLeapMonthBDLunar.Visible = lunarCalendar.GetLeapMonth(lunarCalendar.intLunarYear) == lunarCalendar.intLunarMonth;
            }
            else
            {
                txtDateBDLunar.Text = "";
                txtMonthBDLunar.Text = "";
                txtYearBDLunar.Text = "";
            }
        }

        private void txtDateDDSun_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDateDDLunar_TextChanged(object sender, EventArgs e)
        {
            MaterialTextBox textbox = sender as MaterialTextBox;
            LunarCalendar lunarCalendar = new LunarCalendar();

            if (textbox == txtDateDDLunar && textbox.Text.Length == 2)
            {
                txtMonthDDLunar.FocusAndSelected();
            }
            else if (textbox == txtMonthDDLunar)
            {
                if (textbox.Text.Length == 2)
                {
                    txtYearDDLunar.FocusAndSelected();
                }
            }
            else if (textbox == txtYearDDLunar)
            {

            }

            if (txtYearDDLunar.Text.Length < 4)
            {
                txtDateDDSun.Text = "";
                txtMonthDDSun.Text = "";
                txtYearDDSun.Text = "";
                return;
            }
            if (ConvertDateMoonToDateSun(txtDateDDLunar, txtMonthDDLunar, txtYearDDLunar, chkLeapMonthDDLunar, out DateTime dateTime))
            {
                txtDateDDSun.Text = dateTime.Day.ToString().PadLeft(2, '0');
                txtMonthDDSun.Text = dateTime.Month.ToString().PadLeft(2, '0');
                txtYearDDSun.Text = dateTime.Year.ToString().PadLeft(2, '0');

                int.TryParse(txtMonthDDLunar.Text, out int month);
                int.TryParse(txtYearDDLunar.Text, out int year);
                chkLeapMonthDDLunar.Visible = lunarCalendar.GetLeapMonth(year) == month;
            }
            else
            {
                txtDateDDSun.Text = "";
                txtMonthDDSun.Text = "";
                txtYearDDSun.Text = "";
            }
        }

        private void txtDateBDLunar_TextChanged(object sender, EventArgs e)
        {

        }
        private bool ConvertDateSunToDateMoon(MaterialTextBox txtDateSun, MaterialTextBox txtMonthSun, MaterialTextBox txtYearSun, out LunarCalendar lunarCalendar)
        {
            string sdaySun = txtDateSun.Text;
            string smonthSun = txtMonthSun.Text;
            string syearSun = txtYearSun.Text;
            if (string.IsNullOrEmpty(sdaySun) || string.IsNullOrEmpty(smonthSun) || string.IsNullOrEmpty(syearSun) || syearSun.Length < 4)
            {
                var lunar = new LunarCalendar();
                lunarCalendar = lunar;
                return false;
            }
            int daySun = int.Parse(sdaySun);
            if (daySun < 1)
            {
                var lunar = new LunarCalendar();
                lunarCalendar = lunar;
                return false;
            }
            int monthSun = int.Parse(smonthSun);
            if (monthSun < 1)
            {
                var lunar = new LunarCalendar();
                lunarCalendar = lunar;
                return false;
            }
            int yearSun = int.Parse(syearSun);
            if (yearSun < 1)
            {
                var lunar = new LunarCalendar();
                lunarCalendar = lunar;
                return false;
            }
            int dayInMonth = DateTime.DaysInMonth(yearSun, monthSun);
            if (daySun > dayInMonth)
            {
                AppManager.Dialog.Error($"Tháng {monthSun.ToString().PadLeft(2, '0')} (Dương lịch) có {dayInMonth.ToString().PadLeft(2, '0')} ngày.");
                {
                    var lunar = new LunarCalendar();
                    lunarCalendar = lunar;
                    txtDateSun.FocusAndSelected();
                    return false;
                }
            }
            else
            {
                var lunar = new LunarCalendar(daySun, monthSun, yearSun);
                lunarCalendar = lunar;
                return true;
            }
        }

        private bool ConvertDateMoonToDateSun(MaterialTextBox txtDateMoon, MaterialTextBox txtMonthMoon, MaterialTextBox txtYearMoon, MaterialCheckbox chkLeapMoon, out DateTime dateTime)
        {
            string sdayMoon = txtDateMoon.Text;
            string smonthMoon = txtMonthMoon.Text;
            string syearMoon = txtYearMoon.Text;

            if (string.IsNullOrEmpty(sdayMoon) || string.IsNullOrEmpty(smonthMoon) || string.IsNullOrEmpty(syearMoon) || syearMoon.Length < 4)
            {
                var date = new DateTime();
                dateTime = date;
                return false;
            }
            int dayMoon = int.Parse(sdayMoon);
            if (dayMoon < 1)
            {
                var date = new DateTime();
                dateTime = date;
                return false;
            }
            int monthMoon = int.Parse(smonthMoon);
            if (monthMoon < 1)
            {
                var date = new DateTime();
                dateTime = date;
                return false;
            }
            int yearMoon = int.Parse(syearMoon);
            if (yearMoon < 1)
            {
                var date = new DateTime();
                dateTime = date;
                return false;
            }


            var lunar = new LunarCalendar();
            int dayInMonth = lunar.GetLunarMonthDays(monthMoon, yearMoon, chkLeapMoon.Checked);

            if (dayMoon > dayInMonth)
            {
                AppManager.Dialog.Error($"Tháng {monthMoon.ToString().PadLeft(2, '0')} (Âm lịch) có {dayInMonth.ToString().PadLeft(2, '0')} ngày.");
                {
                    var date = new DateTime();
                    dateTime = date;
                    txtDateMoon.FocusAndSelected();
                    return false;
                }
            }
            else
            {
                DateTime dt = lunar.GetSolarDate(dayMoon, monthMoon, yearMoon, chkLeapMoon.Checked);
                dateTime = dt;
                return true;
            }
        }

        private void txtDateBDSun_Click(object sender, EventArgs e)
        {

        }

        private void txtDateBDLunar_Click(object sender, EventArgs e)
        {

        }

        private void txtDateBDLunar_KeyPress_1(object sender, KeyPressEventArgs e)
        {

        }

        private void txtDateBDSun_KeyPress_1(object sender, KeyPressEventArgs e)
        {

        }

        private void txtEmail1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string exception = @"[]^<>()\/?,;:{}!#$%&*+-=|'""";
            string character = e.KeyChar.ToString();
            if (exception.Contains(character))
            {
                e.KeyChar = (char)Keys.None;
            }
        }
    }
}