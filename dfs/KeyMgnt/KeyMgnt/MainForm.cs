using KeyMgnt.Common;
using KeyMgnt.Screens;
using System;
using System.Windows.Forms;

namespace KeyMgnt
{
    public partial class MainForm : BaseForm
    {
        public MainForm()
        {
            InitializeComponent();

            initForm();
        }

        private void initForm()
        {
            var userManagerForm = new UserManagerForm();
            userManagerForm.TopLevel = false;
            tabPage1.Controls.Add(userManagerForm);
            userManagerForm.Dock = DockStyle.Fill;
            userManagerForm.Show();

            var cusManagerForm = new CustomerManagerForm();
            cusManagerForm.TopLevel = false;
            tabPage2.Controls.Add(cusManagerForm);
            cusManagerForm.Dock = DockStyle.Fill;
            cusManagerForm.Show();

            var deviceManagerForm = new DeviceManagerForm();
            deviceManagerForm.TopLevel = false;
            tabPage3.Controls.Add(deviceManagerForm);
            deviceManagerForm.Dock = DockStyle.Fill;
            deviceManagerForm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedIndex == 0)
            {

            }
            else if (tabMain.SelectedIndex == 1)
            {
                ((CustomerManagerForm)tabPage2.Controls[0]).SetFristFocus();
            }
            else
            {
                ((DeviceManagerForm)tabPage3.Controls[0]).SetFristFocus();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
