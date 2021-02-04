'   ******************************************************************
'      TITLE      : PERSON LIST
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
'　　　FUNCTION   : frmPeronList
'      MEMO       : 
'      CREATE     : 2011/09/14  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmPersonList

    Private Const mcstrClsName As String = "frmPersonList"                              'class name
    Private Const mcintItemPerPage As Integer = 100                                     'item per page

    Private mstSearchInfo As clsDbAccess.stSearch                                       'information struture to search
    Private mstSearchdata As stSearchData                                               'seach data structure
    Private mtblData As DataTable                                                       'datatable 
    Private mtblDataSource As DataTable                                                 'datasource
    Private mintMemberID As Integer                                                     'member id to get parent and spouse
    Private mintSelectedID As Integer                                                   'member id selected
    Private mblnSelected As Boolean                                                     'flag to determine member is selected or not
    Private menFormMode As enFormMode                                                   'form mode

    Private mintCurPage As Integer                                                      'currently showing page
    Private mintTotalPage As Integer                                                    'total of page
    Private mblnFilter As Boolean
    Private mintGenderFilter As Integer

    'form mode
    Public Enum enFormMode As Integer

        SHOW_PARENT
        SHOW_SPOUSE
        SELECT_MEMBER

    End Enum


    'structure to fill grid
    Private Structure stSearchData

        Dim intID As Integer                        'member id
        Dim strFirstName As String                  'first name
        Dim strMidName As String                    'middle name
        Dim strLastName As String                   'last name
        Dim strAlias As String                      'alias

        'Dim dtBirth As Date                         'date of birth
        Dim intBday As Integer
        Dim intBmon As Integer
        Dim intByea As Integer

        Dim intGender As Integer                    'gender
        Dim intRel As Integer                       'relation ship

    End Structure


    '   ******************************************************************
    '　　　FUNCTION   : MemberID Property, return member id
    '      MEMO       : 
    '      CREATE     : 2011/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property MemberID() As Integer

        Get
            Return mintSelectedID
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


    '   ******************************************************************
    '　　　FUNCTION   : Constructor
    '      MEMO       : 
    '      CREATE     : 2011/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub frmPersonList()

    End Sub


#Region "Form events"


    '   ******************************************************************
    '　　　FUNCTION   : frmPersonList_Load, form load
    '      MEMO       : 
    '      CREATE     : 2011/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmPersonList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            xSetting()

            'create datasource table
            mtblDataSource = New DataTable()
            mtblDataSource.Columns.Add("MEMBER_ID")
            mtblDataSource.Columns.Add("GENDER", GetType(Image))
            mtblDataSource.Columns.Add("STT")
            mtblDataSource.Columns.Add("FULL_NAME")
            mtblDataSource.Columns.Add("BDATE")
            mtblDataSource.Columns.Add("RELATIONSHIP")


            If menFormMode = enFormMode.SELECT_MEMBER Then xSearch()

            If menFormMode = enFormMode.SHOW_PARENT Then xGetParent()

            If menFormMode = enFormMode.SHOW_SPOUSE Then xGetSpouse()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmPersonList_Load", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnSearch_Click, search button clicked
    '      MEMO       : 
    '      CREATE     : 2011/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Me.lblResultInfo.Text = ""

            If Not xSearch() Then

                'No result - clear grid
                'dgvMemberList.Rows.Clear()
                If mtblDataSource IsNot Nothing Then mtblDataSource.Rows.Clear()

                'message
                Me.lblResultInfo.Text = basConst.gcstrFindNotFound

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSearch_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnOk_Click, Ok button clicked
    '      MEMO       : 
    '      CREATE     : 2011/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
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


    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click, Cancel button clicked
    '      MEMO       : 
    '      CREATE     : 2011/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
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
    Public Function fncShowForm(ByVal enMode As enFormMode, _
                                Optional ByVal intMemID As Integer = basConst.gcintNO_MEMBER, _
                                Optional ByVal emGenderFilter As clsEnum.emGender = clsEnum.emGender.UNKNOW) As Boolean

        fncShowForm = False

        Try
            Me.menFormMode = enMode
            Me.mintMemberID = intMemID

            mblnSelected = False
            mblnFilter = False
            mintSelectedID = basConst.gcintRootID
            mintGenderFilter = emGenderFilter

            If emGenderFilter <> clsEnum.emGender.UNKNOW Then

                mblnFilter = True
                rdAll.Visible = False
                rdFemale.Visible = False
                rdMale.Visible = False

            End If

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show form
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function xSetting() As Boolean

        xSetting = False

        Try
            'disable search
            If menFormMode = enFormMode.SHOW_PARENT Or menFormMode = enFormMode.SHOW_SPOUSE Then _
                grpSearch.Enabled = False

            mintCurPage = 1

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xSetting", ex)

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
            'get search information
            xGetSearchInfo()

            'get data from database
            mtblData = gobjDB.fncGetQuickSearch(mstSearchInfo)

            'exit if there's no data
            If mtblData Is Nothing Then Exit Function

            If mblnFilter Then

                Dim intID As Integer = 0

                For i As Integer = mtblData.Rows.Count - 1 To 0 Step -1
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("MEMBER_ID")), intID)
                    If basCommon.fncHasSpouse(intID) Then mtblData.Rows(i).Delete()
                Next

            End If

            'fill gird
            xFillGrid()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSearch", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetParent, get list of father and mother
    '      VALUE      : Boolean, true - have result, false - no result
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetParent() As Boolean

        xGetParent = False

        Try
            'get data from database
            mtblData = gobjDB.fncGetParent(mintMemberID, True)

            'exit if there's no data
            If mtblData Is Nothing Then Exit Function

            'fill gird
            xFillGrid()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetParent", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetSpouse, get list of spouse
    '      VALUE      : Boolean, true - have result, false - no result
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetSpouse() As Boolean

        xGetSpouse = False

        Try
            'get data from database
            mtblData = gobjDB.fncGetHusWife(mintMemberID)

            'exit if there's no data
            If mtblData Is Nothing Then Exit Function

            'fill gird
            xFillGrid()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetSpouse", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetSearchInfo, read infor from controls
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetSearchInfo() As Boolean

        xGetSearchInfo = False

        Try

            With mstSearchInfo
                'get keyword
                .strKeyword = txtSearch.Text.Trim()
                .intGender = mintGenderFilter

                'gender
                If Not mblnFilter Then

                    .intGender = clsEnum.emGender.UNKNOW
                    If rdMale.Checked Then .intGender = clsEnum.emGender.MALE
                    If rdFemale.Checked Then .intGender = clsEnum.emGender.FEMALE

                End If

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetSearchInfo", ex)
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

        Dim strContent(5) As Object

        Try
            Dim intStart As Integer
            Dim intEnd As Integer

            'clear grid before filling
            'dgvMemberList.Rows.Clear()
            If mtblDataSource IsNot Nothing Then mtblDataSource.Rows.Clear()

            intStart = (mintCurPage - 1) * mcintItemPerPage
            intEnd = mintCurPage * mcintItemPerPage - 1
            If intEnd > mtblData.Rows.Count - 1 Then intEnd = mtblData.Rows.Count - 1


            'For i As Integer = 0 To mtblData.Rows.Count - 1
            For i As Integer = intStart To intEnd

                With mstSearchdata
                    'get data at row(i)
                    xGetSearchStruc(i)

                    'clear array before use it
                    Array.Clear(strContent, 0, strContent.Length)

                    'member id
                    strContent(0) = .intID

                    'image field
                    strContent(1) = GiaPha.My.Resources.Gender_unknown16
                    If .intGender = clsEnum.emGender.MALE Then strContent(1) = GiaPha.My.Resources.Gender_man16
                    If .intGender = clsEnum.emGender.FEMALE Then strContent(1) = GiaPha.My.Resources.Gender_woman16

                    'NO field
                    strContent(2) = basCommon.fncCnvNullToString(i + 1)

                    'full name
                    strContent(3) = basCommon.fncGetFullName(.strFirstName, .strMidName, .strLastName, .strAlias)

                    'birth date
                    'If .dtBirth > Date.MinValue Then strContent(4) = String.Format(basConst.gcstrDateFormat2, .dtBirth)
                    strContent(4) = basCommon.fncGetDateName("", .intBday, .intBmon, .intByea, True)

                    'relation ship
                    If menFormMode = enFormMode.SHOW_PARENT Or menFormMode = enFormMode.SHOW_SPOUSE Then _
                        strContent(5) = xGetRelationName(.intRel, .intGender)

                End With

                'add new row to gird view
                'dgvMemberList.Rows.Add(strContent)
                mtblDataSource.Rows.Add(strContent)

            Next

            'bind data to fill
            dgvMemberList.AutoGenerateColumns = False
            dgvMemberList.DataSource = mtblDataSource

            'calculate total of page
            mintTotalPage = CInt(Math.Ceiling(mtblData.Rows.Count / mcintItemPerPage))
            basCommon.fncMakeCbPage(mintTotalPage, cbPages)
            cbPages.SelectedIndex = mintCurPage - 1

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillGrid", ex)
        Finally
            Erase strContent
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
                If menFormMode = enFormMode.SELECT_MEMBER Then

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item("MEMBER_ID")), mstSearchdata.intID)

                Else

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item("REL_FMEMBER_ID")), mstSearchdata.intID)

                End If

                'full name
                mstSearchData.strFirstName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))
                mstSearchData.strMidName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                mstSearchdata.strLastName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))
                mstSearchdata.strAlias = basCommon.fncCnvNullToString(.Item("ALIAS_NAME"))

                'birth and decease date
                'Date.TryParse(basCommon.fncCnvNullToString(.Item("BIRTH_DAY")), mstSearchdata.dtBirth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY")), mstSearchdata.intBday)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON")), mstSearchdata.intBmon)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA")), mstSearchdata.intByea)

                'gender
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("GENDER")), mstSearchdata.intGender)

                'relation ship
                If menFormMode = enFormMode.SHOW_PARENT Or menFormMode = enFormMode.SHOW_SPOUSE Then _
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item("RELID")), mstSearchdata.intRel)

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

            'get value at column 0 -> member id
            If Not Integer.TryParse(dgvMemberList.Item(0, intRow).Value.ToString(), mintSelectedID) Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetID", ex)
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
    Private Function xGetRelationName(ByVal intRel As Integer, ByVal intGender As Integer) As String

        Dim strResult As String = ""

        Try
            'if form in SHOW_SPOUSE mode
            If menFormMode = enFormMode.SHOW_SPOUSE Then

                If intGender = clsEnum.emGender.FEMALE Then strResult = basConst.gcstrWife
                If intGender = clsEnum.emGender.MALE Then strResult = basConst.gcstrHusband

                Return strResult

            End If

            'if form in SHOW_PARENT mode
            'Natural relatioship
            If intRel = clsEnum.emRelation.NATURAL Then

                If intGender = clsEnum.emGender.FEMALE Then strResult = basConst.gcstrMother
                If intGender = clsEnum.emGender.MALE Then strResult = basConst.gcstrFather

                Return strResult

            End If

            'Adoptive relatioship
            If intRel = clsEnum.emRelation.ADOPT Then

                If intGender = clsEnum.emGender.FEMALE Then strResult = basConst.gcstrMother & " " & basConst.gcstrAdopt
                If intGender = clsEnum.emGender.MALE Then strResult = basConst.gcstrFather & " " & basConst.gcstrAdopt

                Return strResult

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetID", ex)
        Finally

        End Try

        Return strResult

    End Function



#End Region




End Class