using System;
using System.Windows.Forms;
using GPMain.Common.Navigation;
using MaterialSkin;

namespace GPMain.Common.Dialog
{
    public partial class MessagePage : BaseUserControl
    {
        public const string MSG_CONTENT_KEY = "msg_content_key";
        public const string MSG_ICON_KEY = "msg_icon_key";

        public MessagePage(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            TitleBar = "Thông báo!";
            lblMsg.Font = MaterialSkinManager.Instance.getFontByType(MaterialSkinManager.fontType.Caption);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close(new NavigationResult(DialogResult.OK));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(new NavigationResult(DialogResult.Cancel));
        }

        private void MessagePage_Load(object sender, EventArgs e)
        {
            var msgContent = this.Params.GetValue<string>(MSG_CONTENT_KEY);
            var msgIcon = this.Params.GetValue<int>(MSG_ICON_KEY);

            lblMsg.Text = msgContent;

            if (msgIcon != (int)MessageIcon.Question)
            {
                OnlyButtonOk();
            }

            if (msgIcon == (int)MessageIcon.OK)
            {
                pictureBox1.Image = Properties.Resources.ok_icon;
            }
            else if (msgIcon == (int)MessageIcon.Error)
            {
                pictureBox1.Image = Properties.Resources.error_icon;
            }
            else if (msgIcon == (int)MessageIcon.Warning)
            {
                pictureBox1.Image = Properties.Resources.warn_icon;
            }
            if (msgIcon == (int)MessageIcon.Question)
            {
                pictureBox1.Image = Properties.Resources.question_icon;
            }
        }

        private void OnlyButtonOk()
        {
            btnCancel.Visible = false;
            btnOk.Location = btnCancel.Location;
        }
    }
}
