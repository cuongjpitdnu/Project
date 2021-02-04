'   ******************************************************************
'      TITLE      : MEMBER CARD
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/09/14　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************

Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices

'   ******************************************************************
'　　　FUNCTION   : Member card
'      MEMO       : 
'      CREATE     : 2011/09/14  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class usrMemberCard1
    Implements IDisposable

    Private Const mcstrClsName As String = "usrMemberCard1"      'class name
    Private Const mcstrMale As String = "Nam"                   'male text
    Private Const mcstrFemale As String = "Nữ"                  'female text
    Private Const mcstrUnknown As String = "Không rõ"           'female text

    'Private mintID As Integer                                   'member id

    Private mblnMouseDown As Boolean                            'mouse down flag
    Private mintBeginX As Integer                               'begin X - cordinate
    Private mintGender As Integer                               'gender
    Private mblnAlive As Boolean                                 'Alive (true) or Death (false)

    Private memCardSize As clsEnum.emCardSize = clsEnum.emCardSize.LARGE    'card size

    Public Event evnCardClick(ByVal intMemID As Integer)
    Public Event evnCardDoubleClick(ByVal intMemID As Integer)
    Public Shadows Event evnCardLocationChange(ByVal objCard As usrMemCardBase, ByVal intX As Integer, ByVal intY As Integer)
    'Public Event evnCardMove(ByVal objCard As usrMemCardBase)

    Private mblnRotateText As Boolean = False

#Region "Properties"

    '   ******************************************************************
    '　　　FUNCTION   : CardID Property, Get Member ID
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property CardImage() As Image

        Get
            Return picMember.Image
        End Get

        Set(ByVal value As Image)
            picMember.Image = value
        End Set

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : CardImageLocation Property, Image location
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property CardImageLocation() As String

        Get
            Return picMember.ImageLocation
        End Get

        Set(ByVal value As String)

            If value Is Nothing Then
                picMember.ImageLocation = Nothing
                picMember.Dispose()
                Exit Property
            End If

            If System.IO.File.Exists(value) Then

                picMember.ImageLocation = value
                ' ▽2018/04/24 AKB Nguyen Thanh Tung --------------------------------
                'Fix bug change image when in mode show tree
                picMember.Image = Image.FromFile(value)
                'picMember.Load(value)
                ' △2018/04/24 AKB Nguyen Thanh Tung --------------------------------

            Else
                If Me.CardGender = clsEnum.emGender.MALE Then
                    Me.CardImage = My.Resources.no_avatar_m
                ElseIf Me.CardGender = clsEnum.emGender.FEMALE Then
                    picMember.Image = My.Resources.no_avatar_f
                ElseIf Me.CardGender = clsEnum.emGender.UNKNOW Then
                    picMember.Image = My.Resources.UnknownMember
                End If

                picMember.BorderStyle = Windows.Forms.BorderStyle.None
            End If


        End Set

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : CardName Property, Set card Name
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property CardName() As String

        Set(ByVal value As String)
            lblName.Text = value
        End Set

        Get
            Return lblName.Text
        End Get

    End Property


    ''   ******************************************************************
    ''　　　FUNCTION   : CardBirth Property, Set text
    ''      MEMO       : 
    ''      CREATE     : 2011/09/14  AKB  Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Public Property CardBirth() As String

    '    Set(ByVal value As String)

    '        lblBirth.Text = value

    '    End Set

    '    Get
    '        Return lblBirth.Text
    '    End Get

    'End Property

    ''   ******************************************************************
    ''　　　FUNCTION   : CardDeath Property, Set text
    ''      MEMO       : 
    ''      CREATE     : 2011/09/14  AKB  Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Public Property CardDeath() As String

    '    Set(ByVal value As String)

    '        lblDeath.Text = value

    '    End Set

    '    Get
    '        Return lblDeath.Text
    '    End Get

    'End Property


    '   ******************************************************************
    '　　　FUNCTION   : CardBirth Property, Set text
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property AliveStatus() As Boolean

        Set(ByVal blnAlive As Boolean)

            mblnAlive = blnAlive

            ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
            If My.Settings.blnShowBackgroupDie AndAlso Not mblnAlive Then

                Me.BackColor = My.Settings.objColorBackgroupCardDie

            Else

                Me.BackColor = My.Settings.objColorBackgroupCard

            End If
            ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
        End Set

        Get
            Return mblnAlive
        End Get

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : CardGender Property, set text
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property CardGender() As Integer

        Set(ByVal value As Integer)

            mintGender = value

            'lblGender.Text = mcstrUnknown

            'If value = clsEnum.emGender.MALE Then

            '    lblGender.Text = mcstrMale

            'End If

            'If value = clsEnum.emGender.FEMALE Then

            '    lblGender.Text = mcstrFemale

            'End If

        End Set

        Get
            Return mintGender
        End Get

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : ShowCardSize Property, set card size
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
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

        End Set
    End Property


    ''' <summary>
    ''' Frame backgound
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CardBackground() As Image
        Get
            Return Me.BackgroundImage
        End Get
        Set(ByVal value As Image)
            Me.BackgroundImage = value
        End Set
    End Property

    <Browsable(True)>
    Public Property UseRotateText() As Boolean
        Get
            Return mblnRotateText
        End Get
        Set(value As Boolean)
            mblnRotateText = value
        End Set
    End Property

#Region "NOT USED"

    ''   ******************************************************************
    ''　　　FUNCTION   : CardID Property, Get Member ID
    ''      MEMO       : 
    ''      CREATE     : 2011/09/14  AKB  Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Public ReadOnly Property CardID() As Integer

    '    Get
    '        Return mintID
    '    End Get

    'End Property


    ''   ******************************************************************
    ''　　　FUNCTION   : CardMidTop Property, Get middle point at top
    ''      MEMO       : 
    ''      CREATE     : 2011/09/14  AKB  Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
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


    ''   ******************************************************************
    ''　　　FUNCTION   : CardMidLeft Property, Get middle point at left
    ''      MEMO       : 
    ''      CREATE     : 2011/09/14  AKB  Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
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


    ''   ******************************************************************
    ''　　　FUNCTION   : CardMidRight Property, Get middle point at right
    ''      MEMO       : 
    ''      CREATE     : 2011/09/14  AKB  Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
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


    ''   ******************************************************************
    ''　　　FUNCTION   : CardMidBottom Property, Get middle point at bottom
    ''      MEMO       : 
    ''      CREATE     : 2011/09/14  AKB  Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
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

#End Region


#End Region

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.AutoValidate = Windows.Forms.AutoValidate.Disable
        Me.ResizeRedraw = False
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        picMember.Width = clsDefine.THUMBNAIL_W
        picMember.Height = clsDefine.THUMBNAIL_H
        picMember.Left = CInt((Me.Width - picMember.Width) / 2)

        ' Add any initialization after the InitializeComponent() call.
        ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
        Me.BackColor = My.Settings.objColorBackgroupCard
        lblName.ForeColor = My.Settings.objColorText
        lblName.Font = My.Settings.objFontDefaut
        ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------

    End Sub


    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/09/14  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New(ByVal intID As Integer, ByVal blnIsSmallCard As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
        Me.AutoValidate = Windows.Forms.AutoValidate.Disable
        Me.ResizeRedraw = False
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------

        Me.CardID = intID
        Me.Name = intID.ToString()
        Me.CardSize = clsEnum.emCardSize.LARGE
        If blnIsSmallCard Then Me.CardSize = clsEnum.emCardSize.SMALL
        picMember.Width = clsDefine.THUMBNAIL_W
        picMember.Height = clsDefine.THUMBNAIL_H
        picMember.Left = CInt((Me.Width - picMember.Width) / 2)

        If String.IsNullOrEmpty(My.Settings.strCard1Bg) Then
            Me.BackgroundImage = My.Resources.pic_frame
        Else
            If File.Exists(My.Settings.strCard1Bg) Then
                Me.BackgroundImage = Image.FromFile(My.Settings.strCard1Bg)
            Else
                Me.BackgroundImage = My.Resources.pic_frame
            End If
        End If

        ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
        Me.BackColor = My.Settings.objColorBackgroupCard
        lblName.ForeColor = My.Settings.objColorText
        lblName.Font = My.Settings.objFontDefaut
        ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------

        xInit()

    End Sub

    <DllImportAttribute("uxtheme.dll")>
    Private Shared Function SetWindowTheme(ByVal hWnd As IntPtr, ByVal appname As String, ByVal idlist As String) As Integer
    End Function
    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        SetWindowTheme(Me.Handle, "", "")
        MyBase.OnHandleCreated(e)
    End Sub
#Region "Form events"


    '   ******************************************************************
    '　　　FUNCTION   : usrMemberCard_MouseDown, mouse down event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub usrMemberCard_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown, picMember.MouseDown, lblName.MouseDown

        Try
            xMouseDown(e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_MouseDown", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : usrMemberCard_MouseMove, mouse move event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub usrMemberCard_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove, picMember.MouseMove, lblName.MouseMove

        Try

            '2016/12/20 Start Manh Stop this function
            'xMouseMove(e)
            '2016/12/20 End Manh

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_MouseMove", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : usrMemberCard_MouseUp, mouse up event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub usrMemberCard_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp, picMember.MouseUp, lblName.MouseUp

        Try
            'set flag of mouse down
            mblnMouseDown = False
            MyBase.CardMouseDown = False

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_MouseUp", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : usrMemberCard_MouseHover, mouse hover event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub usrMemberCard_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseHover, picMember.MouseHover, lblName.MouseHover

        Try
            Me.Cursor = Cursors.Hand

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_MouseHover", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : usrMemberCard_DoubleClick, double click event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub usrMemberCard_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.DoubleClick, picMember.DoubleClick, lblName.DoubleClick

        Try
            Dim objEvent As MouseEventArgs

            objEvent = CType(e, MouseEventArgs)

            If objEvent.Button <> Windows.Forms.MouseButtons.Left Then Exit Sub

            RaiseEvent evnCardDoubleClick(Me.CardID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_DoubleClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : usrMemberCard1_Click, click event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub usrMemberCard1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click, picMember.Click, lblName.Click
        Try
            Dim objEvent As MouseEventArgs

            objEvent = CType(e, MouseEventArgs)

            If objEvent.Button <> Windows.Forms.MouseButtons.Left Then Exit Sub

            RaiseEvent evnCardClick(Me.CardID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard1_Click", ex)
        End Try
    End Sub


    ''   ******************************************************************
    ''　　　FUNCTION   : usrMemberCard1_Move, card move
    ''      MEMO       : 
    ''      CREATE     : 2011/09/14  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Sub usrMemberCard1_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Move
    '    Try

    '        'RaiseEvent evnCardMove(Me)

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "usrMemberCard1_Move", ex)
    '    End Try
    'End Sub


#End Region


#Region "Methods"

    '   ******************************************************************
    '　　　FUNCTION   : xInit, init value
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xInit()

        Try
            'lblDeath.Text = ""
            If Not Me.CardSize = clsEnum.emCardSize.SMALL Then Exit Sub

            Me.picMember.Visible = False
            Me.lblName.Location = New Point(6, Me.lblName.Location.Y - picMember.Height)
            'Me.lblBirth.Location = New Point(6, Me.lblBirth.Location.Y - picMember.Height)
            'Me.lblDeath.Location = New Point(6, Me.lblDeath.Location.Y - picMember.Height)
            'Me.lblGender.Location = New Point(6, Me.lblGender.Location.Y - picMember.Height)

            'Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
            'Me.BackgroundImage = Nothing
            Me.Height = clsDefine.MEM_CARD_H_S

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xInit, init value
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub fncSetSize(ByVal intSize As clsEnum.emCardSize)

        Try

            If Me.CardSize = clsEnum.emCardSize.LARGE And intSize = clsEnum.emCardSize.SMALL Then

                Me.picMember.Visible = False

                Me.lblName.Location = New Point(Me.lblName.Location.X, Me.lblName.Location.Y - picMember.Height)
                'Me.lblBirth.Location = New Point(Me.lblBirth.Location.X, Me.lblBirth.Location.Y - picMember.Height)
                'Me.lblDeath.Location = New Point(Me.lblDeath.Location.X, Me.lblDeath.Location.Y - picMember.Height)
                'Me.lblGender.Location = New Point(Me.lblDeath.Location.X, Me.lblGender.Location.Y - picMember.Height)
                Me.Height = clsDefine.MEM_CARD_H_S

            ElseIf Me.CardSize = clsEnum.emCardSize.SMALL And intSize = clsEnum.emCardSize.LARGE Then
                Me.picMember.Visible = True
                Me.Height = clsDefine.MEM_CARD_H_L
                Me.lblName.Location = New Point(Me.lblName.Location.X, Me.lblName.Location.Y + picMember.Height)
                'Me.lblBirth.Location = New Point(Me.lblBirth.Location.X, Me.lblBirth.Location.Y + picMember.Height)
                'Me.lblDeath.Location = New Point(Me.lblDeath.Location.X, Me.lblDeath.Location.Y + picMember.Height)
                'Me.lblGender.Location = New Point(Me.lblGender.Location.X, Me.lblGender.Location.Y + picMember.Height)
            End If

            Me.CardSize = intSize

            'Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
            'Me.BackgroundImage = Nothing


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xMouseDown, mouse down event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
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


    '   ******************************************************************
    '　　　FUNCTION   : xMouseMove, mouse move event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
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

                Me.CardMidBottom.X = Me.CardMidBottom.X + e.X - mintBeginX
                Me.CardMidLeft.X = Me.CardMidLeft.X + e.X - mintBeginX
                Me.CardMidTop.X = Me.CardMidTop.X + e.X - mintBeginX
                Me.CardMidRight.X = Me.CardMidRight.X + e.X - mintBeginX

                'RaiseEvent evnCardMove(Me)
                RaiseEvent evnCardLocationChange(Me, ptLocate.X - ptPreLocation.X, ptLocate.Y - ptPreLocation.Y)

            Else

                Me.Cursor = Cursors.Default

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMouseMove", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xMouseUp, mouse up event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)

        Try
            'set flag of mouse down
            mblnMouseDown = False

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMouseUp", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xMouseHover, mouse hover event
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xMouseHover(ByVal e As System.EventArgs)

        Try
            Me.Cursor = Cursors.Hand

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMouseHover", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : usrMemberCard1_Paint, on paint event
    '      MEMO       : 
    '      CREATE     : 2012/04/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    'Private Sub usrMemberCard1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
    '    Try
    '        If Me.CardSelected Then
    '            'reset backgound
    '            Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle

    '        Else
    '            'reset background
    '            Me.BorderStyle = Windows.Forms.BorderStyle.None

    '        End If

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "usrMemberCard1_Paint", ex)
    '    End Try
    'End Sub


    '   ******************************************************************
    '　　　FUNCTION   : OnPaint, on paint event - draw border
    '      MEMO       : 
    '      CREATE     : 2012/04/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

        MyBase.OnPaint(e)

        If Me.CardSelected Then
            'reset backgound
            ControlPaint.DrawBorder(e.Graphics, MyBase.ClientRectangle, Color.Red, ButtonBorderStyle.Solid)

        Else
            'reset background
            ControlPaint.DrawBorder(e.Graphics, MyBase.ClientRectangle, Color.Transparent, ButtonBorderStyle.Solid)

        End If
    End Sub

    Private Sub LblName_Paint(sender As Object, e As PaintEventArgs) Handles lblName.Paint

        If Not mblnRotateText Then Exit Sub

        lblName.Text = lblName.Text.Replace(vbNewLine, " ")

        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), 0, 0, lblName.Width, lblName.Height)
        e.Graphics.TranslateTransform(CSng(lblName.Width / 2), CSng(lblName.Height / 2))

        If My.Settings.intTypeDrawText = CInt(clsEnum.emTypeDrawText.RotateLeft) Then
            e.Graphics.RotateTransform(90)
        ElseIf My.Settings.intTypeDrawText = CInt(clsEnum.emTypeDrawText.RotateRight) Then
            e.Graphics.RotateTransform(-90)
        End If

        e.Graphics.DrawString(lblName.Text, lblName.Font, New SolidBrush(lblName.ForeColor), New Point(-CInt(lblName.Height / 2), -CInt(lblName.Width / 2)))
        e.Graphics.TranslateTransform(-CSng(lblName.Width / 2), -CSng(lblName.Height / 2))
        e.Graphics.ResetTransform()
    End Sub

    Private Sub usrMemberCard1_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed

        If Me.CardImage IsNot Nothing Then Me.CardImage.Dispose()
        Me.CardImageLocation = Nothing

    End Sub


    ''   ******************************************************************
    ''　　　FUNCTION   : xSaveImage, save image for temporary use
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : strPath String
    ''      MEMO       : 
    ''      CREATE     : 2011/09/15  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xSaveImage(ByVal strPath As String) As Boolean

    '    xSaveImage = False

    '    Dim objImage As Bitmap = Nothing

    '    Try
    '        Dim objRectangle As Rectangle

    '        'drawing area
    '        objRectangle = New Rectangle(0, 0, Me.Width, Me.Height)
    '        objImage = New Bitmap(Me.Width, Me.Height)

    '        'get bitmap
    '        Me.DrawToBitmap(objImage, objRectangle)

    '        'save to JPG image
    '        objImage.Save(strPath, System.Drawing.Imaging.ImageFormat.Png)

    '        Return True

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "fncGetImage", ex)
    '    Finally
    '        If objImage IsNot Nothing Then objImage.Dispose()
    '    End Try

    'End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : fncGetImage, get image from path
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : strPath String
    ''      MEMO       : 
    ''      CREATE     : 2011/09/15  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Public Function fncGetImage(ByVal strPath As String) As String

    '    Dim strReturn As String = String.Empty

    '    Try
    '        'path to save file
    '        strPath = String.Format(basConst.gcstrUsrCardFileFormat, strPath, Me.CardID)

    '        'try to save image
    '        If xSaveImage(strPath) Then strReturn = strPath

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "fncGetImage", ex)
    '    End Try

    '    Return strReturn

    'End Function


#End Region


End Class
