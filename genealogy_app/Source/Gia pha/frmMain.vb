Option Explicit On
Option Strict Off
Imports System.Collections.Specialized
Imports System.IO
'   ******************************************************************
'      TITLE      : MAIN FORM
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/09/14　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Imports System.Net
Imports Config_Gia_Pha
Imports ICSharpCode.SharpZipLib.Zip
Imports OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
'   ******************************************************************
'　　　FUNCTION   : Form Main class
'      MEMO       : 
'      CREATE     : 2011/09/14  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmMain

#Region "Form constants"

    Private Const mcstrClsName As String = "frmMain"                                    'class name

    Private Const mcstrErrorDrawing As String = "Lỗi hiển thị cây phả hệ."                      'error when drawing family tree
    Private Const mcstrPdfMissing As String = "Không tìm thấy tệp tin hướng dẫn."                'can not find pdf file
    Private Const mcstrDateFormat As String = "{0} ({1})"                               'date format
    Private Const mcstrCurTimeFormat As String = "Giờ hiện tại : {0}"                        'current time format
    Private Const mcstrBirth As String = "BIRTH_DAY"                                    'birth day
    Private Const mcstrDeath As String = "DECEASED_DATE"                                'decease day
    Private Const mcstrFieldMEMBER_ID As String = "MEMBER_ID"                           'member id field
    Private Const mcstrNextBirth As String = "Sinh nhật gần nhất : "                          'next birth text
    Private Const mcstrNextDeath As String = "Ngày giỗ gần nhất : "                          'next death text
    Private Const mcstrNoResult As String = "Không có"                                  'no result text

    Private Const mcintSttClmIndex As Integer = 0                                       'constance for printing
    Private Const mcintWM_PRINT As Integer = &H317                                      'constance for printing

    ' ▽ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------
    Private mcintItemPerPage As Integer = 100
    'Private Const mcintItemPerPage As Integer = 100                                     'paging - number of item per page
    ' △ 2017/06/02 AKB Nguyen Thanh Tung --------------------------------
    Private Const mcstrMemberNotDrawn As String = "Thành viên này không được hiển thị trên cây."             'member is not drawn
    Private Const mcstrPdfRequire As String = "Bạn cần cài đặt chương trình đọc file pdf để sử dụng chức năng này." 'pdf reader require
    Private Const mcstrRestoreSuccess As String = "Bạn cần khởi động lại chương trình để cập nhật dữ liệu mới."    'restored successfully
    Private mblnOptChanged As Boolean = True
    Private Const mcintMaxDefaultDrawLevel As Integer = 5

#End Region

#Region "Form variable"

    Private mintID As Integer                                       'member id
    Private mintGender As clsEnum.emGender                          'member gender
    Private mintGeneration As Integer                               'generation
    Private mintRootID As Integer                                   'root id

    Private mintCurPage As Integer                                  'current page
    Private mintTotalPage As Integer                                'total page
    Private mintCurGeneration As Integer                            'current generation of selected member

    Private mmnuRightMouse As ContextMenuStrip                      'right mouse on grid

    Private mblnDragdrop As Boolean = False                         'flag drag and drop
    Private mblnDrawCompactTree As Boolean = False                  'draw tree mode
    Private mstrBackupPath As String                                'backup path
    Private mstrAnniBirthList As String                             'anniversary list
    Private mstrAnniDeceaseList As String                           'anniversary list
    Private memFormMode As emFormMode                               'form mode

    Private mstSearchInfo As clsDbAccess.stSearch                   'information struture to search
    Private mstSearchData As stSearchData                           'search data structure

    Private mtblData As DataTable                                   'table to store data
    Private mtblRel As DataTable                                    'table to store relationship
    Private mtblGridSource As DataTable                             'table to bind on grid

    Private mclsVnCal As clsLunarCalendar                           'lunar calendar instance
    Private mclsDrawCard As clsDrawCard                             'class to draw member card
    Private mclsDrawTreeS1 As clsDrawTreeS1                         'class to draw family tree
    Private mclsDrawTreeS2 As clsDrawTreeS2                         'class to draw family tree
    Private mclsDrawTreeS3 As clsDrawTreeS3                         'class to draw family tree
    'Private mclsDrawTreeS3 As clsDrawTreeSS                         'class to draw family tree

    Private mclsDrawTreeA1 As clsDrawTreeA1
    Private mclsRightMenu As clsRightMenu                           'right menu on grid

    Private mfrmPerInfo As frmPersonInfo                            'form to add or edit user
    Private mfrmWaiting As frmProgress                              'waiting screen
    Private mfrmAnni As frmPersonalAnniversary                      'anniversary form
    Private mobjLoadingThread As System.Threading.Thread            'loading thread
    Private mobjCardThread As System.Threading.Thread            'loading thread

    Private mpnShowTree As pnTreePanel
    Private memCurTree As clsEnum.emCardStyle                       'current style of tree (tree1/tree2)
    Private mobjPrgBar As frmProgressBar
    Private mintLastDrawType As Integer

    ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
    Public mblnUpdatePrint As Boolean = False
    Public mintPrintPageSizeSelected As Integer
    Public mintPrintPageZoomSelected As Integer
    Public mblnPrintPageLandScape As Boolean
    ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
#End Region

    Private Enum enDrawingOptions As Integer

        PRF_CHECKVISIBLE = &H1
        PRF_NONCLIENT = &H2
        PRF_CLIENT = &H4
        PRF_ERASEBKGND = &H8
        PRF_CHILDREN = &H10
        PRF_OWNED = &H20

    End Enum


    Private Enum emFormMode As Integer

        SHOW_CARD
        SHOW_TREE_FULL
        SHOW_TREE_COMPACT

    End Enum

    Declare Auto Function SendMessage Lib "user32" (
        ByVal hWnd As IntPtr,
        ByVal intMsg As Integer,
        ByVal wParam As IntPtr,
        ByVal intParam As Integer) As Integer


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

        'Dim dtDie As Date                           'decease date
        Dim intDday As Integer
        Dim intDmon As Integer
        Dim intDyea As Integer

        Dim intDecease As Integer                   'death or alive
        Dim intGender As Integer                    'gender
        Dim intLevel As Integer                     'generation

    End Structure



#Region "Menu Event"


    '   ******************************************************************
    '　　　FUNCTION   : tsmSysUserInfo_Click, menu item click
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmSysUserInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmSysUserInfo.Click

        Dim frmUserInfo As frmPassChange = Nothing

        Try

            frmUserInfo = New frmPassChange

            If frmUserInfo.fncShowForm() Then

                frmUserInfo.Dispose()

            End If

        Catch ex As Exception

            frmUserInfo = Nothing
            basCommon.fncSaveErr(mcstrClsName, "tsmSysUserInfo_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmSysQuit_Click, menu item click
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmSysQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmSysQuit.Click

        Try

            'mpnShowTree.Visible = False

            basCommon.fncDeleteFolder("temp")

            Me.Close()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "tsmSysQuit_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmFamilyInfo_Click, menu item click
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmFamilyInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmFamilyInfo.Click

        Dim frmFamily As frmFamilyInfo

        Try

            frmFamily = New frmFamilyInfo

            If frmFamily.fncShowForm() Then

                frmFamily.Dispose()

            End If

        Catch ex As Exception

            frmFamily = Nothing
            basCommon.fncSaveErr(mcstrClsName, "tsmFamilyInfo_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmFamilyNewMem_Click, add new member
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmFamilyNewMem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmFamilyNewMem.Click

        Try

            mfrmPerInfo.FormMode = clsEnum.emMode.ADD

            mfrmPerInfo.fncShowForm()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmFamilyNewMem_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmFamilyBuild_Click, menu item click
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmFamilyBuild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmFamilyBuild.Click

        Try
            'do nothing if it is already in SHOW_CARD mode
            If memFormMode = emFormMode.SHOW_CARD Then Exit Sub

            xShowViewTree(False)

            xSetSelectedRow(mintID)

            'xShowViewTree(False)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmFamilyBuild_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmFamilyShowTree_Click, menu item click
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmFamilyShowTree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles tsmFamilyShowTree.Click

        Try

            If mclsDrawCard.ActiveMemberID <= basConst.gcintNO_MEMBER Then Exit Sub

            xShowViewTree(True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmFamilyShowTree_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmFamilySearch_Click, menu item click
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmFamilySearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmFamilySearch.Click

        Dim frmSearchInfo As frmSearch

        Try

            frmSearchInfo = New frmSearch(mtblData)

            If frmSearchInfo.fncShowForm() Then

                frmSearchInfo.Dispose()

            End If

        Catch ex As Exception

            frmSearchInfo = Nothing
            basCommon.fncSaveErr(mcstrClsName, "tsmFamilySearch_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmHelpAbout_Click, show about dialog
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmHelpAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmHelpAbout.Click

        Dim frmAboutDialog As frmAbout = Nothing

        Try
            frmAboutDialog = New frmAbout()

            frmAboutDialog.ShowDialog()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmHelpAbout_Click", ex)
        Finally
            If frmAboutDialog IsNot Nothing Then frmAboutDialog.Dispose()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmSysDataBackup_Click, backup data
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmSysDataBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmSysDataBackup.Click

        Dim objSaveFile As SaveFileDialog = Nothing

        Try
            objSaveFile = New SaveFileDialog()

            objSaveFile.DefaultExt = basConst.gcstrBackupFileExt
            objSaveFile.Filter = basConst.gcstrBackupFileFilter

            'show save file dialog
            If objSaveFile.ShowDialog() = Windows.Forms.DialogResult.OK Then

                mstrBackupPath = objSaveFile.FileName

                mfrmWaiting = New frmProgress()
                mobjLoadingThread = New System.Threading.Thread(AddressOf xBackup)

                mobjLoadingThread.Start()
                mfrmWaiting.ShowDialog()

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmSysDataBackup_Click", ex)
        Finally
            If objSaveFile IsNot Nothing Then objSaveFile.Dispose()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmSysDataRestore_Click, restore data
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmSysDataRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmSysDataRestore.Click

        Try
            Dim frmRest As frmRestore

            frmRest = New frmRestore()

            'show restore form
            If Not frmRest.fncShowForm() Then Exit Sub

            If frmRest.Restored Then

                'force closing for update new data
                basCommon.fncMessageInfo(mcstrRestoreSuccess)
                mpnShowTree.Visible = False
                Me.Close()

                ''re-build data
                ''xQuickSearch()
                'xSetStartID()

                ''show family card
                'xShowViewTree(False)

            End If

            frmRest.Dispose()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmSysDataRestore_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmPersonalInfo_Click, show person infor
    '      MEMO       : 
    '      CREATE     : 2011/12/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmPersonalInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmPersonalInfo.Click

        Try
            xShowPersonInfo(mintID)


            'If mintID <= basConst.gcintNO_MEMBER Then Exit Sub

            ''show form in edit mode
            'mfrmPerInfo.FormMode = clsEnum.emMode.EDIT
            'mfrmPerInfo.MemberID = mintID
            'mfrmPerInfo.fncShowForm()

            'If mfrmPerInfo.FormModified Then xUpdate()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmPersonalInfo_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmPersonalFa_Click, show father - mother
    '      MEMO       : 
    '      CREATE     : 2011/12/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmPersonalFa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmPersonalFa.Click

        Dim fncRel As New frmRelationMem
        Try
            fncRel.RootID = mintID
            fncRel.FormMode = frmRelationMem.emRelMode.Parent
            AddHandler fncRel.evnRefresh, AddressOf xUpdate
            AddHandler fncRel.evnRefreshRelMemList, AddressOf xRefreshSpouseList
            fncRel.fncShow()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmPersonalFa_Click", ex)

        Finally
            fncRel.Dispose()
            fncRel = Nothing

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmPersonalSpouse_Click, show spouse list
    '      MEMO       : 
    '      CREATE     : 2011/12/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmPersonalSpouse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmPersonalSpouse.Click

        Dim fncRel As New frmRelationMem
        Try
            fncRel.RootID = mintID
            fncRel.FormMode = frmRelationMem.emRelMode.Spouse
            AddHandler fncRel.evnRefresh, AddressOf xUpdate
            AddHandler fncRel.evnRefreshRelMemList, AddressOf xRefreshSpouseList
            fncRel.fncShow()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmPersonalSpouse_Click", ex)

        Finally
            fncRel.Dispose()
            fncRel = Nothing

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmPersonalChild_Click, show childlist
    '      MEMO       : 
    '      CREATE     : 2011/12/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmPersonalChild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmPersonalChild.Click

        Dim fncRel As New frmRelationMem
        Try
            fncRel.RootID = mintID
            fncRel.FormMode = frmRelationMem.emRelMode.Childs
            AddHandler fncRel.evnRefresh, AddressOf xUpdate
            AddHandler fncRel.evnRefreshRelMemList, AddressOf xRefreshSpouseList
            fncRel.fncShow()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmPersonalChild_Click", ex)

        Finally
            fncRel.Dispose()
            fncRel = Nothing

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmFamilyReport_Click, show stats
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmFamilyReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmFamilyReport.Click

        Dim frmStats As frmStatistics = Nothing

        Try
            frmStats = New frmStatistics()

            frmStats.fncShowForm()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmFamilyReport_Click", ex)
        Finally
            If frmStats IsNot Nothing Then frmStats.Dispose()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmDrawTreeFull_Click, draw full tree
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmDrawTreeFull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmDrawTreeFull.Click

        Try
            mblnDrawCompactTree = False

            'do nothing if it is already in SHOW_TREE_FULL mode
            If memFormMode = emFormMode.SHOW_TREE_FULL Then Exit Sub

            If mclsDrawCard.ActiveMemberID <= basConst.gcintNO_MEMBER Then Exit Sub
            xShowViewTree(True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmDrawTreeFull_Click", ex, Nothing, False)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmDrawTreeCompact_Click, draw compact tree
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmDrawTreeCompact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmDrawTreeCompact.Click

        Try
            mblnDrawCompactTree = True

            'do nothing if it is already in SHOW_TREE_COMPACT mode
            If memFormMode = emFormMode.SHOW_TREE_COMPACT Then Exit Sub

            If mclsDrawCard.ActiveMemberID <= basConst.gcintNO_MEMBER Then Exit Sub
            xShowViewTree(True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmDrawTreeCompact_Click", ex, Nothing, False)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmTreeFull_Click, draw full tree
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmTreeFull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmTreeFull.Click

        Try
            mblnDrawCompactTree = False

            'do nothing if it is already in SHOW_TREE_FULL mode
            If memFormMode = emFormMode.SHOW_TREE_FULL Then Exit Sub

            If mclsDrawCard.ActiveMemberID <= basConst.gcintNO_MEMBER Then Exit Sub
            xShowViewTree(True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmTreeFull_Click", ex, Nothing, False)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmTreeCompact_Click, draw compact tree
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmTreeCompact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmTreeCompact.Click

        Try
            mblnDrawCompactTree = True

            'do nothing if it is already in SHOW_TREE_COMPACT mode
            If memFormMode = emFormMode.SHOW_TREE_COMPACT Then Exit Sub

            If mclsDrawCard.ActiveMemberID <= basConst.gcintNO_MEMBER Then Exit Sub
            xShowViewTree(True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmTreeCompact_Click", ex, Nothing, False)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmGuide_Click, show guide file
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmGuide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmGuide.Click

        Dim objPDF As clsPdf = Nothing

        Try
            Dim strPdfFile As String

            strPdfFile = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & basConst.gcstrPdfGuide

            'exist if can not find the file
            If Not System.IO.File.Exists(strPdfFile) Then
                basCommon.fncMessageWarning(mcstrPdfMissing)
                Exit Sub
            End If

            'try to open pdf file
            Try
                objPDF = New clsPdf()
                objPDF.fncOpen(strPdfFile)
            Catch ex As Exception

                basCommon.fncMessageWarning(mcstrPdfRequire)
                basCommon.fncSaveErr(mcstrClsName, "tsmGuide_Click", ex, Nothing, False)

            End Try

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmGuide_Click", ex, Nothing, False)
        Finally
            objPDF = Nothing
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmPersonDelete_Click, delete person
    '      MEMO       : 
    '      CREATE     : 2012/02/01  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmPersonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmPersonDelete.Click

        Try
            If mintID <= basConst.gcintNO_MEMBER Then Exit Sub

            'confirm
            If Not basCommon.fncMessageConfirm(String.Format(basConst.gcstrMessageConfirm, basCommon.fncGetMemberName(mintID))) Then Exit Sub

            'try to delete then refresh
            If Not basCommon.fncDeleteMember(mintID) Then basCommon.fncMessageError(gcstrFail) Else xRefresh(mintID, True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmPersonDelete_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsmActivate_Click, activate software
    '      MEMO       : 
    '      CREATE     : 2012/05/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsmActivate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmActivate.Click
        Try

            If basCommon.fncCheckActive(False) Then

                gblnActivated = True
                basCommon.fncLoadInfoVersion()
                Me.Text = basCommon.GetTitleApp()
                tsmActivate.Visible = False
                tspUpVersion.Visible = basConst.gcintMaxLimit <> basConst.gcintMaxLimitUltimate
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmActivate_Click", ex)
        End Try
    End Sub

#End Region


#Region "MenuBar Event"


    '   ******************************************************************
    '　　　FUNCTION   : tsbQuit_Click, menu item click
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbQuit.Click

        Try

            mpnShowTree.Visible = False
            Me.Close()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "tlsQuit_Click", ex)

        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbAddPerson_Click, menu item click
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbAddPerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbAddPerson.Click

        Try

            mfrmPerInfo.FormMode = clsEnum.emMode.ADD

            mfrmPerInfo.fncShowForm()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbAddPerson_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbPrintTree_Click, menu item click
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbPrintTree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPrintTree.Click

        Try

            Dim intHPos As Integer = mpnShowTree.HorizontalScroll.Value
            Dim intVPos As Integer = mpnShowTree.VerticalScroll.Value
            Dim strFolder As String

            mpnShowTree.AutoScrollPosition = New Point(0, 0)


            strFolder = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder
            basCommon.fncDeleteFolder(strFolder)

            Dim frmPrint As New PrintPreview()

            ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
            frmPrint.MainFrom = Me

            If mblnUpdatePrint Then
                frmPrint.mblnUpdatePrint = True
                frmPrint.mintPrintPageSizeSelected = mintPrintPageSizeSelected
                frmPrint.mintPrintPageZoomSelected = mintPrintPageZoomSelected
                frmPrint.mblnPrintPageLandScape = mblnPrintPageLandScape
            End If
            ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------

            Select Case Me.memCurTree 'Case My.Settings.intCardStyle
                Case clsEnum.emCardStyle.CARD1
                    If tsbMenuTree1Basic.Checked Then
                        ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
                        frmPrint.FontUser = mclsDrawTreeS1.FontUser
                        ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
                        frmPrint.Run(mclsDrawTreeS1)
                    ElseIf tsbMenuTree1Open.Checked Then
                        ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
                        frmPrint.FontUser = mclsDrawTreeA1.FontUser
                        ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
                        frmPrint.Run(mclsDrawTreeA1)
                    Else
                        frmPrint.Run(mclsDrawTreeS3)
                    End If

                Case clsEnum.emCardStyle.CARD2
                    frmPrint.Run(mclsDrawTreeS2)

            End Select
            mpnShowTree.AutoScrollPosition = New Point(intHPos, intVPos)

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "tsbPrintTree_Click", ex)

        Finally

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbUserInfo_Click, Change pass word
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbUserInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbUserInfo.Click

        Dim frmUserInfo As frmPassChange = Nothing

        Try

            frmUserInfo = New frmPassChange

            If frmUserInfo.fncShowForm() Then

                frmUserInfo.Dispose()

            End If

        Catch ex As Exception

            frmUserInfo = Nothing
            basCommon.fncSaveErr(mcstrClsName, "tsbUserInfo_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbPersonInfo_Click, Show member infor
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbPersonInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPersonInfo.Click

        Try

            If mintID <= basConst.gcintNO_MEMBER Then Exit Sub

            mfrmPerInfo.FormMode = clsEnum.emMode.EDIT
            mfrmPerInfo.MemberID = mintID

            'show form 
            If Not mfrmPerInfo.fncShowForm() Then Exit Sub

            'if member is edied
            If Not mfrmPerInfo.FormModified Then Exit Sub

            'refresh
            If memFormMode = emFormMode.SHOW_TREE_FULL Or memFormMode = emFormMode.SHOW_TREE_COMPACT Then
                'is showing tree
                'redraw card
                Select Case My.Settings.intCardStyle
                    Case clsEnum.emCardStyle.CARD1
                        If tsbMenuTree1Basic.Checked Then
                            mclsDrawTreeS1.fncRedrawCard(mintID)
                        ElseIf tsbMenuTree1Open.Checked Then
                            mclsDrawTreeA1.fncRedrawCard(mintID)
                        Else
                            mclsDrawTreeS3.fncRedrawCard(mintID)
                        End If


                    Case clsEnum.emCardStyle.CARD2
                        mclsDrawTreeS2.fncRedrawCard(mintID)

                End Select

            Else
                'is showing family card
                mclsDrawCard.ActiveMemberID = mintID

            End If

            xSetSelectedRow(mintID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbPersonInfo_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbFamilyBuild_Click, build family tree
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbFamilyBuild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbFamilyBuild.Click

        Try
            'do nothing if it is already in SHOW_CARD mode
            If memFormMode = emFormMode.SHOW_CARD Then Exit Sub

            xSetVisibleDrawTreeTools(False)
            xShowViewTree(False)

            If CInt(tscboGeneration.SelectedItem) > mcintMaxDefaultDrawLevel Then

                tscboGeneration.SelectedItem = mcintMaxDefaultDrawLevel

            End If

            xSetSelectedRow(mintID)


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbFamilyBuild_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbFamilyShowTree_Click, show family tree
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbFamilyShowTree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbFamilyShowTree.Click, tsbShowTree.ButtonClick, tsmFamilyShowTree.Click

        Try
            'do nothing if it is already in SHOW_TREE mode
            If memFormMode = emFormMode.SHOW_TREE_FULL Or memFormMode = emFormMode.SHOW_TREE_COMPACT Then Exit Sub

            If mclsDrawCard.ActiveMemberID <= basConst.gcintNO_MEMBER Then Exit Sub
            xSetVisibleDrawTreeTools(True)
            xShowViewTree(True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbFamilyShowTree_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbHelp_Click, show about dialog
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbHelp.Click

        Try
            xShowAbout()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbHelp_Click", ex)
        Finally

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbRoot_Click, go to root
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbRoot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRoot.Click
        Try

            If fncGetRoot() <= gcintNO_MEMBER Then Return
            'reset flag
            mblnDragdrop = False
            mintID = fncGetRoot()
            If mintID = 0 Then Return

            xSetSelectedRow(mintID)
            'show tree if the form in SHOWTREE mode
            If memFormMode = emFormMode.SHOW_TREE_FULL Or memFormMode = emFormMode.SHOW_TREE_COMPACT Then

                ' ▽ 2012/02/16 AKB QUYET (still drawing because we have many card style)
                'exit if selected member is same with current member
                'If mintID = mclsDrawTree1.RootID Then Exit Sub
                ' △ 2012/02/16 AKB QUYET ***********************************************

                xShowViewTree(True)
                Exit Sub

            End If

            'exit if selected member is same with current member
            If mintID = mclsDrawCard.ActiveMemberID Then Exit Sub
            mclsDrawCard.ActiveMemberID = mintID
            'redraw
            xSetActiveMember(mintID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbRoot_Click", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbSetting_Click, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbSetting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSetting.Click

        Try
            Using frmOps As New frmOption

                frmOps.ShowDialog()

                mblnOptChanged = frmOps.Changed

                xSetInitGeneration(mblnOptChanged)

                mblnOptChanged = True

                If memFormMode = emFormMode.SHOW_CARD Or Not frmOps.Changed Then Exit Sub

            End Using

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbSetting_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tsbExportWord, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tsbExportWord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbExportWord.Click

        Try
            Using frmWordExport As New frmWord(Me.mtblData)
                frmWordExport.fncShowForm()
            End Using

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbExportWord_Click", ex)
        End Try

    End Sub


    ''' <summary>
    ''' Select tree
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsbSelectTree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSelectTree.Click
        Try
            mpnShowTree.PanelMode = pnTreePanel.emPanelMode._SELECT

            tsbSelectTree.Checked = True
            tsbMoveTree.Checked = False

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbSelectTree_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Move tree
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsbMoveTree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbMoveTree.Click
        Try
            mpnShowTree.PanelMode = pnTreePanel.emPanelMode._MOVE

            tsbSelectTree.Checked = False
            tsbMoveTree.Checked = True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbMoveTree_Click", ex)
        End Try
    End Sub


#End Region


#Region "Form's controls events"

    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/09/14  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        mtblData = New DataTable
        mclsVnCal = New clsLunarCalendar()
        mfrmPerInfo = New frmPersonInfo(clsEnum.emMode.ADD)

    End Sub


    Private Sub xSetVisibleDrawTreeTools(ByVal blnVisible As Boolean)

        tslGenerationNum.Visible = blnVisible
        tscboGeneration.Visible = blnVisible
        tsbSelectTree.Visible = blnVisible
        tsbMoveTree.Visible = blnVisible
        tsbTreeView1.Visible = blnVisible
        ToolStripSeparator9.Visible = blnVisible
        ToolStripSeparator8.Visible = blnVisible

        ' ▽2018/04/24 AKB Nguyen Thanh Tung --------------------------------
        ToolStripSeparator8.Visible = blnVisible
        tsbSetting.Visible = blnVisible
        tsbTreeS1.Visible = blnVisible
        tsbTreeS2.Visible = blnVisible
        tsbTreeS1Hoz.Visible = blnVisible
        tsbTreeS1Ver.Visible = blnVisible
        ToolStripSeparator10.Visible = blnVisible
        tslShowTypeMember.Visible = blnVisible
        tscboShowTypeMember.Visible = blnVisible
        ToolStripSeparator12.Visible = blnVisible
        ' △2018/04/24 AKB Nguyen Thanh Tung --------------------------------
    End Sub

    Private Sub xSetInitGeneration(Optional ByVal blnChangeGenerationSelected As Boolean = True)

        Try

            Dim i As Integer

            tscboGeneration.Items.Clear()
            For i = 1 To My.Settings.intGeneration

                tscboGeneration.Items.Add(i)

            Next


            If CInt(My.Settings.intMaxDrawGeneration) < CInt(My.Settings.intGeneration) Then

                tscboGeneration.SelectedItem = CInt(My.Settings.intMaxDrawGeneration)

            Else

                tscboGeneration.SelectedItem = CInt(My.Settings.intGeneration)

            End If

            My.Settings.Save()

        Catch ex As Exception

        End Try


    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : frmMain_Load, form loaded
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.splMain.SplitterDistance = clsDefine.MAIN_PANEL1_MIN
            Me.mintCurPage = 1
            Me.Text = basCommon.GetTitleApp()

            'set title for trial version
            If Not gblnActivated Then
                tsmActivate.Visible = True
                tspUpVersion.Visible = False
            Else
                tspUpVersion.Visible = basConst.gcintMaxLimit <> basConst.gcintMaxLimitUltimate
            End If

            tsbMenuTree1Impact.Visible = False
            xSetVisibleDrawTreeTools(False)
            xSetInitGeneration()

            ' ▽2018/04/27 AKB Nguyen Thanh Tung --------------------------------
            xBingdingComboTypeShowTree()
            ' △2018/04/27 AKB Nguyen Thanh Tung --------------------------------

            If CInt(tscboGeneration.SelectedItem) > mcintMaxDefaultDrawLevel Then

                tscboGeneration.SelectedItem = mcintMaxDefaultDrawLevel

            End If

            xCreateNoAvatarImage()

            'panel to draw tree
            mpnShowTree = New pnTreePanel()
            splMain.Panel2.Controls.Add(mpnShowTree)

            gintTreePanelDPIX = mpnShowTree.CreateGraphics().DpiX
            gintTreePanelDPIY = mpnShowTree.CreateGraphics().DpiY

            mclsDrawCard = New clsDrawCard(pnFamilyCard, mfrmPerInfo)

            mclsDrawTreeS1 = New clsDrawTreeS1(mpnShowTree, mclsDrawCard.ActiveMemberID, tscboGeneration.SelectedItem, mfrmPerInfo)
            mclsDrawTreeS3 = New clsDrawTreeS3(mpnShowTree, mclsDrawCard.ActiveMemberID, tscboGeneration.SelectedItem, mfrmPerInfo)
            'mclsDrawTreeS3 = New clsDrawTreeSS(mpnShowTree, mclsDrawCard.ActiveMemberID, tscboGeneration.SelectedItem, mfrmPerInfo)

            mclsDrawTreeA1 = New clsDrawTreeA1(mpnShowTree, mclsDrawCard.ActiveMemberID, tscboGeneration.SelectedItem, mfrmPerInfo)

            mclsDrawTreeS2 = New clsDrawTreeS2(mpnShowTree, mfrmPerInfo)
            mclsRightMenu = New clsRightMenu()
            mfrmAnni = New frmPersonalAnniversary()

            xAddHandler()

            'set start id
            xSetStartID()

            'show family card panel and fill quick search grid
            xShowViewTree(False)

            'next birthday and decease day
            'xNextAnniversary()

            're-enable tabstop
            rdFemale.TabStop = True
            rdGenderAll.TabStop = True
            rdMale.TabStop = True


            'FIX: delete temp folder for updating new version
            Dim strTempFolder As String

            strTempFolder = My.Application.Info.DirectoryPath & basConst.gcstrBackupFolder
            basCommon.fncDeleteFolder(strTempFolder)

            strTempFolder = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder
            basCommon.fncDeleteFolder(strTempFolder)

            Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmMain_Load", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : grpQuickView_SizeChanged, resize quick view group
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub grpQuickView_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grpQuickView.SizeChanged
        Try
            xMainFormSizeChange()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "grpQuickView_SizeChanged", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmMain_SizeChanged, form size change event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmMain_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        Try
            xMainFormSizeChange()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmMain_SizeChanged", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnQickSearch_Click, button seach clicked
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnQickSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQickSearch.Click

        Try
            btnQickSearch.Enabled = False
            xSearch()
            btnQickSearch.Enabled = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnQickSearch_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvMain_CellMouseDoubleClick, double click on dgv
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvMain_CellMouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvMain.CellMouseDoubleClick

        Try
            'handle left mouse only
            If e.Button <> Windows.Forms.MouseButtons.Left Then Exit Sub

            'get id from grid
            xGetID(e.RowIndex, mintID, mintGender)

            'reset flag
            mblnDragdrop = False

            'show tree if the form in SHOWTREE mode
            If memFormMode = emFormMode.SHOW_TREE_FULL Or memFormMode = emFormMode.SHOW_TREE_COMPACT Then

                ' ▽ 2012/02/16 AKB QUYET (still drawing because we have many card style)
                'exit if selected member is same with current member
                'If mintID = mclsDrawTree1.RootID Then Exit Sub
                ' △ 2012/02/16 AKB QUYET ***********************************************

                xShowViewTree(True)
                Exit Sub

            End If

            'exit if selected member is same with current member
            If mintID = mclsDrawCard.ActiveMemberID Then Exit Sub

            'redraw
            xSetActiveMember(mintID, e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvMain_CellMouseDoubleClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvMain_CellMouseClick, click on dgv
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvMain_CellMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvMain.CellMouseClick

        Try
            'Dim intID As Integer
            'Dim intGender As clsEnum.emGender

            lblStatus.Text = ""

            'handle left mouse only
            If e.Button <> Windows.Forms.MouseButtons.Left Then Exit Sub

            'get id from grid
            xGetID(e.RowIndex, mintID, mintGender)

            'show tree if the form in SHOWTREE mode
            If memFormMode = emFormMode.SHOW_TREE_FULL Or memFormMode = emFormMode.SHOW_TREE_COMPACT Then

                Select Case My.Settings.intCardStyle
                    Case clsEnum.emCardStyle.CARD1

                        If tsbMenuTree1Basic.Checked Then

                            If Not mclsDrawTreeS1.fncSetFocus(mintID) Then lblStatus.Text = mcstrMemberNotDrawn

                        ElseIf tsbMenuTree1Open.Checked Then

                            If Not mclsDrawTreeA1.fncSetFocus(mintID) Then lblStatus.Text = mcstrMemberNotDrawn

                        Else

                            If Not mclsDrawTreeS3.fncSetFocus(mintID) Then lblStatus.Text = mcstrMemberNotDrawn


                        End If


                    Case clsEnum.emCardStyle.CARD2
                        If Not mclsDrawTreeS2.fncSetFocus(mintID) Then lblStatus.Text = mcstrMemberNotDrawn

                End Select

            End If


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvMain_CellMouseClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvMain_CellMouseDown, mouse down on dgv
    '      MEMO       : 
    '      CREATE     : 2011/11/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvMain_CellMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvMain.CellMouseDown

        Try
            Dim intId As Integer
            Dim emGender As clsEnum.emGender

            If e.RowIndex < 0 Then Exit Sub

            'set flag
            If e.Button = Windows.Forms.MouseButtons.Left Then mblnDragdrop = True

            '--> show right menu
            If e.Button <> Windows.Forms.MouseButtons.Right Then Exit Sub

            'get id and gender
            xGetID(e.RowIndex, intId, emGender)

            'create menu
            mmnuRightMouse = mclsRightMenu.fncGetMenu(intId, "abc", emGender, False)
            dgvMain.ContextMenuStrip = mmnuRightMouse

            'remove menu when it is closed
            AddHandler mmnuRightMouse.Closed, AddressOf xRightMenuClosed

            'select row
            dgvMain.Rows.Item(e.RowIndex).Selected = True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvMain_CellMouseDown", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvMain_CellMouseUp, mouse up on dgv
    '      MEMO       : 
    '      CREATE     : 2011/11/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvMain_CellMouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvMain.CellMouseUp
        Try

            'set flag
            If e.Button = Windows.Forms.MouseButtons.Left Then mblnDragdrop = False

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvMain_CellMouseUp", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvMain_CellMouseMove, mouse move on dgv
    '      MEMO       : 
    '      CREATE     : 2011/11/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvMain_CellMouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvMain.CellMouseMove

        Try
            If Not mblnDragdrop Then Exit Sub

            'set flag
            mblnDragdrop = False

            'get id from grid
            xGetID(e.RowIndex, mintID, mintGender)

            dgvMain.DoDragDrop(mintID, DragDropEffects.Copy)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvMain_CellMouseMove", ex)
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

            mintCurPage = 1
            xFillGrid()
            'cbPages.SelectedIndex = mintCurPage - 1

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnFirstPage_Click", ex)
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

            mintCurPage = mintTotalPage
            xFillGrid()
            'cbPages.SelectedIndex = mintCurPage - 1

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnLastPage_Click", ex)
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

            mintCurPage -= 1
            xFillGrid()
            'cbPages.SelectedIndex = mintCurPage - 1

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

            mintCurPage += 1
            xFillGrid()
            'cbPages.SelectedIndex = mintCurPage - 1

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnNextPage_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' cbPages_SelectedIndexChanged - Selected Index Changed 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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
    ''' cbPages_KeyPress - Handles Key Press
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
            mintCurPage = intPage
            xFillGrid()
            cbPages.SelectAll()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "cbPages_KeyPress", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : splMain_SplitterMoved, resize spliter
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub splMain_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles splMain.SplitterMoved

        Try

            If mclsDrawCard IsNot Nothing Then

                mclsDrawCard.fncClear()
                mclsDrawCard.fncDraw()

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "splMain_SplitterMoved", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : pnShow_Paint, paint event to draw connector
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    'Private Sub pnShow_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles mpnShowTree.Paint

    '    Try
    '        'If mclsDrawCard.ActiveMemberID <= basConst.gcintNO_MEMBER Then Exit Sub

    '        'If mclsDrawTree IsNot Nothing Then mclsDrawTree.fncDrawConnector()


    '    Catch ex As Exception

    '        basCommon.fncSaveErr(mcstrClsName, "pnShow_Paint", ex)

    '        'show message and return main form
    '        basCommon.fncMessageError(mcstrErrorDrawing)
    '        xShowViewTree(False)

    '    End Try

    'End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmMain_FormClosing
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Try
            xClear()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmMain_FormClosing", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : lblAnniBirth_LinkClicked
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub lblAnniBirth_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblAnniBirth.LinkClicked

        Try
            mfrmWaiting = New frmProgress()
            mobjLoadingThread = New System.Threading.Thread(AddressOf xShowAnniBirth)

            mobjLoadingThread.Start()
            mfrmWaiting.ShowDialog()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "lblAnniBirth_LinkClicked", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : lblAnniDecease_LinkClicked
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub lblAnniDecease_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblAnniDecease.LinkClicked

        Try
            mfrmWaiting = New frmProgress()
            mobjLoadingThread = New System.Threading.Thread(AddressOf xShowAnniDecease)

            mobjLoadingThread.Start()
            mfrmWaiting.ShowDialog()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "lblAnniDecease_LinkClicked", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress,handles key pressed event
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtName_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName.KeyPress


        Try
            If e.KeyChar = Convert.ToChar(Keys.Enter) Then xSearch()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "txtName_KeyPress", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvMain_Sorted,grid sorted, refill STT column
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvMain_Sorted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvMain.Sorted
        Try
            Dim intStart As Integer

            intStart = (mintCurPage - 1) * mcintItemPerPage

            'refill STT column
            For i As Integer = 0 To dgvMain.Rows.Count - 1
                dgvMain.Item(clmLevel.Name, i).Value = intStart + 1 'i + 1
                intStart += 1

                If dgvMain.Item("DECEASED", i).Value = basConst.gcintDIED Then
                    dgvMain.Rows(i).DefaultCellStyle.BackColor = Color.LightGray
                End If

            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvMain_Sorted", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : picLogo_Click, logo clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub picLogo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picLogo.Click
        Try
            xShowAbout()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "picLogo_Click", ex)
        Finally

        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : lblCompany_Click, company name clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub lblCompany_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblCompany.Click
        Try
            xShowAbout()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "lblCompany_Click", ex)
        Finally

        End Try
    End Sub


#End Region


#Region "Form Functions"


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show form
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm() As Boolean

        fncShowForm = False

        Dim frmInitInfo As frmFamilyInital = Nothing

        Try
            'convert old database to the new one
            xCheckDatabase()
            tsbTreeView1.Visible = False
            tsbMenuTree1Basic.Checked = True

            'mobjCardThread = New System.Threading.Thread(AddressOf fncMakeMemberCard)
            'mobjCardThread.Start()

            If Not basCommon.fncHasFamilyInfo() Then

                'show init infor if there is no value
                frmInitInfo = New frmFamilyInital()

                If Not frmInitInfo.fncShowForm() Then Exit Function

                If Not frmInitInfo.FamilyUpdated Then Exit Function

            End If

            mintLastDrawType = clsEnum.emCardStyle.CARD1

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        Finally

            If frmInitInfo IsNot Nothing Then frmInitInfo.Dispose()

        End Try


    End Function

    ' This delegate enables asynchronous calls for setting 
    ' the text property on a TextBox control. 
    Delegate Sub SetCallback(ByVal blnValue As Boolean)

    Private Sub SetShowTreeButton(ByVal blnValue As Boolean)

        ' InvokeRequired required compares the thread ID of the 
        ' calling thread to the thread ID of the creating thread. 
        ' If these threads are different, it returns true. 
        If Me.tlsMain.InvokeRequired Then
            Dim d As New SetCallback(AddressOf SetShowTreeButton)
            Me.Invoke(d, New Object() {blnValue})
        Else
            Me.tsbFamilyShowTree.Enabled = blnValue
        End If
    End Sub

    'Public Sub fncMakeMemberCard()

    '    Dim stCard As stCardInfo
    '    Dim objCard As usrMemberCard1
    '    Dim i As Integer
    '    Dim tblUser As DataTable = gobjDB.fncGetMemberMain()
    '    SetShowTreeButton(False)
    '    Try
    '        gtblMemberCard = Nothing
    '        gtblMemberCard = New Hashtable()            'QUYET added this code

    '        If tblUser Is Nothing Then

    '            Return

    '        End If

    '        ' ▽ 2012/11/29   AKB Quyet （Move this code up）************************
    '        'gtblMemberCard = New Hashtable()
    '        ' △ 2012/11/29   AKB Quyet *********************************************


    '        For i = 0 To tblUser.Rows.Count - 1
    '            stCard = fncGetMemberInfo(basCommon.fncCnvToInt(tblUser.Rows(i).Item("MEMBER_ID")), tblUser)
    '            objCard = fncMakeCardInfoType1(stCard, My.Settings.intCardSize <> clsEnum.emCardSize.LARGE)
    '            objCard.Visible = False
    '            gtblMemberCard.Add(stCard.intID, objCard)
    '        Next

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        SetShowTreeButton(True)
    '        mobjCardThread = Nothing
    '    End Try
    'End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xAddHandler, add handler
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddHandler() As Boolean

        xAddHandler = False

        Try
            AddHandler mclsDrawCard.evnRefresh, AddressOf xRefresh
            AddHandler mclsDrawCard.evnCardClick, AddressOf xCardClick

            AddHandler mfrmPerInfo.evnRefresh, AddressOf xRefresh
            AddHandler mfrmPerInfo.evnActivated, AddressOf xActivated

            AddHandler mclsDrawTreeS1.evnCardClicked, AddressOf xSetSelectedRow
            AddHandler mclsDrawTreeS1.evnCardDoubleClicked, AddressOf xSetSelectedRow
            AddHandler mclsDrawTreeS1.evnRefresh, AddressOf xRefresh

            AddHandler mclsDrawTreeS3.evnCardClicked, AddressOf xSetSelectedRow
            AddHandler mclsDrawTreeS3.evnCardDoubleClicked, AddressOf xSetSelectedRow
            AddHandler mclsDrawTreeS3.evnRefresh, AddressOf xRefresh

            AddHandler mclsDrawTreeA1.evnCardClicked, AddressOf xSetSelectedRow
            AddHandler mclsDrawTreeA1.evnCardDoubleClicked, AddressOf xSetSelectedRow
            AddHandler mclsDrawTreeA1.evnRefresh, AddressOf xRefresh
            'AddHandler mclsDrawTree1.evnProgressDone, AddressOf xProgressDone

            AddHandler mclsDrawTreeS2.evnCardClicked, AddressOf xSetSelectedRow
            AddHandler mclsDrawTreeS2.evnCardDoubleClicked, AddressOf xSetSelectedRow
            AddHandler mclsDrawTreeS2.evnRefresh, AddressOf xRefresh
            AddHandler mclsDrawTreeS2.evnProgressDone, AddressOf xProgressDone

            AddHandler mclsRightMenu.evnMenuItemClick, AddressOf xRightMenuItemClick

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xAddHandler", ex)

        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xMainFormSizeChange, form size changed
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xMainFormSizeChange() As Boolean

        xMainFormSizeChange = False

        Try

            If Me.splMain.SplitterDistance <= 0 Then
                Me.splMain.Panel1MinSize = 0

            Else

                ''Left Panel Size Changed
                If Me.splMain.SplitterDistance < clsDefine.MAIN_PANEL1_MIN Then
                    Me.splMain.Panel1MinSize = clsDefine.MAIN_PANEL1_MIN
                Else
                    Me.splMain.Panel1MinSize = 0
                End If

                Me.btnQickSearch.Left = Me.grpQuickView.Width - Me.btnQickSearch.Width - clsDefine.SPEC_CONTROL_VER
                Me.txtName.Width = Me.btnQickSearch.Left - Me.txtName.Left - clsDefine.SPEC_CONTROL_VER
                Me.dgvMain.Height = Me.lblNextBirthDay.Top - Me.dgvMain.Top

                'Right Panel Size Changed

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "MainFormSizeChange", ex)
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xShowViewTree, show family tree
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : blnShowTree Boolean, true - show tree, false - show card
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xShowViewTree(ByVal blnShowTree As Boolean) As Boolean

        xShowViewTree = False
        Dim objThread As System.Threading.Thread = Nothing
        Dim objThreadDraw As System.Threading.Thread = Nothing

        Try

            lblStatus.Text = ""
            ' ▽2018/04/27 AKB Nguyen Thanh Tung --------------------------------
            mpnShowTree.Select()
            tsbTreeS1.Visible = False
            tsbTreeS1Hoz.Visible = False
            tsbTreeS1Ver.Visible = False
            ToolStripSeparator10.Visible = False
            'tsbTreeView1.Visible = False
            ' △2018/04/27 AKB Nguyen Thanh Tung --------------------------------

            If blnShowTree Then

                If mintID <= basConst.gcintNO_MEMBER Then Exit Function

                If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD1 Then
                    ' ▽2018/04/27 AKB Nguyen Thanh Tung --------------------------------
                    tsbTreeS1.Visible = True
                    tsbTreeS1Hoz.Visible = True
                    tsbTreeS1Ver.Visible = True
                    ToolStripSeparator10.Visible = True
                    xEnableComboTypeShowTree(tsbMenuTree1Basic.Checked)
                    'tsbTreeView1.Visible = True
                Else
                    tsbMenuTree1Basic.Checked = False
                    tsbMenuTree1Open.Checked = False
                    xEnableComboTypeShowTree(True)
                    ' △2018/04/27 AKB Nguyen Thanh Tung --------------------------------
                End If
                gintPercent = 0

                mfrmWaiting = New frmProgress()

                mobjLoadingThread = New System.Threading.Thread(AddressOf xDrawTree)
                mobjLoadingThread.Start()
                'objThread.Start()
                mfrmWaiting.ShowDialog()

            Else

                'If splMain.Visible Then Exit Function
                Me.memFormMode = emFormMode.SHOW_CARD

                Me.tsbPrintTree.Enabled = False
                Me.pnFamilyCard.Visible = True
                Me.pnFamilyCard.Dock = DockStyle.Fill
                Me.pnFamilyCard.BringToFront()
                'Me.mclsDrawCard.ActiveMemberID = mintID

                Select Case My.Settings.intCardStyle
                    Case clsEnum.emCardStyle.CARD1
                        If tsbMenuTree1Basic.Checked Then
                            mclsDrawTreeS1.fncClearControls()
                        ElseIf tsbMenuTree1Open.Checked Then
                            mclsDrawTreeA1.fncClearControls()
                        Else
                            mclsDrawTreeS3.fncClearControls()
                        End If

                    Case clsEnum.emCardStyle.CARD2
                        mclsDrawTreeS2.fncClear()

                End Select

                Me.mpnShowTree.Visible = False
                'Me.splMain.Visible = True

                xRefresh(mintID, True)

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "ShowViewTree", ex)
        Finally
            objThreadDraw = Nothing
            objThread = Nothing
        End Try

    End Function
    '   ******************************************************************
    '　　　FUNCTION   : xGetProgress
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Private Sub xGetProgress()
        Try
            Do Until gintPercent = 100

                System.Threading.Thread.Sleep(50)
                mobjPrgBar.UpdatePro(gintPercent)
            Loop
            basCommon.fncMessageInfo("Vẽ cây thành công")
            mobjPrgBar.CloseTheForm()
        Catch ex As Exception

        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xShowTree, show tree
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawTree()

        Dim objDrawTree As MethodInvoker = Nothing

        Try
            'If mpnShowTree.Visible Then Exit Function

            'Me.splMain.Visible = False
            Me.memFormMode = emFormMode.SHOW_TREE_FULL
            If mblnDrawCompactTree Then Me.memFormMode = emFormMode.SHOW_TREE_COMPACT

            'set text
            objDrawTree = New MethodInvoker(AddressOf xInvokeDrawTree)
            Me.Invoke(objDrawTree)


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowTree", ex)
        Finally
            objDrawTree = Nothing
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xInvokeShowTree, draw tree
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xInvokeDrawTree()

        Try
            Me.pnFamilyCard.Visible = False
            Me.tsbPrintTree.Enabled = True

            Me.mpnShowTree.Visible = True
            Me.mpnShowTree.Dock = DockStyle.Fill
            Me.mpnShowTree.BringToFront()

            'draw tree
            Dim dtStart As Date = DateTime.Now
            Select Case My.Settings.intCardStyle
                Case clsEnum.emCardStyle.CARD1
                    'mclsDrawTree1.fncDraw(mintID, CInt(tscboGeneration.SelectedItem))
                    gintPercent = 0

                    mintLastDrawType = clsEnum.emCardStyle.CARD1
                    Me.memCurTree = clsEnum.emCardStyle.CARD1

                    If tsbMenuTree1Basic.Checked Then
                        mclsDrawTreeS1.fncDraw(mintID, CInt(tscboGeneration.SelectedItem), clsDefine.TREE_S1_STARTX, clsDefine.TREE_S1_STARTY)
                    ElseIf tsbMenuTree1Open.Checked Then
                        mclsDrawTreeA1.fncDraw(mintID, CInt(tscboGeneration.SelectedItem), clsDefine.TREE_S1_STARTX, clsDefine.TREE_S1_STARTY)
                    Else
                        mclsDrawTreeS3.fncDraw(mintID, CInt(tscboGeneration.SelectedItem), clsDefine.TREE_S1_STARTX, clsDefine.TREE_S1_STARTY)
                    End If

                    gintPercent = 100
                    xProgressDone()

                Case clsEnum.emCardStyle.CARD2
                    If txtName.Text.Trim() <> "" Or rdGenderAll.Checked = False Then           'reset search box for calculating generation
                        txtName.Text = ""
                        rdGenderAll.Checked = True
                        xQuickSearch(True)
                    End If
                    Me.memCurTree = clsEnum.emCardStyle.CARD2
                    mclsDrawTreeS2.fncDraw(mintID, mtblData, CInt(tscboGeneration.SelectedItem))
                    mintLastDrawType = clsEnum.emCardStyle.CARD2

            End Select
            Dim dtEnd As Date = DateTime.Now
            lblStatus.Text = "Thời gian xử lý: " & CStr(dtEnd.Hour * 3600 + dtEnd.Minute * 60 + dtEnd.Second - (dtStart.Hour * 3600 + dtStart.Minute * 60 + dtStart.Second)) & " giây"
        Catch ex As Exception
            gintPercent = 100
            xProgressDone()
            basCommon.fncSaveErr(mcstrClsName, "xInvokeShowTree", ex)
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Sub


    ''   ******************************************************************
    ''　　　FUNCTION   : xPrintDocument_PrintPage, print document event
    ''      PARAMS     : sender Object
    ''      PARAMS     : e PrintPageEventArgs
    ''      MEMO       : 
    ''      CREATE     : 2011/09/15  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Sub xPrintDocument_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

    '    Try

    '        xPrintImage(e)

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "PrintDocument_PrintPage", ex)
    '    End Try

    'End Sub


    ''   ******************************************************************
    ''　　　FUNCTION   : xPrintImage, export family tree to image
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : e   PrintPageEventArgs
    ''      MEMO       : 
    ''      CREATE     : 2011/09/15  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xPrintImage(ByVal e As System.Drawing.Printing.PrintPageEventArgs) As Boolean

    '    xPrintImage = False

    '    Dim bmpPrint As Bitmap = Nothing
    '    Dim gPrint As Graphics = Nothing
    '    Dim hdc As System.IntPtr

    '    Try

    '        'point the screen to position 0 0
    '        mpnShowTree.AutoScrollPosition = New Point(0, 0)

    '        'new bitmap image
    '        bmpPrint = New Bitmap(mpnShowTree.DisplayRectangle.Width, mpnShowTree.DisplayRectangle.Height)

    '        'draw background image for panel
    '        mclsDrawTree.fncDrawConnector(bmpPrint)
    '        mpnShowTree.BackgroundImage = bmpPrint

    '        'create graphic instance
    '        gPrint = Graphics.FromImage(bmpPrint)

    '        'hide scrollbar
    '        mpnShowTree.AutoScroll = False

    '        'print bitmap image
    '        hdc = gPrint.GetHdc

    '        Call SendMessage(mpnShowTree.Handle, mcintWM_PRINT, hdc, _
    '            enDrawingOptions.PRF_CHILDREN Or _
    '            enDrawingOptions.PRF_CLIENT Or _
    '            enDrawingOptions.PRF_NONCLIENT Or _
    '            enDrawingOptions.PRF_OWNED)

    '        gPrint.ReleaseHdc(hdc)

    '        e.Graphics.DrawImage(bmpPrint, 0, 0)

    '        'remove background image and show scrollbar
    '        mpnShowTree.BackgroundImage = Nothing
    '        mpnShowTree.AutoScroll = True

    '        Return True

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "xPrintImage", ex)
    '    Finally
    '        If gPrint IsNot Nothing Then gPrint.Dispose()
    '        If bmpPrint IsNot Nothing Then bmpPrint.Dispose()
    '    End Try

    'End Function


    '   ******************************************************************
    '　　　FUNCTION   : xShowAbout, show about dialog
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     :
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xShowAbout() As Boolean

        xShowAbout = False

        Dim frmAboutDialog As frmAbout = Nothing

        Try
            frmAboutDialog = New frmAbout()

            frmAboutDialog.ShowDialog()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowAbout", ex, Nothing, False)
        Finally
            If frmAboutDialog IsNot Nothing Then frmAboutDialog.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xClear, dispose variables
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     :
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xClear() As Boolean

        xClear = False

        Try

            mpnShowTree.Visible = False

            Panel2.Visible = False

            If mtblData IsNot Nothing Then mtblData.Dispose()
            If mclsVnCal IsNot Nothing Then mclsVnCal = Nothing
            If mclsDrawCard IsNot Nothing Then mclsDrawCard.Dispose()

            If mclsDrawTreeS1 IsNot Nothing Then mclsDrawTreeS1.Dispose()
            If mclsDrawTreeS3 IsNot Nothing Then mclsDrawTreeS3.Dispose()
            If mclsDrawTreeA1 IsNot Nothing Then mclsDrawTreeA1.Dispose()

            mclsDrawTreeS2 = Nothing
            If mfrmPerInfo IsNot Nothing Then mfrmPerInfo.Dispose()

            xCloseAnniForm()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClear", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetStartID, get son of root id to load at first time run
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     :
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetStartID() As Boolean

        xSetStartID = False

        Dim tblData As DataTable = Nothing

        Try
            'tblData = basCommon.fncGetKids(mcintRootID)

            'default there is no active member
            mintID = basConst.gcintNO_MEMBER
            mintRootID = basCommon.fncGetRoot()

            mintID = mintRootID

            'if there is an ancentor
            'If tblData IsNot Nothing Then Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(0).Item(mcstrFieldMEMBER_ID)), mintID)

            'mclsDrawCard.ActiveMemberID = mintID

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetStartID", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetActiveMember, set active member
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intMemberID Integer, member id
    '      PARAMS     : e DataGridViewCellMouseEventArgs, mouse event
    '      MEMO       : 
    '      CREATE     : 2011/11/28  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetActiveMember(ByVal intMemberID As Integer, Optional ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs = Nothing) As Boolean

        xSetActiveMember = False

        Dim objEffect As clsMovingEffect = Nothing

        Try
            Dim ptStart As Point
            Dim ptEnd As Point
            Dim ptTempMouse As Point
            Dim ptTempPanel As Point
            Dim ptTemp As Point

            'new object
            objEffect = New clsMovingEffect(mclsDrawCard, pnFamilyCard, basConst.gcintTimerInterval, 10)

            'get position of mouse and panel
            ptTempMouse = Windows.Forms.Cursor.Position
            ptTempPanel = Me.splMain.Panel2.PointToScreen(ptTemp)

            'calculate start point
            ptStart.X = 0
            ptStart.Y = ptTempMouse.Y - ptTempPanel.Y

            'end point is the card's position
            ptEnd = mclsDrawCard.MaleCardLocation
            If mintGender = clsEnum.emGender.FEMALE Then ptEnd = mclsDrawCard.FemaleCardLocation

            'start effect
            objEffect.fncStartEffect(ptStart, ptEnd, mintID)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetActiveMember", ex)
        Finally
            objEffect = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xCloseAnniForm, close Anniversary dialog
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCloseAnniForm()

        Try
            Dim objCloseAnniForm As MethodInvoker

            'close thread
            mobjLoadingThread = Nothing

            'close waiting form
            objCloseAnniForm = New MethodInvoker(AddressOf xCloseAnni)
            Me.Invoke(objCloseAnniForm)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCloseAnniForm", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCloseAnni, close Anniversary form
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCloseAnni()

        Try
            If mfrmAnni IsNot Nothing Then

                If mfrmAnni.FormShown Then mfrmAnni.Close()
                mfrmAnni.Dispose()

            End If
            mfrmAnni = Nothing

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCloseAnni", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCreateNoAvatarImage, save default no avatar image
    '      VALUE      : Boolean, true - success, false - failure
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCreateNoAvatarImage() As Boolean

        xCreateNoAvatarImage = False

        Try
            If System.IO.File.Exists(My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & basConst.gcstrNoAvatar) Then Return True

            Return basCommon.fncSaveImage(My.Resources.noavatar, basConst.gcstrImageFolder & basConst.gcstrAvatarPath, basConst.gcstrNoAvatar.Substring(0, basConst.gcstrNoAvatar.IndexOf(".")))

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCreateNoAvatarImage", ex)
        End Try

    End Function


#End Region


#Region "Quick search"

    '   ******************************************************************
    '　　　FUNCTION   : xSearch, handles quick search 
    '      VALUE      : Boolean, true - have result, false - no result
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSearch() As Boolean

        xSearch = False

        Try
            Me.lblResultInfo.Text = ""
            Me.mintCurPage = 1

            'If Not xQuickSearch(False) Then
            If Not xQuickSearch(True) Then

                'No result - clear grid
                'dgvMain.Rows.Clear()

                'message
                Me.lblResultInfo.Text = basConst.gcstrFindNotFound

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSearch", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xQuickSearch, handles quick search 
    '      VALUE      : Boolean, true - have result, false - no result
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xQuickSearch(Optional ByVal blnGetNewData As Boolean = True) As Boolean

        xQuickSearch = False

        Try
            'clear grid before filling
            'dgvMain.Rows.Clear()

            'get search information
            xGetSearchInfo()

            If mtblGridSource IsNot Nothing Then
                mtblGridSource.Rows.Clear()
                mtblGridSource.Dispose()
            End If

            If mtblData IsNot Nothing Then mtblData.Dispose()
            If mtblRel IsNot Nothing Then mtblRel.Dispose()

            'get data from database
            If blnGetNewData Then mtblData = gobjDB.fncGetQuickSearch(mstSearchInfo)

            'exit if there's no data
            If mtblData Is Nothing Then Exit Function

            'calculate generation
            'xFillGeneration()

            'fill gird
            If Not xFillGrid() Then Return False

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xQuickSearch", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetSearchInfo, read infor from controls
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetSearchInfo() As Boolean

        xGetSearchInfo = False

        Try

            With mstSearchInfo
                'get keyword
                .strKeyword = txtName.Text.Trim()

                'gender
                .intGender = clsEnum.emGender.UNKNOW
                If rdMale.Checked Then .intGender = clsEnum.emGender.MALE
                If rdFemale.Checked Then .intGender = clsEnum.emGender.FEMALE

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
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillGrid() As Boolean

        xFillGrid = False

        Dim row() As DataRow

        Try
            Me.Cursor = Cursors.WaitCursor

            Dim i As Integer = 0
            Dim intStart As Integer
            Dim intEnd As Integer
            Dim intNewRow As Integer = 0
            Dim strSelect As String = ""

            mintRootID = basCommon.fncGetRoot()

            'create new datatable for binding
            If mtblGridSource IsNot Nothing Then mtblGridSource.Dispose()
            mtblGridSource = New DataTable()
            mtblGridSource.Columns.Add("GT", GetType(Image))
            mtblGridSource.Columns.Add("STT")
            mtblGridSource.Columns.Add("FULL_NAME")
            mtblGridSource.Columns.Add("LEVEL", GetType(Int32))
            mtblGridSource.Columns.Add("BDATE")
            mtblGridSource.Columns.Add("DDATE")
            mtblGridSource.Columns.Add("MEMBER_ID", GetType(Int32))
            mtblGridSource.Columns.Add("GENDER", GetType(Int32))
            mtblGridSource.Columns.Add("DECEASED", GetType(Int32))
            mtblGridSource.Columns.Add("FATHER")
            mtblGridSource.Columns.Add("MOTHER")

            'add new column LEVEL
            'If Not mtblData.Columns.Contains("GT") Then mtblData.Columns.Add("GT", GetType(Image))
            'If Not mtblData.Columns.Contains("STT") Then mtblData.Columns.Add("STT")
            'If Not mtblData.Columns.Contains("FULL_NAME") Then mtblData.Columns.Add("FULL_NAME")
            'If Not mtblData.Columns.Contains("BDATE") Then mtblData.Columns.Add("BDATE")
            'If Not mtblData.Columns.Contains("DDATE") Then mtblData.Columns.Add("DDATE")

            'strSelect = xBuildSearchQuery()
            'strSort = "LEVEL, MEMBER_ID"
            'row = mtblData.Select(strSelect, strSort)

            mtblData.DefaultView.Sort = "LEVEL ASC, MEMBER_ID"
            row = mtblData.Select()

            If row.Length <= 0 Then Return False

            intStart = (mintCurPage - 1) * mcintItemPerPage
            intEnd = mintCurPage * mcintItemPerPage - 1
            If intEnd > row.Length - 1 Then intEnd = row.Length - 1

            'bind data to fill
            dgvMain.AutoGenerateColumns = False
            dgvMain.DataSource = mtblGridSource 'mtblData
            dgvMain.Visible = False

            'For Each r As DataRow In row
            For index As Integer = intStart To intEnd

                With mstSearchData
                    'get data at row(i)
                    xGetSearchStruct(row(index))

                    Dim dtRow As DataRow = mtblGridSource.NewRow()

                    'image field
                    dtRow("GT") = GiaPha.My.Resources.Gender_unknown16
                    If .intGender = clsEnum.emGender.MALE Then dtRow("GT") = GiaPha.My.Resources.Gender_man16
                    If .intGender = clsEnum.emGender.FEMALE Then dtRow("GT") = GiaPha.My.Resources.Gender_woman16

                    'NO field
                    i += 1
                    dtRow("STT") = basCommon.fncCnvNullToString(index + 1)

                    'full name
                    dtRow("FULL_NAME") = basCommon.fncGetFullName(.strFirstName, .strMidName, .strLastName, .strAlias)

                    'generation is inserted here
                    'r("LEVEL") = ""
                    If .intLevel > 0 Then dtRow("LEVEL") = .intLevel '.ToString()

                    'birth date
                    'If .dtBirth > Date.MinValue Then objContent(4) = String.Format(basConst.gcstrDateFormat2, .dtBirth)
                    dtRow("BDATE") = basCommon.fncGetDateName("", .intBday, .intBmon, .intByea, True)

                    'decease date
                    'If .dtDie > Date.MinValue Then objContent(5) = String.Format(basConst.gcstrDateFormat2, .dtDie)
                    dtRow("DDATE") = basCommon.fncGetDateName("", .intDday, .intDmon, .intDyea, True)

                    dtRow("MEMBER_ID") = .intID
                    dtRow("GENDER") = .intGender
                    dtRow("DECEASED") = .intDecease

                    Dim strFather As String = ""
                    Dim strMother As String = ""

                    fncGetFaMoName(.intID, strFather, strMother)

                    dtRow("FATHER") = strFather
                    dtRow("MOTHER") = strMother

                    mtblGridSource.Rows.Add(dtRow)

                    'new row is in gray if this member is death
                    If .intDecease = basConst.gcintDIED Then
                        dgvMain.Rows(i - 1).DefaultCellStyle.BackColor = Color.LightGray
                    End If

                End With

            Next

            'calculate total of page
            mintTotalPage = Math.Ceiling(row.Length / mcintItemPerPage)
            basCommon.fncMakeCbPage(mintTotalPage, cbPages)
            cbPages.SelectedIndex = mintCurPage - 1

            dgvMain.Visible = True
            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillGrid", ex)
        Finally
            Erase row
            If mtblRel IsNot Nothing Then mtblRel.Dispose()
            Me.Cursor = Cursors.Default
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xBuildSearchQuery, build query string for quick search
    '      VALUE      : String
    '      PARAMS     :
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xBuildSearchQuery() As String

        Dim strSelect As String = ""

        Try
            strSelect &= gobjDB.fncBuildQueryLike("NAME", mstSearchInfo.strKeyword)

            strSelect &= " AND "

            Select Case mstSearchInfo.intGender

                Case clsEnum.emGender.UNKNOW
                    strSelect &= String.Format(" (GENDER = {0} OR GENDER = {1} OR GENDER = {2})", CInt(clsEnum.emGender.UNKNOW), CInt(clsEnum.emGender.MALE), CInt(clsEnum.emGender.FEMALE))

                Case clsEnum.emGender.MALE
                    strSelect &= " GENDER = " & clsEnum.emGender.MALE

                Case clsEnum.emGender.FEMALE
                    strSelect &= " GENDER = " & clsEnum.emGender.FEMALE

            End Select

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xBuildSearchQuery", ex)
        End Try

        Return strSelect

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillGeneration, fill LEVEL column
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillGeneration() As Boolean

        xFillGeneration = False

        Try
            Dim intRoot As Integer

            'mintGeneration = 1
            mintGeneration = My.Settings.intInitGeneration

            'mtblRel = gobjDB.fncGetFatherSon(True)
            mtblRel = gobjDB.fncGetRel()

            'add new column LEVEL
            If mtblData.Columns.Contains("LEVEL") Then mtblData.Columns.Remove("LEVEL")
            mtblData.Columns.Add("LEVEL", GetType(Int32))

            intRoot = basCommon.fncGetRoot()

            'if root member exist
            If intRoot > basConst.gcintNO_MEMBER Then
                xCountGeneration(intRoot)
                xSetGenerationOfSpouse()
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillGeneration", ex, Nothing, False)
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xCountGeneration, calculate generation
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRootID   Integer, member id
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCountGeneration(ByVal intRootID As Integer) As Boolean

        xCountGeneration = False

        Dim vwData As DataView = Nothing

        Try
            Dim intChildID As Integer

            xSetGeneration(intRootID, mintGeneration)
            mintGeneration += 1

            vwData = New DataView(mtblRel)

            ' ▽ 2012/11/14   AKB Quyet （変更内容）*********************************
            'vwData.RowFilter = String.Format("REL_FMEMBER_ID = {0} AND RELID = {1}", intRootID, CInt(clsEnum.emRelation.NATURAL))

            vwData.RowFilter = String.Format("REL_FMEMBER_ID = {0} AND (RELID = {1} OR RELID = {2})", intRootID, CInt(clsEnum.emRelation.NATURAL), CInt(clsEnum.emRelation.ADOPT))
            ' △ 2012/11/14   AKB Quyet *********************************************

            If vwData.Count > 0 Then

                For i As Integer = 0 To vwData.Count - 1

                    Integer.TryParse(basCommon.fncCnvNullToString(vwData.Item(i)("MEMBER_ID")), intChildID)

                    xCountGeneration(intChildID)

                Next


            End If

            mintGeneration -= 1

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCountGeneration", ex, Nothing, False)
        Finally
            If vwData IsNot Nothing Then vwData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetGeneration, set value to LEVEL column
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intMemID    Integer, member id
    '      PARAMS     : intGeneration Integer, generation
    '      MEMO       : 
    '      CREATE     : 2012/01/12  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetGeneration(ByVal intMemID As Integer, ByVal intGeneration As Integer) As Boolean

        xSetGeneration = False

        Try
            Dim intTempID As Integer

            For j As Integer = 0 To mtblData.Rows.Count - 1

                Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(j)("MEMBER_ID")), intTempID)

                If intTempID = intMemID Then
                    mtblData.Rows(j)("LEVEL") = String.Format("{0:000}", intGeneration)
                    Exit For
                End If

            Next

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetGeneration", ex, Nothing, False)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetGenerationOfSpouse, 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intHusID    Integer,    husband id
    '      PARAMS     : intGeneration    Integer,   generation to set
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetGenerationOfSpouse() As Boolean

        xSetGenerationOfSpouse = False

        Dim tblRel As DataTable = Nothing
        Dim vwRel As DataView = Nothing
        Dim vwData As DataView = Nothing

        Try
            Dim intHusID As Integer
            Dim intWifeID As Integer
            Dim intGeneration As Integer

            'get husband-wife relationship
            tblRel = gobjDB.fncGetRel()
            If tblRel Is Nothing Then Exit Function
            vwRel = New DataView(tblRel)
            vwData = New DataView(mtblData)

            'we search for member who doesn't have a generation number
            For i As Integer = 0 To mtblData.Rows.Count - 1

                'exit if member has generation number
                Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("LEVEL")), intGeneration)
                If intGeneration > 0 Then Continue For

                'get wife id
                Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("MEMBER_ID")), intWifeID)

                'find husband
                vwRel.RowFilter = String.Format("MEMBER_ID = {0} AND RELID = {1}", intWifeID, CInt(clsEnum.emRelation.MARRIAGE))

                'if she has a husband
                If vwRel.Count > 0 Then

                    For j As Integer = 0 To vwRel.Count - 1
                        'get husband id
                        Integer.TryParse(basCommon.fncCnvNullToString(vwRel(j)("REL_FMEMBER_ID")), intHusID)

                        intGeneration = 0

                        'get generation
                        vwData.RowFilter = String.Format("MEMBER_ID = {0}", intHusID)
                        If vwData.Count > 0 Then
                            Integer.TryParse(basCommon.fncCnvNullToString(vwData(0)("LEVEL")), intGeneration)
                        End If

                        If intGeneration > 0 Then mtblData.Rows(i)("LEVEL") = String.Format("{0:000}", intGeneration)

                    Next

                End If

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetGenerationOfSpouse", ex)
        Finally
            If tblRel IsNot Nothing Then tblRel.Dispose()
            If vwRel IsNot Nothing Then vwRel.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetSearchStruct, read data at row
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : row DataRow, row to read data
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetSearchStruct(ByVal row As DataRow) As Boolean

        xGetSearchStruct = False

        Try
            'get data at row
            With row
                'member id
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("MEMBER_ID")), mstSearchData.intID)

                'full name
                mstSearchData.strFirstName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))
                mstSearchData.strMidName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                mstSearchData.strLastName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))

                'alias
                mstSearchData.strAlias = basCommon.fncCnvNullToString(.Item("ALIAS_NAME"))

                'birth and decease date
                'Date.TryParse(basCommon.fncCnvNullToString(.Item("BIRTH_DAY")), mstSearchData.dtBirth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY")), mstSearchData.intBday)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON")), mstSearchData.intBmon)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA")), mstSearchData.intByea)


                'Date.TryParse(basCommon.fncCnvNullToString(.Item("DECEASED_DATE")), mstSearchData.dtDie)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_DAY")), mstSearchData.intDday)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_MON")), mstSearchData.intDmon)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_YEA")), mstSearchData.intDyea)

                'gender
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("GENDER")), mstSearchData.intGender)

                'decease
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DECEASED")), mstSearchData.intDecease)

                'generation
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("LEVEL")), mstSearchData.intLevel)

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetSearchStruct", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetID, get user id from grid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : e DataGridViewCellMouseEventArgs,
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetID(ByVal intRowIndex As Integer, ByRef intId As Integer, ByRef intSex As clsEnum.emGender) As Boolean

        xGetID = False

        Try
            Dim intRowID As Integer = clsDefine.NONE_VALUE
            'Dim intMemNumber As Integer = clsDefine.NONE_VALUE
            Dim intGender As Integer = clsDefine.NONE_VALUE

            intRowID = intRowIndex

            If intRowID <= clsDefine.NONE_VALUE Then Exit Function

            ' ▽ 2012/11/14   AKB Quyet （変更内容）*********************************
            'Integer.TryParse(basCommon.fncCnvNullToString(dgvMain.Item(6, intRowID).Value), intId)         '6 is MemberID hidden column
            'Integer.TryParse(basCommon.fncCnvNullToString(dgvMain.Item(7, intRowID).Value), intGender)      '7 is MemberID hidden column


            Try
                intId = CInt(basCommon.fncCnvNullToString(dgvMain.Item(6, intRowID).Value))
            Catch ex As Exception
                intId = basConst.gcintNO_MEMBER
            End Try

            Try
                intGender = CInt(basCommon.fncCnvNullToString(dgvMain.Item(7, intRowID).Value))
            Catch ex As Exception
                intGender = clsDefine.NONE_VALUE
            End Try
            ' △ 2012/11/14   AKB Quyet *********************************************


            ''read row number
            'Integer.TryParse(basCommon.fncCnvNullToString(dgvMain.Item(1, intRowID).Value), intMemNumber)

            ''get id, gender from datatable with row number
            'Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(intMemNumber - 1).Item("MEMBER_ID")), mintID)
            'Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(intMemNumber - 1).Item("GENDER")), intGender)

            'determine gender
            intSex = clsEnum.emGender.MALE
            If intGender = clsEnum.emGender.FEMALE Then intSex = clsEnum.emGender.FEMALE

            'store current generation
            'xGetCurrentGeneration(intRowID)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetID", ex)
        End Try

    End Function


    ''' <summary>
    ''' xGetCurrentGeneration
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xGetCurrentGeneration(ByVal intRowIndex As Integer) As Boolean

        xGetCurrentGeneration = False
        Try
            mintCurGeneration = basCommon.fncCnvToInt(dgvMain.Item(clmLevel.Index, intRowIndex).Value)
            If mintCurGeneration <= 0 Then mintCurGeneration = basConst.gcintNONE_VALUE

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetID", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetSelectedRow, set selected row when member changes
    '      PARAMS     : intCurrentID Integer, current member id
    '      MEMO       : 
    '      CREATE     : 2011/12/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSetSelectedRow(ByVal intCurrentID As Integer)

        Dim dtRow() As DataRow

        Try
            Dim intID As Integer
            Dim intPage As Integer

            If mtblData Is Nothing Then Exit Sub

            mtblData.DefaultView.Sort = "LEVEL ASC, MEMBER_ID"
            dtRow = mtblData.Select()

            'find in datatable
            For index As Integer = 0 To dtRow.Length - 1
                Integer.TryParse(basCommon.fncCnvNullToString(dtRow(index).Item("MEMBER_ID")), intID)

                'continue if does not match
                If intCurrentID <> intID Then Continue For

                'match! find the page which this member is on
                intPage = Math.Ceiling((index + 1) / mcintItemPerPage)
                If intPage = 0 Then intPage = 1

                'if the page is different, go to page
                If intPage <> mintCurPage Then
                    mintCurPage = intPage
                    xFillGrid()
                End If

                'xSetSelectedRowEx(intCurrentID)
                'set select row
                For i As Integer = 0 To dgvMain.Rows.Count - 1

                    'read member id
                    'Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i).Item("MEMBER_ID")), intID)
                    Integer.TryParse(basCommon.fncCnvNullToString(dgvMain.Rows(i).Cells("MEMBER_ID").Value), intID)

                    'continue if does not match
                    If intCurrentID <> intID Then Continue For

                    'match! set this row as selected then exit
                    dgvMain.Rows.Item(i).Selected = True

                    'store current generation
                    'xGetCurrentGeneration(i)

                    dgvMain.FirstDisplayedScrollingRowIndex = i

                    Exit For

                Next

            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetSelectedRow", ex)
        Finally
            Erase dtRow
        End Try

    End Sub


#Region "Next Anniversary"

    '   ******************************************************************
    '　　　FUNCTION   : xNextAnniversary, find next birth-decease date
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xNextAnniversary() As Boolean

        xNextAnniversary = False

        Try
            Dim objThread As System.Threading.Thread
            objThread = New System.Threading.Thread(AddressOf xGetAnniversary)

            lblAnniBirth.Enabled = False
            lblAnniDecease.Enabled = False

            objThread.Start()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xNextAnniversary", ex, Nothing, False)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetAnniversary, find next birth-decease date
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xGetAnniversary()

        Dim lstAnniBirth As List(Of String) = Nothing
        Dim lstAnniDecease As List(Of String) = Nothing
        Dim frmFukc As frmPersonalAnniversary = Nothing

        Try
            'mfrmAnni = New frmPersonalAnniversary()
            'lstAnniBirth = mfrmAnni.AnniBirth
            'lstAnniDecease = mfrmAnni.AnniDecease

            frmFukc = New frmPersonalAnniversary()
            lstAnniBirth = frmFukc.AnniBirth
            lstAnniDecease = frmFukc.AnniDecease

            mstrAnniBirthList = ""
            mstrAnniDeceaseList = ""

            xFillAnniList(lstAnniBirth, mstrAnniBirthList)
            xFillAnniList(lstAnniDecease, mstrAnniDeceaseList)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetAnniversary", ex)
        Finally
            If lstAnniBirth IsNot Nothing Then lstAnniBirth.Clear()
            If lstAnniDecease IsNot Nothing Then lstAnniDecease.Clear()
            Try
                If frmFukc IsNot Nothing Then frmFukc.Dispose()
            Catch ex As Exception
                basCommon.fncSaveErr(mcstrClsName, "xNextAnniversary", ex, Nothing, False)
            End Try
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xFillAnniList, fill to label
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : lstName List(Of String), list of member
    '      PARAMS     : strList String
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillAnniList(ByVal lstName As List(Of String), ByRef strList As String) As Boolean

        xFillAnniList = False

        Dim objSetText As MethodInvoker = Nothing

        Try
            For i As Integer = 0 To lstName.Count - 1

                strList &= lstName(i) & ", "

            Next

            If strList.Length > 30 Then strList = strList.Substring(0, 30)

            strList &= "... (danh sách)"

            'set text
            objSetText = New MethodInvoker(AddressOf xSetAnniText)
            Me.Invoke(objSetText)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillAnniList", ex, Nothing, False)
        Finally
            objSetText = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetAnniText, Set label text
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSetAnniText()

        Try
            lblAnniBirth.Text = mstrAnniBirthList
            lblAnniDecease.Text = mstrAnniDeceaseList

            lblAnniBirth.Enabled = True
            lblAnniDecease.Enabled = True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetAnniText", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xShowAnniBirth, Show form of Anni Birth
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xShowAnniBirth()

        Try
            'exit if the form is showing
            If mfrmAnni IsNot Nothing Then
                If mfrmAnni.FormShown Then
                    xProgressDone()
                    Exit Sub
                End If
            End If

            'new form
            'mfrmAnni = New frmPersonalAnniversary()

            'set event handler to close waiting dialog
            AddHandler mfrmAnni.evnShown, AddressOf xProgressDone

            'show in birth list mode
            mfrmAnni.fncShowForm(frmPersonalAnniversary.emFormMode.BIRTH_LIST)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowAnniBirth", ex)
        Finally
            'If mfrmAnni IsNot Nothing Then mfrmAnni.Dispose()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xShowAnniDecease, Show form of Anni Birth
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xShowAnniDecease()

        Try
            'exit if the form is showing
            If mfrmAnni IsNot Nothing Then
                If mfrmAnni.FormShown Then
                    xProgressDone()
                    Exit Sub
                End If
            End If

            'new form
            'mfrmAnni = New frmPersonalAnniversary()

            'set event handler to close waiting dialog
            AddHandler mfrmAnni.evnShown, AddressOf xProgressDone

            'show in birth list mode
            mfrmAnni.fncShowForm(frmPersonalAnniversary.emFormMode.DECEASE_LIST)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowAnniDecease", ex)
        Finally
            'If mfrmAnni IsNot Nothing Then mfrmAnni.Dispose()
        End Try

    End Sub


    Private Sub xShowPersonInfo(ByVal intMemberID As Integer)

        Try
            If intMemberID <= basConst.gcintNO_MEMBER Then Exit Sub

            'show form in edit mode
            mfrmPerInfo.FormMode = clsEnum.emMode.EDIT
            mfrmPerInfo.MemberID = intMemberID
            mfrmPerInfo.fncShowForm()

            'If mfrmPerInfo.FormModified Then xUpdate()
            'If mfrmPerInfo.FormModified Then xRefresh(mintID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowPersonInfo", ex)

        End Try

    End Sub

#End Region



#End Region


#Region "event handler"


    '   ******************************************************************
    '　　　FUNCTION   : xRefresh, handles refresh event when add new mem
    '      MEMO       : 
    '      CREATE     : 2011/09/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xRefresh(ByVal intCurID As Integer, ByVal blnRedraw As Boolean)

        Try
            If basCommon.fncMemberExist(intCurID) Then
                'if current member is exist
                Me.mintID = intCurID
            Else
                'otherwise set current member is none
                ' ▽ 2012/11/22   AKB Quyet （........）*********************************
                'Me.mintID = basConst.gcintNO_MEMBER

                Dim emGender As clsEnum.emGender = clsEnum.emGender.UNKNOW
                xGetID(0, Me.mintID, emGender)
                ' △ 2012/11/22   AKB Quyet *********************************************
            End If

            'redraw card
            ' ▽ 2012/11/22   AKB Quyet （........）*********************************
            'If blnRedraw Then mclsDrawCard.ActiveMemberID = mintID

            If blnRedraw Then
                If memFormMode = emFormMode.SHOW_CARD Then mclsDrawCard.ActiveMemberID = mintID
            End If
            ' △ 2012/11/22   AKB Quyet *********************************************

            'next birthday and decease day
            xNextAnniversary()

            xQuickSearch()

            'xSetSelectedRow(mclsDrawCard.ActiveMemberID)
            xSetSelectedRow(Me.mintID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRefresh", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xUpdate, reload data when needed
    '      MEMO       : 
    '      CREATE     : 2011/12/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xUpdate()

        Try
            xRefresh(mintID, True)

            ''redraw card
            'mclsDrawCard.ActiveMemberID = mintID

            ''set selected row
            'xSetSelectedRow(mintID)

            ''next birthday and decease day
            'xNextAnniversary()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xUpdate", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xRefreshSpouseList, update spouse list
    '      MEMO       : 
    '      CREATE     : 2011/12/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xRefreshSpouseList()

        Try
            mclsDrawCard.fncRefreshSpouseList()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRefreshSpouseList", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCardClick, handle click on card
    '      MEMO       : 
    '      CREATE     : 2011/11/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCardClick(ByVal intCurID As Integer)

        Try
            mintID = intCurID

            xSetSelectedRow(intCurID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCardClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xBackup, backup data
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xBackup()

        Dim objBackup As clsReplication

        Try
            objBackup = New clsReplication()

            AddHandler objBackup.evnBackedUp, AddressOf xProgressDone

            objBackup.fncBackup(mstrBackupPath)

            'next birthday and decease day
            'xNextAnniversary()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xBackup", ex, Nothing, False)
        Finally
            objBackup = Nothing
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xProgressDone, close waiting dialog
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xProgressDone()

        Try
            Dim objCloseWaitForm As MethodInvoker

            'close thread
            mobjLoadingThread = Nothing

            'close waiting form
            objCloseWaitForm = New MethodInvoker(AddressOf xCloseWaitForm)
            Me.Invoke(objCloseWaitForm)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xProgressDone", ex, Nothing, False)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCloseWaitForm, close waiting form
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCloseWaitForm()

        Try
            If mfrmWaiting IsNot Nothing Then
                mfrmWaiting.Close()
                mfrmWaiting.Dispose()
            End If
            mfrmWaiting = Nothing

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCloseWaitForm", ex, Nothing, False)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xRightMenuClosed, remove context menu from grid
    '      MEMO       : 
    '      CREATE     : 2012/02/01  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xRightMenuClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs)

        Try
            dgvMain.ContextMenuStrip = Nothing

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRightMenuClosed", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xRightMenuItemClick, handle right menu clicked
    '      MEMO       : 
    '      CREATE     : 2012/02/01  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xRightMenuItemClick(ByVal intId As Integer, ByVal sender As System.Object)

        Try
            Dim objMenuItem As ToolStripMenuItem = Nothing
            Dim blnSuccess As Boolean = True
            Dim intRootID As Integer

            'get menu item
            objMenuItem = CType(sender, ToolStripMenuItem)

            Select Case objMenuItem.Text

                'show detail info
                Case My.Resources.StrPersonInfo
                    'if there is an member - show detail form
                    xShowPersonInfo(intId)

                    '================== ▼ Start other process ▼ ====================

                    'refresh generation
                Case My.Resources.StrRefreshGeneration
                    Me.Cursor = Cursors.WaitCursor
                    intRootID = basCommon.fncGetRoot
                    basCommon.fncSetGeneration(intRootID, My.Settings.intInitGeneration)
                    xRefresh(intId, False)

                    'add member to root
                Case My.Resources.StrAddRoot
                    'If intGender <> clsEnum.emGender.MALE Then Exit Select
                    Me.Cursor = Cursors.WaitCursor
                    basCommon.fncSetGeneration(intId, My.Settings.intInitGeneration)
                    If Not gobjDB.fncInsertRoot(intId) Then basCommon.fncMessageError(gcstrFail) Else xRefresh(intId, True)

                    'add member to family head list
                Case My.Resources.StrAddFamilyHead
                    'If intGender <> clsEnum.emGender.MALE Then Exit Select
                    Me.Cursor = Cursors.WaitCursor
                    If Not gobjDB.fncInsertFHead(intId) Then basCommon.fncMessageError(gcstrFail) Else xRefresh(intId, True)

                    'remove member from root list
                Case My.Resources.StrDelFromRoot
                    Me.Cursor = Cursors.WaitCursor
                    If Not gobjDB.fncDelRoot(intId) Then
                        basCommon.fncMessageError(gcstrFail)
                    Else
                        intRootID = basCommon.fncGetRoot()
                        If intRootID > basConst.gcintNO_MEMBER Then
                            basCommon.fncSetGeneration(intRootID, My.Settings.intInitGeneration)
                        Else
                            'clear generation
                            gobjDB.fncSetMemberGeneration(-1)
                        End If
                        xRefresh(intId, True)
                    End If

                    'remove member from family head list
                Case My.Resources.StrDelFromFamilyHead
                    Me.Cursor = Cursors.WaitCursor
                    If Not gobjDB.fncDelFhead(intId) Then basCommon.fncMessageError(gcstrFail) Else xRefresh(intId, True)

                    'delete member
                Case My.Resources.StrDelMember
                    If Not basCommon.fncMessageConfirm(String.Format(basConst.gcstrMessageConfirm, basCommon.fncGetMemberName(intId))) Then Exit Select

                    Me.Cursor = Cursors.WaitCursor
                    If Not basCommon.fncDeleteMember(intId) Then
                        basCommon.fncMessageError(gcstrFail)
                    Else
                        'refresh
                        If intId = mintID Then xRefresh(intId, True) Else xRefresh(mintID, True)

                    End If

            End Select



        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRightMenuItemClick", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xActivated, activate software
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xActivated()

        Try
            If Not tsmActivate.Visible Then Exit Sub

            Me.Text = "Chương trình quản lý Gia Phả"
            tsmActivate.Visible = False

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xActivated", ex)
        End Try

    End Sub


#End Region


#Region "Check Version"


    '   ******************************************************************
    '　　　FUNCTION   : xCheckDatabase, syn database 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/04/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCheckDatabase() As Boolean

        xCheckDatabase = False

        Dim lstFieldName As List(Of String) = New List(Of String)
        Dim lstFieldType As List(Of String) = New List(Of String)
        Dim blnIsOld As Boolean

        Try
            Dim blnRefreshLv As Boolean = False

            blnIsOld = False : lstFieldName.Clear() : lstFieldType.Clear()
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "BIR_DAY") Then lstFieldName.Add("BIR_DAY") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "BIR_MON") Then lstFieldName.Add("BIR_MON") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "BIR_YEA") Then lstFieldName.Add("BIR_YEA") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "DEA_DAY") Then lstFieldName.Add("DEA_DAY") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "DEA_MON") Then lstFieldName.Add("DEA_MON") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "DEA_YEA") Then lstFieldName.Add("DEA_YEA") : lstFieldType.Add("NUMBER") : blnIsOld = True

            'If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "BIR_DAY_SUN") Then lstFieldName.Add("BIR_DAY_SUN") : lstFieldType.Add("NUMBER") : blnIsOld = True
            'If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "BIR_MON_SUN") Then lstFieldName.Add("BIR_MON_SUN") : lstFieldType.Add("NUMBER") : blnIsOld = True
            'If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "BIR_YEA_SUN") Then lstFieldName.Add("BIR_YEA_SUN") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "DEA_DAY_SUN") Then lstFieldName.Add("DEA_DAY_SUN") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "DEA_MON_SUN") Then lstFieldName.Add("DEA_MON_SUN") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "DEA_YEA_SUN") Then lstFieldName.Add("DEA_YEA_SUN") : lstFieldType.Add("NUMBER") : blnIsOld = True

            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "BIR_DAY_LUNAR") Then lstFieldName.Add("BIR_DAY_LUNAR") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "BIR_MON_LUNAR") Then lstFieldName.Add("BIR_MON_LUNAR") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "BIR_YEA_LUNAR") Then lstFieldName.Add("BIR_YEA_LUNAR") : lstFieldType.Add("NUMBER") : blnIsOld = True
            'If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "DEA_DAY_LUNAR") Then lstFieldName.Add("DEA_DAY_LUNAR") : lstFieldType.Add("NUMBER") : blnIsOld = True
            'If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "DEA_MON_LUNAR") Then lstFieldName.Add("DEA_MON_LUNAR") : lstFieldType.Add("NUMBER") : blnIsOld = True
            'If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "DEA_YEA_LUNAR") Then lstFieldName.Add("DEA_YEA_LUNAR") : lstFieldType.Add("NUMBER") : blnIsOld = True


            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "CAREER_TYPE") Then lstFieldName.Add("CAREER_TYPE") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "EDUCATION_TYPE") Then lstFieldName.Add("EDUCATION_TYPE") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "FACT_TYPE") Then lstFieldName.Add("FACT_TYPE") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "CAREER") Then lstFieldName.Add("CAREER") : lstFieldType.Add("MEMO") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "EDUCATION") Then lstFieldName.Add("EDUCATION") : lstFieldType.Add("MEMO") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "FACT") Then lstFieldName.Add("FACT") : lstFieldType.Add("MEMO") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_MAIN", "LEVEL") Then lstFieldName.Add("LEVEL") : lstFieldType.Add("NUMBER") : blnIsOld = True : blnRefreshLv = True

            If blnIsOld Then
                gobjDB.fncInsertColumn("T_FMEMBER_MAIN", lstFieldName, lstFieldType)
                xConvertDateMain()
            End If

            'Modify columns
            blnIsOld = False : lstFieldName.Clear() : lstFieldType.Clear()
            lstFieldName.Add("LAST_NAME") : lstFieldType.Add("TEXT(100)")
            lstFieldName.Add("MIDDLE_NAME") : lstFieldType.Add("TEXT(100)")
            lstFieldName.Add("FIRST_NAME") : lstFieldType.Add("TEXT(100)")
            lstFieldName.Add("ALIAS_NAME") : lstFieldType.Add("TEXT(200)")
            gobjDB.fncAlterColumn("T_FMEMBER_MAIN", lstFieldName, lstFieldType)


            blnIsOld = False : lstFieldName.Clear() : lstFieldType.Clear()
            If Not basCommon.fncIsColumnExist("T_FMEMBER_CAREER", "START_DAY") Then lstFieldName.Add("START_DAY") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_CAREER", "START_MON") Then lstFieldName.Add("START_MON") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_CAREER", "START_YEA") Then lstFieldName.Add("START_YEA") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_CAREER", "END_DAY") Then lstFieldName.Add("END_DAY") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_CAREER", "END_MON") Then lstFieldName.Add("END_MON") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_CAREER", "END_YEA") Then lstFieldName.Add("END_YEA") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If blnIsOld Then
                gobjDB.fncInsertColumn("T_FMEMBER_CAREER", lstFieldName, lstFieldType)
                xConvertDateCareer(clsEnum.emCareerType.CAREER) : xConvertDateCareer(clsEnum.emCareerType.EDU)
            End If


            blnIsOld = False : lstFieldName.Clear() : lstFieldType.Clear()
            If Not basCommon.fncIsColumnExist("T_FMEMBER_FACT", "START_DAY") Then lstFieldName.Add("START_DAY") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_FACT", "START_MON") Then lstFieldName.Add("START_MON") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_FACT", "START_YEA") Then lstFieldName.Add("START_YEA") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_FACT", "END_DAY") Then lstFieldName.Add("END_DAY") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_FACT", "END_MON") Then lstFieldName.Add("END_MON") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If Not basCommon.fncIsColumnExist("T_FMEMBER_FACT", "END_YEA") Then lstFieldName.Add("END_YEA") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If blnIsOld Then
                gobjDB.fncInsertColumn("T_FMEMBER_FACT", lstFieldName, lstFieldType)
                xConvertDateFact()
            End If

            blnIsOld = False : lstFieldName.Clear() : lstFieldType.Clear()
            If Not basCommon.fncIsColumnExist("T_FMEMBER_RELATION", "ROLE_ORDER") Then lstFieldName.Add("ROLE_ORDER") : lstFieldType.Add("NUMBER") : blnIsOld = True
            If blnIsOld Then
                gobjDB.fncInsertColumn("T_FMEMBER_RELATION", lstFieldName, lstFieldType)
            End If

            gobjDB.fncUpdateReligion(1, "Không")
            gobjDB.fncUpdateReligion(4, "Thiên Chúa giáo")

            'refresh level for new database
            If blnRefreshLv Then
                basCommon.fncSetGeneration(basCommon.fncGetRoot(), My.Settings.intInitGeneration)
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCheckDatabase", ex)
        Finally
            lstFieldName = Nothing
            lstFieldType = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xConvertDateMain, convert from old data to the new one
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/04/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xConvertDateMain() As Boolean

        xConvertDateMain = False

        Dim tblData As DataTable = Nothing

        Try
            Dim stBirth As stCalendar
            Dim stDeath As stCalendar

            Dim intMemID As Integer
            Dim dtBirth As Date
            Dim dtDeath As Date

            tblData = gobjDB.fncGetMemberMain()

            If tblData Is Nothing Then Exit Function

            For i As Integer = 0 To tblData.Rows.Count - 1

                stBirth = Nothing
                stDeath = Nothing

                Integer.TryParse(tblData.Rows(i)("MEMBER_ID"), intMemID)
                Date.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("BIRTH_DAY")), dtBirth)
                Date.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("DECEASED_DATE")), dtDeath)

                'get date if available
                If dtBirth > Date.MinValue And dtBirth < Date.MaxValue Then
                    stBirth.intDay = dtBirth.Day
                    stBirth.intMonth = dtBirth.Month
                    stBirth.intYear = dtBirth.Year
                End If

                If dtDeath > Date.MinValue And dtDeath < Date.MaxValue Then
                    stDeath.intDay = dtDeath.Day
                    stDeath.intMonth = dtDeath.Month
                    stDeath.intYear = dtDeath.Year
                End If

                gobjDB.fncFixDateTimeMain(intMemID, stBirth, stDeath)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xConvertDateMain", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xConvertDateCareer, convert from old data to the new one
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : emCareerType    clsEnum.emCareerType
    '      MEMO       : 
    '      CREATE     : 2012/04/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xConvertDateCareer(ByVal emCareerType As clsEnum.emCareerType) As Boolean

        xConvertDateCareer = False

        Dim tblData As DataTable = Nothing

        Try
            Dim stStart As stCalendar
            Dim stEnd As stCalendar

            Dim intMemID As Integer
            Dim intCareerID As Integer
            Dim dtStart As Date
            Dim dtEnd As Date

            tblData = gobjDB.fncGetCareer(emCareerType)

            If tblData Is Nothing Then Exit Function

            For i As Integer = 0 To tblData.Rows.Count - 1

                stStart = Nothing
                stEnd = Nothing

                Integer.TryParse(tblData.Rows(i)("MEMBER_ID"), intMemID)
                Integer.TryParse(tblData.Rows(i)("CAREER_ID"), intCareerID)
                Date.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("START_DATE")), dtStart)
                Date.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("END_DATE")), dtEnd)

                'get date if available
                If dtStart > Date.MinValue And dtStart < Date.MaxValue Then
                    stStart.intDay = dtStart.Day
                    stStart.intMonth = dtStart.Month
                    stStart.intYear = dtStart.Year
                End If

                If dtEnd > Date.MinValue And dtEnd < Date.MaxValue Then
                    stEnd.intDay = dtEnd.Day
                    stEnd.intMonth = dtEnd.Month
                    stEnd.intYear = dtEnd.Year
                End If

                gobjDB.fncFixDateTimeCareer(intMemID, intCareerID, emCareerType, stStart, stEnd)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xConvertDateCareer", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xConvertDateCareer, convert from old data to the new one
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/04/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xConvertDateFact() As Boolean

        xConvertDateFact = False

        Dim tblData As DataTable = Nothing

        Try
            Dim stStart As stCalendar
            Dim stEnd As stCalendar

            Dim intMemID As Integer
            Dim intFactID As Integer
            Dim dtStart As Date
            Dim dtEnd As Date

            tblData = gobjDB.fncGetFact()

            If tblData Is Nothing Then Exit Function

            For i As Integer = 0 To tblData.Rows.Count - 1

                stStart = Nothing
                stEnd = Nothing

                Integer.TryParse(tblData.Rows(i)("MEMBER_ID"), intMemID)
                Integer.TryParse(tblData.Rows(i)("FACT_ID"), intFactID)
                Date.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("START_DATE")), dtStart)
                Date.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("END_DATE")), dtEnd)

                'get date if available
                If dtStart > Date.MinValue And dtStart < Date.MaxValue Then
                    stStart.intDay = dtStart.Day
                    stStart.intMonth = dtStart.Month
                    stStart.intYear = dtStart.Year
                End If

                If dtEnd > Date.MinValue And dtEnd < Date.MaxValue Then
                    stEnd.intDay = dtEnd.Day
                    stEnd.intMonth = dtEnd.Month
                    stEnd.intYear = dtEnd.Year
                End If

                gobjDB.fncFixDateTimeFact(intMemID, intFactID, stStart, stEnd)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xConvertDateFact", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function


#End Region


#Region "Temporary not used"


    ''   ******************************************************************
    ''　　　FUNCTION   : xMenuAction, handle right click on member card
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : sender Object
    ''      PARAMS     : intRelation Integer, relation type
    ''      MEMO       : 
    ''      CREATE     : 2011/09/15  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xMenuAction(ByVal sender As System.Object, ByVal intRelation As Integer) As Boolean

    '    xMenuAction = False

    '    Try
    '        Dim objMenuItem As ToolStripMenuItem
    '        Dim objMenu As ContextMenuStrip
    '        Dim objCard As usrMemberCard

    '        'get menu item
    '        objMenuItem = CType(sender, ToolStripMenuItem)

    '        'get menu
    '        objMenu = CType(objMenuItem.Owner, ContextMenuStrip)

    '        'get card and user id
    '        objCard = CType(objMenu.SourceControl, usrMemberCard)

    '        'add new 
    '        xAddNewMem(objCard, intRelation)

    '        Return True

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "xMenuAction", ex)
    '    End Try

    'End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : xAddNewMem, add new member
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : objCard usrMemberCard, active card
    ''      PARAMS     : intRelation Integer, relation type
    ''      MEMO       : 
    ''      CREATE     : 2011/09/15  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xAddNewMem(ByVal objCard As usrMemberCard, ByVal intRelation As Integer) As Boolean

    '    xAddNewMem = False

    '    Try
    '        Dim blnBeginTrans As Boolean
    '        Dim intMemId As Integer
    '        Dim intNewID As Integer

    '        blnBeginTrans = False

    '        'get active member id
    '        intMemId = objCard.CardID

    '        'set form mode to ADD 
    '        mfrmPerInfo.FormMode = clsEnum.emMode.ADD

    '        'set gender for new member
    '        mfrmPerInfo.MemberGender = clsEnum.emGender.UNKNOW

    '        'show form
    '        If Not mfrmPerInfo.fncShowForm() Then Exit Function

    '        'if member is added
    '        If Not mfrmPerInfo.FormModified Then Exit Function

    '        'get new member id
    '        intNewID = mfrmPerInfo.MemberID

    '        'start stransaction
    '        blnBeginTrans = gobjDB.BeginTransaction()

    '        'add new spouse - add 2 record for 2 way relationship
    '        If intRelation = CInt(clsEnum.emRelation.MARRIAGE) Then

    '            blnBeginTrans = gobjDB.fncInsertRel(intNewID, intMemId, clsEnum.emRelation.MARRIAGE, False)
    '            blnBeginTrans = gobjDB.fncInsertRel(intMemId, intNewID, clsEnum.emRelation.MARRIAGE, False)

    '        End If

    '        'add new child
    '        If intRelation = cint(clsEnum.emRelation.NATURAL) Then

    '            blnBeginTrans = gobjDB.fncInsertRel(intNewID, intMemId, clsEnum.emRelation.NATURAL, False)

    '        End If

    '        'commit and refresh
    '        If blnBeginTrans Then

    '            gobjDB.Commit()

    '            'redraw
    '            mclsDrawTree.fncDraw(0)

    '        Else
    '            'fail - rollback
    '            gobjDB.RollBack()

    '        End If

    '        Return True

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "xAddNewMem", ex)
    '    End Try

    'End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : tsbPDF_Click,handles key pressed event
    ''      MEMO       : 
    ''      CREATE     : 2012/01/12  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Sub tsbPDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        Dim objXlsPrint As clsPdf = New clsPdf(mclsDrawTree1.MaxWidth, mclsDrawTree1.MaxHeight)

    '        'If Not pnShow.Visible Then Exit Sub

    '        'dlgPrint = New PrintDialog()
    '        'prdPrint = New Printing.PrintDocument()

    '        'AddHandler prdPrint.PrintPage, AddressOf xPrintDocument_PrintPage

    '        'dlgPrint.Document = prdPrint

    '        'If dlgPrint.ShowDialog() = Windows.Forms.DialogResult.OK Then

    '        '    prdPrint.Print()

    '        'End If
    '        mpnShowTree.AutoScrollPosition = New Point(0, 0)

    '        'try to export F-tree to Excel
    '        If objXlsPrint.fncExportTree(mclsDrawTree1.DrawingCard, mclsDrawTree1.NotDrawingCard, mblnDrawCompactTree) Then
    '            objXlsPrint.Save("D:\test.pdf")
    '        End If
    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "tsbPDF_Click", ex)
    '    End Try
    'End Sub


#End Region


    Private Sub tscboGeneration_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tscboGeneration.SelectedIndexChanged

        Try
            'do nothing if it is already in SHOW_TREE mode
            If tscboGeneration.Visible = True Then

                My.Settings.intMaxDrawGeneration = CInt(tscboGeneration.SelectedItem)
                My.Settings.Save()

                If mblnOptChanged Then

                    If basCommon.fncMemberExist(Me.mintID) Then
                        xShowViewTree(True)
                    Else
                        'there is no member, turn back to family card
                        xSetVisibleDrawTreeTools(False)
                        xShowViewTree(False)

                    End If

                End If

                mblnOptChanged = True

            End If

        Catch ex As Exception

        End Try

    End Sub



#Region "Move Family Panel"

    Private mblnMovePanel As Boolean = False                    'flag 
    Private mptMouseDownPoint As Point = New Point(0, 0)        'start mouse point  
    Private mintCurX As Integer = 0                             'current X value of scrollbar
    Private mintCurY As Integer = 0                             'current Y value of scrollbar

    ''' <summary>
    ''' Mouse hover on family card panel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pnFamilyCard_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnFamilyCard.MouseHover
        Try
            'change mouse style
            If pnFamilyCard.HorizontalScroll.Visible Or pnFamilyCard.VerticalScroll.Visible Then
                pnFamilyCard.Cursor = Cursors.SizeAll
            Else
                pnFamilyCard.Cursor = Cursors.Arrow
            End If
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "pnFamilyCard_MouseHover", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Mouse down on family card panel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pnFamilyCard_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnFamilyCard.MouseDown
        Try
            If pnFamilyCard.HorizontalScroll.Visible Or pnFamilyCard.VerticalScroll.Visible Then

                'get current values
                mblnMovePanel = True
                mptMouseDownPoint = Windows.Forms.Cursor.Position

                mintCurX = pnFamilyCard.HorizontalScroll.Value
                mintCurY = pnFamilyCard.VerticalScroll.Value

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "pnFamilyCard_MouseDown", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Mouse move on family card panel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pnFamilyCard_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnFamilyCard.MouseMove
        Try
            If Not mblnMovePanel Then Exit Sub
            Dim ptNewMouse As Point
            Dim intX As Integer
            Dim intY As Integer
            Dim intNewX As Integer
            Dim intNewY As Integer

            'calculate and set new position
            ptNewMouse = Windows.Forms.Cursor.Position
            intX = ptNewMouse.X - mptMouseDownPoint.X
            intY = ptNewMouse.Y - mptMouseDownPoint.Y

            intNewX = mintCurX - intX

            intNewY = mintCurY - intY


            pnFamilyCard.AutoScrollPosition = New Point(intNewX, intNewY)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "pnFamilyCard_MouseMove", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Mouse up on family card panel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pnFamilyCard_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnFamilyCard.MouseUp
        Try
            'reset flag
            mblnMovePanel = False

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "pnFamilyCard_MouseUp", ex)
        End Try
    End Sub

#End Region

    Private Sub tsbMenuTree1Basic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbMenuTree1Basic.Click

        tsbMenuTree1Basic.Checked = True
        tsbMenuTree1Open.Checked = False
        tsbMenuTree1Impact.Checked = False
        ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
        xSelectedTypeTree(clsEnum.emCardStyle.CARD1, My.Settings.blnTypeCardShort, My.Settings.intSelectedTypeCardShort)
        ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
        'xShowViewTree(True)    'Remove 2019.08.26

    End Sub

    Private Sub tsbMenuTree1Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbMenuTree1Open.Click
        tsbMenuTree1Basic.Checked = False
        tsbMenuTree1Open.Checked = True
        tsbMenuTree1Impact.Checked = False
        ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
        xSelectedTypeTree(clsEnum.emCardStyle.CARD1, My.Settings.blnTypeCardShort, My.Settings.intSelectedTypeCardShort)
        ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
        'xShowViewTree(True)    'Remove 2019.08.26
    End Sub

    Private Sub tsbMenuTree1Impact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbMenuTree1Impact.Click
        tsbMenuTree1Basic.Checked = False
        tsbMenuTree1Open.Checked = False
        tsbMenuTree1Impact.Checked = True
        xShowViewTree(True)
    End Sub

    '   ******************************************************************
    '　　　	FUNCTION   : btExportExcell_Click
    '      	MEMO       : Export Excell
    '      	CREATE     : 2017/06/02 Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub btExportExcell_Click(sender As Object, e As EventArgs) Handles btExportExcell.Click

        If IsNothing(mtblData) OrElse mtblData.Rows.Count = 0 Then
            fncMessageWarning(gcstrNoData, "")
            Exit Sub
        End If

        Dim i As Integer
        Dim row() As DataRow
        Dim tblData = New DataTable()

        tblData.Columns.Add("GT", GetType(Image))
        tblData.Columns.Add("STT")
        tblData.Columns.Add("FULL_NAME")
        tblData.Columns.Add("LEVEL", GetType(Int32))
        tblData.Columns.Add("BDATE")
        tblData.Columns.Add("DDATE")
        tblData.Columns.Add("MEMBER_ID", GetType(Int32))
        tblData.Columns.Add("GENDER", GetType(Int32))
        tblData.Columns.Add("DECEASED", GetType(Int32))
        tblData.Columns.Add("FATHER")
        tblData.Columns.Add("MOTHER")

        mtblData.DefaultView.Sort = "LEVEL ASC, MEMBER_ID"
        row = mtblData.Select()

        'For Each r As DataRow In row
        For index As Integer = 0 To mtblData.Rows.Count - 1

            With mstSearchData

                'get data at row(i)
                xGetSearchStruct(row(index))

                Dim dtRow As DataRow = tblData.NewRow()

                'image field
                dtRow("GT") = GiaPha.My.Resources.Gender_unknown16
                If .intGender = clsEnum.emGender.MALE Then dtRow("GT") = GiaPha.My.Resources.Gender_man16
                If .intGender = clsEnum.emGender.FEMALE Then dtRow("GT") = GiaPha.My.Resources.Gender_woman16

                'NO field
                i += 1
                dtRow("STT") = basCommon.fncCnvNullToString(index + 1)

                'full name
                dtRow("FULL_NAME") = basCommon.fncGetFullName(.strFirstName, .strMidName, .strLastName, .strAlias)

                'generation is inserted here
                'r("LEVEL") = ""
                If .intLevel > 0 Then dtRow("LEVEL") = .intLevel '.ToString()

                'birth date
                'If .dtBirth > Date.MinValue Then objContent(4) = String.Format(basConst.gcstrDateFormat2, .dtBirth)
                dtRow("BDATE") = basCommon.fncGetDateName("", .intBday, .intBmon, .intByea, True)

                'decease date
                'If .dtDie > Date.MinValue Then objContent(5) = String.Format(basConst.gcstrDateFormat2, .dtDie)
                dtRow("DDATE") = basCommon.fncGetDateName("", .intDday, .intDmon, .intDyea, True)

                dtRow("MEMBER_ID") = .intID
                dtRow("GENDER") = .intGender
                dtRow("DECEASED") = .intDecease

                Dim strFather As String = ""
                Dim strMother As String = ""

                fncGetFaMoName(.intID, strFather, strMother)

                dtRow("FATHER") = strFather
                dtRow("MOTHER") = strMother

                tblData.Rows.Add(dtRow)
            End With
        Next

        DataTableToExcel(dgvMain, tblData, Nothing, lblQuickViewTitle.Text)

        'Dim intCurPage As Integer = mintCurPage
        'Dim intItemPage As Integer = mcintItemPerPage
        'mintCurPage = 1
        'mcintItemPerPage = mtblData.Rows.Count
        'xFillGrid()
        'Dim dgvTemp As DataGridView = CopyDataGridView(dgvMain)
        'mintCurPage = intCurPage
        'mcintItemPerPage = intItemPage
        'xFillGrid()
        'DataGridToExcel(dgvTemp, Nothing, lblQuickViewTitle.Text)
    End Sub

#Region "Add by 2018.04.24 AKB Nguyen Thanh Tung"

    Private Sub xSelectedTypeTree(ByVal emTypeCard As clsEnum.emCardStyle,
                                  Optional blnShortCard As Boolean = False,
                                  Optional ByVal emTypeCardShort As clsEnum.emTypeCardShort = clsEnum.emTypeCardShort.Horizontal)

        My.Settings.intCardStyle = CInt(emTypeCard)

        If emTypeCard = clsEnum.emCardStyle.CARD1 Then

            My.Settings.blnTypeCardShort = blnShortCard

            If blnShortCard Then

                My.Settings.intSelectedTypeCardShort = emTypeCardShort

            End If
        End If

        My.Settings.Save()
        xShowTree()
    End Sub

    Private Sub xShowTree(Optional ByVal blnChangeTree As Boolean = True)

        If blnChangeTree AndAlso (memFormMode = emFormMode.SHOW_TREE_FULL Or memFormMode = emFormMode.SHOW_TREE_COMPACT) Then

            mblnOptChanged = True
            xSetInitGeneration(mblnOptChanged)

        Else

            tsmFamilyShowTree.PerformClick()

        End If
    End Sub

    Private Sub tsbTreeS1_Click(sender As Object, e As EventArgs) Handles tsbTreeS1.Click

        xSelectedTypeTree(clsEnum.emCardStyle.CARD1)

    End Sub

    Private Sub tsbTreeS2_Click(sender As Object, e As EventArgs) Handles tsbTreeS2.Click

        xSelectedTypeTree(clsEnum.emCardStyle.CARD2)

    End Sub

    Private Sub tsbTreeS1Hoz_Click(sender As Object, e As EventArgs) Handles tsbTreeS1Hoz.Click

        xSelectedTypeTree(clsEnum.emCardStyle.CARD1, True, clsEnum.emTypeCardShort.Horizontal)

    End Sub

    Private Sub tsbTreeS1Ver_Click(sender As Object, e As EventArgs) Handles tsbTreeS1Ver.Click

        xSelectedTypeTree(clsEnum.emCardStyle.CARD1, True, clsEnum.emTypeCardShort.Vertical)

    End Sub

    Private Sub tscboShowTypeMember_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tscboShowTypeMember.SelectedIndexChanged

        If tscboShowTypeMember.ComboBox.SelectedIndex < 0 Then Exit Sub
        If Not tscboShowTypeMember.ComboBox.Enabled Then Exit Sub

        My.Settings.intSelectedTypeShowTree = CInt(tscboShowTypeMember.ComboBox.SelectedValue)
        xShowTree()
    End Sub


    Private Function xBingdingComboTypeShowTree() As Boolean

        xBingdingComboTypeShowTree = False

        Dim lstData As New List(Of clsDataSourceComboBox)

        lstData.Add(New clsDataSourceComboBox() With {
            .Display = "Tất cả thành viên",
            .Value = CInt(clsEnum.emTypeShowTree.All)
        })

        lstData.Add(New clsDataSourceComboBox() With {
            .Display = "Chỉ con trai, con gái",
            .Value = CInt(clsEnum.emTypeShowTree.OnlyShowMember)
        })

        lstData.Add(New clsDataSourceComboBox() With {
            .Display = "Chỉ con trai",
            .Value = CInt(clsEnum.emTypeShowTree.OnlyShowMale)
        })

        tscboShowTypeMember.ComboBox.Enabled = False
        basCommon.fncBindingComboBox(tscboShowTypeMember, lstData, My.Settings.intSelectedTypeShowTree)
        tscboShowTypeMember.ComboBox.Enabled = True
    End Function

    Private Function xEnableComboTypeShowTree(Optional ByVal mode As Boolean = True) As Boolean

        tscboShowTypeMember.ComboBox.Enabled = mode

        If mode Then
            tscboShowTypeMember.ComboBox.Enabled = False
            tscboShowTypeMember.ComboBox.SelectedValue = My.Settings.intSelectedTypeShowTree
            tscboShowTypeMember.ComboBox.Enabled = True
        Else
            tscboShowTypeMember.ComboBox.SelectedValue = CInt(clsEnum.emTypeShowTree.All)
        End If

        Return True
    End Function

    Private Sub tsmUpdate_Click(sender As Object, e As EventArgs) Handles tsmUpdate.Click
        If Not basCommon.ComfirmMsg("Hãy sao lữu dữ liệu trước khi cập nhập." & vbNewLine & "Bạn có chắc muốn cập nhập phần mềm?", Me.Text) Then
            Exit Sub
        End If

        gintPercent = 0

        mfrmWaiting = New frmProgress()

        mobjLoadingThread = New System.Threading.Thread(AddressOf UpdateApp)
        mobjLoadingThread.Start()
        mfrmWaiting.ShowDialog()
    End Sub

    Delegate Sub MessageBoxDelegate(ByVal msg As String)

    Private Sub showMessage(ByVal msg As String)
        If Me.InvokeRequired Then
            Me.Invoke(New MessageBoxDelegate(AddressOf showMessage), New Object() {msg})
        Else
            ShowMsg(MessageBoxIcon.Information, msg, Me.Text)
        End If
    End Sub

    Private Sub showMessageErr(ByVal msg As String)
        If Me.InvokeRequired Then
            Me.Invoke(New MessageBoxDelegate(AddressOf showMessageErr), New Object() {msg})
        Else
            ShowMsg(MessageBoxIcon.Error, msg, Me.Text)
        End If
    End Sub

    Private Sub UpdateApp()

        Using wc As New WebClient()

            Dim isSuscess As Boolean = False
            Dim zip As FastZip = Nothing
            Dim pathTemp As String = Application.StartupPath & "\\TempUpdate\\"
            Dim pathFile As String = pathTemp & "temp.zip"
            Dim pathUnZip As String = pathTemp & "UnZip\\"
            Dim pathUpdater As String = pathUnZip & "Updater.exe"
            Dim pathUpdaterPdb As String = pathUnZip & "Updater.pdb"

            Try
                CreateFolder(pathTemp)
                wc.DownloadFile("https://www.akb.com.vn/Giapha_setup/UP_DATE/gpupdate.zip", pathFile)

                If Not File.Exists(pathFile) Then
                    Exit Sub
                End If

                CreateFolder(pathUnZip)
                zip = New FastZip()
                zip.Password = Config.Decrypt(basConst.gcstrZipPassUpdate)
                zip.ExtractZip(pathFile, pathUnZip, "")

                If File.Exists(pathUpdater) Then
                    File.Copy(pathUpdater, Application.StartupPath & "\Updater.exe", True)
                    File.Delete(pathUpdater)
                End If

                If File.Exists(pathUpdaterPdb) Then
                    File.Copy(pathUpdaterPdb, Application.StartupPath & "\Updater.pdb", True)
                    File.Delete(pathUpdaterPdb)
                End If

                isSuscess = True
            Catch ex As Exception
                'MessageBox.Show(ex.Message & vbNewLine & ex.InnerException?.ToString())
            Finally
                zip = Nothing

                gintPercent = 100
                xProgressDone()

                If Not File.Exists(Application.StartupPath & "\\Updater.exe") Then
                    isSuscess = False
                End If

                If isSuscess Then
                    showMessage("Cập nhập thành công. Chương trình sẽ khởi động lại.")
                    Dim p As New Process()
                    p.StartInfo.FileName = Application.StartupPath & "\\Updater.exe"
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    p.Start()
                    Environment.Exit(0)
                Else
                    showMessageErr("Có lỗi trong quá trình xử lí. Vui vòng thử lại sau!")
                End If
            End Try
        End Using
    End Sub

    Private Sub CreateFolder(ByVal path As String)
        basCommon.fncDeleteFolder(path)
        basCommon.fncCreateFolder(path)
    End Sub

    Private Sub tspUpVersion_Click(sender As Object, e As EventArgs) Handles tspUpVersion.Click

        If Not basCommon.UpVersionApp() Then
            Exit Sub
        End If

        tspUpVersion.Visible = basConst.gcintMaxLimit <> basConst.gcintMaxLimitUltimate
    End Sub
#End Region
End Class