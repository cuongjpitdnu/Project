using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using GP40Main.Utility;
using System;
using System.Collections.Generic;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views.Config
{
    /// <summary>
    /// Meno        : Update data for ConfigCommon
    /// Create by   : AKB Nguyễn Thanh Tùng
    /// </summary>
    public partial class popupNRT : BaseUserControl
    {
        public const string KEY_TAB = "tab";
        public const string VALUE_OBJ = "valueObj";

        private string _idUpdate;

        public popupNRT(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            var mappingDisplayText = new Dictionary<string, string[]>();
            mappingDisplayText.Add("tabNational", new string[] { "Thêm mới quốc gia", "Chỉnh sửa quốc gia" });
            mappingDisplayText.Add("tabReligion", new string[] { "Thêm mới tôn giáo", "Chỉnh sửa tôn giáo" });
            mappingDisplayText.Add("tabTypeName", new string[] { "Thêm mới kiểu tên", "Chỉnh sửa kiểu tên" });

            var indexGet = this.GetMode() == ModeForm.New ? 0 : 1;
            var keyTab = this.GetMode() == ModeForm.New ? this.GetParameters().GetValue<string>() : this.GetParameters().GetValue<string>(KEY_TAB);
            var objValue = this.GetParameters().GetValue<object>(VALUE_OBJ);

            if (mappingDisplayText.ContainsKey(keyTab))
            {
                lblMain.Text = mappingDisplayText[keyTab][indexGet];
            }

            _idUpdate = (objValue as BaseModel)?.Id + "";

            if(indexGet == 1)
            {
                txtName.Text = objValue.GetType() != typeof(MNationality) ? objValue.GetType() != typeof(MReligion) ? objValue.GetType() != typeof(MTypeName) ?
                           string.Empty :
                           ((MTypeName)objValue).TypeName :
                           ((MReligion)objValue).RelName :
                           ((MNationality)objValue).NatName;
            }

            txtName.FocusAndSelected();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var keyTab = this.GetMode() == ModeForm.New ? this.GetParameters().GetValue<string>() : this.GetParameters().GetValue<string>(KEY_TAB);
                var objValue = this.GetParameters().GetValue<object>(VALUE_OBJ);

                if (keyTab == "tabNational")
                {
                    var tblMNational = AppManager.DBManager.GetTable<MNationality>();
                    var data = new MNationality();

                    if (!string.IsNullOrEmpty(_idUpdate))
                    {
                        data = tblMNational.CreateQuery(i => i.Id == _idUpdate).FirstOrDefault();
                    } 
                    else
                    {
                        // add new
                        var countData = tblMNational.CreateQuery().Count();
                        countData = countData > 0 ? countData : 0;
                        data.Id = (++countData) + "";
                    }

                    data.NatName = txtName.Text.Trim();

                    if (!(this.GetMode() == ModeForm.New ? tblMNational.InsertOne(data, false) : tblMNational.UpdateOne(data)))
                    {
                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                        return;
                    }
                }
                else if (keyTab == "tabReligion")
                {
                    var tblMReligion = AppManager.DBManager.GetTable<MReligion>();
                    var data = new MReligion();

                    if (!string.IsNullOrEmpty(_idUpdate))
                    {
                        data = tblMReligion.CreateQuery(i => i.Id == _idUpdate).FirstOrDefault();
                    }
                    else
                    {
                        // add new
                        var countData = tblMReligion.CreateQuery().Count();
                        countData = countData > 0 ? countData : 0;
                        data.Id = (++countData) + "";
                    }

                    data.RelName = txtName.Text.Trim();

                    if (!(this.GetMode() == ModeForm.New ? tblMReligion.InsertOne(data, false) : tblMReligion.UpdateOne(data)))
                    {
                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                        return;
                    }
                }
                else if (keyTab == "tabTypeName")
                {
                    var tblMTypeName = AppManager.DBManager.GetTable<MTypeName>();
                    var data = new MTypeName();

                    if (!string.IsNullOrEmpty(_idUpdate))
                    {
                        data = tblMTypeName.CreateQuery(i => i.Id == _idUpdate).FirstOrDefault();
                    }

                    data.TypeName = txtName.Text.Trim();

                    if (!(this.GetMode() == ModeForm.New ? tblMTypeName.InsertOne(data) : tblMTypeName.UpdateOne(data)))
                    {
                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                        return;
                    }
                }

                this.Close();
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(popupNRT), ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}