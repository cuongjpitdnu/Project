'   ******************************************************************
'      TITLE      : Family Information
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/08/16　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On

Imports System.Collections
Imports System.Collections.Generic
Imports System.IO

'   ******************************************************************
'　　　FUNCTION   : Family Information
'      MEMO       : 
'      CREATE     : 2011/08/16  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmFamilyInfo


#Region "Constants"

    Private Const mcstrClsName As String = "frmFamilyInfo"                              'class name

    Private Const mcstrNoData As String = "Không có dữ liệu hoặc không đọc được dữ liệu."            'message when there's no data or error when reading data
    Private Const mcstrNoFile As String = "Không tìm thấy tệp tin văn bản."                      'message when doc file doesn't exist
    Private Const mcstrErrorOpenImage As String = "Không thể mở chương trình xem ảnh."          'message when can't use image viewer
    Private Const mcstrDelConfirm As String = "Bạn có chắc muốn xóa ảnh khỏi album?"             'delete confirm message
    Private Const mcstrDelFileConfirm As String = "Bạn có chắc muốn xóa văn bản này?"           'delete file confirm
    Private Const mcstrDelFileFail As String = "Xóa file không thành công."                              'delete file fail
    Private Const mcstrFileFilter As String = "Tệp tin tài liệu (*.doc,*.docx,*.pdf)|*.doc;*.docx;*.pdf|Tệp tin ảnh (*.jpg,*.png,*.gif)|*.jpg;*.png;*.gif" & _
                                              "|Tệp tin Media (*.wmv,*.mpeg,*.mpg,*.mp3,*.wav,*.wma,*.avi,*.mov)|*.wmv;*.mpeg;*.mpg;*.mp3;*.wav;*.wma;*.avi;*.mov" & _
                                              "|Tất cả (*.*)|*.*" 'file filter
    Private Const mcstrInvalidImageFile As String = "File được chọn không phải là file ảnh."        'invalid file

    Private Const mcstrSectionName As String = "{0:yyyyMMddhhmmss}"                     'id format
    Private Const mcstrEntryTitle As String = "Title"                                   'entry name
    Private Const mcstrEntryPath As String = "Path"                                     'entry name
    Private Const mcstrEntryContent As String = "Content"                               'entry name

    Private Const mcstrMenuBar As String = "Menu Bar"                                   'menu bar for word control
    Private Const mcstrPicBoxName As String = "{0}"                                     'picture box name

    Private Const mcintMarginRight As Integer = 20                                      'set margin right for remark group box
    Private Const mcintMarginBottom As Integer = 110                                    'set margin bottom for remark group box

    Private Const mcintImageHeigh As Integer = 64                                       'heigh of image
    Private Const mcintImageWidth As Integer = 64                                       'width of image
    Private Const mcintImageLeftMargin As Integer = 10                                  'left margin of image
    Private Const mcintY As Integer = 3                                                 'Y cordinate of image
    Private Const mcintDefaultStep As Integer = 5                                       'default step to show image

    Private Const mcintPicBoxCount As Integer = 7                                       'number of picbox

#End Region


#Region "Variables"

    Private mtblFHead As DataTable                                              'data table of family head member
    Private mtblFatherSon As DataTable                                          'data table of father and son

    Private mintLevel As Integer                                                'level of member
    Private mintCount As Integer                                                'total of image can be shown
    Private mintCurCount As Integer                                             'current total image
    Private mintOffset As Integer                                               'offset for showing image
    Private mintStep As Integer                                                 'step of showing image
    Private mintViewingPic As Integer                                           'viewing image index

    Private mstrDocID As String                                                 'document - section id
    Private mblnAddMode As Boolean                                              'form mode

    Private mstrXmlFile As String                                               'xml file

    Private mlstImages As List(Of String)                                       'list of image path
    Private mlstImageID As List(Of Integer)                                     'list of image id

    Private mlstPicBox As List(Of PictureBox)                                   'array of image box

    Private mstHead As stFamilyHead                                             'structure to store information

    Private mfrmImg As frmImage                                                  'form to show / edit / add new image

    Private mstrFilePath As String
    Private mstrThumbnail As String

#End Region


#Region "Structures"


    '   ******************************************************************
    '　　　FUNCTION   : Family Head Structure
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Structure stFamilyHead

        Dim intMemId As Integer                     'member id

        Dim strLastName As String                   'last name
        Dim strMidName As String                    'middle name
        Dim strFirstName As String                  'first name
        Dim strAlias As String                      'alias

        'Dim dtBirth As Date                         'birth date
        'Dim dtDie As Date                           'die date

        'Dim dtBirth As Date                        'date of birth
        Dim intBday As Integer
        Dim intBmon As Integer
        Dim intByea As Integer

        Dim intDday As Integer
        Dim intDmon As Integer
        Dim intDyea As Integer


        Dim dtStart As Date                         'date that becomes head member

        Dim strRemark As String                     'remark

        Dim intLevel As Integer                     'level

    End Structure


#End Region


#Region "Class events"


    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/08/16  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mtblFHead = Nothing
        mtblFatherSon = Nothing

        mintLevel = 1
        mintCurCount = 0
        mintOffset = 0
        mintStep = 5

        mfrmImg = New frmImage()

        mlstPicBox = New List(Of PictureBox)
        mlstImages = New List(Of String)
        mlstImageID = New List(Of Integer)

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmFamilyInfo_Load, Form load
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmFamilyInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            pnImageList.ContextMenuStrip = mnuContext
            mstrXmlFile = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & basConst.gcstrNoteFolder & basConst.gcstrXmlDoc
            mblnAddMode = True
            mintViewingPic = basConst.gcintNONE_VALUE

            tabFamily.TabPages.Remove(tabAlbum)

            'load Family Head Member's information
            xLoadFamilyHead()

            'load family information
            xLoadFamilyInfo()

            'get image list, exit if there's no image
            'xGetImageList()

            '*********************
            'xLoadImage
            xLoadAlbumImage()
            '*********************

            'load docs list
            xLoadDoc()

            'set image for previewing
            'If mlstImages.Count > 0 Then
            '    picPreview.ImageLocation = mlstImages.Item(0)
            '    mintViewingPic = 0
            'End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmFamilyInfo_Load", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmFamilyInfo_Shown, Form shown
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmFamilyInfo_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown

        Try
            'load images
            xLoadImage()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmFamilyInfo_Shown", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnEditFInfo_Click, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnEditFInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditFInfo.Click

        Dim frmInit As frmFamilyInital = Nothing

        Try
            frmInit = New frmFamilyInital()

            frmInit.fncShowForm(True)

            If frmInit.FamilyUpdated Then xLoadFamilyInfo()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmFamilyInfo_Shown", ex)
        Finally
            If frmInit IsNot Nothing Then frmInit.Dispose()
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : ctrlWords_Load, Winword control shown
    '      MEMO       : 
    '      CREATE     : 2011/11/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub ctrlWords_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            'load doc file
            xLoadDoc()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "ctrlWords_Load", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmFamilyInfo_FormClosing, Form shown
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmFamilyInfo_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        'Try
        '    'save document
        '    If ctrlWords IsNot Nothing Then _
        '        If ctrlWords.document IsNot Nothing Then ctrlWords.document.Save()

        'Catch ex As Exception
        '    basCommon.fncSaveErr(mcstrClsName, "frmFamilyInfo_FormClosing", ex)
        'End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmFamilyInfo_FormClosed, raises after form closed
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmFamilyInfo_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed

        Try
            'clear values
            xDispose()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmFamilyInfo_FormClosing", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmFamilyInfo_Resize, resize windows
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmFamilyInfo_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        Try
            'xResizeWinWord()

            If mlstImages IsNot Nothing Then xLoadImage()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmFamilyInfo_Resize", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnPrevious_Click, resize windows
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnPrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevious.Click

        Try

            xShiftLeft()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnPrevious_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnNext_Click, resize windows
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click

        Try

            xShiftRight()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnNext_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : mniViewPic_Click, menu item clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub mniViewPic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mniViewPic.Click

        Try

            If Not xViewPic(sender, e) Then basCommon.fncMessageError(mcstrErrorOpenImage)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "mniViewPic_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : mniAddPic_Click, menu item clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub mniAddPic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mniAddPic.Click

        Try

            xAddPic()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "mniViewPic_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : mniChangePic_Click, menu item clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub mniChangePic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mniChangePic.Click

        Try

            xChangePic(sender, e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "mniChangePic_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : mniDeletePic_Click, menu item clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub mniDeletePic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mniDeletePic.Click

        Try

            xDeletePic(sender, e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "mniDeletePic_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : mnuContext_Opening, Enable/disable menu item
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub mnuContext_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mnuContext.Opening

        Try
            Dim objControl As Control = Nothing                     'picture box object
            Dim objMenu As ContextMenuStrip = Nothing               'context menu

            'get menu
            objMenu = CType(sender, ContextMenuStrip)

            'get picture box
            objControl = CType(objMenu.SourceControl, Control)

            If objControl Is pnImageList Then
                mniViewPic.Enabled = False
                mniDeletePic.Enabled = False
                mniChangePic.Enabled = False
            Else
                mniViewPic.Enabled = True
                mniDeletePic.Enabled = True
                mniChangePic.Enabled = True
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDeletePic", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnPreviewBack_Click, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnPreviewBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreviewBack.Click

        Try
            'if there is no image
            If mlstImages.Count <= 0 Then
                picPreview.ImageLocation = ""
                Exit Sub
            End If

            If mintViewingPic <= 0 Then Exit Sub

            mintViewingPic -= 1

            'if index is out of bound
            If mintViewingPic > mlstImages.Count - 1 Then mintViewingPic = mlstImages.Count - 1

            picPreview.ImageLocation = mlstImages(mintViewingPic)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnPreviewBack_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnPreviewNext_Click, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnPreviewNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreviewNext.Click

        Try
            'if there is no image
            If mlstImages.Count <= 0 Then
                picPreview.ImageLocation = ""
                Exit Sub
            End If

            If mintViewingPic >= mlstImages.Count - 1 Then Exit Sub

            mintViewingPic += 1
            picPreview.ImageLocation = mlstImages(mintViewingPic)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnPreviewNext_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvDocs_CellDoubleClick, cell double click clicked
    '      MEMO       : 
    '      CREATE     : 2011/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvDocs_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDocs.CellDoubleClick

        Try
            If e.RowIndex < 0 Then Exit Sub

            Dim strFile As String

            strFile = basCommon.fncCnvNullToString(dgvDocs.Item(clmDirectory.Name, e.RowIndex).Value)

            fncOpenAppForFile(strFile)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvDocs_CellDoubleClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvDocs_CellContentClick, cell click clicked
    '      MEMO       : 
    '      CREATE     : 2011/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvDocs_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDocs.CellContentClick

        Try
            Dim strID As String
            Dim strPath As String

            'exit if it is header
            If e.RowIndex < 0 Then Exit Sub

            'exit if it is not button column
            If e.ColumnIndex <> clmDel.Index Then Exit Sub

            'show confirm message
            If Not basCommon.fncMessageConfirm(mcstrDelFileConfirm) Then Exit Sub

            strID = basCommon.fncCnvNullToString(dgvDocs.Item(clmID.Name, e.RowIndex).Value)
            strPath = basCommon.fncCnvNullToString(dgvDocs.Item(clmDirectory.Name, e.RowIndex).Value)

            'try to delete file
            If Not xDelDoc(strID, strPath) Then basCommon.fncMessageWarning(mcstrDelFileFail)


            'reload list
            xLoadDoc()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvDocs_CellContentClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnBrowse_Click, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click

        Try
            If mblnAddMode Then xBrowse()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnBrowse_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnBrowse_Click, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            If mblnAddMode Then
                xAddNewDoc()
            Else
                xSaveDoc()
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnAdd_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvDocs_CellClick, cell click
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvDocs_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDocs.CellClick

        Try
            'exit if it is header
            If e.RowIndex < 0 Then Exit Sub

            mblnAddMode = False

            'fill data from grid to text box
            If Not xLoadDataFromGrid(e.RowIndex) Then

                xClear()
                mblnAddMode = True
                txtTitle.Enabled = True

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvDocs_CellClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnCreate_Click, button click
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click

        Try
            mblnAddMode = True
            'txtFile.Enabled = True

            'clear text box
            xClear()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCreate_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnClear_Click, button click
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click

        Try
            'clear text box
            xClear()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnClear_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnAddNew_Click, button click
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        Try
            If Not xAddPic() Then Exit Sub

            'set image for previewing
            If mlstImages.Count = 1 Then
                picPreview.ImageLocation = mlstImages.Item(0)
                mintViewingPic = 0
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnAddNew_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnEdit_Click, button click
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Try
            Dim intPicID As Integer
            Dim strUrl As String

            If mintViewingPic < 0 Then Exit Sub

            intPicID = mlstImageID(mintViewingPic)

            xEditPic(intPicID)

            'reset viewing image
            strUrl = mlstImages(mintViewingPic)
            picPreview.ImageLocation = strUrl

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnEdit_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnDelete_Click, button click
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Try
            If mintViewingPic < 0 Then Exit Sub

            If Not basCommon.fncMessageConfirm(mcstrDelConfirm) Then Exit Sub

            xDeletePic(mintViewingPic)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDelete_Click", ex)
        End Try

    End Sub

#End Region


#Region "Class methods and functions"


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show this form
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
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
    '　　　FUNCTION   : xLoadFamilyHead, load member information
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadFamilyHead() As Boolean

        xLoadFamilyHead = False

        Try
            'get data from database
            If Not xGetData() Then Exit Function

            'process data, get level for member
            xProcessData()

            'fill gridview
            xFillGrid()

            'show current head member - is the last member
            xShowCurHead()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadFamilyHead", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xLoadFamilyInfo, load family information
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadFamilyInfo() As Boolean

        xLoadFamilyInfo = False

        Try
            Dim strFname As String = ""
            Dim strFanni As String = ""
            Dim strFhome As String = ""

            If Not basCommon.fncGetFamilyInfo(strFname, strFanni, strFhome) Then Exit Function

            lblFamilyName.Text = strFname
            lblFamilyAnni.Text = strFanni
            lblFamilyHometown.Text = strFhome
            lblFamilyInitGeneration.Text = My.Settings.intInitGeneration.ToString()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadFamilyInfo", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xLoadDoc, family history document
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadDoc() As Boolean

        xLoadDoc = False

        Dim objXml As AMS.Profile.Xml = Nothing
        Dim objContent(4) As Object

        Try
            Dim intStt As Integer = 0

            mblnAddMode = True

            'clear grid
            dgvDocs.Rows.Clear()

            If Not System.IO.File.Exists(mstrXmlFile) Then Exit Function

            'xml object
            objXml = New AMS.Profile.Xml(mstrXmlFile)

            For Each strId As String In objXml.GetSectionNames

                'clear array
                Array.Clear(objContent, 0, objContent.Length)

                'stt
                intStt += 1
                objContent(0) = intStt.ToString()

                'title
                objContent(1) = basCommon.fncCnvNullToString(objXml.GetValue(strId, mcstrEntryTitle))

                'content
                objContent(2) = basCommon.fncCnvNullToString(objXml.GetValue(strId, mcstrEntryContent))

                'id
                objContent(3) = strId

                'path
                objContent(4) = basCommon.fncCnvNullToString(objXml.GetValue(strId, mcstrEntryPath))

                dgvDocs.Rows.Add(objContent)

            Next

            If dgvDocs.Rows.Count = 0 Then
                mblnAddMode = True
                xClear()
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadDoc", ex)
        Finally
            Erase objContent
            objXml = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetData, get data from database
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetData() As Boolean

        xGetData = False

        Try
            'get family head member
            mtblFHead = gobjDB.fncGetFHead()

            'if there is no data
            If mtblFHead Is Nothing Then Exit Function

            'get father and son table if there is more than 1 head member
            mtblFatherSon = gobjDB.fncGetFatherSon()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetData", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xProcessData, process data
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xProcessData() As Boolean

        xProcessData = False

        Dim vwData As DataView = Nothing                    'dataview to filter

        Try
            Dim intMemID As Integer = -1                    'member id

            'process data, get level of member
            'if there is only 1 head member -> set level and exit
            'If mtblFHead.Rows.Count = 1 Then Exit Function

            'if there is more than 1 -> create dataview
            vwData = New DataView(mtblFatherSon)

            'set member level in family head table
            For Each row As DataRow In mtblFHead.Rows

                'get member id
                Integer.TryParse(basCommon.fncCnvNullToString(row.Item("MEMBER_ID")), intMemID)

                'reset and get new level
                mintLevel = 1
                xGetLevel(intMemID, vwData)

                'set cell value
                row.Item(basConst.gcstrFieldLevel) = mintLevel

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xProcessData", ex)
        Finally
            If vwData IsNot Nothing Then vwData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillGrid, fill data grid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillGrid() As Boolean

        xFillGrid = False
        Dim strContent(6) As String

        Try
            'fill content to data grid from datatable
            For i As Integer = 0 To mtblFHead.Rows.Count - 1

                With mstHead

                    'reset array
                    Array.Clear(strContent, 0, strContent.Length)

                    'get data to struc
                    If Not xGetStHead(i) Then Continue For

                    'No field
                    strContent(0) = basCommon.fncCnvNullToString(i + 1)

                    'full name
                    strContent(1) = basCommon.fncGetFullName(.strFirstName, .strMidName, .strLastName, .strAlias)

                    'birth date
                    'If .dtBirth > Date.MinValue Then strContent(2) = String.Format(basConst.gcstrDateFormat2, .dtBirth)
                    strContent(2) = basCommon.fncGetDateName("", .intBday, .intBmon, .intByea, True)

                    'start date
                    If .dtStart > Date.MinValue Then strContent(3) = String.Format(basConst.gcstrDateFormat2, .dtStart)

                    'end date is die date
                    'If .dtDie > Date.MinValue Then strContent(4) = String.Format(basConst.gcstrDateFormat2, .dtDie)
                    strContent(4) = basCommon.fncGetDateName("", .intDday, .intDmon, .intDyea, True, True)

                    'remark
                    strContent(5) = basCommon.fncCnvRtfToText(.strRemark)

                End With

                'add new row
                dgvFamilyHead.Rows.Add(strContent)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillGrid", ex)
        Finally
            Erase strContent
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xShowCurHead, show current head member
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xShowCurHead() As Boolean

        xShowCurHead = False

        Try
            Dim strName As String

            'format name
            strName = String.Format(basConst.gcstrNameFormat, mstHead.strLastName, mstHead.strMidName, mstHead.strFirstName)
            strName = fncRemove2Space(strName)

            'name
            lblHeadName.Text = strName

            'level
            lblHeadLevel.Text = (mstHead.intLevel + My.Settings.intInitGeneration - 1).ToString()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowCurHead", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetStHead, get data at row
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRow  Integer, row to read data
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetStHead(ByVal intRow As Integer) As Boolean

        xGetStHead = False

        Dim vwData As DataView = Nothing                     'dataview to sort data

        Try

            'sorting table of family head by Level
            vwData = New DataView(mtblFHead)
            vwData.Sort = basConst.gcstrFieldLevel

            'reset structure
            mstHead = Nothing

            'get data at row
            With vwData(intRow)

                'member id
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("MEMBER_ID")), mstHead.intMemId)

                'full name
                mstHead.strLastName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))
                mstHead.strMidName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                mstHead.strFirstName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))
                mstHead.strAlias = basCommon.fncCnvNullToString(.Item("ALIAS_NAME"))

                'birth and die date
                'Date.TryParse(basCommon.fncCnvNullToString(.Item("BIRTH_DAY")), mstHead.dtBirth)
                'Date.TryParse(basCommon.fncCnvNullToString(.Item("DECEASED_DATE")), mstHead.dtDie)

                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY")), mstHead.intBday)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON")), mstHead.intBmon)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA")), mstHead.intByea)

                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_DAY")), mstHead.intDday)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_MON")), mstHead.intDmon)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_YEA")), mstHead.intDyea)

                'get start date - the die date of person forward
                If intRow > 0 Then Date.TryParse(basCommon.fncCnvNullToString(vwData(intRow - 1).Item("DECEASED_DATE")), mstHead.dtStart)

                'remark
                mstHead.strRemark = basCommon.fncCnvNullToString(.Item("REMARK"))

                'level
                Integer.TryParse(basCommon.fncCnvNullToString(.Item(basConst.gcstrFieldLevel)), mstHead.intLevel)

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetStHead", ex)
        Finally
            If vwData IsNot Nothing Then vwData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetLevel, get level of member
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intMemID  Integer, member id to read
    '                 : vwData  DataView, filter
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetLevel(ByVal intMemID As Integer, ByVal vwData As DataView) As Boolean

        xGetLevel = False

        Try
            Dim intFather As Integer                'father id of inputed member

            'create filter to find father of input member
            vwData.RowFilter = String.Format(gcstrRowFilterFormat, basConst.gcstrFieldSon, intMemID)

            'if member has father
            If vwData.Count > 0 Then

                'increase level of member by 1
                mintLevel += 1

                'try to get his father
                Integer.TryParse(basCommon.fncCnvNullToString(vwData(0)(basConst.gcstrFieldFather)), intFather)

                'do recursive to find father of father
                xGetLevel(intFather, vwData)

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetLevel", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetImage, set image to picturebox control
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetImage() As Boolean

        xSetImage = False

        Try

            For i As Integer = 0 To mlstPicBox.Count - 1

                If mlstPicBox.Item(i).Image IsNot Nothing Then mlstPicBox.Item(i).Image.Dispose()
                mlstPicBox.Item(i).Image = basCommon.fncCreateThumbnail(mlstImages.Item(mintOffset + i), mcintImageWidth, mcintImageHeigh, CInt(clsEnum.emGender.FEMALE))

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetImage", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xLoadImage, load picbox control and set image
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadImage() As Boolean

        xLoadImage = False

        Try
            Dim strName As String = String.Empty

            'exit if there is no image data
            If mlstImages.Count <= 0 Then Exit Function

            'calculate how many images can be shown
            xCalculate()

            'do nothing if there is not enough space for at least 1 image
            If mintCount = 0 Then Exit Function

            'exit if number of picbox doesn't change
            If mintCount = mintCurCount Then Exit Function

            'change step
            mintStep = mcintDefaultStep
            If mintStep > mintCount Then mintStep = mintCount

            'add or remove image
            xAddImage()

            'set current number of image showing
            mintCurCount = mintCount

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadImage", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddImage, add or remove image
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddImage() As Boolean
        xAddImage = False

        Try
            'if number of images is less than number of picbox
            If mintCount > mlstImages.Count Then mintCount = mlstImages.Count

            If mintCount > mintCurCount Then

                'add if new count is greater than current count
                For i As Integer = mintCurCount To mintCount - 1

                    xAddImgAt(i)

                Next

                'if picture reaches to the end, decrease offset - use when window is resizing and number of picbox changes
                If (mintCount + mintOffset > mlstImages.Count) Then

                    mintOffset -= mintCount - mintCurCount

                    If mintOffset < 0 Then mintOffset = 0

                End If

                'reset image
                xSetImage()

            Else

                'remove if new count is smaller than current count
                For i As Integer = mintCurCount - 1 To mintCount Step -1
                    xRemoveImgAt(i)
                    'update number of picbox being shown
                    mintCurCount -= 1
                Next

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddImage", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xShiftLeft, shift left image
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xShiftLeft() As Boolean

        xShiftLeft = False

        Try
            'do nothing if offset is 0
            If mintOffset = 0 Then Exit Function

            'if one more step is less than 0
            If mintOffset - mintStep < 0 Then

                mintOffset = 0

            Else

                'decrease offset value by a step
                mintOffset -= mintStep

            End If

            'reset image
            xSetImage()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShiftLeft", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xShiftRight, shift right image
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xShiftRight() As Boolean

        xShiftRight = False

        Try

            'exit if offset reach to maximum
            If mlstImages.Count = mlstPicBox.Count + mintOffset Then Exit Function

            'if one more step is greater than the range of image
            If mintOffset + mlstPicBox.Count + mintStep > mlstImages.Count Then

                mintOffset += mlstImages.Count - (mlstPicBox.Count + mintOffset)

            Else

                'increase offset value by a step
                mintOffset += mintStep


            End If

            'reset image
            xSetImage()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShiftRight", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddImgAt, add image at position
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddImgAt(ByVal intIndex As Integer) As Boolean

        xAddImgAt = False

        Try
            Dim objPicBox As New PictureBox()
            Dim strName As String
            Dim intX As Integer = mcintImageLeftMargin

            strName = String.Format(mcstrPicBoxName, intIndex)

            'name of picture box and size mode
            objPicBox.Name = strName
            objPicBox.SizeMode = PictureBoxSizeMode.StretchImage

            'size
            objPicBox.Width = mcintImageWidth
            objPicBox.Height = mcintImageHeigh

            'location
            intX += intIndex * (mcintImageLeftMargin + mcintImageWidth)
            objPicBox.Location = New System.Drawing.Point(intX, mcintY)

            'context menu
            objPicBox.ContextMenuStrip = mnuContext

            'cursor
            objPicBox.Cursor = Cursors.Hand

            'handler click event
            AddHandler objPicBox.Click, AddressOf mniViewPic_Click

            'add to list
            mlstPicBox.Add(objPicBox)

            'add to panel
            pnImageList.Controls.Add(objPicBox)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddImgAt", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xRemoveImgAt, remove image at position
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xRemoveImgAt(ByVal intIndex As Integer) As Boolean

        xRemoveImgAt = False

        Try
            'remove from panel
            pnImageList.Controls.Remove(mlstPicBox.Item(intIndex))

            'remove from list
            mlstPicBox.RemoveAt(intIndex)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRemoveImgAt", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xCalculate, calculate number of picbox can show
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCalculate() As Boolean

        xCalculate = False

        Try
            'get number of image can be shown
            'mintCount = pnImageList.Width \ (mcintImageWidth + mcintImageLeftMargin)

            mintCount = mcintPicBoxCount

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowImages", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetImageList, get list of image
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetImageList() As Boolean

        xGetImageList = False
        Dim tblData As DataTable = Nothing

        Try

            Dim strPath As String
            Dim intID As Integer = -1

            'get table of image list
            tblData = gobjDB.fncGetFAlbum()

            'exit if there's no image
            If tblData Is Nothing Then Exit Function

            'clear data
            mlstImageID.Clear()
            mlstImages.Clear()

            'set URL value
            For i As Integer = 0 To tblData.Rows.Count - 1
                'path to image
                strPath = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAlbumPath
                strPath &= basCommon.fncCnvNullToString(tblData.Rows(i).Item("IMAGE_NAME"))
                mlstImages.Add(strPath)

                'image id
                Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i).Item("IMAGE_ID")), intID)
                mlstImageID.Add(intID)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetImageList", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xResizeWinWord, resize win word
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/19  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    'Private Function xResizeWinWord() As Boolean

    '    xResizeWinWord = False

    '    Try
    '        'x y cordinate
    '        Dim x As Integer
    '        Dim y As Integer

    '        'calculate x y 
    '        x = Me.Width - (gbRemark.Location.X + gbRemark.Width)
    '        y = Me.Height - (gbRemark.Location.Y + gbRemark.Height)

    '        gbRemark.Width = gbRemark.Width + (x - mcintMarginRight)
    '        gbRemark.Height = gbRemark.Height + (y - mcintMarginBottom)

    '        Return True

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "xResizeWinWord", ex)
    '    End Try

    'End Function


    '   ******************************************************************
    '　　　FUNCTION   : xViewPic, view picture
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : sender  System.Object
    '      PARAMS2    : e  System.EventArgs
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xViewPic(ByVal sender As System.Object, ByVal e As System.EventArgs) As Boolean

        xViewPic = False

        Try
            Dim strUrl As String                                    'path of image
            Dim objPicBox As PictureBox = Nothing                   'picture box object
            Dim objMenuItem As ToolStripMenuItem = Nothing          'menu item
            Dim objMenu As ContextMenuStrip = Nothing               'context menu


            If sender Is mniViewPic Then
                'Called from Menu item
                'get menu item
                objMenuItem = CType(sender, ToolStripMenuItem)

                'get menu
                objMenu = CType(objMenuItem.Owner, ContextMenuStrip)

                'get picture box
                objPicBox = CType(objMenu.SourceControl, PictureBox)

            Else
                'Called from picture box
                objPicBox = CType(sender, PictureBox)

            End If

            'image path
            strUrl = mlstImages(mlstPicBox.IndexOf(objPicBox) + mintOffset)

            ''open view form
            'mfrmImg.FormMode = clsEnum.emMode.VIEW
            'mfrmImg.ImageID = mlstImageID(mlstPicBox.IndexOf(objPicBox) + mintOffset)
            'mfrmImg.fncShowForm()

            picPreview.ImageLocation = strUrl
            mintViewingPic = mlstPicBox.IndexOf(objPicBox) + mintOffset

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xViewPic", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddPic, add picture
    '      VALUE      : Boolean, true - success, false - failure
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddPic() As Boolean

        xAddPic = False

        Try
            Dim strImage As String = ""
            'show add form
            'mfrmImg.FormMode = clsEnum.emMode.ADD

            'close if user doesn't choose any file
            If Not basCommon.fncOpenFileDlg(strImage, basConst.gcstrImageFilter) Then Exit Function

            If Not (strImage.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) Or strImage.EndsWith(".png", StringComparison.OrdinalIgnoreCase) Or strImage.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) Or strImage.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)) Then

                basCommon.fncMessageWarning(mcstrInvalidImageFile)
                Exit Function

            End If

            If mfrmImg.fncShowForm(clsEnum.emMode.ADD, strImage) Then

                're-calculate
                xCalculate()

                'get list of image
                xGetImageList()

                'rebuild
                xLoadImage()

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddPic", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xChangePic, change picture
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : sender  System.Object
    '      PARAMS2    : e  System.EventArgs
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xChangePic(ByVal sender As System.Object, ByVal e As System.EventArgs) As Boolean

        xChangePic = False

        Try
            'Dim strUrl As String                                    'path of image
            Dim objPicBox As PictureBox = Nothing                   'picture box object
            Dim objMenuItem As ToolStripMenuItem = Nothing          'menu item
            Dim objMenu As ContextMenuStrip = Nothing               'context menu
            Dim intPicID As Integer

            'get menu item
            objMenuItem = CType(sender, ToolStripMenuItem)

            'get menu
            objMenu = CType(objMenuItem.Owner, ContextMenuStrip)

            'get picture box
            objPicBox = CType(objMenu.SourceControl, PictureBox)

            'image path
            'strUrl = mlstImages(mlstPicBox.IndexOf(objPicBox) + mintOffset)

            'show form
            'mfrmImg.FormMode = clsEnum.emMode.EDIT
            'mfrmImg.ImageID = mlstImageID(mlstPicBox.IndexOf(objPicBox) + mintOffset)

            'If mfrmImg.fncShowForm(clsEnum.emMode.EDIT) Then

            '    'get list of image
            '    xGetImageList()
            '    xSetImage()

            'End If

            intPicID = mlstImageID(mlstPicBox.IndexOf(objPicBox) + mintOffset)

            If Not xEditPic(intPicID) Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xChangePic", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDeletePic, delete picture
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : sender  System.Object
    '      PARAMS2    : e  System.EventArgs
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDeletePic(ByVal sender As System.Object, ByVal e As System.EventArgs) As Boolean

        xDeletePic = False

        Try
            Dim strIndex As String
            Dim intIndex As Integer
            Dim objPicBox As PictureBox = Nothing                   'picture box object
            Dim objMenuItem As ToolStripMenuItem = Nothing          'menu item
            Dim objMenu As ContextMenuStrip = Nothing               'context menu

            If Not basCommon.fncMessageConfirm(mcstrDelConfirm) Then Exit Function

            'get menu item
            objMenuItem = CType(sender, ToolStripMenuItem)

            'get menu
            objMenu = CType(objMenuItem.Owner, ContextMenuStrip)

            'get picture box
            objPicBox = CType(objMenu.SourceControl, PictureBox)

            'get name of picture - also is index
            strIndex = objPicBox.Name
            Integer.TryParse(strIndex, intIndex)

            'index to delete is index of picbox + offset
            intIndex += mintOffset

            ''delete from harddisk
            'If Not basCommon.fncDeleteFile(mlstImages(intIndex)) Then Exit Function

            ''delete from database
            'gobjDB.fncDelAlbum(mlstImageID(intIndex))

            ''remove from list
            'mlstImages.RemoveAt(intIndex)
            'mlstImageID.RemoveAt(intIndex)

            'If mlstImages.Count < mintCount Then

            '    'reset current count and reload image
            '    mintCurCount = mlstImages.Count + 1
            '    xAddImage()
            '    xSetImage()

            'Else

            '    'shift left if image reach at the end
            '    If mlstPicBox.Count + mintOffset = mlstImages.Count + 1 Then

            '        xShiftLeft()

            '    Else

            '        xSetImage()

            '    End If

            'End If

            If Not xDeletePic(intIndex) Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDeletePic", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xBrowse, browse file
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xBrowse() As Boolean

        xBrowse = False

        Try
            Dim strPath As String = ""

            If Not basCommon.fncOpenFileDlg(strPath, mcstrFileFilter) Then Exit Function

            txtFile.Text = strPath

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xBrowse", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddNewDoc, add new document
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddNewDoc() As Boolean

        xAddNewDoc = False

        Dim objXmlWriter As AMS.Profile.Xml = Nothing

        Try
            Dim strFile As String
            Dim strDesFolder As String

            Dim strID As String
            Dim strTitle As String
            Dim strContent As String

            'get file and destination folder
            strFile = txtFile.Text.Trim()
            strTitle = txtTitle.Text.Trim()
            strContent = txtContent.Text.Trim()
            strDesFolder = My.Application.Info.DirectoryPath & (basConst.gcstrDocsFolder & basConst.gcstrNoteFolder).Replace("\\", "\")

            If basCommon.fncIsBlank(strTitle) Then
                txtTitle.Focus()
                Exit Function
            End If

            'copy
            If Not basCommon.fncCopyFile(strFile, strDesFolder, False) Then
                txtFile.Focus()
                Exit Function
            End If

            strID = String.Format(mcstrSectionName, DateTime.Now)

            'add to xml file
            objXmlWriter = New AMS.Profile.Xml(mstrXmlFile)
            objXmlWriter.SetValue(strID, mcstrEntryTitle, strTitle)
            objXmlWriter.SetValue(strID, mcstrEntryContent, strContent)
            objXmlWriter.SetValue(strID, mcstrEntryPath, strDesFolder)

            mblnAddMode = True

            xClear()

            're-load doc list
            xLoadDoc()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddNewDoc", ex)
        Finally
            objXmlWriter = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSaveDoc, save document
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSaveDoc() As Boolean

        xSaveDoc = False

        Dim objXmlWriter As AMS.Profile.Xml = Nothing

        Try
            Dim strID As String
            Dim strTitle As String
            Dim strContent As String

            'get file and destination folder
            strTitle = txtTitle.Text.Trim()
            strContent = txtContent.Text.Trim()

            If basCommon.fncIsBlank(strTitle) Then
                txtTitle.Focus()
                Exit Function
            End If

            strID = mstrDocID

            'add to xml file
            objXmlWriter = New AMS.Profile.Xml(mstrXmlFile)
            objXmlWriter.SetValue(strID, mcstrEntryTitle, strTitle)
            objXmlWriter.SetValue(strID, mcstrEntryContent, strContent)

            mblnAddMode = True
            xClear()

            're-load doc list
            xLoadDoc()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSaveDoc", ex)
        Finally
            objXmlWriter = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDelDoc, delete document
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : strID   String, xml section
    '      PARAMS     : strPath   String, file path
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDelDoc(ByVal strID As String, ByVal strPath As String) As Boolean

        xDelDoc = False

        Dim objXml As AMS.Profile.Xml = Nothing

        Try
            'try to delete
            Try

                fncDeleteFile(strPath)

                'delete from xml
                objXml = New AMS.Profile.Xml(mstrXmlFile)
                objXml.RemoveSection(strID)

            Catch ex As Exception
                Exit Function
            End Try

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDelDoc", ex)
        Finally
            objXml = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDispose, clear variable
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDispose() As Boolean

        xDispose = False

        Try
            ''clear winword values
            'ctrlWords.RestoreCommandBars()
            'ctrlWords.CloseControl()
            'ctrlWords.Dispose()
            'ctrlWords = Nothing

            If mtblFatherSon IsNot Nothing Then mtblFatherSon.Dispose()
            If mtblFHead IsNot Nothing Then mtblFHead.Dispose()

            mlstImageID.Clear()
            mlstImages.Clear()

            For i As Integer = 0 To mlstPicBox.Count - 1

                If mlstPicBox(i) IsNot Nothing Then mlstPicBox(i).Dispose()

            Next

            mlstPicBox.Clear()

            mfrmImg.Dispose()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDispose", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xLoadDataFromGrid, load data from xml to grid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRowID    Integer, row id
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadDataFromGrid(ByVal intRowID As Integer) As Boolean

        xLoadDataFromGrid = False

        Try
            mstrDocID = basCommon.fncCnvNullToString(dgvDocs.Item(clmID.Name, intRowID).Value)

            txtContent.Text = basCommon.fncCnvNullToString(dgvDocs.Item(clmContent.Name, intRowID).Value)
            txtFile.Text = basCommon.fncCnvNullToString(dgvDocs.Item(clmDirectory.Name, intRowID).Value)
            txtTitle.Text = basCommon.fncCnvNullToString(dgvDocs.Item(clmTitle.Name, intRowID).Value)

            txtFile.Enabled = False

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCreate_Click", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xClear, clear text box
    '      PARAMS     :
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xClear()

        Try
            txtContent.Clear()
            txtTitle.Clear()

            If mblnAddMode Then
                txtFile.Clear()
                txtFile.Focus()
            Else
                txtTitle.Focus()
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClear", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xEditPic, change picture
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intPicID    Integer, picture id
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xEditPic(ByVal intPicID As Integer) As Boolean

        xEditPic = False

        Try
            mfrmImg.ImageID = intPicID

            If mfrmImg.fncShowForm(clsEnum.emMode.EDIT) Then

                'get list of image
                xGetImageList()
                xSetImage()

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xEditPic", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDeletePic, delete picture
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intIndex    Integer, picture index
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDeletePic(ByVal intIndex As Integer) As Boolean

        xDeletePic = False

        Try
            'delete from harddisk
            If Not basCommon.fncDeleteFile(mlstImages(intIndex)) Then Exit Function

            'delete from database
            gobjDB.fncDelAlbum(mlstImageID(intIndex))

            'remove from list
            mlstImages.RemoveAt(intIndex)
            mlstImageID.RemoveAt(intIndex)

            If mlstImages.Count < mintCount Then

                'reset current count and reload image
                mintCurCount = mlstImages.Count + 1
                xAddImage()
                xSetImage()

            Else

                'shift left if image reach at the end
                If mlstPicBox.Count + mintOffset = mlstImages.Count + 1 Then

                    xShiftLeft()

                Else

                    xSetImage()

                End If

            End If

            xSetPreviewImage()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDeletePic", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetPreviewImage, set image for previewing
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     :
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetPreviewImage() As Boolean

        xSetPreviewImage = False

        Try
            Dim intId As Integer

            'set image for previewing
            If mlstImages.Count <= 0 Then

                mintViewingPic = basConst.gcintNONE_VALUE
                picPreview.ImageLocation = ""
                Exit Function

            Else
                'increase viewing image by 1
                intId = mintViewingPic

                intId -= 1

                'if index smaller than 0, we use right image
                If intId < 0 Then

                    intId = mintViewingPic

                    'if right image is out of bound, we use current index
                    If intId > mlstImageID.Count - 1 Then

                        picPreview.ImageLocation = ""
                        mintViewingPic = basConst.gcintNONE_VALUE
                        Exit Function

                    Else
                        intId = mintViewingPic
                    End If

                End If


                picPreview.ImageLocation = mlstImages.Item(intId)

                mintViewingPic = intId

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetPreviewImage", ex)
        End Try

    End Function


#End Region


#Region "Family Album"
    Private Sub PictureDoubleClick()
    End Sub

    '**************************************
    'chk "Chọn tất cả" event click
    '**************************************
    Private Sub chkCheckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCheckAll.CheckedChanged
        Try
            xCheckOrNoCheck(chkCheckAll.Checked)
        Catch ex As Exception
            MessageBox.Show("chkCheckAll_CheckedChanged", ex.Message)
        End Try
    End Sub

    '**************************************
    'btnAddImage event click 
    '**************************************
    Private Sub btnAddImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddImage.Click
        Try
            Dim objViewImage As frmViewImage = New frmViewImage()
            objViewImage.mstrFilePath = mstrFilePath
            objViewImage.mblnFamilyAlbum = True
            objViewImage.ShowDialog()

            If Not xLoadAlbumImage() Then Exit Sub
        Catch ex As Exception
            MessageBox.Show("btnAddImage_Click", ex.Message)
        End Try

    End Sub

    '**************************************
    'btnSaveToFile event click 
    '**************************************
    Private Sub btnSaveToFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveToFile.Click
        Dim ctl As usrPic
        Dim loadedImage As Image = Nothing
        Dim objBrow As New FolderBrowserDialog
        Dim strNewFilePath As String = ""
        Dim strImageFile As String = ""
        Dim blnSaveFile As Boolean = False

        Try
            If (objBrow.ShowDialog() = Windows.Forms.DialogResult.OK) Then

                For Each ctl In flpanelAlbum.Controls
                    If ctl.chkSelect.Checked = True Then
                        strImageFile = ctl.Path

                        loadedImage = Image.FromFile(strImageFile)
                        strNewFilePath = objBrow.SelectedPath & strImageFile.Substring(strImageFile.LastIndexOf("\"))

                        If System.IO.File.Exists(strNewFilePath) = False Then
                            loadedImage.Save(strNewFilePath)
                        End If
                        blnSaveFile = True
                    End If
                Next

                If blnSaveFile = True Then
                    MessageBox.Show("Bạn đã lưu ảnh vào thư mục thành công!")
                Else
                    MessageBox.Show("Bạn đã không chọn ảnh để lưu!")
                End If


            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSaveToFile_Click", ex)
        End Try

    End Sub

    '**************************************
    'Load all image in folder function
    '**************************************
    Private Function xLoadAlbumImage() As Boolean
        xLoadAlbumImage = False
        Try
            lblFName.Text = lblFamilyName.Text.ToUpper()
            Dim strPath As String = Application.StartupPath
            strPath = strPath & "\" & gcstrMemberImageFolder
            strPath = strPath & "\" & gcstrAlbumPath

            Dim strThumbnailPath As String = strPath & "\Thumbnail"

            'Create root album for family
            If Not System.IO.Directory.Exists(strPath) Then
                System.IO.Directory.CreateDirectory(strPath)
            End If

            'Create tumbnail album for family
            If Not System.IO.Directory.Exists(strThumbnailPath) Then
                System.IO.Directory.CreateDirectory(strThumbnailPath)
            End If

            mstrFilePath = strPath
            mstrThumbnail = strThumbnailPath
            flpanelAlbum.Controls.Clear()

            Dim di As New DirectoryInfo(strPath)
            di.Refresh()

            Dim diThumbnail As New DirectoryInfo(strPath)
            diThumbnail.Refresh()

            gobjPic = New List(Of usrPic)
            For Each fi As FileInfo In di.GetFiles("*.*")
                If fi.Extension.ToLower() = gcstrFileGIF Or fi.Extension.ToLower() = gcstrFileJPG Or fi.Extension.ToLower() = gcstrFilePNG Or fi.Extension.ToLower() = gcstrFileBMP Then
                    Dim uCtl As New usrPic()
                    uCtl.ImageLocation = strThumbnailPath & "\" & fi.Name
                    uCtl.objfrmAlbum = Me
                    uCtl.Path = strPath & fi.Name
                    uCtl.mblnFamily = True
                    AddHandler uCtl.MeDoubleClick, AddressOf PictureDoubleClick
                    flpanelAlbum.Controls.Add(uCtl)
                    gobjPic.Add(uCtl)
                End If
            Next

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadImage", ex)
        End Try

    End Function

    '**************************************
    'btnDelImage event click 
    '**************************************
    Private Sub btnDelImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelImage.Click
        Dim ctl As Control
        Dim ArrUCtl() As UserControl = Nothing
        Dim StrArrFileName() As String = Nothing
        Dim i As Integer = 0
        Dim blnChecked As Boolean = False

        Try

            For Each ctl In flpanelAlbum.Controls

                If TypeOf (ctl) Is UserControl Then
                    ReDim Preserve ArrUCtl(i)
                    ReDim Preserve StrArrFileName(i)

                    Dim tbControl As usrPic = DirectCast(ctl, usrPic)
                    If tbControl.chkSelect.Checked = True Then
                        blnChecked = True
                        ArrUCtl(i) = CType(ctl, UserControl)
                        StrArrFileName(i) = tbControl.PicContent.ImageLocation.Substring(tbControl.PicContent.ImageLocation.LastIndexOf("\") + 1)
                    End If

                End If
                i = i + 1
            Next

            'Remove control
            If ArrUCtl Is Nothing Then Exit Sub

            If blnChecked Then
                If Not basCommon.fncMessageConfirm("Ảnh sẽ bị xóa. Bạn có chắc chắn?") Then Exit Sub
            End If

            If ArrUCtl.Length > 0 Then
                For i = 0 To ArrUCtl.Length - 1
                    If ArrUCtl(i) Is Nothing Then Continue For
                    flpanelAlbum.Controls.Remove(ArrUCtl(i))
                    gobjPic.Remove(CType(ArrUCtl(i), usrPic))
                Next
            End If

            'Delete image in file
            If StrArrFileName Is Nothing Then Exit Sub
            If StrArrFileName.Length > 0 Then
                If Not xDelFileImage(mstrFilePath, StrArrFileName) Then Exit Sub
            End If

        Catch ex As Exception
            'Exception
            MessageBox.Show("btnDel_Click" & ex.Message)
        Finally
            Erase ArrUCtl
            Erase StrArrFileName
        End Try
    End Sub

    '**************************************
    'Delete image in folder
    '**************************************
    Private Function xDelFileImage(ByVal vstrPath As String, _
                                   ByVal vArrStrFile() As String) As Boolean

        xDelFileImage = False

        Try
            Dim strTemp As String
            Dim strTempThumbnail As String

            If vstrPath = "" Or vArrStrFile Is Nothing Then Exit Function

            If vArrStrFile.Length > 0 Then

                For i As Integer = 0 To vArrStrFile.Length - 1

                    'Delete in folder
                    strTemp = mstrFilePath & vArrStrFile(i)
                    If System.IO.File.Exists(strTemp) Then
                        System.IO.File.Delete(strTemp)
                    End If

                    'Delete in thumbnail
                    strTempThumbnail = mstrThumbnail & "\" & vArrStrFile(i)
                    If System.IO.File.Exists(strTempThumbnail) Then
                        System.IO.File.Delete(strTempThumbnail)
                    End If

                Next

            End If

            Return True

        Catch ex As Exception
            MessageBox.Show("xDelFileImage", ex.Message)
        End Try
    End Function

    '**************************************
    'Check all or un check all image
    '**************************************
    Private Sub xCheckOrNoCheck(Optional ByVal vblnCheck As Boolean = False)
        Dim ctl As usrPic

        Try
            For Each ctl In flpanelAlbum.Controls
                ctl.chkSelect.Checked = vblnCheck
            Next
        Catch ex As Exception
            MessageBox.Show("xCheckOrNoCheck", ex.Message)
        End Try

    End Sub
#End Region


End Class
