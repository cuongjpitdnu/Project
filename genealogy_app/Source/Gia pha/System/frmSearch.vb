Option Explicit On
Option Strict On
'   ******************************************************************
'      TITLE      : Search
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/08/11　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Imports Config_Gia_Pha

'   ******************************************************************
'　　　FUNCTION   : Search Class
'      MEMO       : 
'      CREATE     : 2011/08/11  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmSearch


#Region "Constant"

    Private Const mcstrClsName As String = "frmSearch"                                      'class name
    Private Const mcstrNoResult As String = "Không tìm thấy kết quả."                             'no result message
    Private Const mcstrNoData As String = "Không có dữ liệu để in."                                'no data to print
    Private Const mcintDateCol As Integer = 9                                               'column to insert date in excel file
    Private Const mcintDateRow As Integer = 2                                               'row to insert data in excel file
    Private Const mcintItemPerPage As Integer = 100                                         'item per page

#End Region


#Region "Variable"

    Private mfrmLunarCal As frmCalendarVN                                           'Lunar calendar form

    Private mstSearchInfo As clsDbAccess.stSearch                                   'Search information
    Private mstResult As stSearchResult                                             'Search results
    Private mstDieFrom As stCalendar                                    'structure that stores date infor
    Private mstDieTo As stCalendar                                      'structure that stores date infor

    Private mtblLevel As DataTable                                                  'datatable from main form
    Private mtblSearchData As DataTable
    Private mtblDataSource As DataTable

    Private mintCurPage As Integer                                                  'currently showing page
    Private mintTotalPage As Integer                                                'total of page

#End Region


#Region "Structure"


    '   ******************************************************************
    '　　　FUNCTION   : Search Result Structure
    '      MEMO       : 
    '      CREATE     : 2011/08/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Structure stSearchResult

        Dim intMemID As Integer                     'mem ID
        Dim intLevel As Integer                     'level/generation

        Dim strLastName As String                   'last name
        Dim strMidName As String                    'middle name
        Dim strFirstName As String                  'first name
        Dim strAlias As String                      'alias
        Dim strAddr As String                       'home address

        'Dim dtBirth As Date                         'date of birth
        Dim intBday As Integer
        Dim intBmon As Integer
        Dim intByea As Integer

        Dim intDday As Integer
        Dim intDmon As Integer
        Dim intDyea As Integer

        Dim strPhone1 As String                     'phone 1
        Dim strPhone2 As String                     'phone 2

        Dim strMail1 As String                      'email 1
        Dim strMail2 As String                      'email 2
        Dim strURL As String                        'URL
        Dim strIM As String                         'IM
        Dim strRemark As String                     'remark

        Dim intDie As Integer                       'deceased or not
        Dim intGender As Integer                    'gender

        'Dim strBirthPlace As String
        Dim strBurryPlace As String
        Dim strHometown As String

    End Structure


#End Region


#Region "Class Constructor"

    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/08/11  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New(ByVal tblLevel As DataTable)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'new lunar calendar form
        mfrmLunarCal = New frmCalendarVN()
        Me.mtblLevel = tblLevel

    End Sub

#End Region


#Region "Control's events"

    '   ******************************************************************
    '　　　FUNCTION   : frmSearch_Load
    '      MEMO       : 
    '      CREATE     : 2011/08/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            'set default selected
            cbGender.SelectedIndex = 0
            cboPosLevelName.SelectedIndex = 0
            mintCurPage = 1

            'hide tab page
            tbcSearch.Controls.Remove(tbpMoreInfo)

            'create showing datatable
            mtblSearchData = New DataTable()
            mtblSearchData.Columns.Add("STT", GetType(Integer))
            mtblSearchData.Columns.Add("GEN", GetType(Int32))
            mtblSearchData.Columns.Add("NAME")
            mtblSearchData.Columns.Add("GENDER")
            mtblSearchData.Columns.Add("CONTACT")
            mtblSearchData.Columns.Add("HOME")
            mtblSearchData.Columns.Add("BIRTH")
            mtblSearchData.Columns.Add("DECEASE")
            mtblSearchData.Columns.Add("REMARK_TEMP")
            mtblSearchData.Columns.Add("REMARK")
            mtblSearchData.Columns.Add("DIED")


            mtblDataSource = New DataTable()
            mtblDataSource.Columns.Add("STT", GetType(Integer))
            mtblDataSource.Columns.Add("GEN", GetType(Int32))
            mtblDataSource.Columns.Add("NAME")
            mtblDataSource.Columns.Add("GENDER")
            mtblDataSource.Columns.Add("CONTACT")
            mtblDataSource.Columns.Add("HOME")
            mtblDataSource.Columns.Add("BIRTH")
            mtblDataSource.Columns.Add("DECEASE")
            mtblDataSource.Columns.Add("REMARK_TEMP")
            mtblDataSource.Columns.Add("REMARK")
            mtblDataSource.Columns.Add("DIED")

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmSearch_Load", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : chkDie_CheckedChanged, checkbox checked
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub chkDie_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDie.CheckedChanged

        Try
            Dim blnEn As Boolean

            blnEn = chkDie.Checked

            'set enable for Die controls
            btnDieFrom.Enabled = blnEn
            btnDieTo.Enabled = blnEn
            dtpDieFrom.Enabled = blnEn
            dtpDieTo.Enabled = blnEn
            btnDeaFrom.Enabled = blnEn
            btnDeaTo.Enabled = blnEn
            lblDeaFrom.Enabled = blnEn
            lblDeaTo.Enabled = blnEn

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "chkDie_CheckedChanged", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dtpBirthFrom_ValueChanged
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dtpBirthFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpBirthFrom.ValueChanged

        Try
            'show lunar date string
            basCommon.fncShowLunarDate(mfrmLunarCal, dtpBirthFrom, lblBirthFrom, False)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dtpBirthFrom_ValueChanged", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dtpBirthTo_ValueChanged
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dtpBirthTo_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpBirthTo.ValueChanged

        Try
            'show lunar date string
            basCommon.fncShowLunarDate(mfrmLunarCal, dtpBirthTo, lblBirthTo, False)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dtpBirthTo_ValueChanged", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dtpDieFrom_ValueChanged
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dtpDieFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpDieFrom.ValueChanged

        Try
            'show lunar date string
            basCommon.fncShowLunarDate(mfrmLunarCal, dtpDieFrom, lblDieFrom, False)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dtpDieFrom_ValueChanged", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dtpDieTo_ValueChanged
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dtpDieTo_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpDieTo.ValueChanged

        Try
            'show lunar date string
            basCommon.fncShowLunarDate(mfrmLunarCal, dtpDieTo, lblDieTo, False)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dtpDieTo_ValueChanged", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnBirthFrom_Click
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnBirthFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBirthFrom.Click

        Try
            'show Lunar form
            basCommon.fncShowLunarDate(mfrmLunarCal, dtpBirthFrom, lblBirthFrom, True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnBirthFrom_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnBirthTo_Click
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnBirthTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBirthTo.Click

        Try
            'show Lunar form
            basCommon.fncShowLunarDate(mfrmLunarCal, dtpBirthTo, lblBirthTo, True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnBirthTo_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnDieFrom_Click
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnDieFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDieFrom.Click

        Try
            'show Lunar form
            basCommon.fncShowLunarDate(mfrmLunarCal, dtpDieFrom, lblDieFrom, True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDieFrom_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnDieTo_Click
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnDieTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDieTo.Click

        Try
            'show Lunar form
            basCommon.fncShowLunarDate(mfrmLunarCal, dtpDieTo, lblDieTo, True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDieTo_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnSearch_Click
    '      MEMO       : 
    '      CREATE     : 2011/08/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim tblData As DataTable = Nothing

        Try
            'get search data
            xGetSearchData()

            'clear grid
            'dgvSearchMember.Rows.Clear()
            mtblSearchData.Clear()

            'do searching
            If Not xSearch(tblData) Then Exit Sub

            'fill grid
            xFillData(tblData)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSearch_Click", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnPrint_Click
    '      MEMO       : 
    '      CREATE     : 2011/08/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Try
            Dim strFileName As String = String.Empty

            'check for printable data
            If dgvSearchMember.Rows.Count < 1 Then

                basCommon.fncMessageWarning(mcstrNoData, txtKeyword)
                Exit Sub

            End If

            xOpenPrintPreview()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnPrint_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click
    '      MEMO       : 
    '      CREATE     : 2011/08/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try

            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmSearch_FormClosing
    '      MEMO       : 
    '      CREATE     : 2011/08/11  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmSearch_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Try
            'clear
            If mfrmLunarCal IsNot Nothing Then mfrmLunarCal.Dispose()
            mstResult = Nothing
            mstSearchInfo = Nothing

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try

    End Sub


    ''' <summary>
    ''' DEA-FROM Button Even
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDeaFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeaFrom.Click
        Try
            basCommon.fncSelectCal(mstDieFrom, frmCalendar.emCalendar.LUNAR, mstSearchInfo.intDFday, mstSearchInfo.intDFmon, mstSearchInfo.intDFyea, True, lblDeaFrom)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDeaFrom_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' DEA-TO Button Even
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDeaTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeaTo.Click

        Try
            basCommon.fncSelectCal(mstDieTo, frmCalendar.emCalendar.LUNAR, mstSearchInfo.intDTday, mstSearchInfo.intDTmon, mstSearchInfo.intDTyea, True, lblDeaTo)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDeaTo_Click", ex)
        End Try

    End Sub


    ''' <summary>
    ''' Refill STT while data grid is sorted
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvSearchMember_Sorted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvSearchMember.Sorted

        Try
            Dim intStart As Integer

            intStart = (mintCurPage - 1) * mcintItemPerPage

            For i As Integer = 0 To Me.dgvSearchMember.RowCount - 1
                Me.dgvSearchMember.Item(clsEnum.SearchGrid.NO, i).Value = i + 1 + intStart
            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvSearchMember_Sorted", ex)
        End Try

    End Sub


    ''' <summary>
    ''' btnFirstPage_Click, button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFirstPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirstPage.Click
        Try
            If mintCurPage <= 1 Then Exit Sub

            xFillDatasource(1)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnFirstPage_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' btnPrePage_Click, button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrePage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrePage.Click
        Try
            If mintCurPage <= 1 Then Exit Sub

            xFillDatasource(mintCurPage - 1)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnPrePage_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' btnNextPage_Click, button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNextPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNextPage.Click
        Try
            If mintCurPage >= mintTotalPage Then Exit Sub

            xFillDatasource(mintCurPage + 1)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnNextPage_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' btnLastPage_Click, button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLastPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLastPage.Click
        Try
            If mintCurPage >= mintTotalPage Then Exit Sub

            xFillDatasource(mintTotalPage)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnFirstPage_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' cbPages_SelectedIndexChanged, button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cbPages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPages.SelectedIndexChanged
        Try
            Dim intSelectedPage As Integer
            intSelectedPage = cbPages.SelectedIndex + 1

            If intSelectedPage = mintCurPage Then Exit Sub

            xFillDatasource(intSelectedPage)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "cbPages_SelectedIndexChanged", ex)
        End Try
    End Sub


    ''' <summary>
    ''' cbPages_KeyPress, button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cbPages_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbPages.KeyPress
        Try
            'do nothing if it is not ENTER key
            If e.KeyChar <> Convert.ToChar(Keys.Enter) Then Exit Sub

            'exit if inputed text is not a number
            If Not IsNumeric(cbPages.Text.Trim) Then Exit Sub

            'try to get the page
            Dim intPage As Integer
            Integer.TryParse(cbPages.Text.Trim(), intPage)

            'exit if the input number is out of bound
            If intPage <= 0 Or intPage > mintTotalPage Then Exit Sub

            're-fill grid
            xFillDatasource(intPage)
            cbPages.SelectAll()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "cbPages_KeyPress", ex)
        End Try
    End Sub

#End Region


#Region "Class methods and function"


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show this form
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm() As Boolean

        fncShowForm = False

        Try

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xSearch, make a search
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       :  
    '      CREATE     : 2011/08/11  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xSearch(ByRef tblData As DataTable) As Boolean

        xSearch = False

        Try

            'do searching
            tblData = gobjDB.fncGetSearch(mstSearchInfo)

            'if there is no data
            If tblData Is Nothing Then
                basCommon.fncMessageWarning(mcstrNoResult, txtKeyword)
                Exit Function
            End If

            'remove duplicated row
            Dim strCols(tblData.Columns.Count - 1) As String

            For index As Integer = 0 To tblData.Columns.Count - 1
                strCols(index) = tblData.Columns(index).ColumnName
            Next

            tblData = tblData.DefaultView.ToTable(True, strCols)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSearch_Click", ex)
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xFillGrid, fill data to grid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : tblData DataTable, 
    '      MEMO       :  
    '      CREATE     : 2011/08/11  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xFillData(ByVal tblData As DataTable) As Boolean

        xFillData = False

        Dim strContent(10) As String
        Dim intAge As Integer

        Try
            For i As Integer = 0 To tblData.Rows.Count - 1

                With mstResult

                    'get data from specific row in datatable
                    xGetRowData(tblData, i)

                    'clear array before use it
                    Array.Clear(strContent, 0, strContent.Length)

                    'row number
                    strContent(clsEnum.SearchGrid.NO) = basCommon.fncCnvNullToString(i + 1)

                    'level
                    'strContent(clsEnum.SearchGrid.LEVEL) = ""
                    If .intLevel > 0 Then strContent(clsEnum.SearchGrid.LEVEL) = .intLevel.ToString()

                    'name
                    strContent(clsEnum.SearchGrid.NAME) = basCommon.fncGetFullName(.strFirstName, .strMidName, .strLastName, .strAlias)

                    'gender
                    strContent(clsEnum.SearchGrid.GENDER) = basConst.gcstrGenderUNKNOW
                    If .intGender = clsEnum.emGender.MALE Then strContent(clsEnum.SearchGrid.GENDER) = basConst.gcstrGenderMALE
                    If .intGender = clsEnum.emGender.FEMALE Then strContent(clsEnum.SearchGrid.GENDER) = basConst.gcstrGenderFEMALE

                    'contact information
                    strContent(clsEnum.SearchGrid.CONTACT) = ""
                    If Not basCommon.fncIsBlank(.strAddr) Then strContent(clsEnum.SearchGrid.CONTACT) = .strAddr & vbCrLf
                    If Not basCommon.fncIsBlank(.strPhone1) Then strContent(clsEnum.SearchGrid.CONTACT) &= "Phone 1 : " & .strPhone1 & vbCrLf
                    If Not basCommon.fncIsBlank(.strPhone2) Then strContent(clsEnum.SearchGrid.CONTACT) &= "Phone 2 : " & .strPhone2 & vbCrLf
                    If Not fncIsBlank(.strMail1) Then strContent(clsEnum.SearchGrid.CONTACT) &= "Email 1 : " & .strMail1 & vbCrLf
                    If Not fncIsBlank(.strMail2) Then strContent(clsEnum.SearchGrid.CONTACT) &= "Email 2 : " & .strMail2 & vbCrLf
                    If Not fncIsBlank(.strURL) Then strContent(clsEnum.SearchGrid.CONTACT) &= .strURL & vbCrLf
                    If Not fncIsBlank(.strIM) Then strContent(clsEnum.SearchGrid.CONTACT) &= .strMail2
                    strContent(clsEnum.SearchGrid.CONTACT) = strContent(clsEnum.SearchGrid.CONTACT).Trim()

                    'hometown
                    strContent(clsEnum.SearchGrid.HOMETOWN) = .strHometown

                    'birth date
                    strContent(clsEnum.SearchGrid.B_DATE) = basCommon.fncGetDateName("", .intBday, .intBmon, .intByea, True)

                    'decease date
                    strContent(clsEnum.SearchGrid.D_DATE) = basCommon.fncGetDateName("", .intDday, .intDmon, .intDyea, True, True)

                    'remark temp - not shown
                    strContent(clsEnum.SearchGrid.NOTE_TEMP) = ""
                    If Not fncIsBlank(.strBurryPlace) Then strContent(clsEnum.SearchGrid.NOTE_TEMP) = "Nơi an táng : " & .strBurryPlace & vbCrLf

                    intAge = .intDyea - .intByea
                    If intAge > 0 Then strContent(clsEnum.SearchGrid.NOTE_TEMP) &= "Thọ : " & intAge & " tuổi" & vbCrLf
                    strContent(clsEnum.SearchGrid.NOTE_TEMP) &= .strRemark
                    strContent(clsEnum.SearchGrid.NOTE_TEMP) = strContent(clsEnum.SearchGrid.NOTE_TEMP).Trim()

                    'remark - shown
                    strContent(clsEnum.SearchGrid.NOTE) = strContent(clsEnum.SearchGrid.NOTE_TEMP)
                    If Not basCommon.fncIsBlank(strContent(clsEnum.SearchGrid.NOTE)) And strContent(clsEnum.SearchGrid.NOTE).Length > 100 Then
                        strContent(clsEnum.SearchGrid.NOTE) = strContent(clsEnum.SearchGrid.NOTE).Substring(0, 100) + "..."
                    End If

                    strContent(clsEnum.SearchGrid.DIED) = "0"
                    If .intDie = basConst.gcintDIED Then strContent(clsEnum.SearchGrid.DIED) = "1"

                    'add to grid
                    'dgvSearchMember.Rows.Add(strContent)
                    mtblSearchData.Rows.Add(strContent)

                    'set color for row if deceased
                    'If .intDie = basConst.gcintDIED Then
                    '    dgvSearchMember.Rows(i).DefaultCellStyle.BackColor = Color.LightGray
                    'End If

                End With

            Next

            'dgvSearchMember.AutoGenerateColumns = False
            'dgvSearchMember.DataSource = mtblSearchData

            mintTotalPage = CInt(Math.Ceiling(mtblSearchData.Rows.Count / mcintItemPerPage))
            basCommon.fncMakeCbPage(mintTotalPage, cbPages)
            xFillDatasource(1)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillData", ex)
        Finally
            Erase strContent
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xFillDatasource, fill datasource
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : tblData DataTable, 
    '      MEMO       :  
    '      CREATE     : 2011/08/11  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xFillDatasource(ByVal intPage As Integer) As Boolean
        xFillDatasource = False

        Try
            Dim intStart As Integer
            Dim intEnd As Integer

            mintCurPage = intPage
            If mtblSearchData Is Nothing Then Exit Function

            'calculate for start and end row
            intStart = (mintCurPage - 1) * mcintItemPerPage
            intEnd = mintCurPage * mcintItemPerPage - 1
            If intEnd > mtblSearchData.Rows.Count Then intEnd = mtblSearchData.Rows.Count - 1

            'clear datasource before refilling
            mtblDataSource.Rows.Clear()

            'add row to datasource
            For index As Integer = intStart To intEnd
                mtblDataSource.ImportRow(mtblSearchData.Rows(index))
            Next

            dgvSearchMember.AutoGenerateColumns = False
            dgvSearchMember.DataSource = mtblDataSource
            cbPages.SelectedIndex = mintCurPage - 1

            'fill STT
            dgvSearchMember_Sorted(Nothing, Nothing)

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillDatasource", ex)
        End Try
    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xGetRowData, get data at specific row
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : tblData DataTable, 
    '                 : intRow Integer, row to get data
    '      MEMO       :  
    '      CREATE     : 2011/08/11  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xGetRowData(ByVal tblData As DataTable, ByVal intRow As Integer) As Boolean

        xGetRowData = False

        Try

            'get data from specific row
            With tblData.Rows(intRow)

                'member id
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("MEMBER_ID")), mstResult.intMemID)
                mstResult.intLevel = xGetLevel(mstResult.intMemID)

                'full name and address
                mstResult.strLastName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))
                mstResult.strMidName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                mstResult.strFirstName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))
                mstResult.strAlias = basCommon.fncCnvNullToString(.Item("ALIAS_NAME"))
                mstResult.strAddr = basCommon.fncCnvNullToString(.Item("HOME_ADD"))

                'date of birth
                'Date.TryParse(basCommon.fncCnvNullToString(.Item("BIRTH_DAY")), mstResult.dtBirth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY")), mstResult.intBday)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON")), mstResult.intBmon)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA")), mstResult.intByea)

                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_DAY")), mstResult.intDday)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_MON")), mstResult.intDmon)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_YEA")), mstResult.intDyea)

                'phone number
                mstResult.strPhone1 = basCommon.fncCnvNullToString(.Item("PHONENUM1"))
                mstResult.strPhone2 = basCommon.fncCnvNullToString(.Item("PHONENUM2"))

                'other contacts and remark
                mstResult.strMail1 = basCommon.fncCnvNullToString(.Item("MAIL_ADD1"))
                mstResult.strMail2 = basCommon.fncCnvNullToString(.Item("MAIL_ADD2"))
                mstResult.strURL = basCommon.fncCnvNullToString(.Item("URL"))
                mstResult.strIM = basCommon.fncCnvNullToString(.Item("IMNICK"))
                mstResult.strRemark = basCommon.fncCnvRtfToText(basCommon.fncCnvNullToString(.Item("REMARK")))

                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DECEASED")), mstResult.intDie)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("GENDER")), mstResult.intGender)

                'mstResult.strBirthPlace = basCommon.fncCnvNullToString(.Item("BIRTH_PLACE"))
                mstResult.strBurryPlace = basCommon.fncCnvNullToString(.Item("BURY_PLACE"))
                mstResult.strHometown = basCommon.fncCnvNullToString(.Item("HOMETOWN"))

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetRowData", ex)
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xSearch, make a search
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       :  
    '      CREATE     : 2011/08/11  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xGetSearchData() As Boolean

        xGetSearchData = False

        Try
            'reset structure
            mstSearchInfo = Nothing

            With mstSearchInfo

                'get keyword
                .strKeyword = txtKeyword.Text.Trim()

                'gender
                .intGender = cbGender.SelectedIndex

                'from birth date - to birth date
                If dtpBirthFrom.Checked Then .dtBirthFrom = dtpBirthFrom.Value
                If dtpBirthTo.Checked Then .dtBirthTo = dtpBirthTo.Value

                .intDie = basConst.gcintALIVE

                'if people died
                If chkDie.Checked Then

                    .intDie = basConst.gcintDIED

                    'from died date - to died date
                    'If dtpDieFrom.Checked Then .dtDieFrom = dtpDieFrom.Value
                    'If dtpDieTo.Checked Then .dtDieTo = dtpDieTo.Value

                    If String.Compare(lblDeaFrom.Text, basConst.gcstrDateUnknown) <> 0 Then

                        .intDFday = mstDieFrom.intDay
                        .intDFmon = mstDieFrom.intMonth
                        .intDFyea = mstDieFrom.intYear

                    End If

                    If String.Compare(lblDeaTo.Text, basConst.gcstrDateUnknown) <> 0 Then

                        .intDTday = mstDieTo.intDay
                        .intDTmon = mstDieTo.intMonth
                        .intDTyea = mstDieTo.intYear

                    End If


                End If

                'occupation
                .strOccupt = txtOccupt.Text.Trim()

                'position
                .strPosition = txtPosition.Text.Trim()

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetSearchData", ex)
        End Try

    End Function


    '   ************************************************************************ 
    '      FUNCTION   : xOpenPrintPreview, Open excel file in print preview mode
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       :  
    '      CREATE     : 2011/12/09  AKB Quyet 
    '      UPDATE     :  
    '   ************************************************************************
    Private Function xOpenPrintPreview() As Boolean

        Dim objExcel As clsExcel = Nothing
        Dim dtRows() As DataRow = Nothing

        Try
            Dim strSort As String = ""
            Dim strDate As String = ""
            Dim strSavePath As String = ""
            Dim intBorder As Integer = 1
            Dim intBgColor As Integer = basConst.gcintNONE_VALUE

            If Not basCommon.fncSaveFileDlg(strSavePath, basConst.gcstrExcelFilter, basConst.gcstrExcelExt) Then Return False

            objExcel = New clsExcel()

            'try to create excel instance
            If Not objExcel.fncCreateXlsApp() Then

                basCommon.fncMessageWarning(basConst.gcstrNoExcel, btnPrint)
                Return False

            End If

            If Not objExcel.fncOpenTemplateSearch(My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & basConst.gcstrXltPath1, gcintSheetNo, Config.Decrypt(basConst.gcstrXltPass)) Then Return False

            'set saved path
            objExcel.SaveName = strSavePath

            'insert date
            strDate = String.Format(basConst.gcstrDateFormat2, Date.Now)
            objExcel.fncSetCellData(mcintDateRow, mcintDateCol, strDate, 0, intBgColor)

            'start fill data to excel
            Try
                strSort = dgvSearchMember.SortedColumn.DataPropertyName
                If dgvSearchMember.SortOrder = SortOrder.Descending Then strSort &= " DESC" Else strSort &= " ASC"
            Catch ex As Exception
            End Try

            dtRows = mtblSearchData.Select("", strSort)

            For row As Integer = 0 To dtRows.Length - 1

                'set thick border for the last line
                If row = dtRows.Length - 1 Then intBorder = 2
                'If dgvSearchMember.Rows(i).DefaultCellStyle.BackColor = Color.LightGray Then intBgColor = basConst.gcintXlsSheetTan

                'STT column
                objExcel.fncSetCellData(row + 5, 1, (row + 1).ToString, intBorder, intBgColor)

                For col As Integer = 1 To mtblSearchData.Columns.Count - 3 '2 last column is REMARK DIED which is not shown

                    'set text
                    objExcel.fncSetCellData(row + 5, col + 1, fncCnvNullToString(dtRows(row).Item(col)), intBorder, intBgColor)

                Next

                'reset value
                intBgColor = basConst.gcintNONE_VALUE

            Next

            objExcel.fncDisplay(True)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xOpenPrintPreview", ex)
        Finally
            If objExcel IsNot Nothing Then objExcel.fncClose()
            objExcel = Nothing
            Erase dtRows
        End Try


    End Function


    '   ************************************************************************ 
    '      FUNCTION   : xGetLevel, get level of a member
    '      VALUE      : Integer, -1 if member is not found.
    '      PARAMS     : intMemID    Integer, member id
    '      MEMO       :  
    '      CREATE     : 2011/12/09  AKB Quyet 
    '      UPDATE     :  
    '   ************************************************************************
    Private Function xGetLevel(ByVal intMemID As Integer) As Integer

        xGetLevel = -1

        Try
            Dim dtRow() As DataRow

            dtRow = mtblLevel.Select(String.Format("MEMBER_ID = {0}", intMemID))

            'return -1 if member is not found
            If dtRow.Length <= 0 Then Exit Function

            'else return level of that member
            Integer.TryParse(basCommon.fncCnvNullToString(dtRow(0)("LEVEL")), xGetLevel)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xOpenPrintPreview", ex)
        End Try

    End Function


#End Region

End Class