'   ****************************************************************** 
'      TITLE      : DRAW F-TREE OPTION
'　　　FUNCTION   :  
'      MEMO       :  
'      CREATE     : 2012/02/17　AKB　Quyet 
'      UPDATE     :  
' 
'           2012 AKB SOFTWARE 
'   ******************************************************************
Option Explicit On
Option Strict Off

''' <summary>
''' Option class
''' </summary>
''' <remarks></remarks>
''' <Create>2012/02/17  AKB Quyet</Create>
Public Class frmPdfOption

    Private Const mcstrClsName As String = "frmPdfOption"                  'class name

    ' ▽ 2017/06/29 AKB Nguyen Thanh Tung --------------------------------
    Private Const mcstr_DEFAUT_GENERATION As Integer = 5
    Private Const mcstr_FOMAT_FAMILY_INFO1 As String = "GIA PHẢ DÒNG HỌ: {0}"
    Private Const mcstr_FOMAT_FAMILY_INFO2 As String = "PHẢ HỆ DÒNG HỌ: {0}"

    Private mstrFName As String

    Private mobjProgressBar As clsProgressBar = Nothing
    Private mobjPdfPrint As clsPdf = Nothing
    Private mblnExportSuscess As Boolean = False
    Private mintNumberExportComplete As Integer = 0
    Private mintMaxDataExportPDF As Integer
    Private mstrFolderTemp As String

    Public Delegate Sub ExportPDF()
    ' △ 2017/06/29 AKB Nguyen Thanh Tung --------------------------------

    Private mstrFamilyInfo As String
    Private mstrFamilyAnniInfo As String
    Private mstrRootInfo As String
    Private mstrCreateDate As String
    Private mfrmPrintPreview As PrintPreview
    Private mstrCreateMember As String
    Private mblnBorder As Boolean

    Private mblnChanged As Boolean = False

    ''' <summary>
    ''' The change is made
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Changed() As Boolean
        Get
            Return mblnChanged
        End Get
    End Property

    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show this form
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : blnIsRollBack   Boolean, flag rollback
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm(ByVal strRootInfo As String, ByVal frmPrintPreview As PrintPreview) As Boolean

        fncShowForm = False

        Try

            Dim strFName As String = "" 'Family Name,
            Dim strFAnni As String = "" 'Information of Root Member
            Dim strFHome As String = "" 'Information about 
            Dim strFAnniInfo As String = ""

            mfrmPrintPreview = frmPrintPreview

            mstrFamilyInfo = ""
            mstrFamilyAnniInfo = ""
            mstrRootInfo = ""
            mstrCreateDate = ""

            ' ▽ 2017/06/29 AKB Nguyen Thanh Tung --------------------------------
            rdoAllGeneration.Checked = True
            txtGeneration.Text = mcstr_DEFAUT_GENERATION
            ' △ 2017/06/29 AKB Nguyen Thanh Tung --------------------------------

            'get family infor
            basCommon.fncGetFamilyInfo(strFName, strFAnni, strFHome)

            If Not basCommon.fncIsBlank(strFHome) Then mstrFamilyAnniInfo = String.Format("NGUYÊN QUÁN: {0}  ", strFHome.ToUpper())
            If Not basCommon.fncIsBlank(strFAnni) Then mstrFamilyAnniInfo &= String.Format("   NGÀY GIỖ TỔ: {0}", strFAnni.ToUpper())
            ' ▽ 2017/08/01 AKB Nguyen Thanh Tung --------------------------------
            'mstrFamilyInfo = String.Format("GIA PHẢ DÒNG HỌ: {0}", strFName.ToUpper())
            mstrFamilyInfo = String.Format(mcstr_FOMAT_FAMILY_INFO1, strFName.ToUpper())
            mstrFName = strFName
            ' △ 2017/08/01 AKB Nguyen Thanh Tung --------------------------------
            If strRootInfo <> "" Then
                mstrRootInfo = "GIA ĐÌNH " & strRootInfo.ToUpper
            End If

            mstrCreateDate = "Ngày tạo: " + dtpDate.Value.ToString("dd/MM/yyyy")

            If mstrFamilyAnniInfo <> "" Then
                txtFamilyAnniInfo.Text = mstrFamilyAnniInfo
            End If
            mstrCreateMember = My.Settings.strCreateMember

            txtRootInfo.Text = mstrRootInfo
            txtFamilyInfo.Text = mstrFamilyInfo
            txtCreateMember.Text = mstrCreateMember

            chkDate.Checked = True
            chkFamilyAnniInfo.Checked = True
            chkRootInfo.Checked = False
            chkFamilyInfo.Checked = True
            chkCreateMember.Checked = True
            chkBorder.Checked = False

            ' ▽ 2017/08/01 AKB Nguyen Thanh Tung --------------------------------
            xBindingCboLocation()
            ' △ 2017/08/01 AKB Nguyen Thanh Tung --------------------------------

            Me.ShowDialog()
            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : FamilyInfo
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property FamilyInfo() As String
        Get
            Return mstrFamilyInfo
        End Get

        Set(ByVal strValue As String)
            mstrFamilyInfo = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : FamilyAnniInfo
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property FamilyAnniInfo() As String
        Get
            Return mstrFamilyAnniInfo
        End Get

        Set(ByVal strValue As String)
            mstrFamilyAnniInfo = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : RootInfo
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property RootInfo() As String
        Get
            Return mstrRootInfo
        End Get

        Set(ByVal strValue As String)
            mstrRootInfo = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : CreatDate
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property CreateDate() As String
        Get
            Return mstrCreateDate
        End Get

        Set(ByVal strValue As String)
            mstrCreateDate = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : CreatDate
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property CreateMember() As String
        Get
            Return mstrCreateMember
        End Get

        Set(ByVal strValue As String)
            mstrCreateMember = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : CreatDate
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property ShowBorder() As Boolean
        Get
            Return mblnBorder
        End Get

        Set(ByVal blnValue As Boolean)
            mblnBorder = blnValue
        End Set

    End Property

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ' ▽ 2017/06/29 AKB Nguyen Thanh Tung --------------------------------
        Dim objPdfPrint As clsPdf = Nothing
        Dim objProgressThread As System.Threading.Thread = Nothing
        Dim objThreadExportPDF As System.Threading.Thread = Nothing

        Try
            ' △ 2017/06/29 AKB Nguyen Thanh Tung ----------------------------

            mblnChanged = True
            mstrCreateDate = dtpDate.Value.ToString("dd/MM/yyyy")
            mstrFamilyAnniInfo = txtFamilyAnniInfo.Text
            mstrFamilyInfo = txtFamilyInfo.Text
            mstrRootInfo = txtRootInfo.Text
            mstrCreateMember = txtCreateMember.Text
            mblnBorder = chkBorder.Checked

            If Not chkDate.Checked Then mstrCreateDate = ""
            If Not chkFamilyAnniInfo.Checked Then mstrFamilyAnniInfo = ""
            If Not chkFamilyInfo.Checked Then mstrFamilyInfo = ""
            If Not chkRootInfo.Checked Then mstrRootInfo = ""
            If Not chkCreateMember.Checked Then mstrCreateMember = ""

            'Dim objPdfPrint As clsPdf = New clsPdf(mintMaxW, mintMaxH)
            objPdfPrint = New clsPdf(mfrmPrintPreview.PdfPagePreview.PageSize.Width, _
                                     mfrmPrintPreview.PdfPagePreview.PageSize.Height, _
                                     fncPdfMetric(mfrmPrintPreview.mintMaxX - mfrmPrintPreview.mintMinX))

            objPdfPrint.FamilyAnniInfo = FamilyAnniInfo
            objPdfPrint.FamilyInfo = FamilyInfo
            objPdfPrint.CreateDate = CreateDate
            objPdfPrint.RootInfo = RootInfo
            objPdfPrint.CreateMember = CreateMember
            objPdfPrint.ShowBorder = ShowBorder

            ' ▽2018/04/27 AKB Nguyen Thanh Tung --------------------------------
            objPdfPrint.FontUser = mfrmPrintPreview.FontUser
            ' △2018/04/27 AKB Nguyen Thanh Tung --------------------------------

            My.Settings.strCreateMember = CreateMember
            My.Settings.Save()

            Me.Cursor = Cursors.WaitCursor

            ' ▽ 2017/06/29 AKB Nguyen Thanh Tung ----------------------------
            If rdoAllGeneration.Checked Then

                objPdfPrint.CreateDate = String.Empty
                objPdfPrint.CreateMember = String.Empty
                ' △ 2017/06/29 AKB Nguyen Thanh Tung ------------------------

                'try to export F-tree to PDF
                'If objPdfPrint.fncExportTree(mtblControl, mlstNormalLine, mlstSpecialLine) Then
                'Dim stRootMember as stCardInfo = stcar
                With mfrmPrintPreview

                    If objPdfPrint.fncExportTree(.mobjImage, .mobjCard, .mlstNormalLine, .mlstSpecialLine) Then

                        ' ▽ 2017/06/29 AKB Nguyen Thanh Tung ----------------
                        'Dim dlgSaveFile As SaveFileDialog = New SaveFileDialog()

                        'dlgSaveFile.CheckPathExists = True
                        'dlgSaveFile.InitialDirectory = Application.StartupPath + "\List"
                        'dlgSaveFile.Title = "Cay pha he " & Now.ToString("ddMMyyyy") & ".pdf"
                        'dlgSaveFile.Filter = "Tệp tin PDF(*.pdf)|*.pdf|Tất cả các file(*.*)|*.*"

                        'If dlgSaveFile.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        '    objPdfPrint.Save(dlgSaveFile.FileName)
                        'End If

                        Dim strText As String = mstrCreateDate

                        If mstrCreateMember <> "" Then
                            If strText <> "" Then
                                strText &= "                      NGƯỜI LẬP: " & mstrCreateMember
                            Else
                                strText = "NGƯỜI LẬP: " & mstrCreateMember
                            End If
                        End If

                        objPdfPrint.fncAddTextLocation(strText.Trim.ToUpper, cboLocation.SelectedIndex)

                        Call xSaveFilePdf(objPdfPrint)
                        mblnExportSuscess = True
                        ' △ 2017/06/29 AKB Nguyen Thanh Tung ----------------
                    End If
                End With

                ' ▽ 2017/06/29 AKB Nguyen Thanh Tung ------------------------
            ElseIf rdoGeneration.Checked Then

                Dim intGenerationMax As Integer

                'Check input Generation
                If Not xCheckInputGeneration(intGenerationMax) Then
                    Me.Cursor = Cursors.Default
                    txtGeneration.Focus()
                    Exit Sub
                End If

                mobjPdfPrint = objPdfPrint
                mstrFolderTemp = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder & "pdf\"
                mintNumberExportComplete = 0

                mobjProgressBar = New clsProgressBar
                mobjProgressBar.fncCreateProgressBar("Đang Xử Lý Tạo File PDF...")
                mobjProgressBar.ComfirmClose = "Bạn có muốn dừng quá trình tạo file PDF?"
                mobjProgressBar.ProcessThread = New System.Threading.Thread(AddressOf InvokeExportPDF)
                mobjProgressBar.fncStartProgressBar()

                If mblnExportSuscess Then
                    Call xSaveFilePdf(mobjPdfPrint)
                End If
            End If
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnOK_Click", ex)
        Finally

            If Not IsNothing(objPdfPrint) Then objPdfPrint.Dispose()
            objPdfPrint = Nothing

            If Not IsNothing(mobjProgressBar) Then
                mobjProgressBar.Dispose()
            End If
            mobjProgressBar = Nothing
            ' △ 2017/06/29 AKB Nguyen Thanh Tung ----------------------------

            Me.Cursor = Cursors.Default
            'Me.Close()

            ' ▽ 2017/06/29 AKB Nguyen Thanh Tung ----------------------------
            If Not mblnExportSuscess Then
                fncMessageError("Quá trình tạo File PDF thất bại.")
            End If
        End Try
        ' △ 2017/06/29 AKB Nguyen Thanh Tung --------------------------------
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        mblnChanged = False
        Me.Close()
    End Sub

#Region "Add by: 2017/06/29 AKB Nguyen Thanh Tung"

    '   ******************************************************************
    '　　　	FUNCTION   : xCheckInputGeneration
    '       VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(OUT) - Integer - Generation input
    '      	MEMO       : Check Input Generation
    '      	CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Function xCheckInputGeneration(ByRef intGeneration As Integer) As Boolean

        xCheckInputGeneration = False

        Try

            'Check Null
            If String.IsNullOrWhiteSpace(txtGeneration.Text) Then
                fncMessageWarning("Bạn chưa chọn số đời.", "")
                Exit Function
            End If

            intGeneration = CInt(txtGeneration.Text)    'Out Generation

            'Generation > 0
            If intGeneration = 0 Then
                fncMessageWarning("Số đời nhập vào lớn hơn 0.", "")
                Exit Function
            End If

            xCheckInputGeneration = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCheckInputGeneration", ex)
            Throw
        End Try
    End Function

    '   ******************************************************************
    '　　　	FUNCTION   : ChangeModeExport
    '      	MEMO       : Change Mode Export
    '      	CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub ChangeModeExport(sender As Object, e As EventArgs) Handles rdoAllGeneration.CheckedChanged, rdoGeneration.CheckedChanged
        Try

            txtGeneration.Enabled = rdoGeneration.Checked
            cboLocation.Enabled = rdoAllGeneration.Checked

            If rdoGeneration.Checked Then
                mstrFamilyInfo = String.Format(mcstr_FOMAT_FAMILY_INFO2, mstrFName.ToUpper())
            Else
                mstrFamilyInfo = String.Format(mcstr_FOMAT_FAMILY_INFO1, mstrFName.ToUpper())
            End If

            txtFamilyInfo.Text = mstrFamilyInfo
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "ChangeModeExport", ex)
        End Try
    End Sub

    '   ******************************************************************
    '		FUNCTION   : xSaveFilePdf
    '		PARAMS     : ARG1(IN) - clsPdf
    '		MEMO       : Save file pdf export
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Sub xSaveFilePdf(ByVal objPdfPrint As clsPdf)

        Dim dlgSaveFile As SaveFileDialog = Nothing

        Try
            If IsNothing(objPdfPrint) Then Exit Sub

            dlgSaveFile = New SaveFileDialog()
            dlgSaveFile.CheckPathExists = True
            dlgSaveFile.InitialDirectory = Application.StartupPath + "\List"
            dlgSaveFile.Title = "Cay pha he " & Now.ToString("ddMMyyyy") & ".pdf"
            dlgSaveFile.Filter = "Tệp tin PDF(*.pdf)|*.pdf|Tất cả các file(*.*)|*.*"

            If dlgSaveFile.ShowDialog() = Windows.Forms.DialogResult.OK Then
                objPdfPrint.Save(dlgSaveFile.FileName)
            End If
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSaveFilePdf", ex)
            Throw
        Finally
            If Not IsNothing(dlgSaveFile) Then dlgSaveFile.Dispose()
            dlgSaveFile = Nothing
        End Try
    End Sub

    '   ******************************************************************
    '　　　	FUNCTION   : InvokeExportPDF
    '      	MEMO       : Call Export PDF in Thead
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub InvokeExportPDF()

        If Me.InvokeRequired Then

            Me.Invoke(New ExportPDF(AddressOf InvokeExportPDF))

        Else
            Try

                mblnExportSuscess = False

                If Not xExportPDF(mobjPdfPrint, mfrmPrintPreview.TreeDraw, CInt(txtGeneration.Text), mstrFolderTemp) Then
                    Exit Sub
                End If

                mobjProgressBar.Percent = 100
                mblnExportSuscess = True
                Application.DoEvents()
            Catch ex As Exception

            Finally
                mobjProgressBar.CompleteProcess = True
                Try
                    basCommon.fncDeleteFolder(mstrFolderTemp)
                Catch
                End Try
            End Try
        End If
    End Sub

    '   ******************************************************************
    '		FUNCTION   : xExportPDF
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - clsPdf
    '                    ARG2(IN) - Object - Tree Draw
    '                    ARG3(IN) - Integer - Generation Max
    '		MEMO       : Export Tree to PDF
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function xExportPDF(ByVal objPdfPrint As clsPdf,
                                ByVal objTreeDraw As Object,
                                ByVal intGenerationMax As Integer,
                                ByVal strFolder As String) As Boolean

        xExportPDF = False

        Dim objDrawTreeS1 As clsDrawTreeS1 = Nothing
        Dim objDrawTreeS2 As clsDrawTreeS2 = Nothing
        Dim objDrawTreeS3 As clsDrawTreeS3 = Nothing
        Dim objDrawTreeSS As clsDrawTreeSS = Nothing
        Dim objDrawTreeA1 As clsDrawTreeA1 = Nothing
        Dim tblData As DataTable = Nothing
        Dim stSearch As clsDbAccess.stSearch

        Try

            If IsNothing(objPdfPrint) OrElse IsNothing(objTreeDraw) Then
                Exit Function
            End If

            stSearch = New clsDbAccess.stSearch With {.strKeyword = String.Empty, .intGender = clsEnum.emGender.UNKNOW}
            tblData = gobjDB.fncGetQuickSearch(stSearch)
            If IsNothing(tblData) Then Exit Function
            mintMaxDataExportPDF = tblData.Rows.Count

            'Create Page Info
            objPdfPrint.fncCreatePageInfo()

            Application.DoEvents()

            'Draw Tree
            If TypeOf objTreeDraw Is clsDrawTreeS1 Then 'Kiểu Khung 1: Cây Cơ Bản

                objDrawTreeS1 = New clsDrawTreeS1(CType(objTreeDraw, clsDrawTreeS1).RootID, intGenerationMax)
                objDrawTreeS1.fncGetData()

                xExportTreeS1(objPdfPrint, objDrawTreeS1, objDrawTreeS1.RootID, intGenerationMax, strFolder, 1)
            ElseIf TypeOf objTreeDraw Is clsDrawTreeS2 Then 'Kiểu Khung 2

                objDrawTreeS2 = New clsDrawTreeS2
                objDrawTreeS2.fncGetData()

                xExportTreeS2(objPdfPrint, objDrawTreeS2, tblData, CType(objTreeDraw, clsDrawTreeS2).RootID, intGenerationMax, strFolder, 1)
            ElseIf TypeOf objTreeDraw Is clsDrawTreeS3 Then

            ElseIf TypeOf objTreeDraw Is clsDrawTreeSS Then

            ElseIf TypeOf objTreeDraw Is clsDrawTreeA1 Then 'Kiểu Khung 1: Cây Mở Rộng

                objDrawTreeA1 = New clsDrawTreeA1(CType(objTreeDraw, clsDrawTreeA1).RootID, intGenerationMax)
                objDrawTreeA1.fncGetData()

                xExportTreeA1(objPdfPrint, objDrawTreeA1, objDrawTreeA1.RootID, intGenerationMax, strFolder, 1)
            End If

            Application.DoEvents()

            xExportPDF = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xExportPDF", ex)
            Throw
        Finally

            If Not IsNothing(objDrawTreeS1) Then objDrawTreeS1.Dispose()
            If Not IsNothing(objDrawTreeS2) Then objDrawTreeS2.Dispose()
            If Not IsNothing(objDrawTreeS3) Then objDrawTreeS3.Dispose()
            If Not IsNothing(objDrawTreeSS) Then objDrawTreeSS.Dispose()
            If Not IsNothing(objDrawTreeA1) Then objDrawTreeA1.Dispose()
            If Not IsNothing(tblData) Then tblData.Dispose()

            objDrawTreeS1 = Nothing
            objDrawTreeS2 = Nothing
            objDrawTreeS3 = Nothing
            objDrawTreeSS = Nothing
            objDrawTreeA1 = Nothing
            tblData = Nothing
            stSearch = Nothing
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xGetExportInfS1
    '		VALUE      : basConst.stExportInfo
    '		PARAMS     : ARG1(IN) - clsDrawTreeS1
    '                    ARG2(IN) - Integer - RootID Card
    '                    ARG3(IN) - Integer - Generation Max
    '                    ARG4(OUT) - List(Of Integer) - List Root ID Max Generation
    '		MEMO       : Get Data Export Tree S1
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function xGetExportInfS1(ByVal objDrawTreeS1 As clsDrawTreeS1,
                                     ByVal intRootID As Integer,
                                     ByVal intGenerationMax As Integer,
                                     ByRef lstRootIDGenerationMax As List(Of Integer)) As basConst.stExportInfo

        xGetExportInfS1 = Nothing

        Dim stExportInf As basConst.stExportInfo = Nothing

        Try

            If Not objDrawTreeS1.fncDrawPDF(intRootID, intGenerationMax, clsDefine.TREE_S1_STARTX, clsDefine.TREE_S1_STARTY, lstRootIDGenerationMax) Then
                Exit Function
            End If

            stExportInf = New basConst.stExportInfo
            stExportInf.intRootID = intRootID
            stExportInf.tblControl = objDrawTreeS1.DrawingCard.Clone
            stExportInf.lstNormalLine = objDrawTreeS1.NormalLine
            stExportInf.lstSpecialLine = objDrawTreeS1.SpecialLine
            stExportInf.tblMemberInfo = objDrawTreeS1.DrawList.Clone

            Application.DoEvents()

            xGetExportInfS1 = stExportInf
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetExportInfS1", ex)
            Throw
        Finally
            stExportInf = Nothing
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xExportTreeS1
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - clsPdf
    '                    ARG2(IN) - clsDrawTreeS1
    '                    ARG3(IN) - Integer - RootID Card
    '                    ARG4(IN) - Integer - Generation Max
    '                    ARG5(IN) - String - Folder Temp
    '                    ARG6(Optional) - Integer - Generation Start Draw (Default: 1)
    '		MEMO       : Export Tree S1
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function xExportTreeS1(ByVal objPdfPrint As clsPdf,
                                   ByVal objDrawTreeS1 As clsDrawTreeS1,
                                   ByVal intRootID As Integer,
                                   ByVal intGenerationMax As Integer,
                                   ByVal strFolderTemp As String,
                                   ByRef intNumberPage As Integer,
                                   Optional intGenerationStart As Integer = 1) As Boolean

        xExportTreeS1 = False

        Const cstr_TITLE As String = "Phả hệ từ {0} đời thứ {1} ngày xuất bản {2}"
        Dim stExportInf As New basConst.stExportInfo
        Dim stCardRoot As basConst.stCardInfo
        Dim lstRootIDGenerationMax As List(Of Integer) = Nothing
        Dim strTitle As String

        Try

            stExportInf = xGetExportInfS1(objDrawTreeS1, intRootID, intGenerationMax, lstRootIDGenerationMax)

            If Not IsNothing(stExportInf) Then

                If intGenerationStart > 1 AndAlso intGenerationMax > 1 Then

                    stCardRoot = CType(stExportInf.tblMemberInfo.Item(intRootID), basConst.stCardInfo)

                    If IsNothing(stCardRoot.lstChild) Then
                        Exit Function
                    End If
                End If

                strTitle = String.Format(cstr_TITLE, objDrawTreeS1.RootMemberInfo, intGenerationStart, DateTime.Now.ToString("dd-MM-yyyy"))

                xExportPagePdf(objPdfPrint, stExportInf, strTitle.ToUpper, strFolderTemp, intNumberPage)
                Application.DoEvents()

                If IsNothing(lstRootIDGenerationMax) OrElse lstRootIDGenerationMax.Count = 0 Then Exit Function

                For Each intID As Integer In lstRootIDGenerationMax
                    xExportTreeS1(objPdfPrint, objDrawTreeS1, intID, intGenerationMax, strFolderTemp, intNumberPage, intGenerationStart + intGenerationMax - 1)
                    Application.DoEvents()
                Next
            End If

            xExportTreeS1 = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xExportTreeS1", ex)
            Throw
        Finally
            stExportInf = Nothing
            stCardRoot = Nothing
            lstRootIDGenerationMax = Nothing
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xGetExportInfS2
    '		VALUE      : basConst.stExportInfo
    '		PARAMS     : ARG1(IN) - clsDrawTreeS1
    '                    ARG2(IN) - DataTable - Data Member
    '                    ARG3(IN) - Integer - RootID Card
    '                    ARG4(IN) - Integer - Generation Max
    '                    ARG5(OUT) - List(Of Integer) - List Root ID Max Generation
    '		MEMO       : Get Data Export Tree S1
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function xGetExportInfS2(ByVal objDrawTreeS2 As clsDrawTreeS2,
                                     ByVal tblData As DataTable,
                                     ByVal intRootID As Integer,
                                     ByVal intGenerationMax As Integer,
                                     ByRef lstRootIDGenerationMax As List(Of Integer)) As basConst.stExportInfo

        xGetExportInfS2 = Nothing

        Dim stExportInf As basConst.stExportInfo = Nothing

        Try

            If Not objDrawTreeS2.fncDrawPDF(intRootID, tblData, intGenerationMax, lstRootIDGenerationMax) Then
                Exit Function
            End If

            stExportInf = New basConst.stExportInfo
            stExportInf.intRootID = intRootID
            stExportInf.tblControl = objDrawTreeS2.DrawingCard.Clone
            stExportInf.lstNormalLine = objDrawTreeS2.NormalLine
            stExportInf.lstSpecialLine = objDrawTreeS2.SpecialLine

            Application.DoEvents()

            xGetExportInfS2 = stExportInf
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetExportInfS2", ex)
            Throw
        Finally
            stExportInf = Nothing
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xExportTreeS2
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - clsPdf
    '                    ARG2(IN) - clsDrawTreeS1
    '                    ARG3(IN) - DataTable - Data Member
    '                    ARG4(IN) - Integer - RootID Card
    '                    ARG5(IN) - Integer - Generation Max
    '                    ARG6(IN) - String - Folder Temp
    '                    ARG7(Optional) - Integer - Generation Start Draw (Default: 1)
    '		MEMO       : Export Tree S1
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function xExportTreeS2(ByVal objPdfPrint As clsPdf,
                                   ByVal objDrawTreeS2 As clsDrawTreeS2,
                                   ByVal tblData As DataTable,
                                   ByVal intRootID As Integer,
                                   ByVal intGenerationMax As Integer,
                                   ByVal strFolderTemp As String,
                                   ByRef intNumberPage As Integer,
                                   Optional intGenerationStart As Integer = 1) As Boolean

        xExportTreeS2 = False

        Const cstr_TITLE As String = "Phả hệ từ {0} đời thứ {1} ngày xuất bản {2}"
        Dim stExportInf As New basConst.stExportInfo
        Dim objCardRoot As usrMemCardBase = Nothing
        Dim lstRootIDGenerationMax As List(Of Integer) = Nothing
        Dim strTitle As String

        Try

            stExportInf = xGetExportInfS2(objDrawTreeS2, tblData, intRootID, intGenerationMax, lstRootIDGenerationMax)

            If Not IsNothing(stExportInf) Then

                If intGenerationStart > 1 AndAlso intGenerationMax > 1 Then

                    Dim blnFlag As Boolean = False

                    If stExportInf.tblControl.Count = 1 Then Exit Function

                    For Each element As DictionaryEntry In stExportInf.tblControl

                        objCardRoot = CType(element.Value, usrMemCardBase)

                        If objCardRoot.DrawLv <> 1 Then
                            blnFlag = True
                            Exit For
                        End If
                    Next

                    If Not blnFlag Then
                        Exit Function
                    End If
                End If

                strTitle = String.Format(cstr_TITLE, objDrawTreeS2.RootMemberInfo, intGenerationStart, DateTime.Now.ToString("dd-MM-yyyy"))

                xExportPagePdf(objPdfPrint, stExportInf, strTitle.ToUpper, strFolderTemp, intNumberPage)
                Application.DoEvents()

                If IsNothing(lstRootIDGenerationMax) OrElse lstRootIDGenerationMax.Count = 0 Then Exit Function

                For Each intID As Integer In lstRootIDGenerationMax
                    xExportTreeS2(objPdfPrint, objDrawTreeS2, tblData, intID, intGenerationMax, strFolderTemp, intNumberPage, intGenerationStart + intGenerationMax - 1)
                    Application.DoEvents()
                Next
            End If

            xExportTreeS2 = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xExportTreeS2", ex)
            Throw
        Finally
            If Not IsNothing(objCardRoot) Then objCardRoot.Dispose()

            stExportInf = Nothing
            objCardRoot = Nothing
            lstRootIDGenerationMax = Nothing
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xGetExportInfA1
    '		VALUE      : basConst.stExportInfo
    '		PARAMS     : ARG1(IN) - clsDrawTreeA1
    '                    ARG2(IN) - Integer - RootID Card
    '                    ARG3(IN) - Integer - Generation Max
    '                    ARG4(OUT) - List(Of Integer) - List Root ID Max Generation
    '		MEMO       : Get Data Export Tree S1
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function xGetExportInfA1(ByVal objDrawTreeA1 As clsDrawTreeA1,
                                     ByVal intRootID As Integer,
                                     ByVal intGenerationMax As Integer,
                                     ByRef lstRootIDGenerationMax As List(Of Integer)) As basConst.stExportInfo

        xGetExportInfA1 = Nothing

        Dim stExportInf As basConst.stExportInfo = Nothing

        Try

            If Not objDrawTreeA1.fncDrawPDF(intRootID, intGenerationMax, clsDefine.TREE_S1_STARTX, clsDefine.TREE_S1_STARTY, lstRootIDGenerationMax) Then
                Exit Function
            End If

            stExportInf = New basConst.stExportInfo
            stExportInf.intRootID = intRootID
            stExportInf.tblControl = objDrawTreeA1.DrawingCard.Clone
            stExportInf.lstNormalLine = objDrawTreeA1.NormalLine
            stExportInf.lstSpecialLine = objDrawTreeA1.SpecialLine
            stExportInf.tblMemberInfo = objDrawTreeA1.DrawList.Clone

            Application.DoEvents()

            xGetExportInfA1 = stExportInf
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetExportInfA1", ex)
            Throw
        Finally
            stExportInf = Nothing
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xExportTreeA1
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - clsPdf
    '                    ARG2(IN) - clsDrawTreeA1
    '                    ARG3(IN) - Integer - RootID Card
    '                    ARG4(IN) - Integer - Generation Max
    '                    ARG5(IN) - String - Folder Temp
    '                    ARG6(Optional) - Integer - Generation Start Draw (Default: 1)
    '		MEMO       : Export Tree A1
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function xExportTreeA1(ByVal objPdfPrint As clsPdf,
                                   ByVal objDrawTreeA1 As clsDrawTreeA1,
                                   ByVal intRootID As Integer,
                                   ByVal intGenerationMax As Integer,
                                   ByVal strFolder As String,
                                   ByRef intNumberPage As Integer,
                                   Optional intGenerationStart As Integer = 1) As Boolean

        xExportTreeA1 = False

        Const cstr_TITLE As String = "Phả hệ từ {0} đời thứ {1} ngày xuất bản {2}"
        Dim stExportInf As New basConst.stExportInfo
        Dim stCardRoot As basConst.stCardInfo
        Dim stCardSpouse As basConst.stCardInfo
        Dim lstRootIDGenerationMax As List(Of Integer) = Nothing
        Dim strTitle As String

        Try

            stExportInf = xGetExportInfA1(objDrawTreeA1, intRootID, intGenerationMax, lstRootIDGenerationMax)

            If Not IsNothing(stExportInf) Then

                If intGenerationStart > 1 AndAlso intGenerationMax > 1 Then

                    stCardRoot = CType(stExportInf.tblMemberInfo.Item(intRootID), basConst.stCardInfo)

                    If IsNothing(stCardRoot.lstChild) AndAlso IsNothing(stCardRoot.lstStepChild) Then

                        If Not IsNothing(stCardRoot.lstSpouse) Then

                            Dim blnFlag As Boolean = False

                            For Each intSpouseID As Integer In stCardRoot.lstSpouse

                                stCardSpouse = CType(stExportInf.tblMemberInfo.Item(intSpouseID), basConst.stCardInfo)

                                If Not IsNothing(stCardSpouse.lstStepChild) OrElse Not IsNothing(stCardSpouse.lstChild) Then
                                    blnFlag = True
                                    Exit For
                                End If
                            Next

                            If blnFlag = False Then
                                Exit Function
                            End If
                        Else
                            Exit Function
                        End If
                    End If
                End If

                strTitle = String.Format(cstr_TITLE, objDrawTreeA1.RootMemberInfo, intGenerationStart, DateTime.Now.ToString("dd-MM-yyyy"))

                xExportPagePdf(objPdfPrint, stExportInf, strTitle.ToUpper, strFolder, intNumberPage)
                Application.DoEvents()

                If IsNothing(lstRootIDGenerationMax) OrElse lstRootIDGenerationMax.Count = 0 Then Exit Function

                For Each intID As Integer In lstRootIDGenerationMax
                    xExportTreeA1(objPdfPrint, objDrawTreeA1, intID, intGenerationMax, strFolder, intNumberPage, intGenerationStart + intGenerationMax - 1)
                    Application.DoEvents()
                Next
            End If

            xExportTreeA1 = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xExportTreeA1", ex)
            Throw
        Finally
            stExportInf = Nothing
            stCardRoot = Nothing
            lstRootIDGenerationMax = Nothing
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xExportPagePdf
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - clsPdf
    '                    ARG2(IN) - basConst.stExportInfo 
    '                    ARG3(IN) - String - Title Page
    '                    ARG4(IN) - String - Folder Temp
    '                    ARG5(OUT) - Integer - Number Page Next
    '		MEMO       : Export a Page PDF
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function xExportPagePdf(ByVal objPdfPrint As clsPdf,
                                    ByVal stExportInf As basConst.stExportInfo,
                                    ByVal strTitle As String,
                                    ByVal strFolderTemp As String,
                                    ByRef intNumberPage As Integer) As Boolean

        xExportPagePdf = False

        Dim arrCard() As usrMemCardBase = Nothing
        Dim objImage() As PdfSharp.Drawing.XImage = Nothing
        Dim intHeight As Integer
        Dim intWidth As Integer

        Try

            xGetImgCardPDF(stExportInf, strFolderTemp, intNumberPage, arrCard, objImage, intWidth, intHeight)

            objPdfPrint.fncAddpage(fncPdfMetric(intWidth), fncPdfMetric(intHeight))
            objPdfPrint.fncExportTree2(objImage, arrCard, stExportInf.lstNormalLine, stExportInf.lstSpecialLine, strTitle)
            objPdfPrint.fncAddNumberPage(intNumberPage)

            intNumberPage += 1
            mintNumberExportComplete += arrCard.Length
            mobjProgressBar.Percent = mobjProgressBar.fncCalculatePercent(mintNumberExportComplete, mintMaxDataExportPDF)

            xExportPagePdf = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xExportPagePdf", ex)
            Throw
        Finally
            arrCard = Nothing
            objImage = Nothing
        End Try
    End Function

    '   ******************************************************************
    '　　　	FUNCTION   : xGetImgCard
    '		PARAMS     : ARG1(IN) - basConst.stExportInfo
    '                    ARG2(IN) - String - Folder Temp
    '                    ARG3(IN) - String - Folder Temp
    '                    ARG4(OUT) - usrMemCardBase() - Array Card
    '                    ARG5(OUT) - PdfSharp.Drawing.XImage() - Array Image Card
    '                    ARG6(OUT) - Integer
    '                    ARG7(OUT) - Integer
    '      	MEMO       : Get Image to Card And Calculate Width, Height to Tree
    '      	CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Function xGetImgCardPDF(ByVal stExportInf As basConst.stExportInfo,
                                    ByVal strFolder As String,
                                    ByVal intPage As Integer,
                                    ByRef arrCard() As usrMemCardBase,
                                    ByRef objImage() As PdfSharp.Drawing.XImage,
                                    ByRef intWidth As Integer,
                                    ByRef intHeight As Integer) As String

        xGetImgCardPDF = Nothing

        Dim i As Integer
        Dim intId As Integer
        Dim strPath As String

        Dim objCard As usrMemCardBase = Nothing
        Dim NoAvatar_Img As PdfSharp.Drawing.XImage = fncMakeImage(My.Application.Info.DirectoryPath & "\docs\no_avatar_m.jpg")
        Dim NoAvatar_F_Img As PdfSharp.Drawing.XImage = fncMakeImage(My.Application.Info.DirectoryPath & "\docs\no_avatar_f.jpg")
        Dim UnKnowAvatar_F_Img As PdfSharp.Drawing.XImage = fncMakeImage(My.Application.Info.DirectoryPath & "\docs\UnknownMember.jpg")

        Const intPadding As Integer = 20
        Dim intMinX As Integer
        Dim intMaxX As Integer
        Dim intMinY As Integer
        Dim intMaxY As Integer

        Try

            ReDim objImage(stExportInf.tblControl.Count - 1)
            ReDim arrCard(stExportInf.tblControl.Count - 1)

            strFolder = strFolder & intPage.ToString & "\"
            i = -1

            If Not basCommon.fncCreateFolder(strFolder, True) Then Exit Function

            For Each element As DictionaryEntry In stExportInf.tblControl

                objCard = CType(element.Value, usrMemCardBase)

                If objCard.Visible = True Then

                    i = i + 1
                    intId = CInt(element.Key)
                    arrCard(i) = objCard

                    'Calculate Max, Min of Location X
                    If intMinX > objCard.CardCoor.X Then
                        intMinX = objCard.CardCoor.X
                    End If

                    If intMaxX < objCard.CardCoor.X + objCard.Width Then

                        intMaxX = objCard.CardCoor.X + objCard.Width

                    End If

                    'Calculate Max, Min of Location Y
                    If intMinY > objCard.CardCoor.Y Then
                        intMinY = objCard.CardCoor.Y
                    End If

                    If intMaxY < objCard.CardCoor.Y + objCard.Height Then

                        intMaxY = objCard.CardCoor.Y + objCard.Height

                    End If

                    strPath = arrCard(i).fncGetImage(strFolder)

                    If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD2 Then
                        objImage(i) = PdfSharp.Drawing.XImage.FromFile(strPath)
                    Else
                        objImage(i) = mfrmPrintPreview.xGetMemberAvatarImage(CType(arrCard(i), usrMemberCard1))
                    End If
                End If
            Next

            'Calculate Width, Height of Tree
            intWidth = intMaxX - intMinX + 2 * intPadding
            intHeight = intMaxY - intMinY + 2 * intPadding

            xGetImgCardPDF = strFolder
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetImgCard", ex)
            Throw
        End Try
    End Function

    '   ******************************************************************
    '　　　	FUNCTION   : xBindingCboLocation
    '      	MEMO       : 
    '      	CREATE     : 2017/08/01 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub xBindingCboLocation()
        Try

            cboLocation.Items.Clear()
            cboLocation.Items.Add("TRÊN - TRÁI")
            cboLocation.Items.Add("TRÊN - PHẢI")
            cboLocation.Items.Add("DƯỚI - TRÁI")
            cboLocation.Items.Add("DƯỚI - PHẢI")
            cboLocation.SelectedIndex = 0
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xBindingCboLocation", ex)
            Throw
        End Try
    End Sub
#End Region
End Class