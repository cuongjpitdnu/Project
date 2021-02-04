using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using GP40Main.Utility;
using MaterialSkin;
using MaterialSkin.Controls;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views.Member
{
    public partial class addMember : BaseUserControl
    {
        //public List<string> arrSuggestHomeTown = new List<string>();
        //public List<string> arrSuggestBirthPlace = new List<string>();
        //public List<string> arrSuggestDeadPlace = new List<string>();

        public const string cstrLabelTextUndefined = "Chưa rõ";
        public string idTempSelected = "";
        public List<ExTMemberJob> gLstMemberJob;
        public List<ExTMemberSchool> gLstMemberSchool;
        public List<ExTMemberEvent> gLstMemberEvent;
        public Dictionary<string, MaterialTextBox> _mappingControlSocial = new Dictionary<string, MaterialTextBox>();

        public addMember(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            var newY = txtFax.Location.Y;
            var marginTextbox = Math.Abs(txtEmail1.Location.Y - txtFax.Location.Y);

            var lstSocialNetwork = AppManager.DBManager.GetTable<MSocialNetwork>().CreateQuery().ToList(); ;
            if(lstSocialNetwork != null)
            {
                var index = 0;
                foreach (var obj in lstSocialNetwork)
                {
                    var newLocation = new Point(txtFax.Location.X, newY + marginTextbox);
                    var txtAdd = CreateTextboxSocial("txtSocial"+(++index), obj.SocialName, newLocation, txtAddress.Size);

                    _mappingControlSocial.Add(obj.SocialName, txtAdd);
                    tabPage1.Controls.Add(txtAdd);

                    newY = newY + marginTextbox;
                }
            }

            BindingTableToCombo<MTypeName>(cmbTypeName, "TypeName");
            BindingTableToCombo<MReligion>(cmbReligion, "RelName");
            BindingTableToCombo<MNationality>(cmbNational, "NatName");

            var queryTypeName = AppManager.DBManager.GetTable<MTypeName>().CreateQuery();
            var objTypeNameDefault = queryTypeName.Where(i => i.IsDefault == true).FirstOrDefault();
            if (objTypeNameDefault != null)
            {
                cmbTypeName.SelectedValue = objTypeNameDefault.Id;
            }

            var queryReligion = AppManager.DBManager.GetTable<MReligion>().CreateQuery();
            var objRReligionDefault = queryReligion.Where(i => i.IsDefault == true).FirstOrDefault();
            if (objRReligionDefault != null)
            {
                cmbReligion.SelectedValue = objRReligionDefault.Id;
            }

            var queryNational = AppManager.DBManager.GetTable<MNationality>().CreateQuery();
            var objNationalDefault = queryNational.Where(i => i.IsDefault == true).FirstOrDefault();
            if (objNationalDefault != null)
            {
                cmbNational.SelectedValue = objNationalDefault.Id;
            }

            if (this.GetMode() == ModeForm.Edit)
            {
                var objParam = this.GetParameters().GetValue<TMember>();
                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                if (objParam.Id != "")
                {
                    // check member exist
                    var objMember = tblTMember.CreateQuery(i => i.Id == objParam.Id).FirstOrDefault();
                    if (objMember == null)
                    {
                        return;
                    }

                    //AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
                    //allowedTypes.AddRange(arrSuggestHomeTown.ToArray());
                    //txtHometown.AutoCompleteCustomSource = allowedTypes;
                    //txtHometown.AutoCompleteMode = AutoCompleteMode.Suggest;
                    //txtHometown.AutoCompleteSource = AutoCompleteSource.CustomSource;

                    // binding data to form
                    txtName.Text = objMember.Name.Trim();
                    if (objMember.TypeName.Count > 0)
                    {
                        cmbTypeName.SelectedValue = objMember.TypeName.First().Key ?? "";
                        txtCommonName.Text = objMember.TypeName.First().Value ?? "";
                    }
                    txtChildLevelInFamily.Text = (objMember.ChildLevelInFamily != -1) ? objMember.ChildLevelInFamily.ToString() : "";
                    if (objMember.Gender == (int)GenderMember.Male)
                    {
                        rdoMale.Checked = true;
                    }
                    else
                    {
                        if (objMember.Gender == (int)GenderMember.Female)
                        {
                            rdoFemale.Checked = true;
                        }
                        else
                        {
                            rdoUnknown.Checked = true;
                        }
                    }
                    txtHometown.Text = objMember.HomeTown?.Trim();
                    cmbNational.SelectedValue = objMember.National ?? "";
                    cmbReligion.SelectedValue = objMember.Religion ?? "";

                    txtDateBDSun.Text = (objMember.Birthday.DaySun != -1) ? objMember.Birthday.DaySun.ToString() : "";
                    txtMonthBDSun.Text = (objMember.Birthday.DaySun != -1) ? objMember.Birthday.MonthSun.ToString() : "";
                    txtYearBDSun.Text = (objMember.Birthday.DaySun != -1) ? objMember.Birthday.YearSun.ToString() : "";
                    txtDateBDLunar.Text = (objMember.Birthday.DaySun != -1) ? objMember.Birthday.DayMoon.ToString() : "";
                    txtMonthBDLunar.Text = (objMember.Birthday.DaySun != -1) ? objMember.Birthday.MonthMoon.ToString() : "";
                    txtYearBDLunar.Text = (objMember.Birthday.DaySun != -1) ? objMember.Birthday.YearMoon.ToString() : "";
                    chkLeapMonthBDLunar.Checked = (objMember.IsLeapMonthDB) ? true : false;
                    txtBirthPlace.Text = objMember.BirthPlace?.Trim();

                    if (objMember.IsDeath == true)
                    {
                        txtDateDDSun.Text = (objMember.DeadDay.DaySun != -1) ? objMember.DeadDay.DaySun.ToString() : "";
                        txtMonthDDSun.Text = (objMember.DeadDay.DaySun != -1) ? objMember.DeadDay.MonthSun.ToString() : "";
                        txtYearDDSun.Text = (objMember.DeadDay.DaySun != -1) ? objMember.DeadDay.YearSun.ToString() : "";
                        txtDateDDLunar.Text = (objMember.DeadDay.DaySun != -1) ? objMember.DeadDay.DayMoon.ToString() : "";
                        txtMonthDDLunar.Text = (objMember.DeadDay.DaySun != -1) ? objMember.DeadDay.MonthMoon.ToString() : "";
                        txtYearDDLunar.Text = (objMember.DeadDay.DaySun != -1) ? objMember.DeadDay.YearMoon.ToString() : "";
                        chkLeapMonthBDLunar.Checked = (objMember.IsLeapMonthDD == true) ? true : false;
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

                    if (objMember.Job.Count > 0)
                    {
                        gLstMemberJob = objMember.Job.AsEnumerable().Select(i => new ExTMemberJob
                        {
                            Id = i.Id,
                            CompanyName = i.CompanyName?.Trim(),
                            CompanyAddress = i.CompanyAddress?.Trim(),
                            Position = i.Position?.Trim(),
                            Job = i.Job?.Trim(),
                            TimeShow = ((i.StartDate != null) ? i.StartDate.ToDateSun() : "") + " ~ " + ((i.EndDate != null) ? i.EndDate.ToDateSun() : ""),
                        }).ToList();

                        BindingHelper.BindingDataGrid(gridJob, gLstMemberJob);
                    }

                    if (objMember.School.Count > 0)
                    {
                        gLstMemberSchool = objMember.School.AsEnumerable().Select(i => new ExTMemberSchool
                        {
                            Id = i.Id,
                            SchoolName = i.SchoolName?.Trim(),
                            Description = i.Description?.Trim(),
                            TimeShow = ((i.StartDate != null) ? i.StartDate.ToDateSun() : "") + " ~ " + ((i.EndDate != null) ? i.EndDate.ToDateSun() : ""),
                        }).ToList();
                        BindingHelper.BindingDataGrid(gridSchool, gLstMemberSchool);
                    }

                    if (objMember.Event.Count > 0)
                    {
                        var gLstMemberEvent = objMember.Event.AsEnumerable().Select(i => new ExTMemberEvent
                        {
                            Id = i.Id,
                            EventName = i.EventName?.Trim(),
                            Location = i.Location?.Trim(),
                            Description = i.Description?.Trim(),
                            TimeShow = ((i.StartDate != null) ? i.StartDate.ToDateSun() : "") + " ~ " + ((i.EndDate != null) ? i.EndDate.ToDateSun() : ""),
                        }).ToList();
                        BindingHelper.BindingDataGrid(gridEvent, gLstMemberEvent);
                    }
                }
            }

            // test suggestion
            //var tblTMember = AppManager.DBManager.GetTable<TMember>()
            //var lstMember = tblTMember.CreateQuery().Where(i => i.HomeTown != "" || i.BirthPlace != "" || i.DeadPlace != "");
            //if (lstMember != null)
            //{
            //    foreach (var member in lstMember)
            //    {
            //        if (!string.IsNullOrEmpty(member.HomeTown) && !arrSuggestHomeTown.Contains(member.HomeTown))
            //        {
            //            arrSuggestHomeTown.Add(member.HomeTown);
            //        }
            //        if (!string.IsNullOrEmpty(member.BirthPlace) && !arrSuggestHomeTown.Contains(member.BirthPlace))
            //        {
            //            arrSuggestBirthPlace.Add(member.BirthPlace);
            //        }
            //        if (!string.IsNullOrEmpty(member.DeadPlace) && !arrSuggestHomeTown.Contains(member.DeadPlace))
            //        {
            //            arrSuggestDeadPlace.Add(member.DeadPlace);
            //        }
            //    }
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập tên thành viên!");
                    return;
                }

                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                TMember objMember;
                bool actionInsert = (this.GetMode() == AppConst.ModeForm.Edit) ? false : true;
                var emGender = GenderMember.Unknown;
                if (rdoMale.Checked == true)
                {
                    emGender = GenderMember.Male;
                }
                if (rdoFemale.Checked == true)
                {
                    emGender = GenderMember.Female;
                }

                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                if (actionInsert)
                {
                    objMember = new TMember();
                }
                else
                {
                    var objParam = this.GetParameters().GetValue<TMember>();
                    objMember = tblTMember.CreateQuery(i => i.Id == objParam.Id).FirstOrDefault();
                    if (objMember == null)
                    {
                        return;
                    }
                }

                objMember.Name = txtName.Text.Trim();
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
                objMember.ChildLevelInFamily = ConvertHelper.CnvNullToInt(txtChildLevelInFamily.Text.Trim());

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

                objMember.IsDeath = (chkIsDeath.Checked == true) ? true : false;
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

                foreach (var socialNetwork in _mappingControlSocial)
                {
                    if (objMember.Contact.SocialNetwork.ContainsKey(socialNetwork.Key))
                    {
                        objMember.Contact.SocialNetwork[socialNetwork.Key] = socialNetwork.Value.Text;
                    }
                    else
                    {
                        objMember.Contact.SocialNetwork.Add(socialNetwork.Key, socialNetwork.Value.Text);
                    }
                }
                objMember.Contact.Note = txtNote.Text.Trim();

                if (gLstMemberJob != null)
                {
                    objMember.Job.Clear();
                    foreach(var objJob in gLstMemberJob)
                    {
                        var newObj = new TMemberJob();
                        newObj.Id = objJob.Id;
                        newObj.CompanyName = objJob.CompanyName;
                        newObj.CompanyAddress = objJob.CompanyAddress;
                        newObj.Position = objJob.Position;
                        newObj.Job = objJob.Job;
                        newObj.StartDate = objJob.StartDate;
                        newObj.EndDate = objJob.EndDate;
                        objMember.Job.Add(newObj);
                    }
                }

                if (gLstMemberSchool != null)
                {
                    objMember.School.Clear();
                    foreach(var objSchool in gLstMemberSchool)
                    {
                        var newObj = new TMemberSchool();
                        newObj.Id = objSchool.Id;
                        newObj.SchoolName = objSchool.SchoolName;
                        newObj.Description = objSchool.Description;
                        newObj.StartDate = objSchool.StartDate;
                        newObj.EndDate = objSchool.EndDate;
                        objMember.School.Add(newObj);
                    }
                }

                if (gLstMemberEvent != null)
                {
                    objMember.Event.Clear();
                    foreach (var objEvent in gLstMemberEvent)
                    {
                        var newObj = new TMemberEvent();
                        newObj.EventName = objEvent.EventName;
                        newObj.Location = objEvent.Location;
                        newObj.Description = objEvent.Description;
                        newObj.StartDate = objEvent.StartDate;
                        newObj.EndDate = objEvent.EndDate;
                        objMember.Event.Add(newObj);
                    }
                }

                var rst = actionInsert ? tblTMember.InsertOne(objMember)
                                       : tblTMember.UpdateOne(i => i.Id == objMember.Id, objMember);

                if (rst == false)
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                if (actionInsert)
                {
                    //check param exist to add relation
                    var primaryMember = this.GetParameters().GetValue<TMember>("primary_member");
                    var relationType = this.GetParameters().GetValue<string>("relation_type");
                    if (primaryMember != null && relationType != null)
                    {
                        addMemberRelation(relationType, primaryMember, objMember);
                    }
                }
              
                this.Close();
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(addMember), ex);
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
                var objRelationType = tblMRelation.CreateQuery(i => i.Id == idTypeRelation.ToString()).FirstOrDefault();

                if (objRelationType == null)
                {
                    return;
                }

                var objNewMainMember = tblTMember.CreateQuery(i => i.Id == objMainMember.Id).FirstOrDefault();

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
                if (objRelationType.MainRelation.Contains(cstrPreFixDAD) || objRelationType.MainRelation.Contains(cstrPreFixMOM))
                {
                    objNewMainMember.ListPARENT.Add(objMemberRelation.relMemberId);
                }
                else if (objRelationType.MainRelation.Contains(cstrPreFixHUSBAND) || objRelationType.MainRelation.Contains(cstrPreFixWIFE))
                {
                    objNewMainMember.ListSPOUSE.Add(objMemberRelation.relMemberId);
                }
                else if (objRelationType.MainRelation.Contains(cstrPreFixCHILD))
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
                if (objRelationType.RelatedRelation.Contains(cstrPreFixDAD) || objRelationType.RelatedRelation.Contains(cstrPreFixMOM))
                {
                    objRelatedMember.ListPARENT.Add(objMemberRelatedRelation.relMemberId);
                }
                else if (objRelationType.RelatedRelation.Contains(cstrPreFixHUSBAND) || objRelationType.RelatedRelation.Contains(cstrPreFixWIFE))
                {
                    objRelatedMember.ListSPOUSE.Add(objMemberRelatedRelation.relMemberId);
                }
                else if (objRelationType.RelatedRelation.Contains(cstrPreFixCHILD))
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
                txtDateDDSun.Enabled = true;
                txtMonthDDSun.Enabled = true;
                txtYearDDSun.Enabled = true;
                txtDateDDLunar.Enabled = true;
                txtMonthDDLunar.Enabled = true;
                txtYearDDLunar.Enabled = true;
                txtDeadPlace.Enabled = true;

                btnChooseCalendarDDSun.Enabled = true;
                chkLeapMonthDDLunar.Enabled = true;
            }

            if (chkIsDeath.Checked == false)
            {
                txtDateDDSun.Enabled = false;
                txtMonthDDSun.Enabled = false;
                txtYearDDSun.Enabled = false;
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

        private void txtYearDDSun_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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
            idTempSelected = "";
        }

        private void btnSchoolClear_Click(object sender, EventArgs e)
        {
            txtSchoolName.Text = "";
            txtSchoolDescription.Text = "";
            lblSchoolFrom.Text = cstrLabelTextUndefined;
            lblSchoolTo.Text = cstrLabelTextUndefined;
            gridSchool.ClearSelection();
            idTempSelected = "";
        }

        private void btnEventClear_Click(object sender, EventArgs e)
        {
            txtEvent.Text = "";
            txtEventLocation.Text = "";
            txtEventDescription.Text = "";
            lblEventFrom.Text = cstrLabelTextUndefined;
            lblEventTo.Text = cstrLabelTextUndefined;
            gridEvent.ClearSelection();
            idTempSelected = "";
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

                if (idTempSelected != "")
                {
                    objTMemberJob = gLstMemberJob.AsEnumerable().FirstOrDefault(i => i.Id == idTempSelected);
                    objTMemberJob.Id = ((objTMemberJob == null)) ? LiteDBManager.CreateNewId() : idTempSelected;
                }

                objTMemberJob.CompanyName = txtCompanyName.Text.Trim();
                objTMemberJob.CompanyAddress = txtCompanyAddress.Text.Trim();
                objTMemberJob.Position = txtPosition.Text.Trim();
                objTMemberJob.Job = txtJob.Text.Trim();

                if (idTempSelected != "")
                {
                    var objUpdate = gLstMemberJob.FirstOrDefault(i => i.Id == idTempSelected);
                    objUpdate = objTMemberJob;
                }
                else
                {
                    gLstMemberJob.Add(objTMemberJob);
                }

                BindingHelper.BindingDataGrid(gridJob, gLstMemberJob);

                // reset form
                idTempSelected = "";
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

                if (idTempSelected != "")
                {
                    objTMemberSchool = gLstMemberSchool.AsEnumerable().FirstOrDefault(i => i.Id == idTempSelected);
                    objTMemberSchool.Id = ((objTMemberSchool == null)) ? LiteDBManager.CreateNewId() : idTempSelected;
                }

                objTMemberSchool.SchoolName = txtSchoolName.Text.Trim();
                objTMemberSchool.Description = txtSchoolDescription.Text.Trim();

                if (idTempSelected != "")
                {
                    var objUpdate = gLstMemberSchool.FirstOrDefault(i => i.Id == idTempSelected);
                    objUpdate = objTMemberSchool;
                }
                else
                {
                    gLstMemberSchool.Add(objTMemberSchool);
                }

                BindingHelper.BindingDataGrid(gridSchool, gLstMemberSchool);

                // reset form
                idTempSelected = "";
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

                if (idTempSelected != "")
                {
                    objTMemberEvent = gLstMemberEvent.AsEnumerable().FirstOrDefault(i => i.Id == idTempSelected);
                    objTMemberEvent.Id = ((objTMemberEvent == null)) ? LiteDBManager.CreateNewId() : idTempSelected;
                }

                objTMemberEvent.EventName = txtEvent.Text.Trim();
                objTMemberEvent.Location = txtEventLocation.Text.Trim();
                objTMemberEvent.Description = txtEventDescription.Text.Trim();

                if (idTempSelected != "")
                {
                    var objUpdate = gLstMemberEvent.FirstOrDefault(i => i.Id == idTempSelected);
                    objUpdate = objTMemberEvent;
                }
                else
                {
                    gLstMemberEvent.Add(objTMemberEvent);
                }
                BindingHelper.BindingDataGrid(gridEvent, gLstMemberEvent);

                // reset form
                idTempSelected = "";
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
                if (gridJob.SelectedCells.Count > 0 && idTempSelected != "")
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        
                    }
                    var obj = gLstMemberJob.AsEnumerable().FirstOrDefault(i => i.Id == idTempSelected);
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
                if (gridSchool.SelectedCells.Count > 0 && idTempSelected != "")
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }

                    var obj = gLstMemberSchool.AsEnumerable().FirstOrDefault(i => i.Id == idTempSelected);
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
                if (gridEvent.SelectedCells.Count > 0 && idTempSelected != "")
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }
                    var obj = gLstMemberEvent.AsEnumerable().FirstOrDefault(i => i.Id == idTempSelected);
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
                    idTempSelected = rowSelected.Id;
                    txtCompanyName.Text = rowSelected.CompanyName;
                    txtCompanyAddress.Text = rowSelected.CompanyAddress;
                    txtPosition.Text = rowSelected.Position;
                    txtJob.Text = rowSelected.Job;
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
                    idTempSelected = rowSelected.Id;
                    txtSchoolName.Text = rowSelected.SchoolName;
                    txtSchoolDescription.Text = rowSelected.Description;
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
                    idTempSelected = rowSelected.Id;
                    txtEvent.Text = rowSelected.EventName;
                    txtEventLocation.Text = rowSelected.Location;
                    txtEventDescription.Text = rowSelected.Description;
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
    }
}
