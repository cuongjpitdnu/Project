'   ******************************************************************
'      TITLE      : USER CONTROL SPOUSE ITEM
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2012/11/20　AKB Quyet
'      UPDATE     : 
'
'           2012 AKB SOFTWARE
'   ******************************************************************


''' <summary>
''' Spouse item class
''' </summary>
''' <remarks></remarks>
Public Class usrSpouseItem

    Private Const mcstrClsName As String = "clsDrawCard"        'class name

    Private memGender As clsEnum.emGender                       'member gender  
    Private mintID As Integer                                   'member id
    Private mstrName As String                                  'member name
    Private mblnAddMode As Boolean

    Public Event evnSpouseClicked(ByVal intID As Integer)         'event when link is clicked
    Public Event evnAddNew()
    Public Event evnSubLinkClicked()


    ''' <summary>
    ''' Gender - Get Set member gender
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property MemberGender() As clsEnum.emGender
        Get
            Return memGender
        End Get
        Set(ByVal value As clsEnum.emGender)
            memGender = value

            'reset avatar
            picAvatar.Image = My.Resources.Gender_man16
            If memGender = clsEnum.emGender.FEMALE Then
                picAvatar.Image = My.Resources.Gender_woman16
            End If

            If mblnAddMode Then picAvatar.Image = My.Resources.plus16

        End Set
    End Property


    ''' <summary>
    ''' MemberID - Get Set member ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property MemberID() As Integer
        Get
            Return mintID
        End Get
        Set(ByVal value As Integer)
            mintID = value
        End Set
    End Property


    ''' <summary>
    ''' MemberName - Get Set member name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property MemberName() As String
        Get
            Return mstrName
        End Get
        Set(ByVal value As String)
            mstrName = value

            xInit()

        End Set
    End Property


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        xInit()

    End Sub


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <param name="emGender"></param>
    ''' <param name="intId"></param>
    ''' <param name="strName"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal emGender As clsEnum.emGender, ByVal intId As Integer, ByVal strName As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.memGender = emGender
        Me.mintID = intId
        Me.mstrName = strName
        Me.mblnAddMode = False

        xInit()

    End Sub


    ''' <summary>
    ''' CONSTRUCTOR - form in ADD_NEW mode
    ''' </summary>
    ''' <param name="emGender">member gender</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal emGender As clsEnum.emGender, ByVal strText As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.memGender = emGender
        Me.mstrName = strText
        Me.mblnAddMode = True
        Me.lnkSubname.Visible = True

        xInit()

    End Sub


    ''' <summary>
    ''' Initialize Component 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xInit() As Boolean

        xInit = False

        Try
            'avatar
            picAvatar.Image = My.Resources.Gender_man16
            If memGender = clsEnum.emGender.FEMALE Then
                picAvatar.Image = My.Resources.Gender_woman16
            End If

            If mblnAddMode Then picAvatar.Image = My.Resources.plus16

            'name
            lnkName.Text = mstrName


            If lnkSubname.Visible Then
                lnkSubname.Left = lnkName.Left + lnkName.Width
                Me.Width = lnkSubname.Left + lnkSubname.Width + 5
            Else
                Me.Width = lnkName.Left + lnkName.Width + 5
            End If


            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)
        End Try

    End Function


    ''' <summary>
    ''' Link Clicked event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lnkName_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkName.LinkClicked
        Try
            If mblnAddMode Then
                RaiseEvent evnAddNew()
            Else
                RaiseEvent evnSpouseClicked(mintID)
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "lnkName_LinkClicked", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Sub Link Clicked event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lnkSubname_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkSubname.LinkClicked
        Try
            If mblnAddMode Then
                RaiseEvent evnSubLinkClicked()
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "lnkSubname_LinkClicked", ex)
        End Try
    End Sub

End Class
