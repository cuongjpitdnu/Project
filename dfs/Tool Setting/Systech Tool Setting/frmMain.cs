namespace Systech_Tool_Setting
{
    using AKBMySqlCore;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public partial class frmMain : Form
    {
        const string FOMAT_CONNECTION = "Server={0}; database={4}; port={1}; UID={2}; password={3}; Connection Timeout=30; charset=utf8; persistsecurityinfo=True; SslMode=none";

        public frmMain()
        {
            InitializeComponent();

            txtIpAddress.Text = "localhost";
            txtPort.Text = "3306";
            txtUser.Text = "root";
            txtPass.Text = "";
        }

        private void btCreateSetting_Click(object sender, EventArgs e)
        {
            var ConnectionString = string.Format(FOMAT_CONNECTION,
                                                 txtIpAddress.Text,
                                                 txtPort.Text,
                                                 txtUser.Text,
                                                 txtPass.Text,
                                                 "mysql");

            try
            {
                using (var objDb = new clsDBCore(ConnectionString))
                {
                    if (objDb.Open())
                    {
                        var objChecDBExits = objDb.GetTable("SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'systech'");
                        var flag = true;

                        if (objChecDBExits != null && objChecDBExits.Rows.Count > 0)
                        {
                            if (MessageBox.Show("systech database already exists. Would you overwrite this database?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                            {
                                flag = false;
                            }
                        }

                        if (flag)
                        {
                            string fileName = "";

                            if (rdVersion1.Checked)
                            {
                                fileName = @"\systech_v1.sql";
                            }
                            else if (rdVersion2.Checked)
                            {
                                fileName = @"\systech_v2.sql";
                            }

                            using (var objFile = new StreamReader(Application.StartupPath + fileName))
                            {
                                var sql = objFile.ReadToEnd();
                                if (!objDb.ExecuteNonQuery(sql))
                                {
                                    MessageBox.Show("There was an error processing. Please contact admin!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }

                        var objXml = new XmlDocument();
                        objXml.Load(Application.StartupPath + @"\config.xml");
                        var nodeAppSetting = objXml.DocumentElement.SelectSingleNode("//appSettings");

                        if (nodeAppSetting.HasChildNodes)
                        {
                            foreach (XmlNode objChild in nodeAppSetting.ChildNodes)
                            {
                                if (objChild.Attributes.Count == 0)
                                {
                                    continue;
                                }

                                if (objChild.Attributes["key"].Value == "connectionString")
                                {
                                    objChild.Attributes["value"].Value = string.Format(FOMAT_CONNECTION,
                                                                         txtIpAddress.Text,
                                                                         txtPort.Text,
                                                                         txtUser.Text,
                                                                         txtPass.Text,
                                                                         "systech"); ;
                                    break;
                                }
                            }
                        }

                        var pathSave = Application.StartupPath + @"\Output\";

                        if (!Directory.Exists(pathSave))
                        {
                            Directory.CreateDirectory(pathSave);
                        }

                        objXml.Save(pathSave + @"MeaDSF601.exe.config");

                        MessageBox.Show("Setting suscess.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start(pathSave);
                    }
                    else
                    {
                        MessageBox.Show("Connect to database fail. Please check info connect!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    objDb.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error processing. Please contact admin!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rdVersion1_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = rdVersion1.Checked;
            if (isChecked)
            {
                rdVersion2.Checked = false;
            }
            else
            {
                rdVersion2.Checked = true;
            }
        }

        private void rdVersion2_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = rdVersion2.Checked;
            if (isChecked)
            {
                rdVersion1.Checked = false;
            }
            else
            {
                rdVersion1.Checked = true;
            }
        }
    }
}
