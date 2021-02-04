using GP40Common;
using GP40DrawTree;
using GPCommon;
using GPConst;
using GPMain.Common;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Views.Controls;
using GPMain.Views.Member;
using GPModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Words.NET;

namespace GPMain.Views
{
    public partial class ListMember : BaseUserControl
    {
        private ContextMenuStripData<TMember> _contextMember;
        private DrawListMember drawListMember;

        public string strAfterSearchKeyWord = "";
        public int intAfterSearchGender = -1;
        public int intAfterSearchLiveOrDie = -1;

        public ListMember(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            TitleBar = "Danh sách thành viên";
            _contextMember = ContextMenuStripManager.CreateForMember();
            _contextMember.ItemClickEnd += (sender, e) => LoadListTMember();

            Dictionary<int, string> dicGender = new Dictionary<int, string>();
            dicGender.Add(-1, "Chọn giới tính");
            dicGender.Add((int)EmGender.Unknown, "Chưa rõ");
            dicGender.Add((int)EmGender.Male, "Nam");
            dicGender.Add((int)EmGender.FeMale, "Nữ");
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

            drawListMember = new DrawListMember();
            drawListMember.MemberDoubleClick += (sender, e) =>
            {
                var memberSelected = sender as clsFamilyMember;

                var objTMember = memberSelected.HasValue()
                                 ? AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == memberSelected.Id)
                                 : null;

                if (objTMember.HasValue())
                {
                    if (AppManager.Navigation.ShowDialogWithParam<addMember, TMember>(objTMember, ModeForm.Edit, new StatusBarColor() { BackColor = AppConst.StatusBarBackColor, ForeColor = AppConst.StatusBarForeColor }).Result == DialogResult.OK)
                    {
                        LoadListTMember();
                    }
                }
            };

            drawListMember.MemberRightClick += (sender, objMember) =>
            {
                if (objMember.HasValue() && !String.IsNullOrEmpty(objMember.Id) && _contextMember.HasValue())
                {
                    var objTMember = AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == objMember.Id);
                    _contextMember.Show(UIHelper.GetCursorPosition(), objTMember);
                }
            };

            plnListMember.SuspendLayout();
            plnListMember.Controls.Add(drawListMember.Tree);
            plnListMember.ResumeLayout(true);

            this.Resize += (sender, e) => LoadListTMember();

            this.Disposed += (sender, e) =>
            {
                ObjectHelper.FreeMemory(ref _contextMember);
                ObjectHelper.FreeMemory(ref drawListMember);
            };
        }

        public override void ProcessAffterAddForm()
        {
            LoadListTMember();
        }

        private void btn_addEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                var content = AppManager.Navigation.ShowDialog<addMember>(ModeForm.New, AppConst.StatusBarColor);
                LoadListTMember();
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        public void LoadListTMember(string keyword = "", string gender = "", string liveOrDie = "", bool inClan = true)
        {
            try
            {
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
                    ).ToList();

                drawListMember.Load(dtaTMember);
                //BindingHelper.BindingDataGrid(gridListMember, dtaTMember);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var strKeyword = txtKeyword.Text.Trim();
                var genderSelected = cmbGender.SelectedValue.ToString();
                var LiveOrDieSelected = cmbLiveOrDie.SelectedValue.ToString();
                var chkInClan = (chkboxInClan.Checked) ? true : false;

                LoadListTMember(strKeyword, genderSelected, LiveOrDieSelected, chkInClan);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        //        private void gridListMember_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //        {
        //            try
        //            {
        //                if (e.RowIndex < 0 || e.ColumnIndex < 0)
        //                {
        //                    return;
        //                }
        //
        //                var rowSelected = gridListMember.Rows[e.RowIndex].DataBoundItem as TMember;
        //
        //                if (rowSelected == null)
        //                {
        //                    return;
        //                }
        //
        //                // view screen relationship of user
        //                if (e.ColumnIndex == ViewRelationship.Index)
        //                {
        //                    AppManager.Navigation.ShowDialog<ListMemberRelation, TMember>(new NavigationParameters(rowSelected), ModeForm.None);
        //                }
        //
        //                if (e.ColumnIndex == ActionDelete.Index)
        //                {
        //                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
        //                    {
        //                        return;
        //                    }
        //
        //                    var tblTMember = AppManager.DBManager.GetTable<TMember>();
        //
        //                    var objMember = tblTMember.CreateQuery(i => i.Id == rowSelected.Id).FirstOrDefault();
        //                    if (objMember == null)
        //                    {
        //                        return;
        //                    }
        //
        //                    // set date delete
        //                    objMember.DeleteDate = DateTime.Now;
        //
        //                    // check related member relation to delete (TMemberRelation)
        //                    var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
        //                    // find all in TMemberRelation
        //                    var listMemberRelation = tblTMemberRelation.CreateQuery(i => i.memberId == objMember.Id || i.relMemberId == objMember.Id).ToList();
        //                    if (listMemberRelation != null)
        //                    {
        //                        foreach (var member in listMemberRelation)
        //                        {
        //                            member.DeleteDate = DateTime.Now;
        //
        //                            // find member in TMember related with member want to delete -> remove listPARENT/SPOUSE/CHILDREN or props Relation
        //                            var objMemberRelated = tblTMember.CreateQuery(i => i.Id == ((objMember.Id == member.memberId) ? member.relMemberId : member.memberId)).FirstOrDefault();
        //                            if (objMemberRelated != null)
        //                            {
        //                                // listPARENT - SPOUSE - CHILD
        //                                if (member.relType.Contains(Relation.PREFIX_DAD) || member.relType.Contains(Relation.PREFIX_MOM))
        //                                {
        //                                    objMemberRelated.ListPARENT.Remove(objMember.Id);
        //                                }
        //                                else if (member.relType.Contains(Relation.PREFIX_HUSBAND) || member.relType.Contains(Relation.PREFIX_WIFE))
        //                                {
        //                                    objMemberRelated.ListSPOUSE.Remove(objMember.Id);
        //                                }
        //                                else if (member.relType.Contains(Relation.PREFIX_CHILD))
        //                                {
        //                                    objMemberRelated.ListCHILDREN.Remove(objMember.Id);
        //                                }
        //                                var findInRelation = objMemberRelated.Relation.FindAll(i => i.memberId == member.memberId && i.relMemberId != member.relMemberId);
        //                                objMemberRelated.Relation = findInRelation;
        //
        //                                if (!tblTMember.UpdateOne(i => i.Id == objMemberRelated.Id, objMemberRelated))
        //                                {
        //                                    AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
        //                                    return;
        //                                }
        //                            }
        //
        //                            if (!tblTMemberRelation.UpdateOne(i => i.Id == member.Id, member))
        //                            {
        //                                AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
        //                                return;
        //                            }
        //                        }
        //                    }
        //
        //                    if (!tblTMember.UpdateOne(i => i.Id == objMember.Id, objMember))
        //                    {
        //                        AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
        //                        return;
        //                    }
        //                    
        //                    // remove member if exist in list family head
        //                    var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
        //                    var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == AppManager.LoginUser.FamilyId).FirstOrDefault();
        //                    if (objMFamilyInfo != null)
        //                    {
        //                        if (objMFamilyInfo.ListFamilyHead.Contains(rowSelected.Id))
        //                        {
        //                            objMFamilyInfo.ListFamilyHead.Remove(rowSelected.Id);
        //                            // update new current family head
        //                            objMFamilyInfo.CurrentFamilyHead = (objMFamilyInfo.ListFamilyHead.HasValue()) ? objMFamilyInfo.ListFamilyHead[objMFamilyInfo.ListFamilyHead.Count - 1] : "";
        //
        //                            if (!tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo))
        //                            {
        //                                AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
        //                                return;
        //                            }
        //                        }
        //                    }
        //                    LoadListTMember();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                AppManager.Dialog.Error(ex.Message);
        //                AppManager.LoggerApp.Error(typeof(ListMember), ex);
        //            }
        //        }

        //private void gridListMember_MouseClick(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Button == MouseButtons.Right)
        //        {
        //            gridListMember.ClearSelection();

        //            ContextMenuStrip m = new ContextMenuStrip();

        //            int currentMouseOverRow = gridListMember.HitTest(e.X, e.Y).RowIndex;

        //            if (currentMouseOverRow >= 0)
        //            {
        //                gridListMember.Rows[currentMouseOverRow].Selected = true;

        //                var memberSelected = gridListMember.SelectedRows[0].DataBoundItem as TMember;

        //                _contextMember.Show(UIHelper.GetCursorPosition(), memberSelected);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppManager.Dialog.Error(ex.Message);
        //        AppManager.LoggerApp.Error(typeof(ListMember), ex);
        //    }
        //}

        private void btn_exportEmployee_Click(object sender, EventArgs e)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Keyword", strAfterSearchKeyWord);
            param.Add("Gender", intAfterSearchGender);
            param.Add("LiveOrDie", intAfterSearchLiveOrDie);
            param.Add("Inclan", chkboxInClan.Checked);
            using (MemberHelper memberHelper = new MemberHelper())
            {
                var lstMember = memberHelper.FindMember(strAfterSearchKeyWord, intAfterSearchGender, intAfterSearchLiveOrDie);
                if(!lstMember.HasValue())
                {
                    AppManager.Dialog.Warning("Không có dữ liệu để in!");
                }
            }
            AppManager.Navigation.ShowDialogWithParam<ExportListMember>(param, ModeForm.None, AppConst.StatusBarColor);

            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "File word gia pha|*.docx";
            //sfd.FileName = "Gia Pha.docx";
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    ExportData(sfd.FileName);
            //}

            //try
            //{
            //    AppManager.LoggerApp.Debug(typeof(Application), "Start" + $"|{(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds}", $"Export member");
            //    var strKeyword = strAfterSearchKeyWord;
            //    var intGender = intAfterSearchGender;
            //    var intLiveOrDie = intAfterSearchLiveOrDie;

            //    var isDeath = intLiveOrDie == 0 ? true : false;
            //    var chkInClan = (chkboxInClan.Checked) ? true : false;

            //    // Create a new document.
            //    using (var document = DocX.Create(@"NewFile.docx"))
            //    {
            //        var tblMember = AppManager.DBManager.GetTable<TMember>();
            //        var lstMember = tblMember.CreateQuery(i => i.DeleteDate == null
            //                                                    && (intGender < 0 || i.Gender == intGender) // gender
            //                                                    && (intLiveOrDie < 0 || i.IsDeath == isDeath) // death
            //                                                    && (string.IsNullOrEmpty(strKeyword) || i.Name.ToLower().Contains(strKeyword)
            //                                                                                         || i.Contact.Tel_1.ToLower().Contains(strKeyword)
            //                                                                                         || i.Contact.Tel_2.ToLower().Contains(strKeyword)
            //                                                                                         || i.Contact.Email_1.ToLower().Contains(strKeyword)
            //                                                                                         || i.Contact.Email_2.ToLower().Contains(strKeyword)
            //                                                                                         || i.Contact.Address.ToLower().Contains(strKeyword)
            //        )).ToList();
            //        if (lstMember != null)
            //        {

            //            foreach (var member in lstMember)
            //            {
            //                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
            //                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();

            //                using (var template = DocX.Load("./Data/Docs/TempMemberInfo.docx"))
            //                {
            //                    var objTableTemplate = template.Tables.FirstOrDefault();

            //                    var strTel = string.IsNullOrEmpty(member.Contact.Tel_1) ? (string.IsNullOrEmpty(member.Contact.Tel_2) ? "" : member.Contact.Tel_2) : member.Contact.Tel_1;
            //                    var strMail = string.IsNullOrEmpty(member.Contact.Email_1) ? (string.IsNullOrEmpty(member.Contact.Email_2) ? "" : member.Contact.Email_2) : member.Contact.Email_1;
            //                    var typeName = member.TypeName?.FirstOrDefault();
            //                    objTableTemplate.Rows[0].Cells[2].Paragraphs[0].Append(member.Name);
            //                    objTableTemplate.Rows[1].Cells[2].Paragraphs[0].Append(typeName?.Value + "");
            //                    objTableTemplate.Rows[2].Cells[2].Paragraphs[0].Append((member.ChildLevelInFamily != -1) ? member.ChildLevelInFamily.ToString() : "");
            //                    objTableTemplate.Rows[4].Cells[2].Paragraphs[0].Append(member.Birthday?.ToDateSun());
            //                    objTableTemplate.Rows[5].Cells[2].Paragraphs[0].Append(member.Birthday?.ToDateMoon());
            //                    objTableTemplate.Rows[6].Cells[2].Paragraphs[0].Append(member.BirthPlace);
            //                    objTableTemplate.Rows[7].Cells[2].Paragraphs[0].Append(member.HomeTown);
            //                    objTableTemplate.Rows[8].Cells[2].Paragraphs[0].Append(member.Contact.Address);
            //                    objTableTemplate.Rows[9].Cells[2].Paragraphs[0].Append(strTel + "\n" + strMail);
            //                    objTableTemplate.Rows[11].Cells[2].Paragraphs[0].Append(member.DeadDay?.ToDateSun());
            //                    objTableTemplate.Rows[12].Cells[2].Paragraphs[0].Append(member.DeadDay?.ToDateMoon());
            //                    objTableTemplate.Rows[13].Cells[2].Paragraphs[0].Append(member.DeadPlace);

            //                    var strPARENT = "";
            //                    var strSPOUSE = "";
            //                    var strCHILDREN = "";

            //                    // find related member of member
            //                    try
            //                    {
            //                        var dataLinQ = (from tMemberRelation in tblTMemberRelation.AsEnumerable()
            //                                        join tMember in tblMember.AsEnumerable()
            //                                        on tMemberRelation.relMemberId equals tMember.Id
            //                                        join mRelation in tblMRelation.AsEnumerable()
            //                                        on tMemberRelation.relType equals mRelation.MainRelation
            //                                        where tMemberRelation.memberId == member.Id
            //                                        select new ExTMember
            //                                        {
            //                                            Name = tMember.Name,
            //                                            RelTypeShow = tMemberRelation.relType
            //                                        }).ToList();

            //                        if (dataLinQ != null)
            //                        {
            //                            foreach (var item in dataLinQ)
            //                            {
            //                                if (item.RelTypeShow.Contains(Relation.PREFIX_DAD) || item.RelTypeShow.Contains(Relation.PREFIX_MOM))
            //                                {
            //                                    strPARENT += (string.IsNullOrEmpty(strPARENT)) ? item.Name : "\n" + item.Name;
            //                                }
            //                                if (item.RelTypeShow.Contains(Relation.PREFIX_HUSBAND) || item.RelTypeShow.Contains(Relation.PREFIX_WIFE))
            //                                {
            //                                    strSPOUSE += (string.IsNullOrEmpty(strSPOUSE)) ? item.Name : "\n" + item.Name;
            //                                }
            //                                if (item.RelTypeShow.Contains(Relation.PREFIX_CHILD))
            //                                {
            //                                    strCHILDREN += (string.IsNullOrEmpty(strCHILDREN)) ? item.Name : "\n" + item.Name;
            //                                }
            //                            }
            //                        }

            //                        objTableTemplate.Rows[15].Cells[0].Paragraphs[0].Append(strPARENT);
            //                        objTableTemplate.Rows[17].Cells[0].Paragraphs[0].Append(strSPOUSE);
            //                        objTableTemplate.Rows[17].Cells[1].Paragraphs[0].Append(strCHILDREN);
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        AppManager.LoggerApp.Error(typeof(ListMember), ex);
            //                        return;
            //                    }

            //                    document.InsertTable(objTableTemplate);
            //                    document.InsertSectionPageBreak();
            //                }
            //            }
            //        }
            //        document.Save();
            //        AppManager.LoggerApp.Debug(typeof(Application), "End" + $"|{(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds}", $"Export member");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    AppManager.Dialog.Error(ex.Message);
            //    AppManager.LoggerApp.Error(typeof(ListMember), ex);
            //}
        }
    }
}