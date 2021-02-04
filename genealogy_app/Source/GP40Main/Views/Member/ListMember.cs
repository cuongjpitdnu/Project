using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GP40Main.Core;
using GP40Main.Views.Member;
using GP40Main.Utility;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using static GP40Main.Core.AppConst;
using System.Drawing;

using Xceed.Document.NET;
using Xceed.Words.NET;
using System.IO;
using GP40Main.Themes;

namespace GP40Main.Views
{
    public partial class ListMember : BaseUserControl
    {
        private ContextMenuStripData<TMember> _contextMember;

        public string strAfterSearchKeyWord = "";
        public int intAfterSearchGender = -1;
        public int intAfterSearchLiveOrDie = -1;

        public ListMember(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            _contextMember = ContextMenuStripManager.CreateForMember();
          //  _contextMember. += (sender, e) => LoadListTMember();

            Dictionary<int, string> dicGender = new Dictionary<int, string>();
            dicGender.Add(-1, "Chọn giới tính");
            dicGender.Add((int)GenderMember.Unknown, "Chưa rõ");
            dicGender.Add((int)GenderMember.Male, "Nam");
            dicGender.Add((int)GenderMember.Female, "Nữ");
            cmbGender.DataSource = new BindingSource(dicGender, null);
            cmbGender.DisplayMember = "Value";
            cmbGender.ValueMember = "Key";

            Dictionary<int, string> dicLOrD = new Dictionary<int, string>();
            dicLOrD.Add(-1, "Chọn trạng thái");
            dicLOrD.Add(1, "Còn sống");
            dicLOrD.Add(0, "Đã mất");
            cmbLiveOrDie.DataSource = new BindingSource(dicLOrD, null);
            cmbLiveOrDie.DisplayMember = "Value";
            cmbLiveOrDie.ValueMember = "Key";

            LoadListTMember();
        }
        private void btn_addEmployee_Click(object sender, EventArgs e)
        {
            try {
                var content = AppManager.Navigation.ShowDialog<addMember>(AppConst.ModeForm.New);
                LoadListTMember();
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        public void LoadListTMember(string keyword = "", string gender = "", string liveOrDie = "", bool inClan = true)
        {
            try {
                var intGender = ConvertHelper.CnvNullToInt(gender);
                var intLiveOrDie = ConvertHelper.CnvNullToInt(liveOrDie);
                var isDeath = intLiveOrDie == 0 ? true : false;

                keyword = keyword.ToLower();

                strAfterSearchKeyWord = keyword;
                intAfterSearchGender = intGender;
                intAfterSearchLiveOrDie = intLiveOrDie;

                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                var test = tblTMember.CreateQuery().ToList();
                var dtaTMember = tblTMember.CreateQuery(
                    i => (intGender < 0 || i.Gender == intGender) // gender
                         && (intLiveOrDie < 0 || i.IsDeath == isDeath) // death
                         && (string.IsNullOrEmpty(keyword) 
                             || i.Name.ToLower().Contains(keyword)
                             || i.Contact.Tel_1.ToLower().Contains(keyword)
                             || i.Contact.Tel_2.ToLower().Contains(keyword)
                             || i.Contact.Email_1.ToLower().Contains(keyword)
                             || i.Contact.Email_2.ToLower().Contains(keyword)
                             || i.Contact.Address.ToLower().Contains(keyword))
                    ).ToList().Select(i => new ExTMember()
                    {
                        Id = i.Id,
                        Name = i.Name + "",
                        Gender = i.Gender,
                        IsDeath = i.IsDeath,
                        GenderShow = (i.Gender == (int)GenderMember.Male) ? "Nam" : ((i.Gender == (int)GenderMember.Female) ? "Nữ" : "Chưa rõ"),
                        BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
                        Tel_1 = i.Contact.Tel_1 + "",
                        Tel_2 = i.Contact.Tel_2 + "",
                        Email_1 = i.Contact.Email_1 + "",
                        Email_2 = i.Contact.Email_2 + "",
                        Address = i.Contact.Address + "",
                        Relation = i.Relation
                    }).ToList();

                BindingHelper.BindingDataGrid(gridListMember, dtaTMember);
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        private void gridListMember_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                var rowSelected = gridListMember.Rows[e.RowIndex].DataBoundItem as TMember;

                if (rowSelected != null)
                {
                    AppManager.Navigation.ShowDialog<addMember, TMember>(new NavigationParameters(rowSelected), AppConst.ModeForm.Edit);
                    LoadListTMember();
                }
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try {
                var strKeyword = txtKeyword.Text.Trim();
                var genderSelected = cmbGender.SelectedValue.ToString();
                var LiveOrDieSelected = cmbLiveOrDie.SelectedValue.ToString();
                var chkInClan = (chkboxInClan.Checked) ? true : false;

                LoadListTMember(strKeyword, genderSelected, LiveOrDieSelected, chkInClan);
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        private void gridListMember_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                var rowSelected = gridListMember.Rows[e.RowIndex].DataBoundItem as TMember;

                if (rowSelected == null)
                {
                    return;
                }

                // view screen relationship of user
                if (e.ColumnIndex == ViewRelationship.Index)
                {
                    AppManager.Navigation.ShowDialog<ListMemberRelation, TMember>(new NavigationParameters(rowSelected), AppConst.ModeForm.None);
                }

                if (e.ColumnIndex == ActionDelete.Index)
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }

                    var tblTMember = AppManager.DBManager.GetTable<TMember>();

                    var objMember = tblTMember.CreateQuery(i => i.Id == rowSelected.Id).FirstOrDefault();
                    if (objMember == null)
                    {
                        return;
                    }

                    // set date delete
                    objMember.DeleteDate = DateTime.Now;

                    // check related member relation to delete (TMemberRelation)
                    var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                    // find all in TMemberRelation
                    var listMemberRelation = tblTMemberRelation.CreateQuery(i => i.memberId == objMember.Id || i.relMemberId == objMember.Id).ToList();
                    if (listMemberRelation != null)
                    {
                        foreach (var member in listMemberRelation)
                        {
                            member.DeleteDate = DateTime.Now;

                            // find member in TMember related with member want to delete -> remove listPARENT/SPOUSE/CHILDREN or props Relation
                            var objMemberRelated = tblTMember.CreateQuery(i => i.Id == ((objMember.Id == member.memberId) ? member.relMemberId : member.memberId)).FirstOrDefault();
                            if (objMemberRelated != null)
                            {
                                // listPARENT - SPOUSE - CHILD
                                if (member.relType.Contains(cstrPreFixDAD) || member.relType.Contains(cstrPreFixMOM))
                                {
                                    objMemberRelated.ListPARENT.Remove(objMember.Id);
                                }
                                else if (member.relType.Contains(cstrPreFixHUSBAND) || member.relType.Contains(cstrPreFixWIFE))
                                {
                                    objMemberRelated.ListSPOUSE.Remove(objMember.Id);
                                }
                                else if (member.relType.Contains(cstrPreFixCHILD))
                                {
                                    objMemberRelated.ListCHILDREN.Remove(objMember.Id);
                                }
                                var findInRelation = objMemberRelated.Relation.FindAll(i => i.memberId == member.memberId && i.relMemberId != member.relMemberId);
                                objMemberRelated.Relation = findInRelation;

                                if (!tblTMember.UpdateOne(i => i.Id == objMemberRelated.Id, objMemberRelated))
                                {
                                    AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                                    return;
                                }
                            }

                            if (!tblTMemberRelation.UpdateOne(i => i.Id == member.Id, member))
                            {
                                AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                                return;
                            }
                        }
                    }

                    if (!tblTMember.UpdateOne(i => i.Id == objMember.Id, objMember))
                    {
                        AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                        return;
                    }
                    LoadListTMember();
                }
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        private void gridListMember_MouseClick(object sender, MouseEventArgs e)
        {
            try {
                if (e.Button == MouseButtons.Right)
                {
                    gridListMember.ClearSelection();

                    ContextMenuStrip m = new ContextMenuStrip();

                    int currentMouseOverRow = gridListMember.HitTest(e.X, e.Y).RowIndex;

                    if (currentMouseOverRow >= 0)
                    {
                        gridListMember.Rows[currentMouseOverRow].Selected = true;

                        var memberSelected = gridListMember.SelectedRows[0].DataBoundItem as TMember;

                        _contextMember.Show(AppManager.GetCursorPosition(), memberSelected);
                    }
                }
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        private void btn_exportEmployee_Click(object sender, EventArgs e)
        {
            try {
                var strKeyword = strAfterSearchKeyWord;
                var intGender = intAfterSearchGender;
                var intLiveOrDie = intAfterSearchLiveOrDie;

                var isDeath = intLiveOrDie == 0 ? true : false;
                var chkInClan = (chkboxInClan.Checked) ? true : false;

                // Create a new document.
                using (var document = DocX.Create(@"NewFile.docx"))
                {
                    var tblMember = AppManager.DBManager.GetTable<TMember>();
                    var lstMember = tblMember.CreateQuery(i => i.DeleteDate == null
                                                                && (intGender < 0 || i.Gender == intGender) // gender
                                                                && (intLiveOrDie < 0 || i.IsDeath == isDeath) // death
                                                                && (string.IsNullOrEmpty(strKeyword) || i.Name.ToLower().Contains(strKeyword)
                                                                                                     || i.Contact.Tel_1.ToLower().Contains(strKeyword)
                                                                                                     || i.Contact.Tel_2.ToLower().Contains(strKeyword)
                                                                                                     || i.Contact.Email_1.ToLower().Contains(strKeyword)
                                                                                                     || i.Contact.Email_2.ToLower().Contains(strKeyword)
                                                                                                     || i.Contact.Address.ToLower().Contains(strKeyword)
                    )).ToList();
                    if(lstMember != null)
                    {
                        foreach (var member in lstMember)
                        {
                            var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                            var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();

                            using (var template = DocX.Load("./Data/Docs/TempMemberInfo.docx"))
                            {
                                var objTableTemplate = template.Tables.FirstOrDefault();

                                var strTel = string.IsNullOrEmpty(member.Contact.Tel_1) ? (string.IsNullOrEmpty(member.Contact.Tel_2) ? "" : member.Contact.Tel_2) : member.Contact.Tel_1;
                                var strMail = string.IsNullOrEmpty(member.Contact.Email_1) ? (string.IsNullOrEmpty(member.Contact.Email_2) ? "" : member.Contact.Email_2) : member.Contact.Email_1;
                                var typeName = member.TypeName?.FirstOrDefault();
                                objTableTemplate.Rows[0].Cells[2].Paragraphs[0].Append(member.Name);
                                objTableTemplate.Rows[1].Cells[2].Paragraphs[0].Append(typeName?.Value + "");
                                objTableTemplate.Rows[2].Cells[2].Paragraphs[0].Append((member.ChildLevelInFamily != -1) ? member.ChildLevelInFamily.ToString() : "");
                                objTableTemplate.Rows[4].Cells[2].Paragraphs[0].Append(member.Birthday?.ToDateSun());
                                objTableTemplate.Rows[5].Cells[2].Paragraphs[0].Append(member.Birthday?.ToDateMoon());
                                objTableTemplate.Rows[6].Cells[2].Paragraphs[0].Append(member.BirthPlace);
                                objTableTemplate.Rows[7].Cells[2].Paragraphs[0].Append(member.HomeTown);
                                objTableTemplate.Rows[8].Cells[2].Paragraphs[0].Append(member.Contact.Address);
                                objTableTemplate.Rows[9].Cells[2].Paragraphs[0].Append(strTel + "\n" + strMail);
                                objTableTemplate.Rows[11].Cells[2].Paragraphs[0].Append(member.DeadDay?.ToDateSun());
                                objTableTemplate.Rows[12].Cells[2].Paragraphs[0].Append(member.DeadDay?.ToDateMoon());
                                objTableTemplate.Rows[13].Cells[2].Paragraphs[0].Append(member.DeadPlace);

                                var strPARENT = "";
                                var strSPOUSE = "";
                                var strCHILDREN = "";

                                // find related member of member
                                try
                                {
                                    var dataLinQ = (from tMemberRelation in tblTMemberRelation.AsEnumerable()
                                                    join tMember in tblMember.AsEnumerable()
                                                    on tMemberRelation.relMemberId equals tMember.Id
                                                    join mRelation in tblMRelation.AsEnumerable()
                                                    on tMemberRelation.relType equals mRelation.MainRelation
                                                    where tMemberRelation.memberId == member.Id
                                                    select new ExTMember
                                                    {
                                                        Name = tMember.Name,
                                                        RelTypeShow = tMemberRelation.relType
                                                    }).ToList();

                                    if (dataLinQ != null)
                                    {
                                        foreach (var item in dataLinQ)
                                        {
                                            if (item.RelTypeShow.Contains(cstrPreFixDAD) || item.RelTypeShow.Contains(cstrPreFixMOM))
                                            {
                                                strPARENT += (string.IsNullOrEmpty(strPARENT)) ? item.Name : "\n" + item.Name;
                                            }
                                            if (item.RelTypeShow.Contains(cstrPreFixHUSBAND) || item.RelTypeShow.Contains(cstrPreFixWIFE))
                                            {
                                                strSPOUSE += (string.IsNullOrEmpty(strSPOUSE)) ? item.Name : "\n" + item.Name;
                                            }
                                            if (item.RelTypeShow.Contains(cstrPreFixCHILD))
                                            {
                                                strCHILDREN += (string.IsNullOrEmpty(strCHILDREN)) ? item.Name : "\n" + item.Name;
                                            }
                                        }
                                    }

                                    objTableTemplate.Rows[15].Cells[0].Paragraphs[0].Append(strPARENT);
                                    objTableTemplate.Rows[17].Cells[0].Paragraphs[0].Append(strSPOUSE);
                                    objTableTemplate.Rows[17].Cells[1].Paragraphs[0].Append(strCHILDREN);
                                } catch (Exception ex) {
                                    return;
                                }

                                document.InsertTable(objTableTemplate);
                                document.InsertSectionPageBreak();
                            }
                        }
                    }
                    document.Save();
                }
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }
    }
}
