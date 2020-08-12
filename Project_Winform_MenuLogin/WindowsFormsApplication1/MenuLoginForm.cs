using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class MenuLoginForm : Form
    {
        int startX = 20;
        int startY = 0;
        int paddingButton = 10;
        Dictionary<string, string> fileCountDict;
        public MenuLoginForm()
        {
            InitializeComponent();

            this.MinimumSize = this.Size;
            var sizeTemplate = btnAddTemplate.Size;
            var locationTemplate = btnAddTemplate.Location;
            var countButonRecord = pnlContain.Controls.Count;
            paddingButton = btnAddTemplate2.Margin.Left;
            startX = locationTemplate.X;
            startY = locationTemplate.Y;

            pnlContain.Controls.Clear();
            // count forder store array
            fileCountDict = new Dictionary<string, string>();
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath);          //Specify the directories you want to manipulate
            var cnn = 1;
            var flat = 1;
            var sizeUp = 0;
            var countFile = di.EnumerateDirectories().Count();

            foreach (DirectoryInfo tmpDi in di.EnumerateDirectories())
            {
                var nameFolder = Path.GetFileNameWithoutExtension(tmpDi.Name);
                var btnAdd = CreateButtonAdd("btnAdd" + cnn.ToString(), nameFolder, sizeTemplate, new Point(startX, startY));

                //Gắn vào panel
                this.pnlContain.SuspendLayout();
                this.pnlContain.Controls.Add(btnAdd);
                this.pnlContain.SuspendLayout();
                this.pnlContain.ResumeLayout(false);

                fileCountDict.Add(btnAdd.Name, tmpDi.FullName);
                startX += btnAdd.Size.Width + paddingButton;

                if (flat % countButonRecord == 0 && flat != countFile)
                {
                    sizeUp += btnAdd.Size.Height + paddingButton;
                    startY += sizeUp;
                    startX = locationTemplate.X;
                }

                flat++;
                cnn++;
            }

            this.Size = new Size(this.Size.Width, this.Size.Height + sizeUp);
        }

        private Button CreateButtonAdd(string buttonName, string buttonText, Size sizeTemplate, Point buttonLocation)
        {
            var btnAdd = new System.Windows.Forms.Button();
            // button add
            btnAdd.Location = buttonLocation;
            btnAdd.Name = buttonName;
            btnAdd.Size = sizeTemplate;
            btnAdd.UseVisualStyleBackColor = true;
            var longCount = buttonText.Length;
            btnAdd.Text = longCount > 12 ? buttonText.Substring(0, 11) + "..." : buttonText;

            // Set event
            btnAdd.Click += new System.EventHandler(btnAddClick);

            //Hover button toltip show
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(btnAdd, buttonText);
            return btnAdd;
        }

        private void btnAddClick(object sender, EventArgs e)
        {
            var objButton = sender as Button;
            var dataMsg = new MsgData
            {
                UserName = txtUserName.Text.Trim(),
                Password = txtPassword.Text.Trim(),
            };

            if (objButton == null)
            {
                return;
            }
            if (!fileCountDict.ContainsKey(objButton.Name))
            {
                return;
            }
            var path = fileCountDict[objButton.Name];
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.exe"); //Getting Text files

            //Check validate
            if (EmptyValidator(txtUserName))
            {
                if (IsValid(txtUserName))
                {
                    return;
                }
                else if (!EmptyValidator(txtPassword, false))
                {
                    return;
                }
            }
            else { return; }

            string fullFilePath = "";
            var jsonData = JsonConvert.SerializeObject(dataMsg); //Convert sang json

            foreach (FileInfo file in Files)
            {
                fullFilePath = file.FullName;
            }
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = fullFilePath;
                    myProcess.StartInfo.Arguments = Base64Encode(jsonData); //chuyển chuỗi json sang base64 để tránh việc mất ký tự
                    myProcess.StartInfo.CreateNoWindow = false;
                    myProcess.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Cover string to Base64Encode
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //validate nhap a-z && 0-9 string and number
        private bool IsValid(TextBox txt, bool hasTrim = true)
        {
            string pattern = "[^0-9a-zA-Z]";
            if (hasTrim)
            {
                txt.Text.Trim();
            }
            if (Regex.IsMatch(txt.Text, pattern))
            {
                MessageBox.Show("Please enter the correct alphanumeric format only");
                txt.Focus();
                return true;
            }

            return false;
        }
        //validate empty
        private bool EmptyValidator(TextBox txt, bool hasTrim = true)
        {
            if (hasTrim)
            {
                txt.Text.Trim();
            }

            if (string.IsNullOrEmpty(txt.Text))
            {
                MessageBox.Show("You must enter a user and pass.");
                txt.Focus();
                return false;
            }

            return true;
        }
        //#region
        ////validate password
        //private bool PassValidator(string input)
        //{
        //    string pattern = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s).*$";    // vali nhập vào it nhất 1 ký tự hoa, 1 ký tự thường, 1 ký tự số
        //    if (Regex.IsMatch(input, pattern))
        //    {
        //        MessageBox.Show("Please enter the correct alphanumeric format only");
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ////vali mail
        //private bool MailValidator(string input)
        //{
        //    string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        //    if (Regex.IsMatch(input, pattern))
        //    {
        //        MessageBox.Show("Please enter the correct alphanumeric format only");
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ////validate string
        //private bool StringValidator(string input)
        //{
        //    string pattern = "[^a-zA-Z]";

        //    return Regex.IsMatch(input, pattern);
        //}
        ////validate integer 
        //private bool IntegerValidator(string input)
        //{
        //    string pattern = "[^0-9]";
        //    if (Regex.IsMatch(input, pattern))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion
        //clear input txtbox
        private void ClearTexts(string user, string pass)
        {
            user = String.Empty;
            pass = String.Empty;
        }

        internal class MsgData
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}

