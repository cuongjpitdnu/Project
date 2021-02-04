Option Explicit On
Option Strict On
Imports System.Runtime.InteropServices

Public Class pnTreePanel
    Inherits Panel

    Private Const mcstrClsName As String = "pnTreePanel"    'class name

    Private mblnMouseDown As Boolean = False                'flag to start event
    Private mptStart As Point
    Private mptEnd As Point
    Private mobjSelect As clsSelection
    Private memMode As emPanelMode

    Private mintCurScrollX As Integer
    Private mintCurScrollY As Integer

    Public Enum emPanelMode

        _SELECT
        _MOVE

    End Enum

    Public Event evnMultiSelection(ByVal rect As Rectangle)


    Public Property PanelMode() As emPanelMode
        Get
            Return memMode
        End Get
        Set(ByVal value As emPanelMode)
            Me.memMode = value
            If value = emPanelMode._SELECT Then
                Me.Cursor = Cursors.Arrow
            Else
                Me.Cursor = Cursors.SizeAll
            End If
        End Set
    End Property

    <DllImportAttribute("uxtheme.dll")>
    Private Shared Function SetWindowTheme(ByVal hWnd As IntPtr, ByVal appname As String, ByVal idlist As String) As Integer
    End Function
    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        SetWindowTheme(Me.Handle, "", "")
        MyBase.OnHandleCreated(e)
    End Sub

    Public Sub New()

        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer, True)

        Me.AutoScroll = True
        Me.AutoSize = True
        Me.AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = Color.White
        Me.Font = New Font("Microsoft Sans Serif", 8)
        Me.PanelMode = emPanelMode._MOVE

    End Sub

    'Protected Overrides ReadOnly Property CreateParams As CreateParams
    '    Get
    '        Dim cp As CreateParams = MyBase.CreateParams
    '        cp.ExStyle = cp.ExStyle Or &H2000000
    '        Return cp
    '    End Get
    'End Property

    Private Sub me_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        Try
            mblnMouseDown = True

            'selection mode
            If memMode = emPanelMode._SELECT Then

                mobjSelect = New clsSelection()
                mobjSelect.FillColor = Color.Transparent
                'mobjSelect.Opacity = 20
                'mobjSelect.BackColor = Color.Blue

                mptStart = e.Location
                mobjSelect.Location = mptStart

                Me.Controls.Add(mobjSelect)
                mobjSelect.BringToFront()

            Else
                'moving mode
                mptStart = Windows.Forms.Cursor.Position

                mintCurScrollX = Me.HorizontalScroll.Value
                mintCurScrollY = Me.VerticalScroll.Value

            End If


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "me_MouseDown", ex)
        End Try
    End Sub


    Private Sub me_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        Try
            If Not mblnMouseDown Then Exit Sub

            If memMode = emPanelMode._SELECT Then

                Dim intHeight As Integer
                Dim intWidth As Integer

                mptEnd = e.Location
                intWidth = Math.Abs(mptEnd.X - mptStart.X)
                intHeight = Math.Abs(mptEnd.Y - mptStart.Y)

                'resize
                mobjSelect.Width = intWidth
                mobjSelect.Height = intHeight

                'relocate control
                If mptStart.X > mptEnd.X Then mobjSelect.Left = mptEnd.X
                If mptStart.Y > mptEnd.Y Then mobjSelect.Top = mptEnd.Y

            Else
                Dim ptNewMouse As Point
                Dim intX As Integer
                Dim intY As Integer
                Dim intNewX As Integer
                Dim intNewY As Integer

                'calculate and set new position
                ptNewMouse = Windows.Forms.Cursor.Position
                intX = ptNewMouse.X - mptStart.X
                intY = ptNewMouse.Y - mptStart.Y

                intNewX = mintCurScrollX - intX
                intNewY = mintCurScrollY - intY

                Me.AutoScrollPosition = New Point(intNewX, intNewY)

            End If

            Me.Refresh()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "me_MouseMove", ex)
        End Try
    End Sub


    Private Sub me_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        Try


            If memMode = emPanelMode._SELECT Then

                If mblnMouseDown And mobjSelect IsNot Nothing Then

                    Dim rectSelection As Rectangle
                    rectSelection = New Rectangle(mobjSelect.Location.X, mobjSelect.Location.Y, mobjSelect.Width, mobjSelect.Height)

                    'raise event of selecting multi-control
                    RaiseEvent evnMultiSelection(rectSelection)

                End If

                mblnMouseDown = False

                Me.Controls.Remove(mobjSelect)

                mobjSelect = Nothing

            Else

                mblnMouseDown = False

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "me_MouseUp", ex)
        End Try
    End Sub

End Class
