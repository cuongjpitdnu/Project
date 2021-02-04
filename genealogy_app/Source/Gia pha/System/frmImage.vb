'   ******************************************************************
'      TITLE      : Family Image
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/08/16　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : Family Image
'      MEMO       : 
'      CREATE     : 2011/08/16  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmImage

#Region "Constants"

    Private Const mcstrClsName As String = "frmImage"                              'class name

    Private Const mcstrSaveError As String = "Lỗi trong quá trình lưu dữ liệu."               'error message when saving or updating

#End Region


#Region "Variables"

    Private mintMode As clsEnum.emMode                                          'form mode ADD / EDIT / VIEW

    Private mstrImageLocation As String                                         'image location

    Private mstImageInfor As clsDbAccess.stAlbum                                'structure

#End Region


#Region "Properties"

    '   ******************************************************************
    '　　　FUNCTION   : ImageID Property, Image ID
    '      MEMO       : 
    '      CREATE     : 2011/07/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property ImageID() As Integer

        Get
            Return mstImageInfor.intID
        End Get

        Set(ByVal value As Integer)
            mstImageInfor.intID = value
        End Set

    End Property


#End Region


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mintMode = clsEnum.emMode.EDIT

    End Sub


#Region "Class events"


    '   ******************************************************************
    '　　　FUNCTION   : frmImage_Load, Form Load
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmImage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            xFormLoad()

        Catch ex As Exception

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnSave_Click, Save button click
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            Dim btnSuccess As Boolean = False

            Select Case mintMode

                Case clsEnum.emMode.ADD
                    'try to save
                    btnSuccess = xSaveImage()


                Case clsEnum.emMode.EDIT
                    'try to save
                    btnSuccess = xUpdateImage()

            End Select

            If Not btnSuccess Then

                basCommon.fncMessageError(mcstrSaveError, btnSave)
                Exit Sub

            End If

            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnChooseImg_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnChooseImg_Click, change image clicked
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnChooseImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseImg.Click

        Try

            xOpenFile()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnChooseImg_Click", ex)
        End Try

    End Sub


#End Region


#Region "Class methods and functions"


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show this form
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm(ByVal emMode As clsEnum.emMode, Optional ByVal strImageLocation As String = "") As Boolean

        fncShowForm = False

        Try
            Me.mintMode = emMode
            Me.mstrImageLocation = strImageLocation

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFormLoad, form load
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFormLoad() As Boolean

        xFormLoad = False

        Try
            'set window's size
            'grpTools.Dock = DockStyle.Bottom
            'WindowState = FormWindowState.Maximized
            'pbImage.Height = Me.Height - grpTools.Height

            'clear value
            xClear()

            Select Case mintMode

                Case clsEnum.emMode.VIEW
                    xLoadView()

                Case clsEnum.emMode.ADD
                    xLoadAdd()

                Case clsEnum.emMode.EDIT
                    xLoadEdit()

            End Select

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFormLoad", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xLoadView, form load in VIEW mode
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadView() As Boolean

        xLoadView = False

        Try
            'get image's information
            xGetImageInfo()

            pbImage.ImageLocation = mstrImageLocation

            'set value
            txtTitle.Text = mstImageInfor.strTitle
            txtDesc.Text = mstImageInfor.strDesc

            grpInfo.Enabled = False

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadView", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xLoadAdd, form load in ADD mode
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadAdd() As Boolean

        xLoadAdd = False

        Try
            ''close if user doesn't choose any file
            'If Not xOpenFile() Then Me.Close()
            pbImage.ImageLocation = mstrImageLocation

            grpInfo.Enabled = True

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadAdd", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xLoadEdit, form load in EDIT mode
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadEdit() As Boolean

        xLoadEdit = False

        Try
            'load information
            xLoadView()
            grpInfo.Enabled = True

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadEdit", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetImageInfo, get infor from database
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetImageInfo() As Boolean

        xGetImageInfo = False

        Dim tblData As DataTable = Nothing

        Try

            tblData = gobjDB.fncGetFAlbum(mstImageInfor.intID)

            If tblData Is Nothing Then Exit Function

            With tblData.Rows(0)

                mstImageInfor.strTitle = basCommon.fncCnvNullToString(.Item("IMAGE_TITLE"))
                mstImageInfor.strDesc = basCommon.fncCnvNullToString(.Item("IMAGE_DES"))
                mstrImageLocation = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAlbumPath & basCommon.fncCnvNullToString(.Item("IMAGE_NAME"))

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetImageInfo", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xOpenFile, open file
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xOpenFile() As Boolean

        xOpenFile = False

        Try
            Dim strImage As String = ""

            'exit if user doens't choose any file
            If Not basCommon.fncOpenFileDlg(strImage, basConst.gcstrImageFilter) Then Exit Function

            'exit if it is not a valid image
            If Not basCommon.fncIsValidImage(strImage) Then Exit Function

            mstrImageLocation = strImage
            pbImage.ImageLocation = mstrImageLocation

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xOpenFile", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSaveImage, save image
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSaveImage() As Boolean

        xSaveImage = False

        Try
            With mstImageInfor
                'get id
                .intID = gobjDB.fncGetMaxID(clsEnum.emTable.M_FAMILY_IMAGE) + 1

                'try to copy image to album folder
                If Not basCommon.fncCopyFile(mstrImageLocation, basConst.gcstrImageFolder & basConst.gcstrAlbumPath, String.Format(basConst.gcstrImgFormat, .intID), .strName) Then Exit Function

                'image title and description
                .strTitle = txtTitle.Text.Trim()
                .strDesc = txtDesc.Text.Trim()

            End With

            If Not gobjDB.fncInsertAlbum(mstImageInfor) Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSaveImage", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xUpdateImage, update image
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xUpdateImage() As Boolean

        xUpdateImage = False

        Try
            With mstImageInfor

                'try to copy image to album folder
                If Not basCommon.fncCopyFile(mstrImageLocation, basConst.gcstrImageFolder & basConst.gcstrAlbumPath, String.Format(basConst.gcstrImgFormat, .intID), .strName) Then Exit Function

                'image title and description
                .strTitle = txtTitle.Text.Trim()
                .strDesc = txtDesc.Text.Trim()

            End With

            gobjDB.fncUpdateAlbum(mstImageInfor)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xUpdateImage", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xClear, clear control's content
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xClear() As Boolean

        xClear = False

        Try
            txtDesc.Text = ""
            txtTitle.Text = ""
            pbImage.ImageLocation = ""

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClear", ex)
        End Try

    End Function


#End Region


End Class