using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPMain.Common;
using GPCommon;
using GPMain.Common.Navigation;
using GPModels;
using GPMain.Common.Helper;
using GPConst;

namespace GPMain.Views.FamilyInfo
{
    public partial class AddFamilyHead : BaseUserControl
    {
        public AddFamilyHead(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            TitleBar = "Thêm tộc trưởng";

            this.BackColor = AppConst.PopupBackColor;
            dgvListMember.BackgroundColor = AppConst.PopupBackColor;

            LoadListTMember();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvListMember.SelectedRows.Count > 0)
                {
                    var selectedRows = dgvListMember.SelectedRows[0].DataBoundItem as TMember;

                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }
                    var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                    var objMFamilyInfo = tblMFamilyInfo.FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                    if (objMFamilyInfo != null)
                    {
                        // add new member to list family head
                        if (!objMFamilyInfo.ListFamilyHead.HasValue())
                        {
                            objMFamilyInfo.ListFamilyHead = new List<string>();
                        }
                        if (objMFamilyInfo.ListFamilyHead.Contains(selectedRows.Id))
                        {
                            AppManager.Dialog.Error("Thành viên đã có trong danh sách tộc trưởng!");
                            return;
                        }
                        objMFamilyInfo.ListFamilyHead.Add(selectedRows.Id);
                        objMFamilyInfo.CurrentFamilyHead = selectedRows.Id;
                        if (!tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo))
                        {
                            AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                            return;
                        }
                    }
                    this.Close();
                }
                else
                {
                    AppManager.Dialog.Warning("Vui lòng chọn thành viên!");
                    return;
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddFamilyHead), ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var strKeyword = txtKeyword.Text.Trim();
                LoadListTMember(strKeyword);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddFamilyHead), ex);
            }
        }

        private void LoadListTMember(string keyword = "")
        {
            try
            {
                keyword = keyword.ToLower();

                var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                var objMFamilyInfo = tblMFamilyInfo.FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                var tblTMember = AppManager.DBManager.GetTable<TMember>();

                if (objMFamilyInfo != null)
                {
                    var dtaTMember = tblTMember.CreateQuery(
                        i => !objMFamilyInfo.ListFamilyHead.Contains(i.Id)
                            && i.Gender == (int)EmGender.Male
                            && (string.IsNullOrEmpty(keyword) || i.Name.ToLower().Contains(keyword)) // key
                    ).ToList().Select(i => new ExTMember()
                    {
                        Id = i.Id,
                        Name = i.Name + "",
                        BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
                        Address = i.Contact.Address + "",
                    }).ToList();

                    BindingHelper.BindingDataGrid(dgvListMember, dtaTMember);
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddFamilyHead), ex);
            }
        }
    }
}
