Imports System.Collections
Imports System.Text
Imports System.IO

Public Class frmViewImage

    Private mfrmMain As usrPic
    Public mstrFilePath = ""
    Public mCurrentPic As usrPic
    Private mstrImagePath As String
    Private mstrClassName As String = "ViewImage"
    Private mblnDoubleView As Boolean = False
    Private mstrImageNewPath As String = ""
    Private mstrExtensionJpg As String = ".jpg"
    Private mstrExtensionBmp As String = ".bmp"
    Private mstrExtensionGif As String = ".gif"
    Private mstrExtensionPng As String = ".png"
    Private mintImageFile As Integer
    Private mstrThumbnail As String = "Thumbnail"
    Public mblnFamilyAlbum As Boolean = False

    Public Property blnDoubleView() As Boolean
        Get
            Return mblnDoubleView
        End Get
        Set(ByVal value As Boolean)
            mblnDoubleView = value
        End Set
    End Property

    '**************************************
    'ShowForm function
    '**************************************
    Public Function ShowForm(ByVal frmParent As UserControl, ByVal strPath As String) As Boolean
        ShowForm = False

        Try
            If Not frmParent Is Nothing Then mfrmMain = frmParent
            mstrFilePath = strPath

            If Not fncShowImage() Then Exit Function
            Me.ShowDialog()

            ShowForm = True

        Catch ex As Exception
            MessageBox.Show("ShowForm ->" & mstrClassName & ex.Message)
        End Try

    End Function

    '**************************************
    'Show Image
    '**************************************
    Private Function fncShowImage() As Boolean

        fncShowImage = False

        Try

            Dim sizeTmp As System.Drawing.Size
            sizeTmp.Height = PictureBox1.Height
            sizeTmp.Width = PictureBox1.Width
            PictureBox1.ImageLocation = mCurrentPic.Path  'mCurrentPic.PicContent.ImageLocation
            Dim loadImage As Image
            loadImage = Image.FromFile(mCurrentPic.Path)
            PictureBox1.Image = xResizeImage(loadImage, sizeTmp)
            loadImage.Dispose()

            'Show description 
            Dim EX As New ExifWorks(PictureBox1.ImageLocation)
            txtTitle.Text = EX.GetPropertyString(ExifWorks.TagNames.ImageDescription).ToString()
            Me.Text = txtTitle.Text
            EX.Dispose()

            fncShowImage = True

        Catch ex As Exception

        End Try
    End Function

    '**************************************
    'Form load event 
    '**************************************
    Private Sub ViewImage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            OpenFileDialog1.Multiselect = False
            OpenFileDialog1.Filter = "Tệp tin ảnh (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG"

            If PictureBox1.ImageLocation <> "" Then
                Dim EX As New ExifWorks(PictureBox1.ImageLocation)
                txtTitle.Text = EX.GetPropertyString(ExifWorks.TagNames.ImageDescription).ToString()
                mstrImagePath = PictureBox1.ImageLocation

                EX.Dispose()

                btnOpenFile.Visible = False
                If mblnFamilyAlbum = True Then btnSetAvatar.Visible = False
            Else
                'btnSetAvatar.Visible = False
                'pnlFunction.Visible = False
                btnCusBack.Enabled = False
                btnCusNext.Enabled = False
                btnSetAvatar.Enabled = False
                btnSaveToFolder.Enabled = False

                'Me.Height = Me.Height - Panel1.Height
                'Panel2.Top = Panel2.Top + Panel1.Height
            End If

            'xInitControlLocation()
        Catch ex As Exception
            MessageBox.Show("ViewImage_Load", ex.Message)
        End Try


    End Sub

    '**************************************
    'btnSave image click event 
    '**************************************
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            If mstrImagePath = "" Then Exit Sub
            Dim strTmp As String = mstrImagePath
            Dim strSave As String = strTmp.Substring(strTmp.LastIndexOf("\") + 1)
            Dim strFileName As String = strSave.Substring(0, strSave.LastIndexOf("."))
            Dim strImage As String = ""
            Dim loadedImage As Image = Nothing
            Dim objDirInfo As New DirectoryInfo(mstrFilePath.substring(0, mstrFilePath.lastindexof("\")))
            Dim strImageThumbnail As String = ""
            Dim strImageThumbnailNewPath As String = ""
            Dim strNewName As String = ""

            loadedImage = Image.FromFile(mstrImagePath)

            'Convert image to jpg format with extension is bmp 
            If strSave.ToLower().Contains(".bmp") Then
                If System.IO.File.Exists(mstrFilePath & strFileName & ".jpg") = True Then
                    System.IO.File.Delete(mstrFilePath & strFileName & ".jpg")
                End If
                loadedImage.Save(mstrFilePath & strFileName & ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
                strImage = mstrFilePath & strFileName & ".jpg"
                PictureBox1.ImageLocation = strImage
            End If

            'Save image file and description of file
            Dim EX As New ExifWorks(PictureBox1.ImageLocation)
            EX.SetPropertyString(ExifWorks.TagNames.ImageDescription, txtTitle.Text.Trim())
            strNewName = Date.Now.ToString("hhmmss")
            If strImage = "" Then
                mstrImageNewPath = mstrFilePath.substring(0, mstrFilePath.lastindexof("\") + 1) & strNewName & strSave.Substring(strSave.LastIndexOf("."))     'Add
                strImageThumbnailNewPath = mstrFilePath.substring(0, mstrFilePath.lastindexof("\") + 1) & mstrThumbnail & "\" & strNewName & strSave.Substring(strSave.LastIndexOf("."))
            Else
                mstrImageNewPath = mstrFilePath.substring(0, mstrFilePath.lastindexof("\") + 1) & strNewName & ".jpg"
                strImageThumbnailNewPath = mstrFilePath.substring(0, mstrFilePath.lastindexof("\") + 1) & mstrThumbnail & "\" & strNewName & ".jpg"
            End If
            If Not EX.Save(mstrImageNewPath) Then Exit Sub

            'Save Image with small size
            Dim size As System.Drawing.Size
            size.Width = 150
            size.Height = 163
            If Not EX.Save(mstrImageNewPath, strImageThumbnailNewPath, size) Then Exit Sub

            'Set object is nothing and dispose object 
            EX.Dispose()
            PictureBox1.ImageLocation = Nothing
            loadedImage.Dispose()

            If strImage <> "" Then
                If System.IO.File.Exists(strImage) = True Then
                    System.IO.File.Delete(strImage)
                End If

            Else
                Dim strDirImage As New DirectoryInfo(strTmp.Substring(0, strTmp.LastIndexOf("\") + 1))
                If strDirImage.Name = objDirInfo.Name And System.IO.File.Exists(strTmp) = True Then
                    System.IO.File.Delete(strTmp)
                End If

                strImageThumbnail = strTmp.Substring(0, strTmp.LastIndexOf("\") + 1) & mstrThumbnail & "\" & strTmp.Substring(strTmp.LastIndexOf("\") + 1)
                If System.IO.File.Exists(strImageThumbnail) = True Then
                    System.IO.File.Delete(strImageThumbnail)
                End If

            End If

            objDirInfo.Refresh()
            PictureBox1.ImageLocation = mstrImageNewPath
            If mCurrentPic IsNot Nothing Then
                mCurrentPic.PicContent.ImageLocation = strImageThumbnailNewPath
                mCurrentPic.Path = mstrImageNewPath
            Else
                mCurrentPic = New usrPic
                mCurrentPic.PicContent.ImageLocation = strImageThumbnailNewPath
                mCurrentPic.Path = mstrImageNewPath

                gobjPic.Add(mCurrentPic)
            End If

            MessageBox.Show("Bạn lưu ảnh thành công!")
            btnCusBack.Enabled = True
            btnCusNext.Enabled = True
            btnSetAvatar.Enabled = True
            btnSaveToFolder.Enabled = True

            If mblnFamilyAlbum = False Then
                btnSetAvatar.Visible = True
            Else
                btnSetAvatar.Visible = False
            End If


        Catch ex As Exception
            MessageBox.Show("btnSave_Click", ex.Message)
        End Try

    End Sub

    '**************************************
    'Open file Click event 
    '**************************************
    Private Sub btnOpenFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenFile.Click

        Dim dr As DialogResult = OpenFileDialog1.ShowDialog()
        Dim strFile As String
        Try
            If (dr = System.Windows.Forms.DialogResult.OK) Then

                strFile = OpenFileDialog1.FileName

                Dim objFileInfo As New FileInfo(strFile)
                If objFileInfo.Extension.ToLower() = mstrExtensionJpg Or objFileInfo.Extension.ToLower() = mstrExtensionBmp Or _
                   objFileInfo.Extension.ToLower() = mstrExtensionGif Or objFileInfo.Extension.ToLower() = mstrExtensionPng Then

                    PictureBox1.ImageLocation = strFile
                    mstrImagePath = strFile

                    'Show description image
                    Dim EX As New ExifWorks(PictureBox1.ImageLocation)
                    txtTitle.Text = EX.GetPropertyString(ExifWorks.TagNames.ImageDescription).ToString()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show("btnOpenFile_Click", ex.Message)
        End Try
    End Sub

    Private Sub xInitControlLocation()
        Dim loadedImage As Image = Nothing

        Try
            'PictureBox1.Width = Me.Width - 30
            'PictureBox1.Height = Me.Height - 200

            If Not PictureBox1.Image Is Nothing Then
                loadedImage = Image.FromFile(PictureBox1.ImageLocation)
                If PictureBox1.Height > 0 Then
                    PictureBox1.Image = xResizeImage(loadedImage, PictureBox1.Size)
                End If
            End If

            'Panel2.Location = New Point(Me.Width / 2 - Panel2.Width / 2, Me.Height - Panel2.Height - Panel1.Height - 50)
            If pnlFunction.Visible Then
                If Not btnSetAvatar.Visible Then
                    pnlFunction.Width = pnlFunction.Width - btnSetAvatar.Width
                End If
                pnlFunction.Location = New Point(Me.Width / 2 - pnlFunction.Width / 2, pnlFunction.Location.Y)
            Else

                pnlEditImage.Location = New Point(pnlEditImage.Location.X, pnlFunction.Location.Y)
                Me.Height = Me.Height - pnlFunction.Height
            End If


            'btnClose.Location = New Point(Panel2.Location.X + Panel2.Width - btnClose.Width - 5, Panel1.Location.Y + 10)
            'btnClose.Location = btnSetAvatar.Location

            If Not loadedImage Is Nothing Then loadedImage.Dispose()

        Catch ex As Exception
            MessageBox.Show("xInitControlLocation", ex.Message)
        End Try
    End Sub

    '**************************************
    'Resize form event
    '**************************************
    Private Sub ViewImage_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize



    End Sub

    '**************************************
    'Resize image function
    '**************************************
    Private Function xResizeImage(ByVal imgToResize As Image, _
                                  ByVal size As Size) As Image

        xResizeImage = Nothing

        Dim sourceWidth As Integer = imgToResize.Width
        Dim sourceHeight As Integer = imgToResize.Height

        Dim nPercent As Single = 0
        Dim nPercentW As Single = 0
        Dim nPercentH As Single = 0

        Try
            nPercentW = (CSng(size.Width) / CSng(sourceWidth))
            nPercentH = (CSng(size.Height) / CSng(sourceHeight))

            If nPercentH < nPercentW Then
                nPercent = nPercentH
            Else
                nPercent = nPercentW
            End If

            Dim destWidth As Integer = CInt(sourceWidth * nPercent)
            Dim destHeight As Integer = CInt(sourceHeight * nPercent)

            Dim b As New Bitmap(destWidth, destHeight)
            Dim g As Graphics = Graphics.FromImage(DirectCast(b, Image))

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight)
            g.Dispose()

            Return DirectCast(b, Image)

        Catch ex As Exception
            MessageBox.Show("xResizeImage", ex.Message)
        End Try

    End Function

    '**************************************
    'btnSetAvatar click event 
    '**************************************
    Private Sub btnSetAvatar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetAvatar.Click
        Try
            Dim strFileName As String
            If mstrFilePath = "" Then Exit Sub
            Dim intIdPerson As Integer

            strFileName = mstrFilePath.Substring(0, mstrFilePath.LastIndexOf("\"))
            strFileName = strFileName.Substring(strFileName.LastIndexOf("\") + 1)
            If Not Integer.TryParse(strFileName, intIdPerson) Then Exit Sub

            Using frmCropt As New frmCropImage(PictureBox1.ImageLocation)

                frmCropt.ShowDialog()
                If Not frmCropt.ReturnOK Then Exit Sub

                gobjImageAvatar = frmCropt.PatientPicture

            End Using
            MessageBox.Show("Bạn thiết lập ảnh đại diện thành công!")

        Catch ex As Exception
            MessageBox.Show("btnSetAvatar_Click: " & ex.Message)
        End Try
    End Sub


    '**************************************
    'Change image function
    '**************************************
    Private Sub xChangeImage(ByVal vkeyCode As System.Windows.Forms.Keys)
        Try
            If vkeyCode = 39 Then
                btnCusNext.Focus()
                btnCusNext_Click(Nothing, Nothing)

            ElseIf vkeyCode = 37 Then
                btnCusBack.Focus()
                btnCusBack_Click(Nothing, Nothing)
            End If

        Catch ex As Exception
            MessageBox.Show("xChangeImage: " & ex.Message)
        End Try

    End Sub

    '**************************************
    'btnCusBack click event
    '**************************************
    Private Sub btnCusBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCusBack.Click
        Try

            Dim intCurrentImage As Integer = 0

            Dim index As Integer = gobjPic.IndexOf(mCurrentPic)
            If index <= 0 Then
                Return
            Else
                index = index - 1
                mCurrentPic = gobjPic(index)
                mstrImagePath = mCurrentPic.Path
                fncShowImage()
            End If
            btnCusBack.Focus()
        Catch ex As Exception
            MessageBox.Show(mstrClassName & ": btnBack_Click" & ex.Message)
        End Try
    End Sub

    '**************************************
    'btnCusBack keyup event
    '**************************************
    Private Sub btnCusBack_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnCusBack.KeyUp, btnCusNext.KeyUp
        xChangeImage(e.KeyCode)
    End Sub

    '**************************************
    'btnCusNext click event
    '**************************************
    Private Sub btnCusNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCusNext.Click
        Try
            Dim index As Integer = gobjPic.IndexOf(mCurrentPic)

            If index = gobjPic.Count - 1 Then
                Return
            Else
                index = index + 1
                mCurrentPic = gobjPic(index)
                mstrImagePath = mCurrentPic.Path
                fncShowImage()
            End If
            btnCusNext.Focus()
        Catch ex As Exception
            MessageBox.Show(mstrClassName & ": btnNext_Click" & ex.Message)
        End Try
    End Sub

    '**************************************
    'PictureBox1 click event
    '**************************************
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        btnCusBack.Focus()
    End Sub

    '**************************************
    'button "Đóng" click event
    '**************************************
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            Me.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    '**************************************
    'button "Lưu ảnh vào thư mục" click event
    '**************************************
    Private Sub btnSaveToFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveToFolder.Click
        Try
            Dim strImageFile As String = PictureBox1.ImageLocation
            If strImageFile = "" Then Exit Sub

            Dim loadedImage As Image = Nothing
            Dim objSaveFileImage As New SaveFileDialog
            Dim strFileExtension = strImageFile.Substring(strImageFile.LastIndexOf("."))

            objSaveFileImage.FileName = "Image"
            objSaveFileImage.Filter = "Tệp tin ảnh  (" & strFileExtension & ")|*" & strFileExtension
            loadedImage = Image.FromFile(strImageFile)

            If objSaveFileImage.ShowDialog() = Windows.Forms.DialogResult.OK Then
                loadedImage.Save(objSaveFileImage.FileName)
                MessageBox.Show("Bạn đã lưu ảnh vào thư mục thành công!")
            End If

            loadedImage = Nothing
        Catch ex As Exception
            MessageBox.Show("btnSaveToFolder_Click", ex.Message)

        End Try

    End Sub

End Class

Partial Public Class CustomButton
    Inherits Button
    Public Sub New()
        Me.SetStyle(ControlStyles.Selectable, False)
    End Sub

End Class




