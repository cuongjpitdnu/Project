using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPModels;
using Xceed.Words.NET;
using GPConst;
using GPMain.Common.Navigation;
using GPMain.Common;
using GPMain.Views.Tree.Build;
using System.IO;
using GPMain.Common.Excel;
using GPMain.Properties;
using System.Diagnostics;
using GPMain.Common.Helper;
using GPMain.Common.Database;
using System.Threading;
using GPMain.Core;

namespace GPMain.Views
{
    public partial class ExportListMember : BaseUserControl
    {
        string strAfterSearchKeyWord;
        int intAfterSearchGender;
        int intAfterSearchLiveOrDie;
        bool inclan;
        public ExportListMember(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            TitleBar = "Xuất danh sách thành viên";
            strAfterSearchKeyWord = parameters.GetValue<string>("Keyword");
            intAfterSearchGender = parameters.GetValue<int>("Gender");
            intAfterSearchLiveOrDie = parameters.GetValue<int>("LiveOrDie");
            inclan = parameters.GetValue<bool>("Inclan");
        }
        private void ExportDocX(string pathSave)
        {
            try
            {
                using (var memberHelper = new MemberHelper())
                {
                    var strKeyword = strAfterSearchKeyWord;
                    var intGender = intAfterSearchGender;
                    var intLiveOrDie = intAfterSearchLiveOrDie;

                    var isDeath = intLiveOrDie == 0 ? true : false;
                    var chkInClan = inclan;

                    string templatePath = "./Data/Docs/TempMemberInfo.docx";

                    if (!Directory.Exists("./Temp"))
                    {
                        Directory.CreateDirectory("./Temp");
                    }

                    string tempPath = $"./Temp/{ Guid.NewGuid()}.docx";

                    // Create a new document.
                    File.Copy(templatePath, tempPath);

                    var document = DocX.Load(tempPath);

                    var lstMember = memberHelper.FindMember(strKeyword, intGender, intLiveOrDie);

                    var lstAllMember = memberHelper.FindExTMemberOutDictionary("", "", "");

                    if (!(lstMember == null || lstMember.Count == 0))
                    {
                        AppManager.Dialog.ShowProgressBar((progressBar) =>
                        {
                            using var template = DocX.Load(templatePath);
                            var objTableTemplate = template.Tables.FirstOrDefault();
                            progressBar.total = lstMember.Count;
                            progressBar.count = 0;
                            if (lstMember.Count > 0)
                            {
                                InsertDataToTable(document.Tables[0], lstAllMember, lstMember[0]);
                                document.Tables[0].InsertPageBreakAfterSelf();
                            }
                            lstMember.ForEach(member =>
                            {
                                if (member != lstMember[0])
                                {
                                    Xceed.Document.NET.Table objTable = objTableTemplate;

                                    InsertDataToTable(objTable, lstAllMember, member);

                                    document.InsertTable(objTable).InsertPageBreakAfterSelf();

                                    progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                                }
                            });
                        }, "Đang xuất danh sách thành viên...", $"{AppConst.TitleBarFisrt}Xuất file word");
                        if (document.Tables.Count == lstMember.Count)
                        {
                            lblinfo.Text = "Đang lưu file...Xin vui lòng đợi!!!"; Application.DoEvents();
                            document.Save();
                            File.Copy(tempPath, pathSave);
                            AppManager.Dialog.Ok("Xuất file docx thành công.");
                        }
                        else
                        {
                            AppManager.Dialog.Error("Xuất file docx thất bại");
                        }
                        this.Cursor = Cursors.Default;
                    }
                    else
                    {
                        AppManager.Dialog.Error("Danh sách thành viên rỗng.\nXuất file docx thất bại");

                    }
                    File.Delete(tempPath);
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMember), ex);
            }
        }

        private void InsertDataToTable(Xceed.Document.NET.Table objTableTemplate, Dictionary<string, ExTMember> lstAllMember, ExTMember member)
        {
            if (member == null) return;
            Xceed.Document.NET.Table objTable = objTableTemplate;
            var strTel = string.IsNullOrEmpty(member.Contact.Tel_1) ? (string.IsNullOrEmpty(member.Contact.Tel_2) ? "" : member.Contact.Tel_2) : member.Contact.Tel_1;
            var strMail = string.IsNullOrEmpty(member.Contact.Email_1) ? (string.IsNullOrEmpty(member.Contact.Email_2) ? "" : member.Contact.Email_2) : member.Contact.Email_1;
            var typeName = (member.TypeName == null || member.TypeName.Count == 0) ? "" : member.TypeName.FirstOrDefault().Value;

            objTable.Rows[0].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[0].Cells[2].Paragraphs[0].Text, member.Name + " ");
            objTable.Rows[1].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[1].Cells[2].Paragraphs[0].Text, typeName + "");
            objTable.Rows[2].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[2].Cells[2].Paragraphs[0].Text, member.LevelInFamily.ToString());
            objTable.Rows[4].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[4].Cells[2].Paragraphs[0].Text, member.Birthday?.ToDateSun() + "");
            objTable.Rows[5].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[5].Cells[2].Paragraphs[0].Text, member.Birthday?.ToDateMoon() + "");
            objTable.Rows[6].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[6].Cells[2].Paragraphs[0].Text, member.BirthPlace + "");
            objTable.Rows[7].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[7].Cells[2].Paragraphs[0].Text, member.HomeTown + "");
            objTable.Rows[8].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[8].Cells[2].Paragraphs[0].Text, member.Contact?.Address + "");
            objTable.Rows[9].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[9].Cells[2].Paragraphs[0].Text, strTel + "\n" + strMail);
            objTable.Rows[11].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[11].Cells[2].Paragraphs[0].Text, member.IsDeath ? member.DeadDay?.ToDateSun() : "");
            objTable.Rows[12].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[12].Cells[2].Paragraphs[0].Text, member.IsDeath ? member.DeadDay?.ToDateMoon() : "");
            objTable.Rows[13].Cells[2].Paragraphs[0].ReplaceText(objTable.Rows[13].Cells[2].Paragraphs[0].Text, member.DeadPlace + "");

            // find related member of member
            var lstRelationMember = GetMemberRelation2(lstAllMember, member);

            objTable.Rows[15].Cells[0].Paragraphs[0].ReplaceText(objTable.Rows[15].Cells[0].Paragraphs[0].Text, lstRelationMember[0] + "");
            objTable.Rows[15].Cells[1].Paragraphs[0].ReplaceText(objTable.Rows[15].Cells[1].Paragraphs[0].Text, lstRelationMember[3] + "");
            objTable.Rows[17].Cells[0].Paragraphs[0].ReplaceText(objTable.Rows[17].Cells[0].Paragraphs[0].Text, lstRelationMember[1] + "");
            objTable.Rows[17].Cells[1].Paragraphs[0].ReplaceText(objTable.Rows[17].Cells[1].Paragraphs[0].Text, lstRelationMember[2] + "");

            var lstJob = member.Job.Select(x => $"Công ty:{x.CompanyName}. Vị trí:{x.Position}. Công việc:{x.Job}").ToList();
            if (lstJob.Count > 0)
            {
                objTable.Rows[19].Cells[0].Paragraphs[0].ReplaceText(objTable.Rows[19].Cells[0].Paragraphs[0].Text, string.Join("\n", lstJob));
            }
            var lstEvent = member.Event.Select(x => $"{x.EventName}. Thời gian:{x.StartDate.ToDateSun()}~{x.EndDate.ToDateSun()}").ToList();
            if (lstEvent.Count > 0)
            {
                objTable.Rows[21].Cells[0].Paragraphs[0].ReplaceText(objTable.Rows[21].Cells[0].Paragraphs[0].Text, string.Join("\n", lstEvent));
            }
            objTable.Rows[23].Cells[0].Paragraphs[0].ReplaceText(objTable.Rows[23].Cells[0].Paragraphs[0].Text, member.Contact.Note);
        }

        private string[] GetMemberRelation2(Dictionary<string, ExTMember> tblTMember, ExTMember mainMember)
        {
            var lstParent = mainMember.ListPARENT;
            var lstSpouse = mainMember.ListSPOUSE;
            var lstChildren = mainMember.ListCHILDREN;

            if (lstParent == null)
            {
                lstParent = new List<string>();
            }
            if (lstSpouse == null)
            {
                lstSpouse = new List<string>();
            }
            if (lstChildren == null)
            {
                lstChildren = new List<string>();
            }

            string[] lstMemberRet = new string[4];
            if (lstParent.Count > 0)
            {
                List<string> lstNameParent = new List<string>();
                var lstChildOfParent = tblTMember[lstParent[0]].ListCHILDREN;
                foreach (string mem in lstParent)
                {
                    var member = tblTMember[mem];
                    lstNameParent.Add(member.Name);
                    if (member.ListCHILDREN.Count > lstChildOfParent.Count)
                    {
                        lstChildOfParent = member.ListCHILDREN;
                    }
                };
                lstMemberRet[0] = (string.Join("\n", lstNameParent.Where(x => !string.IsNullOrEmpty(x))));

                var lstSiblings = lstChildOfParent.Where(x => x != mainMember.Id).ToList();
                if (lstSiblings.Count > 0)
                {
                    List<string> arrSiblings = new List<string>();

                    foreach (var sib in lstSiblings)
                    {
                        var member = tblTMember[sib];
                        if (member != null)
                        {
                            arrSiblings.Add(member.Name);
                        }
                    }
                    lstMemberRet[3] = (string.Join("\n", arrSiblings.Where(x => !string.IsNullOrEmpty(x))));
                }
            }

            if (lstSpouse.Count > 0)
            {
                List<string> lstNameSpouse = new List<string>();

                foreach (string mem in lstSpouse)
                {
                    var member = tblTMember[mem];
                    lstNameSpouse.Add(member.Name);
                };
                lstMemberRet[1] = (string.Join("\n", lstNameSpouse.Where(x => !string.IsNullOrEmpty(x))));
            }

            if (lstChildren.Count > 0)
            {
                List<string> lstNameChildren = new List<string>();

                foreach (string mem in lstChildren)
                {
                    var member = tblTMember[mem];
                    lstNameChildren.Add(member.Name);
                };
                lstMemberRet[2] = (string.Join("\n", lstNameChildren.Where(x => !string.IsNullOrEmpty(x))));
            }
            return lstMemberRet;
        }
        private void btnword_Click(object sender, EventArgs e)
        {
        SaveFileDialog:
            string pathFileToSave = AppManager.Dialog.SaveFile($"Danh sách thành viên {DateTime.Now.ToString("ddMMyyyy-HHmm")}.docx", "File word gia pha|*.docx", "Xuất file danh sách thành viên");
            if (string.IsNullOrEmpty(pathFileToSave)) return;
            if (File.Exists(pathFileToSave))
            {
                try
                {
                    File.Delete(pathFileToSave);
                }
                catch
                {
                    AppManager.Dialog.Error("Tên file đã tồn tại và đang được mở.");
                    goto SaveFileDialog;
                }
            }

            try
            {
                this.Cursor = Cursors.WaitCursor; Application.DoEvents();
                btnword.Enabled = btnexcel.Enabled = false;
                ExportDocX(pathFileToSave);
                Close();
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(Events), ex);
            }
            finally
            {
                btnword.Enabled = btnexcel.Enabled = true;
                Application.UseWaitCursor = false;
                if (File.Exists(pathFileToSave) && AppManager.Dialog.Confirm($"Xuất danh sách sự kiện thành công!\n{Resources.MSG_CONFIRM_OPEN_FILE}"))
                {
                    Process.Start(pathFileToSave);
                }
            }
        }

        private void btnexcel_Click(object sender, EventArgs e)
        {

            string strKeyword = strAfterSearchKeyWord;
            int intGender = intAfterSearchGender;
            int intLiveOrDie = intAfterSearchLiveOrDie;
            var chkInClan = inclan;

            string pathTemplate = "./Data/Excel/ListMemberTemplate.xltx";
        SaveFileDialog:
            string pathFileToSave = AppManager.Dialog.SaveFile($"Danh sách thành viên {DateTime.Now.ToString("ddMMyyyy-HHmm")}.xlsx", "File Excel|*.xlsx");
            if (string.IsNullOrEmpty(pathFileToSave)) return;
            Stopwatch sw = new Stopwatch();

            if (File.Exists(pathFileToSave))
            {
                try
                {
                    File.Delete(pathFileToSave);
                }
                catch
                {
                    AppManager.Dialog.Error("Tên file đã tồn tại và đang được mở.");
                    goto SaveFileDialog;
                }
            }
            if (!Directory.Exists("./Temp"))
            {
                Directory.CreateDirectory("./Temp");
            }
            string tempPath = "./Temp/" + Guid.NewGuid() + ".xlsx";
            File.Copy(pathTemplate, tempPath);
            try
            {
                using (MemberHelper memberHelper = new MemberHelper())
                {
                    sw.Restart();
                    var lstMember = memberHelper.FindMember(strKeyword, intGender, intLiveOrDie);
                    ExportExcelListMember exportExcel = new ExportExcelListMember();

                    this.Cursor = Cursors.WaitCursor;
                    btnword.Enabled = btnexcel.Enabled = false;
                    AppManager.Dialog.ShowProgressBar(progressBar =>
                    {
                        exportExcel.Export(lstMember, tempPath, pathFileToSave, progressBar);
                    }, "Đang xuất danh sách thành viên...", $"{AppConst.TitleBarFisrt}Xuất file excel");
                    sw.Stop();
                    Close();
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(Events), ex);
            }
            finally
            {
                btnword.Enabled = btnexcel.Enabled = false;
                if (File.Exists(pathFileToSave) && AppManager.Dialog.Confirm($"Xuất danh sách sự kiện thành công!\n{Resources.MSG_CONFIRM_OPEN_FILE}"))
                {
                    Process.Start(pathFileToSave);
                }
            }
        }
    }

}
