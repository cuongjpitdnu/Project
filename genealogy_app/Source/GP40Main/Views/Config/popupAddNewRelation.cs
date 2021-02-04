using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GP40Main.Core;
using GP40Main.Services.Navigation;
using static GP40Main.Core.AppConst;
using GP40Main.Models;

namespace GP40Main.Views.Config
{
    public partial class popupAddNewRelation : BaseUserControl
    {
        public popupAddNewRelation(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            // binding const to combo main  & related relation
            Dictionary<string, string> dicRelation = new Dictionary<string, string>();
            dicRelation.Add(cstrPreFixDAD, "Bố/Cha");
            dicRelation.Add(cstrPreFixMOM, "Mẹ/Má");
            dicRelation.Add(cstrPreFixHUSBAND, "Chồng");
            dicRelation.Add(cstrPreFixWIFE, "Vợ");
            dicRelation.Add(cstrPreFixCHILD, "Con");

            cboMainPrefix.DataSource = new BindingSource(dicRelation, null);
            cboMainPrefix.DisplayMember = "Value";
            cboMainPrefix.ValueMember = "Key";

            cboRelatedPrefix.DataSource = new BindingSource(dicRelation, null);
            cboRelatedPrefix.DisplayMember = "Value";
            cboRelatedPrefix.ValueMember = "Key";

            if (this.GetMode() == ModeForm.New)
            {
                lblMain.Text = "Thêm mới quan hệ";
            } else
            {
                lblMain.Text = "Chỉnh sửa quan hệ";
                cboMainPrefix.Enabled = false;
                cboRelatedPrefix.Enabled = false;

                var objParam = this.GetParameters().GetValue<MRelation>();
                if (objParam != null)
                {
                    var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                    var objMRelation = tblMRelation.CreateQuery(i => i.Id == objParam.Id).FirstOrDefault();
                    if (objMRelation != null)
                    {
                        var strMainKey = objMRelation.MainRelation.Substring(0, 3);
                        var strRelatedKey = objMRelation.RelatedRelation.Substring(0, 3);
                        cboMainPrefix.SelectedValue = strMainKey;
                        cboRelatedPrefix.SelectedValue = strRelatedKey;

                        txtMainName.Text = objMRelation.NameOfRelation;
                        var objRelatedMRelation = tblMRelation.CreateQuery(i => i.MainRelation == objMRelation.RelatedRelation).FirstOrDefault();
                        if (objRelatedMRelation != null)
                        {
                            txtRelatedName.Text = objRelatedMRelation.NameOfRelation;
                        }

                        chkIsMain.Checked = objMRelation.IsMain;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtMainName.Text) || string.IsNullOrEmpty(txtRelatedName.Text))
            {
                AppManager.Dialog.Warning("Vui lòng không để trống!");
                if(string.IsNullOrEmpty(txtMainName.Text))
                {
                    txtMainName.Focus();
                }

                if (string.IsNullOrEmpty(txtRelatedName.Text))
                {
                    txtRelatedName.Focus();
                }
                return;
            }

            if(!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
            {
                return;
            }

            if (this.GetMode() == ModeForm.New)
            {
                var keyMainSelected = cboMainPrefix.SelectedValue.ToString();
                var keyRelatedSelected = cboRelatedPrefix.SelectedValue.ToString();
                bool isMainValue = chkIsMain.Checked == true ? true : false;

                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                // query db to get value exist -> count max + 1
                var countMain = tblMRelation.CreateQuery(i => i.MainRelation.Contains(keyMainSelected)).Count();
                var countRelated = tblMRelation.CreateQuery(i => i.MainRelation.Contains(keyRelatedSelected)).Count();

                var maxMain = countMain + 1;
                var maxRelated = countRelated + 1;

                var newKeyMain = keyMainSelected + "" + (countMain > 10 ? maxMain.ToString() : "0"+ maxMain.ToString());
                var newKeyRelated = keyRelatedSelected + "" + (countRelated > 10 ? maxRelated.ToString() : "0"+ maxRelated.ToString());

                // insert to db
                var objMainMRelation = new MRelation();
                objMainMRelation.MainRelation = newKeyMain;
                objMainMRelation.NameOfRelation = txtMainName.Text.Trim();
                objMainMRelation.RelatedRelation = newKeyRelated;
                objMainMRelation.IsMain = isMainValue;

                var objRelatedMRelation = new MRelation();
                objRelatedMRelation.MainRelation = newKeyRelated;
                objRelatedMRelation.NameOfRelation = txtRelatedName.Text.Trim();
                objRelatedMRelation.RelatedRelation = newKeyMain;
                objRelatedMRelation.IsMain = isMainValue;

                if (!tblMRelation.InsertOne(objMainMRelation))
                {
                    AppManager.Dialog.Error("Thêm dữ liệu thất bại!");
                    return;
                }

                if (!tblMRelation.InsertOne(objRelatedMRelation))
                {
                    AppManager.Dialog.Error("Thêm dữ liệu tin thất bại!");
                    return;
                }
            }
            else
            {
                var objParam = this.GetParameters().GetValue<MRelation>();
                bool isMainValue = chkIsMain.Checked == true ? true : false;
                if (objParam != null)
                {
                    var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                    var objMRelation = tblMRelation.CreateQuery(i => i.Id == objParam.Id).FirstOrDefault();
                    if(objMRelation != null)
                    {
                        objMRelation.NameOfRelation = txtMainName.Text.Trim();
                        objMRelation.IsMain = isMainValue;

                        if (!tblMRelation.UpdateOne(i => i.Id == objMRelation.Id, objMRelation))
                        {
                            AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                            return;
                        }

                        var objRelatedMRelation = tblMRelation.CreateQuery(i => i.MainRelation == objMRelation.RelatedRelation).FirstOrDefault();
                        if (objRelatedMRelation != null)
                        {
                            objRelatedMRelation.NameOfRelation = txtRelatedName.Text.Trim();
                            objRelatedMRelation.IsMain = isMainValue;

                            if (!tblMRelation.UpdateOne(i => i.Id == objRelatedMRelation.Id, objRelatedMRelation))
                            {
                                AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                return;
                            }
                        }
                    }
                }
            }
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
