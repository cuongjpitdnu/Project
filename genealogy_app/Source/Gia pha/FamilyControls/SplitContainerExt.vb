Public Class SplitContainerExt
    Inherits SplitContainer

    Public Sub New()
        initArrows()
        MyBase.SplitterWidth = 10
    End Sub

    'splitter width must be 10 or higher to fit current icons
    Public Overloads Property SplitterWidth() As Integer
        Get
            Return MyBase.SplitterWidth
        End Get
        Set(ByVal value As Integer)
            If (value <> MyBase.SplitterWidth) Then
                If (value < 10) Then
                    Throw New ArgumentOutOfRangeException("SplitterWidth", "Invalid lower bound, must be 10 or higher.")
                End If
                MyBase.SplitterWidth = value
            End If
        End Set
    End Property

    'draw arrows
    Private rctUp, rctDown, rctLeft, rctRight As Rectangle
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim rct As New Rectangle(Me.SplitterRectangle.Location, New Size(Me.SplitterWidth, Me.SplitterWidth))
        If Me.Orientation = Windows.Forms.Orientation.Vertical Then
            rctLeft = rct
            e.Graphics.DrawImage(Arrows(direction.left), rct)
            rct.Offset(0, rct.Width)
            rctRight = rct
            e.Graphics.DrawImage(Arrows(direction.right), rct)
        Else
            rctUp = rct
            e.Graphics.DrawImage(Arrows(direction.up), rct)
            rct.Offset(rct.Width, 0)
            rctDown = rct
            e.Graphics.DrawImage(Arrows(direction.down), rct)
        End If
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        Cursor = Cursors.Default
    End Sub

    'get arrow of direction (caches all 4 directions in a dictionary)
    Private Arrows As New Dictionary(Of direction, Bitmap)
    Private Sub initArrows()
        For Each dir As direction In System.Enum.GetValues(GetType(direction))
            Arrows.Add(dir, getArrow(dir))
        Next
    End Sub
    Private Enum direction
        up
        down
        left
        right
    End Enum
    Private Function getArrow(ByVal dir As direction) As Bitmap
        Dim s As IO.Stream = GetType(Form).Assembly.GetManifestResourceStream("System.Windows.Forms.Arrow.ico")
        Dim bmp As Bitmap = Image.FromStream(s)
        s.Close()
        Select Case dir
            Case direction.down
            Case direction.up
                bmp.RotateFlip(RotateFlipType.Rotate180FlipNone)
            Case direction.right
                bmp.RotateFlip(RotateFlipType.Rotate270FlipNone)
            Case direction.left
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone)
        End Select
        Return bmp
    End Function

    'set splitter 
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        If Me.Orientation = Windows.Forms.Orientation.Vertical Then
            If rctLeft.Contains(e.Location) Then
                Me.SplitterDistance = 0
            ElseIf rctRight.Contains(e.Location) Then
                Me.SplitterDistance = Me.ClientSize.Width - Me.SplitterWidth
            End If
        Else
            If rctUp.Contains(e.Location) Then
                Me.SplitterDistance = 0
            ElseIf rctDown.Contains(e.Location) Then
                Me.SplitterDistance = Me.ClientSize.Width - Me.SplitterWidth
            End If
        End If
        Me.Cursor = Cursors.Default
    End Sub

    'set cursor
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Me.Orientation = Windows.Forms.Orientation.Vertical Then
            If rctLeft.Contains(e.Location) Then
                Me.Cursor = Cursors.PanWest
            ElseIf rctRight.Contains(e.Location) Then
                Me.Cursor = Cursors.PanEast
            Else
                Me.Cursor = Cursors.Default
            End If
        Else
            If rctUp.Contains(e.Location) Then
                Me.Cursor = Cursors.PanNorth
            ElseIf rctDown.Contains(e.Location) Then
                Me.Cursor = Cursors.PanSouth
            Else
                Me.Cursor = Cursors.Default
            End If
        End If
    End Sub

    Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Size = New System.Drawing.Size(150, 100)
        Me.SplitContainer1.TabIndex = 0
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
End Class
