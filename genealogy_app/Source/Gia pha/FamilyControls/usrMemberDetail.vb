'   ****************************************************************** 
'      TITLE      : MEMBER DETAIL CARD
'　　　FUNCTION   :  
'      MEMO       :  
'      CREATE     : 2012/02/13　AKB　Quyet 
'      UPDATE     :  
' 
'           2012 AKB SOFTWARE 
'   ******************************************************************
Option Explicit On
Option Strict On

''' <summary>
''' CLASS MEMBER DETAIL CARD
''' </summary>
''' <remarks></remarks>
''' <Create>2012/02/13  AKB Quyet</Create>
Public Class usrMemberDetail

    Private Const mcstrClsName As String = "usrMemberDetail"            'class name

    Private mintID As Integer                                           'card id
    Private mintGender As Integer                                       'card gender
    Private mobjContainer As usrMemberCard2                             'container

    Private mblnShowAlias As Boolean = True                             'flag to show alias
    Private mblnShowBirth As Boolean = True                             'flag to show birth
    Private mblnShowDecease As Boolean = True                           'flag to show decease
    Private mblnShowRemark As Boolean = True                            'flag to show remark
    Private mblnShowImage As Boolean = True                             'flag to show image
    Private mblnIsFHead As Boolean = False                              'flag to show head

    Private memCardSize As clsEnum.emCardSize = clsEnum.emCardSize.LARGE    'card size

    Private mblnMouseDown As Boolean                                    'mouse down flag
    Private mintBeginX As Integer                                       'begin X - cordinate

    Public Event evnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)                           'mouse down event
    Public Event evnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)                           'mouse move event
    Public Event evnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)                             'mouse up event
    Public Event evnCardDoubleClick(ByVal intMemID As Integer, ByVal objCardDetail As usrMemberDetail)  'double click event
    Public Event evnCardClick(ByVal intMemID As Integer, ByVal objCardDetail As usrMemberDetail)        'click event

#Region "PROPERTIES"

    ''' <summary>
    ''' CardID
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>Integer</returns>
    ''' <remarks>Gets Card ID</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property CardID() As Integer

        Get
            Return mintID
        End Get

    End Property


    ''' <summary>
    ''' CardImage
    ''' </summary>
    ''' <value>Image</value>
    ''' <returns>Image</returns>
    ''' <remarks>Gets or Sets card image</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardImage() As Image

        Get
            Return picAvatar.Image
        End Get

        Set(ByVal value As Image)
            picAvatar.Image = value
        End Set

    End Property


    ''' <summary>
    ''' CardImageLocation
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks>Gets or Sets image location</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardImageLocation() As String

        Get
            Return picAvatar.ImageLocation
        End Get

        Set(ByVal value As String)

            If value Is Nothing Then
                picAvatar.ImageLocation = Nothing
                picAvatar.Dispose()
                Exit Property
            End If

            If System.IO.File.Exists(value) Then

                picAvatar.ImageLocation = value
                ' ▽2018/04/24 AKB Nguyen Thanh Tung --------------------------------
                'Fix bug change image when in mode show tree
                picAvatar.Image = Image.FromFile(value)
                'picAvatar.Load(value)
                ' △2018/04/24 AKB Nguyen Thanh Tung --------------------------------

            Else
                If Me.CardGender = clsEnum.emGender.MALE Then
                    Me.CardImage = My.Resources.no_avatar_m
                ElseIf Me.CardGender = clsEnum.emGender.FEMALE Then
                    picAvatar.Image = My.Resources.no_avatar_f
                ElseIf Me.CardGender = clsEnum.emGender.UNKNOW Then
                    picAvatar.Image = My.Resources.UnknownMember
                End If

            End If

        End Set

    End Property


    ''' <summary>
    ''' CardName
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks>Gets or Sets card name</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardName() As String

        Set(ByVal value As String)
            lblName.Text = value
        End Set
        Get
            Return lblName.Text
        End Get

    End Property


    ''' <summary>
    ''' CardAlias
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks>Gets or Sets card alias</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardAlias() As String

        Set(ByVal value As String)
            If Not basCommon.fncIsBlank(value) Then
                lblAlias.Text = String.Format("({0})", value)
                Me.ShowAlias = True
            Else
                Me.ShowAlias = False
            End If
        End Set
        Get
            Return lblAlias.Text
        End Get

    End Property


    ''' <summary>
    ''' CardDie
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks>Gets or Sets decease string</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardDie() As String

        Set(ByVal value As String)

            If basCommon.fncIsBlank(value) Then
                Me.ShowDecease = False
                Exit Property
            End If

            ' ▽ 2012/11/23   AKB Quyet （変更内容）*********************************
            'lblDecease.Text = String.Format("Mất: {0}", value)
            lblDecease.Text = value
            'lblDecease.Width = 500
            ' △ 2012/11/23   AKB Quyet *********************************************

            Me.ShowDecease = True

        End Set
        Get
            Return lblDecease.Text
        End Get

    End Property


    ''' <summary>
    ''' CardBirth
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks>Gets or Sets birth string</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardBirth() As String

        Set(ByVal value As String)

            If basCommon.fncIsBlank(value) Then
                Me.ShowBirth = False
                Exit Property
            End If

            'lblBirth.Text = String.Format("Sn: {0}", value)
            lblBirth.Text = value
            Me.ShowBirth = True

        End Set
        Get
            Return lblBirth.Text
        End Get

    End Property


    ''' <summary>
    ''' CardRemark
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks>Gets or Sets remark string</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardRemark() As String

        Set(ByVal value As String)

            If basCommon.fncIsBlank(value) Then
                Me.ShowRemark = False
                Exit Property
            End If

            Me.ShowRemark = True
            lblRemark.Text = basCommon.fncCnvRtfToText(value)
            'lblRemark.Text = value

        End Set
        Get
            Return lblRemark.Text
        End Get

    End Property


    ''' <summary>
    ''' CardLevel
    ''' </summary>
    ''' <value>Integer</value>
    ''' <remarks>Sets card level</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public WriteOnly Property CardLevel() As Integer

        Set(ByVal value As Integer)

            If value > 0 Then
                lblGeneration.Text = String.Format("Đời thứ: {0}", value)
            Else
                lblGeneration.Text = "Đời thứ: ..."
            End If

        End Set

    End Property


    ''' <summary>
    ''' CardGender
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>Integer</returns>
    ''' <remarks>Gets or Sets gender</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardGender() As Integer

        Set(ByVal value As Integer)

            mintGender = value

        End Set

        Get
            Return mintGender
        End Get

    End Property


    ''' <summary>
    ''' ShowAlias
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>Boolean</returns>
    ''' <remarks>Visibles or Invisibles alias</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowAlias() As Boolean
        Get
            Return mblnShowAlias
        End Get
        Set(ByVal value As Boolean)
            mblnShowAlias = value
            lblAlias.Visible = value
        End Set
    End Property


    ''' <summary>
    ''' ShowBirth
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>Boolean</returns>
    ''' <remarks>Visibles or Invisibles birth</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowBirth() As Boolean
        Get
            Return mblnShowBirth
        End Get
        Set(ByVal value As Boolean)
            mblnShowBirth = value
            lblBirth.Visible = value

            'If Not basCommon.fncIsBlank(lblDecease.Text) And Not basCommon.fncIsBlank(lblBirth.Text) Then Exit Property
            If Not lblDecease.Visible And Not lblBirth.Visible Then
                lblRemark.Top = lblBirth.Top
                Exit Property
            End If

            If mblnShowBirth Then
                'Me.Height += lblBirth.Height + 5
                'lblRemark.Top += lblBirth.Height
                lblRemark.Top = lblBirth.Top + lblBirth.Height
                If mblnShowDecease Then lblRemark.Top = lblDecease.Top + lblDecease.Height

            Else
                'Me.Height -= lblBirth.Height - 5
                'lblRemark.Top -= lblBirth.Height
                If Not lblDecease.Visible Then lblRemark.Top = lblBirth.Top

            End If

            'Me.Height = xGetHeight()

        End Set
    End Property


    ''' <summary>
    ''' ShowDecease
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>Boolean</returns>
    ''' <remarks>Visibles or Invisibles decease</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowDecease() As Boolean
        Get
            Return mblnShowDecease
        End Get
        Set(ByVal value As Boolean)
            mblnShowDecease = value
            lblDecease.Visible = value

            'if both of death and birth are invisible, move up the remark label
            If Not lblDecease.Visible And Not lblBirth.Visible Then
                lblRemark.Top = lblBirth.Top
                Exit Property
            End If

            If mblnShowDecease Then

                lblRemark.Top = lblBirth.Top + lblBirth.Height
                If mblnShowBirth Then lblRemark.Top = lblDecease.Top + lblDecease.Height

            Else
                'Me.Height -= lblDecease.Height - 5
                'lblRemark.Top -= lblBirth.Height
                If Not lblBirth.Visible Then lblRemark.Top = lblBirth.Top

            End If

            'Me.Height = xGetHeight()

        End Set
    End Property


    ''' <summary>
    ''' ShowRemark
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>Boolean</returns>
    ''' <remarks>Visibles or Invisibles remakr</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowRemark() As Boolean
        Get
            Return mblnShowRemark
        End Get
        Set(ByVal value As Boolean)
            mblnShowRemark = value
            lblRemark.Visible = value

            'If mblnShowRemark Then
            '    Me.Height += lblRemark.Height
            '    'Me.Height += 40


            'Else
            '    Me.Height -= lblRemark.Height - 3
            '    'Me.Height -= 40
            'End If


            'Me.Height = xGetHeight()

        End Set
    End Property


    ''' <summary>
    ''' ShowImage
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>Boolean</returns>
    ''' <remarks>Visibles or Invisibles image</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property ShowImage() As Boolean
        Get
            Return mblnShowImage
        End Get
        Set(ByVal value As Boolean)
            mblnShowImage = value
            picAvatar.Visible = value

            'change size of card and position of controls
            If mblnShowImage Then
                lblName.Left = picAvatar.Location.X + picAvatar.Width + 7
                lblAlias.Left = picAvatar.Location.X + picAvatar.Width + 7
                lblGeneration.Left = picAvatar.Location.X + picAvatar.Width + 7
                'lblBirth.Left = picAvatar.Location.X + picAvatar.Width + 7
                'lblDecease.Left = picAvatar.Location.X + picAvatar.Width + 7
            Else
                'If picHead.Visible = False Then lblName.Left = 0 Else lblName.Left = 18
                lblName.Left = 0
                lblAlias.Left = 0
                lblGeneration.Left = 0
                'lblBirth.Left = 0
                'lblDecease.Left = 0
            End If

        End Set
    End Property


    ''' <summary>
    ''' CardSize
    ''' </summary>
    ''' <value>clsEnum.CardSize</value>
    ''' <returns>clsEnum.CardSize</returns>
    ''' <remarks>Gets or Sets card size</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardSize() As clsEnum.emCardSize
        Get
            Return memCardSize
        End Get
        Set(ByVal value As clsEnum.emCardSize)

            'align right for F-Head mark
            'Me.picHead.Left = Me.Width - 16

            'change size of card 
            If value = clsEnum.emCardSize.LARGE Then
                memCardSize = clsEnum.emCardSize.LARGE

                Me.ShowImage = True
                Me.Width += picAvatar.Width
                Me.Height = lblBirth.Top

                If Me.ShowBirth Then Me.Height += lblBirth.Height
                If Me.ShowRemark Then Me.Height += Me.lblRemark.Height


            ElseIf value = clsEnum.emCardSize.SMALL Then
                memCardSize = clsEnum.emCardSize.SMALL

                Me.ShowImage = False
                Me.Width -= picAvatar.Width
                Me.Height = lblAlias.Top

                If Me.ShowAlias Then Me.Height += lblAlias.Height
                If Me.ShowBirth Then Me.Height += lblBirth.Height
                If Me.ShowRemark Then Me.Height += Me.lblRemark.Height

                Me.fncResize()

            End If

        End Set
    End Property


    ''' <summary>
    ''' IsHead
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>Boolean</returns>
    ''' <remarks>Gets or Sets head flag</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property IsHead() As Boolean
        Get
            Return mblnIsFHead
        End Get
        Set(ByVal value As Boolean)
            mblnIsFHead = value
            picHead.Visible = mblnIsFHead
        End Set
    End Property


    ''' <summary>
    ''' CardContainer
    ''' </summary>
    ''' <value>usrMemberCard2</value>
    ''' <returns>usrMemberCard2</returns>
    ''' <remarks>Gets or Sets container</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardContainer() As usrMemberCard2
        Get
            Return mobjContainer
        End Get
        Set(ByVal value As usrMemberCard2)
            mobjContainer = value
        End Set
    End Property


    ''' <summary>
    ''' CardWidth - set card width and reset width for remark field
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardWidth() As Integer
        Get
            Return Me.Width
        End Get
        Set(ByVal value As Integer)

            Me.Width = value
            lblRemark.Width = value

        End Set
    End Property


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
        xInit()

    End Sub


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <param name="intID">Card id</param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Sub New(ByVal intID As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mintID = intID
        xInit()

    End Sub



#Region "FORM's METHODs"


    ''' <summary>
    ''' fncResize - set size for card
    ''' </summary>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Sub fncResize()

        Try
            Me.Height = xGetHeight()
            Me.Width = xGetWidth()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncResize", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xInit
    ''' </summary>
    ''' <remarks>Init values</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xInit()
        Try
            mblnShowAlias = lblAlias.Visible
            mblnShowBirth = lblBirth.Visible
            mblnShowDecease = lblDecease.Visible
            mblnShowRemark = lblRemark.Visible
            mblnShowImage = picAvatar.Visible
            memCardSize = clsEnum.emCardSize.LARGE

            Me.lblAlias.Text = ""
            Me.lblBirth.Text = ""
            Me.lblDecease.Text = ""
            Me.lblGeneration.Text = ""
            Me.lblName.Text = ""
            Me.lblRemark.Text = ""

            'Me.Width = clsDefine.MEMCARD_DETAIL_W
            'Me.Height = clsDefine.MEMCARD_DETAIL_H
            'Me.Height = xGetHeight()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xGetHeight
    ''' </summary>
    ''' <returns>Integer - Card height</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xGetHeight() As Integer

        'minimum is from 0 to top left of birth label
        Dim intHeight As Integer = lblBirth.Top

        Try
            'if birth or decease is available, the heigh is from 0 to topleft of remark label
            'If lblBirth.Visible Or lblDecease.Visible Then intHeight = lblRemark.Top
            If lblBirth.Visible Then intHeight = lblDecease.Top
            If lblDecease.Visible Then
                If Not lblBirth.Visible Then lblDecease.Top = lblBirth.Top
                intHeight = lblRemark.Top
            End If

            'if remark is available
            If lblRemark.Visible Then
                intHeight += lblRemark.Height + 5
            End If

            If Not Me.ShowAlias And memCardSize = clsEnum.emCardSize.SMALL Then
                intHeight -= lblAlias.Height
                lblBirth.Top -= lblAlias.Height
                lblDecease.Top -= lblAlias.Height
                lblRemark.Top -= lblAlias.Height
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetHeight", ex)
        End Try

        Return intHeight

    End Function


    ''' <summary>
    ''' xGetWidth
    ''' </summary>
    ''' <returns>Integer - Card weight</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xGetWidth() As Integer

        Dim intWidth As Integer = Me.Width

        Try
            Dim intMaxW As Integer = 0
            Dim intW As Integer = 0

            'find max width 
            For Each ctrl As Control In Me.Controls

                If ctrl.Visible = False Then Continue For
                If TypeOf ctrl Is PictureBox Then Continue For
                If ctrl Is lblRemark Then Continue For

                intW = ctrl.Location.X + ctrl.Width
                If intW > intMaxW Then intMaxW = intW

            Next

            intWidth = intMaxW

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetWidth", ex)
        End Try

        Return intWidth

    End Function


#End Region


#Region "FORM's EVENTs"


    ''' <summary>
    ''' usrMemberCard_MouseDown
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Mouse down event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub usrMemberCard_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown, picHead.MouseDown, picAvatar.MouseDown, lblRemark.MouseDown, lblName.MouseDown, lblGeneration.MouseDown, lblDecease.MouseDown, lblBirth.MouseDown, lblAlias.MouseDown

        Try

            RaiseEvent evnMouseDown(e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_MouseDown", ex)
        End Try

    End Sub


    ''' <summary>
    ''' usrMemberCard_MouseMove
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Mouse move event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub usrMemberCard_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove, picHead.MouseMove, picAvatar.MouseMove, lblRemark.MouseMove, lblName.MouseMove, lblGeneration.MouseMove, lblDecease.MouseMove, lblBirth.MouseMove, lblAlias.MouseMove

        Try

            '2016/12/20 Start Manh Stop this function
            'RaiseEvent evnMouseMove(e)
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
    Private Sub usrMemberCard_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp, picHead.MouseUp, picAvatar.MouseUp, lblRemark.MouseUp, lblName.MouseUp, lblGeneration.MouseUp, lblDecease.MouseUp, lblBirth.MouseUp, lblAlias.MouseUp

        Try
            RaiseEvent evnMouseUp(e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_MouseUp", ex)
        End Try

    End Sub


    ''' <summary>
    ''' usrMemberDetail_DoubleClick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Double click event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub usrMemberDetail_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.DoubleClick, picHead.DoubleClick, picAvatar.DoubleClick, lblRemark.DoubleClick, lblName.DoubleClick, lblGeneration.DoubleClick, lblDecease.DoubleClick, lblBirth.DoubleClick, lblAlias.DoubleClick

        Try
            Dim objEvent As MouseEventArgs

            objEvent = CType(e, MouseEventArgs)

            If objEvent.Button <> Windows.Forms.MouseButtons.Left Then Exit Sub

            RaiseEvent evnCardDoubleClick(mintID, Me)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard_DoubleClick", ex)
        End Try

    End Sub

    ''' <summary>
    ''' usrMemberDetail_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Click event</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub usrMemberDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click, picHead.Click, picAvatar.Click, lblRemark.Click, lblName.Click, lblGeneration.Click, lblDecease.Click, lblBirth.Click, lblAlias.Click

        Try
            Dim objEvent As MouseEventArgs

            objEvent = CType(e, MouseEventArgs)

            If objEvent.Button <> Windows.Forms.MouseButtons.Left Then Exit Sub

            RaiseEvent evnCardClick(mintID, Me)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberDetail_Click", ex)
        End Try

    End Sub

    Private Sub usrMemberDetail_MouseHover(sender As Object, e As EventArgs) Handles MyBase.MouseHover
        Me.Cursor = Cursors.Hand
    End Sub

#End Region


End Class
