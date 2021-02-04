'   ****************************************************************** 
'      TITLE      : MEMBER CARD
'　　　FUNCTION   :  
'      MEMO       :  
'      CREATE     : 2012/02/13　AKB　Quyet 
'      UPDATE     :  
' 
'           2012 AKB SOFTWARE 
'   ******************************************************************
Option Explicit On
Option Strict Off

Imports System.Drawing.Drawing2D

''' <summary>
''' USERCONTROL MEMBER CARD
''' </summary>
''' <remarks></remarks>
''' <Create>2012/02/13  AKB Quyet</Create>
Public Class usrMemberCard2

    Private Const mcstrClsName As String = "usrMemberCard2"             'class name
    Private mintRadius As Int32 = 10 '20                                'Radius of the Corner Curve
    Private mintOpacity As Int32 = 125                                  'Opacity of the Control
    Private mobjBackColor As System.Drawing.Color = Color.Green         'Back Color of Control
    'Private mintID As Integer                                           'card id
    Private mintLevel As Integer                                        'level
    Private mblnShowAlias As Boolean = True                             'flag to show alias
    Private mblnShowBirth As Boolean = True                             'flag to show birth date
    Private mblnShowDecease As Boolean = True                           'flag to show decease date
    Private mblnShowRemark As Boolean = True                            'flag to show remark
    Private mblnShowImage As Boolean = True                             'flag to show image
    Private memCardSize As clsEnum.emCardSize = clsEnum.emCardSize.LARGE    'card size
    Private mblnMouseDown As Boolean                            'mouse down flag
    Private mintBeginX As Integer                               'begin X - cordinate
    Private mlstCardElement As List(Of Object)                   'list of object
    Private mlstCardMember As List(Of usrMemberDetail)

    'Public Event evnCardMove(ByVal objCard As usrMemCardBase)   'card moved event
    Public Shadows Event evnCardLocationChange(ByVal objCard As usrMemCardBase, ByVal intX As Integer, ByVal intY As Integer)


#Region "PROPERTIES"

    ''' <summary>
    ''' ShowAlias
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Visble or Invisible Alias field</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowAlias() As Boolean
        Get
            Return mblnShowAlias
        End Get
        Set(ByVal value As Boolean)
            mblnShowAlias = value

            For index As Integer = 0 To Me.mlstCardElement.Count - 1
                If TypeOf mlstCardElement(index) Is usrMemberDetail Then mlstCardElement(index).ShowAlias = value
            Next

        End Set
    End Property


    ''' <summary>
    ''' ShowBirth
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Visble or Invisible Birth field</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowBirth() As Boolean
        Get
            Return mblnShowBirth
        End Get
        Set(ByVal value As Boolean)
            mblnShowBirth = value

            For index As Integer = 0 To Me.mlstCardElement.Count - 1
                If TypeOf mlstCardElement(index) Is usrMemberDetail Then mlstCardElement(index).ShowBirth = value
            Next

        End Set
    End Property


    ''' <summary>
    ''' ShowDecease
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Visble or Invisible Decease field</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowDecease() As Boolean
        Get
            Return mblnShowDecease
        End Get
        Set(ByVal value As Boolean)
            mblnShowDecease = value

            For index As Integer = 0 To Me.mlstCardElement.Count - 1
                If TypeOf mlstCardElement(index) Is usrMemberDetail Then mlstCardElement(index).ShowDecease = value
            Next

        End Set
    End Property


    ''' <summary>
    ''' ShowRemark
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Visble or Invisible Remark field</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowRemark() As Boolean
        Get
            Return mblnShowRemark
        End Get
        Set(ByVal value As Boolean)
            mblnShowRemark = value

            For index As Integer = 0 To Me.mlstCardElement.Count - 1
                If TypeOf mlstCardElement(index) Is usrMemberDetail Then mlstCardElement(index).ShowRemark = value
            Next

            fncAlignControls()

        End Set
    End Property


    ''' <summary>
    ''' ShowImage
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Visble or Invisible Image field</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowImage() As Boolean
        Get
            Return mblnShowImage
        End Get
        Set(ByVal value As Boolean)
            mblnShowImage = value

            For index As Integer = 0 To Me.mlstCardElement.Count - 1
                If TypeOf mlstCardElement(index) Is usrMemberDetail Then mlstCardElement(index).ShowImage = value
            Next

        End Set
    End Property


    ''' <summary>
    ''' ShowCardSize
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Set or get card size</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardSize() As clsEnum.emCardSize
        Get
            Return memCardSize
        End Get
        Set(ByVal value As clsEnum.emCardSize)

            If value <> clsEnum.emCardSize.LARGE And value <> clsEnum.emCardSize.SMALL Then Exit Property

            'change size of card 
            If value = clsEnum.emCardSize.LARGE Then
                memCardSize = clsEnum.emCardSize.LARGE

            ElseIf value = clsEnum.emCardSize.SMALL Then
                memCardSize = clsEnum.emCardSize.SMALL

            End If

            For index As Integer = 0 To Me.mlstCardElement.Count - 1
                If TypeOf mlstCardElement(index) Is usrMemberDetail Then mlstCardElement(index).CardSize = memCardSize
            Next

            fncAlignControls()

        End Set
    End Property


    ''' <summary>
    ''' CardLevel
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Gest or sets card level</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardLevel() As Integer
        Get
            Return mintLevel
        End Get
        Set(ByVal value As Integer)
            mintLevel = value
        End Set
    End Property


    ''' <summary>
    ''' CardMember
    ''' </summary>
    ''' <value></value>
    ''' <returns>List of member detail</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CardMember() As List(Of usrMemberDetail)
        Get
            Return mlstCardMember
        End Get
    End Property


#Region "NOT USED"


    '================= NOT USED =================

    '''' <summary>
    '''' CardID
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks>Gets or Set card id</remarks>
    '''' <Create>2012/02/13  AKB Quyet</Create>
    'Public Property CardID() As Integer
    '    Get
    '        Return mintID
    '    End Get
    '    Set(ByVal value As Integer)
    '        mintID = value
    '    End Set
    'End Property


    '''' <summary>
    '''' CardMidTop
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks>Gets mid top point</remarks>
    '''' <Create>2012/02/13  AKB Quyet</Create>
    'Public ReadOnly Property CardMidTop() As Point

    '    Get
    '        'get current location
    '        Dim intX As Integer = Me.Location.X
    '        Dim intY As Integer = Me.Location.Y

    '        'middle top: X changes, Y doesn't
    '        intX += Me.Width \ 2

    '        Return New Point(intX, intY)

    '    End Get

    'End Property


    '''' <summary>
    '''' CardMidBottom
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks>Gets mid bottom point</remarks>
    '''' <Create>2012/02/13  AKB Quyet</Create>
    'Public ReadOnly Property CardMidBottom() As Point

    '    Get
    '        'get current location
    '        Dim intX As Integer = Me.Location.X
    '        Dim intY As Integer = Me.Location.Y

    '        'middle top: X changes, Y changes
    '        intX += Me.Width \ 2
    '        intY += Me.Height

    '        Return New Point(intX, intY)

    '    End Get

    'End Property


    '''' <summary>
    '''' CardMidLeft
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks>Gets middle left point</remarks>
    '''' <Create>2012/02/13  AKB Quyet</Create>
    'Public ReadOnly Property CardMidLeft() As Point

    '    Get
    '        'get current location
    '        Dim intX As Integer = Me.Location.X
    '        Dim intY As Integer = Me.Location.Y

    '        'middle top: Y changes, X doesn't
    '        intY += Me.Height \ 2

    '        Return New Point(intX, intY)

    '    End Get

    'End Property


    '''' <summary>
    '''' CardMidRight
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks>Get middle right point</remarks>
    '''' <Create>2012/02/13  AKB Quyet</Create>
    'Public ReadOnly Property CardMidRight() As Point

    '    Get
    '        'get current location
    '        Dim intX As Integer = Me.Location.X
    '        Dim intY As Integer = Me.Location.Y

    '        'middle top: X Y changes
    '        intX += Me.Width
    '        intY += Me.Height \ 2

    '        Return New Point(intX, intY)

    '    End Get

    'End Property


#End Region



#End Region



    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mlstCardElement = New List(Of Object)
        mlstCardMember = New List(Of usrMemberDetail)

        Me.Width = clsDefine.MEMCARD_2_W

    End Sub



#Region "FORM's METHODs"

    ''' <summary>
    ''' fncAddItem
    ''' </summary>
    ''' <param name="objCardDetail">Detail card to add</param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Sub fncAddItem(ByVal objCardDetail As usrMemberDetail)
        Try
            Dim objSeparator As FamilyLine

            If mlstCardElement.Count = 0 Then
                'if there is no item
                mlstCardElement.Add(objCardDetail)
                Me.Controls.Add(objCardDetail)

            Else
                'there is at least 1 item
                'add new separator line and new item
                objSeparator = New FamilyLine()
                'objSeparator.Width = objCardDetail.Width

                mlstCardElement.Add(objSeparator)
                mlstCardElement.Add(objCardDetail)

                Me.Controls.Add(objSeparator)
                Me.Controls.Add(objCardDetail)

            End If

            'add this member to list
            mlstCardMember.Add(objCardDetail)

            'drag drop
            AddHandler objCardDetail.evnMouseDown, AddressOf xMouseDown
            AddHandler objCardDetail.evnMouseMove, AddressOf xMouseMove
            AddHandler objCardDetail.evnMouseUp, AddressOf xMouseUp

            fncAlignControls()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncAddItem", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xAlignControls 
    ''' </summary>
    ''' <remarks>align control, set card width and height</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Sub fncAlignControls()
        Try
            Dim intY As Integer = 10
            Dim intCardCount As Integer = 0
            Dim intCardHeight As Integer = 0
            Dim intCardWidth As Integer = 0

            intCardWidth = xGetMaxElementWidth() + 3

            For index As Integer = 0 To Me.mlstCardElement.Count - 1

                mlstCardElement.Item(index).Left = 10

                'if control is card
                If TypeOf mlstCardElement.Item(index) Is usrMemberDetail Then


                    'intY = (intCardHeight + 10) * intCardCount + 10
                    intY = intCardHeight + 10 * intCardCount + 10


                    intCardHeight += mlstCardElement.Item(index).Height + 5
                    'intCardWidth = mlstCardDetail.Item(index).Width


                    intCardCount += 1
                    mlstCardElement.Item(index).Top = intY
                    mlstCardElement.Item(index).CardWidth = intCardWidth

                    'intY = intCardHeight + 10 * intCardCount + 10

                End If

                'if control is Line
                If TypeOf mlstCardElement.Item(index) Is FamilyLine Then
                    'mlstCardDetail.Item(index).Top = intY + intCardHeight + 1
                    intY = intCardHeight + 10 * intCardCount
                    mlstCardElement.Item(index).Top = intY
                    mlstCardElement.Item(index).Width = intCardWidth - 10

                End If

            Next

            'Me.Height = intCardCount * (intCardHeight + 10) + 5
            Me.Height = intCardCount * 10 + intCardHeight + 5
            Me.Width = intCardWidth + 10 '20

            Invalidate()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncAlignControls", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xGetMaxElementWidth
    ''' </summary>
    ''' <returns>integer - returns max width of cards</returns>
    ''' <remarks></remarks>
    Private Function xGetMaxElementWidth() As Integer

        Dim intMaxW As Integer = 0

        Try
            'find max width 
            For Each ctrl As Control In Me.Controls

                If ctrl.Width > intMaxW Then intMaxW = ctrl.Width

            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncAddItem", ex)
        End Try

        Return intMaxW

    End Function


    ''' <summary>
    ''' xMouseDown
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks>Mouse down event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)

        Try
            'handles left mouse
            If e.Button = Windows.Forms.MouseButtons.Left Then

                'set flag of mouse down
                mblnMouseDown = True
                mintBeginX = e.X

                MyBase.CardMouseDown = True

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMouseDown", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xMouseMove
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks>Mouse move event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)

        Try
            Dim ptLocate As New Point
            Dim ptPreLocation As Point

            'just doing when mouse is down
            If mblnMouseDown Then

                'set cursor
                Me.Cursor = Cursors.Hand
                Me.BringToFront()

                'get current location
                ptPreLocation = Me.Location

                'change location
                ptLocate = Me.Location
                ptLocate.X = ptLocate.X + e.X - mintBeginX
                If ptLocate.X < 0 Then ptLocate.X = 0
                Me.Location = ptLocate

                'RaiseEvent evnCardMove(Me)
                RaiseEvent evnCardLocationChange(Me, ptLocate.X - ptPreLocation.X, ptLocate.Y - ptPreLocation.Y)

            Else

                Me.Cursor = Cursors.Default

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMouseMove", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xMouseUp
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks>Mouse up event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)

        Try
            'set flag of mouse down
            mblnMouseDown = False
            MyBase.CardMouseDown = False

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMouseUp", ex)
        End Try

    End Sub


#End Region

#Region "FORM's EVENTS"

    ''' <summary>
    ''' usrMemberCard_MouseDown
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Mouse down event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub usrMemberCard_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown

        Try
            xMouseDown(e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_MouseDown", ex)
        End Try

    End Sub


    ''' <summary>
    ''' usrMemberCard_MouseMove
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Mouse moved event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub usrMemberCard_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove

        Try

            '2016/12/20 Start Manh Stop this function
            'xMouseMove(e)
            '2016/12/20 End Manh


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_MouseMove", ex)
        End Try

    End Sub


    ''' <summary>
    ''' usrMemberCard_MouseUp
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Mouse up event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub usrMemberCard_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp

        Try
            xMouseUp(e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_MouseUp", ex)
        End Try

    End Sub

#End Region

#Region "Background"

    ''' <summary>
    ''' UserControl1_Paint
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Control's paint event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub UserControl1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        Try

            Dim rect As Rectangle = Me.ClientRectangle 'Drawing Rounded Rectangle
            Dim color As Color

            If Me.CardSelected Then
                color = Drawing.Color.Red
            Else
                color = Drawing.Color.Gray
            End If

            rect.X = rect.X + 1
            rect.Y = rect.Y + 1
            rect.Width -= 2
            rect.Height -= 2
            Using bb As GraphicsPath = GetPath(rect, mintRadius)

                'Using br As Brush = New SolidBrush(Color.FromArgb(_opacity, _backColor))
                '    e.Graphics.FillPath(br, bb)
                'End Using

                'DrawGradient(Color.LightGray, Color.Gray, LinearGradientMode.Vertical, e.Graphics, bb)

                'Using p As Pen = New Pen(color.Gray, 1)
                Using p As Pen = New Pen(color, 1)
                    'e.Graphics.Clear(Color.White)
                    e.Graphics.DrawPath(p, bb)
                End Using

            End Using

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "UserControl1_Paint", ex)
        End Try

    End Sub

    ''' <summary>
    ''' GetPath
    ''' </summary>
    ''' <param name="rc"></param>
    ''' <param name="r"></param>
    ''' <returns></returns>
    ''' <remarks>Returns the GraphicsPath to Draw a RoundedRectangle</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Protected Function GetPath(ByVal rc As Rectangle, ByVal r As Int32) As GraphicsPath

        Dim path As GraphicsPath = Nothing

        Try
            Dim x As Int32 = rc.X, y As Int32 = rc.Y, w As Int32 = rc.Width, h As Int32 = rc.Height
            r = r << 1
            path = New GraphicsPath()
            If r > 0 Then
                If (r > h) Then r = h
                If (r > w) Then r = w
                path.AddArc(x, y, r, r, 180, 90)
                path.AddArc(x + w - r, y, r, r, 270, 90)
                path.AddArc(x + w - r, y + h - r, r, r, 0, 90)
                path.AddArc(x, y + h - r, r, r, 90, 90)
                path.CloseFigure()
            Else
                path.AddRectangle(rc)
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "GetPath", ex)
        End Try

        Return path

    End Function

    ''' <summary>
    ''' DrawGradient
    ''' </summary>
    ''' <param name="color1"></param>
    ''' <param name="color2"></param>
    ''' <param name="mode"></param>
    ''' <param name="g"></param>
    ''' <param name="drawingPath"></param>
    ''' <remarks>Fill gradient background</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub DrawGradient(ByVal color1 As Color, ByVal color2 As Color, ByVal mode As System.Drawing.Drawing2D.LinearGradientMode, ByVal g As Graphics, ByVal drawingPath As GraphicsPath)
        Try
            Dim a As New System.Drawing.Drawing2D.LinearGradientBrush(New RectangleF(0, 0, Me.Width, Me.Height), color1, color2, mode)

            'g.FillRectangle(a, New RectangleF(0, 0, Me.Width, Me.Height))
            g.FillPath(a, drawingPath)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "DrawGradient", ex)
        End Try
    End Sub

#End Region


End Class
