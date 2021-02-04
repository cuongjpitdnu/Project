'   ******************************************************************
'      TITLE      : RELATION MEMBER MANAGEMENT
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2012/01/03　KBS ANH
'      UPDATE     : 
'
'           2012 KBS ANH SOFTWARE
'   ******************************************************************

Option Explicit On

'   ******************************************************************
'　　　FUNCTION   : Relation Member Management Class
'      MEMO       : 
'      CREATE     : 2012/01/03  KBS ANH
'      UPDATE     : 
'   ******************************************************************
Public Class frmRelationMem

    Public Event evnRefresh()
    Public Event evnRefreshRelMemList()
    'RaiseEvent evnRefresh()

#Region "Constants"

    Private Const mcstrClsName As String = "frmRelationMem"                              'class name
    Private Const mcstrMsgCnnErr As String = "Không kết nối được với cơ sở dữ liệu."                 'Not Connect DataBase
    Private Const mcstrMsgErrNoId As String = "Không tìm thấy thành viên cần hiển thị."              'Not Found Persion
    Private Const mcstrError As String = "Lỗi khởi tạo ban đầu."                               'error when form loading
    Private Const mcstrModeChi As String = " các con"
    Private Const mcstrModeWif As String = " vợ"
    Private Const mcstrModeHus As String = " chồng"
    Private Const mcstrModePar As String = " cha mẹ"

#End Region

#Region "Structure And Enum Define"

    '   ******************************************************************
    '　　　FUNCTION   : stUserInfo Structure
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Public Structure stUserInfo

        Dim intID As Integer                'member id
        Dim strName As String               'first name
        'Dim dtBirth As Date                 'date of birth
        Dim intBday As Integer
        Dim intBmon As Integer
        Dim intByea As Integer
        Dim intGender As Integer
        Dim enRel As clsEnum.emRelation     'relationship

    End Structure

    '   ******************************************************************
    '　　　FUNCTION   : ダイアログモード    
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Public Enum emRelMode
        Parent                                                                 '親情報表示モード    
        Childs                                                                 '子供達情報表示モード    
        Spouse                                                                 '夫婦情報表示モード    
    End Enum

#End Region

#Region "Variable Define"

    Private mstRootInfo As stUserInfo
    Private mintRootID As Integer = basConst.gcintNONE_VALUE
    Private mintRootGen As Integer = basConst.gcintNONE_VALUE
    Private mstrIdField As String = String.Empty
    Private memMode As emRelMode = emRelMode.Parent

#End Region

#Region "Properties"

    '   ******************************************************************
    '　　　FUNCTION   : 現在対応メンバーID値のプロパティ   
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Public Property RootID() As Integer
        Get
            Return mintRootID
        End Get
        Set(ByVal value As Integer)
            mintRootID = value
        End Set
    End Property

    '   ******************************************************************
    '　　　FUNCTION   : ダイアログモードプロパティ   
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Public Property FormMode() As emRelMode
        Get
            Return memMode
        End Get
        Set(ByVal value As emRelMode)
            memMode = value
        End Set
    End Property

#End Region

#Region "Form Controls Event"

    '   ******************************************************************
    '　　　FUNCTION   : Cancel Button Click's Event
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try

            Me.Close()

        Catch ex As Exception
            'ログファイルに例外エラーを出力する   
            basCommon.fncSaveErr(mcstrClsName, "fncShow", ex)

        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : Member Double Click's Event
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub grdRelation_CellMouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvRelation.CellMouseDoubleClick

        Dim frmEditInfo As New frmPersonInfo(clsEnum.emMode.EDIT)
        Try
            Dim intMemId As Integer = basConst.gcintNONE_VALUE
            Dim intMemGen As Integer = clsEnum.emGender.UNKNOW

            If String.IsNullOrEmpty(mstrIdField) Then Exit Sub

            'マウスクリックを確認する   
            If e.Button <> Windows.Forms.MouseButtons.Left Then Exit Sub
            'メンバーID値を取得する   
            If Not (e.RowIndex > basConst.gcintNONE_VALUE) Then Exit Sub
            If Not Integer.TryParse(dgvRelation.Rows(e.RowIndex).Cells(mstrIdField).Value.ToString(), intMemId) Then Exit Sub
            If Not (intMemId > basConst.gcintRootID) Then Exit Sub

            '編集データ情報を設定する    
            frmEditInfo.MemberID = intMemId
            frmEditInfo.MemberGender = intMemGen

            frmEditInfo.fncShowForm()

            '選択したメンバー情報を編集したか確認する   
            If frmEditInfo.FormModified Then

                xLoadRelData()

                xSelMemById(intMemId)

                'メインフォームにイベントを登録する   
                RaiseEvent evnRefresh()

            End If

        Catch ex As Exception
            'ログファイルに例外エラーを出力する   
            basCommon.fncSaveErr(mcstrClsName, "xDataRowToRelationName", ex)

        Finally
            'オブジェクトを解除する   
            If Not IsNothing(frmEditInfo) Then
                frmEditInfo.Dispose()
                frmEditInfo = Nothing
            End If

        End Try

    End Sub


    ''' <summary>
    ''' Up Button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUp1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp1.Click
        Try
            'If basCommon.fncMoveGridRow(dgvRelation, +1) Then btnSave.Enabled = True
            If fncMoveGridRow(dgvRelation, +1) Then btnSave.Enabled = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnUp1_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Down button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDown1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown1.Click
        Try
            'If basCommon.fncMoveGridRow(dgvRelation, -1) Then btnSave.Enabled = True
            If fncMoveGridRow(dgvRelation, -1) Then btnSave.Enabled = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDown1_Click", ex)
        End Try
    End Sub

    ''' <summary>
    ''' fncMoveGridRow
    ''' </summary>
    ''' <param name="dgvData"></param>
    ''' <param name="intValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncMoveGridRow(ByVal dgvData As DataGridView, ByVal intValue As Integer) As Boolean
        fncMoveGridRow = False

        Dim tblData As DataTable

        Try
            If dgvData.RowCount <= 0 Then Exit Function
            If dgvData.SelectedRows.Count <= 0 Then Exit Function

            Dim intCurIndex As Integer = dgvData.SelectedRows(0).Index
            Dim intNewIndex As Integer
            tblData = CType(dgvData.DataSource, DataTable)

            intNewIndex = intCurIndex - intValue
            If intNewIndex < 0 Or intNewIndex > dgvData.Rows.Count - 1 Then Exit Function

            Dim intIndex As Integer = intNewIndex

            'clone new row
            Dim dtRow As DataRow = tblData.NewRow()
            For intIndex = 0 To tblData.Columns.Count - 1
                dtRow(intIndex) = tblData.Rows(intCurIndex)(intIndex)
            Next

            tblData.Rows.RemoveAt(intCurIndex)
            tblData.Rows.InsertAt(dtRow, intNewIndex)

            'dgvData.DataSource = tblData

            dgvData.Rows(intNewIndex).Selected = True

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncMoveGridRow", ex)
        Finally
            tblData = Nothing
        End Try
    End Function


    ''' <summary>
    ''' Save-button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim blnSuccess As Boolean = True
        Dim blnTrans As Boolean = False

        Try
            Dim intID As Integer

            blnTrans = gobjDB.BeginTransaction()

            For i As Integer = 0 To dgvRelation.Rows.Count - 1

                intID = basConst.gcintNO_MEMBER
                intID = basCommon.fncCnvToInt(dgvRelation.Item(clmMemID.Name, i).Value)

                Select Case FormMode
                    Case emRelMode.Childs
                        blnSuccess = blnSuccess And gobjDB.fncUpdateMemberFamilyOrder(intID, i + 1, False)

                    Case emRelMode.Spouse
                        'change role order
                        blnSuccess = blnSuccess And gobjDB.fncSetSpouseRoleOrder(mintRootID, intID, i + 1, clsEnum.emRelation.MARRIAGE, False)

                End Select

            Next

            If blnTrans Then
                If blnSuccess Then
                    gobjDB.Commit()
                Else
                    gobjDB.RollBack()
                End If
            End If

            'btnSave.Enabled = False
            RaiseEvent evnRefreshRelMemList()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSave_Click", ex)
            If blnTrans Then
                gobjDB.RollBack()
            End If
        End Try

    End Sub

#End Region

#Region "Class Function"

    '   ******************************************************************
    '　　　FUNCTION   : Form Dippay
    '      VALUE      : Boolean, True: Success, False: failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShow() As Boolean
        fncShow = False
        Dim objDt As New DataTable
        Try
            'データベース接続を確認する   
            If IsNothing(gobjDB) Then
                basCommon.fncMessageError(mcstrMsgCnnErr)
                Exit Function

            End If

            If Not gobjDB.IsConnect() Then
                basCommon.fncMessageError(mcstrMsgCnnErr)
                Exit Function

            End If

            '元ID値を確認する    
            objDt = gobjDB.fncGetMemberMain(mintRootID)
            If IsNothing(objDt) Then
                basCommon.fncMessageWarning(mcstrMsgErrNoId)
                Exit Function

            End If

            'Get Root's Member Information
            If Not xDataRowToUserInfo(objDt.Rows(0), mstRootInfo) Then Exit Function

            lblTitle.Text &= vbCrLf & mstRootInfo.strName
            btnUp1.Enabled = False
            btnDown1.Enabled = False
            btnSave.Enabled = False
            btnCancel.Focus()
            mstrIdField = clmMemID.Name
            Select Case FormMode

                Case emRelMode.Parent                                          '親情報表示モード    
                    grpListMem.Text &= mcstrModePar
                    'mstrIdField = "REL_FMEMBER_ID"

                Case emRelMode.Childs                                          '子供達情報表示モード    
                    'mstrIdField = "MEMBER_ID"
                    grpListMem.Text &= mcstrModeChi
                    btnUp1.Enabled = True
                    btnDown1.Enabled = True
                    btnSave.Enabled = True

                Case emRelMode.Spouse                                          '夫婦情報表示モード    
                    'mstrIdField = "REL_FMEMBER_ID"
                    Select Case mstRootInfo.intGender
                        Case clsEnum.emGender.MALE
                            grpListMem.Text &= mcstrModeWif

                        Case clsEnum.emGender.FEMALE
                            grpListMem.Text &= mcstrModeHus

                        Case Else
                            grpListMem.Text = String.Empty

                    End Select

                    btnUp1.Enabled = True
                    btnDown1.Enabled = True
                    btnSave.Enabled = True

                Case Else
                    grpListMem.Text = String.Empty

            End Select

            'データを取得したら一覧表に設定する  
            xLoadRelData()

            Me.ShowDialog()

            fncShow = True
        Catch ex As Exception
            'ログファイルに例外エラーを出力する   
            basCommon.fncSaveErr(mcstrClsName, "fncShow", ex)

        Finally
            'オブジェクトを解除する   
            objDt = Nothing

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : 関係のメンバーのデータを取得する    
    '      VALUE      : 関連メンバー、Boolean・True:有る、 False: 無し   
    '      PARAMS     : 無し   
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadRelData() As Boolean
        xLoadRelData = False

        Dim objdvSort As DataView
        Dim tblData As New DataTable
        Dim lstDtField As New List(Of String)

        Try
            lstDtField.Add("GEN")
            lstDtField.Add("NAME")
            'lstDtField.Add("BIRTH_DAY")
            lstDtField.Add("REL_TYPE")
            lstDtField.Add("BIRTH_DAY_NEW")
            lstDtField.Add("MEMBER_ID")

            Select Case FormMode
                Case emRelMode.Parent
                    tblData = gobjDB.fncGetParent(mintRootID)

                Case emRelMode.Childs
                    tblData = basCommon.fncGetKids(mintRootID)

                Case emRelMode.Spouse
                    tblData = gobjDB.fncGetHusWife(mintRootID)

            End Select

            If IsNothing(tblData) Then Exit Function

            ' ▽ 2012/11/27   AKB Quyet （temporary not used）***********************
            'objdvSort = objDt.DefaultView
            'objdvSort.Sort = "BIRTH_DAY ASC "
            'objdvSort.Sort = "BIR_YEA, BIR_MON, BIR_DAY"

            'objDt = objdvSort.ToTable()
            ' △ 2012/11/27   AKB Quyet *********************************************

            'データテーブルの列を追加する   
            tblData.Columns.Add("GEN", GetType(Image))
            tblData.Columns.Add("NAME")
            tblData.Columns.Add("REL_TYPE")
            tblData.Columns.Add("BIRTH_DAY_NEW")
            Try
                tblData.Columns.Add("MEMBER_ID")
            Catch ex As Exception
            End Try


            '一覧表にデータを追加する    
            xLoadRelData = xAddGridView(tblData, lstDtField)

        Catch ex As Exception
            'ログファイルに例外エラーを出力する   
            basCommon.fncSaveErr(mcstrClsName, "xLoadRelData", ex)

        Finally
            'オブジェクトを解除する   
            tblData = Nothing
            objdvSort = Nothing
            lstDtField = Nothing

        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : 一覧グリッドビューにデータを設定する    
    '      VALUE      : 設定結果、Boolean・True:成功、 False: 失敗   
    '      PARAMS     : DataTable      、表示データ   
    '                 : List(Of String)、表示フィールド   
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddGridView(ByRef vobjData As DataTable, ByVal vlstDtField As List(Of String)) As Boolean
        xAddGridView = False

        Try
            Dim intGenVal As Integer
            Dim dtVal As Date = Date.MinValue
            Dim strName As String = String.Empty

            Dim intRelNo As Integer
            Dim blnMulMarried As Boolean = False

            Dim intMemID As Integer
            Dim intBday As Integer
            Dim intBmon As Integer
            Dim intByea As Integer

            '表示データが存在しない    
            If IsNothing(vobjData) Then Exit Function
            If (Not vobjData.Columns.Count > 0) Or (Not vobjData.Rows.Count > 0) Then Exit Function

            If vobjData.Rows.Count > 1 Then blnMulMarried = True

            For intRelNo = 0 To vobjData.Rows.Count - 1

                '人間の性値を取得する    
                intGenVal = clsEnum.emGender.UNKNOW
                vobjData.Rows(intRelNo)("GEN") = GiaPha.My.Resources.Gender_unknown16

                If Integer.TryParse(vobjData.Rows(intRelNo)("GENDER"), intGenVal) Then
                    Select Case intGenVal
                        Case clsEnum.emGender.FEMALE
                            vobjData.Rows(intRelNo)("GEN") = GiaPha.My.Resources.Gender_woman16

                        Case clsEnum.emGender.MALE
                            vobjData.Rows(intRelNo)("GEN") = GiaPha.My.Resources.Gender_man16

                    End Select
                End If

                '人間の名前を取得する   
                vobjData.Rows(intRelNo)("NAME") = xDataRowToName(vobjData.Rows(intRelNo))

                '関係事を設定する                   
                vobjData.Rows(intRelNo)("REL_TYPE") = xDataRowToRelationName(vobjData.Rows(intRelNo), _
                                                                            intGenVal, _
                                                                            mstRootInfo.intGender, _
                                                                            blnMulMarried, _
                                                                            intRelNo)

                'birth date
                Integer.TryParse(basCommon.fncCnvNullToString(vobjData.Rows(intRelNo)("BIR_DAY")), intBday)
                Integer.TryParse(basCommon.fncCnvNullToString(vobjData.Rows(intRelNo)("BIR_MON")), intBmon)
                Integer.TryParse(basCommon.fncCnvNullToString(vobjData.Rows(intRelNo)("BIR_YEA")), intByea)
                vobjData.Rows(intRelNo)("BIRTH_DAY_NEW") = basCommon.fncGetDateName("", intBday, intBmon, intByea, True)

                'id 
                Select Case FormMode
                    Case emRelMode.Parent
                        Integer.TryParse(basCommon.fncCnvNullToString(vobjData.Rows(intRelNo)("REL_FMEMBER_ID")), intMemID)

                    Case emRelMode.Childs
                        Integer.TryParse(basCommon.fncCnvNullToString(vobjData.Rows(intRelNo)("MEMBER_ID")), intMemID)

                    Case emRelMode.Spouse
                        Integer.TryParse(basCommon.fncCnvNullToString(vobjData.Rows(intRelNo)("REL_FMEMBER_ID")), intMemID)

                End Select
                vobjData.Rows(intRelNo)("MEMBER_ID") = intMemID

            Next

            '一覧グリッドにデータを設定する   
            dgvRelation.DataSource = vobjData

            '一覧グリッドで非表示列を設定する   
            For Each objCol As DataGridViewColumn In dgvRelation.Columns

                If Not vlstDtField.Contains(objCol.DataPropertyName) Then objCol.Visible = False

            Next

            xAddGridView = True

        Catch ex As Exception
            'ログファイルに例外エラーを出力する   
            basCommon.fncSaveErr(mcstrClsName, "xAddGridView", ex)

        Finally

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : 取得して行データからメンバー構造体にデータを設定する    
    '      VALUE      : 設定結果、Boolean・True:成功、 False: 失敗   
    '      PARAMS     : DataRow   、データ行オブジェクト   
    '                 : stUserInfo、メンバー構造体   
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDataRowToUserInfo(ByVal objRowData As DataRow, ByRef rstUserInfo As stUserInfo) As Boolean
        xDataRowToUserInfo = False
        Try
            Dim strName As String = String.Empty
            Dim dtVal As Date = Date.MinValue

            'データが存在しない   
            If IsNothing(objRowData) Then Exit Function
            If IsNothing(objRowData("MEMBER_ID")) Then Exit Function
            If IsNothing(objRowData("GENDER")) Then Exit Function

            If Not Integer.TryParse(objRowData("MEMBER_ID"), rstUserInfo.intID) Then Exit Function
            If Not Integer.TryParse(objRowData("GENDER"), rstUserInfo.intGender) Then Exit Function
            strName = String.Format(basConst.gcstrNameFormat, _
                                    objRowData("LAST_NAME").ToString(), _
                                    objRowData("MIDDLE_NAME").ToString(), _
                                    objRowData("FIRST_NAME").ToString())

            rstUserInfo.strName = strName

            'rstUserInfo.dtBirth = Date.MinValue
            'If Date.TryParse(objRowData("BIRTH_DAY").ToString(), dtVal) Then rstUserInfo.dtBirth = dtVal
            'If Not Integer.TryParse(basCommon.fncCnvNullToString(objRowData("BIR_DAY")), rstUserInfo.intBday) Then Exit Function
            'If Not Integer.TryParse(basCommon.fncCnvNullToString(objRowData("BIR_MON")), rstUserInfo.intBmon) Then Exit Function
            'If Not Integer.TryParse(basCommon.fncCnvNullToString(objRowData("BIR_YEA")), rstUserInfo.intByea) Then Exit Function
            Integer.TryParse(basCommon.fncCnvNullToString(objRowData("BIR_DAY")), rstUserInfo.intBday)
            Integer.TryParse(basCommon.fncCnvNullToString(objRowData("BIR_MON")), rstUserInfo.intBmon)
            Integer.TryParse(basCommon.fncCnvNullToString(objRowData("BIR_YEA")), rstUserInfo.intByea)

            xDataRowToUserInfo = True
        Catch ex As Exception
            'ログファイルに例外エラーを出力する   
            basCommon.fncSaveErr(mcstrClsName, "xDataRowToUserInfo", ex)

        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : 取得して行データからメンバー名を取得する    
    '      VALUE      : メンバー名、String「LAST_NAME MIDDLE_NAME FIRST_NAME」   
    '      PARAMS     : DataRow   、データ行オブジェクト   
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDataRowToName(ByVal ovbjRowData As DataRow) As String
        xDataRowToName = String.Empty

        Try
            xDataRowToName = basCommon.fncGetFullName(ovbjRowData("FIRST_NAME").ToString(), _
                                                      ovbjRowData("MIDDLE_NAME").ToString(), _
                                                      ovbjRowData("LAST_NAME").ToString(), _
                                                      ovbjRowData("ALIAS_NAME").ToString())

        Catch ex As Exception
            'ログファイルに例外エラーを出力する   
            basCommon.fncSaveErr(mcstrClsName, "xDataRowToName", ex)

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : 取得して行データから関連種類名を取得する    
    '      VALUE      : 関連種類名、String   
    '      PARAMS     : DataRow 、データ行オブジェクト   
    '                 : Integer 、現在データの性   
    '                 : Integer 、元データの性   
    '                 : Boolean 、複数結婚しているフラグ   
    '                 : Integer 、現在データのインデックス値   
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDataRowToRelationName(ByVal ovbjRowData As DataRow, _
                                            ByVal vintCurGen As Integer, _
                                            ByVal vintRootGen As Integer, _
                                            ByVal vblnMulMar As Boolean, _
                                            ByVal vintIndex As Integer) As String
        xDataRowToRelationName = String.Empty
        Dim strRelType As String = String.Empty

        Try
            Dim intRelType As Integer = basConst.gcintNONE_VALUE

            If Integer.TryParse(ovbjRowData("RELID"), intRelType) Then

                Select Case FormMode
                    Case emRelMode.Parent
                        '親の情報を設定する   
                        Select Case vintCurGen
                            Case clsEnum.emGender.MALE
                                strRelType = basConst.gcstrFather

                            Case clsEnum.emGender.FEMALE
                                strRelType = basConst.gcstrMother

                        End Select

                    Case emRelMode.Spouse
                        '夫婦の情報を設定する   
                        Select Case mstRootInfo.intGender
                            Case clsEnum.emGender.MALE
                                strRelType = basConst.gcstrWife

                            Case clsEnum.emGender.FEMALE
                                strRelType = basConst.gcstrHusband

                        End Select
                        If vblnMulMar Then strRelType &= String.Format(" {0}", vintIndex + 1)

                    Case emRelMode.Childs
                        '子供達の情報を設定する   
                        Select Case intRelType
                            Case clsEnum.emRelation.NATURAL
                                Select Case vintCurGen
                                    Case clsEnum.emGender.MALE
                                        strRelType = String.Format("{0} {1}", basConst.gcstrKid, basConst.gcstrBoy)

                                    Case clsEnum.emGender.FEMALE
                                        strRelType = String.Format("{0} {1}", basConst.gcstrKid, basConst.gcstrGirl)

                                End Select

                            Case clsEnum.emRelation.ADOPT
                                strRelType = String.Format("{0} {1}", basConst.gcstrKid, basConst.gcstrAdopt)

                        End Select

                End Select

            End If

            xDataRowToRelationName = strRelType

        Catch ex As Exception
            'ログファイルに例外エラーを出力する   
            basCommon.fncSaveErr(mcstrClsName, "xDataRowToRelationName", ex)

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : メンバーID値について行選択状態を設定する    
    '      VALUE      : 無し   
    '      PARAMS     : Integer 、現在データメンバーのID値   
    '      MEMO       : 
    '      CREATE     : 2012/01/03  KBS ANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSelMemById(ByVal vintMemId As Integer)
        Try
            Dim intCurMemId As Integer = basConst.gcintNONE_VALUE

            If String.IsNullOrEmpty(mstrIdField) Then Exit Sub

            For Each objRow As DataGridViewRow In dgvRelation.Rows

                intCurMemId = basConst.gcintNONE_VALUE
                If Not Integer.TryParse(objRow.Cells(mstrIdField).Value.ToString(), intCurMemId) Then Continue For

                If intCurMemId = vintMemId Then
                    objRow.Selected = True
                    Exit Sub
                End If

            Next

        Catch ex As Exception
            'ログファイルに例外エラーを出力する   
            basCommon.fncSaveErr(mcstrClsName, "xSelMemById", ex)

        End Try
    End Sub

#End Region

    Private Sub btnExcelExport_Click(sender As Object, e As EventArgs) Handles btnExcelExport.Click
        DataGridToExcel(dgvRelation, Nothing, lblTitle.Text.Trim)
    End Sub
End Class