Option Explicit On

'Imports FamilyCommon

Public Class FamilyLine

#Region "Line Variable"

    Dim miLineLenght As Integer = clsDefine.LINE_LENGHT
    Dim memLineType As clsEnum.emLineDirection = clsEnum.emLineDirection.HORIZONTAL

    Dim miRootID As Integer
    Dim miRootSubID As Integer
    Dim miPosNo As Integer

#End Region

#Region "Line Properties"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LineType() As clsEnum.emLineDirection

        Get
            Return Me.memLineType
        End Get

        Set(ByVal value As clsEnum.emLineDirection)

            Me.memLineType = value
            Me.SetLineType(Me.memLineType)

        End Set

    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Lenght() As Integer

        Get
            Return Me.miLineLenght
        End Get

        Set(ByVal value As Integer)

            Me.miLineLenght = value
            SetLineType(Me.memLineType)

        End Set

    End Property

    Public Property LineRootSubID() As Integer
        Get
            Return miRootSubID
        End Get
        Set(ByVal value As Integer)
            miRootSubID = value
        End Set
    End Property

    Public Property LineRootID() As Integer
        Get
            Return miRootID
        End Get
        Set(ByVal value As Integer)
            miRootID = value
        End Set
    End Property

    Public Property LinePosNo() As Integer
        Get
            Return Me.miPosNo
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            Me.miPosNo = value
        End Set
    End Property

#End Region

#Region "Line Cotrol Function"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="emLineType"></param>
    ''' <remarks></remarks>
    Private Sub SetLineType(ByVal emLineType As clsEnum.emLineDirection)

        Try
            Dim szTmp As Size

            If emLineType = clsEnum.emLineDirection.HORIZONTAL Then

                szTmp = New Size(Me.Lenght, Me.LineSize)

            ElseIf emLineType = clsEnum.emLineDirection.VERTICAL Then

                szTmp = New Size(Me.LineSize, Me.Lenght)

            End If

            Me.Size = szTmp
            'Me.Width = szTmp.Width
            'Me.Height = szTmp.Height

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

#End Region

#Region "Line Control Event"

    Private Sub FamilyLine_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            'Me.LineType = clsEnum.LineType.HORIZONTAL

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub FamilyLine_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Try

            Dim objDraw As clsDraw = New clsDraw(e.Graphics)
            Dim pStart As Point
            Dim pEnd As Point

            If Me.LineType = clsEnum.emLineDirection.HORIZONTAL Then

                pStart = New Point(0, Me.MidPointCenter.Y - Me.Location.Y)
                pEnd = New Point(Me.Width, Me.MidPointCenter.Y - Me.Location.Y)

            Else

                pStart = New Point(Me.MidPointCenter.X - Me.Location.X, 0)
                pEnd = New Point(Me.MidPointCenter.X - Me.Location.X, Me.Height)

            End If

            objDraw.DrawLine(pStart, pEnd, Color.Black, clsDefine.PENSIZE, Drawing2D.DashStyle.Dot)

            Dim pStart2 As Point
            Dim pEnd2 As Point

            pStart2 = pStart
            pEnd2 = pEnd

            pStart2.Y += 15
            pEnd2.Y += 15

            objDraw.DrawLine(pStart2, pEnd2, Color.Black, clsDefine.PENSIZE, Drawing2D.DashStyle.Dot)

            objDraw = Nothing

        Catch ex As Exception

            Throw ex

        End Try
    End Sub

    Private Sub FamilyLine_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try

            If Me.LineType = clsEnum.emLineDirection.HORIZONTAL Then

                Me.Lenght = Me.Width

            ElseIf Me.LineType = clsEnum.emLineDirection.VERTICAL Then

                Me.Lenght = Me.Height

            End If

            Me.SetLineType(Me.LineType)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region


    Public Event evnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
    Public Event evnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
    Public Event evnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer, True)

        ' Add any initialization after the InitializeComponent() call.
        Me.LineType = clsEnum.emLineDirection.HORIZONTAL
        Me.Visible = True
        Me.Height = 50

    End Sub


    Private Sub FamilyLine_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp
        Try
            RaiseEvent evnMouseUp(e)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FamilyLine_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        Try
            RaiseEvent evnMouseDown(e)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FamilyLine_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        Try
            RaiseEvent evnMouseMove(e)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


End Class

