using GPMain.Common;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GPMain.Views.Config
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

            this.BackColor = AppConst.PopupBackColor;

            var mappingDisplayText = new Dictionary<string, string[]>();
            mappingDisplayText.Add("tabNational", new string[] { "Thêm mới quốc gia", "Chỉnh sửa quốc gia" });
            mappingDisplayText.Add("tabReligion", new string[] { "Thêm mới tôn giáo", "Chỉnh sửa tôn giáo" });
            mappingDisplayText.Add("tabTypeName", new string[] { "Thêm mới kiểu tên", "Chỉnh sửa kiểu tên" });

            var indexGet = this.Mode == ModeForm.New ? 0 : 1;
            var keyTab = this.Mode == ModeForm.New ? this.Params.GetValue<string>() : this.Params.GetValue<string>(KEY_TAB);
            var objValue = this.Params.GetValue<object>(VALUE_OBJ);

            if (mappingDisplayText.ContainsKey(keyTab))
            {
                TitleBar = lblMain.Text = mappingDisplayText[keyTab][indexGet];
            }

            _idUpdate = (objValue as BaseModel)?.Id + "";

            if (indexGet == 1)
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
            try
            {


                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var keyTab = this.Mode == ModeForm.New ? this.Params.GetValue<string>() : this.Params.GetValue<string>(KEY_TAB);
                var objValue = this.Params.GetValue<object>(VALUE_OBJ);

                if (keyTab == "tabNational")
                {
                    if (string.IsNullOrEmpty(txtName.Text))
                    {
                        AppManager.Dialog.Error("Tên quốc gia không được để trống.");
                        return;
                    }

                    var tblMNational = AppManager.DBManager.GetTable<MNationality>();
                    var data = new MNationality();

                    if (!string.IsNullOrEmpty(_idUpdate))
                    {
                        data = tblMNational.FirstOrDefault(i => i.Id == _idUpdate);
                    }
                    else
                    {
                        // add new
                        var countData = tblMNational.Count;
                        countData = countData > 0 ? countData : 0;
                        data.Id = (++countData) + "";
                    }

                    data.NatName = txtName.Text.Trim();

                    if (!(this.Mode == ModeForm.New ? tblMNational.InsertOne(data, false) : tblMNational.UpdateOne(data)))
                    {
                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                        return;
                    }
                }
                else if (keyTab == "tabReligion")
                {
                    if (string.IsNullOrEmpty(txtName.Text))
                    {
                        AppManager.Dialog.Error("Tên tôn giáo không được để trống.");
                        return;
                    }

                    var tblMReligion = AppManager.DBManager.GetTable<MReligion>();
                    var data = new MReligion();

                    if (!string.IsNullOrEmpty(_idUpdate))
                    {
                        data = tblMReligion.FirstOrDefault(i => i.Id == _idUpdate);
                    }
                    else
                    {
                        // add new
                        var countData = tblMReligion.Count;
                        countData = countData > 0 ? countData : 0;
                        data.Id = (++countData) + "";
                    }

                    data.RelName = txtName.Text.Trim();

                    if (!(this.Mode == ModeForm.New ? tblMReligion.InsertOne(data, false) : tblMReligion.UpdateOne(data)))
                    {
                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                        return;
                    }
                }
                else if (keyTab == "tabTypeName")
                {
                    if (string.IsNullOrEmpty(txtName.Text))
                    {
                        AppManager.Dialog.Error("Kiểu tên không được để trống.");
                        return;
                    }

                    var tblMTypeName = AppManager.DBManager.GetTable<MTypeName>();
                    var data = new MTypeName();

                    if (!string.IsNullOrEmpty(_idUpdate))
                    {
                        data = tblMTypeName.FirstOrDefault(i => i.Id == _idUpdate);
                    }

                    data.TypeName = txtName.Text.Trim();

                    if (!(this.Mode == ModeForm.New ? tblMTypeName.InsertOne(data) : tblMTypeName.UpdateOne(data)))
                    {
                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                        return;
                    }
                }

                this.Close(DialogResult.OK);
            }
            catch (Exception ex)
            {
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