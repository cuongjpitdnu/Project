Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.Design

<Designer(GetType(TranspControlDesigner))> _
Public Class clsSelection
    Inherits Control
    Public drag As Boolean = False
    Public enab As Boolean = False
    Private m_fillColor As Color = Color.White
    Private m_opacity As Integer = 100
    Private alpha As Integer

    Public Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Opaque, True)
        Me.BackColor = Color.Transparent
    End Sub

    Public Property FillColor() As Color
        Get
            Return Me.m_fillColor
        End Get
        Set(ByVal value As Color)
            Me.m_fillColor = value
            If Me.Parent IsNot Nothing Then
                Parent.Invalidate(Me.Bounds, True)
            End If
        End Set
    End Property

    Public Property Opacity() As Integer
        Get
            If m_opacity > 100 Then
                m_opacity = 100
            ElseIf m_opacity < 1 Then
                m_opacity = 1
            End If
            Return Me.m_opacity
        End Get
        Set(ByVal value As Integer)
            Me.m_opacity = value
            If Me.Parent IsNot Nothing Then
                Parent.Invalidate(Me.Bounds, True)
            End If
        End Set
    End Property

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H20
            Return cp
        End Get
    End Property

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        Dim bounds As New Rectangle(0, 0, Me.Width - 1, Me.Height - 1)

        Dim frmColor As Color = Me.Parent.BackColor
        Dim brushColor As Brush
        Dim bckColor As Brush

        alpha = (m_opacity * 255) \ 100

        If drag Then
            Dim dragFillColor As Color
            Dim dragBckColor As Color

            If BackColor <> Color.Transparent Then
                Dim Rb As Integer = BackColor.R * alpha \ 255 + frmColor.R * (255 - alpha) \ 255
                Dim Gb As Integer = BackColor.G * alpha \ 255 + frmColor.G * (255 - alpha) \ 255
                Dim Bb As Integer = BackColor.B * alpha \ 255 + frmColor.B * (255 - alpha) \ 255
                dragBckColor = Color.FromArgb(Rb, Gb, Bb)
            Else
                dragBckColor = frmColor
            End If

            If m_fillColor <> Color.Transparent Then
                Dim Rf As Integer = m_fillColor.R * alpha \ 255 + frmColor.R * (255 - alpha) \ 255
                Dim Gf As Integer = m_fillColor.G * alpha \ 255 + frmColor.G * (255 - alpha) \ 255
                Dim Bf As Integer = m_fillColor.B * alpha \ 255 + frmColor.B * (255 - alpha) \ 255
                dragFillColor = Color.FromArgb(Rf, Gf, Bf)
            Else
                dragFillColor = dragBckColor
            End If

            alpha = 255
            brushColor = New SolidBrush(Color.FromArgb(alpha, dragFillColor))
            bckColor = New SolidBrush(Color.FromArgb(alpha, dragBckColor))
        Else
            Dim color__1 As Color = m_fillColor
            brushColor = New SolidBrush(Color.FromArgb(alpha, color__1))
            bckColor = New SolidBrush(Color.FromArgb(alpha, Me.BackColor))
        End If

        Dim pen As New Pen(Me.ForeColor)
        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot

        If Me.BackColor <> Color.Transparent Or drag Then
            g.FillRectangle(bckColor, bounds)
        End If

        If FillColor <> Color.Transparent Or drag Then
            'g.FillEllipse(brushColor, bounds);
            g.FillRectangle(brushColor, bounds)
        Else
            g.FillRectangle(New SolidBrush(Color.FromArgb(1, Color.White)), bounds)
        End If
        'else g.FillEllipse(new SolidBrush(Color.FromArgb(1, Color.White)), bounds);

        'g.DrawEllipse(pen, bounds);
        g.DrawRectangle(pen, bounds)

        pen.Dispose()
        brushColor.Dispose()
        bckColor.Dispose()
        g.Dispose()
        MyBase.OnPaint(e)
    End Sub

    Protected Overrides Sub OnBackColorChanged(ByVal e As EventArgs)
        If Me.Parent IsNot Nothing Then
            Parent.Invalidate(Me.Bounds, True)
        End If
        MyBase.OnBackColorChanged(e)
    End Sub

    Protected Overrides Sub OnParentBackColorChanged(ByVal e As EventArgs)
        Me.Invalidate()
        MyBase.OnParentBackColorChanged(e)
    End Sub
End Class

Friend Class TranspControlDesigner
    Inherits ControlDesigner
    Private myControl As clsSelection

    Protected Overrides Sub OnMouseDragMove(ByVal x As Integer, ByVal y As Integer)
        myControl = DirectCast(Me.Control, clsSelection)
        myControl.drag = True
        MyBase.OnMouseDragMove(x, y)
    End Sub

    Protected Overrides Sub OnMouseLeave()
        myControl = DirectCast(Me.Control, clsSelection)
        myControl.drag = False
        MyBase.OnMouseLeave()
    End Sub
End Class
