Option Explicit On
Option Strict On

Imports System.Windows.Forms
Imports System.Drawing
'Imports FamilyCommon

Public Class FamilyCard
    Implements IDisposable

    Public Event LabelClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs, ByVal intID As Integer, ByVal objCard As FamilyCard)
    Public Event CardClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs, ByVal intID As Integer)
    Public Event ManageChildClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
    Public Event CardDragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs, ByVal intID As Integer, ByVal strLabel As String)
    Public Event ImageClick(ByVal objCard As FamilyCard, ByVal intID As Integer)
    Public Event DelSpouseRel()

#Region "Constants"

    Private Const mcstrClsName As String = "FamilyCard"                                          'class name
    Private Const mcstrChild As String = "Có {0,2} người con"
    Private Const mcstrAliasFormat As String = "( {0} )"

#End Region

#Region "Control variable"

    Private mfrmImgMan As FamilyForm

    'Person imformation form
    Private mfrmPerInfo As FamilyForm

    'Person picture in database
    Private mstrPerPic As String

    'Card Gender
    Private menmPerGender As clsEnum.emGender

    'Card Size
    Private menmCardSize As clsEnum.emCardSize
    Private mszCurSize As Size

    'Card Mode
    Private mblnIsActiveCard As Boolean
    'Private mblnIsChildsCard As Boolean

    Private mintTotalChilds As Integer
    Private mintTotalSpouse As Integer

    'Card ID relation
    Private mintCurID As Integer
    Private mintRootID As Integer
    Private mintRootSubID As Integer


#End Region

#Region "Control Properties"

    Public WriteOnly Property PersonInfoDlg() As FamilyForm

        Set(ByVal value As FamilyForm)
            Me.mfrmPerInfo = value
        End Set

    End Property

    Public WriteOnly Property PersonImgDlg() As FamilyForm

        Set(ByVal value As FamilyForm)
            Me.mfrmImgMan = value
        End Set

    End Property

    Public Property PersonName() As String
        Get
            Return lnkName.Text
        End Get

        Set(ByVal value As String)
            lnkName.Text = value
        End Set

    End Property

    Public Property PersonAlias() As String
        Get
            Return lblAlias.Text

        End Get

        Set(ByVal value As String)

            lblAlias.Text = ""

            If basCommon.fncIsBlank(value) Then Exit Property

            lblAlias.Text = String.Format(mcstrAliasFormat, value)

        End Set

    End Property

    Public Property PersonPicLocate() As String

        Get
            Return Me.mstrPerPic
        End Get

        Set(ByVal value As String)
            Me.mstrPerPic = value
            picUserCard.ImageLocation = mstrPerPic
        End Set

    End Property

    Public Property PersonPicImage() As Image

        Get
            Return picUserCard.Image
        End Get

        Set(ByVal value As Image)
            picUserCard.Image = value
            'picUserCard.Refresh()
        End Set

    End Property

    Public Property PersonBirth() As String
        Get
            Return lblBirth.Text
        End Get
        Set(ByVal value As String)
            lblBirth.Text = value
        End Set
    End Property

    Public Property PersonBirthDie() As String
        Get
            Return lblBirthDie.Text
        End Get
        Set(ByVal value As String)
            lblBirthDie.Text = value
        End Set
    End Property

    Public Property PersonLunarBirth() As String
        Get
            Return lblLunarBirth.Text
        End Get
        Set(ByVal value As String)
            lblLunarBirth.Text = value
        End Set
    End Property

    Public Property PersonBirhPlace() As String
        Get
            Return lblBirthPlace.Text
        End Get
        Set(ByVal value As String)
            lblBirthPlace.Text = value
        End Set
    End Property

    Public Property PersonHome() As String
        Get
            Return lblHometown.Text
        End Get
        Set(ByVal value As String)
            lblHometown.Text = value
        End Set
    End Property

    Public Property PersonRelation() As String
        Get
            Return lblRelation.Text
        End Get
        Set(ByVal value As String)
            lblRelation.Text = value
        End Set
    End Property

    Public Property PersonGender() As clsEnum.emGender

        Get
            Return Me.menmPerGender
        End Get
        Set(ByVal value As clsEnum.emGender)
            Me.menmPerGender = value
            CardGenderChange()
        End Set

    End Property

    Public Property PersonStatus() As String

        Get
            Return lblStatus.Text
        End Get

        Set(ByVal value As String)
            lblStatus.Text = value
        End Set

    End Property

    Public Property PersonSize() As clsEnum.emCardSize

        Get
            Return menmCardSize
        End Get
        Set(ByVal value As clsEnum.emCardSize)
            Me.menmCardSize = value
            CardSizeChange()
        End Set

    End Property

    Public Property PersonActice() As Boolean
        Get
            Return Me.mblnIsActiveCard
        End Get
        Set(ByVal value As Boolean)
            Me.mblnIsActiveCard = value
        End Set
    End Property

    Public Property PersonTotalChilds() As Integer
        Get
            Return Me.mintTotalChilds
        End Get
        Set(ByVal value As Integer)
            Me.mintTotalChilds = value
            Me.lblChildsList.Text = String.Format(mcstrChild, Me.mintTotalChilds)
        End Set
    End Property

    Public Property PersonTotalSpouse() As Integer
        Get
            Return Me.mintTotalSpouse
        End Get
        Set(ByVal value As Integer)
            Me.mintTotalSpouse = value
        End Set
    End Property

    Public Property PersonID() As Integer
        Get
            Return Me.mintCurID
        End Get
        Set(ByVal value As Integer)
            Me.mintCurID = value
        End Set
    End Property

    Public Property PersonRootID() As Integer
        Get
            Return Me.mintRootID
        End Get
        Set(ByVal value As Integer)
            Me.mintRootID = value
        End Set
    End Property

    Public Property PersonRootSubID() As Integer
        Get
            Return Me.mintRootSubID
        End Get
        Set(ByVal value As Integer)
            Me.mintRootSubID = value
        End Set
    End Property


#End Region

#Region "Control Function"

    Private Sub CardLinkChange()
        Try
            'Me.lblName.Location = Me.lnkName.Location
            Me.lblStatusCap.Top = Me.lnkName.Top
            Me.lblStatus.Top = Me.lblStatusCap.Top

            Select Case Me.menmCardSize

                Case clsEnum.emCardSize.LARGE

                Case clsEnum.emCardSize.MIDDLE

                Case clsEnum.emCardSize.SMALL

                Case clsEnum.emCardSize.MINI
                    Me.lnkName.Text = Global.GiaPha.My.Resources.Resources.StrAddFather
                    If Me.menmPerGender = clsEnum.emGender.FEMALE Then Me.lnkName.Text = Global.GiaPha.My.Resources.Resources.StrAddMother
                    If Me.menmPerGender = clsEnum.emGender.UNKNOW Then Me.lnkName.Text = Global.GiaPha.My.Resources.Resources.StrAddChild

                Case clsEnum.emCardSize.ORTHER
                    Me.lnkName.Visible = False
                    Me.lblChildsList.Visible = True
                    Me.lblStatusCap.Visible = True
                    Me.lblStatus.Visible = True
                    Me.lnkChildMan.Visible = True
                    Me.btnDelSpouseRel.Visible = True

            End Select

            If Me.mblnIsActiveCard Then
                Me.lnkName.Visible = False
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CardPicChange()
        Try
            Me.picUserCard.Top = clsDefine.SPEC_CARD_MIN_TOP
            Me.picUserCard.Left = clsDefine.SPEC_CARD_MIN_LEFT

            If Me.menmCardSize = clsEnum.emCardSize.ORTHER Then Me.BackgroundImage = Nothing

            CardLinkChange()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CardGenderChange()

        Try

            Me.BackgroundImage = Global.GiaPha.My.Resources.Resources.BgMale

            If Me.menmPerGender = clsEnum.emGender.FEMALE Then Me.BackgroundImage = Global.GiaPha.My.Resources.Resources.BgFemale
            If Me.menmPerGender = clsEnum.emGender.UNKNOW Then Me.BackgroundImage = Nothing

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub CardSizeChange()

        Dim szCardSize As Size, szPicSize As Size

        Try
            Me.picUserCard.Visible = True

            Select Case Me.menmCardSize

                Case clsEnum.emCardSize.LARGE
                    szCardSize = New Size(clsDefine.CARD_LARG_W, clsDefine.CARD_LARG_H)
                    szPicSize = New Size(clsDefine.PIC_LARG_W, clsDefine.PIC_LARG_H)

                    'hide some controls
                    lblTextBirth.Visible = True
                    lblAlias.Visible = True

                    'align name
                    Me.lnkName.Top = clsDefine.SPEC_CARD_MIN_TOP
                    Me.lnkName.Left = clsDefine.MARGIN_LARG_LEFT
                    Me.lnkName.Font = New Font("Arial", 12, FontStyle.Bold)

                    'align birth-die
                    Me.lblBirthDie.Top = clsDefine.SPEC_CARD_MIN_TOP * 3 + clsDefine.PIC_LARG_H
                    Me.lblBirthDie.Left = clsDefine.SPEC_CARD_MIN_LEFT * 3

                Case clsEnum.emCardSize.MIDDLE
                    szCardSize = New Size(clsDefine.CARD_MIDD_W, clsDefine.CARD_MIDD_H)
                    szPicSize = New Size(clsDefine.PIC_MIDD_W, clsDefine.PIC_MIDD_H)

                    'hide some controls
                    lblTextBirth.Visible = False
                    'lblBirth.Visible = False
                    'lblAlias.Visible = True
                    lblLunarBirth.Visible = False

                    'align 
                    Me.lnkName.Top = clsDefine.SPEC_CARD_MIN_TOP
                    Me.lnkName.Left = clsDefine.MARGIN_MID_LEFT
                    Me.lnkName.Font = New Font("Arial", 10, FontStyle.Bold)


                    Me.lblAlias.Visible = True
                    Me.lblAlias.Top = clsDefine.SPEC_CARD_MIN_TOP + 20
                    Me.lblAlias.Left = clsDefine.MARGIN_MID_LEFT
                    lblAlias.Font = New Font("Arial", 8, FontStyle.Regular)

                    'Me.lblTextBirth.Top = clsDefine.SPEC_CARD_MIN_TOP * 10
                    'Me.lblTextBirth.Left = clsDefine.MARGIN_MID_LEFT
                    Me.lblBirth.Top = clsDefine.SPEC_CARD_MIN_TOP * 10
                    Me.lblBirth.Left = clsDefine.MARGIN_MID_LEFT
                    Me.lblBirth.Font = New Font("Arial", 8, FontStyle.Regular)

                    'Me.lblTextBirthPlace.Top = clsDefine.SPEC_CARD_MIN_TOP * 14
                    'Me.lblTextBirthPlace.Left = clsDefine.MARGIN_MID_LEFT
                    'Me.lblBirthPlace.Top = clsDefine.SPEC_CARD_MIN_TOP * 14
                    'Me.lblBirthPlace.Left = clsDefine.MARGIN_MID_LEFT

                    'Me.lnkTextHometown.Top = clsDefine.SPEC_CARD_MIN_TOP * 18
                    'Me.lnkTextHometown.Left = clsDefine.MARGIN_MID_LEFT
                    Me.lblHometown.Top = clsDefine.SPEC_CARD_MIN_TOP * 14
                    Me.lblHometown.Left = clsDefine.MARGIN_MID_LEFT
                    Me.lblHometown.Font = New Font("Arial", 8, FontStyle.Regular)

                    'align birth-die
                    Me.lblBirthDie.Top = clsDefine.SPEC_CARD_MIN_TOP * 2 + clsDefine.PIC_MIDD_H
                    Me.lblBirthDie.Left = clsDefine.SPEC_CARD_MIN_LEFT
                    Me.lblBirthDie.Font = New Font("Arial", 8, FontStyle.Regular)

                Case clsEnum.emCardSize.SMALL
                    szCardSize = New Size(clsDefine.CARD_SMAL_W, clsDefine.CARD_SMAL_H)
                    szPicSize = New Size(clsDefine.PIC_SMAL_W, clsDefine.PIC_SMAL_H)

                    lblAlias.Visible = True
                    lblAlias.Location = New Point(49, 30)

                    Me.lnkName.Top = clsDefine.SPEC_CARD_MIN_TOP
                    Me.lnkName.Left = clsDefine.MARGIN_SMAL_LEFT
                    Me.lnkName.Font = New Font("Arial", 10, FontStyle.Bold)
                    Me.lblAlias.Font = New Font("Arial", 8, FontStyle.Regular)

                Case clsEnum.emCardSize.MINI
                    szCardSize = New Size(clsDefine.CARD_MINI_W, clsDefine.CARD_MINI_H)

                    Me.lnkName.Top = clsDefine.SPEC_CARD_MIN_TOP * 2
                    Me.lnkName.Left = clsDefine.SPEC_CARD_MIN_LEFT
                    Me.lnkName.Font = New Font("Arial", 10, FontStyle.Bold)
                    Me.lblAlias.Visible = False

                Case clsEnum.emCardSize.ORTHER
                    szCardSize = New Size(clsDefine.CARD_ADD_W, clsDefine.CARD_ADD_H)
                    Me.lblAlias.Visible = False

            End Select

            Me.picUserCard.Size = szPicSize

            Me.mszCurSize = szCardSize
            Me.Size = Me.mszCurSize

            '=============== EDIT ===================
            Me.picUserCard.Top = clsDefine.SPEC_CARD_MIN_TOP
            Me.picUserCard.Left = clsDefine.SPEC_CARD_MIN_LEFT

        Catch ex As Exception
            Throw ex
        End Try

        szCardSize = Nothing
        szPicSize = Nothing

    End Sub


    Private Function SetUserImg(ByVal viUserID As Integer) As Boolean

        Dim blnRst As Boolean = False

        Try

            Dim strPath As String = String.Empty
            'strPath = My.Resources.PathUserImg & viUserID.ToString() & ".jpg"

            'If Not Me.mobjCommon.IsHaveFile(strPath) Then Exit Try

            If Not System.IO.File.Exists(strPath) Then Exit Try

            Me.picUserCard.Image = System.Drawing.Image.FromFile(strPath, True)
            Me.picUserCard.SizeMode = PictureBoxSizeMode.StretchImage


            blnRst = True

        Catch ex As Exception
            Throw ex

        End Try

        Return blnRst

    End Function


#End Region

#Region "Control constructor"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 375x195

        'Control's objects
        Me.mstrPerPic = String.Empty
        Me.mfrmImgMan = Nothing
        Me.mfrmPerInfo = Nothing

        'Gender
        'Me.memPerGender = clsEnum.Gender.MALE
        PersonGender = clsEnum.emGender.MALE

        'Card size
        'Me.CardSize = clsEnum.CardSize.LARGE
        Me.PersonSize = clsEnum.emCardSize.LARGE

        'Card mode
        Me.mblnIsActiveCard = False

        'Person's childs and spouse number
        Me.mintTotalSpouse = 0
        Me.mintTotalChilds = 0

        'Person ID and Person's parent ID
        Me.mintCurID = clsDefine.NONE_VALUE
        Me.mintRootID = clsDefine.NONE_VALUE
        Me.mintRootSubID = clsDefine.NONE_VALUE

        'xAddHandler(Me)
        'AddHandler mnuPopup.Opening, AddressOf popupMenuOpening

        lblBirthPlace.Text = String.Empty
        lblHometown.Text = String.Empty
    End Sub

    Protected Overrides Sub Finalize()

        MyBase.Finalize()

    End Sub

#End Region

#Region "Control Event"

    Protected Overrides Sub OnCreateControl()
        Try
            CardPicChange()

        Catch ex As Exception
            Throw ex
        End Try

        MyBase.OnCreateControl()
    End Sub

    'Make fix size for control
    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As System.Windows.Forms.BoundsSpecified)
        'MyBase.SetBoundsCore(x, y, width, height, specified)
        MyBase.SetBoundsCore(x, y, mszCurSize.Width, mszCurSize.Height, specified)
    End Sub

    Private Sub lnkName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkName.Click

        Try

            Dim mouseEvent As System.Windows.Forms.MouseEventArgs
            mouseEvent = CType(e, System.Windows.Forms.MouseEventArgs)

            RaiseEvent LabelClick(sender, mouseEvent, Me.mintCurID, Me)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub FamilyCard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click

        Try
            Dim mouseEvent As System.Windows.Forms.MouseEventArgs
            mouseEvent = CType(e, System.Windows.Forms.MouseEventArgs)

            RaiseEvent CardClick(sender, mouseEvent, Me.mintCurID)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub lnkChildMan_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkChildMan.LinkClicked

        Try

            RaiseEvent ManageChildClick(sender, e)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub FamilyCard_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop

        Try
            RaiseEvent CardDragDrop(sender, e, Me.mintCurID, lnkName.Text)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub picUserCard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picUserCard.Click

        Try
            RaiseEvent ImageClick(Me, Me.mintCurID)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Private Function xAddHandler(ByVal objCtrl As Control) As Boolean

    '    xAddHandler = False

    '    Try

    '        For Each ctrChild As Control In objCtrl.Controls

    '            If TypeOf ctrChild Is Label Then _
    '                AddHandler ctrChild.Click, AddressOf xLabelClick

    '            If TypeOf ctrChild Is LinkLabel Then _
    '                AddHandler ctrChild.Click, AddressOf lnkName_Click

    '            xAddHandler(ctrChild)

    '        Next

    '        Return True

    '    Catch ex As Exception

    '        basCommon.fncSaveErr(mcstrClsName, "xAddHandler", ex)
    '        Return Nothing

    '    End Try

    'End Function

    Private Sub xLabelClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAlias.Click, lblBirth.Click, lblBirthDie.Click, lblBirthPlace.Click, lblHometown.Click, lblLunarBirth.Click, lblStatus.Click, lblTextBirth.Click, lblTextBirthPlace.Click, lblTextHometown.Click, lblRelation.Click

        Try
            Dim mouseEvent As System.Windows.Forms.MouseEventArgs
            mouseEvent = CType(e, System.Windows.Forms.MouseEventArgs)

            RaiseEvent CardClick(Me, mouseEvent, Me.mintCurID)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    ''' <summary>
    ''' btnDelSpouseRel_Click - button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelSpouseRel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelSpouseRel.Click
        Try
            RaiseEvent DelSpouseRel()

        Catch ex As Exception

            Throw ex

        End Try
    End Sub

#End Region


End Class
