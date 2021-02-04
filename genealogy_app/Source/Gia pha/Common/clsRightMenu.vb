'   ****************************************************************** 
'      TITLE      : RIGHT MENU
'　　　FUNCTION   :  
'      MEMO       :  
'      CREATE     : 2012/02/17　AKB　Quyet 
'      UPDATE     :  
' 
'           2012 AKB SOFTWARE 
'   ******************************************************************
Option Explicit On
Option Strict On

''' <summary>
''' Create Right Menu 
''' </summary>
''' <remarks></remarks>
''' <Create>2012/02/17  AKB Quyet</Create>
Public Class clsRightMenu


    Private Const mcstrClsName As String = "clsRightMenu"                           'class name

    Private mintID As Integer                                                       'member id
    Private mstrFullName As String                                                  'member name
    Private memGender As clsEnum.emGender                                           'gender

    Private mmnuRightMouse As ContextMenuStrip                                      'right mouse menu on active member card
    Private mmniHusWif As ToolStripMenuItem

    'Public Event evnShowPerInfo(ByVal intMemId As Integer)
    Public Event evnSpouseChange(ByVal intSpouseID As Integer)
    Public Event evnMenuItemClick(ByVal intMemId As Integer, ByVal sender As System.Object)


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Public Sub New()
        mmnuRightMouse = New ContextMenuStrip()
    End Sub


    ''' <summary>
    ''' fncGetMenu - Gets context menu
    ''' </summary>
    ''' <param name="intMemID">Integer - member id</param>
    ''' <param name="strFullName">String - member name</param>
    ''' <param name="emGender">clsEnum.emGender - gender</param>
    ''' <param name="blnIsCardMenu">Boolean - menu for card or grid</param>
    ''' <returns>ContextMenuStrip</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Public Function fncGetMenu(ByVal intMemID As Integer, ByVal strFullName As String, ByVal emGender As clsEnum.emGender, Optional ByVal blnIsCardMenu As Boolean = True) As ContextMenuStrip

        Try
            Me.mintID = intMemID
            Me.mstrFullName = strFullName
            Me.memGender = emGender

            mmnuRightMouse.Items.Clear()

            If intMemID <= basConst.gcintNO_MEMBER Then Return mmnuRightMouse

            xMenuShowPerInfo(blnIsCardMenu)

            'menu for grid does not show these items
            If blnIsCardMenu Then

                xMenuFatherMother()
                xMenuHusWife()
                xMenuBrother()
                xMenuChildren()

            End If

            xMenuRootHeadDelete()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncGetCardMenu", ex)
        End Try

        Return mmnuRightMouse

    End Function


    ''' <summary>
    ''' xMenuShowPerInfo - create sub menu
    ''' </summary>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub xMenuShowPerInfo(ByVal blnIsCardMenu As Boolean)

        Try
            Dim mniPerInfo As ToolStripMenuItem                         'show person info
            Dim mniRefeshLv As ToolStripMenuItem                        'refresh member generation
            Dim mniSeparator As ToolStripSeparator

            mniPerInfo = New ToolStripMenuItem(My.Resources.StrPersonInfo)
            mniRefeshLv = New ToolStripMenuItem(My.Resources.StrRefreshGeneration)
            mniSeparator = New ToolStripSeparator()

            mniPerInfo.Image = GiaPha.My.Resources.MemberInfo32
            mniRefeshLv.Image = GiaPha.My.Resources.refresh32

            'add show person info and a separator line
            mmnuRightMouse.Items.Add(mniPerInfo)
            If Not blnIsCardMenu Then mmnuRightMouse.Items.Add(mniRefeshLv)
            mmnuRightMouse.Items.Add(mniSeparator)

            AddHandler mniPerInfo.Click, AddressOf xItemClick
            AddHandler mniRefeshLv.Click, AddressOf xItemClick

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMenuShowPerInfo", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xMenuFatherMother - create sub menu
    ''' </summary>
    ''' <param name="blnShowAddnew"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub xMenuFatherMother(Optional ByVal blnShowAddnew As Boolean = True)

        Dim tblData As DataTable = Nothing

        Try
            'Dim intFaID As Integer
            'Dim intMoID As Integer

            Dim mniFaMo As ToolStripMenuItem                           'title
            'Dim mniTitle As ToolStripMenuItem                           'title
            Dim mniFather As ToolStripMenuItem = Nothing                'father name
            Dim mniMother As ToolStripMenuItem = Nothing                'mothername

            Dim mniAddFa As ToolStripMenuItem                           'add new father
            Dim mniAddMo As ToolStripMenuItem                           'add new mother
            Dim mniAddAF As ToolStripMenuItem                           'add new adopt father
            Dim mniAddAM As ToolStripMenuItem                           'add new adopt mother
            Dim mniAddFML As ToolStripMenuItem                          'add new father mother from list
            Dim mniDelFM As ToolStripMenuItem                           'delete relationship

            'Dim mniSeparator1 As ToolStripSeparator
            Dim mniSeparator2 As ToolStripSeparator
            Dim mniSeparator3 As ToolStripSeparator

            mniFaMo = New ToolStripMenuItem(My.Resources.StrFaMo)
            mniFaMo.Image = GiaPha.My.Resources.TreeView32

            'mniTitle = New ToolStripMenuItem(String.Format("Cha mẹ của {0}", mstrFullName))
            'mniSeparator1 = New ToolStripSeparator()

            'basCommon.fncGetFaMoID(mintID, intFaID, intMoID)

            'If intFaID > basConst.gcintNO_MEMBER Then
            '    mniFather = New ToolStripMenuItem(String.Format("Cha: {0}", basCommon.fncGetMemberName(intFaID)))
            '    mniFather.Image = GiaPha.My.Resources.user_male_white_blue_black
            '    AddHandler mniFather.Click, AddressOf xShowInfo
            'End If

            'If intMoID > basConst.gcintNO_MEMBER Then
            '    mniMother = New ToolStripMenuItem(String.Format("Mẹ: {0}", basCommon.fncGetMemberName(intMoID)))
            '    mniMother.Image = GiaPha.My.Resources.user_female_white_pink_brown
            '    AddHandler mniMother.Click, AddressOf xShowInfo
            'End If

            ''add sub menu
            'mniFaMo.DropDownItems.Add(mniTitle)
            'mniFaMo.DropDownItems.Add(mniSeparator1)
            'If mniFather IsNot Nothing Then mniFaMo.DropDownItems.Add(mniFather)
            'If mniMother IsNot Nothing Then mniFaMo.DropDownItems.Add(mniMother)

            'no need to show menu "add new"
            'If Not blnShowAddnew Then 

            'create new
            mniAddFa = New ToolStripMenuItem(My.Resources.StrAddFather)
            mniAddMo = New ToolStripMenuItem(My.Resources.StrAddMother)
            mniAddAF = New ToolStripMenuItem(My.Resources.StrAddAdoptFather)
            mniAddAM = New ToolStripMenuItem(My.Resources.StrAddAdoptMother)
            mniAddFML = New ToolStripMenuItem(My.Resources.StrAddFaMoFromList)
            mniDelFM = New ToolStripMenuItem(My.Resources.StrDelFaMoRel)
            mniSeparator2 = New ToolStripSeparator()
            mniSeparator3 = New ToolStripSeparator()

            'image
            mniAddFa.Image = GiaPha.My.Resources.user_male_white_blue_black
            mniAddAF.Image = GiaPha.My.Resources.user_male_white_blue_black
            mniAddMo.Image = GiaPha.My.Resources.user_female_white_pink_brown
            mniAddAM.Image = GiaPha.My.Resources.user_female_white_pink_brown
            mniAddFML.Image = GiaPha.My.Resources.MemberSearch32
            mniDelFM.Image = GiaPha.My.Resources.Cancel

            'sub menu
            'mniFaMo.DropDownItems.Add(mniSeparator2)
            mniFaMo.DropDownItems.Add(mniAddFa)
            mniFaMo.DropDownItems.Add(mniAddMo)
            mniFaMo.DropDownItems.Add(mniAddAF)
            mniFaMo.DropDownItems.Add(mniAddAM)
            mniFaMo.DropDownItems.Add(mniAddFML)
            mniFaMo.DropDownItems.Add(mniSeparator3)
            mniFaMo.DropDownItems.Add(mniDelFM)

            AddHandler mniAddFa.Click, AddressOf xItemClick
            AddHandler mniAddMo.Click, AddressOf xItemClick
            AddHandler mniAddAF.Click, AddressOf xItemClick
            AddHandler mniAddAM.Click, AddressOf xItemClick
            AddHandler mniAddFML.Click, AddressOf xItemClick
            AddHandler mniDelFM.Click, AddressOf xItemClick

            'End If

            'add menu
            mmnuRightMouse.Items.Add(mniFaMo)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMenuFatherMother", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' xMenuHusWife - create sub menu
    ''' </summary>
    ''' <param name="blnShowAddnew"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub xMenuHusWife(Optional ByVal blnShowAddnew As Boolean = True)

        Dim objDict As Dictionary(Of Integer, String) = Nothing

        Try
            Dim mniTitle As ToolStripMenuItem
            Dim mniAddSpouse As ToolStripMenuItem
            Dim mniAddSpouseList As ToolStripMenuItem
            Dim mniTemp As ToolStripMenuItem
            Dim mniSeparator1 As ToolStripSeparator
            Dim mniSeparator2 As ToolStripSeparator
            Dim mniSeparator3 As ToolStripSeparator

            objDict = basCommon.fncGetHusWifeList(mintID)

            mmniHusWif = New ToolStripMenuItem(My.Resources.StrHusWif)
            mmniHusWif.Image = GiaPha.My.Resources.HusbandWife16

            mniTitle = New ToolStripMenuItem()
            mniAddSpouse = New ToolStripMenuItem()
            mniAddSpouseList = New ToolStripMenuItem()
            mniSeparator1 = New ToolStripSeparator()
            mniSeparator2 = New ToolStripSeparator()
            mniSeparator3 = New ToolStripSeparator()

            'icon
            mniTitle.Image = GiaPha.My.Resources.MemberInfo32
            mniAddSpouse.Image = GiaPha.My.Resources.MemberAdd16
            mniAddSpouseList.Image = GiaPha.My.Resources.MemberSearch32

            'text
            mniTitle.Text = String.Format("Thành viên có quan hệ hôn nhân với  {0} ({1})", mstrFullName, objDict.Count)
            mniAddSpouse.Text = My.Resources.StrAddHusWif
            mniAddSpouseList.Text = My.Resources.StrAddHusWifFromList

            'add new menu item
            mmniHusWif.DropDownItems.Add(mniTitle)
            mmniHusWif.DropDownItems.Add(mniSeparator1)

            For Each element As KeyValuePair(Of Integer, String) In objDict

                mniTemp = New ToolStripMenuItem()
                mniTemp.Image = GiaPha.My.Resources.user_male_white_blue_black
                If memGender = clsEnum.emGender.MALE Or memGender = clsEnum.emGender.UNKNOW Then mniTemp.Image = GiaPha.My.Resources.user_female_white_pink_brown

                mniTemp.Text = element.Value.ToString()
                mniTemp.Name = element.Key.ToString()
                mmniHusWif.DropDownItems.Add(mniTemp)

                AddHandler mniTemp.Click, AddressOf xSpouseChange

            Next

            'add sub menu
            mmniHusWif.DropDownItems.Add(mniSeparator2)
            mmniHusWif.DropDownItems.Add(mniAddSpouse)
            mmniHusWif.DropDownItems.Add(mniAddSpouseList)

            'add menu
            mmnuRightMouse.Items.Add(mmniHusWif)

            AddHandler mniAddSpouse.Click, AddressOf xItemClick
            AddHandler mniAddSpouseList.Click, AddressOf xItemClick

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMenuHusWife", ex)
        Finally
            If objDict IsNot Nothing Then objDict.Clear()
        End Try

    End Sub


    ''' <summary>
    ''' xMenuBrother - create sub menu
    ''' </summary>
    ''' <param name="blnShowAddnew"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub xMenuBrother(Optional ByVal blnShowAddnew As Boolean = True)

        Try
            Dim mniBros As ToolStripMenuItem                          'add new brother
            Dim mniAddBro As ToolStripMenuItem                          'add new brother
            Dim mniAddSis As ToolStripMenuItem                          'add new sister
            Dim mniAddYBro As ToolStripMenuItem                         'add new younger brother
            Dim mniAddYSis As ToolStripMenuItem                         'add new younger sister
            Dim mniAddBSL As ToolStripMenuItem                          'add new brother sister from list
            Dim mniSeparator As ToolStripSeparator

            mniBros = New ToolStripMenuItem(My.Resources.StrBros)
            mniAddBro = New ToolStripMenuItem(My.Resources.StrAddBro)
            mniAddSis = New ToolStripMenuItem(My.Resources.StrAddSis)
            mniAddYBro = New ToolStripMenuItem(My.Resources.StrAddYoBro)
            mniAddYSis = New ToolStripMenuItem(My.Resources.StrAddYoSis)
            mniAddBSL = New ToolStripMenuItem(My.Resources.StrAddBroSisFromList)
            mniSeparator = New ToolStripSeparator()


            mniBros.Image = GiaPha.My.Resources.TreeView32
            mniAddBro.Image = GiaPha.My.Resources.MemberAdd32
            mniAddSis.Image = GiaPha.My.Resources.MemberAdd32
            mniAddYBro.Image = GiaPha.My.Resources.MemberAdd32
            mniAddYSis.Image = GiaPha.My.Resources.MemberAdd32
            mniAddBSL.Image = GiaPha.My.Resources.MemberSearch32

            mniBros.DropDownItems.Add(mniAddBro)
            mniBros.DropDownItems.Add(mniAddSis)
            mniBros.DropDownItems.Add(mniAddYBro)
            mniBros.DropDownItems.Add(mniAddYSis)
            mniBros.DropDownItems.Add(mniSeparator)
            mniBros.DropDownItems.Add(mniAddBSL)

            mmnuRightMouse.Items.Add(mniBros)

            AddHandler mniAddBro.Click, AddressOf xItemClick
            AddHandler mniAddSis.Click, AddressOf xItemClick
            AddHandler mniAddYBro.Click, AddressOf xItemClick
            AddHandler mniAddYSis.Click, AddressOf xItemClick
            AddHandler mniAddBSL.Click, AddressOf xItemClick

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMenuBrother", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xMenuChildren - create sub menu
    ''' </summary>
    ''' <param name="blnShowAddnew"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub xMenuChildren(Optional ByVal blnShowAddnew As Boolean = True)

        Dim tblData As DataTable = Nothing

        Try
            Dim mniKids As ToolStripMenuItem                          'add new son
            'Dim mniTitle As ToolStripMenuItem                          'add new son

            Dim mniAddSon As ToolStripMenuItem                          'add new son
            Dim mniAddDau As ToolStripMenuItem                          'add new dauter
            Dim mniAddAKid As ToolStripMenuItem                         'add new adopt kid
            Dim mniAddKidL As ToolStripMenuItem                         'add new kid from list
            Dim mniSeparator As ToolStripSeparator

            mniKids = New ToolStripMenuItem(My.Resources.StrKids)
            mniAddSon = New ToolStripMenuItem(My.Resources.StrAddSon)
            mniAddDau = New ToolStripMenuItem(My.Resources.StrAddDaughter)
            mniAddAKid = New ToolStripMenuItem(My.Resources.StrAddAdoptChild)
            mniAddKidL = New ToolStripMenuItem(My.Resources.StrAddKidFromList)
            mniSeparator = New ToolStripSeparator()

            mniKids.Image = GiaPha.My.Resources.AddChilds48
            mniAddSon.Image = GiaPha.My.Resources.NewImg_B_36_48
            mniAddDau.Image = GiaPha.My.Resources.NewImg_G
            mniAddAKid.Image = GiaPha.My.Resources.MemberAdd32
            mniAddKidL.Image = GiaPha.My.Resources.MemberSearch32

            ''get list of kids
            'tblData = basCommon.fncGetKids(mintID)

            'If tblData IsNot Nothing Then

            '    mniTitle = New ToolStripMenuItem(String.Format("Thành viên {0} có {1} người con", mstrFullName, tblData.Rows.Count))
            '    mniTitle.Image = GiaPha.My.Resources.MemberInfo32
            '    mniKids.DropDownItems.Add(mniTitle)
            '    mniKids.DropDownItems.Add(New ToolStripSeparator())

            '    'add children
            '    xAddChild(tblData, mniKids)

            '    mniKids.DropDownItems.Add(New ToolStripSeparator())

            'End If

            'children
            mniKids.DropDownItems.Add(mniAddSon)
            mniKids.DropDownItems.Add(mniAddDau)
            mniKids.DropDownItems.Add(mniAddAKid)
            mniKids.DropDownItems.Add(mniSeparator)
            mniKids.DropDownItems.Add(mniAddKidL)

            mmnuRightMouse.Items.Add(mniKids)
            mmnuRightMouse.Items.Add(New ToolStripSeparator())

            AddHandler mniAddSon.Click, AddressOf xItemClick
            AddHandler mniAddDau.Click, AddressOf xItemClick
            AddHandler mniAddAKid.Click, AddressOf xItemClick
            AddHandler mniAddKidL.Click, AddressOf xItemClick

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMenuChildren", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' xMenuRootHeadDelete - create sub menu
    ''' </summary>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub xMenuRootHeadDelete()

        Try
            Dim mniDelMem As ToolStripMenuItem                           'delete member
            Dim mniAddRoot As ToolStripMenuItem                          'add root
            Dim mniAddHead As ToolStripMenuItem                          'add head
            Dim mniDelRoot As ToolStripMenuItem                          'delete root
            Dim mniDelHead As ToolStripMenuItem                          'delete head

            mniAddRoot = New ToolStripMenuItem(My.Resources.StrAddRoot)
            mniAddHead = New ToolStripMenuItem(My.Resources.StrAddFamilyHead)
            mniDelRoot = New ToolStripMenuItem(My.Resources.StrDelFromRoot)
            mniDelHead = New ToolStripMenuItem(My.Resources.StrDelFromFamilyHead)
            mniDelMem = New ToolStripMenuItem(My.Resources.StrDelMember)

            mniAddHead.Image = GiaPha.My.Resources.medal_gold_add
            mniDelHead.Image = GiaPha.My.Resources.medal_gold_delete
            mniAddRoot.Image = GiaPha.My.Resources.star_add2
            mniDelRoot.Image = GiaPha.My.Resources.star_delete
            mniDelMem.Image = GiaPha.My.Resources.Cancel

            mmnuRightMouse.Items.Add(mniAddRoot)
            mmnuRightMouse.Items.Add(mniDelRoot)
            mmnuRightMouse.Items.Add(mniAddHead)
            mmnuRightMouse.Items.Add(mniDelHead)
            mmnuRightMouse.Items.Add(New ToolStripSeparator())
            mmnuRightMouse.Items.Add(mniDelMem)

            'reset value
            mniAddRoot.Enabled = False
            mniDelRoot.Enabled = False
            mniAddHead.Enabled = False
            mniDelHead.Enabled = False

            'enable/disable root and head menu item - for male only
            If memGender = clsEnum.emGender.MALE Then

                mniAddRoot.Enabled = True
                mniDelRoot.Enabled = True
                mniAddHead.Enabled = True
                mniDelHead.Enabled = True

                If basCommon.fncIsRoot(mintID) Then
                    mniAddRoot.Visible = False
                    mniDelRoot.Visible = True
                Else
                    mniAddRoot.Visible = True
                    mniDelRoot.Visible = False
                End If

                If basCommon.fncIsFhead(mintID) Then
                    mniAddHead.Visible = False
                    mniDelHead.Visible = True
                Else
                    mniAddHead.Visible = True
                    mniDelHead.Visible = False
                End If

            Else
                mniDelHead.Visible = False
                mniDelRoot.Visible = False
            End If

            AddHandler mniAddHead.Click, AddressOf xItemClick
            AddHandler mniDelHead.Click, AddressOf xItemClick
            AddHandler mniAddRoot.Click, AddressOf xItemClick
            AddHandler mniDelRoot.Click, AddressOf xItemClick
            AddHandler mniDelMem.Click, AddressOf xItemClick

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMenuRootHeadDelete", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xShowInfo - create sub menu
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub xShowInfo(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            'RaiseEvent evnShowPerInfo(mintID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowInfo", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xItemClick - raise event item click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub xItemClick(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            RaiseEvent evnMenuItemClick(mintID, sender)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xItemClick", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xSpouseChange - raise event when spouse changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub xSpouseChange(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            Dim objMenuItem As ToolStripMenuItem
            Dim intSpouseID As Integer

            objMenuItem = CType(sender, ToolStripMenuItem)

            'get index of spouse
            'intIndex = mmniHusWif.DropDownItems.IndexOf(objMenuItem)

            intSpouseID = basCommon.fncCnvToInt(objMenuItem.Name)

            RaiseEvent evnSpouseChange(intSpouseID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xItemClick", ex)
        End Try

    End Sub


#Region "Not used"

    Private Sub xAddChild(ByVal tblData As DataTable, ByRef mniKids As ToolStripMenuItem)

        Try
            Dim strFName As String
            Dim strLName As String
            Dim strMName As String
            Dim strAlias As String
            Dim strFullName As String
            Dim intMemID As Integer
            Dim intGender As Integer

            For i As Integer = 0 To tblData.Rows.Count - 1

                strFName = fncCnvNullToString(tblData.Rows(i).Item("FIRST_NAME"))
                strMName = fncCnvNullToString(tblData.Rows(i).Item("MIDDLE_NAME"))
                strLName = fncCnvNullToString(tblData.Rows(i).Item("LAST_NAME"))
                strAlias = fncCnvNullToString(tblData.Rows(i).Item("ALIAS_NAME"))
                strFullName = fncGetFullName(strFName, strMName, strLName, strAlias)

                Integer.TryParse(fncCnvNullToString(tblData.Rows(i).Item("MEMBER_ID")), intMemID)
                Integer.TryParse(fncCnvNullToString(tblData.Rows(i).Item("GENDER")), intGender)

                Dim mniChild As ToolStripMenuItem
                mniChild = New ToolStripMenuItem(strFullName)

                mniChild.Image = GiaPha.My.Resources.user_female_white_pink_brown
                If intGender = clsEnum.emGender.MALE Or intGender = clsEnum.emGender.UNKNOW Then mniChild.Image = GiaPha.My.Resources.user_male_white_blue_black

                mniKids.DropDownItems.Add(mniChild)

                AddHandler mniKids.Click, AddressOf xShowInfo

            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddChild", ex)
        End Try

    End Sub

#End Region

End Class
