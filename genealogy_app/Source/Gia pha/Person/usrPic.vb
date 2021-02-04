Imports System.IO

Public Class usrPic
    Private mstrPath As String
    Private mobjImage As Image
    Private mtooltipTitle As ToolTip = New ToolTip()
    Private mfrmMain As Form
    Public mobjfrmPerson As Form
    Public mblnFamily As Boolean = False

    Public Event MeDoubleClick()

    Public Property Path() As String
        Get
            Return mstrPath
        End Get
        Set(ByVal value As String)
            mstrPath = value
        End Set
    End Property

    Public Property objfrmAlbum() As Form
        Get
            Return mfrmMain
        End Get
        Set(ByVal value As Form)
            mfrmMain = value
        End Set
    End Property

    Public Property ImageLocation() As String
        Get
            Return PicContent.ImageLocation
        End Get
        Set(ByVal value As String)
            PicContent.ImageLocation = value
        End Set
    End Property

    '**************************************
    'Picturebox doubleClick event 
    '**************************************
    Private Sub PicContent_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PicContent.DoubleClick
        Dim frmView As New frmViewImage
        frmView.mCurrentPic = Me
        frmView.mblnFamilyAlbum = mblnFamily
        frmView.ShowForm(Me, mstrPath)

        RaiseEvent MeDoubleClick()
    End Sub

    '**************************************
    'Picture box mouse hover event 
    '**************************************
    Private Sub PicContent_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PicContent.MouseHover

        Dim ExWork As New ExifWorks(mstrPath)

        Try
            mtooltipTitle.SetToolTip(PicContent, ExWork.GetPropertyString(ExifWorks.TagNames.ImageDescription))

            ExWork.Dispose()
        Catch ex As Exception
            'Exception
            MessageBox.Show("PicContent_MouseHover" & ex.Message)
        End Try

    End Sub

    '**************************************
    'Picture box mouse leave event 
    '**************************************
    Private Sub PicContent_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PicContent.MouseLeave
        Try
            mtooltipTitle.Hide(Me)
        Catch ex As Exception
            MessageBox.Show("PicContent_MouseLeave", ex.Message)
        End Try

    End Sub

    '**************************************
    'Form load event 
    '**************************************
    Private Sub Pic_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If mblnFamily = True Then
                chkSelect.Top = chkSelect.Top - chkSelect.Height + 2
            Else
                Dim nPercent As Single = 0
                Dim nPercentW As Single = 0
                Dim nPercentH As Single = 0
                Dim loadedImage As Image = Nothing
                loadedImage = Image.FromFile(PicContent.ImageLocation)

                nPercentW = (CSng(PicContent.Width) / CSng(loadedImage.Width))
                nPercentH = (CSng(PicContent.Height) / CSng(loadedImage.Height))

                If nPercentH < nPercentW Then
                    nPercent = nPercentH
                Else
                    nPercent = nPercentW
                End If

                Dim destHeight As Integer = CInt(loadedImage.Height * nPercent)
                Dim destWidth As Integer = CInt(loadedImage.Width * nPercent)

                If destHeight < PicContent.Height Then
                    PicContent.Top = PicContent.Top + (PicContent.Height - destHeight)
                    PicContent.Height = PicContent.Height - (PicContent.Height - destHeight)
                End If

                'If destWidth < PicContent.Width Then 'And destHeight <> PicContent.Height Then
                '    chkSelect.Left = (PicContent.Width - destWidth) / 2 + 2
                'End If

                loadedImage.Dispose()
            End If

        Catch ex As Exception
            MessageBox.Show("Pic_Load", ex.Message)
        End Try
    End Sub

    Private Sub PicContent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PicContent.Click
        'If chkSelect.Checked = True Then
        '    chkSelect.Checked = False
        'Else
        '    chkSelect.Checked = True
        'End If

    End Sub
End Class
