using KeyMgnt.Common;
using KeyMgnt.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyMgnt.Screens
{
    public partial class UserManagerForm : BaseForm
    {
        public UserManagerForm()
        {
            InitializeComponent();
            this.dgvUserList.AutoGenerateColumns = false;

            loadListUser();
        }

        #region functions

        private void loadListUser()
        {
            try
            {
                List<MUser> lstUser = SQLiteCommon.GetALlUsers();
                BindingDataGridView(dgvUserList, lstUser);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }

        #endregion

        #region events

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserForm userForm = new UserForm();
            userForm.ViewMode = UserForm.Mode.CREATE;
            userForm.InitForm();
            DialogResult result = userForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (userForm.isSuccess)
                {
                    loadListUser();
                }
            }
        }

        private void dgvUserList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            using (var userForm = new UserForm())
            {
                userForm.ViewMode = UserForm.Mode.EDIT;
                userForm.paramUpdate = (MUser)dgvUserList.CurrentRow.DataBoundItem;
                userForm.InitForm();
                DialogResult result = userForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (userForm.isSuccess)
                    {
                        loadListUser();
                    }
                }
            }
        }

        #endregion
    }
}
