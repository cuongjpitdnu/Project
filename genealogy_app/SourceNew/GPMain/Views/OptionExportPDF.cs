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
using GPMain.Common.Navigation;
using GPModels;
using GPMain.Common.Helper;

namespace GPMain.Views
{
    public partial class OptionExportPDF : BaseUserControl
    {
        public OptionExportPDF(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            this.BackColor = AppConst.PopupBackColor;
            LoadInfo();
        }
        MemberHelper memberHelper = new MemberHelper();
        private void LoadInfo()
        {
            //Ngày tạo
            string dateCreate = DateTime.Now.ToString("dd/MM/yyyy");
            lbldateCreate.Text = dateCreate;
            //Thông tin dòng họ
            var familyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(x => x.Id == AppManager.LoginUser.FamilyId);
            if (familyInfo == null) return;
            //Tên dòng họ
            txtfamilyName.Text = familyInfo.FamilyName ?? "";
            //Quê quán
            txthomeTower.Text = familyInfo.FamilyHometown ?? "";
            //Tổ phụ
            var rootMember = memberHelper.RootTMember;
            if (rootMember == null) return;
            txtrootMember.Text = rootMember.Name;

            Dictionary<string, string> dicPositionUserCreate = new Dictionary<string, string>()
            {
                {AppConst.PositionUserCreate.Top_Left.ToString(),"Trên - Trái" },
                {AppConst.PositionUserCreate.Top_Right.ToString(),"Trên - Phải" },
                {AppConst.PositionUserCreate.Bottom_Left.ToString(),"Dưới - Trái" },
                {AppConst.PositionUserCreate.Bottom_Right.ToString(),"Dưới - Phải" }
            };

            cbpositionUserCreate.DataSource = new BindingSource(dicPositionUserCreate, null);
            cbpositionUserCreate.DisplayMember = "Value";
            cbpositionUserCreate.ValueMember = "Key";
        }



        private void btnexportFile_Click(object sender, EventArgs e)
        {
            NavigationResult naviResult = new NavigationResult() { Result = DialogResult.OK };

            InfoOptionExportPDF optionExportPDF = new InfoOptionExportPDF()
            {
                DateCreateSelect = ckdateCreate.Checked,
                UserCreateSelect = ckuserCreate.Checked,
                FamilyNameSelect = ckfamilyName.Checked,
                HomeTowerSelect = ckhomeTower.Checked,
                RootMemberSelect = ckrootMember.Checked,
                DateCreate = lbldateCreate.Text,
                UserCreate = txtuserCreate.Text,
                FamilyName = txtfamilyName.Text,
                HomeTower = txthomeTower.Text,
                RootMember = txtrootMember.Text

            };
            string temp = (string)cbpositionUserCreate.SelectedValue;
            optionExportPDF.PositionUserCreate = (AppConst.PositionUserCreate)Enum.Parse(typeof(AppConst.PositionUserCreate), temp);
            naviResult.Add("OptionExportPDF", optionExportPDF);
            this.Close(naviResult);
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            NavigationResult naviResult = new NavigationResult() { Result = DialogResult.Cancel };

            this.Close(naviResult);
        }
    }
    public class InfoOptionExportPDF
    {
        public bool DateCreateSelect { get; set; }
        public bool UserCreateSelect { get; set; }
        public bool FamilyNameSelect { get; set; }
        public bool HomeTowerSelect { get; set; }
        public bool RootMemberSelect { get; set; }
        public string DateCreate { get; set; }
        public string UserCreate { get; set; }
        public string FamilyName { get; set; }
        public string HomeTower { get; set; }
        public string RootMember { get; set; }
        public AppConst.PositionUserCreate PositionUserCreate { get; set; }
    }
}
