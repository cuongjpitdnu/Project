'   ******************************************************************
'      TITLE      : MANAGE CHILDREN
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/08/31　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************

Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : Manage children Class
'      MEMO       : 
'      CREATE     : 2011/08/31  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmChildsManager


#Region "Constants"

    Private Const mcstrClsName As String = "frmChildsManager"                                            'class name
    Private Const mcstrError As String = "Lỗi khởi tạo ban đầu."                                               'error when form loading

#End Region


#Region "Variable Define"

    Private mintMemID As Integer                                'Father/mother id
    Private mintKidID As Integer                                'kid id

    Private mtblData As DataTable                               'table of data
    Private mstUserInfo As stUserInfo                           'structure that store data
    Private mfrmPersonInfo As frmPersonInfo                     'person info form

#End Region


#Region "Structure"


    '   ******************************************************************
    '　　　FUNCTION   : stUserInfo Structure
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Structure stUserInfo

        Dim intID As Integer                'member id
        Dim strFirstName As String          'first name
        Dim strMidName As String            'middle name
        Dim strLastName As String           'last name
        Dim dtBirth As Date                 'date of birth
        Dim enRel As clsEnum.emRelation     'relationship

    End Structure


#End Region


    'event to refresh data
    Public Event evnRefresh()


    '   ******************************************************************
    '　　　FUNCTION   : CONSTRUCTOR
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal intID As Integer, Optional ByVal frmPerInfo As frmPersonInfo = Nothing)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mintMemID = intID
        Me.mtblData = Nothing
        Me.mfrmPersonInfo = frmPerInfo

    End Sub


#Region "Form Controls Event"


    '   ******************************************************************
    '　　　FUNCTION   : frmChildsManager_Load, Form load
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmChildsManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            'call form load function
            If Not xFormLoad() Then basCommon.fncMessageError(mcstrError)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmChildsManager_Load", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvChildren_CellClick, cell clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvChildren_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvChildren.CellClick

        Try

            xFillFromCell(e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvChildren_CellClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnOK_Click, Ok button clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Try
            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnOK_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnFindMember_Click, Find button clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnFindMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindMember.Click

        Try


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnFindMember_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvChildren_CellMouseDoubleClick, double click
    '      MEMO       : 
    '      CREATE     : 2011/12/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvChildren_CellMouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvChildren.CellMouseDoubleClick

        Try
            'handle left mouse only
            If e.Button <> Windows.Forms.MouseButtons.Left Then Exit Sub

            'get id from grid
            xGetID(e)

            'show person infor form
            If mfrmPersonInfo Is Nothing Then Exit Sub

            mfrmPersonInfo.MemberID = mintKidID
            mfrmPersonInfo.FormMode = clsEnum.emMode.EDIT
            mfrmPersonInfo.MemberGender = clsEnum.emGender.UNKNOW

            'show form 
            If Not mfrmPersonInfo.fncShowForm() Then Exit Sub

            'if member is edied
            If Not mfrmPersonInfo.FormModified Then Exit Sub

            'reload data on this form
            xFormLoad()

            'raise event to manager form
            RaiseEvent evnRefresh()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvChildren_CellMouseDoubleClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click, Cancel button clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try

    End Sub


#End Region


#Region "Class Function"


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show this form
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
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
    '　　　FUNCTION   : xFormLoad, Form load event
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFormLoad() As Boolean

        xFormLoad = False

        Try
            'get data
            If Not xGetData(mintMemID) Then Return True

            'fill gird
            xFillGrid(mtblData, mstUserInfo)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFormLoad", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetData, get data
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMemID    Integer, member to get child
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetData(ByVal intMemID As Integer) As Boolean

        xGetData = False

        Try
            'get data
            'mtblData = gobjDB.fncGetKids(intMemID)
            mtblData = basCommon.fncGetKids(intMemID)

            If mtblData Is Nothing Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetData", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillGrid, fill grid
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : tblData DataTable, table of data
    '      PARAMS     : stUser  stUserInfo, structure that store infor
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillGrid(ByVal tblData As DataTable, ByVal stUser As stUserInfo) As Boolean

        xFillGrid = False

        Try
            'clear before using
            dgvChildren.Rows.Clear()

            'get data at row and add to grid
            For i As Integer = 0 To tblData.Rows.Count - 1

                xGetDataAt(i, stUser)

                xAdd2Grid(stUser)

            Next


            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillGrid", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetDataAt, get data at specific row
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intRow Integer, row to get data
    '      PARAMS     : stUser  stUserInfo, structure that store infor
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetDataAt(ByVal intRow As Integer, ByRef stUser As stUserInfo) As Boolean

        xGetDataAt = False

        Try
            Dim intRelation As Integer = clsEnum.emRelation.NATURAL

            With mtblData.Rows(intRow)
                'user id
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("MEMBER_ID")), stUser.intID)

                'full name
                stUser.strFirstName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))
                stUser.strMidName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                stUser.strLastName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))

                'date of birth
                Date.TryParse(basCommon.fncCnvNullToString(.Item("BIRTH_DAY")), stUser.dtBirth)

                'relationship
                stUser.enRel = clsEnum.emRelation.NATURAL
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("RELID")), intRelation)
                If intRelation = clsEnum.emRelation.ADOPT Then stUser.enRel = clsEnum.emRelation.ADOPT

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetDataAt", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAdd2Grid, add data to grid
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : stUser  stUser
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAdd2Grid(ByVal stUser As stUserInfo) As Boolean

        xAdd2Grid = False

        Dim strContent(4) As String

        Try
            With stUser
                'Row NO
                strContent(0) = (dgvChildren.Rows.Count + 1).ToString()

                'full name
                strContent(1) = String.Format(basConst.gcstrNameFormat, .strLastName, .strMidName, .strFirstName)
                strContent(1) = basCommon.fncRemove2Space(strContent(1))

                'date of birth
                If .dtBirth > Date.MinValue Then strContent(2) = String.Format(basConst.gcstrDateFormat1, .dtBirth)

                'relationship
                strContent(3) = "Con đẻ"
                If .enRel = clsEnum.emRelation.ADOPT Then strContent(3) = "Con nuôi"

                'add to grid
                dgvChildren.Rows.Add(strContent)

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAdd2Grid", ex)
        Finally
            Erase strContent
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillFromCell, get data from cell to fill control
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : e   DataGridViewCellEventArgs
    '      MEMO       : 
    '      CREATE     : 2011/08/31  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillFromCell(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) As Boolean

        xFillFromCell = False

        Try
            Dim intRow As Integer
            Dim dtBirth As Date

            intRow = e.RowIndex

            'exit if it is header
            If intRow < 0 Then Return True

            'get data from row
            With dgvChildren.Rows(intRow)
                'full name at column id 1
                txtName.Text = basCommon.fncCnvNullToString(.Cells(1).Value)

                'date at column id 2
                Date.TryParse(basCommon.fncCnvNullToString(.Cells(2).Value), dtBirth)
                If dtBirth > Date.MinValue Then dtpBirth.Value = dtBirth

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillFromCell", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetID, get user id from grid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : e DataGridViewCellMouseEventArgs,
    '      MEMO       : 
    '      CREATE     : 2011/12/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetID(ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) As Boolean

        xGetID = False

        Try
            Dim intRowID As Integer = clsDefine.NONE_VALUE
            Dim intMemNumber As Integer = clsDefine.NONE_VALUE
            Dim intGender As Integer = clsDefine.NONE_VALUE

            intRowID = e.RowIndex

            If intRowID = clsDefine.NONE_VALUE Then Exit Function

            'read row number
            If Not Integer.TryParse(basCommon.fncCnvNullToString(dgvChildren.Item(0, intRowID).Value), intMemNumber) Then Return False

            'get id from datatable with row number
            If Not Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(intMemNumber - 1).Item("MEMBER_ID")), mintKidID) Then Return False

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetID", ex)
        End Try

    End Function


#End Region

End Class