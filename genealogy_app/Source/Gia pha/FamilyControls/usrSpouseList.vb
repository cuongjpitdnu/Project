'   ******************************************************************
'      TITLE      : USER CONTROL SPOUSE LIST
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2012/11/20　AKB Quyet
'      UPDATE     : 
'
'           2012 AKB SOFTWARE
'   ******************************************************************

''' <summary>
''' Spouse List Class
''' </summary>
''' <remarks></remarks>
Public Class usrSpouseList

    Private Const mcstrClsName As String = "clsDrawCard"        'class name
    Private Const mcintDefaultTop As Integer = 25               'margin top
    Private Const mcintItemPerCol As Integer = 4                'default item per column
    Private Const mcintCol1Left As Integer = 15                 'default left value of column 1
    Private Const mcintCol2Left As Integer = 180                'default left value of column 2
    Private Const mcstrAddWife As String = "Thêm vợ"             'add wife text
    Private Const mcstrAddHus As String = "Thêm chồng"            'add husband text

    Private Const mcstrTitle1 As String = "Thành viên là vợ  của ông {0} :"
    Private Const mcstrTitle2 As String = "Thành viên là chồng của bà {0} :"
    Private mstrAddWifeText() As String = {"Thêm vợ", "Thêm vợ thứ hai", "Thêm vợ ba", "Thêm vợ thứ tư", "Thêm vợ thứ năm", "Thêm vợ thứ sáu", "Thêm vợ thứ bảy", "Thêm vợ thứ tám", "Thêm vợ thứ chín", "Thêm vợ thứ mười", _
                                           "Thêm vợ thứ mười một", "Thêm vợ thứ mười hai", "Thêm vợ thứ mười ba", "Thêm vợ thứ mười bốn", "Thêm vợ thứ mười năm", "Thêm vợ thứ mười sáu", "Thêm vợ thứ mười bảy", "Thêm vợ thứ mười tám", "Thêm vợ thứ mười chín", "Thêm vợ thứ hai mươi"}

    Private mobjAddItem As usrSpouseItem                        'add spouse item
    Private mstrAddItemText As String                           'add spouse text

    Private memListGender As clsEnum.emGender                   'spouse's gender
    Private mstrActiveMemName As String                         'active member name

    Private mlstItem As List(Of usrSpouseItem)                  'list of items

    Public Event evnSpouseChange(ByVal intID As Integer)
    Public Event evnAddMember()
    Public Event evnSubLinkClicked()
    Public Event evnSpouseListClicked()


    ''' <summary>
    ''' Property SpouseList
    ''' </summary>
    ''' <value></value>
    ''' <returns>List of item</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SpouseList() As List(Of usrSpouseItem)
        Get
            Return mlstItem
        End Get
    End Property


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mlstItem = New List(Of usrSpouseItem)

    End Sub


    ''' <summary>
    ''' Reset value and redraw
    ''' </summary>
    ''' <param name="emGender"></param>
    ''' <param name="strActiveMemberName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncReset(ByVal emGender As clsEnum.emGender, ByVal strActiveMemberName As String) As Boolean
        fncReset = False

        Try
            'set value and clear previous items
            Me.memListGender = emGender
            Me.mstrActiveMemName = strActiveMemberName

            For index As Integer = 0 To Me.mlstItem.Count - 1
                mlstItem(index).Dispose()
            Next

            mlstItem.Clear()

            mstrAddItemText = mcstrAddHus
            If emGender = clsEnum.emGender.FEMALE Then mstrAddItemText = mcstrAddWife

            lblTitle.Text = String.Format(mcstrTitle2, strActiveMemberName)
            If emGender = clsEnum.emGender.FEMALE Then lblTitle.Text = String.Format(mcstrTitle1, strActiveMemberName)

            lnkSpouseList.Left = lblTitle.Left + lblTitle.Width

            xInit()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncReset", ex)
        End Try
    End Function


    ''' <summary>
    ''' Initialize Component 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xInit() As Boolean

        xInit = False

        Try
            Me.Width = clsDefine.CARD_LARG_W
            mlstItem = New List(Of usrSpouseItem)

            'the first item is add new
            mobjAddItem = New usrSpouseItem(Me.memListGender, mstrAddItemText)
            Me.Controls.Add(mobjAddItem)
            mlstItem.Add(mobjAddItem)
            AddHandler mobjAddItem.evnAddNew, AddressOf xAddMember
            AddHandler mobjAddItem.evnSubLinkClicked, AddressOf xSubLinkClicked

            xAlignItems()
            xDefineAddItem()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)
        End Try

    End Function


    ''' <summary>
    ''' Additem to list
    ''' </summary>
    ''' <param name="emGender"></param>
    ''' <param name="intID"></param>
    ''' <param name="strName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncAddItem(ByVal emGender As clsEnum.emGender, ByVal intID As Integer, ByVal strName As String) As Boolean

        fncAddItem = False

        Try
            Dim objItem As usrSpouseItem

            objItem = New usrSpouseItem(emGender, intID, strName)

            Me.Controls.Add(objItem)
            mlstItem.Insert(mlstItem.Count - 1, objItem)
            AddHandler objItem.evnSpouseClicked, AddressOf xSpouseChange

            xAlignItems()
            xDefineAddItem()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncAddItem", ex)
        End Try

    End Function


    ''' <summary>
    ''' Aligning items
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xAlignItems() As Boolean

        xAlignItems = False

        Try
            Dim intItemPerCol As Integer
            Dim intLeft As Integer
            Dim intTop As Integer

            'init value
            intItemPerCol = Math.Ceiling(mlstItem.Count / 2)
            If intItemPerCol <= 3 Then intItemPerCol = mcintItemPerCol

            intLeft = mcintCol1Left
            intTop = mcintDefaultTop

            For i As Integer = 0 To mlstItem.Count - 1

                'reset value
                If i = intItemPerCol Then
                    intLeft = mcintCol2Left
                    intTop = mcintDefaultTop
                End If

                mlstItem(i).Top = intTop + 2
                mlstItem(i).Left = intLeft

                intTop += mlstItem(i).Height

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAlignItems", ex)
        End Try

    End Function


    ''' <summary>
    ''' Change text of Add-item
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xDefineAddItem() As Boolean

        xDefineAddItem = False

        Try
            'define for wife only
            If memListGender <> clsEnum.emGender.FEMALE Then Exit Function

            If mlstItem.Count > mstrAddWifeText.Length Then
                'number of wife is out of pre-define array
                mstrAddItemText = mcstrAddWife
            Else
                mstrAddItemText = mstrAddWifeText(mlstItem.Count - 1)
            End If

            'set text
            mobjAddItem.MemberName = mstrAddItemText

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDefineAddItem", ex)
        End Try

    End Function


    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub xAddMember()
        Try
            RaiseEvent evnAddMember()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddMember", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub xSubLinkClicked()
        Try
            RaiseEvent evnSubLinkClicked()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSubLinkClicked", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="intID">member id</param>
    ''' <remarks></remarks>
    Private Sub xSpouseChange(ByVal intID As Integer)
        Try
            RaiseEvent evnSpouseChange(intID)
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSpouseChange", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lnkSpouseList_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkSpouseList.LinkClicked
        Try
            RaiseEvent evnSpouseListClicked()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "lnkSpouseList_LinkClicked", ex)
        End Try
    End Sub
End Class
