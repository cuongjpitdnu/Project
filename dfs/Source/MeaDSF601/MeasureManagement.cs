namespace MeaDSF601
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MeasureManagement : BaseForm
    {
        private string MSG_BLANK_FOLDER = "Folder errors is blank";
        private string COMFIRM_NEXT_FILE = "You want to next File?";
        private string FIX_ERR_SUCCESS = "Fix Measure Success";

        private string PathBase;
        private string TempPath;
        private string TargetPath;
        //private const string SOURCE_PATH = @"\Template\";
        private const string SOURCE_PATH = @"\Excel Template\";
        private const string TARGET_PATH = @"\Output\";
        private const string File_Name = "Report.xlsx";
        private const string File_Name_Walking = "ReportWalking.xlsx";

        public MeasureManagement()
        {
            InitializeComponent();

            // Limit resize form
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;

            IntForm();
        }

        private void IntForm()
        {
            PathBase = Application.StartupPath;            

            dgvMeasure.AutoGenerateColumns = false;

            using (var objDb = new clsDBUltity())
            {
                // Binding combobox User
                var tblUser = objDb.GetMUser();
                if(tblUser != null)
                {
                    var rowFirst = tblUser.NewRow();
                    rowFirst["username"] = "All user";
                    rowFirst["userid"] = -1;
                    tblUser.Rows.InsertAt(rowFirst, 0);
                    Common.BindingDataTableToComboBox(cmbUser, tblUser, "username", "userid", -1);
                }                

                // Binding Type
                cmbType.DisplayMember = "Display";
                cmbType.ValueMember = "Value";
                cmbType.Items.Add(new { Display = "All type", Value = -1 });
                cmbType.Items.Add(new { Display = "Alarm Test", Value = (int)clsDBUltity.emMeasureType.AlarmTest });
                cmbType.Items.Add(new { Display = "Walking Test", Value = (int)clsDBUltity.emMeasureType.WalkingTest });
                cmbType.SelectedIndex = 0;

                // Binding Status
                cmbStatus.DisplayMember = "Display";
                cmbStatus.ValueMember = "Value";
                cmbStatus.Items.Add(new { Display = "Complete", Value = (int)clsDBUltity.emMeasureStatus.Complete });
                cmbStatus.Items.Add(new { Display = "Error", Value = (int)clsDBUltity.emMeasureStatus.Error });
                cmbStatus.SelectedIndex = 0;

                var dtStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, 0, 0, 0);
                var dtEnd = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, 23, 59, 59);

                var strSDate = dtStart.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
                var strEDate = dtEnd.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
                int intUser = Common.CnvNullToInt(cmbUser.SelectedValue);
                int intType = Common.CnvNullToInt(((dynamic)cmbType.SelectedItem).Value);
                int intResult = Common.CnvNullToInt(((dynamic)cmbResult.SelectedItem).Value);
                int intStatus = Common.CnvNullToInt(((dynamic)cmbStatus.SelectedItem).Value);

                // Binding grid measure
                dgvMeasure.DataSource = objDb.GetTBLMeasure(strSDate, strEDate, intUser, intType, intResult, intStatus);                
                dgvMeasure.ClearSelection();
                if (dgvMeasure.Rows.Count > 0)
                {
                    dgvMeasure_CellClick(dgvMeasure, new DataGridViewCellEventArgs(0, 0));
                }

                dgvMeasure.Columns["colEnable"].Visible = false;

                if (Common.ModeApp == Common.emModeApp.User)
                {
                    lblStatus.Visible = false;
                    cmbStatus.Visible = false;
                    dgvMeasure.Columns["colDelete"].Visible = false;
                }
            }
        }

        private void dgvMeasure_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            dgvMeasureDetail.AutoGenerateColumns = false;

            // btnDelete
            if (e.ColumnIndex == dgvMeasure.Columns["colDelete"].Index && e.RowIndex >= 0)
            {
                var measureid = Common.CnvNullToInt(dgvMeasure.Rows[e.RowIndex].Cells[MeasureID.Index].Value);
                using (var objDb = new clsDBUltity())
                {
                    // Binding grid measure detail
                    if(Common.ComfirmMsg("Do you want delete this record?"))
                    {
                        objDb.DeleteMeasure(measureid);

                        var dtStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, 0, 0, 0);
                        var dtEnd = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, 23, 59, 59);

                        var strSDate = dtStart.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
                        var strEDate = dtEnd.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
                        int intUser = Common.CnvNullToInt(cmbUser.SelectedValue);
                        int intType = Common.CnvNullToInt(((dynamic)cmbType.SelectedItem).Value);
                        int intResult = Common.CnvNullToInt(((dynamic)cmbResult.SelectedItem).Value);
                        int intStatus = Common.CnvNullToInt(((dynamic)cmbStatus.SelectedItem).Value);

                        // Binding grid measure
                        dgvMeasure.DataSource = objDb.GetTBLMeasure(strSDate, strEDate, intUser, intType, intResult, intStatus);
                        dgvMeasure.ClearSelection();
                        dgvMeasureDetail.DataSource = null;
                        dgvMeasureDetail.ClearSelection();
                    } else return;                   
                }
                return;
            }

            // btnEnable
            if(e.ColumnIndex == dgvMeasure.Columns["colEnable"].Index && e.RowIndex >= 0 && dgvMeasure.Columns["colEnable"].Visible)
            {
                var measureid_selected = Common.CnvNullToInt(dgvMeasure.Rows[e.RowIndex].Cells[MeasureID.Index].Value);
                var pathErrors = Common.PathDataErrors + @"\" + measureid_selected;
                string[] files = null;

                if (Directory.Exists(pathErrors))
                {
                    files = Directory.GetFiles(pathErrors);
                } else
                {
                    using (var fbd = new FolderBrowserDialog())
                    {
                        DialogResult result = fbd.ShowDialog();

                        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                        {
                            files = Directory.GetFiles(fbd.SelectedPath);
                        } else
                        {
                            return;
                        }
                    }
                }
                
                xExecuteNonQueryFromFile(files, measureid_selected);
            }

            var measure_id = Common.CnvNullToString(dgvMeasure.Rows[e.RowIndex].Cells[MeasureID.Index].Value);
            using (var objDb = new clsDBUltity())
            {
                // Binding grid measure detail
                dgvMeasureDetail.DataSource = objDb.GetTBLMeasureDetail(measure_id, chkView.Checked);
                dgvMeasureDetail.ClearSelection();
            }

            // keep selected row after onclick
            int index = e.RowIndex;
            dgvMeasure.Rows[index].Selected = true;
        }

        // Format grid measure
        private void dgvMeasure_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == No.Index)
            {
                e.Value = e.RowIndex + 1;
            }

            if (e.ColumnIndex == MeasureType.Index)
            {
                var intMeasureType = Common.CnvNullToInt(e.Value);
                e.Value = string.Empty;

                if (intMeasureType == (int)clsDBUltity.emMeasureType.AlarmTest)
                {
                    e.Value = "Alarm Test";
                }
                else if (intMeasureType == (int)clsDBUltity.emMeasureType.WalkingTest)
                {
                    e.Value = "Walking Test";
                }
            }

            if (e.ColumnIndex == Result.Index)
            {
                e.Value = Common.MeasureResultDisplay(Common.CnvNullToInt(e.Value));
            }
        }

        // Format grid measure detail
        private void dgvMeasureDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == NoDetail.Index)
            {
                e.Value = e.RowIndex + 1;
            }

            if (e.ColumnIndex == ResultDetail.Index)
            {
                e.Value = Common.MeasureResultDisplay(Common.CnvNullToInt(e.Value));
            }
        }

        private void cmbType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!cmbResult.Enabled || cmbType.SelectedIndex < 0)
            {
                return;
            }

            int selectedValue = Common.CnvNullToInt(((dynamic)cmbType.SelectedItem).Value);

            // Clear all item in combobox
            cmbResult.Items.Clear();

            switch (selectedValue)
            {
                case (int)clsDBUltity.emMeasureType.AlarmTest:
                    cmbResult.DisplayMember = "Display";
                    cmbResult.ValueMember = "Value";
                    cmbResult.Items.Add(new { Display = "All result", Value = -1 });
                    cmbResult.Items.Add(new { Display = "Normal", Value = (int)clsDBUltity.emMeasureResult.Normal });
                    cmbResult.Items.Add(new { Display = "Alarm", Value = (int)clsDBUltity.emMeasureResult.Alarm });
                    cmbResult.SelectedIndex = 0;
                    break;
                case (int)clsDBUltity.emMeasureType.WalkingTest:
                    cmbResult.DisplayMember = "Display";
                    cmbResult.ValueMember = "Value";
                    cmbResult.Items.Add(new { Display = "All result", Value = -1 });
                    cmbResult.Items.Add(new { Display = "Pass", Value = (int)clsDBUltity.emMeasureResult.Pass });
                    cmbResult.Items.Add(new { Display = "Fail", Value = (int)clsDBUltity.emMeasureResult.Fail });
                    cmbResult.SelectedIndex = 0;
                    break;
                default:
                    cmbResult.DisplayMember = "Display";
                    cmbResult.ValueMember = "Value";
                    cmbResult.Items.Add(new { Display = "All result", Value = -1 });
                    cmbResult.Items.Add(new { Display = "Pass", Value = (int)clsDBUltity.emMeasureResult.Pass });
                    cmbResult.Items.Add(new { Display = "Fail", Value = (int)clsDBUltity.emMeasureResult.Fail });
                    cmbResult.Items.Add(new { Display = "Normal", Value = (int)clsDBUltity.emMeasureResult.Normal });
                    cmbResult.Items.Add(new { Display = "Alarm", Value = (int)clsDBUltity.emMeasureResult.Alarm });
                    cmbResult.SelectedIndex = 0;
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Get value for search
            var dtStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, 0, 0, 0);
            var dtEnd = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, 23, 59, 59);

            var strSDate = dtStart.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
            var strEDate = dtEnd.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
            int intUser = Common.CnvNullToInt(cmbUser.SelectedValue);
            int intType = Common.CnvNullToInt(((dynamic)cmbType.SelectedItem).Value);
            int intResult = Common.CnvNullToInt(((dynamic)cmbResult.SelectedItem).Value);
            int intStatus = Common.CnvNullToInt(((dynamic)cmbStatus.SelectedItem).Value);

            if (intStatus == (int)clsDBUltity.emMeasureStatus.Complete)
            {
                dgvMeasure.Columns["colEnable"].Visible = false;
                //dgvMeasure.Columns["colDelete"].Visible = true;
            } else
            {
                dgvMeasure.Columns["colEnable"].Visible = true;
                //dgvMeasure.Columns["colDelete"].Visible = false;
            }

            using (var objDb = new clsDBUltity())
            {
                // Binding grid measure
                dgvMeasure.DataSource = objDb.GetTBLMeasure(strSDate, strEDate, intUser, intType, intResult, intStatus);
                dgvMeasure.ClearSelection();
                dgvMeasureDetail.DataSource = null;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvMeasure.SelectedRows.Count == 1)
            {
                var targetPath = string.Empty;
                // Get value for export
                //var dtStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, 0, 0, 0);
                //var dtEnd = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, 23, 59, 59);

                var strSDate = dgvMeasure.CurrentRow.Cells[StartTime.Index].Value.ToString();
                var strEDate = dgvMeasure.CurrentRow.Cells[EndTime.Index].Value.ToString();
                int intUser = Common.CnvNullToInt(cmbUser.SelectedValue);
                int intType = Common.CnvNullToInt(((dynamic)cmbType.SelectedItem).Value);
                int intResult = Common.CnvNullToInt(((dynamic)cmbResult.SelectedItem).Value);

                var intMeasureId = Common.CnvNullToString(dgvMeasure.CurrentRow.Cells[MeasureID.Index].Value);
                var strDeviceName = Common.CnvNullToString(dgvMeasure.CurrentRow.Cells[DeviceName.Index].Value);
                var intMeasureType = Common.CnvNullToInt(dgvMeasure.CurrentRow.Cells[MeasureType.Index].Value);
                var strType = string.Empty;

                if (intMeasureType == (int)clsDBUltity.emMeasureType.AlarmTest)
                {
                    strType = "Alarm Test";
                }
                else if (intMeasureType == (int)clsDBUltity.emMeasureType.WalkingTest)
                {
                    strType = "Walking Test";
                }

                var strResult = Common.CnvNullToString(Common.MeasureResultDisplay(Common.CnvNullToInt(dgvMeasure.CurrentRow.Cells[Result.Index].Value)));
                var strUser = Common.CnvNullToString(dgvMeasure.CurrentRow.Cells[User.Index].Value);

                // Export excel
                var exportExcel = new AkbExcel();

                TempPath = (intMeasureType == 0) ? PathBase + SOURCE_PATH + File_Name : PathBase + SOURCE_PATH + File_Name_Walking;
                TargetPath = PathBase + SOURCE_PATH;

                if (!File.Exists(TempPath))
                {
                    Common.ShowMsg(MessageBoxIcon.Warning, "Not excel template");
                    return;
                }

                var flag = false;

                try
                {
                    if (intMeasureType == 0)
                    {
                        var strAlarmValue = Common.CnvNullToString(dgvMeasure.CurrentRow.Cells[AlarmValue.Index].Value);
                        string dtNow = DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
                        var sourcePath = TargetPath + "Report.xlsx";
                        targetPath = ValidateExportExcel(sourcePath, DateTime.Now.ToString("yyyyMMddHHmmss") + "_Report");

                        if (string.IsNullOrEmpty(targetPath))
                        {
                            return;
                        }
                        
                        exportExcel.OpenExcel(targetPath, false, false, sourcePath);

                        using (var objDb = new clsDBUltity())
                        {
                            // Binding grid measure detail
                            var dataExport = objDb.GetTBLMeasureDetail(intMeasureId, chkView.Checked);

                            if (Common.TableIsNullOrEmpty(dataExport))
                            {
                                return;
                            }

                            var cellTimeExport = new AkbExcelCellCoordinate(3, 5);          //Time export
                            var cellMeasureID = new AkbExcelCellCoordinate(4, 3);           //Measure ID
                            var cellStart = new AkbExcelCellCoordinate(5, 3);               //Start
                            var cellEnd = new AkbExcelCellCoordinate(6, 3);                 //End                
                            var cellAlarmValue = new AkbExcelCellCoordinate(7, 3);          //Alarm Value
                            var cellUser = new AkbExcelCellCoordinate(4, 5);                //Alarm Value
                            var cellType = new AkbExcelCellCoordinate(5, 5);                //Type
                            var cellResult = new AkbExcelCellCoordinate(6, 5);              //Result
                            var cellDeviceName = new AkbExcelCellCoordinate(7, 5);          //Device Name
                            var cellStartBindingGrid = new AkbExcelCellCoordinate(11, 2);   //Starting row to binding data

                            exportExcel.SetCellValue(cellTimeExport, dtNow);
                            exportExcel.SetCellValue(cellMeasureID, intMeasureId);
                            exportExcel.SetCellValue(cellStart, strSDate);
                            exportExcel.SetCellValue(cellEnd, strEDate);
                            exportExcel.SetCellValue(cellAlarmValue, strAlarmValue);
                            exportExcel.SetCellValue(cellUser, strUser);
                            exportExcel.SetCellValue(cellType, strType);
                            exportExcel.SetCellValue(cellResult, strResult);
                            exportExcel.SetCellValue(cellDeviceName, strDeviceName);

                            var objData = new object[dataExport.Rows.Count, 4];
                            int i = 0;
                            var no = 0;
                            foreach (DataRow item in dataExport.Rows)
                            {
                                objData[i, 0] = Common.CnvNullToString(++no);
                                objData[i, 1] = Common.CnvNullToString(item["samples_time"]);
                                objData[i, 2] = Common.CnvNullToString(item["actual_delegate"]);
                                objData[i, 3] = Common.CnvNullToString(Common.MeasureResultDisplay(Common.CnvNullToInt(item["result"])));
                                i++;
                            }
                            string strRange = exportExcel.SetRangeValue(cellStartBindingGrid, objData);
                            exportExcel.SetBorderAllRange(strRange, Color.Black);
                            if (!exportExcel.Save())
                            {
                                Common.ShowMsg(MessageBoxIcon.Warning, "Error exporting excel data");
                                return;
                            }                            
                        }
                    } else
                    {
                        var strFailLevel = Common.CnvNullToString(dgvMeasure.CurrentRow.Cells[FailLevel.Index].Value);
                        var strPeriod = Common.CnvNullToString(dgvMeasure.CurrentRow.Cells[colPeriod.Index].Value);
                        string dtNow = DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatMiliSecond);
                        var sourcePath = TargetPath + "ReportWalking.xlsx";
                        targetPath = ValidateExportExcel(sourcePath, DateTime.Now.ToString("yyyyMMddHHmmss") + "_ReportWalking");

                        if(string.IsNullOrEmpty(targetPath))
                        {
                            return;
                        }
                        
                        exportExcel.OpenExcel(targetPath, false, false, sourcePath);

                        using (var objDb = new clsDBUltity())
                        {
                            // Get value measure detail limit
                            var dataLimit = objDb.GetTBLMeasureDetailLimit(intMeasureId);

                            // Get value measure detail
                            var dataExport = objDb.GetTBLMeasureDetail(intMeasureId, chkView.Checked);

                            if (Common.TableIsNullOrEmpty(dataLimit) && Common.TableIsNullOrEmpty(dataExport))
                            {
                                return;
                            }

                            var cellTimeExport = new AkbExcelCellCoordinate(3, 5);          //Time export
                            var cellMeasureID = new AkbExcelCellCoordinate(4, 3);           //Measure ID
                            var cellStart = new AkbExcelCellCoordinate(5, 3);               //Start
                            var cellEnd = new AkbExcelCellCoordinate(6, 3);                 //End                
                            var cellAlarmValue = new AkbExcelCellCoordinate(7, 3);          //Alarm Value
                            var cellUser = new AkbExcelCellCoordinate(4, 5);                //Alarm Value
                            var cellType = new AkbExcelCellCoordinate(5, 5);                //Type
                            var cellResult = new AkbExcelCellCoordinate(6, 5);              //Result
                            var cellDeviceName = new AkbExcelCellCoordinate(7, 5);          //Device Name
                            var cellFailLevel = new AkbExcelCellCoordinate(8, 3);           //Fail Level
                            var cellStartBindingLimit = new AkbExcelCellCoordinate(12, 2);   //Fail Level
                            var cellStartBindingGrid = new AkbExcelCellCoordinate(20, 2);   //Starting row to binding data

                            exportExcel.SetCellValue(cellTimeExport, dtNow);
                            exportExcel.SetCellValue(cellMeasureID, intMeasureId);
                            exportExcel.SetCellValue(cellStart, strSDate);
                            exportExcel.SetCellValue(cellEnd, strEDate);
                            exportExcel.SetCellValue(cellAlarmValue, strPeriod);
                            exportExcel.SetCellValue(cellUser, strUser);
                            exportExcel.SetCellValue(cellType, strType);
                            exportExcel.SetCellValue(cellResult, strResult);
                            exportExcel.SetCellValue(cellFailLevel, strFailLevel);
                            exportExcel.SetCellValue(cellDeviceName, strDeviceName);

                            var objLimit = new object[dataLimit.Rows.Count, 3];
                            var j = 0;
                            var num = 0;
                            foreach (DataRow item in dataLimit.Rows)
                            {
                                DateTime samplesTime;
                                objLimit[j, 0] = Common.CnvNullToString(++num);                                

                                if (DateTime.TryParse(Common.CnvNullToString(item["samples_time"]), out samplesTime))
                                {
                                    objLimit[j, 1] = ((DateTime)item["samples_time"]).ToString(clsDBUltity.cstrDateTimeFormatMiliSecond);
                                } else
                                {
                                    objLimit[j, 1] = string.Empty;
                                }

                                objLimit[j, 2] = Common.CnvNullToString(item["actual_delegate"]);
                                j++;
                            }
                            string strRangeLimit = exportExcel.SetRangeValue(cellStartBindingLimit, objLimit);
                            exportExcel.SetBorderAllRange(strRangeLimit, Color.Black);

                            var objData = new object[dataExport.Rows.Count, 3];
                            int i = 0;
                            var no = 0;
                            foreach (DataRow item in dataExport.Rows)
                            {
                                DateTime samplesTime;
                                objData[i, 0] = Common.CnvNullToString(++no);

                                if (DateTime.TryParse(Common.CnvNullToString(item["samples_time"]), out samplesTime))
                                {
                                    objData[i, 1] = ((DateTime)item["samples_time"]).ToString(clsDBUltity.cstrDateTimeFormatMiliSecond);
                                }
                                else
                                {
                                    objData[i, 1] = string.Empty;
                                }

                                objData[i, 2] = Common.CnvNullToString(item["actual_value"]);
                                i++;
                            }
                            string strRange = exportExcel.SetRangeValue(cellStartBindingGrid, objData);
                            exportExcel.SetBorderAllRange(strRange, Color.Black);

                            if (!exportExcel.Save())
                            {
                                Common.ShowMsg(MessageBoxIcon.Warning, "Error exporting excel data");
                                return;
                            }
                        }
                    }

                    flag = true;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (exportExcel != null)
                    {
                        exportExcel.CloseExcel();
                    }
                    exportExcel = null;

                    // Open file saved
                    if (flag && Common.ComfirmMsg("Do you want open file?") == true)
                    {
                        if (File.Exists(targetPath))
                        {
                            System.Diagnostics.Process.Start(targetPath);
                        } else
                        {
                            Common.ShowMsg(MessageBoxIcon.Error, "File export not found");
                        }
                    }
                }
            }
            else
            {
                Common.ShowMsg(MessageBoxIcon.Warning, "Please select 1 row data to export excel");
                return;
            }
        }

        private string ValidateExportExcel(string tempFile, string name = "Book1")
        {
            var saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "xlsx",
                Filter = "Excel Workbook (*.xls, *.xlsx)|*.xls;*.xlsx",
                AddExtension = true,
                RestoreDirectory = true,
                Title = "Save as",
                FileName = name,
                InitialDirectory = @"D:\",
            };

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return "";
            }

            var targetPath = saveFileDialog1.FileName;

            return targetPath;
        }

        private void xExecuteNonQueryFromFile(string[] files, int measureid_selected)
        {
            if (files == null || files.Length == 0)
            {
                Common.ShowMsg(MessageBoxIcon.Warning, "");
                return;
            }

            bool flagCheck = false;
            foreach (var path in files)
            {
                var filename = Path.GetFileName(path);
                var arrTemp = filename.Split('_');

                if (arrTemp.Length == 3)
                {
                    var measureId = Common.CnvNullToInt(arrTemp[0]);

                    if (measureId == measureid_selected)
                    {
                        using (var db = new clsDBUltity())
                        {
                            flagCheck = db.ExecuteNonQueryFromFile(path, false);

                            if (!flagCheck)
                            {
                                Common.ShowMsg(MessageBoxIcon.Error, path);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!Common.ComfirmMsgErr(path + Environment.NewLine + COMFIRM_NEXT_FILE))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (!Common.ComfirmMsgErr(path + Environment.NewLine + COMFIRM_NEXT_FILE))
                    {
                        return;
                    }
                }
            }

            if (flagCheck)
            {
                btnSearch.PerformClick();
                Common.ShowMsg(MessageBoxIcon.Information, FIX_ERR_SUCCESS);
            }
        }

        private void chkView_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvMeasure.CurrentCell != null)
            {
                dgvMeasure_CellClick(dgvMeasure, new DataGridViewCellEventArgs(dgvMeasure.CurrentCell.ColumnIndex, dgvMeasure.CurrentCell.RowIndex));
            }
        }
    }
}
