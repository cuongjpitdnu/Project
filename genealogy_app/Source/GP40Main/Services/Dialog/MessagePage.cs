using System;
using System.Windows.Forms;
using GP40Main.Core;
using GP40Main.Services.Navigation;
using MaterialSkin;
using static GP40Main.Core.AppConst;

namespace GP40Main.Services.Dialog
{
    public partial class MessagePage : BaseUserControl
    {
        public const string MSG_CONTENT_KEY = "msg_content_key";
        public const string MSG_ICON_KEY = "msg_icon_key";

        public MessagePage(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            this.SetDefaultUI();
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
            var param = this.GetParameters();
            var msgContent = param.GetValue<string>(MSG_CONTENT_KEY);
            var msgIcon = param.GetValue<int>(MSG_ICON_KEY);

            lblMsg.Text = msgContent;

            if (msgIcon != (int)DialogManager.MessageIcon.Question)
            {
                OnlyButtonOk();
            }

            if (msgIcon == (int)DialogManager.MessageIcon.OK)
            {
                pictureBox1.Image = Properties.Resources.ok_icon;
            }
            else if (msgIcon == (int)DialogManager.MessageIcon.Error)
            {
                pictureBox1.Image = Properties.Resources.error_icon;
            }
            else if (msgIcon == (int)DialogManager.MessageIcon.Warning)
            {
                pictureBox1.Image = Properties.Resources.warn_icon;
            }
            if (msgIcon == (int)DialogManager.MessageIcon.Question)
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
