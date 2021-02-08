using KeyMgnt.Common;
using KeyMgnt.Models;
using System;
using System.Windows.Forms;

namespace KeyMgnt.Screens
{
    public partial class CustomerForm : BaseForm
    {
        #region variables

        private string ADD_CUSTOMER_SUCCESS = "Add new customer success!";
        private string ADD_CUSTOMER_FAIL = "Add new customer fail!";

        private string UPDATE_CUSTOMER_SUCCESS = "Update a customer success!";
        private string UPDATE_CUSTOMER_FAIL = "Update a customer fail!";

        private string DELETE_CUSTOMER_SUCCESS = "Delete a customer success!";
        private string DELETE_CUSTOMER_FAIL = "Delete a customer fail!";

        public bool isSuccess { get; set; }

        public MCustomer paramUpdate { get; set; }

        public enum Mode
        {
            CREATE = 0,
            EDIT = 1
        }

        public Mode ViewMode { get; set; }

        #endregion

        public CustomerForm()
        {
            InitializeComponent();
        }

        #region functions

        public void InitForm()
        {
            switch (ViewMode)
            {
                case Mode.CREATE:
                    this.Text = "Add New Customer";
                    this.lblCaption.Text = "Add New Customer";
                    this.btnAdd.Visible = true;
                    this.btnAdd.Enabled = true;
                    this.btnUpdate.Visible = false;
                    this.btnUpdate.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.txtCusName.Focus();
                    break;

                case Mode.EDIT:
                    this.Text = "Edit A Customer";
                    this.lblCaption.Text = "Edit A Customer";
                    this.btnAdd.Visible = false;
                    this.btnAdd.Enabled = false;
                    this.btnUpdate.Visible = true;
                    this.btnUpdate.Enabled = true;
                    this.btnDelete.Enabled = true;

                    bindingMCustomer();
                    break;

                default:
                    break;
            }
        }


        private void bindingMCustomer()
        {
            this.txtCusName.Focus();
            this.txtCompanyName.Text = this.paramUpdate.CompanyName;
            this.txtCompanyAddress.Text = this.paramUpdate.CompanyAddress;
            this.txtCompanyMobile.Text = this.paramUpdate.CompanyMobile;
            this.txtCusName.Text = this.paramUpdate.CusName;
            this.txtCusEmail.Text = this.paramUpdate.CusEmail;
            this.txtCusMobile.Text = this.paramUpdate.CusMobile;
        }

        private bool validateCustomer()
        {
            var companyName = this.txtCompanyName.Text.Trim();
            var companyAddress = this.txtCompanyAddress.Text.Trim();
            var companyMobile = this.txtCompanyMobile.Text.Trim();
            var cusName = this.txtCusName.Text.Trim();
            var cusEmail = this.txtCusEmail.Text.Trim();
            var cusMobile = this.txtCusMobile.Text.Trim();

            if (string.IsNullOrEmpty(cusName))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input CustomerName!");
                this.txtCusName.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(cusMobile))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input CustomerMobile!");
                this.txtCusMobile.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(cusEmail))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input CustomerEmail!");
                this.txtCusEmail.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(companyName))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input CompanyName!");
                this.txtCompanyName.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(companyMobile))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input CompanyMobile!");
                this.txtCompanyMobile.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(companyAddress))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input CompanyAddress!");
                this.txtCompanyAddress.Focus();

                return false;
            }

            return true;
        }

        #endregion

        #region events

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!validateCustomer())
            {
                return;
            }

            var companyName = this.txtCompanyName.Text.Trim();
            var companyAddress = this.txtCompanyAddress.Text.Trim();
            var companyMobile = this.txtCompanyMobile.Text.Trim();
            var cusName = this.txtCusName.Text.Trim();
            var cusEmail = this.txtCusEmail.Text.Trim();
            var cusMobile = this.txtCompanyMobile.Text.Trim();

            SQLiteCommon.BeginTran();

            try
            {
                string sql = "UPDATE M_CUSTOMERS SET COMPANYNAME = '{0}', COMPANYADDRESS = '{1}', COMPANYMOBILE = '{2}', CUSNAME = '{3}' , CUSEMAIL = '{4}' , CUSMOBILE = '{5}', USERID = '{6}' WHERE ID = {7}";
                sql = string.Format(sql, companyName, companyAddress, companyMobile, cusName, cusEmail, cusMobile, CURRENT_USER, paramUpdate.ID);
                SQLiteCommon.ExecuteSqlNonResult(sql);

                SQLiteCommon.CommitTran();
                ShowMsg(MessageBoxIcon.Information, UPDATE_CUSTOMER_SUCCESS);
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, UPDATE_CUSTOMER_FAIL + " : " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!validateCustomer())
            {
                return;
            }

            var companyName = this.txtCompanyName.Text.Trim();
            var companyAddress = this.txtCompanyAddress.Text.Trim();
            var companyMobile = this.txtCompanyMobile.Text.Trim();
            var cusName = this.txtCusName.Text.Trim();
            var cusEmail = this.txtCusEmail.Text.Trim();
            var cusMobile = this.txtCompanyMobile.Text.Trim();

            try
            {
                SQLiteCommon.BeginTran();
                string sql = "INSERT INTO M_CUSTOMERS (COMPANYNAME, COMPANYADDRESS, COMPANYMOBILE, CUSNAME, CUSEMAIL, CUSMOBILE, USERID) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
                sql = string.Format(sql, companyName, companyAddress, companyMobile, cusName, cusEmail, cusMobile, CURRENT_USER);
                SQLiteCommon.ExecuteSqlNonResult(sql);
                SQLiteCommon.CommitTran();
                ShowMsg(MessageBoxIcon.Information, ADD_CUSTOMER_SUCCESS);
                this.btnAdd.Enabled = false;
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ADD_CUSTOMER_FAIL + " : " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SQLiteCommon.BeginTran();

            try
            {
                string sql = "DELETE FROM M_CUSTOMERS WHERE ID = {0}";
                sql = string.Format(sql, paramUpdate.ID);
                SQLiteCommon.ExecuteSqlNonResult(sql);
                SQLiteCommon.CommitTran();
                ShowMsg(MessageBoxIcon.Information, DELETE_CUSTOMER_SUCCESS);
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, DELETE_CUSTOMER_FAIL + " : " + ex.Message);
            }
        }


        private void CustomerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.isSuccess = true;
            this.DialogResult = DialogResult.OK;
        }

        #endregion
    }
}
