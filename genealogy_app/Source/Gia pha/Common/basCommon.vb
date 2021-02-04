'   ******************************************************************
'      TITLE      : Common Functions
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/07/14　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict Off

Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports System.IO
Imports System
Imports System.Management

Imports System.Net.NetworkInformation
Imports System.Net
Imports System.Text
Imports PdfSharp.Drawing


Imports System.Drawing.Imaging
Imports PdfSharp.Pdf
Imports PdfSharp
Imports OfficeOpenXml
Imports System.Threading
Imports Config_Gia_Pha
Imports System.Reflection



'   ******************************************************************
'　　　FUNCTION   : Common class
'      MEMO       : 
'      CREATE     : 2011/07/18  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Module basCommon


    Private Const mcstrClsName As String = "basCommon"                                         'class name

    Private Const mcstrImageNotExist As String = "File ảnh được chọn không tồn tại."                      'message when image file doesn't exist
    Private Const mcstrFileNotExist As String = "File được chọn không tồn tại."                          'message when file doesn't exist
    Private Const mcstrFolderNotExist As String = "Folder không tồn tại."                              'message when image file doesn't exist
    Private Const mcstrImageWrong As String = "File được chọn không phải là file ảnh."                       'message when image file is incorrect format
    Private Const mcstrTrialFail As String = "Bạn không thể lưu trữ quá {0} thành viên trong phiên bản dùng thử"  'message when does not pass trial check
    Private Const mcstrMsWordRequired As String = "Bạn cần cài đặt Microsoft Word để mở file này."            'message when can not open word file
    Private Const mcstrFileExist As String = "File đã tồn tại, file hiện tại sẽ mất nếu bạn tiếp tục. Bạn có muốn tiếp tục?"    'file exist message

    Private Const mcstrGenderFilter As String = "GENDER = {0}"                                  'filter by gender
    Private Const mcstrDDMMformat As String = "{0} tháng {1}"                              'date format
    Private Const mcstrDDMMYYYYformat As String = "{0} tháng {1} năm {2}"                  'date format
    Private Const mcstrMMYYYYformat As String = "Tháng {0} năm {1}"                             'date format
    Private Const mcstrDDMMformatShort As String = "{0}/{1}"                                    'date format
    Private Const mcstrDDMMYYYYformatShort As String = "{0}/{1}/{2}"                            'date format
    Private Const mcstrMMYYYYformatShort As String = "{0}/{1}"                                  'date format

    Public Const gcstrFileGIF As String = ".gif"                                               'file format
    Public Const gcstrFileJPG As String = ".jpg"                                               'file format
    Public Const gcstrFilePNG As String = ".bmp"                                               'file format
    Public Const gcstrFileBMP As String = ".png"                                               'file format
    Public Const gcstrMapFolder As String = "Software\98ace5bb95e715a0f8780be16d9960c3\c0d474c4fa48e7b3216896a2655ed0ce"     'Registry

    Public gobjImageAvatar As Image = Nothing
    Public gobjPic As List(Of usrPic)
    Public gblnAddNew As Boolean = False
    Public gstrDataPath As String = ""

    ' ▽ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------
    Public Const gcstrNoData As String = "Không có dữ liệu để in."
    ' △ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------

    Public Sub fncSetBufferBetween2Card()

        clsDefine.MEM_CARD_HORIZON_BUFFER_L = My.Settings.intHozBuffer
        clsDefine.MEM_CARD_VERTICAL_BUFFER_L = My.Settings.intVerBuffer
        clsDefine.MEM_CARD_HORIZON_BUFFER_S = My.Settings.intHozBuffer
        clsDefine.MEM_CARD_VERTICAL_BUFFER_S = My.Settings.intVerBuffer
        clsDefine.MEMCARD_2_VERTICAL_BUFFER = My.Settings.intVerBuffer

    End Sub

#Region "Export Excel"

    Public Function CopyDataGridView(dgv_org As DataGridView) As DataGridView

        Dim dgv_copy As New DataGridView()

        Try

            If dgv_copy.Columns.Count = 0 Then
                For Each dgvc As DataGridViewColumn In dgv_org.Columns
                    dgv_copy.Columns.Add(TryCast(dgvc.Clone(), DataGridViewColumn))
                Next
            End If

            Dim row As New DataGridViewRow()

            For i As Integer = 0 To dgv_org.Rows.Count - 1
                row = DirectCast(dgv_org.Rows(i).Clone(), DataGridViewRow)
                Dim intColIndex As Integer = 0
                For Each cell As DataGridViewCell In dgv_org.Rows(i).Cells
                    row.Cells(intColIndex).Value = cell.Value
                    intColIndex += 1
                Next
                dgv_copy.Rows.Add(row)
            Next
            dgv_copy.AllowUserToAddRows = False

            dgv_copy.Refresh()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "DataTableToExcel", ex)
        End Try

        Return dgv_copy
    End Function

    Public Function DataTableToExcel(ByVal datagridview As DataGridView,
                                     ByVal tblData As DataTable,
                                     ByVal strTempFile As String,
                                     ByVal strTitle As String) As String

        DataTableToExcel = String.Empty

        Const cstr_FOMAT_RANGE_TITLE As String = "A1:{0}1"
        Const cstr_FOMAT_RANGE_NGAY_XUAT_BAN As String = "A2:{0}2"
        Const cstr_FOMAT_NGAY_XUAT_BAN As String = "Ngày xuất bản: {0}"
        Const cstr_CELL_TITLE As String = "A1"
        Const cstr_CELL_NGAY_XUAT_BAN As String = "A2"
        Const START_COLUMN As Integer = 1
        Const START_ROW As Integer = 3

        Dim excelPackage As New ExcelPackage()

        Try

            ' ▽ 2017/06/02 AKB Nguyen Thanh Tung -------------------------------
            If IsNothing(tblData) Then
                fncMessageWarning(gcstrNoData, "")
                Exit Function
            End If

            If tblData.Rows.Count <= 0 Then
                fncMessageWarning(gcstrNoData, "")
                Exit Function
            End If
            ' △ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------

            Dim SHEET_NAME As String = "Danh sach"

            'Threading.Thread.CurrentThread.SetApartmentState(Threading.ApartmentState.STA)
            Dim saveFileDialog1 As New SaveFileDialog()
            Dim t As Threading.Thread = New Threading.Thread(
                    DirectCast(Sub()

                                   saveFileDialog1.DefaultExt = "xlsx"
                                   saveFileDialog1.Filter = "Excel Workbook (*.xlsx)|*.xlsx"
                                   saveFileDialog1.AddExtension = True
                                   saveFileDialog1.RestoreDirectory = True
                                   saveFileDialog1.Title = "Save as"
                                   saveFileDialog1.InitialDirectory = Application.ExecutablePath

                                   ' ▽ 2017/06/02 AKB Nguyen Thanh Tung ---------
                                   saveFileDialog1.FileName = strTitle.Replace(vbCrLf, "").Replace(":", "-")
                                   ' △ 2017/06/02 AKB Nguyen Thanh Tung ---------

                                   If saveFileDialog1.ShowDialog() <> DialogResult.OK Then
                                       Return
                                   End If

                               End Sub, ThreadStart)
              )

            t.SetApartmentState(ApartmentState.STA)
            t.Start()
            t.Join()

            Dim filePath As String = saveFileDialog1.FileName
            Dim worksheet As ExcelWorksheet

            Dim i_StartCol As Integer = 0, i_StartRow As Integer = 0
            If strTempFile Is Nothing Then
                'Create a sheet
                excelPackage.Workbook.Worksheets.Add(SHEET_NAME)
                worksheet = excelPackage.Workbook.Worksheets(1)
                ' Openning first Worksheet
                i_StartCol = 1
                i_StartRow = 1

                'Set title file excel
                If Not String.IsNullOrEmpty(strTitle) Then
                    i_StartCol = START_COLUMN
                    i_StartRow = START_ROW
                End If
            Else
                ' 'Template1.xlsx' is treated as template file
                Dim templateFile As New FileInfo(strTempFile)
                ' Making a new file
                Dim newFile As New FileInfo(filePath)

                ' If there is any file having same name as newFile, then delete it first
                If newFile.Exists Then
                    newFile.Delete()
                    newFile = New FileInfo(filePath)
                End If
                excelPackage = New ExcelPackage(newFile, templateFile)
                worksheet = excelPackage.Workbook.Worksheets(1)
                i_StartCol = START_COLUMN
                i_StartRow = START_ROW
            End If

            ' Set row header file excel
            Dim excelColumn As Integer = 0
            For i As Integer = 0 To datagridview.Columns.Count - 1
                If datagridview.Columns(i).Visible = False Then
                    Continue For
                End If
                ' Set width of column in file excel
                Dim dgWidth As Integer = datagridview.Columns(i).Width
                If dgWidth > 0 Then
                    Dim excelWidth As Double = dgWidth * 10 / 75
                    'Excel: column width type 10 (75 pixels wide)
                    worksheet.Column(i_StartCol + excelColumn).Width = excelWidth
                End If
                ' Set header of column in excel file
                worksheet.Cells(i_StartRow, i_StartCol + excelColumn).Value = datagridview.Columns(i).HeaderText

                Dim intMaxWidthColumn As Integer = 0

                If datagridview.Rows(0).Cells(i).ValueType = GetType(System.Drawing.Image) _
                Or datagridview.Rows(0).Cells(i).Value.ToString.Trim.ToUpper = "System.Drawing.Bitmap".ToUpper Then
                    Dim objIMG As System.Drawing.Image
                    Dim objExcelIMG As OfficeOpenXml.Drawing.ExcelPicture

                    For j As Integer = 0 To tblData.Rows.Count - 1

                        objIMG = CType(tblData.Rows(j)(i), Image)

                        'If tblData.Rows(j)(i) = clsEnum.emGender.MALE Then
                        '    objIMG = GiaPha.My.Resources.Gender_man16
                        'Else
                        '    objIMG = GiaPha.My.Resources.Gender_woman16
                        'End If

                        objExcelIMG = worksheet.Drawings.AddPicture(j.ToString, objIMG)
                        objExcelIMG.SetPosition(i_StartRow + j, 3, excelColumn, 3)
                    Next
                Else
                    For j As Integer = 0 To tblData.Rows.Count - 1
                        worksheet.Cells(i_StartRow + j + 1, i_StartCol + excelColumn).Value = tblData.Rows(j)(i).ToString
                    Next

                    worksheet.Column(i_StartCol + excelColumn).AutoFit()
                    worksheet.Column(i_StartCol + excelColumn).BestFit = True
                End If

                excelColumn += 1
            Next

            If Not String.IsNullOrEmpty(strTitle) Then
                'Fomat Cell
                worksheet.Cells(String.Format(cstr_FOMAT_RANGE_TITLE, xGetNameColumnExcel(excelColumn))).Merge = True
                worksheet.Cells(String.Format(cstr_FOMAT_RANGE_NGAY_XUAT_BAN, xGetNameColumnExcel(excelColumn))).Merge = True
                worksheet.Cells(cstr_CELL_TITLE).Style.Font.Bold = True

                'Set Title and Ngay Xuat Ban
                worksheet.Cells(cstr_CELL_TITLE).Value = strTitle
                worksheet.Cells(cstr_CELL_NGAY_XUAT_BAN).Value = String.Format(cstr_FOMAT_NGAY_XUAT_BAN, DateTime.Now().ToString("dd/MM/yyy"))

                'HorizontalAlignment for Title and Ngay Xuat Ban
                worksheet.Cells(cstr_CELL_TITLE & ":" & cstr_CELL_NGAY_XUAT_BAN).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
            End If

            ' Save data to file
            Dim bin As [Byte]() = excelPackage.GetAsByteArray()
            File.WriteAllBytes(filePath, bin)

            'Open file excel after export
            If System.IO.File.Exists(filePath) Then
                System.Diagnostics.Process.Start(filePath)
            End If

            Return filePath
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "DataTableToExcel", ex)
        Finally
            excelPackage.Dispose()
        End Try
    End Function

    Public Function DataGridToExcel(ByVal datagridview As DataGridView,
                                    ByVal tempFile As String,
                                    ByVal strTitle As String) As String

        DataGridToExcel = String.Empty

        Dim excelPackage As New ExcelPackage()
        Try
            ' ▽ 2017/06/02 AKB Nguyen Thanh Tung -------------------------------
            If IsNothing(datagridview) Then
                fncMessageWarning(gcstrNoData, "")
                Exit Function
            End If

            If datagridview.Rows.Count <= 0 Then
                fncMessageWarning(gcstrNoData, "")
                Exit Function
            End If
            ' △ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------
            Dim START_ROW As Integer = 3
            ' ▽ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------
            Const cstr_FOMAT_RANGE_TITLE As String = "A1:{0}1"
            Const cstr_FOMAT_RANGE_NGAY_XUAT_BAN As String = "A2:{0}2"
            Const cstr_FOMAT_NGAY_XUAT_BAN As String = "Ngày xuất bản: {0}"
            Const cstr_CELL_TITLE As String = "A1"
            Const cstr_CELL_NGAY_XUAT_BAN As String = "A2"

            Const START_COLUMN As Integer = 1
            'Dim START_COLUMN As Integer = 2
            ' △ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------
            Dim SHEET_NAME As String = "Danh sach"

            'Threading.Thread.CurrentThread.SetApartmentState(Threading.ApartmentState.STA)
            Dim saveFileDialog1 As New SaveFileDialog()
            Dim t As Threading.Thread = New Threading.Thread(
                    DirectCast(Sub()

                                   saveFileDialog1.DefaultExt = "xlsx"
                                   saveFileDialog1.Filter = "Excel Workbook (*.xlsx)|*.xlsx"
                                   saveFileDialog1.AddExtension = True
                                   saveFileDialog1.RestoreDirectory = True
                                   saveFileDialog1.Title = "Save as"
                                   saveFileDialog1.InitialDirectory = Application.ExecutablePath

                                   ' ▽ 2017/06/02 AKB Nguyen Thanh Tung ---------
                                   saveFileDialog1.FileName = strTitle.Replace(vbCrLf, "").Replace(":", "-")
                                   ' △ 2017/06/02 AKB Nguyen Thanh Tung ---------

                                   If saveFileDialog1.ShowDialog() <> DialogResult.OK Then
                                       Return
                                   End If

                               End Sub, ThreadStart)
              )

            t.SetApartmentState(ApartmentState.STA)
            t.Start()
            t.Join()

            Dim filePath As String = saveFileDialog1.FileName
            Dim worksheet As ExcelWorksheet

            Dim i_StartCol As Integer = 0, i_StartRow As Integer = 0
            If tempFile Is Nothing Then
                'Create a sheet
                excelPackage.Workbook.Worksheets.Add(SHEET_NAME)
                worksheet = excelPackage.Workbook.Worksheets(1)
                ' Openning first Worksheet
                i_StartCol = 1
                i_StartRow = 1
                ' ▽ 2017/06/02 AKB Nguyen Thanh Tung -----------------------------
                'Set title file excel
                If Not String.IsNullOrEmpty(strTitle) Then
                    i_StartCol = START_COLUMN
                    i_StartRow = START_ROW

                    ''Fomat Cell
                    'worksheet.Cells(String.Format(cstr_FOMAT_RANGE_TITLE, xGetNameColumnExcel(datagridview.Columns.Count - 1))).Merge = True
                    'worksheet.Cells(String.Format(cstr_FOMAT_RANGE_NGAY_XUAT_BAN, xGetNameColumnExcel(datagridview.Columns.Count - 1))).Merge = True
                    'worksheet.Cells(cstr_CELL_TITLE).Style.Font.Bold = True

                    ''Set Title and Ngay Xuat Ban
                    'worksheet.Cells(cstr_CELL_TITLE).Value = strTitle
                    'worksheet.Cells(cstr_CELL_NGAY_XUAT_BAN).Value = String.Format(cstr_FOMAT_NGAY_XUAT_BAN, DateTime.Now().ToString("dd/MM/yyy"))

                    ''HorizontalAlignment for Title and Ngay Xuat Ban
                    'worksheet.Cells(cstr_CELL_TITLE & ":" & cstr_CELL_NGAY_XUAT_BAN).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                End If
                ' △ 2017/06/02 AKB Nguyen Thanh Tung -----------------------------
            Else
                ' 'Template1.xlsx' is treated as template file
                Dim templateFile As New FileInfo(tempFile)
                ' Making a new file
                Dim newFile As New FileInfo(filePath)

                ' If there is any file having same name as newFile, then delete it first
                If newFile.Exists Then
                    newFile.Delete()
                    newFile = New FileInfo(filePath)
                End If
                excelPackage = New ExcelPackage(newFile, templateFile)
                worksheet = excelPackage.Workbook.Worksheets(1)
                i_StartCol = START_COLUMN
                i_StartRow = START_ROW
            End If

            ' Set row header file excel
            Dim excelColumn As Integer = 0
            For i As Integer = 0 To datagridview.Columns.Count - 1
                If datagridview.Columns(i).Visible = False Then
                    Continue For
                End If
                ' Set width of column in file excel
                Dim dgWidth As Integer = datagridview.Columns(i).Width
                If dgWidth > 0 Then
                    Dim excelWidth As Double = dgWidth * 10 / 75
                    'Excel: column width type 10 (75 pixels wide)
                    worksheet.Column(i_StartCol + excelColumn).Width = excelWidth
                End If
                ' Set header of column in excel file
                worksheet.Cells(i_StartRow, i_StartCol + excelColumn).Value = datagridview.Columns(i).HeaderText

                ' ▽ 2017/06/02 AKB Nguyen Thanh Tung ----------------------------
                Dim intMaxWidthColumn As Integer = 0

                If datagridview.Rows(0).Cells(i).ValueType = GetType(System.Drawing.Image) _
                Or datagridview.Rows(0).Cells(i).Value.ToString.Trim.ToUpper = "System.Drawing.Bitmap".ToUpper Then
                    Dim objIMG As System.Drawing.Image
                    Dim objExcelIMG As OfficeOpenXml.Drawing.ExcelPicture

                    For j As Integer = 0 To datagridview.Rows.Count - 1
                        objIMG = CType(datagridview.Rows(j).Cells(i).Value, System.Drawing.Image)
                        objExcelIMG = worksheet.Drawings.AddPicture(j.ToString, objIMG)
                        objExcelIMG.SetPosition(i_StartRow + j, 3, excelColumn, 3)
                    Next
                Else
                    For j As Integer = 0 To datagridview.Rows.Count - 1
                        worksheet.Cells(i_StartRow + j + 1, i_StartCol + excelColumn).Value = datagridview.Rows(j).Cells(i).Value.ToString
                    Next

                    worksheet.Column(i_StartCol + excelColumn).AutoFit()
                    worksheet.Column(i_StartCol + excelColumn).BestFit = True
                End If
                ' △ 2017/06/02 AKB Nguyen Thanh Tung ----------------------------

                excelColumn += 1
            Next

            If Not String.IsNullOrEmpty(strTitle) Then
                'Fomat Cell
                worksheet.Cells(String.Format(cstr_FOMAT_RANGE_TITLE, xGetNameColumnExcel(excelColumn))).Merge = True
                worksheet.Cells(String.Format(cstr_FOMAT_RANGE_NGAY_XUAT_BAN, xGetNameColumnExcel(excelColumn))).Merge = True
                worksheet.Cells(cstr_CELL_TITLE).Style.Font.Bold = True

                'Set Title and Ngay Xuat Ban
                worksheet.Cells(cstr_CELL_TITLE).Value = strTitle
                worksheet.Cells(cstr_CELL_NGAY_XUAT_BAN).Value = String.Format(cstr_FOMAT_NGAY_XUAT_BAN, DateTime.Now().ToString("dd/MM/yyy"))

                'HorizontalAlignment for Title and Ngay Xuat Ban
                worksheet.Cells(cstr_CELL_TITLE & ":" & cstr_CELL_NGAY_XUAT_BAN).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
            End If
            ' ▽ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------
            'Dim dtList As DataTable = GetDataTableFromDGV(datagridview)

            '' Fill data from datagridview to sheet at cell found
            'worksheet.Cells(i_StartRow + 1, i_StartCol).LoadFromDataTable(dtList, False)
            ' △ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------

            'Format excel columns the same as datagridview columns
            'FormatColumnDataExcel(datagridview, worksheet, i_StartRow, i_StartCol)

            ' Save data to file
            Dim bin As [Byte]() = excelPackage.GetAsByteArray()
            File.WriteAllBytes(filePath, bin)

            ' ▽ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------
            'Open file excel after export
            If System.IO.File.Exists(filePath) Then
                System.Diagnostics.Process.Start(filePath)
            End If
            ' △ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------
            Return filePath
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        Finally
            excelPackage.Dispose()
        End Try
        Return Nothing
    End Function

    '   ******************************************************************
    '　　　	FUNCTION    : xGetNameColumnExcel
    '      	MEMO        : Convert index column to name column
    '       PARAMS      : ARG1 - Interger - Index Column(start from 0)
    '      	CREATE      : 2017/06/02 AKB Nguyen Thanh Tung
    '      	UPDATE      : 
    '   ******************************************************************
    Private Function xGetNameColumnExcel(Number As Integer) As String
        Number = Number - 1
        Dim St As String = (If(Number >= 26, Microsoft.VisualBasic.Strings.Chr(CInt(Microsoft.VisualBasic.Conversion.Int(Number / 26) + 64)).ToString(), "")) + Microsoft.VisualBasic.Strings.Chr(Number Mod 26 + 65).ToString()
        Return St
    End Function

    Private Function GetDataTableFromDGV(dgv As DataGridView) As DataTable
        Dim dt As DataTable = New DataTable()
        Dim intCols As Integer

        intCols = 0
        For Each column As DataGridViewColumn In dgv.Columns
            If column.Visible Then
                ' You could potentially name the column based on the DGV column name (beware of dupes)
                ' or assign a type based on the data type of the data bound to this DGV column.
                dt.Columns.Add()
                intCols += 1
            End If
        Next

        Dim cellValues As Object() = New Object(intCols - 1) {}
        Dim j As Integer
        For Each row As DataGridViewRow In dgv.Rows

            j = 0
            For i As Integer = 0 To row.Cells.Count - 1
                If (row.Cells(i).Visible) Then
                    cellValues(j) = row.Cells(i).Value
                    j += 1
                End If
            Next
            dt.Rows.Add(cellValues)
        Next

        Return dt
    End Function
#End Region

    '   ******************************************************************
    '　　　FUNCTION   : CnvV2E
    '      MEMO       : 
    '      CREATE     : 2012/12/06  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCnvV2E(ByVal strV As String) As String
        fncCnvV2E = ""
        Try
            Dim strVal As String
            strVal = strV
            Const TextToFind As String = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ"
            Const TextToReplace As String = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY"

            Dim index As Int32 = -1

            While (strVal.IndexOfAny(TextToFind.ToCharArray()) <> -1)
                index = strVal.IndexOfAny(TextToFind.ToCharArray())
                Dim index2 As Integer = TextToFind.IndexOf(strVal(index))
                strVal = strVal.Replace(strVal(index), TextToReplace(index2))
            End While

            Return strVal
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncCnvV2E", ex)
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncMessageInfo, show info message dialog
    '　　　VALUE      : DialogResult
    '      PARAMS1    : strMsg string, message to show
    '      PARAMS2    : strTitle string, title of messagebox
    '      MEMO       : 
    '      CREATE     : 2011/11/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMessageInfo(ByVal strMsg As String, Optional ByVal strTitle As String = Nothing) As DialogResult

        fncMessageInfo = DialogResult.OK

        Try
            Dim strMsgTitle As String = ""

            'set default title
            strMsgTitle = basConst.gcstrProductName

            'set title if parameter is avaiable
            If Not String.IsNullOrEmpty(strTitle) Then strMsgTitle = strTitle

            fncMessageInfo = MessageBox.Show(strMsg, strMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            Throw ex

        End Try

        Return fncMessageInfo

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetDeadDateStringDisplaybyOption, Get String format for Dead of Day in Lunar of Sun calender by Setting
    '      VALUE      : 
    '      PARAMS     : CardInfo
    '      PARAMS     : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2016/12/20  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetDeadDateStringDisplaybyOption(ByVal stCard As stBasicCardInfo, ByVal intDisplayType As Integer) As String
        Dim strRet As String
        strRet = ""

        If (stCard.intDecease <> basConst.gcintDIED) Then Return ""

        With stCard
            If (intDisplayType = clsEnum.DeadDateShowType.SUN_CALENDAR) Then
                strRet = basCommon.fncGetDateStatus(.stDeadDaySun, basConst.gcintDIED)
            Else
                strRet = basCommon.fncGetDateStatus(.stDeadDayMoon, basConst.gcintDIED)

                If (.strDeadLunarYearName = "") Then

                    If (.stDeadDayMoon.intYear > 0) Then
                        .strDeadLunarYearName = fncGetLunarYearName(.stDeadDayMoon.intYear)
                    End If

                End If

                If (.strDeadLunarYearName <> "") Then strRet += vbCrLf + "(" + .strDeadLunarYearName + ")"

            End If
        End With

        Return strRet
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncGetDeadYearNameToDisplayInTreeCard, 
    '                   Get Dead year Name for dead day display in Tree Card with format
    '                  (Year Name)
    '      VALUE      : 
    '      PARAMS     : CardInfo
    '      PARAMS     : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2016/12/20  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetDeadYearNameToDisplayInTreeCard(ByVal intYear As Integer) As String

        Dim objVnCal As clsLunarCalendar
        Dim strLunarYearName As String
        Dim strRet As String = ""
        objVnCal = New clsLunarCalendar()

        strLunarYearName = objVnCal.fncGetLunarYearName(intYear)
        If strLunarYearName <> "" Then
            strRet = "(" + strLunarYearName + ")"
            objVnCal = Nothing
        End If

        Return strRet
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xFillCardBase, base function for filling card
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : objCard usrMemberCard, user card
    '      PARAMS     : intX    Integer, X location
    '      PARAMS     : intY    Integer, Y location
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncUpdateCardBase1(ByVal objCard As usrMemberCard1,
                                     ByVal stCard As stCardInfo,
                                     ByVal blnSmall As Boolean) As Boolean
        fncUpdateCardBase1 = False

        Try
            Dim strBirth As String = ""
            Dim strDeath As String = ""

            With stCard
                'fullname
                .stBasicInfo.strFullName = String.Format(basConst.gcstrNameFormat, .stBasicInfo.strLastName, .stBasicInfo.strMidName, .stBasicInfo.strFirstName)
                .stBasicInfo.strFullName = basCommon.fncRemove2Space(.stBasicInfo.strFullName)
                If Not basCommon.fncIsBlank(.stBasicInfo.strAlias) Then .stBasicInfo.strFullName = String.Format("{0}{1}({2})", .stBasicInfo.strFullName, vbCrLf, .stBasicInfo.strAlias)

                'other values for card
                strBirth = basCommon.fncGetDateStatus(.stBasicInfo.stBirthDaySun, basConst.gcintALIVE) 'basCommon.fncGetBirthDieText(.intByea, .intDyea, .intDecease)
                objCard.AliveStatus = .stBasicInfo.intDecease <> basConst.gcintDIED
                If Not objCard.AliveStatus Then

                    '2016/12/20 - Manh Start, Add the display format by Lunar or Sun Calendar
                    strDeath = fncGetDeadDateStringDisplaybyOption(stCard.stBasicInfo, My.Settings.intDeadDateShowType)

                End If

                objCard.CardName = String.Format("{0}" & vbCrLf & "{1}" & vbCrLf & "{2}", .stBasicInfo.strFullName, strBirth, strDeath)
                objCard.CardGender = .stBasicInfo.intGender


                'set image if available and is large card
                If Not blnSmall Then
                    '.strImgLocation = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & .strImgLocation
                    'objCard.CardImage = basCommon.fncCreateThumbnail(.strImgLocation, clsDefine.THUMBNAIL_W, clsDefine.THUMBNAIL_H, .intGender)
                    .stBasicInfo.strImgLocation = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath & .stBasicInfo.strImgLocation
                    objCard.CardImageLocation = .stBasicInfo.strImgLocation
                End If

                'If intX > basConst.gcintNONE_VALUE And intY > basConst.gcintNONE_VALUE Then objCard.Location = New Point(intX, intY)

                'set max with and height of panel for exporting to excel and pdf
                'If intX > mintMaxPanelWith Then mintMaxPanelWith = intX
                'If intY > mintMaxPanelHeight Then mintMaxPanelHeight = intY

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncUpdateCardBase1", ex)
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xFillCardBase, base function for filling card
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : objCard usrMemberCard, user card
    '      PARAMS     : intX    Integer, X location
    '      PARAMS     : intY    Integer, Y location
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncUpdateCardBase1(ByVal objCard As usrMemberCard3,
                                       ByVal stCard As stCardInfo,
                                       ByVal blnSmall As Boolean) As Boolean
        fncUpdateCardBase1 = False

        Try
            With stCard
                'fullname
                .stBasicInfo.strFullName = String.Format(basConst.gcstrNameFormat, .stBasicInfo.strLastName, .stBasicInfo.strMidName, .stBasicInfo.strFirstName)
                .stBasicInfo.strFullName = basCommon.fncRemove2Space(.stBasicInfo.strFullName)
                If Not basCommon.fncIsBlank(.stBasicInfo.strAlias) Then .stBasicInfo.strFullName = String.Format("{0}{1}({2})", .stBasicInfo.strFullName, vbCrLf, .stBasicInfo.strAlias)

                'other values for card
                objCard.CardName = .stBasicInfo.strFullName
                'objCard.CardBirthDie = basCommon.fncGetBirthDieText(.dtBirth, .dtDeath, .intDecease)
                objCard.CardBirth = basCommon.fncGetDateStatus(.stBasicInfo.stBirthDaySun, basConst.gcintALIVE) 'basCommon.fncGetBirthDieText(.intByea, .intDyea, .intDecease)
                objCard.AliveStatus = .stBasicInfo.intDecease <> basConst.gcintDIED
                If Not objCard.AliveStatus Then

                    '2016/12/20 - Manh Start, Add the display format by Lunar or Sun Calendar
                    'objCard.CardDeath = basCommon.fncGetDateStatus(.intDyea, .intDmon, .intDday, basConst.gcintDIED)
                    objCard.CardDeath = fncGetDeadDateStringDisplaybyOption(stCard.stBasicInfo, My.Settings.intDeadDateShowType)

                End If

                objCard.CardGender = .stBasicInfo.intGender
            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncUpdateCardBase1", ex)
        End Try

    End Function

    'Public Function fncUpdateMC1ToHasTable(ByVal intID As Integer) As usrMemberCard1

    '    If gtblMemberCard.Contains(intID) Then Return Nothing

    '    Dim tblUser As DataTable = gobjDB.fncGetMemberMain(intID)

    '    Dim stCard As stCardInfo
    '    If tblUser Is Nothing Then Return Nothing

    '    Dim objCard As usrMemberCard1 = CType(gtblMemberCard.Item(intID), usrMemberCard1)
    '    stCard = fncGetMemberInfo(intID, tblUser)
    '    fncUpdateCardBase1(objCard, stCard, My.Settings.intCardSize <> clsEnum.emCardSize.LARGE)

    '    Return objCard
    'End Function

    'Public Function fncAddMC1ToHasTable(ByVal intID As Integer) As usrMemberCard1


    '    gtblMemberCard.Remove(intID)

    '    Dim tblUser As DataTable = gobjDB.fncGetMemberMain(intID)
    '    Dim objCard As usrMemberCard1
    '    Dim stCard As stCardInfo
    '    If tblUser Is Nothing Then Return Nothing

    '    stCard = fncGetMemberInfo(intID, tblUser)
    '    objCard = fncMakeCardInfoType1(stCard, My.Settings.intCardSize <> clsEnum.emCardSize.LARGE)
    '    gtblMemberCard.Add(stCard.intID, objCard)

    '    Return objCard
    'End Function

    Public Function fncMakeCardInfoType1(ByVal stCard As stCardInfo, ByVal blnSmall As Boolean) As usrMemberCard1

        fncMakeCardInfoType1 = Nothing

        Dim objCard As usrMemberCard1 = New usrMemberCard1(stCard.intID, blnSmall)
        objCard.Location = New Point(stCard.intX, stCard.intY)

        Dim strBirth As String = ""
        Dim strDeath As String = ""

        With stCard
            'objCard.CardName = stCard.strName
            'objCard.CardBirth = basCommon.fncGetDateStatus(.intByea, .intBmon, .intBday, basConst.gcintALIVE)
            strBirth = basCommon.fncGetDateStatus(.stBasicInfo.stBirthDaySun, basConst.gcintALIVE)
            objCard.AliveStatus = .stBasicInfo.intDecease <> basConst.gcintDIED
            'objCard.CardDeath = ""
            If Not objCard.AliveStatus Then

                '2016/12/20 Start Manh Add
                'Display Dead Date by Option
                'objCard.CardDeath = basCommon.fncGetDateStatus(.intDyea, .intDmon, .intDday, basConst.gcintDIED)
                'strDeath = basCommon.fncGetDateStatus(.intDyea, .intDmon, .intDday, basConst.gcintDIED)
                strDeath = fncGetDeadDateStringDisplaybyOption(stCard.stBasicInfo, My.Settings.intDeadDateShowType)
                '2016/12/20 End Manh

            End If
            objCard.CardGender = .stBasicInfo.intGender
            objCard.CardName = String.Format("{0}" & vbCrLf & "{1}" & vbCrLf & "{2}", .stBasicInfo.strFullName, strBirth, strDeath)

            'set image if available and is large card

            '.strImgLocation = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & .strImgLocation
            'objCard.CardImage = basCommon.fncCreateThumbnail(.strImgLocation, clsDefine.THUMBNAIL_W, clsDefine.THUMBNAIL_H, .intGender)
            .stBasicInfo.strImgLocation = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath & .stBasicInfo.strImgLocation
            objCard.CardImageLocation = .stBasicInfo.strImgLocation

            objCard.CardID = stCard.intID
            objCard.Name = CStr(stCard.intID)

        End With

        Return objCard
    End Function

    Public Function fncMakeCardInfoType3(ByVal stCard As stCardInfo, ByVal blnSmall As Boolean) As usrMemberCard3

        fncMakeCardInfoType3 = Nothing

        Dim objCard As usrMemberCard3 = New usrMemberCard3(stCard.intID, blnSmall)
        objCard.Location = New Point(stCard.intX, stCard.intY)

        With stCard
            objCard.CardName = stCard.stBasicInfo.strFullName
            objCard.CardBirth = basCommon.fncGetDateStatus(.stBasicInfo.stBirthDaySun, basConst.gcintALIVE)
            objCard.AliveStatus = .stBasicInfo.intDecease <> basConst.gcintDIED
            objCard.CardDeath = ""
            If Not objCard.AliveStatus Then

                '2016/12/20 Start Manh Add
                'Display Dead Date by Option
                'objCard.CardDeath = basCommon.fncGetDateStatus(.intDyea, .intDmon, .intDday, basConst.gcintDIED)
                objCard.CardDeath = fncGetDeadDateStringDisplaybyOption(stCard.stBasicInfo, My.Settings.intDeadDateShowType)
                '2016/12/20 End Manh

            End If
            objCard.CardGender = .stBasicInfo.intGender
            objCard.CardID = stCard.intID
            objCard.Name = CStr(stCard.intID)
        End With

        Return objCard
    End Function


    Public Function fncMakeMemberIDList(ByVal drMember As DataRow(), ByVal strFieldID As String) As List(Of Integer)

        If drMember Is Nothing Then Return Nothing
        If drMember.Length <= 0 Then Return Nothing

        Dim i As Integer
        Dim intID As Integer
        Dim lstData As List(Of Integer)

        lstData = New List(Of Integer)

        For i = 0 To drMember.Length - 1

            Integer.TryParse(fncCnvNullToString(drMember(i).Item(strFieldID)), intID)
            lstData.Add(intID)

        Next

        Return lstData

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncGetMemberInfo, Get member info to cardinfo
    '      VALUE      : Card Info
    '      PARAMS     : intID    Integer, member ID
    '      PARAMS     : tblUser  DataTable
    '      MEMO       : 
    '      CREATE     : 2012/11/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetMemberInfo(ByVal intID As Integer, ByVal tblUser As DataTable) As stCardInfo

        fncGetMemberInfo = Nothing

        If tblUser Is Nothing Then Return Nothing

        Dim stCard As New stCardInfo
        Dim vwUser As DataView = New DataView(tblUser)

        vwUser.RowFilter = String.Format("MEMBER_ID = {0}", intID)
        If vwUser.Count = 0 Then Return Nothing

        With stCard
            .intID = intID
            .stBasicInfo.strLastName = basCommon.fncCnvNullToString(vwUser(0)("LAST_NAME"))
            .stBasicInfo.strFirstName = basCommon.fncCnvNullToString(vwUser(0)("FIRST_NAME"))
            .stBasicInfo.strMidName = basCommon.fncCnvNullToString(vwUser(0)("MIDDLE_NAME"))
            .stBasicInfo.strAlias = basCommon.fncCnvNullToString(vwUser(0)("ALIAS_NAME"))
            .stBasicInfo.stBirthDaySun.intDay = basCommon.fncCnvToInt(vwUser(0)("BIR_DAY"))
            .stBasicInfo.stBirthDaySun.intMonth = basCommon.fncCnvToInt(vwUser(0)("BIR_MON"))
            .stBasicInfo.stBirthDaySun.intYear = basCommon.fncCnvToInt(vwUser(0)("BIR_YEA"))
            .stBasicInfo.intGender = basCommon.fncCnvToInt(vwUser(0)("GENDER"))

            '2016/12/20 - Manh Start 
            'Dead Day in Lunar Calendar
            .stBasicInfo.stDeadDayMoon.intDay = basCommon.fncCnvToInt(vwUser(0)("DEA_DAY"))
            .stBasicInfo.stDeadDayMoon.intMonth = basCommon.fncCnvToInt(vwUser(0)("DEA_MON"))
            .stBasicInfo.stDeadDayMoon.intYear = basCommon.fncCnvToInt(vwUser(0)("DEA_YEA"))

            'Dead Day in Sun Calendar
            .stBasicInfo.stDeadDaySun.intDay = basCommon.fncCnvToInt(vwUser(0)("DEA_DAY_SUN"))
            .stBasicInfo.stDeadDaySun.intMonth = basCommon.fncCnvToInt(vwUser(0)("DEA_MON_SUN"))
            .stBasicInfo.stDeadDaySun.intYear = basCommon.fncCnvToInt(vwUser(0)("DEA_YEA_SUN"))

            Dim objVnCal As clsLunarCalendar
            objVnCal = New clsLunarCalendar()
            .stBasicInfo.strDeadLunarYearName = objVnCal.fncGetLunarYearName(.stBasicInfo.stDeadDayMoon.intYear)
            objVnCal = Nothing
            '2016/12/20 - Manh End

            .stBasicInfo.intDecease = basCommon.fncCnvToInt(vwUser(0)("DECEASED"))
            .stBasicInfo.strFullName = (.stBasicInfo.strLastName & " " & .stBasicInfo.strMidName & " " & .stBasicInfo.strFirstName).Replace("  ", " ").Trim()
            If Not basCommon.fncIsBlank(.stBasicInfo.strAlias) Then
                .stBasicInfo.strFullName = String.Format("{0}{1}({2})", .stBasicInfo.strFullName, vbCrLf, .stBasicInfo.strAlias)
            End If
            .stBasicInfo.strImgLocation = fncCnvNullToString(vwUser(0)("AVATAR_PATH"))
            .lstChild = Nothing
            .lstSibling = Nothing
            .lstSpouse = Nothing
            .lstStepChild = Nothing

            .intLevel = basCommon.fncCnvToInt(vwUser(0)("LEVEL"))
        End With

        Return stCard
    End Function

    Public Function fncGetKidListToDataTable(ByVal intParent As Integer, ByVal tblRelChild As DataTable) As DataTable

        Try

            Dim vwRel As New DataView(tblRelChild)

            vwRel.RowFilter = String.Format("REL_FMEMBER_ID = {0}", intParent)

            If vwRel.Count <= 0 Then Return Nothing

            Return vwRel.ToTable

        Catch ex As Exception
            Throw ex
        End Try

        Return Nothing
    End Function

    Public Function fncGetRowsFromDataTable(ByVal tblData As DataTable,
                                            Optional ByVal strSelect As String = "",
                                            Optional ByVal strOrder As String = "") As DataRow()

        If tblData Is Nothing Then Return Nothing
        If tblData.Rows.Count = 0 Then Return Nothing

        Dim drRows As DataRow() = tblData.Select(strSelect, strOrder)

        If drRows Is Nothing Then Return Nothing
        If drRows.Length <= 0 Then Return Nothing

        Return drRows
    End Function


    Private Function fncGetSpouseListToDataRow(ByVal intID As Integer, ByVal tblRelMarriage As DataTable) As DataRow()

        Try

            Dim drRows As DataRow() = tblRelMarriage.Select(String.Format("MEMBER_ID = {0} AND RELID = {1}", intID, CInt(clsEnum.emRelation.MARRIAGE)), "ROLE_ORDER ASC")

            If drRows Is Nothing Then Return Nothing
            If drRows.Length <= 0 Then Return Nothing

            Return drRows
        Catch ex As Exception
            Throw ex
        End Try

        Return Nothing
    End Function

    Public Function fncGetKidListToDataTable(ByVal intParent As Integer, ByVal intParent2 As Integer,
                                             ByVal tblRelChild As DataTable) As DataTable

        Try

            Dim vwRel As New DataView(tblRelChild)

            vwRel.RowFilter = String.Format("REL_FMEMBER_ID = {0} OR REL_FMEMBER_ID = {1}", intParent, intParent2)

            If vwRel.Count <= 0 Then Return Nothing

            Return vwRel.ToTable

        Catch ex As Exception
            Throw ex
        End Try

        Return Nothing
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncMessageWarning, show warning message dialog
    '　　　VALUE      : DialogResult
    '      PARAMS1    : strMsg string, message to show
    '      PARAMS2    : strTitle string, title of messagebox
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMessageWarning(ByVal strMsg As String, Optional ByVal strTitle As String = Nothing) As DialogResult

        fncMessageWarning = DialogResult.OK

        Try
            Dim strMsgTitle As String = ""

            'set default title
            strMsgTitle = basConst.gcstrProductName

            'set title if parameter is avaiable
            If Not String.IsNullOrEmpty(strTitle) Then strMsgTitle = strTitle

            fncMessageWarning = MessageBox.Show(strMsg, strMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning)

        Catch ex As Exception

            Throw ex

        End Try

        Return fncMessageWarning

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMessageWarning, show warning message dialog
    '　　　VALUE      : DialogResult
    '      PARAMS1    : strMsg string, message to show
    '      PARAMS2    : objControlToFocus Control, control to set focus
    '      PARAMS3    : strTitle string, title of messagebox
    '      MEMO       : 
    '      CREATE     : 2011/07/18  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMessageWarning(ByVal strMsg As String,
                                        ByVal objControlToFocus As Control,
                                        Optional ByVal strTitle As String = Nothing) As DialogResult

        fncMessageWarning = DialogResult.OK

        Try

            'focus to control if avaiable
            If objControlToFocus IsNot Nothing Then objControlToFocus.Focus()
            fncMessageWarning = fncMessageWarning(strMsg, strTitle)

        Catch ex As Exception

            Throw ex

        End Try

        Return fncMessageWarning

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMessageError, show error message dialog
    '　　　VALUE      : DialogResult
    '      PARAMS1    : strMsg string, message to show
    '      PARAMS2    : strTitle string, title of messagebox
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMessageError(ByVal strMsg As String, Optional ByVal strTitle As String = Nothing) As DialogResult

        fncMessageError = DialogResult.OK

        Try
            Dim strMsgTitle As String = ""

            'set default title
            strMsgTitle = basConst.gcstrProductName

            'set title if parameter is avaiable
            If Not String.IsNullOrEmpty(strTitle) Then strMsgTitle = strTitle

            fncMessageError = MessageBox.Show(strMsg, strMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception

            Throw ex

        End Try

        Return fncMessageError

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMessageError, show error message dialog
    '　　　VALUE      : DialogResult
    '      PARAMS1    : strMsg string, message to show
    '      PARAMS2    : objControlToFocus Control, control to set focus
    '      PARAMS3    : strTitle string, title of messagebox
    '      MEMO       : 
    '      CREATE     : 2011/07/18  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMessageError(ByVal strMsg As String,
                                        ByVal objControlToFocus As Control,
                                        Optional ByVal strTitle As String = Nothing) As DialogResult

        fncMessageError = DialogResult.OK

        Try
            'set focus to control if avaiable
            If objControlToFocus IsNot Nothing Then objControlToFocus.Focus()
            fncMessageError = fncMessageError(strMsg, strTitle)

        Catch ex As Exception

            Throw ex

        End Try

        Return fncMessageError

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMessageConfirm, show ask to confirm message dialog
    '　　　VALUE      : boolean, true - yes, false - no
    '      PARAMS1    : strMsg string, message to show
    '      PARAMS2    : strTitle string, title of messagebox
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMessageConfirm(ByVal strMsg As String, Optional ByVal strTitle As String = Nothing) As Boolean

        fncMessageConfirm = False

        Try
            Dim strMsgTitle As String = ""

            'set default title
            strMsgTitle = basConst.gcstrProductName

            'set title if avaiable
            If Not String.IsNullOrEmpty(strTitle) Then strMsgTitle = strTitle

            If MessageBox.Show(strMsg, strMsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then

                fncMessageConfirm = True

            End If

        Catch ex As Exception

            Throw ex

        End Try

        Return fncMessageConfirm

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMessageConfirm, show ask to confirm message dialog
    '　　　VALUE      : boolean, true - yes, false - no
    '      PARAMS1    : strMsg string, message to show
    '      PARAMS2    : objControlToFocus Control, control to set focus
    '      PARAMS3    : strTitle string, title of messagebox
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMessageConfirm(ByVal strMsg As String,
                                        ByVal objControlToFocus As Control,
                                        Optional ByVal strTitle As String = Nothing) As Boolean

        fncMessageConfirm = False

        Try
            'set focus to control if avaiable
            If objControlToFocus IsNot Nothing Then objControlToFocus.Focus()
            fncMessageConfirm = fncMessageConfirm(strMsg, strTitle)

        Catch ex As Exception
            Throw ex
        End Try

        Return fncMessageConfirm

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncIsBlank, check a blank textbox
    '　　　VALUE      : boolean, true - is blank, false - not blank
    '      PARAMS1    : strText string, string to check
    '      PARAMS2    : strMsg string, message to show
    '      PARAMS3    : objCtrl string, control to focus
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncIsBlank(ByVal strText As String,
                                Optional ByVal strMsg As String = Nothing,
                                Optional ByVal objCtrl As Control = Nothing) As Boolean

        Dim blnResult As Boolean = False

        Try
            If IsDBNull(strText) Then Return True

            If String.IsNullOrEmpty(strText) Then

                'show message and set focus if available
                If strMsg IsNot Nothing Then fncMessageWarning(strMsg, objCtrl)

                blnResult = True

            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return blnResult

    End Function


    '   ******************************************************************
    '      FUNCTION   : fncEncyptPass
    '      VALUE      : string, encrypted password in SHA1
    '      PARAMS     : strPass as string, password
    '      MEMO       : 
    '      CREATE     : 2011/07/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncEncyptPass(ByVal strPass As String) As String

        Dim strResult As String = ""
        Dim bytesToHash() As Byte

        Try
            Dim objSHA1 As System.Security.Cryptography.SHA1CryptoServiceProvider

            objSHA1 = New System.Security.Cryptography.SHA1CryptoServiceProvider
            bytesToHash = System.Text.Encoding.ASCII.GetBytes(strPass)

            bytesToHash = objSHA1.ComputeHash(bytesToHash)

            For Each b As Byte In bytesToHash

                strResult += b.ToString(basConst.gcstrEncryptFormat)

            Next

        Catch ex As Exception

            Throw ex

        End Try

        Erase bytesToHash

        Return strResult

    End Function


    '   ******************************************************************
    '      FUNCTION   : 例外エラーメッセージ
    '      VALUE      : 無し

    '      PARAMS     : 引数1  String   , 発生元インスタンス名

    '                   引数2  String   , 発生元メソッド名

    '                   引数3  Exception, 例外オブジェクト

    '                   引数4  String   , 備考

    '      MEMO       : 
    '      CREATE     : 2011/07/18  AKB     Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub fncSaveErr(ByVal strInstanceName As String,
                          ByVal strMethodName As String,
                          ByVal objException As System.Exception,
                          Optional ByVal strRemark As String = Nothing,
                          Optional ByVal blnShowMessage As Boolean = True)
        Try

            Dim strErrMsg As String         '警告内容
            Dim strLogMsg As String         'ログ内容
            Dim strAppFullName As String    'exeフルパス
            Dim strLogFileName As String    'ログファイルのパス

            '----------------
            ' エラーメッセージ表示

            'メッセージを作成
            strErrMsg = objException.Message

            If Not String.IsNullOrEmpty(strRemark) Then

                strErrMsg += vbCrLf + strRemark

            End If
            strErrMsg += vbCrLf + "[" + strInstanceName + "]" + vbCrLf + "[" + strMethodName + "]"

            '表示
            'If blnShowMessage Then MessageBox.Show(strErrMsg, basConst.gcstrProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)

            '----------------
            ' エラーログ生成

            'メッセージを作成
            strLogMsg = ""
            strLogMsg += "------------------------------------------------------------------------------------------------------------------------------------------------"
            strLogMsg += vbCrLf + "ログ出力日時：" + Date.Now.ToString("yyyy/MM/dd HH:mm:ss")
            strLogMsg += vbCrLf + "エラー発生モジュール：" + strInstanceName
            strLogMsg += vbCrLf + "エラー発生プロシージャ：" + strMethodName
            strLogMsg += vbCrLf + "エラー内容：" + objException.Message

            If Not IsNothing(strRemark) AndAlso strRemark.Length > 0 Then

                strLogMsg += vbCrLf + strRemark

            End If

            strLogMsg += vbCrLf
            strLogMsg += vbCrLf

            'exeのフルパスを取得

            strAppFullName = System.Reflection.Assembly.GetExecutingAssembly().Location

            'ログファイルのパスを取得

            strLogFileName = ""
            strLogFileName += System.IO.Path.GetDirectoryName(strAppFullName)
            strLogFileName += "\" + System.IO.Path.GetFileNameWithoutExtension(strAppFullName) + ".log"

            'ファイルの末尾にログを書き加える。ファイルがなければ、作成される

            Try
                System.IO.File.AppendAllText(strLogFileName, strLogMsg, System.Text.Encoding.GetEncoding(932))
            Catch ex As Exception

            End Try

        Catch ex As Exception

            MessageBox.Show("Err!" + vbCrLf + ex.Message + vbCrLf + " At " + mcstrClsName & " fncSaveErr")

        End Try
    End Sub


    '   ******************************************************************
    '      FUNCTION   : fncSpecialCharacter, to check character is not A-Z a-z 0-9
    '      VALUE      : boolean, true - match, false - not match
    '      PARAMS1    : strText string, string to check
    '      PARAMS2    : strMsg string, message to show
    '      PARAMS3    : objCtrl string, control to focus
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncHasSpecialCharacter(ByVal strText As String,
                                            Optional ByVal strMsg As String = Nothing,
                                            Optional ByVal objCtrl As Control = Nothing) As Boolean

        fncHasSpecialCharacter = True

        Try

            If Regex.IsMatch(strText, basConst.gcstrAlphabetFormat) Then

                If Not String.IsNullOrEmpty(strMsg) Then fncMessageWarning(strMsg, objCtrl)

                Exit Function

            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return False

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncSendTAB, send TAB key
    '      PARAMS     : e   KeyPressEventArgs,
    '      MEMO       : 
    '      CREATE     : 2011/07/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub fncSendTAB(ByVal e As System.Windows.Forms.KeyPressEventArgs)

        Try

            If e.KeyChar = Convert.ToChar(Keys.Enter) Then SendKeys.Send("{TAB}")

        Catch ex As Exception

            Throw ex

        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncCnvNullToString, convert Null to String
    '      VALUE      : String
    '      PARAMS     : vobjValue   Object
    '      MEMO       : 
    '      CREATE     : 2011/07/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCnvNullToString(ByVal vobjValue As Object) As String

        fncCnvNullToString = ""

        Dim strRet As String

        Try

            strRet = ""

            If Not IsDBNull(vobjValue) Then

                If Not IsNothing(vobjValue) Then

                    strRet = CStr(vobjValue)

                End If

            End If

            Return strRet

        Catch ex As Exception

            Throw ex

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncIsValidImage, check validation of an image
    '      VALUE      : Boolean, true - valid, false - invalid
    '      PARAMS     : vobjValue   Object
    '      MEMO       : 
    '      CREATE     : 2011/07/28  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncIsValidImage(ByVal strPath As String) As Boolean

        fncIsValidImage = False

        Try

            Dim dirInfo As FileInfo
            Dim strFile As String

            'get directory info
            dirInfo = New FileInfo(strPath)

            'check the file is existing
            If Not dirInfo.Exists Then
                fncMessageWarning(mcstrImageNotExist)
                Exit Function
            End If

            'get file's extension and check the format
            strFile = dirInfo.Extension()
            If Not (String.Compare(strFile, gcstrFileBMP, True) = 0 Or
                    String.Compare(strFile, gcstrFileGIF, True) = 0 Or
                    String.Compare(strFile, gcstrFileJPG, True) = 0 Or
                    String.Compare(strFile, gcstrFilePNG, True) = 0) Then

                fncMessageWarning(mcstrImageWrong)
                Exit Function

            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncCopyFile, copy a file 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : strPath String, source path
    '      PARAMS2    : strDestFolder String, destination folder
    '      PARAMS3    : strFileName String, file name to save
    '      PARAMS4    : strReturnFileName String, file name saved
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncCopyFile(ByVal strSource As String,
                                ByVal strDestFolder As String,
                                ByVal strFileName As String,
                                ByRef strReturnFileName As String) As Boolean

        fncCopyFile = False

        Try

            Dim dirInfo As DirectoryInfo
            Dim strFileExtension As String
            Dim strCopyPath As String

            dirInfo = New DirectoryInfo(strSource)

            'check the file is existing
            If dirInfo.Exists Then

                fncMessageWarning(mcstrImageNotExist)
                Exit Function

            End If

            'check copy path, if it's not existing, create ones
            strCopyPath = My.Application.Info.DirectoryPath & strDestFolder
            If Not Directory.Exists(strCopyPath) Then Directory.CreateDirectory(strCopyPath)

            'get extension of the file
            strFileExtension = dirInfo.Extension

            'build copy path
            strCopyPath &= strFileName & strFileExtension

            'if source and destination are different then copy file
            If String.Compare(strSource, strCopyPath) <> 0 Then System.IO.File.Copy(strSource, strCopyPath, True)

            'copy successed, return file name
            strReturnFileName = strFileName & strFileExtension

            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncCopyFile, copy a file 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : strPath String, source path
    '      PARAMS2    : strDestFolder String, destination folder
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncCopyFile(ByVal strSrcFile As String,
                                ByRef strDesFolder As String,
                                Optional ByVal blnOverwrite As Boolean = False) As Boolean

        fncCopyFile = False

        Dim objFileInfo As FileInfo

        Try
            If fncIsBlank(strSrcFile) Then Exit Function

            objFileInfo = New FileInfo(strSrcFile)

            'check the file is existing
            If Not objFileInfo.Exists Then

                fncMessageWarning(mcstrFileNotExist)
                Exit Function

            End If

            If Not Directory.Exists(strDesFolder) Then Directory.CreateDirectory(strDesFolder)

            'build copy path
            strDesFolder = strDesFolder & objFileInfo.Name

            If System.IO.File.Exists(strDesFolder) Then

                If Not fncMessageConfirm(mcstrFileExist) Then Exit Function

                blnOverwrite = True

            End If

            'if source and destination are different then copy file
            If String.Compare(strSrcFile, strDesFolder) = 0 Then Exit Function

            Try
                System.IO.File.Copy(strSrcFile, strDesFolder, blnOverwrite)
            Catch ex As Exception
                Exit Function
            End Try


            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function



    '   ****************************************************************** 
    '      FUNCTION   : fncSaveFile, save file dialog
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : strReturnPath String, return path
    '      MEMO       :  
    '      CREATE     : 2011/08/12  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncSaveFileDlg(ByRef strReturnPath As String, ByVal strFilter As String, ByVal strDefaultExt As String) As Boolean

        fncSaveFileDlg = False

        Try
            Dim dlgSave As SaveFileDialog

            dlgSave = New SaveFileDialog()

            'default file extension
            dlgSave.AddExtension = True
            'dlgSave.Filter = basConst.gcstrExcelFilter
            'dlgSave.DefaultExt = basConst.gcstrExcelExt
            dlgSave.Filter = strFilter
            dlgSave.DefaultExt = strDefaultExt

            'open save file dialog then return the path
            If dlgSave.ShowDialog = DialogResult.OK Then

                strReturnPath = dlgSave.FileName
                Return True

            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncOpenFile, open file dialog
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : strReturnPath String, return path
    '      MEMO       :  
    '      CREATE     : 2011/08/22  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncOpenFileDlg(ByRef strReturnPath As String, Optional ByVal strFilter As String = "") As Boolean

        fncOpenFileDlg = False

        Dim dlgOpen As OpenFileDialog = Nothing

        Try

            dlgOpen = New OpenFileDialog()

            If Not fncIsBlank(strFilter) Then dlgOpen.Filter = strFilter

            'open file dialog then return the path
            If dlgOpen.ShowDialog = DialogResult.OK Then

                strReturnPath = dlgOpen.FileName
                Return True

            End If

        Catch ex As Exception
            Throw ex
        Finally
            If dlgOpen IsNot Nothing Then dlgOpen.Dispose()
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncDeleteFile, open file dialog
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : strFile String, file path to delete
    '      MEMO       :  
    '      CREATE     : 2011/08/22  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncDeleteFile(ByVal strFile As String) As Boolean

        fncDeleteFile = False

        Try

            If System.IO.File.Exists(strFile) Then
                File.SetAttributes(strFile, FileAttributes.Normal)
                File.Delete(strFile)
            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncCreateExcel, create folder and return path
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : dgvData DataGridView, data
    '      PARAMS2    : strPath String, file name to save
    '      MEMO       :  
    '      CREATE     : 2011/08/12  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    'Public Function fncExportExcel(ByVal dgvData As DataGridView, ByVal strPath As String) As Boolean

    '    fncExportExcel = False

    '    Dim xlsApp As Excel.Application = Nothing           'excel application object
    '    Dim xlsBook As Excel.Workbook = Nothing             'excel file
    '    Dim xlsSheet As Excel.Worksheet = Nothing           'sheet

    '    Try
    '        Dim objFile As FileInfo

    '        Dim xlsRange As Excel.Range                     'to format cell
    '        Dim objMisValue As Object                       'missing values object

    '        Dim strDir As String                            'directory path to save file

    '        objFile = New FileInfo(strPath)
    '        strDir = objFile.DirectoryName

    '        'create EXCEL object
    '        objMisValue = System.Reflection.Missing.Value
    '        xlsApp = New Excel.Application
    '        xlsBook = xlsApp.Workbooks.Add(objMisValue)
    '        xlsSheet = CType(xlsBook.Worksheets(basConst.gcintSheetNo), Excel.Worksheet)


    '        'create directory if it doesn't exist
    '        If Not Directory.Exists(strDir) Then Directory.CreateDirectory(strDir)

    '        'fill header text
    '        For i As Integer = 0 To dgvData.ColumnCount - 1

    '            'set text
    '            xlsSheet.Cells(1, i + 1) = dgvData.Columns(i).HeaderText

    '            'set font to bold and fill border
    '            xlsRange = CType(xlsSheet.Cells(1, i + 1), Excel.Range)
    '            xlsRange.Font.Bold = True
    '            xlsRange.BorderAround()

    '        Next

    '        'start fill data to excel
    '        For i As Integer = 0 To dgvData.Rows.Count - 1

    '            For j As Integer = 0 To dgvData.Columns.Count - 1

    '                'set text
    '                xlsSheet.Cells(i + 2, j + 1) = fncCnvNullToString(dgvData(j, i).Value)

    '                'set auto fit width and set boder
    '                xlsRange = CType(xlsSheet.Cells(i + 2, j + 1), Excel.Range)
    '                xlsRange.EntireColumn.AutoFit()
    '                xlsRange.BorderAround()

    '                'if member died, fill cell's color to gray and text is white
    '                If dgvData.Rows(i).DefaultCellStyle.BackColor = Color.Gray Then

    '                    xlsRange.Interior.ColorIndex = basConst.gcintXlsSheetGray
    '                    xlsRange.Font.ColorIndex = basConst.gcintXlsFontWhite

    '                End If

    '            Next


    '        Next

    '        'set the orientation is landscape
    '        xlsSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape

    '        'align vertical to top
    '        xlsRange = xlsSheet.Cells()
    '        xlsRange.VerticalAlignment = Excel.Constants.xlTop

    '        'align center the No. column
    '        xlsRange = CType(xlsSheet.Columns(1), Excel.Range)
    '        xlsRange.HorizontalAlignment = Excel.Constants.xlCenter

    '        'save and close
    '        xlsBook.SaveAs(strPath)
    '        xlsBook.Close()
    '        xlsApp.Quit()

    '        Return True

    '    Catch ex As Exception

    '        Throw ex

    '    Finally

    '        fncReleaseObject(xlsApp)
    '        fncReleaseObject(xlsBook)
    '        fncReleaseObject(xlsSheet)

    '    End Try

    'End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncReleaseObject, clear object
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : obj Object, object to clear
    '      MEMO       :  
    '      CREATE     : 2011/08/12  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncReleaseObject(ByVal obj As Object) As Boolean

        fncReleaseObject = False

        Try

            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing

            Return True

        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncRemoveSpace, remove double space
    '      VALUE      : String
    '      PARAMS     : strInput String, input string
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncRemove2Space(ByVal strInput As String) As String

        fncRemove2Space = strInput

        Try

            Return fncRemove2Space.Replace("  ", " ")

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncShowLunarDate, show lunar date
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : frmLunar frmCalendarVN, CalendarVn form
    '      PARAMS2    : objControl  DateTimePicker
    '      PARAMS3    : objLabel    Label
    '      PARAMS4    : blnShowForm Boolean, true to show form
    '      MEMO       :  
    '      CREATE     : 2011/08/05  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncShowLunarDate(ByVal frmLunar As frmCalendarVN,
                                    ByVal objControl As DateTimePicker,
                                    ByRef objLabel As Label,
                                    ByVal blnShowForm As Boolean) As Boolean

        fncShowLunarDate = False

        Try
            frmLunar.EnCalPicker = objControl


            'set date label's text
            objLabel.Text = frmLunar.LunarString

            'if not checked, clear text then exit
            If Not objControl.Checked Then objLabel.Text = ""

            'show form or not?
            If Not blnShowForm Then Return True


            If frmLunar.fncShowForm() Then
                'set date
                objControl.Value = frmLunar.SolarDate

            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCreateThumbnail, get thumbnail of an image
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : strImgLocation  String, image location
    '      PARAMS     : intWidth  Integer, 
    '      PARAMS     : intHeight  Integer, 
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCreateThumbnail(ByVal strImgLocation As String, ByVal intWidth As Integer, ByVal intHeight As Integer, ByVal intGender As Integer) As Image

        fncCreateThumbnail = Nothing

        Dim img As Image = Nothing
        Dim imgThumbNail As Image = Nothing

        Try

            If Not File.Exists(strImgLocation) Or fncIsBlank(strImgLocation) Then
                If intGender = clsEnum.emGender.MALE Then
                    img = GiaPha.My.Resources.no_avatar_m
                ElseIf intGender = clsEnum.emGender.FEMALE Then
                    img = GiaPha.My.Resources.no_avatar_f
                ElseIf intGender = clsEnum.emGender.UNKNOW Then
                    img = GiaPha.My.Resources.UnknownMember
                End If
            Else

                img = Image.FromFile(strImgLocation)

            End If

            imgThumbNail = img.GetThumbnailImage(intWidth, intHeight, Nothing, System.IntPtr.Zero)

            Return imgThumbNail

        Catch ex As Exception
            Throw ex
        Finally
            If img IsNot Nothing Then img.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCreateThumbnail, get thumbnail of an image
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : strImgLocation  String, image location
    '      PARAMS     : intWidth  Integer, 
    '      PARAMS     : intHeight  Integer, 
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCreateThumbnail(ByVal bmpImage As Bitmap, ByVal intWidth As Integer, ByVal intHeight As Integer) As Image

        fncCreateThumbnail = Nothing

        Dim imgThumbNail As Image = Nothing

        Try
            If bmpImage Is Nothing Then Exit Function

            imgThumbNail = bmpImage

            imgThumbNail = imgThumbNail.GetThumbnailImage(intWidth, intHeight, Nothing, System.IntPtr.Zero)

            Return imgThumbNail

        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCreateThumbnail, get thumbnail of an image
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : strImgLocation  String, image location
    '      PARAMS     : strFolder  String, image folder
    '      PARAMS     : strFileName  String, image name
    '      PARAMS     : intWidth  Integer, 
    '      PARAMS     : intHeight  Integer, 
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCreateThumbnailAndSave(ByVal strImageLocation As String,
                                              ByVal strFolder As String,
                                              ByVal strFileName As String,
                                              ByVal intWidth As Integer,
                                              ByVal intHeight As Integer) As Boolean

        fncCreateThumbnailAndSave = False

        Dim imgThumbNail As Image = Nothing

        Try
            Dim strSavePath As String

            'check the file is existing
            If Not System.IO.File.Exists(strImageLocation) Then

                fncMessageWarning(mcstrImageNotExist)
                Exit Function

            End If

            'check copy path, if it's not existing, create ones
            strSavePath = My.Application.Info.DirectoryPath & strFolder
            If Not Directory.Exists(strSavePath) Then Directory.CreateDirectory(strSavePath)
            strSavePath = strSavePath & strFileName & gcstrFileJPG

            'if source and destination are the same, exit
            If String.Compare(strImageLocation, strSavePath) = 0 Then Exit Function

            imgThumbNail = Image.FromFile(strImageLocation).GetThumbnailImage(intWidth, intHeight, Nothing, System.IntPtr.Zero)

            imgThumbNail.Save(strSavePath)

            'return file name
            'strFileName = strFileName & mcstrFileJPG

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If imgThumbNail IsNot Nothing Then imgThumbNail.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCreateThumbnail, get thumbnail of an image
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : imgAvatar  Image, image 
    '      PARAMS     : strFolder  String, image folder
    '      PARAMS     : strFileName  String, image name
    '      PARAMS     : intWidth  Integer, 
    '      PARAMS     : intHeight  Integer, 
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCreateThumbnailAndSave(ByVal imgAvatar As Image,
                                              ByVal strFolder As String,
                                              ByVal strFileName As String,
                                              ByVal intWidth As Integer,
                                              ByVal intHeight As Integer) As Boolean

        fncCreateThumbnailAndSave = False

        Dim imgThumbNail As Image = Nothing

        Try
            Dim strSavePath As String

            'check the file is existing
            If imgAvatar Is Nothing Then

                fncMessageWarning(mcstrImageNotExist)
                Exit Function

            End If

            'check copy path, if it's not existing, create ones
            strSavePath = My.Application.Info.DirectoryPath & strFolder
            If Not Directory.Exists(strSavePath) Then Directory.CreateDirectory(strSavePath)
            strSavePath = strSavePath & strFileName & gcstrFileJPG

            ''if source and destination are the same, exit
            'If String.Compare(strImageLocation, strSavePath) = 0 Then Exit Function

            'imgThumbNail = imgAvatar.GetThumbnailImage(intWidth, intHeight, Nothing, System.IntPtr.Zero)
            imgThumbNail = New Bitmap(imgAvatar, intWidth, intHeight)
            imgThumbNail.Save(strSavePath)

            'return file name
            'strFileName = strFileName & mcstrFileJPG

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If imgThumbNail IsNot Nothing Then imgThumbNail.Dispose()
        End Try

    End Function


    Public Function fncPdfMetric(ByVal intValue As Integer) As Integer

        Dim intDPI As Integer = CInt(intValue * 0.75)
        Return intDPI

    End Function

    'Public Sub fncDrawPdfConnector(ByVal gfx As PdfSharp.Drawing.XGraphics, _
    '                               ByVal lstLine As List(Of usrLine), _
    '                               ByVal objPen As PdfSharp.Drawing.XPen, _
    '                               Optional ByVal intStartX As Integer = 0, _
    '                               Optional ByVal intStartY As Integer = 0)

    '    Dim i As Integer
    '    Dim ptStart As Point
    '    Dim ptEnd As Point

    '    'draw special line
    '    For i = 0 To lstLine.Count - 1

    '        ptStart = lstLine(i).Location
    '        ptStart.X += intStartX
    '        ptStart.Y += intStartY

    '        ptEnd = ptStart

    '        If lstLine(i).LineDirection = clsEnum.emLineDirection.HORIZONTAL Then
    '            ptEnd.X += lstLine(i).Width
    '        Else
    '            ptEnd.Y += lstLine(i).Height
    '        End If

    '        gfx.DrawLine(objPen, fncPdfMetric(ptStart.X), fncPdfMetric(ptStart.Y), fncPdfMetric(ptEnd.X), fncPdfMetric(ptEnd.Y))

    '    Next

    'End Sub
    Public Sub fncDrawPdfConnector(ByVal gfx As PdfSharp.Drawing.XGraphics,
                                 ByVal lstLine As List(Of usrLine),
                                 ByVal objPen As PdfSharp.Drawing.XPen,
                                 Optional ByVal intStartX As Integer = 0,
                                 Optional ByVal intStartY As Integer = 0)

        Dim i As Integer
        Dim ptStart As clsCoordinate
        Dim ptEnd As clsCoordinate
        Dim intX, intY As Integer

        'draw special line
        For i = 0 To lstLine.Count - 1

            ptStart = lstLine(i).LineCoor
            intX = ptStart.X + intStartX
            intY = ptStart.Y + intStartY
            ptStart = New clsCoordinate(intX, intY)

            ptEnd = New clsCoordinate(intX, intY)

            If lstLine(i).LineDirection = clsEnum.emLineDirection.HORIZONTAL Then
                ptEnd.X += lstLine(i).Width
            Else
                ptEnd.Y += lstLine(i).Height
            End If

            gfx.DrawLine(objPen, fncPdfMetric(ptStart.X), fncPdfMetric(ptStart.Y), fncPdfMetric(ptEnd.X), fncPdfMetric(ptEnd.Y))

        Next

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncCreateThumbnail, get thumbnail of an image
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : imgAvatar  Image, image 
    '      PARAMS     : strFolder  String, image folder
    '      PARAMS     : strFileName  String, image name
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncSaveImage(ByVal imgAvatar As Image,
                                 ByVal strFolder As String,
                                 ByRef strFileName As String,
                                 Optional ByRef strReturnPath As String = "") As Boolean

        fncSaveImage = False
        Dim imgThumbNail As Bitmap = Nothing

        Try
            Dim strSavePath As String

            'check the file is existing
            If imgAvatar Is Nothing Then

                fncMessageWarning(mcstrImageNotExist)
                Exit Function

            End If

            'check copy path, if it's not existing, create ones
            strSavePath = My.Application.Info.DirectoryPath & strFolder
            If Not Directory.Exists(strSavePath) Then Directory.CreateDirectory(strSavePath)
            strFileName &= gcstrFileJPG
            strSavePath = strSavePath & strFileName

            'imgAvatar.Save(strSavePath)

            'imgThumbNail = New Bitmap(imgAvatar, clsDefine.PIC_LARG_W, clsDefine.PIC_LARG_H)
            imgThumbNail = New Bitmap(imgAvatar, clsDefine.PIC_LARG_W, CInt(clsDefine.PIC_LARG_W / clsDefine.PIC_CROP_RATIO))
            imgThumbNail.Save(strSavePath)

            strReturnPath = strSavePath

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If imgThumbNail IsNot Nothing Then imgThumbNail.Dispose()
            imgThumbNail = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncIsAncentor, checking a member is the ancentor
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intId  Integer, member id
    '      PARAMS     : intFather  Integer, root id
    '      MEMO       : 
    '      CREATE     : 2011/09/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncIsAncentor(ByVal intId As Integer, ByVal intFather As Integer) As Boolean

        fncIsAncentor = False

        Dim tblData As DataTable = Nothing

        Try
            'read data
            tblData = gobjDB.fncGetRel(intId, intFather)

            If tblData Is Nothing Then Exit Function

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncIsFhead, checking a member is the head
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMemId  Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/11/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncIsFhead(ByVal intMemId As Integer) As Boolean

        fncIsFhead = False

        Dim tblData As DataTable = Nothing
        Dim vwData As DataView = Nothing

        Try
            'read data
            tblData = gobjDB.fncGetFHead()

            If tblData Is Nothing Then Exit Function

            vwData = New DataView(tblData)

            vwData.RowFilter = String.Format(basConst.gcstrMemberFilter, intMemId)

            If vwData.Count < 1 Then Exit Function

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            If vwData IsNot Nothing Then vwData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncIsRoot, checking a member is the root
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMemId  Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/11/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncIsRoot(ByVal intMemId As Integer) As Boolean

        fncIsRoot = False

        Try
            Dim intRootID As Integer

            intRootID = fncGetRoot()

            If intRootID <> intMemId Then Exit Function

            Return True

        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetBirthDieText, return text birth - die
    '      VALUE      : String
    '      PARAMS     : dtBirth  Date, birth date
    '      PARAMS     : dtDie  Date, decease date
    '      PARAMS     : intStatus  Integer, ALIVE or DIE
    '      MEMO       : 
    '      CREATE     : 2011/09/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetBirthDieText(ByVal dtBirth As Date, ByVal dtDie As Date, ByVal intStatus As Integer) As String

        fncGetBirthDieText = ""

        Try

            fncGetBirthDieText = fncGetBirthDieText(dtBirth.Year, dtDie.Year, intStatus)

            'Select Case intStatus
            '    Case basConst.gcintALIVE
            '        If dtBirth > Date.MinValue Then fncGetBirthDieText &= dtBirth.Year.ToString()

            '    Case basConst.gcintDIED

            '        If dtBirth > Date.MinValue Then
            '            fncGetBirthDieText &= dtBirth.Year.ToString()
            '        Else
            '            fncGetBirthDieText &= "????"
            '        End If

            '        fncGetBirthDieText &= " - "

            '        If dtDie > Date.MinValue Then
            '            fncGetBirthDieText &= dtDie.Year.ToString()
            '        Else
            '            fncGetBirthDieText &= "????"
            '        End If

            'End Select

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetBirthDieText, return text birth - die
    '      VALUE      : String
    '      PARAMS     : intByea  Integer, birth year
    '      PARAMS     : intDyea  Integer, decease year
    '      PARAMS     : intStatus  Integer, ALIVE or DIE
    '      MEMO       : 
    '      CREATE     : 2011/09/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetBirthDieText(ByVal intByea As Integer, ByVal intDyea As Integer, ByVal intStatus As Integer) As String

        fncGetBirthDieText = ""

        Try
            Select Case intStatus
                Case basConst.gcintALIVE
                    If intByea > 0 Then fncGetBirthDieText &= intByea.ToString()
                    'has birth date only
                    If intDyea <= 0 And intByea > 0 Then
                        fncGetBirthDieText = "Sinh " & intByea.ToString()
                        Exit Function
                    End If

                Case basConst.gcintDIED

                    If intByea > 0 Then
                        fncGetBirthDieText &= intByea.ToString()
                    Else
                        fncGetBirthDieText &= "????"
                    End If

                    fncGetBirthDieText &= " - "

                    If intDyea > 0 Then
                        fncGetBirthDieText &= intDyea.ToString()
                    Else
                        fncGetBirthDieText &= "????"
                    End If

                    If String.Compare(fncGetBirthDieText, "???? - ????") = 0 Then
                        fncGetBirthDieText = basConst.gcstrDeadDateUNKNOWText
                        Exit Function
                    End If

                    'has decease date only
                    If intDyea > 0 And intByea <= 0 Then
                        fncGetBirthDieText = "Mất " & intDyea.ToString()
                        Exit Function
                    End If

            End Select

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function fncTryDate(ByVal intYear As Integer, ByVal intMonth As Integer, ByVal intDay As Integer) As Boolean
        fncTryDate = False
        Try

            Dim dtDate As Date = New Date(intYear, intMonth, intDay)

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function fncGetDateStatus(ByVal stDate As stCalendar,
                                     Optional ByVal intAliveStatus As Integer = basConst.gcintALIVE) As String

        Return fncGetDateStatus(stDate.intYear, stDate.intMonth, stDate.intDay, intAliveStatus)

    End Function

    Public Function fncGetDateStatus(ByVal intYear As Integer,
                                     ByVal intMonth As Integer,
                                     ByVal intDay As Integer,
                                     Optional ByVal intAliveStatus As Integer = basConst.gcintALIVE) As String
        fncGetDateStatus = IIf(My.Settings.blnShowUnknownBirthDay = True, "Ngày sinh: không rõ", "")
        Dim strRet As String = ""

        If intAliveStatus = basConst.gcintALIVE Then
            strRet = "Sinh "
        Else
            strRet = "Mất "
        End If

        Try

            If intYear <= 0 And intMonth <= 0 And intDay <= 0 Then

                If (intAliveStatus = basConst.gcintALIVE) Then
                    Return IIf(My.Settings.blnShowUnknownBirthDay = True, "Ngày sinh: không rõ", "")
                Else
                    Return basConst.gcstrDeadDateUNKNOWText
                End If


            End If
            If intAliveStatus = basConst.gcintALIVE Then
                If fncTryDate(intYear, intMonth, intDay) Then
                    Return strRet & Format(New Date(intYear, intMonth, intDay), "dd/MM/yyyy")
                End If
            End If


            strRet = strRet & CStr(IIf(intDay <= 0, "??", Format(intDay, "0#"))) & "/" & CStr(IIf(intMonth <= 0, "??", Format(intMonth, "0#"))) & "/" & CStr(IIf(intYear <= 0, "????", CStr(intYear)))

            strRet = strRet.Replace("??/??/", "")
            strRet = CStr(IIf(strRet.IndexOf("/??/") < 0, strRet.Replace("??/", ""), strRet))
            strRet = strRet.Replace("??", "--")

            Return strRet

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncGetDateStatus", ex)
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncHasFaMo, check if a member has father or mother
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : intID  Integer, member id
    '      PARAMS     : intFaMo  clsEnum.emGender, gender
    '      MEMO       : 
    '      CREATE     : 2011/09/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncHasFaMo(ByVal intID As Integer, ByVal emFaMo As clsEnum.emGender) As Boolean

        fncHasFaMo = False

        Dim tblData As DataTable = Nothing
        Dim vwData As DataView = Nothing

        Try
            Dim intMale As Integer = clsEnum.emGender.MALE
            Dim intFemale As Integer = clsEnum.emGender.FEMALE

            'get father and mother in natural relationship only
            tblData = gobjDB.fncGetParent(intID, False)

            If tblData Is Nothing Then Exit Function

            vwData = New DataView(tblData)

            If emFaMo = clsEnum.emGender.MALE Or emFaMo = clsEnum.emGender.UNKNOW Then vwData.RowFilter = String.Format(mcstrGenderFilter, intMale)

            If emFaMo = clsEnum.emGender.FEMALE Then vwData.RowFilter = String.Format(mcstrGenderFilter, intFemale)

            If vwData.Count = 0 Then Return False

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            If vwData IsNot Nothing Then vwData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncHasRel, check if a member has a relationship
    '      VALUE      : Boolean, true - has rel, false - has not
    '      PARAMS     : intID  Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/11/18  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncHasRel(ByVal intID As Integer) As Boolean

        fncHasRel = False

        Dim tblData As DataTable = Nothing

        Try
            'get relation
            tblData = gobjDB.fncGetRel(intID)

            If tblData Is Nothing Then Exit Function

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncHasSpouse, check if a member has a relationship
    '      VALUE      : Boolean, true - has rel, false - has not
    '      PARAMS     : intID  Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/11/18  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncHasSpouse(ByVal intID As Integer) As Boolean

        fncHasSpouse = False

        Dim tblData As DataTable = Nothing

        Try
            'get relation
            tblData = gobjDB.fncGetRel(intID, -1, CInt(clsEnum.emRelation.MARRIAGE))

            If tblData Is Nothing Then Exit Function

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetFaMoID, get fa/mo id
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : intMemID  Integer, member id
    '      PARAMS     : intFaID  Integer, return father id
    '      PARAMS     : intMoID  Integer, return mother id
    '      MEMO       : 
    '      CREATE     : 2011/11/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetFaMoID(ByVal intMemID As Integer, ByRef intFaID As Integer, ByRef intMoID As Integer) As Boolean

        fncGetFaMoID = False

        Dim tblData As DataTable = Nothing
        Dim vwData As DataView = Nothing

        Try
            Dim intGender As Integer

            tblData = gobjDB.fncGetParent(intMemID, False)

            'default is no member
            intFaID = clsDefine.NONE_VALUE
            intMoID = clsDefine.NONE_VALUE

            If tblData IsNot Nothing Then

                vwData = New DataView(tblData)

                'find id of father
                intGender = clsEnum.emGender.MALE
                vwData.RowFilter = String.Format(mcstrGenderFilter, intGender)
                If vwData.Count > 0 Then Integer.TryParse(fncCnvNullToString(vwData(0)("REL_FMEMBER_ID")), intFaID)

                intGender = clsEnum.emGender.FEMALE
                vwData.RowFilter = String.Format(mcstrGenderFilter, intGender)
                If vwData.Count > 0 Then Integer.TryParse(fncCnvNullToString(vwData(0)("REL_FMEMBER_ID")), intMoID)

            End If

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            If vwData IsNot Nothing Then vwData.Dispose()
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncGetFaMoID, get fa/mo id
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : intMemID  Integer, member id
    '      PARAMS     : intFaID  Integer, return father id
    '      PARAMS     : intMoID  Integer, return mother id
    '      MEMO       : 
    '      CREATE     : 2011/11/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetFaMo(ByVal intMemID As Integer, ByRef intFaID As Integer, ByRef intMoID As Integer, ByRef strFatherName As String, ByRef strMotherName As String) As Boolean

        fncGetFaMo = False

        Dim tblData As DataTable = Nothing
        Dim vwData As DataView = Nothing

        Try
            Dim intGender As Integer

            tblData = gobjDB.fncGetParent(intMemID, False)

            'default is no member
            intFaID = clsDefine.NONE_VALUE
            intMoID = clsDefine.NONE_VALUE

            If tblData IsNot Nothing Then

                vwData = New DataView(tblData)

                'find id of father
                intGender = clsEnum.emGender.MALE
                vwData.RowFilter = String.Format(mcstrGenderFilter, intGender)
                If vwData.Count > 0 Then
                    Integer.TryParse(fncCnvNullToString(vwData(0)("REL_FMEMBER_ID")), intFaID)
                    strFatherName = basCommon.fncGetFullName(fncCnvNullToString(vwData(0)("FIRST_NAME")), fncCnvNullToString(vwData(0)("MIDDLE_NAME")), fncCnvNullToString(vwData(0)("LAST_NAME")), fncCnvNullToString(vwData(0)("ALIAS_NAME")))
                End If

                intGender = clsEnum.emGender.FEMALE
                vwData.RowFilter = String.Format(mcstrGenderFilter, intGender)
                If vwData.Count > 0 Then
                    Integer.TryParse(fncCnvNullToString(vwData(0)("REL_FMEMBER_ID")), intMoID)
                    strMotherName = basCommon.fncGetFullName(fncCnvNullToString(vwData(0)("FIRST_NAME")), fncCnvNullToString(vwData(0)("MIDDLE_NAME")), fncCnvNullToString(vwData(0)("LAST_NAME")), fncCnvNullToString(vwData(0)("ALIAS_NAME")))
                End If

            End If

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            If vwData IsNot Nothing Then vwData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetKids, get kids
    '      VALUE      : DataTable
    '      PARAMS     : intFather  Integer, father id
    '      PARAMS     : intMother  Integer, mother id
    '      MEMO       : 
    '      CREATE     : 2011/12/13  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetKids(ByVal intFather As Integer, Optional ByVal intMother As Integer = basConst.gcintNO_MEMBER) As DataTable

        Dim tblDataRel As DataTable = Nothing
        Dim tblDataAll As DataTable = Nothing
        Dim vwDataMo As DataView = Nothing

        Try
            Dim intKidId As Integer

            'get all kids
            tblDataRel = gobjDB.fncGetKids(intFather)

            'exit if there is no kid
            If tblDataRel Is Nothing Then Return Nothing

            'return kids if there is no mother
            If intMother <= basConst.gcintNO_MEMBER Then Return tblDataRel

            'filter by mother
            tblDataAll = gobjDB.fncGetRel()
            vwDataMo = New DataView(tblDataAll)

            'loop for each kid. delete kids who don't have the specified mother
            For i As Integer = tblDataRel.Rows.Count - 1 To 0 Step -1

                If Not Integer.TryParse(tblDataRel.Rows(i)("MEMBER_ID").ToString(), intKidId) Then Continue For

                If Not xHasMotherIs(intMother, intKidId, vwDataMo) Then tblDataRel.Rows(i).Delete()

            Next

            If tblDataRel.Rows.Count <= 0 Then Return Nothing

            Return tblDataRel

        Catch ex As Exception
            Throw ex
        Finally
            If tblDataAll IsNot Nothing Then tblDataAll.Dispose()
            If vwDataMo IsNot Nothing Then vwDataMo.Dispose()
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncGetKidMaxOrder, get kid max order
    '      VALUE      : DataTable
    '      PARAMS     : intFather  Integer, father id
    '      PARAMS     : intMother  Integer, mother id
    '      MEMO       : 
    '      CREATE     : 2011/12/13  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetKidMaxOrder(ByVal intFather As Integer, Optional ByVal intMother As Integer = basConst.gcintNO_MEMBER) As Integer

        Dim dtKidsList As DataTable = fncGetKids(intFather, intMother)

        Try

            If dtKidsList Is Nothing Then Return 1
            Dim i As Integer
            Dim intMaxOrder As Integer = -1

            For i = 0 To dtKidsList.Rows.Count - 1
                If intMaxOrder < basCommon.fncCnvToInt(dtKidsList.Rows(i).Item("FAMILY_ORDER")) Then
                    intMaxOrder = basCommon.fncCnvToInt(dtKidsList.Rows(i).Item("FAMILY_ORDER"))
                End If
            Next

            Return intMaxOrder + 1
        Catch ex As Exception

            Return 1
            Throw ex

        Finally
            If dtKidsList IsNot Nothing Then dtKidsList.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetRoot, get Root of Family
    '      VALUE      : DataTable
    '      PARAMS     : intFather  Integer, father id
    '      PARAMS     : intMother  Integer, mother id
    '      MEMO       : 
    '      CREATE     : 2011/12/13  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetRoot() As Integer

        Dim tblData As DataTable = Nothing
        Dim intResult As Integer = basConst.gcintNO_MEMBER

        Try
            tblData = gobjDB.fncGetRoot(False)

            If tblData Is Nothing Then Return intResult

            If Not Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(0)("MEMBER_ID")), intResult) Then Return intResult

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

        Return intResult

    End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : fncIsDownLineOf, check if a member is the ancentor of other
    ''      VALUE      : Boolean, true - yes, false - no
    ''      PARAMS     : intUpperID  Integer, father id
    ''      PARAMS     : intMemID  Integer, selected id
    ''      MEMO       : 
    ''      CREATE     : 2012/01/03  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Public Function fncIsDownLineOf(ByVal intUpperID As Integer, ByVal intMemID As Integer) As Boolean

    '    fncIsDownLineOf = False

    '    Dim tblRel As DataTable = Nothing

    '    Try
    '        tblRel = gobjDB.fncGetRel()

    '        If tblRel Is Nothing Then Exit Function

    '        xIsDownLineOf(intUpperID, intMemID, tblRel, fncIsDownLineOf)

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If tblRel IsNot Nothing Then tblRel.Dispose()
    '    End Try

    'End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : fncIsDownLineOf, check if a member is the ancentor of other
    ''      VALUE      : Boolean, true - yes, false - no
    ''      PARAMS     : intUpperID  Integer, father id
    ''      PARAMS     : intMemID  Integer, selected id
    ''      PARAMS     : tblRel  DataTable, datatable to search
    ''      PARAMS     : blnIsDownline  Boolean, flag
    ''      MEMO       : 
    ''      CREATE     : 2012/01/03  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xIsDownLineOf(ByVal intUpperID As Integer, ByVal intMemID As Integer, ByVal tblRel As DataTable, ByRef blnIsDownline As Boolean) As Boolean

    '    xIsDownLineOf = False

    '    Dim vwRel As DataView = Nothing

    '    Try
    '        Dim intKid As Integer
    '        Dim intRelID As Integer

    '        intRelID = clsEnum.emRelation.NATURAL

    '        'filter by father
    '        vwRel = New DataView(tblRel)
    '        vwRel.RowFilter = String.Format("REL_FMEMBER_ID = {0} AND RELID = {1}", intUpperID, intRelID)

    '        'loop for each child
    '        For i As Integer = 0 To vwRel.Count - 1

    '            'read child
    '            Integer.TryParse(fncCnvNullToString(vwRel(i)("MEMBER_ID")), intKid)

    '            'if child matches the selected member
    '            If intMemID = intKid Then
    '                'set flag as found
    '                blnIsDownline = True
    '                Exit Function
    '            Else
    '                If Not xIsDownLineOf(intKid, intMemID, tblRel, blnIsDownline) Then Exit For
    '            End If

    '        Next

    '        Return True

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If vwRel IsNot Nothing Then vwRel.Dispose()
    '    End Try

    'End Function


    '   ******************************************************************
    '　　　FUNCTION   : xHasMotherIs, check mother if a member
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : intMother  Integer, mother id
    '      PARAMS     : intMember  Integer, member id
    '      PARAMS     : vwData     DataView, list of relation
    '      MEMO       : 
    '      CREATE     : 2011/12/13  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xHasMotherIs(ByVal intMother As Integer, ByVal intMember As Integer, ByVal vwData As DataView) As Boolean

        xHasMotherIs = False

        Try
            'filter by mother
            vwData.RowFilter = String.Format("REL_FMEMBER_ID = {0} AND MEMBER_ID = {1}", intMother, intMember)

            If vwData.Count <= 0 Then Exit Function

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMemberExist, check if a member has a relationship
    '      VALUE      : Boolean, true - has rel, false - has not
    '      PARAMS     : intMemID  Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/12/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMemberExist(ByVal intMemID As Integer) As Boolean

        fncMemberExist = False

        Dim tblData As DataTable = Nothing

        Try
            'get relation
            tblData = gobjDB.fncGetMemberMain(intMemID)

            If tblData Is Nothing Then Exit Function

            Return True

        Catch ex As Exception
            'Throw ex
            Return False
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMemberExist, check if a member has a relationship
    '      VALUE      : Boolean, true - has record, false - has not
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncHasFamilyInfo() As Boolean

        fncHasFamilyInfo = False

        Dim tblData As DataTable = Nothing

        Try
            'get relation
            tblData = gobjDB.fncGetFamilyInfo()

            If tblData Is Nothing Then Exit Function

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCreateFolder, create a folder
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : strFolderPath  String, folder path to create
    '      PARAMS     : blnIsHidden    Boolean, hidden attribute
    '      MEMO       : 
    '      CREATE     : 2011/12/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************

    Public Function fncCreateFolder(ByVal strFolderPath As String, Optional ByVal blnIsHidden As Boolean = True) As Boolean

        fncCreateFolder = False

        Dim objDirInfo As System.IO.DirectoryInfo = Nothing

        Try
            objDirInfo = New System.IO.DirectoryInfo(strFolderPath)

            'check existance of temp folder
            If Not objDirInfo.Exists Then

                'create folder
                objDirInfo.Create()

                'set hidden
                If blnIsHidden Then objDirInfo.Attributes = System.IO.FileAttributes.Hidden

            End If

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            objDirInfo = Nothing
        End Try

    End Function

    Public Function fncGetZoomValue(ByVal dblValue As Double) As Double
        Return dblValue
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xGetMemberImagePath
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetMemberImagePath(ByVal objCard As usrMemberCard1) As String

        GetMemberImagePath = My.Application.Info.DirectoryPath & "\docs\no_avatar_m.jpg"
        Try

            If objCard.CardImageLocation() <> "" Then Return objCard.CardImageLocation()

            If objCard.CardGender = clsEnum.emGender.FEMALE Then

                Return My.Application.Info.DirectoryPath & "\docs\no_avatar_f.jpg"

            ElseIf objCard.CardGender = clsEnum.emGender.UNKNOW Then

                Return My.Application.Info.DirectoryPath & "\docs\UnknownMember.jpg"

            End If
        Catch ex As Exception

        End Try


    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncDeleteFolder, create a folder
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : strFolderPath  String, folder path to delete
    '      MEMO       : 
    '      CREATE     : 2011/12/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncDeleteFolder(ByVal strFolderPath As String) As Boolean

        fncDeleteFolder = False

        Try
            If System.IO.Directory.Exists(strFolderPath) Then

                Dim objDirInfo As New DirectoryInfo(strFolderPath)

                'reset attribute before deleting
                xSetFolderAttr(objDirInfo, FileAttributes.Normal)
                objDirInfo.Delete(True)

            End If

            Return True

        Catch ex As Exception
            Return False
            Throw ex
        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetFolderAttr, set folder attribute
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : objDir  DirectoryInfo, folder
    '      PARAMS     : emAttr  FileAttributes, file's attribute
    '      MEMO       : 
    '      CREATE     : 2012/10/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetFolderAttr(ByVal objDir As System.IO.DirectoryInfo, ByVal emAttr As FileAttributes) As Boolean

        xSetFolderAttr = False

        Try
            If objDir.Exists Then

                'set this folder's attribute
                objDir.Attributes = FileAttributes.Normal

                'set file's attribute
                For Each objFile As FileInfo In objDir.GetFiles
                    objFile.Attributes = FileAttributes.Normal
                Next

                'set folder's attribute
                For Each objFolder As DirectoryInfo In objDir.GetDirectories
                    xSetFolderAttr(objFolder, emAttr)
                Next

            End If

            Return True

        Catch ex As Exception
            Return False
            Throw ex
        Finally

        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCopyFolder, copy a folder
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : strSrcPath  String, source folder
    '      PARAMS     : strDesPath  String, destination folder
    '      PARAMS     : blnOverwrite  Boolean, overwrite flag
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCopyFolder(ByVal strSrcPath As String, ByVal strDesPath As String, ByVal blnIgnoreOpenFile As Boolean, Optional ByVal blnOverwrite As Boolean = False) As Boolean

        fncCopyFolder = False

        Try
            Dim objSrcDir As DirectoryInfo
            Dim objDesDir As DirectoryInfo
            Dim objChildFile As FileInfo

            objSrcDir = New DirectoryInfo(strSrcPath)
            objDesDir = New DirectoryInfo(strDesPath)

            ' the source directory must exist, otherwise exit
            If Not objSrcDir.Exists Then Exit Function

            ' if destination SubDir's parent SubDir does not exist throw an exception
            If Not objDesDir.Parent.Exists Then Exit Function

            If Not objDesDir.Exists Then objDesDir.Create()

            ' copy all the files of the current directory
            For Each objChildFile In objSrcDir.GetFiles()

                If blnOverwrite Then

                    Try
                        'objChildFile.Attributes = FileAttributes.Normal
                        objChildFile.CopyTo(Path.Combine(objDesDir.FullName, objChildFile.Name), True)

                    Catch ex As Exception
                        If blnIgnoreOpenFile Then
                            Continue For
                        Else
                            Return False
                        End If
                    End Try

                Else

                    ' if Overwrite = false, copy the file only if it does not exist
                    ' this is done to avoid an IOException if a file already exists
                    ' this way the other files can be copied anyway...
                    Try
                        If Not File.Exists(Path.Combine(objDesDir.FullName, objChildFile.Name)) Then

                            'objChildFile.Attributes = FileAttributes.Normal
                            objChildFile.CopyTo(Path.Combine(objDesDir.FullName, objChildFile.Name), False)

                        End If

                    Catch ex As Exception
                        If blnIgnoreOpenFile Then
                            Continue For
                        Else
                            Return False
                        End If
                    End Try

                End If

            Next

            ' copy all the sub-directories by recursively calling this same routine

            Dim SubDir As DirectoryInfo

            For Each SubDir In objSrcDir.GetDirectories()

                If Not fncCopyFolder(SubDir.FullName, Path.Combine(objDesDir.FullName, SubDir.Name), blnOverwrite) Then Return False

            Next

            Return True

        Catch ex As Exception
            Throw ex
        End Try


    End Function


    ''' <summary>
    ''' Rename a folder
    ''' </summary>
    ''' <param name="strOldDir"></param>
    ''' <param name="strNewName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncRenameFolder(ByVal strOldDir As String, ByVal strNewName As String) As Boolean

        fncRenameFolder = False

        Try
            If Not Directory.Exists(strOldDir) Then Return True

            Try
                'Directory.Move(strOldName, strNewName)
                FileIO.FileSystem.RenameDirectory(strOldDir, strNewName)
            Catch ex As Exception
                Return False
            End Try

            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function fncOpenAppForFile(ByVal strFile As String) As Boolean
        Try
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            MessageBox.Show("Không thể tìm thấy tệp tin " & strFile & " hoặc không có chương trình để mở!", "Phần mềm quản lý gia phả", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncOpenWordFile, open a word file
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : strFile  String, file path
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncOpenWordFile(ByVal strFile As String) As Boolean

        fncOpenWordFile = False

        Dim objAppWord As Object = Nothing
        Dim objDocWord As Object = Nothing

        Try

            If Not System.IO.File.Exists(strFile) Then Exit Function

            'create word application
            Try
                objAppWord = CreateObject("Word.Application")
            Catch e As Exception
                fncMessageWarning(mcstrMsWordRequired)
                fncReleaseObject(objAppWord)
                Return False
            End Try

            'open file
            objDocWord = objAppWord.Documents.Open(strFile)

            objAppWord.Visible = True

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            'fncReleaseObject(objAppWord)
            'fncReleaseObject(objDocWord)
        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncIsValidSolarDate, check if a date is from Jan 25/1800 to Dec 31/2199
    '      VALUE      : Boolean, true - has fa/mo, false - has not
    '      PARAMS     : dtSolar  Date, 
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncIsValidSolarDate(ByVal dtSolar As Date) As Boolean

        fncIsValidSolarDate = False

        Try
            Dim dtMin As Date                       'min date

            dtMin = New Date(gcintMinYear, gcintMinMonth, gcintMinDay)

            'check null
            If dtSolar <= Date.MinValue Then Exit Function

            'return empty string if this year is >minyear and <maxyear
            If dtSolar <= dtMin Or dtSolar.Year >= gcintMaxYear Then Exit Function

            Return True

        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Function


    '******************************************************************
    '　　　FUNCTION     : Nullの場合 0 にして返す
    '　　　MEMO         : 無し 
    '　　　VALUE        : integer      Nullチェック済みの値
    '      PARAMS       : Object       値
    '      CREATE       : 2009/09/02   AKB 
    '      UPDATE       : 
    '******************************************************************
    Public Function fncCnvToInt(ByVal vobjValue As Object) As Integer
        fncCnvToInt = 0

        Try

            Dim intValue As Integer

            intValue = 0

            If IsDBNull(vobjValue) Then Return 0

            If fncIsBlank(vobjValue) Then Return 0

            If IsNumeric(vobjValue) Then

                intValue = CInt(vobjValue)

            End If

            Return intValue

        Catch ex As Exception

            fncSaveErr(mcstrClsName, "fncCnvToInt", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCnvRtfToText, convert RTF to plain text
    '      VALUE      : String in plain text
    '      PARAMS     : strRTF  String, 
    '      MEMO       : 
    '      CREATE     : 2012/09/26  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCnvRtfToText(ByVal strRTF As String) As String

        fncCnvRtfToText = ""

        Try
            If fncIsBlank(strRTF) Then Exit Function

            Using txtRich As New RichTextBox
                Try
                    txtRich.Rtf = strRTF
                    fncCnvRtfToText = txtRich.Text
                Catch ex As Exception
                    fncCnvRtfToText = strRTF
                End Try
            End Using

        Catch ex As Exception
            fncSaveErr(mcstrClsName, "fncCnvRtfToText", ex)
        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncCnvRtfToText, convert RTF to plain text
    '      VALUE      : String in plain text
    '      PARAMS     : strRTF  String, 
    '      MEMO       : 
    '      CREATE     : 2012/09/26  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCnvTextToRtf(ByVal strText As String) As String

        fncCnvTextToRtf = ""

        Try
            If fncIsBlank(strText) Then Exit Function

            Using txtRich As New RichTextBox
                txtRich.Text = strText
                fncCnvTextToRtf = txtRich.Rtf
            End Using

        Catch ex As Exception
            fncSaveErr(mcstrClsName, "fncCnvRtfToText", ex)
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncSetRemarkField, try setting richtextbox value
    '      VALUE      : boolean
    '      PARAMS     : rtfCtrl  richtextbox, 
    '      PARAMS     : strValue  String, 
    '      MEMO       : 
    '      CREATE     : 2012/10/26  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncSetRemarkField(ByVal rtfCtrl As RichTextBox, ByVal strValue As String) As Boolean
        Dim blnCheck As Boolean = True

        Try
            rtfCtrl.Rtf = strValue

        Catch ex As Exception

            blnCheck = False

        Finally

            If Not blnCheck Then
                rtfCtrl.LanguageOption = RichTextBoxLanguageOptions.DualFont
                rtfCtrl.Text = strValue
            End If
        End Try

        Return blnCheck
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetMemberName, get name of member by id
    '      VALUE      : String
    '      PARAMS     : intID  Integer, member id
    '      MEMO       : 
    '      CREATE     : 2012/01/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetMemberName(ByVal intID As Integer) As String

        fncGetMemberName = ""

        Dim tblData As DataTable = Nothing

        Try
            Dim strFName As String
            Dim strMName As String
            Dim strLName As String
            Dim strAlias As String

            tblData = gobjDB.fncGetMemberMain(intID)

            If tblData Is Nothing Then Exit Function

            strFName = fncCnvNullToString(tblData.Rows(0).Item("FIRST_NAME"))
            strMName = fncCnvNullToString(tblData.Rows(0).Item("MIDDLE_NAME"))
            strLName = fncCnvNullToString(tblData.Rows(0).Item("LAST_NAME"))
            strAlias = fncCnvNullToString(tblData.Rows(0).Item("ALIAS_NAME"))

            fncGetMemberName = fncGetFullName(strFName, strMName, strLName, strAlias)

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetHusWifList, get list of spouse by id
    '      VALUE      : Hashtable
    '      PARAMS     : intID  Integer, member id
    '      MEMO       : 
    '      CREATE     : 2012/01/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetHusWifeList(ByVal intID As Integer) As Dictionary(Of Integer, String)

        Dim objDict As Dictionary(Of Integer, String) = Nothing
        Dim tblData As DataTable = Nothing

        Try
            Dim strFName As String
            Dim strLName As String
            Dim strMName As String
            Dim strAlias As String
            Dim strFullName As String
            Dim intMemID As Integer

            'get list of husband and wife
            tblData = gobjDB.fncGetHusWife(intID)

            objDict = New Dictionary(Of Integer, String)

            If tblData Is Nothing Then Return objDict

            'loop for each husband/wife then add to hastable
            For i As Integer = 0 To tblData.Rows.Count - 1

                strFName = fncCnvNullToString(tblData.Rows(i).Item("FIRST_NAME"))
                strMName = fncCnvNullToString(tblData.Rows(i).Item("MIDDLE_NAME"))
                strLName = fncCnvNullToString(tblData.Rows(i).Item("LAST_NAME"))
                strAlias = fncCnvNullToString(tblData.Rows(i).Item("ALIAS_NAME"))
                strFullName = fncGetFullName(strFName, strMName, strLName, strAlias)

                Integer.TryParse(fncCnvNullToString(tblData.Rows(i).Item("REL_FMEMBER_ID")), intMemID)

                objDict.Add(intMemID, strFullName)

            Next

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

        Return objDict

    End Function


    'Public Function fncGetHusWifeList(ByVal intID As Integer) As Hashtable

    '    Dim tblHash As Hashtable = Nothing
    '    Dim tblData As DataTable = Nothing

    '    Try
    '        Dim strFName As String
    '        Dim strLName As String
    '        Dim strMName As String
    '        Dim strAlias As String
    '        Dim strFullName As String
    '        Dim intMemID As Integer

    '        tblHash = New Hashtable()

    '        'get list of husband and wife
    '        tblData = gobjDB.fncGetHusWife(intID)


    '        If tblData Is Nothing Then Return tblHash

    '        'loop for each husband/wife then add to hastable
    '        For i As Integer = 0 To tblData.Rows.Count - 1

    '            strFName = fncCnvNullToString(tblData.Rows(i).Item("FIRST_NAME"))
    '            strMName = fncCnvNullToString(tblData.Rows(i).Item("MIDDLE_NAME"))
    '            strLName = fncCnvNullToString(tblData.Rows(i).Item("LAST_NAME"))
    '            strAlias = fncCnvNullToString(tblData.Rows(i).Item("ALIAS_NAME"))
    '            strFullName = fncGetFullName(strFName, strMName, strLName, strAlias)

    '            Integer.TryParse(fncCnvNullToString(tblData.Rows(i).Item("REL_FMEMBER_ID")), intMemID)

    '            tblHash.Add(intMemID, strFullName)

    '        Next

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If tblData IsNot Nothing Then tblData.Dispose()
    '    End Try

    '    Return tblHash

    'End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetKidList, get list of kids by id
    '      VALUE      : Hashtable
    '      PARAMS     : intID  Integer, member id
    '      PARAMS     : emRel  emRelation, MARRIAGE for both
    '      MEMO       : 
    '      CREATE     : 2012/01/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    'Public Function fncGetKidList(ByVal intID As Integer, Optional ByVal emRel As clsEnum.emRelation = clsEnum.emRelation.MARRIAGE) As Hashtable

    '    Dim tblHash As Hashtable = Nothing
    '    Dim tblData As DataTable = Nothing
    '    Dim row() As DataRow = Nothing

    '    Try
    '        Dim strFName As String
    '        Dim strLName As String
    '        Dim strMName As String
    '        Dim strAlias As String
    '        Dim strFullName As String
    '        Dim intMemID As Integer

    '        tblHash = New Hashtable()

    '        'get list of husband and wife
    '        tblData = fncGetKids(intID)

    '        If tblData Is Nothing Then Return tblHash

    '        'filter by relation ship, - MARRIAGE for both
    '        Select Case emRel
    '            Case clsEnum.emRelation.ADOPT
    '                row = tblData.Select(String.Format("RELID = {0}", CInt(clsEnum.emRelation.ADOPT)))

    '            Case clsEnum.emRelation.NATURAL
    '                row = tblData.Select(String.Format("RELID = {0}", CInt(clsEnum.emRelation.NATURAL)))

    '            Case clsEnum.emRelation.MARRIAGE
    '                row = tblData.Select()

    '        End Select

    '        'loop for each husband/wife then add to hastable
    '        For i As Integer = row.Length - 1 To 0 Step -1

    '            strFName = fncCnvNullToString(row(i).Item("FIRST_NAME"))
    '            strMName = fncCnvNullToString(row(i).Item("MIDDLE_NAME"))
    '            strLName = fncCnvNullToString(row(i).Item("LAST_NAME"))
    '            strAlias = fncCnvNullToString(row(i).Item("ALIAS_NAME"))
    '            strFullName = fncGetFullName(strFName, strMName, strLName, strAlias)

    '            Integer.TryParse(fncCnvNullToString(row(i).Item("MEMBER_ID")), intMemID)

    '            tblHash.Add(intMemID, strFullName)
    '            tblHash.a()

    '        Next

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If tblData IsNot Nothing Then tblData.Dispose()
    '        Erase row
    '    End Try

    '    Return tblHash

    'End Function
    Public Function fncGetKidList(ByVal intID As Integer, Optional ByVal emRel As clsEnum.emRelation = clsEnum.emRelation.MARRIAGE) As List(Of Integer)

        Dim lstKidID As List(Of Integer) = New List(Of Integer)
        Dim tblData As DataTable = Nothing
        Dim row() As DataRow = Nothing

        Try
            Dim intMemID As Integer

            'get list of husband and wife
            tblData = fncGetKids(intID)

            If tblData Is Nothing Then Exit Try

            'filter by relation ship, - MARRIAGE for both
            Select Case emRel
                Case clsEnum.emRelation.ADOPT
                    row = tblData.Select(String.Format("RELID = {0}", CInt(clsEnum.emRelation.ADOPT)))

                Case clsEnum.emRelation.NATURAL
                    row = tblData.Select(String.Format("RELID = {0}", CInt(clsEnum.emRelation.NATURAL)))

                Case clsEnum.emRelation.MARRIAGE
                    row = tblData.Select()

            End Select

            'loop for each husband/wife then add to hastable
            For i As Integer = 0 To row.Length - 1

                Integer.TryParse(fncCnvNullToString(row(i).Item("MEMBER_ID")), intMemID)
                lstKidID.Add(intMemID)

            Next

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            Erase row
        End Try

        Return lstKidID

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMemberExist, check if a member has a relationship
    '      VALUE      : Boolean, true - has rel, false - has not
    '      PARAMS     : intMemID  Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/12/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetFullName(ByVal strFirstName As Object, ByVal strMiddleName As Object, ByVal strLastName As Object, ByVal Optional strAlias As Object = "") As String

        fncGetFullName = ""

        Try
            Dim strName As String
            Dim strAliasTemp As String

            'get full name
            strName = String.Format(basConst.gcstrNameFormat, basCommon.fncCnvNullToString(strLastName), basCommon.fncCnvNullToString(strMiddleName), basCommon.fncCnvNullToString(strFirstName))
            strName = basCommon.fncRemove2Space(strName)

            strAliasTemp = basCommon.fncCnvNullToString(strAlias)
            'name with alias
            If Not basCommon.fncIsBlank(strAliasTemp) Then strName = String.Format(basConst.gcstrNameWithAlias, strName, strAliasTemp)

            fncGetFullName = strName

        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetFamilyInfo, get family information
    '      VALUE      : Boolean, true - has rel, false - has not
    '      PARAMS     : strFName  String, family name
    '      PARAMS     : strFAnni  String, family anni
    '      PARAMS     : strFHome  String, family hometown
    '      MEMO       : 
    '      CREATE     : 2011/12/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetFamilyInfo(ByRef strFName As String,
                                     ByRef strFAnni As String,
                                     ByRef strFHome As String) As Boolean

        fncGetFamilyInfo = False

        Dim tblData As DataTable = Nothing

        Try
            tblData = gobjDB.fncGetFamilyInfo()

            If tblData Is Nothing Then Exit Function

            strFName = fncCnvNullToString(tblData.Rows(0)("FAMILY_NAME"))
            strFAnni = fncCnvNullToString(tblData.Rows(0)("FAMILY_ANNIVERSARY"))
            strFHome = fncCnvNullToString(tblData.Rows(0)("FAMILY_HOMETOWN"))

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncTrialCheck, Check for trial
    '      VALUE      : Boolean, false - not pass, true - pass
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncTrialCheck(Optional ByVal blnGreaterOrEqual As Boolean = False, Optional ByVal blnShowWarningMsg As Boolean = False) As Boolean

        fncTrialCheck = False

        Try
            Dim intMinLimit As Integer = basConst.gcintLimitMember
            Dim intMaxLimit As Integer = gcintMaxLimit
            Dim intNoMem As Integer = gobjDB.fncNumOfMem

            If blnGreaterOrEqual Then
                intMinLimit -= 1
                intMaxLimit -= 1
            End If

            If intNoMem > intMaxLimit Then
                basCommon.fncMessageWarning("Bạn không thể quản lý nhiều hơn " & CStr(intMaxLimit) & " thành viên." & vbNewLine & "Hãy nâng cấp phiên bản gia phả hiện tại.")
                Return UpVersionApp()
            End If


            'do not check if software is activated
            If gblnActivated Then Return True

            If intNoMem > intMinLimit Or gblnFirstUsed Then

                'basCommon.fncMessageWarning(String.Format(mcstrTrialFail, basConst.gcintLimitMember))

                If Not basCommon.fncCheckActive() Then

                    If blnShowWarningMsg Then basCommon.fncMessageWarning(String.Format(mcstrTrialFail, basConst.gcintLimitMember) & vbNewLine & "Hãy nâng cấp phiên bản gia phả hiện tại.")

                    'it's ok if it's the first start
                    If gblnFirstUsed And intNoMem <= intMinLimit Then Return True

                    If blnShowWarningMsg Then
                        Return UpVersionApp()
                    Else
                        Return False
                    End If
                Else

                    gblnActivated = True
                    Return True
                End If

                'Exit Function

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncTrialCheck", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : Read Registry key
    '　　　VALUE      : String, value of Registry key   
    '      PARAMS     : 引数1 vstrRegistryName String,registry key
    '      MEMO       : 
    '      CREATE     : 2010/11/11  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncReadRegistry(ByVal vstrRegistryName As String, ByVal intFlg As clsEnum.RegistryLocation) As String

        fncReadRegistry = ""

        Try

            Dim objRegKey As RegistryKey = Nothing

            If Not IsNothing(vstrRegistryName) Then

                Select Case intFlg
                    Case clsEnum.RegistryLocation.CurrentUser
                        objRegKey = Registry.CurrentUser.OpenSubKey(gcstrMapFolder, True)
                    Case clsEnum.RegistryLocation.Machine
                        objRegKey = Registry.LocalMachine.OpenSubKey(gcstrMapFolder, True)
                    Case clsEnum.RegistryLocation.Users
                        objRegKey = Registry.Users.OpenSubKey(gcstrMapFolder, True)
                End Select

                'if registry is exist
                If Not IsNothing(objRegKey) Then

                    'retrn value of registry
                    Return CStr(objRegKey.GetValue(vstrRegistryName, ""))

                End If

            End If

        Catch ex As Exception

            fncSaveErr("", "xReadRegistry", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDeleteMember, delete a member
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMember   Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncDeleteMember(ByVal intMember As Integer) As Boolean

        fncDeleteMember = False
        Dim blnTrans As Boolean = False
        Dim blnSuccess As Boolean = True

        Try
            Dim strFileName As String
            Dim strAvatar As String
            Dim strThumbnail As String

            blnTrans = gobjDB.BeginTransaction()

            'delete main information
            blnSuccess = gobjDB.fncDelMemberMain(intMember, False) And blnSuccess

            'delete contact
            blnSuccess = gobjDB.fncDelContact(intMember, False) And blnSuccess

            'delete career
            blnSuccess = gobjDB.fncDelCareer(clsEnum.emCareerType.CAREER, intMember, False) And blnSuccess
            blnSuccess = gobjDB.fncDelCareer(clsEnum.emCareerType.EDU, intMember, False) And blnSuccess

            'delete fact
            blnSuccess = gobjDB.fncDelFact(intMember, False) And blnSuccess

            'delete relationship
            blnSuccess = gobjDB.fncDelRel(intMember, -1, False) And blnSuccess

            'detete from family head
            blnSuccess = gobjDB.fncDelFhead(intMember, False) And blnSuccess

            'detete from root
            blnSuccess = gobjDB.fncDelRoot(intMember, False) And blnSuccess

            'delete image from hard disk
            strFileName = String.Format(gcstrImgFormat & gcstrFileJPG, intMember)
            strAvatar = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & strFileName
            strThumbnail = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath & strFileName

            blnSuccess = fncDeleteFile(strAvatar) And blnSuccess
            blnSuccess = fncDeleteFile(strThumbnail) And blnSuccess

            If blnTrans And blnSuccess Then
                gobjDB.Commit()
            Else
                gobjDB.RollBack()
                Return False
            End If

            Return True

        Catch ex As Exception
            If blnTrans Then gobjDB.RollBack()
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDeleteMember, delete a member
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMember   Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function IsConnectedToInternet() As Boolean

        IsConnectedToInternet = False
        Try
            Dim strCHECKURL As String = "http://akb.com.vn/Giapha/CheckConnect.aspx"
            Dim strReturn As String = fncResponse(strCHECKURL, "")
            If strReturn = "<TEXT>CONNECT</TEXT>" Then Return True

        Catch ex As Exception

            'fncMessageWarning(ex.Message)
            Return False

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDeleteMember, delete a member
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMember   Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function IsConnectedToInternet(ByVal strServer As String) As Boolean

        IsConnectedToInternet = False

        Dim result As Boolean = False

        Dim p As Ping = New Ping()

        Try
            Dim reply As PingReply = p.Send(strServer, 3000)
            If reply.Status = IPStatus.Success Then
                Return True
            End If

            Return result
        Catch ex As Exception

            'MessageBox.Show(ex.Message)
            Return False

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDeleteMember, delete a member
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMember   Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncResponse(ByVal strUrl As String, ByVal strRequest As String) As String

        fncResponse = ""

        Try
            Dim objRequest As HttpWebRequest = Nothing

            Dim request As WebRequest = WebRequest.Create(strUrl)

            ' Set the Method property of the request to POST.
            request.Method = "POST"


            ' Create POST data and convert it to a byte array.

            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strRequest)

            ' Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded"

            ' Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length

            ' Get the request stream.
            Dim dataStream As Stream = request.GetRequestStream()

            ' Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length)

            ' Close the Stream object.
            dataStream.Close()

            ' Get the response.
            Dim response As Net.WebResponse = request.GetResponse()

            dataStream = response.GetResponseStream()

            ' Open the stream using a StreamReader for easy access.
            Dim reader As New StreamReader(dataStream)

            ' Read the content.
            Dim responseFromServer As String = reader.ReadToEnd()

            fncResponse = responseFromServer

        Catch ex As Exception

            'MessageBox.Show(ex.Message)
            Return ""

        End Try

    End Function


    Function getMD5Hash(ByVal strToHash As String) As String
        Dim md5Obj As New MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)

        bytesToHash = md5Obj.ComputeHash(bytesToHash)

        Dim strResult As String = ""

        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next

        Return strResult
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetNationName, Get Nationality
    '      VALUE      : String
    '      PARAMS     : intNationID   Integer, nation id
    '      MEMO       : 
    '      CREATE     : 2012/03/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetNationName(ByVal intNationID As Integer) As String

        fncGetNationName = ""

        Dim tblData As DataTable = Nothing

        Try
            tblData = gobjDB.fncGetNation(intNationID)

            If tblData Is Nothing Then Exit Function

            'has value, return nation name
            fncGetNationName = fncCnvNullToString(tblData.Rows(0)("NAT_NAME"))

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetReligionName, Get Religion
    '      VALUE      : String
    '      PARAMS     : intNationID   Integer, nation id
    '      MEMO       : 
    '      CREATE     : 2012/03/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetReligionName(ByVal intReligionID As Integer) As String

        fncGetReligionName = ""

        Dim tblData As DataTable = Nothing

        Try
            tblData = gobjDB.fncGetReligion(intReligionID)

            If tblData Is Nothing Then Exit Function

            'has value, return nation name
            fncGetReligionName = fncCnvNullToString(tblData.Rows(0)("REL_NAME"))

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMultiSelectCtrl, Multi-select controls
    '      VALUE      : Boolean
    '      PARAMS     : rectArea            Rectangle, selection area
    '      PARAMS     : mlstSelectedCtrl    List, list of selected controls
    '      PARAMS     : mtblControl         Hashtable, control list
    '      MEMO       : 
    '      CREATE     : 2012/04/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMultiSelectCtrl(ByVal rectArea As Rectangle, ByRef mlstSelectedCtrl As List(Of usrMemCardBase), ByVal mtblControl As Hashtable) As Boolean

        fncMultiSelectCtrl = False

        Try
            Dim intTemp As Integer
            Dim intX1 As Integer
            Dim intX2 As Integer
            Dim intY1 As Integer
            Dim intY2 As Integer
            Dim objCard As usrMemCardBase

            intX1 = rectArea.Location.X
            intX2 = intX1 + rectArea.Width
            intY1 = rectArea.Location.Y
            intY2 = intY1 + rectArea.Height

            'reset state
            For i As Integer = 0 To mlstSelectedCtrl.Count - 1
                mlstSelectedCtrl(i).CardSelected = False
            Next

            mlstSelectedCtrl.Clear()
            If rectArea.Width = 0 And rectArea.Height = 0 Then Exit Function

            'set state
            For Each element As DictionaryEntry In mtblControl

                objCard = CType(element.Value, usrMemCardBase)

                'card is out of range
                If objCard.Location.X > intX2 Then Continue For
                If objCard.Location.Y > intY2 Then Continue For

                'location of card is out of range but its width is still in range
                If objCard.Location.X < intX1 Then

                    intTemp = objCard.Location.X + objCard.Width
                    If intTemp < intX1 Then Continue For

                End If

                'location of card is out of range but its height is still in range
                If objCard.Location.Y < intY1 Then

                    intTemp = objCard.Location.Y + objCard.Height
                    If intTemp < intY1 Then Continue For

                End If

                'GREATE! card in range
                objCard.CardSelected = True
                objCard.BringToFront()

                mlstSelectedCtrl.Add(objCard)

            Next

            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncMoveCards, move multi-card
    '      VALUE      : Boolean
    '      PARAMS     : objCard          usrMemCardBase, card be move
    '      PARAMS     : intX             Integer, offset
    '      PARAMS     : intY             Integer, offset
    '      PARAMS     : mlstSelectedCtrl List, list of selected cards
    '      MEMO       : 
    '      CREATE     : 2012/04/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMoveCards(ByVal objCard As usrMemCardBase, ByVal intX As Integer, ByVal intY As Integer, ByRef mlstSelectedCtrl As List(Of usrMemCardBase)) As Boolean

        fncMoveCards = False

        Try
            Dim ptLocation As Point

            'exit if this is a single card
            If Not objCard.CardSelected Then Exit Function

            'this card is in the list of selected card
            For i As Integer = 0 To mlstSelectedCtrl.Count - 1

                If objCard Is mlstSelectedCtrl(i) Then Continue For

                'reset location
                ptLocation = mlstSelectedCtrl(i).Location
                ptLocation.X = ptLocation.X + intX
                ptLocation.Y = ptLocation.Y + intY

                mlstSelectedCtrl(i).CardMidBottom.X += intX
                mlstSelectedCtrl(i).CardMidTop.X += intX
                mlstSelectedCtrl(i).CardMidLeft.X += intX
                mlstSelectedCtrl(i).CardMidRight.X += intX

                mlstSelectedCtrl(i).Location = ptLocation

            Next

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '2017/02/27 Manh
    Public Function fncGetLunarYearName(ByVal intYear As Integer) As String

        Dim objLunar As clsLunarCalendar = Nothing
        objLunar = New clsLunarCalendar()

        If objLunar.fncIsValidSupportLunarYear(intYear) Then Return objLunar.GetYearName(intYear)

        objLunar = Nothing

        Return ""

    End Function

    '2017/02/27 Manh
    Private Function xPrefixDateItem(ByVal strLongFormat As String, ByVal strPrefix As String, ByVal intItem As Integer, ByVal strFormat As String)

        Dim strRet As String = ""
        strRet = strLongFormat

        strRet = strPrefix & " " & IIf(intItem > 0, intItem.ToString(strFormat), "  ")

        If (strLongFormat <> "") Then

            strRet = strLongFormat & " " & strRet

        End If

        Return strRet

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncGetDateNameLongFormat, return date string
    '      VALUE      : String
    '      PARAMS     : strInitText     String
    '      PARAMS     : intDay          Integer
    '      PARAMS     : intMon          Integer
    '      PARAMS     : intYea          Integer
    '      PARAMS     : stDate          stCalendar
    '      PARAMS     : blnIsLunar      Boolean
    '      MEMO       : 
    '      CREATE     : 2017/02/27  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetDateNameLongFormat(ByVal strInitText As String,
                                   ByVal intDay As Integer,
                                   ByVal intMon As Integer,
                                   ByVal intYea As Integer,
                                   Optional ByVal blnIsLunar As Boolean = False) As String

        fncGetDateNameLongFormat = strInitText

        Dim objLunar As clsLunarCalendar = Nothing
        Dim strRet As String = strInitText
        Dim strTemp As String = ""

        Try

            strRet = xPrefixDateItem(strRet, "ngày", intDay, "0#")
            strRet = xPrefixDateItem(strRet, "tháng", intMon, "0#")
            strRet = xPrefixDateItem(strRet, "năm", intYea, "000#")

            'concat year name if available
            If blnIsLunar And intYea > 0 Then

                strTemp = fncGetLunarYearName(intYea)

                If (strTemp <> "") Then
                    strRet &= " - " & strTemp
                End If


            End If

            Return strRet

        Catch ex As Exception
            Throw ex
        Finally
            objLunar = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetDateNamShortFormat, return date string
    '      VALUE      : String
    '      PARAMS     : strInitText     String
    '      PARAMS     : intDay          Integer
    '      PARAMS     : intMon          Integer
    '      PARAMS     : intYea          Integer
    '      PARAMS     : stDate          stCalendar
    '      PARAMS     : blnIsLunar      Boolean
    '      MEMO       : 
    '      CREATE     : 2017/02/27  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetDateNamShortFormat(ByVal strInitText As String,
                                             ByVal intDay As Integer,
                                             ByVal intMon As Integer,
                                             ByVal intYea As Integer,
                                             Optional ByVal blnIsLunar As Boolean = False) As String

        fncGetDateNamShortFormat = strInitText

        Dim objLunar As clsLunarCalendar = Nothing
        Dim strRet As String = strInitText
        Dim strTemp As String = ""

        Try

            strRet = xPrefixDateItem(strRet, "", intDay, "0#")
            strRet = xPrefixDateItem(strRet, "/", intMon, "0#")
            strRet = xPrefixDateItem(strRet, "/", intYea, "000#")

            strRet = strRet.Replace(" ", "")
            If (strRet.Replace("/", "").Replace(" ", "") = "") Then Return ""


            'concat year name if available
            If blnIsLunar And intYea > 0 Then

                strTemp = fncGetLunarYearName(intYea)

                If (strTemp <> "") Then
                    strRet &= " - " & strTemp
                End If


            End If

            Return strRet

        Catch ex As Exception
            Throw ex
        Finally
            objLunar = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetDateName, return date string
    '      VALUE      : String
    '      PARAMS     : strInitText     String
    '      PARAMS     : intDay          Integer
    '      PARAMS     : intMon          Integer
    '      PARAMS     : intYea          Integer
    '      PARAMS     : stDate          stCalendar
    '      PARAMS     : blnShortFormat  Boolean
    '      PARAMS     : blnIsLunar      Boolean
    '      MEMO       : 
    '      CREATE     : 2017/02/27  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetDateName(ByVal strInitText As String,
                                   ByVal intDay As Integer,
                                   ByVal intMon As Integer,
                                   ByVal intYea As Integer,
                                   ByVal blnShortFormat As Boolean,
                                   Optional ByVal blnIsLunar As Boolean = False) As String

        fncGetDateName = strInitText


        Try

            If (Not blnShortFormat) Then
                fncGetDateName = fncGetDateNameLongFormat(strInitText, intDay, intMon, intYea, blnIsLunar)
            Else

                fncGetDateName = fncGetDateNamShortFormat(strInitText, intDay, intMon, intYea, blnIsLunar)

            End If


        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetDateName, return date string
    '      VALUE      : String
    '      PARAMS     : strInitText     String
    '      PARAMS     : stDate          stCalendar
    '      PARAMS     : blnShortFormat  Boolean
    '      PARAMS     : blnIsLunar      Boolean
    '      MEMO       : 
    '      CREATE     : 2012/04/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetDateName(ByVal strInitText As String,
                                   ByVal stDate As stCalendar,
                                   ByVal blnShortFormat As Boolean,
                                   Optional ByVal blnIsLunar As Boolean = False) As String

        Dim strResult As String = ""

        Try

            strResult = fncGetDateName(strInitText, stDate.intDay, stDate.intMonth, stDate.intYear, blnShortFormat, blnIsLunar)

        Catch ex As Exception
            Throw ex
        End Try

        Return strResult

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetSolar2LunarDateName, return lunar date string
    '      VALUE      : String
    '      PARAMS     : strInitText     String
    '      PARAMS     : stDate          stCalendar
    '      MEMO       : 
    '      CREATE     : 2012/04/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetSolar2LunarDateName(ByVal strInitText As String,
                                        ByVal stDate As stCalendar) As String

        Dim strResult As String = ""

        Try

            strResult = fncGetSolar2LunarDateName(strInitText, stDate.intDay, stDate.intMonth, stDate.intYear)

        Catch ex As Exception
            Throw ex
        End Try

        Return strResult

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetSolar2LunarDateName, return lunar date string
    '      VALUE      : String
    '      PARAMS     : strInitText     String
    '      PARAMS     : intDay          Integer
    '      PARAMS     : intMon          Integer
    '      PARAMS     : intYea          Integer
    '      MEMO       : 
    '      CREATE     : 2012/04/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetSolar2LunarDateName(ByVal strInitText As String,
                                        ByVal intDay As Integer,
                                        ByVal intMon As Integer,
                                        ByVal intYea As Integer) As String

        Dim strResult As String = ""
        Dim objVnCal As clsLunarCalendar = Nothing

        Try
            Dim dtSolar As Date

            strResult = strInitText

            If intDay > 0 And intMon > 0 And intYea > 0 Then

                dtSolar = New Date(intYea, intMon, intDay)
                objVnCal = New clsLunarCalendar()

                If objVnCal.MinSupportedDateTime < dtSolar And dtSolar < objVnCal.MaxSupportedDateTime Then

                    strResult = objVnCal.fncGetLunarDateString(dtSolar, 2)

                End If

            End If

        Catch ex As Exception
            Throw ex
        Finally
            objVnCal = Nothing
        End Try

        Return strResult

    End Function

    ''' <summary>
    ''' fncGetLunarDate - convert sun date to lunar date
    ''' </summary>
    ''' <param name="stSolar"></param>
    ''' <param name="stLunar"></param>
    ''' <returns>Year name</returns>
    ''' <remarks></remarks>
    Public Function fncGetLunarDate(ByVal stSolar As stCalendar,
                                    ByRef stLunar As stCalendar) As String

        Return fncGetLunarDate(stSolar.intDay, stSolar.intMonth, stSolar.intYear, stLunar)

    End Function

    ''' <summary>
    ''' fncGetLunarDate - convert sun date to lunar date
    ''' </summary>
    ''' <param name="intSolarDay"></param>
    ''' <param name="intSolarMon"></param>
    ''' <param name="intSolarYear"></param>
    ''' <param name="stLunar"></param>
    ''' <returns>Year name</returns>
    ''' <remarks></remarks>
    Public Function fncGetLunarDate(ByVal intSolarDay As Integer,
                                    ByVal intSolarMon As Integer,
                                    ByVal intSolarYear As Integer,
                                    ByRef stLunar As stCalendar) As String

        fncGetLunarDate = ""

        Dim strYearName As String
        Dim objVnCal As clsLunarCalendar

        Try
            Dim dtSolar As Date
            Dim intLeapMon As Integer

            Try
                dtSolar = New Date(intSolarYear, intSolarMon, intSolarDay)
            Catch ex As Exception
                Exit Function
            End Try

            objVnCal = New clsLunarCalendar()
            With stLunar
                .intDay = objVnCal.GetDayOfMonth(dtSolar)
                .intMonth = objVnCal.GetMonth(dtSolar)
                .intYear = objVnCal.GetYear(dtSolar)
                strYearName = objVnCal.GetYearName(.intYear)

                If objVnCal.IsLeapYear(.intYear) Then

                    intLeapMon = objVnCal.GetLeapMonth(.intYear)
                    'selected month is bigger than leap month
                    If .intMonth > intLeapMon Then .intMonth = .intMonth - 1

                End If

            End With

            Return strYearName

        Catch ex As Exception
            Exit Function
        Finally
            objVnCal = Nothing
        End Try

    End Function


    ''' <summary>
    ''' fncGetSolarDate - convert sun date to lunar date
    ''' </summary>
    ''' <param name="stLunar"></param>
    ''' <param name="stSolar"></param>
    ''' <returns>Year name</returns>
    ''' <remarks></remarks>
    Public Function fncGetSolarDate(ByVal stLunar As stCalendar,
                                    ByRef stSolar As stCalendar) As String

        Return fncGetSolarDate(stLunar.intDay, stLunar.intMonth, stLunar.intYear, stSolar)

    End Function

    ''' <summary>
    ''' fncGetSolarDate - convert lunar date to solar date
    ''' </summary>
    ''' <param name="intLunarDay"></param>
    ''' <param name="intLunarMon"></param>
    ''' <param name="intLunarYear"></param>
    ''' <param name="stSolar"></param>
    ''' <returns>solar year name</returns>
    ''' <remarks></remarks>
    Public Function fncGetSolarDate(ByVal intLunarDay As Integer,
                                    ByVal intLunarMon As Integer,
                                    ByVal intLunarYear As Integer,
                                    ByRef stSolar As stCalendar) As String
        fncGetSolarDate = ""

        Dim objVnCal As clsLunarCalendar

        Try
            Dim dtSolar As Date
            objVnCal = New clsLunarCalendar()

            '2016/12/20 Start AKB Manh, change the way to get Lunar Year Name
            'If intLunarYear >= objVnCal.MinSupportedDateTime.Year And intLunarYear <= objVnCal.MaxSupportedDateTime.Year Then
            'fncGetSolarDate = objVnCal.GetYearName(intLunarYear)
            'End If
            fncGetSolarDate = objVnCal.fncGetLunarYearName(intLunarYear)
            '2016/12/20 End AKB Manh

            Try

                ' ▽ 2012/12/14   AKB Quyet （変更内容）*********************************
                'dtSolar = objVnCal.fncGetSolarDate(intLunarDay, intLunarMon, intLunarYear)
                dtSolar = objVnCal.fncGetSolarDate2(intLunarDay, intLunarMon, intLunarYear)
                ' △ 2012/12/14   AKB Quyet *********************************************
            Catch ex As Exception
                Exit Function
            End Try

            With stSolar
                .intDay = dtSolar.Day
                .intMonth = dtSolar.Month
                .intYear = dtSolar.Year
            End With

        Catch ex As Exception
            Throw ex
        Finally
            objVnCal = Nothing
        End Try
    End Function

    ''' <summary>
    ''' fncGetSolarYearName
    ''' </summary>
    ''' <param name="intYear"></param>
    ''' <returns>solar year name</returns>
    ''' <remarks></remarks>
    Public Function fncGetSolarYearName(ByVal intYear As Integer) As String
        fncGetSolarYearName = ""

        Dim clsLunarCal As clsLunarCalendar

        Try
            clsLunarCal = New clsLunarCalendar

            If intYear < clsLunarCal.MinSupportedDateTime.Year Or intYear > clsLunarCal.MaxSupportedDateTime.Year Then Exit Function

            fncGetSolarYearName = clsLunarCal.GetYearName(intYear)

        Catch ex As Exception
            Throw ex
        Finally
            clsLunarCal = Nothing
        End Try
    End Function
    '   ******************************************************************
    '　　　FUNCTION   : fncGetDateNameSolar, return solar date string
    '      VALUE      : String
    '      PARAMS     : strInitText     String
    '      PARAMS     : intDay          Integer, day in lunar
    '      PARAMS     : intMon          Integer, month in lunar
    '      PARAMS     : intYea          Integer, year in lunar
    '      MEMO       : 
    '      CREATE     : 2012/04/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetLunar2SolarDateName(ByVal strInitText As String,
                                        ByVal intLDay As Integer,
                                        ByVal intLMon As Integer,
                                        ByVal intLYea As Integer) As String

        Dim strResult As String = ""
        Dim objVnCal As clsLunarCalendar = Nothing

        Try
            Dim dtSolar As Date

            strResult = strInitText

            If intLDay > 0 And intLMon > 0 And intLYea > 0 Then

                objVnCal = New clsLunarCalendar()

                Try
                    dtSolar = objVnCal.fncGetSolarDate(intLDay, intLMon, intLYea)
                    strResult = fncGetDateName("", dtSolar.Day, dtSolar.Month, dtSolar.Year, True)

                Catch ex As Exception
                End Try

            End If

        Catch ex As Exception
            Throw ex
        Finally
            objVnCal = Nothing
        End Try

        Return strResult

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncIsColumnExist, check existence of column
    '      VALUE      : Boolean true - exist, false - not exist
    '      PARAMS     : strColName      String
    '      PARAMS     : strTableName    String
    '      MEMO       : 
    '      CREATE     : 2012/04/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncIsColumnExist(ByVal strTableName As String, ByVal strColName As String) As Boolean

        fncIsColumnExist = False

        Dim tblData As DataTable = Nothing

        Try
            'Dim strTemp As String

            tblData = gobjDB.fncGetTable(strTableName, strColName)

            If tblData Is Nothing Then Exit Function

            'Try
            '    strTemp = fncCnvNullToString(tblData.Rows(0)(strColName))
            '    Return True
            'Catch ex As Exception
            '    Return False
            'End Try

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function


    ''' <summary>
    ''' Compare 2 date
    ''' </summary>
    ''' <param name="intYea"></param>
    ''' <param name="intMon"></param>
    ''' <param name="intDay"></param>
    ''' <param name="dtCompare"></param>
    ''' <returns>1 - greater ; 0 - equal ; -1 - smaller</returns>
    ''' <remarks></remarks>
    Public Function fncCompareDate(ByVal intYea As Integer, ByVal intMon As Integer, ByVal intDay As Integer, ByVal dtCompare As Date) As Integer

        Dim intResult As Integer = 0

        Try
            'compare year
            If intYea > dtCompare.Year Then
                intResult = 1

            ElseIf intYea = dtCompare.Year Then

                'years are equal; compare month
                If intMon > dtCompare.Month Then

                    intResult = 1

                ElseIf intMon = dtCompare.Month Then

                    'month are equal; compare day
                    If intDay > dtCompare.Day Then
                        intResult = 1
                    ElseIf intDay = dtCompare.Day Then
                        intResult = 0
                    Else
                        intResult = -1
                    End If

                Else
                    intResult = -1

                End If

            Else
                intResult = -1

            End If


        Catch ex As Exception

        End Try

        Return intResult

    End Function


    ''' <summary>
    ''' fncSelectCal, select calendar form
    ''' </summary>
    ''' <param name="stInitValue"></param>
    ''' <param name="emCalMode"></param>
    ''' <param name="intReturnDay"></param>
    ''' <param name="intReturnMon"></param>
    ''' <param name="intReturnYea"></param>
    ''' <param name="blnIsShortForm"></param>
    ''' <param name="ctrlLabel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncSelectCal(ByRef stInitValue As stCalendar,
                                 ByVal emCalMode As frmCalendar.emCalendar,
                                 ByRef intReturnDay As Integer,
                                 ByRef intReturnMon As Integer,
                                 ByRef intReturnYea As Integer,
                                 ByVal blnIsShortForm As Boolean,
                                 ByVal ctrlLabel As Label) As Boolean

        fncSelectCal = False

        Try

            Using frmCal As New frmCalendar

                frmCal.fncShowForm(emCalMode, stInitValue)
                If frmCal.DateChosen Then stInitValue = frmCal.SelectedDate
                fncSelectCal = frmCal.DateChosen
            End Using

            With stInitValue

                intReturnDay = .intDay
                intReturnMon = .intMonth
                intReturnYea = .intYear
                If ctrlLabel IsNot Nothing Then
                    ' ▽2018/03/29 AKB Nguyen Thanh Tung --------------------------------
                    If emCalMode = frmCalendar.emCalendar.SUN Then
                        ctrlLabel.Text = basCommon.fncGetDateName("", .intDay, .intMonth, .intYear, blnIsShortForm, False)
                    Else
                        ctrlLabel.Text = basCommon.fncGetDateName("", .intDay, .intMonth, .intYear, blnIsShortForm, True)
                    End If

                    'If emCalMode = frmCalendar.emCalendar.SUN Then
                    '    ctrlLabel.Text = basCommon.fncGetDateName(basConst.gcstrDateUnknown, .intDay, .intMonth, .intYear, blnIsShortForm, False)
                    'Else
                    '    ctrlLabel.Text = basCommon.fncGetDateName(basConst.gcstrDateUnknown, .intDay, .intMonth, .intYear, blnIsShortForm, True)
                    'End If
                    ' △2018/03/29 AKB Nguyen Thanh Tung --------------------------------
                End If
            End With

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSelectCalBirth_Click", ex)
        End Try

    End Function


    ''' <summary>
    ''' fncGetDateDDMMYYYY
    ''' </summary>
    ''' <param name="intDay"></param>
    ''' <param name="intMon"></param>
    ''' <param name="intYea"></param>
    ''' <returns>string in format DD/MM/YYYY</returns>
    ''' <remarks></remarks>
    Public Function fncGetDateDDMMYYYY(ByVal intDay As Integer, ByVal intMon As Integer, ByVal intYea As Integer) As String

        fncGetDateDDMMYYYY = ""

        Try
            Const strFormat As String = "{0}/{1}/{2}"

            fncGetDateDDMMYYYY = String.Format(strFormat, intDay, intMon, intYea)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncGetDateDDMMYYYY", ex)
        End Try
    End Function


    ''' <summary>
    ''' Rename template file from Xlt -> Xls
    ''' </summary>
    ''' <param name="strTemplateFile"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function fncRenameTemplate(ByVal strTemplateFile As String) As Boolean

        fncRenameTemplate = False

        Try
            Dim strOldFile As String

            strOldFile = strTemplateFile.Substring(0, strTemplateFile.Length - 1) + "t"

            If Not System.IO.File.Exists(strOldFile) Then
                Return False
            End If

            'Xlt file exits, try to rename from Xlt -> Xls
            System.IO.File.Move(strOldFile, strTemplateFile)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncRenameTemplate", ex)
        End Try
    End Function


    ''' <summary>
    ''' fncCreateTestData - Create sample data for testing
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function fncCreateTestData() As Boolean

        fncCreateTestData = False

        Dim blnBegin As Boolean = False
        Dim blnSuccess As Boolean = True

        Try
            Dim stHusMain As clsDbAccess.stMemberInfoMain = Nothing
            Dim stWifMain As clsDbAccess.stMemberInfoMain = Nothing

            Dim stHusInfo As clsDbAccess.stMemberInfoContact = Nothing
            Dim stWifInfo As clsDbAccess.stMemberInfoContact = Nothing
            Dim intPreID As Integer = basConst.gcintRootID

            blnBegin = gobjDB.BeginTransaction()


            For i As Integer = 1 To 25000

                If Not blnSuccess Then Exit For

                Dim rd As New Random()
                Dim intID As Integer = gobjDB.fncGetMaxID(clsEnum.emTable.T_FMEMBER_MAIN)

                stHusMain.intID = intID + 1
                stHusMain.strFirstName = rd.Next()
                stHusMain.strLastName = rd.Next()
                stHusMain.intGender = clsEnum.emGender.MALE

                stWifMain.intID = intID + 2
                stWifMain.strFirstName = rd.Next()
                stWifMain.strLastName = rd.Next()
                stWifMain.intGender = clsEnum.emGender.FEMALE

                stHusInfo.intID = intID + 1
                stWifInfo.intID = intID + 2

                'insert 2 new member
                blnSuccess = blnSuccess And gobjDB.fncInsertMemberMain(stHusMain, False)
                blnSuccess = blnSuccess And gobjDB.fncInsertMemberMain(stWifMain, False)

                blnSuccess = blnSuccess And gobjDB.fncInsertContact(stHusInfo, False)
                blnSuccess = blnSuccess And gobjDB.fncInsertContact(stWifInfo, False)

                'make these couple is husband and wife
                blnSuccess = blnSuccess And gobjDB.fncInsertRel(stHusInfo.intID, stWifInfo.intID, clsEnum.emRelation.MARRIAGE, False)
                blnSuccess = blnSuccess And gobjDB.fncInsertRel(stWifInfo.intID, stHusInfo.intID, clsEnum.emRelation.MARRIAGE, False)

                'current member is the son of the previous one
                blnSuccess = blnSuccess And gobjDB.fncInsertRel(stHusInfo.intID, intPreID, clsEnum.emRelation.NATURAL, False)
                intPreID = stHusInfo.intID

            Next

            If blnBegin And blnSuccess Then
                gobjDB.Commit()
            Else
                gobjDB.RollBack()
            End If

            Return True
        Catch ex As Exception

        End Try

    End Function


    ''' <summary>
    ''' fncMakeCbPage - Create pages combobox
    ''' </summary>
    ''' <param name="intTotalPage"></param>
    ''' <param name="cbPage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncMakeCbPage(ByVal intTotalPage As Integer, ByVal cbPage As ComboBox) As Boolean

        fncMakeCbPage = False

        Try
            cbPage.Items.Clear()

            For i As Integer = 1 To intTotalPage
                cbPage.Items.Add(i)
            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncMakeCbPage", ex)
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCheckActive, check for existance of database
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCheckActive(Optional ByVal blnShowWarningMsg As Boolean = True) As Boolean
        fncCheckActive = False

        Try

            'check active file
            Dim mstrActiveFile As String = Application.StartupPath + "\Activekey.txt"

            Dim Computer As New clsComputerInfo
            Dim strComputerID As String
            strComputerID = Computer.GetVolumeSerial + Computer.GetProcessorId


            If System.IO.File.Exists(mstrActiveFile) Then

                'Get data from file to Array
                Dim strlines() As String = My.Computer.FileSystem.ReadAllText(mstrActiveFile).Replace(vbLf, "").Split(CChar(vbCr))

                If strlines.Length < 2 Then
                    MessageBox.Show("Mã sản phẩm không phù hợp.")
                    basCommon.fncDeleteFile(mstrActiveFile)
                    Return False
                End If

                If IsConnectedToInternet() Then

                    If fncGetComputerID(strlines(1)) <> strComputerID Then

                        MessageBox.Show("Mã sản phẩm không phù hợp." + vbCrLf + "Xin vui lòng nhập lại mã sản phẩm tại màn hình sau đây.")
                        basCommon.fncDeleteFile(mstrActiveFile)

                        Dim frmActive As frmActiveKey = New frmActiveKey

                        'QUYET comment dong nay ▼
                        frmActive.Run(strComputerID, 2)

                        If frmActive.mblnActiveOk = False Then Return False

                    End If

                Else

                    If strlines(0) <> getMD5Hash(strComputerID + "AKB") Then

                        MessageBox.Show("Mã sản phẩm không phù hợp." + vbCrLf + "Xin vui lòng nhập lại mã sản phẩm tại màn hình sau đây.")
                        basCommon.fncDeleteFile(mstrActiveFile)

                        Dim frmActive As frmActiveKey = New frmActiveKey

                        'QUYET comment dong nay ▼
                        frmActive.Run(strComputerID, 2)


                        If frmActive.mblnActiveOk = False Then Return False

                    End If

                End If

            Else

                Dim frmActive As frmActiveKey = New frmActiveKey

                If blnShowWarningMsg Then fncMessageWarning("Bạn đang sử dụng phiên bản dùng thử. Hãy kích hoạt sản phẩm để sử dụng phiên bản đầy đủ.")

                'QUYET comment dong nay ▼
                frmActive.Run(strComputerID, 1)

                If frmActive.mblnActiveOk = False Then Return False

            End If

            Return True
        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncCheckActive", ex)

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xGetMemberImagePath
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMakeImage(ByVal strFile As String) As XImage
        fncMakeImage = Nothing
        Try

            If System.IO.File.Exists(strFile) Then

                Return XImage.FromFile(strFile)

            End If

        Catch ex As Exception

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncGetComputerID
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function fncGetComputerID(ByVal strKey As String) As String
        fncGetComputerID = ""
        Try

            Dim strGETCOMPUTERURL As String = "http://akb.com.vn/Giapha/ActiveKey.aspx?CID={0}&KEY={1}&Phone={2}&Name={3}&Birth={4}&Type=1"
            'Dim strGETCOMPUTERURL As String = "http://localhost:1272/GiaphaActive/ActiveKey.aspx?CID={0}&KEY={1}&Phone={2}&Name={3}&Birth={4}&Type=1"
            Dim strLink = String.Format(strGETCOMPUTERURL, " ", strKey, " ", " ", " ")

            Dim postData As String = ""

            ' Read the content.
            Dim responseFromServer As String = fncResponse(strLink, Config.Decrypt(gcstrServerPass))

            fncGetComputerID = responseFromServer

        Catch ex As Exception

            fncMessageWarning(ex.Message)

        End Try

    End Function


    ''' <summary>
    ''' Move data grid view row
    ''' </summary>
    ''' <param name="dgvData"></param>
    ''' <param name="intValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncMoveGridRow(ByVal dgvData As DataGridView, ByVal intValue As Integer) As Boolean
        fncMoveGridRow = False
        Try
            If dgvData.RowCount <= 0 Then Exit Function
            If dgvData.SelectedRows.Count <= 0 Then Exit Function

            Dim dtCurRow As DataGridViewRow = dgvData.SelectedRows(0)
            Dim intCurIndex As Integer = dtCurRow.Index
            Dim intNewIndex As Integer

            intNewIndex = intCurIndex - intValue
            If intNewIndex < 0 Or intNewIndex > dgvData.Rows.Count - 1 Then Exit Function

            dgvData.Rows.Remove(dtCurRow)
            dgvData.Rows.Insert(intNewIndex, dtCurRow)
            dtCurRow.Selected = True

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncMoveGridRow", ex)
        End Try
    End Function


    Public Function fncGetRootMemberInfoDisplay(ByVal stCard As stCardInfo) As String

        fncGetRootMemberInfoDisplay = ""

        If stCard.stBasicInfo.intGender = clsEnum.emGender.FEMALE Then
            Return "Bà " & stCard.stBasicInfo.strFullName.ToUpper.Replace(vbCrLf, " ") '& " (Đời " & CStr(intGen) & ")"
        ElseIf stCard.stBasicInfo.intGender = clsEnum.emGender.MALE Then
            Return "Ông " & stCard.stBasicInfo.strFullName.ToUpper.Replace(vbCrLf, " ") '& " (Đời " & CStr(intGen) & ")"

        End If

        Return "Thành viên " & stCard.stBasicInfo.strFullName.ToUpper.Replace(vbCrLf, " ") '& " (Đời " & CStr(intGen) & ")"

    End Function

    ''' <summary>
    ''' Get member' father/mother infor
    ''' </summary>
    ''' <param name="intMemID">member id</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Public Function fncGetFaMo(ByVal intMemID As Integer, ByVal emFaMo As clsEnum.emGender) As String

        fncGetFaMo = ""

        Try
            Dim intFaID As Integer
            Dim intMoID As Integer
            'Dim strFaName As String
            'Dim strMoName As String

            basCommon.fncGetFaMoID(intMemID, intFaID, intMoID)

            Select Case emFaMo
                Case clsEnum.emGender.MALE
                    'strFaName = basCommon.fncGetMemberName(intFaID)
                    If intFaID > basConst.gcintNO_MEMBER Then fncGetFaMo = basCommon.fncGetMemberName(intFaID)

                Case clsEnum.emGender.FEMALE
                    'strMoName = basCommon.fncGetMemberName(intMoID)
                    If intMoID > basConst.gcintNO_MEMBER Then fncGetFaMo = basCommon.fncGetMemberName(intMoID)

            End Select

            'xGetFaMo &= "Cha : " & vbTab & vbTab & strFaName & vbCrLf
            'xGetFaMo &= "Mẹ : " & vbTab & vbTab & strMoName

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncGetFaMo", ex)
        Finally
        End Try

    End Function

    ''' <summary>
    ''' Get member' father/mother infor
    ''' </summary>
    ''' <param name="intMemID">member id</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Public Function fncGetFaMoName(ByVal intMemID As Integer, ByRef strFatherName As String, ByRef strMotherName As String) As String

        fncGetFaMoName = ""

        Try
            Dim intFaID As Integer
            Dim intMoID As Integer
            'Dim strFaName As String
            'Dim strMoName As String

            basCommon.fncGetFaMo(intMemID, intFaID, intMoID, strFatherName, strMotherName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncGetFaMoName", ex)
        Finally
        End Try

    End Function

#Region "Generation"

    ''' <summary>
    ''' Fill all generation from begin member
    ''' </summary>
    ''' <param name="intStartMember">beginning member id</param>
    ''' <param name="intStartGeneration">beginning generation</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncSetGeneration(ByVal intStartMember As Integer, ByVal intStartGeneration As Integer) As Boolean

        fncSetGeneration = False

        Dim objSpouse As Dictionary(Of Integer, String) = Nothing
        Dim blnSucces As Boolean = True

        Try
            Dim blnStop As Boolean = False

            'intStartMember = basCommon.fncGetRoot()
            objSpouse = basCommon.fncGetHusWifeList(intStartMember)

            gobjDB.BeginTransaction()

            'reset all generation
            blnSucces = blnSucces And gobjDB.fncSetMemberGeneration(-1, 0, False)

            If intStartMember <= basConst.gcintNO_MEMBER Then Return True

            'set root generation
            blnSucces = blnSucces And gobjDB.fncSetMemberGeneration(intStartGeneration, intStartMember, False)

            objSpouse = fncGetHusWifeList(intStartMember)
            For Each element As KeyValuePair(Of Integer, String) In objSpouse
                blnSucces = blnSucces And gobjDB.fncSetMemberGeneration(intStartGeneration, element.Key, False)
            Next

            'set child generation
            For i As Integer = 0 To 45
                If blnStop Then Exit For
                blnSucces = blnSucces And gobjDB.fncSetGeneration(i, i + intStartGeneration + 1, intStartMember, blnStop, False)
            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncGetGeneration", ex, Nothing, False)
        Finally
            If blnSucces Then
                gobjDB.Commit()
            Else
                gobjDB.RollBack()
            End If
        End Try

    End Function


    ''' <summary>
    ''' Find someone is downline of someone
    ''' </summary>
    ''' <param name="intRootID">Top member</param>
    ''' <param name="intMemberID">Bottom member</param>
    ''' <param name="intSpouseID">Spouse of bottom member</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncIsDownlineOf(ByVal intRootID As Integer, ByVal intMemberID As Integer, Optional ByVal intSpouseID As Integer = -1) As Boolean

        fncIsDownlineOf = False

        Try
            Return gobjDB.fncIsDownlineOf(intRootID, intMemberID, intSpouseID)

        Catch ex As Exception
            Throw ex
        End Try

    End Function


#End Region

    Public Class clsComputerInfo

        Friend Function GetProcessorId() As String
            Dim strReturnVal As String = "0123456789"
            Try
                Dim strProcessorId As String = String.Empty
                Dim query As New SelectQuery("Win32_processor")
                Dim search As New ManagementObjectSearcher(query)
                Dim info As ManagementObject

                For Each info In search.Get()
                    strReturnVal = info("processorId").ToString()
                Next


            Catch ex As Exception

            End Try

            Return strReturnVal

        End Function

        Friend Function GetMACAddress() As String

            Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
            Dim moc As ManagementObjectCollection = mc.GetInstances()
            Dim MACAddress As String = String.Empty
            For Each mo As ManagementObject In moc

                If (MACAddress.Equals(String.Empty)) Then
                    If CBool(mo("IPEnabled")) Then MACAddress = mo("MacAddress").ToString()

                    mo.Dispose()
                End If
                MACAddress = MACAddress.Replace(":", String.Empty)

            Next
            Return MACAddress
        End Function

        Friend Function GetVolumeSerial(Optional ByVal strDriveLetter As String = "C") As String

            Dim strReturnVal As String = "0123456789"
            Try
                Dim disk As ManagementObject = New ManagementObject(String.Format("win32_logicaldisk.deviceid=""{0}:""", strDriveLetter))
                disk.Get()
                strReturnVal = disk("VolumeSerialNumber").ToString()


            Catch ex As Exception

            End Try
            Return strReturnVal
        End Function

        Friend Function GetMotherBoardID() As String

            Dim strMotherBoardID As String = String.Empty
            Dim query As New SelectQuery("Win32_BaseBoard")
            Dim search As New ManagementObjectSearcher(query)
            Dim info As ManagementObject
            For Each info In search.Get()

                strMotherBoardID = info("SerialNumber").ToString()

            Next
            Return strMotherBoardID

        End Function

    End Class


    Public Class clsListItem

        Private mstrText As String
        Private mobjTag As Object

        Public Property Tag() As Object
            Get
                Return Me.mobjTag
            End Get
            Set(ByVal value As Object)
                Me.mobjTag = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return Me.mstrText
            End Get
            Set(ByVal value As String)
                Me.mstrText = value
            End Set
        End Property

        Public Sub New(ByVal strText As String, ByVal objValue As Object)
            Me.Text = strText
            Me.Tag = objValue
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Text
        End Function

    End Class

#Region "Add by: 2018/04/24 AKB Nguyen Thanh Tung"

    Private Function fncGetSizeText(ByVal str As String, ByVal objFont As Font) As Size

        Dim rst As New Size

        If String.IsNullOrEmpty(str) Then Return rst

        Dim arrMaxName As String() = str.Split(vbNewLine)

        Using objPic As New Bitmap(1, 1)
            Using objGrap As Graphics = Graphics.FromImage(objPic)
                For Each chacter As String In arrMaxName

                    chacter = Replace(chacter, vbNewLine, String.Empty)

                    Dim sizeText As Size = objGrap.MeasureString(chacter, objFont).ToSize()

                    rst.Height = rst.Height + objFont.Height

                    If rst.Width < sizeText.Width Then
                        rst.Width = sizeText.Width
                    End If
                Next
            End Using
        End Using

        Return rst
    End Function

    Public Function fncModeShortCard(ByVal mtblCardInfo As Hashtable,
                                     ByVal mblnIsSmallCard As Boolean,
                                     ByRef intWCard As Integer,
                                     ByRef intHCard As Integer,
                                     ByRef mobjFont As Font) As Boolean

        fncModeShortCard = False

        Dim stCard As stCardInfo = Nothing
        Dim sizeText As Size
        Dim objFont As Font = My.Settings.objFontDefaut
        Dim intMaxWidth As Integer = 0
        Dim intMaxHeight As Integer = 0
        Dim strMaxName As String = String.Empty
        Dim intWidth As Integer = My.Settings.intHozSize
        Dim intHeight As Integer = My.Settings.intVerSize
        Dim intPading As Integer
        Dim upDown As Single = 0.15
        Dim ratePadding As Double = 0.025
        Dim strNameMaxWidth As String = String.Empty
        Dim strNameMaxHeight As String = String.Empty

        objFont = New Font(objFont.Name, objFont.Size, GraphicsUnit.Point)

        Try

            If My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Horizontal _
               AndAlso My.Settings.intTypeDrawText <> CInt(clsEnum.emTypeDrawText.Normal) Then

                For Each element As DictionaryEntry In mtblCardInfo

                    Dim cardName As String = fncGetDisplayShortTree(CType(mtblCardInfo.Item(element.Key), stCardInfo))
                    cardName = cardName.Replace(vbNewLine, " ")

                    sizeText = fncGetSizeText(cardName, objFont)

                    If sizeText.Width > intMaxWidth Then
                        intMaxWidth = sizeText.Width
                        strMaxName = cardName
                    End If
                Next

                ratePadding = ratePadding * 4

                'sizeText = fncGetSizeText(strMaxName, objFont)
                intPading = Math.Floor(objFont.Height * ratePadding)

                If objFont.Height + intPading > intWidth Then

                    While objFont.Height + intPading > intWidth

                        objFont = New Font(objFont.Name, objFont.Size - upDown, GraphicsUnit.Point)
                        intPading = Math.Floor(objFont.Height * ratePadding)

                    End While

                    mobjFont = objFont
                Else

                    While objFont.Height + intPading < intWidth

                        objFont = New Font(objFont.Name, objFont.Size + upDown, GraphicsUnit.Point)
                        intPading = Math.Floor(objFont.Height * ratePadding)

                    End While

                    mobjFont = New Font(objFont.Name, objFont.Size - upDown * 2, GraphicsUnit.Point)
                End If

                sizeText = fncGetSizeText(strMaxName, mobjFont)

                intWCard = intWidth
                intHCard = sizeText.Width

                Return True
            End If

            For Each element As DictionaryEntry In mtblCardInfo

                Dim cardName As String = fncGetDisplayShortTree(CType(mtblCardInfo.Item(element.Key), stCardInfo))

                If My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Vertical Then

                    sizeText = fncGetSizeText(cardName, objFont)

                    If sizeText.Height > intMaxHeight Then
                        intMaxHeight = sizeText.Height
                        strNameMaxHeight = cardName
                    End If

                    If sizeText.Width > intMaxWidth Then
                        intMaxWidth = sizeText.Width
                        strNameMaxWidth = cardName
                    End If
                Else

                    sizeText = fncGetSizeText(cardName, objFont)

                    If sizeText.Width > intMaxWidth Then
                        intMaxWidth = sizeText.Width
                        strMaxName = cardName
                    End If
                End If
            Next

            If My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Horizontal Then

                sizeText = fncGetSizeText(strMaxName, objFont)
                intPading = Math.Floor(sizeText.Width * ratePadding)

                If sizeText.Width + intPading > intWidth Then

                    While sizeText.Width + intPading > intWidth

                        objFont = New Font(objFont.Name, objFont.Size - upDown, GraphicsUnit.Point)
                        sizeText = fncGetSizeText(strMaxName, objFont)
                        intPading = Math.Floor(sizeText.Width * ratePadding)

                    End While

                    mobjFont = objFont
                Else

                    While sizeText.Width + intPading < intWidth

                        objFont = New Font(objFont.Name, objFont.Size + upDown, GraphicsUnit.Point)
                        sizeText = fncGetSizeText(strMaxName, objFont)
                        intPading = Math.Floor(sizeText.Width * ratePadding)

                    End While

                    mobjFont = New Font(objFont.Name, objFont.Size - upDown, GraphicsUnit.Point)
                End If

                intWCard = intWidth
                intHCard = sizeText.Height + intPading

            Else

                strMaxName = strNameMaxWidth

                Dim arrNameMaxHeight As String() = strNameMaxHeight.Split(vbNewLine)
                Dim arrNameMaxWidth As String() = strNameMaxWidth.Split(vbNewLine)
                Dim intCountAdd As Integer = arrNameMaxHeight.Length - arrNameMaxWidth.Length

                For i As Integer = 0 To intCountAdd - 1
                    strMaxName &= vbNewLine
                Next

                sizeText = fncGetSizeText(strMaxName, objFont)
                ratePadding = ratePadding * 4
                intPading = Math.Floor(sizeText.Height * ratePadding)

                If sizeText.Height + intPading > intHeight Then

                    While sizeText.Height + intPading > intHeight

                        objFont = New Font(objFont.Name, objFont.Size - upDown, GraphicsUnit.Point)
                        sizeText = fncGetSizeText(strMaxName, objFont)
                        intPading = Math.Floor(sizeText.Height * ratePadding)

                    End While

                    mobjFont = objFont
                Else

                    While sizeText.Height + intPading < intHeight

                        objFont = New Font(objFont.Name, objFont.Size + upDown, GraphicsUnit.Point)
                        sizeText = fncGetSizeText(strMaxName, objFont)
                        intPading = Math.Floor(sizeText.Height * ratePadding)

                    End While

                    mobjFont = New Font(objFont.Name, objFont.Size - upDown, GraphicsUnit.Point)
                End If

                intHCard = intHeight
                intWCard = sizeText.Width + 3
            End If

            Return True
        Catch ex As Exception

        Finally
            mtblCardInfo = Nothing
            stCard = Nothing
            sizeText = Nothing
        End Try

        'If My.Settings.intSelectedTypeCardShort = CInt(clsEnum.emTypeCardShort.Horizontal) Then

        '    intWCard = 130
        '    intHCard = 30

        'Else

        '    intWCard = 60
        '    intHCard = 100

        'End If

        Return True
    End Function

    Public Function fncGetDisplayShortTree(ByVal stCard As stCardInfo) As String

        fncGetDisplayShortTree = String.Empty

        If IsNothing(stCard) Then Exit Function

        Dim strLevelShow As String = String.Empty
        Dim strFullName As String = String.Empty
        Dim strGenderShow As String = String.Empty
        Dim strName As String = String.Empty
        With stCard

            If My.Settings.blnShowLevel AndAlso .intLevel > 0 Then strLevelShow = "Đời " & .intLevel

            strFullName = fncGetFullName(.stBasicInfo.strFirstName,
                                         .stBasicInfo.strMidName,
                                         .stBasicInfo.strLastName,
                                         .stBasicInfo.strAlias)

            If My.Settings.intSelectedTypeShowTree <> CInt(clsEnum.emTypeShowTree.OnlyShowMale) Then
                If My.Settings.blnShowGender Then strGenderShow = fncGetGenderShow(.stBasicInfo.intGender)
            End If

            If Not String.IsNullOrEmpty(strGenderShow) Then strGenderShow = "(" & strGenderShow & ")"

            If My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Vertical Then

                strName = (strFullName & "").Trim()
                strName = strName.Replace("  ", " ")

                Dim arrName() As String = (strName & "").Split({" "}, StringSplitOptions.RemoveEmptyEntries)

                If Not IsNothing(arrName) AndAlso arrName.Length > 0 Then

                    strName = String.Join(vbNewLine, arrName)

                End If

                strName = basCommon.fncAddSpecial(vbNewLine, strLevelShow, strName, strGenderShow)

                'objCard.lblName.Dock = DockStyle.Fill
                'objCard.lblName.AutoSize = False
            Else

                If Not String.IsNullOrEmpty(strLevelShow) Then

                    strLevelShow &= " " & strGenderShow
                    strName &= fncAddSpecial(vbNewLine, strLevelShow, strFullName)

                Else

                    strName &= fncAddSpecial(vbNewLine, strFullName, strGenderShow)

                End If
            End If
        End With

        Return strName
    End Function

    Public Function fncMakeCardInfoType2(ByVal stCard As stCardInfo, ByVal blnSmall As Boolean) As usrMemberCard1

        fncMakeCardInfoType2 = Nothing

        Dim objCard As usrMemberCard1 = New usrMemberCard1(stCard.intID, blnSmall)

        objCard.Location = New Point(stCard.intX, stCard.intY)
        objCard.picMember.Visible = False
        objCard.lblName.TextAlign = ContentAlignment.MiddleCenter
        'objCard.lblName.AutoSize = True
        objCard.BorderStyle = BorderStyle.FixedSingle
        objCard.BackgroundImage = Nothing
        objCard.lblName.Dock = DockStyle.Fill

        objCard.CardName = fncGetDisplayShortTree(stCard)
        objCard.CardID = stCard.intID
        objCard.Name = CStr(stCard.intID)

        objCard.UseRotateText = My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Horizontal _
                                AndAlso My.Settings.intTypeDrawText <> CInt(clsEnum.emTypeDrawText.Normal)
        Return objCard
    End Function

    '   ******************************************************************
    '		FUNCTION   : fncBindingComboBox
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - ComboBox
    '                    ARG2(IN) - List(Of clsDataSourceComboBox)
    '                    ARG3(Optional) - Object
    '		MEMO       : 
    '		CREATE     :2018/03/13 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncBindingComboBox(ByVal cbo As ComboBox,
                                       ByVal lstData As List(Of clsDataSourceComboBox),
                                       Optional selectValue As Object = Nothing) As Boolean

        fncBindingComboBox = False

        Try

            cbo.Items.Clear()
            cbo.DisplayMember = "Display"
            cbo.ValueMember = "Value"
            cbo.DataSource = lstData

            If Not IsNothing(selectValue) Then
                If cbo.Items.Count > 0 Then cbo.SelectedValue = selectValue
            End If

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncBindingComboBox", ex)
        End Try
    End Function

    Public Function fncBindingComboBox(ByVal cbo As ToolStripComboBox,
                                       ByVal lstData As List(Of clsDataSourceComboBox),
                                       Optional selectValue As Object = Nothing) As Boolean

        fncBindingComboBox = False

        Try

            cbo.Items.Clear()
            cbo.ComboBox.DisplayMember = "Display"
            cbo.ComboBox.ValueMember = "Value"
            cbo.ComboBox.DataSource = lstData

            If Not IsNothing(selectValue) Then
                If cbo.Items.Count > 0 Then cbo.ComboBox.SelectedValue = selectValue
            End If

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncBindingComboBox", ex)
        End Try
    End Function

    Public Function fncBindingComboBox(ByVal cbo As ComboBox,
                                      ByVal tbl As DataTable,
                                      ByVal strDisplay As String,
                                      ByVal strValue As String,
                                      Optional selectValue As Object = Nothing) As Boolean

        fncBindingComboBox = False

        Try

            cbo.DisplayMember = strDisplay
            cbo.ValueMember = strValue
            cbo.DataSource = tbl

            If Not IsNothing(selectValue) Then
                If cbo.Items.Count > 0 Then cbo.SelectedValue = selectValue
            End If

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncBindingComboBox", ex)
        End Try
    End Function

    Public Function fncAddSpecial(ByVal strSpeacial As String, ParamArray arrInput() As String) As String

        If IsNothing(arrInput) Then Return String.Empty

        Dim intCount As Integer = UBound(arrInput)
        Dim lstReturn As New List(Of String)

        For i As Integer = 0 To intCount

            If String.IsNullOrEmpty(arrInput(i)) Then Continue For

            lstReturn.Add(arrInput(i).Trim())
        Next

        Return String.Join(strSpeacial, lstReturn)
    End Function

    Public Function fncGetGenderShow(ByVal intGender As Integer,
                                     Optional ByVal defaultValue As String = "")

        If intGender = CInt(clsEnum.emGender.MALE) Then

            Return basConst.gcstrGenderMALE

        End If

        If intGender = CInt(clsEnum.emGender.FEMALE) Then

            Return basConst.gcstrGenderFEMALE

        End If

        Return defaultValue
    End Function
#End Region

    '   ******************************************************************
    '　　　FUNCTION   : fncLoadInfoVersion
    '      MEMO       : Load info version
    '      CREATE     : 2018/10/08 AKB Nguyen Thanh Tung
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub fncLoadInfoVersion()
        Dim strActiveFile As String = Application.StartupPath + "\Activekey.txt"
        basConst.gcintMaxLimit = basConst.gcintMaxLimit500

        Try
            If Not System.IO.File.Exists(strActiveFile) Then
                Exit Sub
            End If

            Dim strlines() As String = My.Computer.FileSystem.ReadAllText(strActiveFile).Replace(vbLf, "").Split(CChar(vbCr))

            If strlines.Length < 2 Then
                Exit Sub
            End If

            Dim strKey As String = strlines(1)

            If String.IsNullOrEmpty(strKey) Then
                Exit Sub
            End If

            If strKey.StartsWith(basConst.gcstrVersion1000) Then
                basConst.gcintMaxLimit = basConst.gcintMaxLimit1000
            ElseIf strKey.StartsWith(basConst.gcstrVersionUltimate) Then
                basConst.gcintMaxLimit = basConst.gcintMaxLimitUltimate
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function GetTitleApp() As String

        Dim strInfo As String = "Phiên bản dùng thử"
        Dim strAppVer As String = "Version " & Assembly.GetExecutingAssembly().GetName().Version.ToString()

        If gblnActivated Then

            If basConst.gcintMaxLimit = basConst.gcintMaxLimitUltimate Then
                strInfo = "Phiên bản không giới hạn thành viên"
            Else
                strInfo = String.Format("Phiên bản {0} thành viên", basConst.gcintMaxLimit.ToString("N0"))
            End If
        End If

        Dim filePath = Assembly.GetExecutingAssembly().Location
        Const c_PeHeaderOffset As Integer = 60
        Const c_LinkerTimestampOffset As Integer = 8
        Dim buffer = New Byte(2047) {}

        Using stream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
            stream.Read(buffer, 0, 2048)
        End Using

        Dim offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset)
        Dim secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset)
        Dim epoch = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        Dim linkTimeUtc = epoch.AddSeconds(secondsSince1970)
        Dim tz = If(Nothing, TimeZoneInfo.Local)
        Dim localTime As Date = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz)

        Return String.Format("Chương trình quản lý Gia Phả - {0} - {1} (Cập nhập ngày {2})", strInfo, strAppVer, localTime.ToString("dd/MM/yyyy"))
    End Function

    Public Function ComfirmMsg(ByVal strMsg As String, ByVal Optional strTitle As String = "Phần mềm quản lý gia phả") As Boolean
        Return MessageBox.Show(strMsg, strTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK
    End Function

    Public Function ShowMsg(ByVal type As MessageBoxIcon, ByVal strMsg As String, ByVal Optional strTitle As String = "Phần mềm quản lý gia phả") As Boolean
        MessageBox.Show(strMsg, strTitle, MessageBoxButtons.OK, type)

        If type = MessageBoxIcon.[Error] OrElse type = MessageBoxIcon.Warning Then
            Return False
        End If

        Return True
    End Function

    Public Function UpVersionApp() As Boolean

        UpVersionApp = False

        Using frmActive As frmActiveKey = New frmActiveKey
            frmActive.Label10.Visible = False
            frmActive.Label5.AutoSize = False
            frmActive.Label5.Text = "Nâng Cấp Phiên Bản Sản Phẩm"
            frmActive.Label5.AutoSize = True
            frmActive.Label5.Left = (frmActive.Width - frmActive.Label5.Width) / 2
            frmActive.Height = 300
            frmActive.btnTrial.Visible = False
            frmActive.btnActive.Left = (frmActive.Width - frmActive.btnActive.Width) / 2

            For Each objControl As Control In frmActive.Controls
                objControl.Top = objControl.Top - 100
            Next

            Dim strComputerID As String
            Dim Computer As New clsComputerInfo
            strComputerID = Computer.GetVolumeSerial + Computer.GetProcessorId
            frmActive.Run(strComputerID, 2)
            Return frmActive.mblnActiveOk
        End Using
    End Function

    Public Function CallSaveDialog(ByRef pathSave As String, Optional ByVal titleSave As String = "Save as") As Boolean

        Using saveDialog As New SaveFileDialog

            With saveDialog
                .DefaultExt = "docx"
                .Filter = "Word (*.docx)|*.docx"
                .AddExtension = True
                .RestoreDirectory = True
                .Title = titleSave
                .InitialDirectory = Application.ExecutablePath
            End With

            If saveDialog.ShowDialog() <> DialogResult.OK Then
                Return False
            End If

            pathSave = saveDialog.FileName

            Return True
        End Using
    End Function
End Module
