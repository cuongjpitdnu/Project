Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D

Public Class frmCropImage
    ' Constants
    Private Const InitCropSize = 101

    ' image processing stuff
    Private bmpPicture As Bitmap
    Private iaPicture As System.Drawing.Imaging.ImageAttributes
    Private cmPicture As System.Drawing.Imaging.ColorMatrix
    Private gfxPicture As Graphics
    Private rctPicture As Rectangle

    ' cropping view stuff
    Private CropRect As Rectangle
    Private rcLT As Rectangle, rcRT As Rectangle, rcLB As Rectangle, rcRB As Rectangle
    Private rcMT As Rectangle, rcMB As Rectangle, rcML As Rectangle, rcMR As Rectangle
    Private rcOld As Rectangle, rcNew As Rectangle
    Private rcOriginal As Rectangle
    Private rcBegin As Rectangle
    Private BrushRect As SolidBrush
    Private BrushRectSmall As SolidBrush
    Private BrushColor As Color
    Private PenCrop As Pen
    Private PenWrapperConner As Pen
    Private GraphicsPathCrop As GraphicsPath

    Private AlphaBlend As Integer
    Private nSize As Integer
    Private nWd As Integer
    Private nHt As Integer
    Private nResizeRT As Integer
    Private nResizeBL As Integer
    Private nResizeLT As Integer
    Private nResizeRB As Integer
    Private nResizeMT As Integer
    Private nResizeMB As Integer
    Private nResizeML As Integer
    Private nResizeMR As Integer
    Private nThatsIt As Integer
    Private nCropRect As Integer
    Private CropWidth As Integer

    Private imageWidth As Integer
    Private imageHeight As Integer
    Private HeightOffset As Integer

    Private CropRatio As Double
    Private CropAspectRatio As Double
    Private ImageAspectRatio As Double
    Private ZoomedRatio As Double

    Private ptOld As Point
    Private ptNew As Point

    Private filename As String
    Private CropOriginPoint(8) As Point
    Private ratios As List(Of Double)

    Private _ReturnOK As Boolean = False
    Private _PatientPicture As Image

    Private mpicSize As Size
    Private mctrlPicSize As Size

    Private Enum FlagOutside
        None
        Top_Top
        Top_Left
        Top_Right
        Right_Right
        Right_Top
        Right_Bottom
        Bottom_Bottom
        Bottom_Left
        Bottom_Right
        Left_Left
        Left_Top
        Left_Bottom
    End Enum
    Private PositionOutSide As FlagOutside

    Public ReadOnly Property ReturnOK() As Boolean
        Get
            Return _ReturnOK
        End Get
    End Property

    Public Property PatientPicture() As Image
        Get
            Return _PatientPicture
        End Get
        Set(ByVal value As Image)
            _PatientPicture = value
        End Set
    End Property

    Public Sub New(ByVal strFilePath As String)
        InitializeComponent()

        mctrlPicSize = New Size(pnCropFrame.Size)

        ' double buffer
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)

        ' build list of crop ratios
        ratios = New List(Of Double)()
        'ratios.Add(1.0R)
        ratios.Add(clsDefine.PIC_CROP_RATIO)

        ' offset to make width & height proportional to image
        HeightOffset = 42 + SystemInformation.CaptionHeight + (SystemInformation.BorderSize.Height * 2)

        ' do initializations
        InitializeCropRectangle()
        LoadImage(strFilePath)
        UpdateAspectRatio()

    End Sub

    Private Sub InitializeCropRectangle()
        AlphaBlend = 150

        nSize = 5
        nWd = CropWidth = 101
        nHt = 1

        nThatsIt = 0
        nResizeRT = 0
        nResizeBL = 0
        nResizeLT = 0
        nResizeRB = 0
        nResizeMT = 0
        nResizeMB = 0
        nResizeML = 0
        nResizeMR = 0

        PenCrop = New Pen(New SolidBrush(Color.DarkGray))
        PenCrop.DashCap = DashCap.Round
        PenCrop.DashStyle = DashStyle.Dash

        PenWrapperConner = New Pen(New SolidBrush(Color.DarkGray))
        PenCrop.DashCap = DashCap.Round

        GraphicsPathCrop = New GraphicsPath()

        CropAspectRatio = ratios(0)
        BrushColor = Color.Black
        BrushRect = New SolidBrush(Color.FromArgb(AlphaBlend, BrushColor.R, _
                                                  BrushColor.G, BrushColor.B))

        BrushColor = Color.White
        BrushRectSmall = New SolidBrush(Color.FromArgb(200, BrushColor.R, _
                                                  BrushColor.G, BrushColor.B))

        ptOld = New Point(0, 0)
        rcBegin = New Rectangle()

        ptOld = New Point(0, 0)
        rcBegin = New Rectangle()
        rcOriginal = New Rectangle(0, 0, 0, 0)
        rcLT = New Rectangle(0, 0, nSize, nSize)
        rcRT = New Rectangle(0, 0, nSize, nSize)
        rcLB = New Rectangle(0, 0, nSize, nSize)
        rcRB = New Rectangle(0, 0, nSize, nSize)
        rcMT = New Rectangle(0, 0, nSize, nSize)
        rcMB = New Rectangle(0, 0, nSize, nSize)
        rcML = New Rectangle(0, 0, nSize, nSize)
        rcMR = New Rectangle(0, 0, nSize, nSize)
        rcOld = New Rectangle(0, 0, nWd, nHt)

        ' Init eight origin points of crop rectangle
        For i As Integer = 0 To 7
            CropOriginPoint(i) = New Point(0, 0)
        Next

        PositionOutSide = FlagOutside.None

        CropRect = rcOld
        AdjustResizeRects()
    End Sub

    Private Sub LoadImage(ByVal file As String)

        Cursor = Cursors.AppStarting
        Try

            _PatientPicture = Nothing


            _PatientPicture = ResizeImage(Image.FromFile(file), New Size(1.5 * 1280, 1.5 * 1024))
            imageWidth = _PatientPicture.Width
            imageHeight = _PatientPicture.Height

            ImageAspectRatio = CDbl(imageHeight) / CDbl(imageWidth)
            'Me.Height = 650 + 30
            'Me.Width = CInt(Me.Height / ImageAspectRatio) - 34
            pbxPatient.Width = CInt(pbxPatient.Height / ImageAspectRatio)
            mpicSize = New Size(CInt(pbxPatient.Height / ImageAspectRatio), pbxPatient.Height)

            Do While pbxPatient.Width > mctrlPicSize.Width

                pbxPatient.Height = pbxPatient.Height - 10
                pbxPatient.Width = CInt(pbxPatient.Height / ImageAspectRatio)
                mpicSize = New Size(CInt(pbxPatient.Height / ImageAspectRatio), pbxPatient.Height)

            Loop

            'If imageWidth > imageHeight Then
            '    ImageAspectRatio = CDbl(imageWidth) / CDbl(imageHeight)
            '    'Me.Width = 700 + (SystemInformation.BorderSize.Width * 2) + 30
            '    'Me.Height = CInt(Me.Width / ImageAspectRatio) + HeightOffset
            '    pbxPatient.Height = CInt(pbxPatient.Width / ImageAspectRatio) '+ HeightOffset
            '    mpicSize = New Size(pbxPatient.Width, CInt(pbxPatient.Width / ImageAspectRatio))
            'Else
            '    ImageAspectRatio = CDbl(imageHeight) / CDbl(imageWidth)
            '    'Me.Height = 650 + 30
            '    'Me.Width = CInt(Me.Height / ImageAspectRatio) - 34
            '    pbxPatient.Width = CInt(pbxPatient.Height / ImageAspectRatio)
            '    mpicSize = New Size(CInt(pbxPatient.Height / ImageAspectRatio), pbxPatient.Height)
            'End If

            pbxPatient.Image = _PatientPicture
            pnCropFrame.Location = New Point(CInt((Me.Width - mpicSize.Width) / 2), CInt((btnSave.Top - mpicSize.Height) / 2))
            'pnCropFrame.Location = pbxPatient.Location

            'Me.Height += 20

        Catch ex As Exception

        End Try

        ShowForm()
        Cursor = Cursors.[Default]
    End Sub

    ' Visual Basic
    Public Shared Function ResizeImage(ByVal image As Image, _
                                       ByVal size As Size, Optional ByVal preserveAspectRatio As Boolean = True) As Image
        Dim newWidth As Integer
        Dim newHeight As Integer
        If preserveAspectRatio Then
            Dim originalWidth As Integer = image.Width
            Dim originalHeight As Integer = image.Height
            Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
            Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
            Dim percent As Single = IIf(percentHeight < percentWidth, percentHeight, percentWidth)
            newWidth = CInt(originalWidth * percent)
            newHeight = CInt(originalHeight * percent)
        Else
            newWidth = size.Width
            newHeight = size.Height
        End If
        Dim newImage As Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function

    Private Sub pbxPatient_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles pbxPatient.Paint
        If pbxPatient.Image Is Nothing Then
            ' display checkerboard
            Dim xGrayBox As Boolean = True
            Dim backgroundX As Integer = 0
            While backgroundX < pbxPatient.Width
                Dim backgroundY As Integer = 0
                Dim yGrayBox As Boolean = xGrayBox
                While backgroundY < pbxPatient.Height
                    Dim recWidth As Integer = CInt((IIf((backgroundX + 50 > pbxPatient.Width), pbxPatient.Width - backgroundX, 50)))
                    Dim recHeight As Integer = CInt((IIf((backgroundY + 50 > pbxPatient.Height), pbxPatient.Height - backgroundY, 50)))
                    e.Graphics.FillRectangle(CType((IIf(yGrayBox, Brushes.LightGray, Brushes.Gainsboro)), Brush), backgroundX, backgroundY, recWidth + 2, recHeight + 2)
                    backgroundY += 50
                    yGrayBox = Not yGrayBox
                End While
                backgroundX += 50
                xGrayBox = Not xGrayBox
            End While
        Else
            ' corner drag boxes
            e.Graphics.FillRectangle(BrushRectSmall, rcLT)
            e.Graphics.FillRectangle(BrushRectSmall, rcRT)
            e.Graphics.FillRectangle(BrushRectSmall, rcLB)
            e.Graphics.FillRectangle(BrushRectSmall, rcRB)
            e.Graphics.FillRectangle(BrushRectSmall, rcMT)
            e.Graphics.FillRectangle(BrushRectSmall, rcMR)
            e.Graphics.FillRectangle(BrushRectSmall, rcMB)
            e.Graphics.FillRectangle(BrushRectSmall, rcML)

            ' main crop box 
            GraphicsPathCrop.Reset()
            GraphicsPathCrop.AddRectangle(CropRect)
            Using rgnWrapperCrop As Region = New Region(GraphicsPathCrop)
                e.Graphics.ExcludeClip(rgnWrapperCrop)
            End Using
            e.Graphics.FillRectangle((BrushRect), Me.ClientRectangle)
            e.Graphics.DrawRectangle(PenCrop, CropRect.X - 1, CropRect.Y - 1, _
                                     CropRect.Width + 1, CropRect.Height + 1)

            ' redraw corner drag boxes
            e.Graphics.FillRectangle(BrushRectSmall, rcLT)
            e.Graphics.FillRectangle(BrushRectSmall, rcRT)
            e.Graphics.FillRectangle(BrushRectSmall, rcLB)
            e.Graphics.FillRectangle(BrushRectSmall, rcRB)
            e.Graphics.FillRectangle(BrushRectSmall, rcMT)
            e.Graphics.FillRectangle(BrushRectSmall, rcMR)
            e.Graphics.FillRectangle(BrushRectSmall, rcMB)
            e.Graphics.FillRectangle(BrushRectSmall, rcML)

            AdjustResizeRects()
        End If
        MyBase.OnPaint(e)
    End Sub

    Public Sub AdjustResizeRects()
        rcLT.X = CropRect.Left - Math.Round(nSize / 2)
        rcLT.Y = CropRect.Top - Math.Round(nSize / 2)

        rcRT.X = CropRect.Right - rcRT.Width + Math.Round(nSize / 2)
        rcRT.Y = CropRect.Top - Math.Round(nSize / 2)

        rcLB.X = CropRect.Left - Math.Round(nSize / 2)
        rcLB.Y = CropRect.Bottom - rcLB.Height + Math.Round(nSize / 2)

        rcRB.X = CropRect.Right - rcRB.Width + Math.Round(nSize / 2)
        rcRB.Y = CropRect.Bottom - rcRB.Height + Math.Round(nSize / 2)

        rcMT.X = CropRect.Left + Math.Round(CropRect.Width / 2) - Math.Round(nSize / 2)
        rcMT.Y = CropRect.Top - Math.Round(nSize / 2)

        rcMB.X = CropRect.Left + Math.Round(CropRect.Width / 2) - Math.Round(nSize / 2)
        rcMB.Y = CropRect.Bottom - Math.Round(nSize / 2)

        rcML.X = CropRect.Left - Math.Round(nSize / 2)
        rcML.Y = CropRect.Top + Math.Round(CropRect.Height / 2) - Math.Round(nSize / 2)

        rcMR.X = CropRect.Right - rcRB.Width + Math.Round(nSize / 2)
        rcMR.Y = CropRect.Top + Math.Round(CropRect.Height / 2) - Math.Round(nSize / 2)
    End Sub

    Private Sub DrawDragRect(ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            AdjustResizeRects()
            pbxPatient.Invalidate()
        End If
    End Sub

    Private Sub pbxPatient_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles pbxPatient.MouseMove
        If pbxPatient.Image Is Nothing Then
            Return
        End If
        Dim pt As New Point(e.X, e.Y)
        Dim OriStatic As New Point()

        If rcLT.Contains(pt) Then
            Cursor = Cursors.SizeNWSE
        ElseIf rcRT.Contains(pt) Then
            Cursor = Cursors.SizeNESW
        ElseIf rcLB.Contains(pt) Then
            Cursor = Cursors.SizeNESW
        ElseIf rcRB.Contains(pt) Then
            Cursor = Cursors.SizeNWSE
        ElseIf rcMT.Contains(pt) Then
            Cursor = Cursors.SizeNS
        ElseIf rcMR.Contains(pt) Then
            Cursor = Cursors.SizeWE
        ElseIf rcMB.Contains(pt) Then
            Cursor = Cursors.SizeNS
        ElseIf rcML.Contains(pt) Then
            Cursor = Cursors.SizeWE
        ElseIf CropRect.Contains(pt) Then
            Cursor = Cursors.SizeAll
        Else
            Cursor = Cursors.[Default]
        End If

        If e.Button = Windows.Forms.MouseButtons.Left Then
            If nResizeRB = 1 Then
                rcNew.X = CropRect.X
                rcNew.Y = CropRect.Y
                If pt.X - rcNew.Left > pt.Y - rcNew.Top Then
                    'rcNew.Width = pt.X - rcNew.Left
                    rcNew.Height = pt.X - rcNew.Left
                    rcNew.Width = rcNew.Height * ratios(0)
                Else
                    'rcNew.Width = pt.Y - rcNew.Top
                    rcNew.Height = pt.Y - rcNew.Top
                    rcNew.Width = rcNew.Height * ratios(0)
                End If

                'Commented by AKB Quyet - 2012 09 18
                'If rcNew.Width < CropRatio OrElse rcNew.Height < CropRatio Then
                '    rcNew = CropRect
                '    rcNew.Width = CropRatio * ratios(0)
                '    rcNew.Height = CropRatio
                '    'rcNew.Width = rcNew.Height * ratios(0)
                'End If

                If rcNew.Height > pbxPatient.Height - rcNew.Y - 3 Then
                    'rcNew.Width = pbxPatient.Height - rcNew.Y - 3
                    rcNew.Height = pbxPatient.Height - rcNew.Y - 3
                    rcNew.Width = rcNew.Height * ratios(0)
                ElseIf rcNew.Width > pbxPatient.Width - rcNew.X - 3 Then
                    rcNew.Width = pbxPatient.Width - rcNew.X - 3
                    'rcNew.Height = pbxPatient.Width - rcNew.X - 3
                    'rcNew.Width = rcNew.Height * ratios(0)
                    rcNew.Height = rcNew.Width / ratios(0)
                End If

                DrawDragRect(e)
                rcOld = rcNew
                CropRect = rcNew
                Cursor = Cursors.SizeNWSE

                'ElseIf nResizeBL = 1 Then
                '    rcNew.Y = CropRect.Y
                '    If CropRect.Right - pt.X > pt.Y - rcNew.Top Then
                '        rcNew.X = CropRect.X - (CropRect.X - pt.X)
                '        'rcNew.Width = CropRect.Right - pt.X
                '        rcNew.Height = CropRect.Right - pt.X
                '        rcNew.Width = rcNew.Height * ratios(0)
                '    Else
                '        rcNew.X = CropRect.X + (CropRect.Bottom - pt.Y)
                '        'rcNew.Width = pt.Y - rcNew.Top
                '        rcNew.Height = pt.Y - rcNew.Top
                '        rcNew.Width = rcNew.Height * ratios(0)
                '    End If

                '    If rcNew.Width < CropRatio OrElse rcNew.Height < CropRatio Then
                '        rcNew.X = CropRect.X + (CropRect.Width - CInt(CropRatio))
                '        'rcNew.Width = (CropRatio)
                '        rcNew.Height = (CropRatio)
                '        rcNew.Width = rcNew.Height * ratios(0)
                '    End If

                '    If rcNew.Height > pbxPatient.Height - rcNew.Y - 3 Then
                '        rcNew.X = CropRect.Right - (pbxPatient.Height - rcNew.Y - 3)
                '        'rcNew.Width = pbxPatient.Height - rcNew.Y - 3
                '        rcNew.Height = pbxPatient.Height - rcNew.Y - 3
                '        rcNew.Width = rcNew.Height * ratios(0)
                '    ElseIf rcNew.Width > CropRect.Right - 1 Then
                '        rcNew.X = 1
                '        'rcNew.Width = CropRect.Right - 1
                '        rcNew.Height = CropRect.Right - 1
                '        rcNew.Width = rcNew.Height * ratios(0)
                '    End If

                '    DrawDragRect(e)
                '    rcOld = rcNew
                '    CropRect = rcNew
                '    Cursor = Cursors.SizeNESW

                'ElseIf nResizeRT = 1 Then
                '    rcNew.X = CropRect.X
                '    If pt.X - CropRect.X > CropRect.Bottom - pt.Y Then
                '        rcNew.Y = CropRect.Y - (pt.X - CropRect.Right)
                '        rcNew.Width = pt.X - CropRect.X
                '        rcNew.Height = pt.X - CropRect.X
                '    Else
                '        rcNew.Y = CropRect.Y - (CropRect.Top - pt.Y)
                '        rcNew.Width = CropRect.Bottom - pt.Y
                '        rcNew.Height = CropRect.Bottom - pt.Y
                '    End If

                '    If rcNew.Width < CropRatio OrElse rcNew.Height < CropRatio Then
                '        rcNew.Y = CropRect.Y + (CropRect.Height - CInt(CropRatio))
                '        rcNew.Width = CInt(CropRatio)
                '        rcNew.Height = CInt(CropRatio)
                '    End If

                '    If rcNew.Height > CropRect.Bottom - 1 Then
                '        rcNew.Y = 1
                '        rcNew.Width = CropRect.Bottom - 1
                '        rcNew.Height = CropRect.Bottom - 1
                '    ElseIf rcNew.Width > pbxPatient.Width - CropRect.X - 3 Then
                '        rcNew.Y = CropRect.Bottom - (pbxPatient.Width - CropRect.X - 3)
                '        rcNew.Width = pbxPatient.Width - CropRect.Left - 3
                '        rcNew.Height = pbxPatient.Width - CropRect.Left - 3
                '    End If

                '    DrawDragRect(e)
                '    rcOld = rcNew
                '    CropRect = rcNew
                '    Cursor = Cursors.SizeNESW

                'ElseIf nResizeLT = 1 Then
                '    If CropRect.Right - pt.X > CropRect.Bottom - pt.Y Then
                '        rcNew.X = pt.X
                '        rcNew.Y = CropRect.Y + (pt.X - CropRect.X)
                '        rcNew.Width = CropRect.Right - pt.X
                '        rcNew.Height = CropRect.Right - pt.X
                '    Else
                '        rcNew.X = CropRect.X + (pt.Y - CropRect.Y)
                '        rcNew.Y = pt.Y
                '        rcNew.Width = CropRect.Bottom - pt.Y
                '        rcNew.Height = CropRect.Bottom - pt.Y
                '    End If

                '    If rcNew.Width < CropRatio OrElse rcNew.Height < CropRatio Then
                '        rcNew.X = CropRect.X - (CInt(CropRatio) - CropRect.Width)
                '        rcNew.Y = CropRect.Y - (CInt(CropRatio) - CropRect.Height)
                '        rcNew.Width = CInt(CropRatio)
                '        rcNew.Height = CInt(CropRatio)
                '    End If

                '    If rcNew.Height > CropRect.Bottom - 1 Then
                '        rcNew.X = CropRect.Right - CropRect.Bottom + 1
                '        rcNew.Y = 1
                '        rcNew.Width = CropRect.Bottom - 1
                '        rcNew.Height = CropRect.Bottom - 1
                '    ElseIf rcNew.Width > CropRect.Right - 1 Then
                '        rcNew.X = 1
                '        rcNew.Y = CropRect.Bottom - (CropRect.Right - 1)
                '        rcNew.Width = CropRect.Right - 1
                '        rcNew.Height = CropRect.Right - 1
                '    End If

                '    DrawDragRect(e)
                '    rcOld = rcNew
                '    CropRect = rcNew
                '    Cursor = Cursors.SizeNWSE

                'ElseIf nResizeMT = 1 Then
                '    rcNew.X = CropOriginPoint(5).X - CInt(((CropOriginPoint(5).Y - pt.Y) / 2) * ratios(0))
                '    rcNew.Y = pt.Y
                '    rcNew.Width = (CropOriginPoint(5).Y - pt.Y) * ratios(0)
                '    rcNew.Height = CropOriginPoint(5).Y - pt.Y

                '    If rcNew.Width < CropRatio OrElse rcNew.Height < CropRatio Then
                '        rcNew.X = CropOriginPoint(5).X - CInt(CInt(CropRatio) / 2)
                '        rcNew.Y = CropOriginPoint(5).Y - CInt(CropRatio)
                '        rcNew.Width = CInt(CropRatio * ratios(0))
                '        rcNew.Height = CInt(CropRatio)
                '    End If

                '    If rcNew.X <= 0 AndAlso PositionOutSide <> FlagOutside.Top_Top Then
                '        rcNew.X = 1
                '        rcNew.Y = CropOriginPoint(5).Y - (CropOriginPoint(5).X - 1) * 2
                '        rcNew.Width = ((CropOriginPoint(5).X - 1) * 2) * ratios(0)
                '        rcNew.Height = (CropOriginPoint(5).X - 1) * 2
                '    ElseIf rcNew.Right > pbxPatient.Width - 3 AndAlso PositionOutSide <> FlagOutside.Top_Top Then
                '        rcNew.X = 2 * CropOriginPoint(5).X - pbxPatient.Width + 3
                '        rcNew.Y = CropOriginPoint(5).Y - (pbxPatient.Width - 3 - CropOriginPoint(5).X) * 2
                '        rcNew.Width = ((pbxPatient.Width - 3 - CropOriginPoint(5).X) * 2) * ratios(0)
                '        rcNew.Height = (pbxPatient.Width - 3 - CropOriginPoint(5).X) * 2
                '    ElseIf rcNew.Height > CropRect.Bottom - 1 Then
                '        PositionOutSide = FlagOutside.Top_Top
                '        rcNew.X = CropOriginPoint(5).X - CInt(((CropOriginPoint(5).Y - 1) / 2) * ratios(0))
                '        rcNew.Y = 1
                '        rcNew.Width = (CropOriginPoint(5).Y - 1) * ratios(0)
                '        rcNew.Height = CropOriginPoint(5).Y - 1
                '    End If

                '    DrawDragRect(e)
                '    rcOld = rcNew
                '    CropRect = rcNew
                '    Cursor = Cursors.SizeNS

                'ElseIf (nResizeMR = 1) Then
                '    rcNew.X = CropOriginPoint(7).X
                '    rcNew.Y = CropOriginPoint(7).Y - CInt(((pt.X - CropOriginPoint(7).X) / 2) * ratios(0))
                '    rcNew.Width = pt.X - CropOriginPoint(7).X
                '    rcNew.Height = pt.X - CropOriginPoint(7).X

                '    If rcNew.Width < CropRatio OrElse rcNew.Height < CropRatio Then
                '        rcNew.Y = CropOriginPoint(7).Y - CInt((CInt(CropRatio) / 2) * ratios(0))
                '        rcNew.Width = CInt(CropRatio * ratios(0))
                '        rcNew.Height = CInt(CropRatio)
                '    End If

                '    If rcNew.Y < 0 AndAlso PositionOutSide <> FlagOutside.Right_Right Then
                '        rcNew.Y = 1
                '        rcNew.X = CropOriginPoint(7).X
                '        rcNew.Width = ((CropOriginPoint(7).Y - 1) * 2) * ratios(0)
                '        rcNew.Height = (CropOriginPoint(7).Y - 1) * 2
                '    ElseIf rcNew.Bottom > pbxPatient.Height - 3 AndAlso PositionOutSide <> FlagOutside.Right_Right Then
                '        rcNew.X = CropOriginPoint(7).X
                '        rcNew.Y = 2 * CropOriginPoint(7).Y - pbxPatient.Height + 3
                '        rcNew.Width = ((pbxPatient.Height - 3 - CropOriginPoint(7).Y) * 2) * ratios(0)
                '        rcNew.Height = (pbxPatient.Height - 3 - CropOriginPoint(7).Y) * 2
                '    ElseIf rcNew.Width > pbxPatient.Width - 3 - CropRect.X Then
                '        PositionOutSide = FlagOutside.Right_Right
                '        rcNew.Y = CropOriginPoint(7).Y - CInt((pbxPatient.Width - 3 - CropOriginPoint(7).X) / 2)
                '        rcNew.Width = ((pbxPatient.Width - 3 - CropOriginPoint(7).X)) * ratios(0)
                '        rcNew.Height = (pbxPatient.Width - 3 - CropOriginPoint(7).X)
                '    End If

                '    DrawDragRect(e)
                '    rcOld = rcNew
                '    CropRect = rcNew
                '    Cursor = Cursors.SizeWE

                'ElseIf (nResizeMB = 1) Then
                '    rcNew.X = CropOriginPoint(1).X - CInt(((pt.Y - CropOriginPoint(1).Y) / 2) * ratios(0))
                '    rcNew.Y = CropOriginPoint(1).Y
                '    rcNew.Width = (pt.Y - CropOriginPoint(1).Y) * ratios(0)
                '    rcNew.Height = pt.Y - CropOriginPoint(1).Y

                '    If rcNew.Width < CropRatio OrElse rcNew.Height < CropRatio Then
                '        rcNew.X = CropOriginPoint(1).X - CInt(CInt(CropRatio) / 2)
                '        rcNew.Width = CInt(CropRatio * ratios(0))
                '        rcNew.Height = CInt(CropRatio)
                '    End If

                '    If rcNew.Right > pbxPatient.Width - 3 AndAlso PositionOutSide <> FlagOutside.Bottom_Bottom Then
                '        rcNew.X = 2 * CropOriginPoint(1).X - pbxPatient.Width + 3
                '        rcNew.Y = CropOriginPoint(1).Y
                '        rcNew.Width = ((pbxPatient.Width - 3 - CropOriginPoint(1).X) * 2) * ratios(0)
                '        rcNew.Height = (pbxPatient.Width - 3 - CropOriginPoint(1).X) * 2
                '    ElseIf rcNew.X <= 0 AndAlso PositionOutSide <> FlagOutside.Bottom_Bottom Then
                '        rcNew.X = 1
                '        rcNew.Y = CropOriginPoint(1).Y
                '        rcNew.Width = ((CropOriginPoint(1).X - 1) * 2) * ratios(0)
                '        rcNew.Height = (CropOriginPoint(1).X - 1) * 2
                '    ElseIf rcNew.Height > pbxPatient.Height - CropRect.Y - 3 Then
                '        PositionOutSide = FlagOutside.Bottom_Bottom
                '        rcNew.X = CropOriginPoint(1).X - CInt(((pbxPatient.Height - 3 - CropOriginPoint(1).Y) / 2) * ratios(0))
                '        rcNew.Width = (pbxPatient.Height - 3 - CropOriginPoint(1).Y) * ratios(0)
                '        rcNew.Height = pbxPatient.Height - 3 - CropOriginPoint(1).Y
                '    End If

                '    DrawDragRect(e)
                '    rcOld = rcNew
                '    CropRect = rcNew
                '    Cursor = Cursors.SizeNS

                'ElseIf (nResizeML = 1) Then
                '    rcNew.X = pt.X
                '    rcNew.Y = CropOriginPoint(3).Y - CInt(((CropOriginPoint(3).X - pt.X) / 2) * ratios(0))
                '    rcNew.Width = (CropRect.Right - pt.X) * ratios(0)
                '    rcNew.Height = CropRect.Right - pt.X

                '    If rcNew.Width <= CropRatio OrElse rcNew.Height <= CropRatio Then
                '        rcNew.X = CropOriginPoint(3).X - CInt(CropRatio)
                '        rcNew.Y = CropOriginPoint(3).Y - CInt(CInt(CropRatio) / 2)
                '        rcNew.Width = CInt(CropRatio * ratios(0))
                '        rcNew.Height = CInt(CropRatio)
                '    End If

                '    If rcNew.Y <= 0 AndAlso PositionOutSide <> FlagOutside.Left_Left Then
                '        rcNew.X = CropOriginPoint(3).X - (CropOriginPoint(3).Y - 1) * 2
                '        rcNew.Y = 1
                '        rcNew.Width = ((CropOriginPoint(3).Y - 1) * 2) * ratios(0)
                '        rcNew.Height = (CropOriginPoint(3).Y - 1) * 2
                '    ElseIf rcNew.Bottom > pbxPatient.Height - 3 AndAlso PositionOutSide <> FlagOutside.Left_Left Then
                '        rcNew.X = CropOriginPoint(3).X - (pbxPatient.Height - 3 - CropOriginPoint(3).Y) * 2
                '        rcNew.Y = 2 * CropOriginPoint(3).Y - pbxPatient.Height + 3
                '        rcNew.Width = ((pbxPatient.Height - 3 - CropOriginPoint(3).Y) * 2) * ratios(0)
                '        rcNew.Height = (pbxPatient.Height - 3 - CropOriginPoint(3).Y) * 2
                '    ElseIf rcNew.Width > CropRect.Right - 1 Then
                '        PositionOutSide = FlagOutside.Left_Left
                '        rcNew.X = 1
                '        rcNew.Y = CropOriginPoint(3).Y - CInt((CropOriginPoint(3).X - 1) / 2)
                '        rcNew.Width = (CropOriginPoint(3).X - 1) * ratios(0)
                '        rcNew.Height = CropOriginPoint(3).X - 1
                '    End If

                '    DrawDragRect(e)
                '    rcOld = rcNew
                '    CropRect = rcNew
                '    Cursor = Cursors.SizeWE

            ElseIf (nCropRect = 1) Then 'Moving the rectangle
                ptNew = pt
                Dim dx As Integer = ptNew.X - ptOld.X
                Dim dy As Integer = ptNew.Y - ptOld.Y

                rcNew.Offset(dx, dy)
                ' Check crop rectangle is outside at top
                If rcNew.Y < 1 Then
                    dy += 1 - rcNew.Y
                End If
                ' Check crop rectangle is outside at right
                If rcNew.Right > pbxPatient.Width - 3 Then
                    dx -= rcNew.Right - pbxPatient.Right + 3
                End If
                ' Check crop rectangle is outside at bottom
                If rcNew.Bottom >= pbxPatient.Height - 3 Then
                    dy -= rcNew.Bottom - pbxPatient.Bottom + 3
                End If
                ' Check crop rectangle is outside at left
                If rcNew.X < 1 Then
                    dx += 1 - rcNew.X
                End If

                CropRect.Offset(dx, dy)
                rcNew = CropRect
                DrawDragRect(e)
                ptOld = ptNew
            End If

            AdjustResizeRects()
            pbxPatient.Update()
        End If
        MyBase.OnMouseMove(e)
    End Sub

    Private Sub pbxPatient_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles pbxPatient.MouseDown
        Dim pt As Point = New Point(e.X, e.Y)
        rcOriginal = CropRect
        rcBegin = CropRect

        If rcRB.Contains(pt) Then
            rcOld = New Rectangle(CropRect.X, CropRect.Y, CropRect.Width, CropRect.Height)
            rcNew = rcOld
            nResizeRB = 1
        ElseIf (rcLB.Contains(pt)) Then
            rcOld = New Rectangle(CropRect.X, CropRect.Y, CropRect.Width, CropRect.Height)
            rcNew = rcOld
            nResizeBL = 1
        ElseIf (rcRT.Contains(pt)) Then
            rcOld = New Rectangle(CropRect.X, CropRect.Y, CropRect.Width, CropRect.Height)
            rcNew = rcOld
            nResizeRT = 1
        ElseIf (rcLT.Contains(pt)) Then
            rcOld = New Rectangle(CropRect.X, CropRect.Y, CropRect.Width, CropRect.Height)
            rcNew = rcOld
            nResizeLT = 1
        ElseIf (rcMT.Contains(pt)) Then
            rcOld = New Rectangle(CropRect.X, CropRect.Y, CropRect.Width, CropRect.Height)
            rcNew = rcOld
            nResizeMT = 1
        ElseIf (rcMR.Contains(pt)) Then
            rcOld = New Rectangle(CropRect.X, CropRect.Y, CropRect.Width, CropRect.Height)
            rcNew = rcOld
            nResizeMR = 1
        ElseIf (rcMB.Contains(pt)) Then
            rcOld = New Rectangle(CropRect.X, CropRect.Y, CropRect.Width, CropRect.Height)
            rcNew = rcOld
            nResizeMB = 1
        ElseIf (rcML.Contains(pt)) Then
            rcOld = New Rectangle(CropRect.X, CropRect.Y, CropRect.Width, CropRect.Height)
            rcNew = rcOld
            nResizeML = 1
        ElseIf (CropRect.Contains(pt)) Then
            nResizeBL = nResizeLT = nResizeRB = nResizeRT = 0
            nCropRect = 1
            ptNew = pt
            ptOld = pt
        End If
        PositionOutSide = FlagOutside.None
        CaculatorOriginPoint()
        nThatsIt = 1
        MyBase.OnMouseDown(e)
    End Sub

    Private Sub pbxPatient_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles pbxPatient.MouseUp
        If nThatsIt = 0 Then
            Return
        End If
        nCropRect = 0
        nResizeRB = 0
        nResizeBL = 0
        nResizeRT = 0
        nResizeLT = 0
        nResizeMT = 0
        nResizeMR = 0
        nResizeMB = 0
        nResizeML = 0

        If CropRect.Width <= 0 OrElse CropRect.Height <= 0 Then
            CropRect = rcOriginal
        End If

        If CropRect.Right > ClientRectangle.Width Then
            CropRect.Width = ClientRectangle.Width - CropRect.X
        End If

        If CropRect.Bottom > ClientRectangle.Height Then
            CropRect.Height = ClientRectangle.Height - CropRect.Y
        End If

        If (CropRect.X < 0) Then CropRect.X = 0

        If (CropRect.Y < 0) Then CropRect.Y = 0


        ' need to add logic for portrait mode of crop box in this area

        ' now that the crop box position is established
        ' force it to the proper aspect ratio
        ' and scale it

        If (CropRect.Width > CropRect.Height) Then
            CropRect.Height = CInt(CropRect.Width / CropAspectRatio)
        Else
            CropRect.Width = CInt(CropRect.Height * CropAspectRatio)
        End If

        If CropRect.Width < CropRatio AndAlso CropRect.Height < CropRatio Then
            CropRect.Width = CropRatio * ratios(0)
            CropRect.Height = CropRatio
        End If

        If CropRect.Bottom - pbxPatient.Height + 3 > 0 Then
            CropRect.Y = pnCropFrame.Height - CropRect.Height - 3
        End If

        If CropRect.Right - pnCropFrame.Width + 3 > 0 Then
            CropRect.X = pnCropFrame.Width - CropRect.Width - 3
        End If

        'Commented by AKB Quyet - 2012 09 18
        'If CropRect.Width > pnCropFrame.Width Then
        '    CropRect.X = 0
        '    CropRect.Y = 0
        '    CropRect.Width = (pnCropFrame.Width - 3) * ratios(0)
        '    CropRect.Height = pnCropFrame.Width - 3
        'End If

        'If CropRect.Height > pnCropFrame.Height Then
        '    CropRect.X = 0
        '    CropRect.Y = 0
        '    CropRect.Width = (pnCropFrame.Height - 3) * ratios(0)
        '    CropRect.Height = pnCropFrame.Height - 3
        'End If

        AdjustResizeRects()
        pbxPatient.Refresh()

        MyBase.OnMouseUp(e)
        CaculatorOriginPoint()
        nWd = rcNew.Width * ratios(0)
        nHt = rcNew.Height
        rcBegin = rcNew
    End Sub

    Private Sub pbxPatient_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles pbxPatient.MouseLeave
        Cursor = Cursors.Default
    End Sub

    Private Sub PreparePicture()
        ' If there's a picture
        If pbxPatient.Image IsNot Nothing Then
            ' Create new Bitmap object with the size of the picture
            bmpPicture = New Bitmap(pbxPatient.Image.Width, pbxPatient.Image.Height)

            ' Image attributes for setting the attributes of the picture
            iaPicture = New System.Drawing.Imaging.ImageAttributes()
        End If
    End Sub

    Private Sub FinalizePicture()
        ' Set the new color matrix
        iaPicture.SetColorMatrix(cmPicture)

        ' Set the Graphics object from the bitmap
        gfxPicture = Graphics.FromImage(bmpPicture)

        ' New rectangle for the picture, same size as the original picture
        rctPicture = New Rectangle(0, 0, pbxPatient.Image.Width, pbxPatient.Image.Height)

        ' Draw the new image
        gfxPicture.DrawImage(pbxPatient.Image, rctPicture, 0, 0, pbxPatient.Image.Width, pbxPatient.Image.Height, _
         GraphicsUnit.Pixel, iaPicture)

        ' Set the PictureBox to the new bitmap
        pbxPatient.Image = bmpPicture
    End Sub

    Private Sub UpdateAspectRatio()
        Dim ratioIndex As Integer = 0

        CropAspectRatio = ratios(ratioIndex)
        Dim CropHeight As Integer = CInt(((CropWidth / CropAspectRatio)))

        Try
            ZoomedRatio = pbxPatient.ClientRectangle.Width / CDbl(imageWidth)
        Catch
            ' imageWidth is not yet established (division by zero) force a value
            ZoomedRatio = 1.0R
        End Try

        ' update crop box and refresh everything
        nThatsIt = 1
        pbxPatient_MouseUp(Nothing, Nothing)
    End Sub

    

    Private Sub saveJpeg(ByVal path As String, ByVal img As Bitmap, ByVal quality As Long)
        ' Encoder parameter for image quality
        Dim qualityParam As New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, CLng(quality))

        ' Jpeg image codec
        Dim jpegCodec As ImageCodecInfo = getEncoderInfo("image/jpeg")

        If jpegCodec Is Nothing Then
            MessageBox.Show("Can't find JPEG encoder?", "saveJpeg()")
            Exit Sub
        End If
        Dim encoderParams As New EncoderParameters(1)
        encoderParams.Param(0) = qualityParam

        img.Save(path, jpegCodec, encoderParams)
    End Sub

    Private Function getEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
        ' Get image codecs for all image formats
        Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()

        ' Find the correct image codec
        For i As Integer = 0 To codecs.Length - 1
            If codecs(i).MimeType = mimeType Then
                Return codecs(i)
            End If
        Next

        Return Nothing
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ' output image size is based upon the visible crop rectangle and scaled to 
        ' the ratio of actual image size to displayed image size
        Dim ScaledCropRect As New Rectangle()
        ScaledCropRect.X = CInt((CropRect.X / ZoomedRatio))
        ScaledCropRect.Y = CInt((CropRect.Y / ZoomedRatio))
        ScaledCropRect.Width = CInt((CDbl((CropRect.Width)) / ZoomedRatio))
        ScaledCropRect.Height = CInt((CDbl((CropRect.Height)) / ZoomedRatio))

        Try
            _PatientPicture = CType(CropImage(_PatientPicture, ScaledCropRect), Bitmap)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "btnOK_Click()")
        End Try
        _ReturnOK = True
    End Sub

    Private Function CropImage(ByVal img As Image, ByVal cropArea As Rectangle) As Image

        Dim OriginalBitmap As Bitmap = New Bitmap(img)
        Dim cropBitmap As Bitmap = New Bitmap(cropArea.Width, cropArea.Height)
        Try

            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(OriginalBitmap, 0, 0, cropArea, GraphicsUnit.Pixel)

        Catch ex As OutOfMemoryException
            pbxPatient_MouseMove(Nothing, Nothing)
            cropBitmap = OriginalBitmap.Clone(cropArea, OriginalBitmap.PixelFormat)
        End Try

        Return CType((cropBitmap), Image)

        'Dim bmpImage As New Bitmap(img)
        'Dim bmpCrop As Bitmap = Nothing
        'Try
        '    bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat)
        'Catch ex As OutOfMemoryException
        '    pbxPatient_MouseMove(Nothing, Nothing)
        '    bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat)
        'End Try
        'Return CType((bmpCrop), Image)
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub ShowForm()
        If (ImageAspectRatio = 0) Then Return
        'CropRatio = CDbl((InitCropSize * (Me.Width - SystemInformation.BorderSize.Width * 2 - 30)) / imageWidth)
        CropRatio = CDbl((InitCropSize * (mpicSize.Width)) / imageWidth)

        'pnCropFrame.Width = Me.Width - SystemInformation.BorderSize.Width * 2 - 30
        'pnCropFrame.Height = Me.Height - (26 + SystemInformation.ToolWindowCaptionHeight + 45) - 20
        pnCropFrame.Width = mpicSize.Width
        pnCropFrame.Height = mpicSize.Height

        ' Resize Image
        'Dim bmp As Bitmap = New Bitmap(pnCropFrame.Width - 4, pnCropFrame.Height - 3)
        'Dim g As Graphics = Graphics.FromImage(bmp)
        'g.DrawImage(_PatientPicture, 0, 0, pnCropFrame.Width - 4, pnCropFrame.Height - 4)
        'pbxPatient.Image = bmp
        'g.Dispose()

        If CropRatio < nSize * 3 + 2 Then
            CropRatio = nSize * 3 + 2
        End If

        'If pnCropFrame.Width > imageWidth OrElse pnCropFrame.Height > imageHeight Then
        '    If pnCropFrame.Width > pnCropFrame.Height Then
        '        CropRatio = pnCropFrame.Height - 3
        '    Else
        '        CropRatio = imageHeight - 3
        '    End If
        'End If

        ' logic for portrait mode goes here for form resize
        CropRect.X = CInt((pnCropFrame.Width - CropRect.Width - CropRatio) / 2)
        CropRect.Y = CInt((pnCropFrame.Height - CropRect.Height - CropRatio) / 2)
        CropRect.Width = CInt(CropRatio)
        CropRect.Height = CInt(CropRatio)
        UpdateAspectRatio()

        'btnSave.Left = CInt(Me.Width / 2) - SystemInformation.BorderSize.Width - btnSave.Width - 10
        'btnCancel.Left = CInt(Me.Width / 2) - SystemInformation.BorderSize.Width + 10
        Me.Refresh()
    End Sub

    Private Sub CaculatorOriginPoint()
        CropOriginPoint(0).X = CropRect.X                               '   0 ------ 1 ------ 2
        CropOriginPoint(0).Y = CropRect.Y                               '   |                 |     
        CropOriginPoint(1).X = CropRect.X + CropRect.Width / 2          '   |                 |
        CropOriginPoint(1).Y = CropRect.Y                               '   |                 |
        CropOriginPoint(2).X = CropRect.X + CropRect.Width              '   7                 3
        CropOriginPoint(2).Y = CropRect.Y                               '   |                 |
        CropOriginPoint(3).X = CropRect.X + CropRect.Width              '   |                 |              
        CropOriginPoint(3).Y = CropRect.Y + CropRect.Height / 2         '   |                 |
        CropOriginPoint(4).X = CropRect.X + CropRect.Width              '   6 ------ 5 ------ 4
        CropOriginPoint(4).Y = CropRect.Y + CropRect.Height
        CropOriginPoint(5).X = CropRect.X + CropRect.Width / 2
        CropOriginPoint(5).Y = CropRect.Y + CropRect.Height
        CropOriginPoint(6).X = CropRect.X
        CropOriginPoint(6).Y = CropRect.Y + CropRect.Height
        CropOriginPoint(7).X = CropRect.X
        CropOriginPoint(7).Y = CropRect.Y + CropRect.Height / 2
    End Sub

   
    Private Sub btnSelectImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectImage.Click
        'show dialog
        If Not dlgOpenImage.ShowDialog = Windows.Forms.DialogResult.OK Then Exit Sub

        'check validation of image and get path
        If Not basCommon.fncIsValidImage(dlgOpenImage.FileName) Then Exit Sub

        ' do initializations
        pbxPatient.Image = Nothing

        pnCropFrame.Size = New Size(mctrlPicSize)
        pbxPatient.Size = New Size(pnCropFrame.Size)

        InitializeCropRectangle()
        LoadImage(dlgOpenImage.FileName)
        UpdateAspectRatio()

    End Sub
End Class