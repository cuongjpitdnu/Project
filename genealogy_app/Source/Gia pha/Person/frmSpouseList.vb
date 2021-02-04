'   ******************************************************************
'      TITLE      : SPOUSE LIST
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/11/14　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On


'   ******************************************************************
'　　　FUNCTION   : Spouse list class
'      MEMO       : 
'      CREATE     : 2011/09/14  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmSpouseList

    Private Const mcstrClsName As String = "frmSpouseList"      'class name
    Private Const mcintItemPerPage As Integer = 100             'item per page

    Private mstrKeyword As String                               'keyword
    Private mintCurMem As Integer                               'current member id
    Private mintHID As Integer                                  'husband id
    Private mintWID As Integer                                  'wife id
    Private mblnSelected As Boolean                             'member selected flag
    Private mtblData As DataTable                               'datatable to search
    Private mtblDataSource As DataTable                         'data source
    Private mtblDatasourceTemp As DataTable
    Private mstSearchData As stSearchData                       'search info

    Private mintCurPage As Integer                              'currently showing page
    Private mintTotalPage As Integer                            'total of page

    Private Structure stSearchData

        Dim intHId As Integer                        'member id
        Dim strHFirstName As String                  'first name
        Dim strHMidName As String                    'middle name
        Dim strHLastName As String                   'last name
        Dim strHAlias As String                      'alias

        Dim intWId As Integer                        'member id
        Dim strWFirstName As String                  'first name
        Dim strWMidName As String                    'middle name
        Dim strWLastName As String                   'last name
        Dim strWAlias As String                      'alias

    End Structure


    '   ******************************************************************
    '　　　FUNCTION   : HusbandId Property, return husband id
    '      MEMO       : 
    '      CREATE     : 2011/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property HusbandId() As Integer
        Get
            Return mintHID
        End Get
    End Property


    '   ******************************************************************
    '　　　FUNCTION   : WifeId Property, return wife id
    '      MEMO       : 
    '      CREATE     : 2011/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property WifeId() As Integer
        Get
            Return mintWID
        End Get
    End Property


    '   ******************************************************************
    '　　　FUNCTION   : MemberSelected Property, return member is selected
    '      MEMO       : 
    '      CREATE     : 2011/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property MemberSelected() As Boolean

        Get
            Return mblnSelected
        End Get

    End Property


#Region "CONSTRUCTOR"


    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="intCurMem"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal intCurMem As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        mintCurPage = 1
        Me.mintCurMem = intCurMem
        mtblDataSource = New DataTable()
        mtblDataSource.Columns.Add("HID")
        mtblDataSource.Columns.Add("WID")
        mtblDataSource.Columns.Add("STT")
        mtblDataSource.Columns.Add("HUSBAND")
        mtblDataSource.Columns.Add("WIFE")


    End Sub


#End Region


#Region "Form events"


    ''' <summary>
    ''' Form loaded
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmSpouseList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            xSearch()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmPersonList_Load", ex)
        End Try

    End Sub


    ''' <summary>
    ''' SEARCH Button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Me.lblResultInfo.Text = ""

            If Not xSearch() Then

                'No result - clear grid
                'dgvMemberList.Rows.Clear()
                If mtblDataSource IsNot Nothing Then mtblDataSource.Rows.Clear()
                If mtblDatasourceTemp IsNot Nothing Then mtblDatasourceTemp.Rows.Clear()
                cbPages.Items.Clear()

                'message
                Me.lblResultInfo.Text = basConst.gcstrFindNotFound

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSearch_Click", ex)
        End Try


    End Sub


    ''' <summary>
    ''' OK button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

        Try
            If Not xGetID() Then

                basCommon.fncMessageInfo(basConst.gcstrNoUserSelected)
                Exit Sub

            End If

            mblnSelected = True
            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnOk_Click", ex)
        End Try

    End Sub


    ''' <summary>
    ''' CANCEL button click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try

    End Sub


    ''' <summary>
    ''' btnFirstPage_Click, button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <create>AKB QUYET 2012 10 01</create>
    Private Sub btnFirstPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirstPage.Click
        Try
            If mintCurPage <= 1 Then Exit Sub

            mintCurPage = 1
            xFillGrid()

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
    ''' <create>AKB QUYET 2012 10 01</create>
    Private Sub btnPrePage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrePage.Click
        Try
            If mintCurPage <= 1 Then Exit Sub

            mintCurPage -= 1
            xFillGrid()

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
    ''' <create>AKB QUYET 2012 10 01</create>
    Private Sub btnNextPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNextPage.Click
        Try
            If mintCurPage >= mintTotalPage Then Exit Sub

            mintCurPage += 1
            xFillGrid()

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
    ''' <create>AKB QUYET 2012 10 01</create>
    Private Sub btnLastPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLastPage.Click
        Try
            If mintCurPage >= mintTotalPage Then Exit Sub

            mintCurPage = mintTotalPage
            xFillGrid()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnLastPage_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' cbPages_SelectedIndexChanged, button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <create>AKB QUYET 2012 10 01</create>
    Private Sub cbPages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPages.SelectedIndexChanged
        Try
            Dim intSelectedPage As Integer
            intSelectedPage = cbPages.SelectedIndex + 1

            If intSelectedPage = mintCurPage Then Exit Sub

            mintCurPage = intSelectedPage
            xFillGrid()

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
    ''' <create>AKB QUYET 2012 10 01</create>
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
            mintCurPage = intPage
            xFillGrid()
            cbPages.SelectAll()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "cbPages_KeyPress", ex)
        End Try
    End Sub


#End Region


#Region "Methods"

    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show form
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm() As Boolean

        fncShowForm = False

        Try

            mblnSelected = False
            mintHID = basConst.gcintNO_MEMBER
            mintWID = basConst.gcintNO_MEMBER

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSearch, do searching
    '      VALUE      : Boolean, true - have result, false - no result
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSearch() As Boolean

        xSearch = False

        Try
            mstrKeyword = txtSearch.Text.Trim()

            'get data from database
            mtblData = gobjDB.fncGetSpouseList(mstrKeyword)

            'exit if there's no data
            If mtblData Is Nothing Then Exit Function

            'fill gird
            xFillData()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSearch", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillData, fill result on data table
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillData() As Boolean

        xFillData = False

        Dim strContent(4) As Object

        Try
            Dim intSTT As Integer = 0

            'clear grid before filling
            If mtblDataSource IsNot Nothing Then mtblDataSource.Rows.Clear()

            For i As Integer = 0 To mtblData.Rows.Count - 1

                With mstSearchData
                    'get data at row(i)
                    xGetSearchStruc(i)

                    'check if these couples are in a family
                    If (basCommon.fncIsDownLineOf(mintCurMem, .intHId) Or basCommon.fncIsAncentor(mintCurMem, .intHId)) Then Continue For
                    If (basCommon.fncIsDownLineOf(mintCurMem, .intWId) Or basCommon.fncIsAncentor(mintCurMem, .intWId)) Then Continue For

                    'clear array before use it
                    Array.Clear(strContent, 0, strContent.Length)

                    'member id
                    strContent(0) = .intHId
                    strContent(1) = .intWId

                    'NO field
                    intSTT += 1
                    strContent(2) = intSTT.ToString()

                    'full husband name
                    strContent(3) = basCommon.fncGetFullName(.strHFirstName, .strHMidName, .strHLastName, .strHAlias)

                    'full wife name
                    strContent(4) = basCommon.fncGetFullName(.strWFirstName, .strWMidName, .strWLastName, .strWAlias)

                End With

                'add new row to gird view
                mtblDataSource.Rows.Add(strContent)

            Next

            mintCurPage = 1
            xFillGrid()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillData", ex)
        Finally
            If mtblData IsNot Nothing Then mtblData.Dispose()
            Erase strContent
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillGrid, fill result on grid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillGrid() As Boolean

        xFillGrid = False

        Dim strContent(4) As Object

        Try
            Dim intStart As Integer
            Dim intEnd As Integer
            Dim intSTT As Integer = 0

            mtblDatasourceTemp = New DataTable()
            mtblDatasourceTemp.Columns.Add("HID")
            mtblDatasourceTemp.Columns.Add("WID")
            mtblDatasourceTemp.Columns.Add("STT")
            mtblDatasourceTemp.Columns.Add("HUSBAND")
            mtblDatasourceTemp.Columns.Add("WIFE")

            intStart = (mintCurPage - 1) * mcintItemPerPage
            intEnd = mintCurPage * mcintItemPerPage - 1
            If intEnd > mtblDataSource.Rows.Count - 1 Then intEnd = mtblDataSource.Rows.Count - 1

            For i As Integer = intStart To intEnd
                mtblDatasourceTemp.ImportRow(mtblDataSource.Rows(i))
            Next

            'bind data to fill
            dgvMemberList.AutoGenerateColumns = False
            dgvMemberList.DataSource = mtblDatasourceTemp

            'calculate total of page
            mintTotalPage = CInt(Math.Ceiling(mtblDataSource.Rows.Count / mcintItemPerPage))
            basCommon.fncMakeCbPage(mintTotalPage, cbPages)
            cbPages.SelectedIndex = mintCurPage - 1

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillGrid", ex)
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetSearchStruc, read data at row
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRow Integer, row to read data
    '      MEMO       : 
    '      CREATE     : 2011/11/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetSearchStruc(ByVal intRow As Integer) As Boolean

        xGetSearchStruc = False

        Try
            'get data at row
            With mtblData.Rows(intRow)
                'member id
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("HUSBAND.MEMBER_ID")), mstSearchData.intHId)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("WIFE.MEMBER_ID")), mstSearchData.intWId)

                'full name
                mstSearchData.strHFirstName = basCommon.fncCnvNullToString(.Item("HUSBAND.FIRST_NAME"))
                mstSearchData.strHMidName = basCommon.fncCnvNullToString(.Item("HUSBAND.MIDDLE_NAME"))
                mstSearchData.strHLastName = basCommon.fncCnvNullToString(.Item("HUSBAND.LAST_NAME"))
                mstSearchData.strHAlias = basCommon.fncCnvNullToString(.Item("HUSBAND.ALIAS_NAME"))

                mstSearchData.strWFirstName = basCommon.fncCnvNullToString(.Item("WIFE.FIRST_NAME"))
                mstSearchData.strWMidName = basCommon.fncCnvNullToString(.Item("WIFE.MIDDLE_NAME"))
                mstSearchData.strWLastName = basCommon.fncCnvNullToString(.Item("WIFE.LAST_NAME"))
                mstSearchData.strWAlias = basCommon.fncCnvNullToString(.Item("WIFE.ALIAS_NAME"))

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetSearchStruc", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetID, get user id from grid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetID() As Boolean

        xGetID = False

        Try
            Dim intRow As Integer

            If dgvMemberList.Rows.Count <= 0 Then Exit Function

            intRow = dgvMemberList.CurrentRow.Index

            If intRow < 0 Then Exit Function

            'get value at column 0 1 -> husband wife
            If Not Integer.TryParse(dgvMemberList.Item(0, intRow).Value.ToString(), mintHID) Then Exit Function
            If Not Integer.TryParse(dgvMemberList.Item(1, intRow).Value.ToString(), mintWID) Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetID", ex)
        End Try

    End Function

#End Region


End Class