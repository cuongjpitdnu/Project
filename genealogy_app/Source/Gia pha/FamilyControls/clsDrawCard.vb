'   ******************************************************************
'      TITLE      : DRAW MEMBER CARD
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/08/30　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************

Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : Draw member card class
'      MEMO       : 
'      CREATE     : 2011/08/30  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class clsDrawCard
    Implements IDisposable

    Public Event evnRefresh(ByVal intCurID As Integer, ByVal blnRedraw As Boolean)
    Public Event evnRedraw()
    Public Event evnCardClick(ByVal intCurID As Integer)

#Region "Constants"

    Private Const mcstrClsName As String = "clsDrawCard"                                        'class name

    Private Const mcstrMemIdCol As String = "MEMBER_ID"                                         'member id field
    Private Const mcstrSingle As String = "Độc thân"                                              'single text
    Private Const mcstrMarried As String = "Đã kết hôn"                                            'married text
    Private Const mcstrBirthFormat As String = "Ngày sinh: {0} {1}"                             'birth date text
    Private Const mcstrHomeFormat As String = "Quê quán : {0}"                                  'hometown text
    Private Const mcstrAdopt As String = "(Con nuôi)"                                           'adopt
    Private Const mcstrNoParent As String = "Thành viên này hiện không có cha mẹ."                       'no parent message

    Private Const mcintMARGIN As Integer = 5                                                    'margin from center X of panel
    Private Const mcintPADDING As Integer = 20                                                  'padding between 2 control
    Private Const mcintBASELINE_PADDING As Integer = 200                                        'padding between center Y of panel  

#End Region


#Region "Variable"

    'card for members in family
    Private mobjHusbandCard As FamilyCard                                           'husband (left card)
    Private mobjWifeCard As FamilyCard                                              'wife (right card)
    Private mobjFatherCard As FamilyCard                                            'father
    Private mobjMotherCard As FamilyCard                                            'mother
    Private mobjBoardCard As FamilyCard                                             'board to show information
    Private mobjAddChildCard As FamilyCard                                          'add child button
    Private mobjVnCal As clsLunarCalendar                                           'lunar calendar
    Private mobjRightMenu As clsRightMenu                                           'class to create right menu
    Private mobjSpouseList As usrSpouseList                                         'list of spouse

    'list of child
    Private mlstChildCard As List(Of FamilyCard)                                    'his/her children

    Private mpnDrawPanel As Panel                                                   'panel to draw

    Private mfrmPerInfo As frmPersonInfo                                            'form to add or view detail personal information

    Private mptCenter As Point                                                      'center point of panel

    Private mstActive As stMemberInfo                                               'info of active member
    Private mstSpouse As stMemberInfo                                               'info of hus/wife
    Private mstFather As stMemberInfo                                               'info of father
    Private mstMother As stMemberInfo                                               'info of mother
    Private mlstChild As List(Of stMemberInfo)                                      'info of kids
    Private mlstSpouse As List(Of stMemberInfo)                                     'info of spouse

    Private mmnuRightMouse As ContextMenuStrip                                      'right mouse menu on active member card
    'Private mmniHusWif As ToolStripMenuItem                                         'menu item husband / wife
    'Private mmniAddRoot As ToolStripMenuItem                                        'menu item add member to root
    'Private mmniAddHead As ToolStripMenuItem                                        'menu item add member to family head
    'Private mmniDelRoot As ToolStripMenuItem                                        'menu item delete member from root
    'Private mmniDelHead As ToolStripMenuItem                                        'menu item delete member from family head

    Private mvwDetail As DataView                                                   'dataview to store data of members

    Private mintActiveID As Integer                                                 'id of current active member
    Private mintPreviousID As Integer                                               'id of previous member
    Private mintTotalKidWidth As Integer                                            'total width to draw kids

    Private disposedValue As Boolean = False                                        'To detect redundant calls

#End Region


#Region "Structure"

    '   ******************************************************************
    '　　　FUNCTION   : Member's information Structure
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Structure stMemberInfo

        Dim intID As Integer                        'id of member

        Dim strFirstName As String                  'first name
        Dim strMidName As String                    'middle name
        Dim strLastName As String                   'last name
        Dim strFullName As String                   'full name
        Dim strFullNameWAlias As String             'full name with alias
        Dim strAlias As String                      'alias
        Dim strBirthPlace As String                 'birth place
        Dim strHometown As String                   'hometown
        Dim strImage As String                      'image location

        'Dim dtBirth As Date                         'birth date
        Dim intBday As Integer
        Dim intBmon As Integer
        Dim intByea As Integer

        'Dim dtDie As Date                           'die date
        Dim intDday As Integer
        Dim intDmon As Integer
        Dim intDyea As Integer

        Dim intDie As Integer                       'deceased or not
        Dim intGender As clsEnum.emGender           'gender
        Dim intRel As Integer                       'relationship 
        Dim intFamilyFlag As clsEnum.emFamily_Flag  'family flag
        Dim intRoleOrder As Integer                 'relationship order

    End Structure

#End Region


#Region "Properties"

    '   ******************************************************************
    '　　　FUNCTION   : ActiveMemberID Property, Set Member ID
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property ActiveMemberID() As Integer

        Get
            Return mintActiveID
        End Get

        Set(ByVal value As Integer)

            mintPreviousID = mintActiveID
            mintActiveID = value
            mstActive.intID = value

            'get data
            xGetData()

            'get information again
            xGetInfo()

            'redraw
            fncDraw()

        End Set

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : MaleCardLocation Property, Get Husband card location
    '      MEMO       : 
    '      CREATE     : 2011/11/28  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property MaleCardLocation() As Point

        Get

            Return mobjHusbandCard.Location

        End Get

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : FemaleCardLocation Property, Get Wife card location
    '      MEMO       : 
    '      CREATE     : 2011/11/28  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property FemaleCardLocation() As Point

        Get

            Return mobjWifeCard.Location

        End Get

    End Property

#End Region


#Region "Constructor"


    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/08/30  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New(ByVal pnDraw As Panel, ByVal frmInfo As frmPersonInfo)

        Me.mpnDrawPanel = pnDraw
        Me.mfrmPerInfo = frmInfo
        'Me.mmnuContext = mnuContext

        'pnDraw.AutoScroll = True

        mvwDetail = Nothing

        mintActiveID = basConst.gcintNO_MEMBER
        mintTotalKidWidth = clsDefine.NONE_VALUE
        mintPreviousID = basConst.gcintNO_MEMBER

        'init value
        mobjHusbandCard = New FamilyCard()
        mobjWifeCard = New FamilyCard()
        mobjFatherCard = New FamilyCard()
        mobjMotherCard = New FamilyCard()
        mobjBoardCard = New FamilyCard()
        mobjAddChildCard = New FamilyCard()
        mobjVnCal = New clsLunarCalendar()
        mobjRightMenu = New clsRightMenu()
        mobjSpouseList = New usrSpouseList()

        mlstChildCard = New List(Of FamilyCard)
        mlstChild = New List(Of stMemberInfo)
        'mlstSpouse = New List(Of stMemberInfo)

        'mmnuSpouse = New ContextMenuStrip()
        mmnuRightMouse = New ContextMenuStrip()
        mptCenter = New Point(clsDefine.NONE_VALUE, clsDefine.NONE_VALUE)

        mobjSpouseList.Height = clsDefine.CARD_ADD_H + clsDefine.CARD_MINI_H + mcintPADDING

        xCreateRightMenu()

        xAddHandler()

        xGetData()

        'xDrawAkbLogo()

    End Sub


#End Region


#Region "Functions and methods"


    '   ******************************************************************
    '　　　FUNCTION   : xDraw, draw member cards
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncDraw() As Boolean

        fncDraw = False

        Try
            'enable autoscroll for calculating value
            mpnDrawPanel.AutoScroll = True

            'invisible then visible to prevent fragment
            mobjFatherCard.Visible = False
            mobjMotherCard.Visible = False
            mobjHusbandCard.Visible = False
            mobjWifeCard.Visible = False

            'get center point
            xGetCenterPoint()

            'draw active member
            xDrawMember()

            'draw children
            xDrawChild()

            'draw father
            xDrawParent()

            'draw add child control
            xDrawAddChild()

            'draw board
            xDrawBoard()

            'draw spouse board
            xDrawSpouseList()

            'draw connector
            xDrawLine()

            'set scroll area
            xSetScrollBar()
            xSetDisplayArea()

            'invisible then visible to prevent fragment
            mobjFatherCard.Visible = True
            mobjMotherCard.Visible = True
            mobjHusbandCard.Visible = True
            mobjWifeCard.Visible = True

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDraw", ex)
        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncRefreshSpouseList, redraw spouse list card
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub fncRefreshSpouseList()
        'fncRefreshSpouseList = False

        Try
            Dim stTempSpouse As stMemberInfo = mstSpouse

            'get his/her spouse
            xGetSpouse(mstActive.intID, mstSpouse)

            'draw spouse board
            xDrawSpouseList()

            xSetDisplayArea()

            mstSpouse = stTempSpouse

            'Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncRefreshSpouseList", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xGetCenterPoint, get center X Y cordinate of panel
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetCenterPoint() As Boolean

        xGetCenterPoint = False

        Try
            Dim intX As Integer
            Dim intY As Integer
            Dim intWidthOfMem As Integer                    'total width to draw active member and his wife
            Dim intHeightOfMem As Integer

            mpnDrawPanel.AutoScrollPosition = New Point(0, 0)

            intX = Me.mpnDrawPanel.Width \ 2                'horizontal
            intY = Me.mpnDrawPanel.Height \ 2               'vertical

            intWidthOfMem = clsDefine.CARD_LARG_W * 2 + mcintPADDING
            intHeightOfMem = mcintBASELINE_PADDING + clsDefine.CARD_MIDD_H + mcintPADDING + mcintMARGIN
            mintTotalKidWidth = mlstChild.Count * clsDefine.CARD_SMAL_W + (mlstChild.Count - 1) * mcintPADDING

            mptCenter.X = intX
            mptCenter.Y = intY

            'check if panel's width is not enough space to draw
            If mpnDrawPanel.Width < mintTotalKidWidth Or mpnDrawPanel.Width < intWidthOfMem Then

                'if width of member's is longer than with of kids
                If intWidthOfMem > mintTotalKidWidth Then

                    mptCenter.X = intWidthOfMem \ 2

                Else

                    mptCenter.X = mintTotalKidWidth \ 2

                End If

            End If

            'check height
            If mptCenter.Y - intHeightOfMem < 0 Then

                mptCenter.Y += (intHeightOfMem - mptCenter.Y)

            End If


            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetCenterPoint", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetDisplayArea, set display area of panel
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/12/13  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetDisplayArea() As Boolean

        xSetDisplayArea = False

        Try
            Dim ptPointToShow As Point

            mpnDrawPanel.HorizontalScroll.Visible = False

            'check if panel's width is not enough space to draw
            If mpnDrawPanel.Width < mintTotalKidWidth Then

                ptPointToShow = New Point(0, 0)
                ptPointToShow.X = (mintTotalKidWidth - mpnDrawPanel.Width) \ 2

                mpnDrawPanel.HorizontalScroll.Visible = True
                mpnDrawPanel.AutoScrollPosition = ptPointToShow

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetDisplayArea", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetScrollBar, create scroll bar
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2012/09/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetScrollBar() As Boolean
        xSetScrollBar = False
        Try

            Dim intValue As Integer
            Dim intMin As Integer
            Dim intMax As Integer
            Dim intSmall As Integer
            Dim intLarge As Integer

            intValue = mpnDrawPanel.HorizontalScroll.Value
            intMin = mpnDrawPanel.HorizontalScroll.Minimum
            intMax = mpnDrawPanel.HorizontalScroll.Maximum
            intSmall = mpnDrawPanel.HorizontalScroll.SmallChange
            intLarge = mpnDrawPanel.HorizontalScroll.LargeChange

            mpnDrawPanel.AutoScroll = False
            'mpnDrawPanel.HorizontalScroll.Visible = True
            mpnDrawPanel.HorizontalScroll.Value = intValue
            mpnDrawPanel.HorizontalScroll.Minimum = intMin
            mpnDrawPanel.HorizontalScroll.Maximum = intMax
            mpnDrawPanel.HorizontalScroll.SmallChange = intSmall
            mpnDrawPanel.HorizontalScroll.LargeChange = intLarge

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetScrollBar", ex)
        End Try
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawMember, draw active member and spouse
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawMember() As Boolean

        xDrawMember = False

        Try
            Dim ptLocation1 As Point
            Dim ptLocation2 As Point
            Dim strName As String = String.Empty
            Dim intGender As clsEnum.emGender = clsEnum.emGender.FEMALE

            ptLocation1 = New Point(clsDefine.NONE_VALUE, clsDefine.NONE_VALUE)
            ptLocation2 = New Point(clsDefine.NONE_VALUE, clsDefine.NONE_VALUE)

            'set gender of active member at first load or gender is UNKNOWN
            'If mstActive.intGender = clsEnum.emGender.UNKNOW Then mstActive.intGender = clsEnum.emGender.MALE

            'find gender of spouse if spouse doesn't exist
            If mstSpouse.intID <= basConst.gcintNO_MEMBER Then
                'spouse's gender is opposite with active member
                mstSpouse.intGender = clsEnum.emGender.FEMALE
                If mstActive.intGender = clsEnum.emGender.FEMALE Then mstSpouse.intGender = clsEnum.emGender.MALE

            End If

            'draw active member
            ptLocation1.X = mptCenter.X - mcintMARGIN - clsDefine.CARD_LARG_W
            ptLocation1.Y = mptCenter.Y - mcintBASELINE_PADDING + mcintPADDING

            'draw spouse
            ptLocation2.Y = ptLocation1.Y
            ptLocation2.X = mptCenter.X + mcintMARGIN

            'fil data
            If mstActive.intGender = clsEnum.emGender.MALE Or mstActive.intGender = clsEnum.emGender.UNKNOW Then
                xFillCard(mobjHusbandCard, mstActive)
                xFillCard(mobjWifeCard, mstSpouse)
                xDrawCardBase(ptLocation1, mstActive.intGender, clsEnum.emCardSize.LARGE, mobjHusbandCard)
                xDrawCardBase(ptLocation2, mstSpouse.intGender, clsEnum.emCardSize.LARGE, mobjWifeCard)
            Else
                xFillCard(mobjWifeCard, mstActive)
                xFillCard(mobjHusbandCard, mstSpouse)
                xDrawCardBase(ptLocation1, mstSpouse.intGender, clsEnum.emCardSize.LARGE, mobjHusbandCard)
                xDrawCardBase(ptLocation2, mstActive.intGender, clsEnum.emCardSize.LARGE, mobjWifeCard)
            End If

            'set context menu
            mobjHusbandCard.ContextMenuStrip = Nothing
            mobjWifeCard.ContextMenuStrip = Nothing

            'only set context menu for active member
            xCreateRightMenu()

            'disable/enable control when there's no active member
            xEnableControl()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawMember", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawParent, draw parent card
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawParent() As Boolean

        xDrawParent = False

        Try
            Dim ptLocationH As Point
            Dim ptLocationW As Point

            ptLocationH = New Point(clsDefine.NONE_VALUE, clsDefine.NONE_VALUE)
            ptLocationW = New Point(clsDefine.NONE_VALUE, clsDefine.NONE_VALUE)

            'FATHER
            If mstFather.intID > basConst.gcintNO_MEMBER Then

                ptLocationH.X = mptCenter.X - mcintMARGIN - clsDefine.CARD_MIDD_W
                ptLocationH.Y = mptCenter.Y - mcintBASELINE_PADDING - mcintPADDING - clsDefine.CARD_MIDD_H
                xDrawCardBase(ptLocationH, mstFather.intGender, clsEnum.emCardSize.MIDDLE, mobjFatherCard)
                xFillCard(mobjFatherCard, mstFather)

            Else

                ptLocationH.X = mptCenter.X - mcintMARGIN - clsDefine.CARD_MINI_W
                ptLocationH.Y = mptCenter.Y - mcintBASELINE_PADDING - mcintPADDING - clsDefine.CARD_MINI_H
                'reset name and draw card
                mobjFatherCard.PersonName = Global.GiaPha.My.Resources.Resources.StrAddFather
                xDrawCardBase(ptLocationH, clsEnum.emGender.MALE, clsEnum.emCardSize.MINI, mobjFatherCard)

            End If

            'reset position of panel for drawing
            'mpnDrawPanel.AutoScrollPosition = New Point(0, 0)

            'MOTHER
            If mstMother.intID > basConst.gcintNO_MEMBER Then

                ptLocationW.X = mptCenter.X + mcintMARGIN
                ptLocationW.Y = mptCenter.Y - mcintBASELINE_PADDING - mcintPADDING - clsDefine.CARD_MIDD_H
                xDrawCardBase(ptLocationW, mstMother.intGender, clsEnum.emCardSize.MIDDLE, mobjMotherCard)
                xFillCard(mobjMotherCard, mstMother)

            Else

                ptLocationW.X = mptCenter.X + mcintMARGIN
                ptLocationW.Y = mptCenter.Y - mcintBASELINE_PADDING - mcintPADDING - clsDefine.CARD_MINI_H
                'reset name and draw card
                mobjMotherCard.PersonName = Global.GiaPha.My.Resources.Resources.StrAddMother
                xDrawCardBase(ptLocationW, clsEnum.emGender.FEMALE, clsEnum.emCardSize.MINI, mobjMotherCard)

            End If

            'disable/enable control when there's no active member
            xEnableControl()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawParent", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawBoard, draw board card that show information
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 2011/12/13  AKB Quyet
    '   ******************************************************************
    Private Function xDrawBoard() As Boolean

        xDrawBoard = False

        Dim tblDataKid As DataTable = Nothing

        Try
            'point to draw
            Dim ptLocate As Point

            ptLocate = New Point(clsDefine.NONE_VALUE, clsDefine.NONE_VALUE)

            'X-middle of panel; Y
            ' ▽ 2012/11/14   AKB Quyet （変更内容）*********************************
            'ptLocate.X = mptCenter.X - clsDefine.CARD_LARG_W \ 2


            ptLocate.X = mptCenter.X - clsDefine.CARD_LARG_W - mcintMARGIN

            If mstActive.intGender = clsEnum.emGender.FEMALE Then
                ptLocate.X = mptCenter.X + mcintMARGIN
            End If

            ' △ 2012/11/14   AKB Quyet *********************************************

            ptLocate.Y = mptCenter.Y + CInt(mcintPADDING * 1.5)

            'total child of active member
            tblDataKid = basCommon.fncGetKids(mintActiveID)
            If tblDataKid Is Nothing Then
                mobjBoardCard.PersonTotalChilds = 0
            Else
                mobjBoardCard.PersonTotalChilds = tblDataKid.Rows.Count
            End If

            'married status
            mobjBoardCard.PersonStatus = mcstrSingle
            If mstSpouse.intID > basConst.gcintNO_MEMBER Then mobjBoardCard.PersonStatus = mcstrMarried

            'draw card
            xDrawCardBase(ptLocate, clsEnum.emGender.UNKNOW, clsEnum.emCardSize.ORTHER, mobjBoardCard)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawBoard", ex)
        Finally
            If tblDataKid IsNot Nothing Then tblDataKid.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawSpouseList, draw spouse list board
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 2011/12/13  AKB Quyet
    '   ******************************************************************
    Private Function xDrawSpouseList() As Boolean

        xDrawSpouseList = False

        Try
            'point to draw
            Dim ptLocate As Point
            Dim emGender As clsEnum.emGender

            'location
            ptLocate = New Point(clsDefine.NONE_VALUE, clsDefine.NONE_VALUE)

            ptLocate.X = mptCenter.X + mcintMARGIN
            ptLocate.Y = mobjBoardCard.Location.Y

            If mstActive.intGender = clsEnum.emGender.FEMALE Then
                ptLocate.X = mptCenter.X - mobjSpouseList.Width - mcintMARGIN
            End If

            'create board
            emGender = clsEnum.emGender.FEMALE
            If mstActive.intGender = clsEnum.emGender.FEMALE Then emGender = clsEnum.emGender.MALE

            mobjSpouseList.fncReset(emGender, mstActive.strFullNameWAlias)

            For i As Integer = 0 To mlstSpouse.Count - 1
                mobjSpouseList.fncAddItem(emGender, mlstSpouse(i).intID, mlstSpouse(i).strFullName)
            Next

            Me.mpnDrawPanel.Controls.Add(mobjSpouseList)
            mobjSpouseList.Location = ptLocate

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawSpouseList", ex)
        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawChild, draw child cards
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawChild() As Boolean

        xDrawChild = False

        Try
            'point to draw
            Dim ptLocate As Point
            ptLocate = New Point(clsDefine.NONE_VALUE, clsDefine.NONE_VALUE)

            If mlstChild.Count <= 0 Then Exit Function

            ptLocate.Y = mptCenter.Y + CInt(mcintPADDING * 1.5) + clsDefine.CARD_ADD_H + clsDefine.CARD_SMAL_H
            ptLocate.X = mptCenter.X - mintTotalKidWidth \ 2

            For i As Integer = 0 To mlstChild.Count - 1

                'draw card
                xDrawCardBase(ptLocate, mlstChild(i).intGender, clsEnum.emCardSize.SMALL, mlstChildCard(i))

                'increase X cordinate
                ptLocate.X = ptLocate.X + clsDefine.CARD_SMAL_W + mcintPADDING

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawChild", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawAddChild, draw add new child cards
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawAddChild() As Boolean

        xDrawAddChild = False

        Try
            Dim ptLocate As Point
            ptLocate = New Point(clsDefine.NONE_VALUE, clsDefine.NONE_VALUE)

            'X Y cordinate
            ' ▽ 2012/11/14  AKB Quyet （Change x）*********************************
            'ptLocate.X = mptCenter.X - clsDefine.CARD_MINI_W \ 2

            ptLocate.X = mptCenter.X - clsDefine.CARD_MINI_W - mcintMARGIN

            If mstActive.intGender = clsEnum.emGender.FEMALE Then
                ptLocate.X = mptCenter.X + mcintMARGIN
            End If

            ' △ 2012/11/14  AKB Quyet *********************************************

            'ptLocate.Y = mclsBoardCard.Location.Y + mclsBoardCard.Height + mcintPADDING
            'ptLocate.Y = mptCenter.Y + mobjBoardCard.Height + mcintPADDING * 3
            ptLocate.Y = mptCenter.Y + clsDefine.CARD_ADD_H + CInt(mcintPADDING * 2) 'mcintPADDING * 3

            'if there is a child
            'If mlstChild.Count > 0 Then ptLocate.Y += clsDefine.CARD_SMAL_H + mcintPADDING

            xDrawCardBase(ptLocate, clsEnum.emGender.UNKNOW, clsEnum.emCardSize.MINI, mobjAddChildCard)

            'disable/enable control when there's no active member
            xEnableControl()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawAddChild", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawCardBase, base function to draw card
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : ptPosition  Point, point to draw
    '      PARAMS2    : enmCardGender   clsEnum.Gender, gender of member
    '      PARAMS3    : enmCardSize clsEnum.CardSize, card size
    '      PARAMS4    : objCtrlCard FamilyCard, card instance
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawCardBase(ByVal ptPosition As Point, _
                                   ByVal enmCardGender As clsEnum.emGender, _
                                   ByVal enmCardSize As clsEnum.emCardSize, _
                                   ByVal objCtrlCard As FamilyCard) As Boolean
        xDrawCardBase = False

        Try
            objCtrlCard.Visible = True
            objCtrlCard.TabStop = False
            objCtrlCard.Location = ptPosition
            objCtrlCard.PersonGender = enmCardGender
            objCtrlCard.PersonSize = enmCardSize

            objCtrlCard.PersonInfoDlg = Me.mfrmPerInfo

            Me.mpnDrawPanel.Controls.Add(objCtrlCard)

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xDrawCardBase", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawAkbLogo, draw logo
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : 
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawAkbLogo() As Boolean
        xDrawAkbLogo = False

        Try
            Dim picLogo As PictureBox

            picLogo = New PictureBox()
            picLogo.Height = 150
            picLogo.Width = 150
            picLogo.SizeMode = PictureBoxSizeMode.Zoom
            picLogo.Location = New Point(0, 0)
            picLogo.Image = My.Resources.AKBlogo

            mpnDrawPanel.Controls.Add(picLogo)

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xDrawAkbLogo", ex, Nothing, False)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillCard, fill information to card
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : objCard FamilyCard, card instance
    '      PARAMS2    : stInfo  stMemberInfo, structure that store infor
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillCard(ByVal objCard As FamilyCard, ByVal stInfo As stMemberInfo) As Boolean

        xFillCard = False

        Try
            With stInfo
                'id
                objCard.PersonID = .intID

                objCard.PersonAlias = ""

                'name
                If .intID > basConst.gcintNO_MEMBER Then

                    objCard.PersonName = .strFullName

                    If Not basCommon.fncIsBlank(.strAlias) Then objCard.PersonAlias = .strAlias

                Else

                    objCard.PersonName = My.Resources.StrNewMem

                End If

                'homtown, birth date and avatar
                objCard.PersonHome = ""
                objCard.PersonBirhPlace = ""
                If Not basCommon.fncIsBlank(.strHometown) Then objCard.PersonHome = .strHometown
                If Not basCommon.fncIsBlank(.strBirthPlace) Then objCard.PersonBirhPlace = .strBirthPlace

                objCard.PersonBirth = ""
                objCard.PersonLunarBirth = ""
                'If .dtBirth > Date.MinValue Then
                '    objCard.PersonBirth = String.Format(basConst.gcstrDateFormat2, .dtBirth)
                '    objCard.PersonLunarBirth = mobjVnCal.fncGetLunarDateString(.dtBirth)
                'End If
                objCard.PersonBirth = basCommon.fncGetDateName("", .intBday, .intBmon, .intByea, False)
                objCard.PersonLunarBirth = basCommon.fncGetSolar2LunarDateName("", .intBday, .intBmon, .intByea)

                objCard.PersonBirthDie = ""
                'If .intDie = basConst.gcintDIED Then objCard.PersonBirthDie = basCommon.fncGetBirthDieText(.dtBirth, .dtDie, .intDie)
                If .intDie = basConst.gcintDIED Then objCard.PersonBirthDie = basCommon.fncGetBirthDieText(.intByea, .intDyea, .intDie)
                'objCard.PersonBirthDie = basCommon.fncGetDateName("", .intDday, .intDmon, .intDyea, True, True)

                'set avatar
                If Not basCommon.fncIsBlank(.strImage) Then
                    'if user has avatar, set location
                    objCard.PersonPicImage = basCommon.fncCreateThumbnail(.strImage, clsDefine.CARD_LARG_W, clsDefine.CARD_LARG_H, .intGender)
                Else
                    'set default if there is no avatar
                    ' ▽ 2012/12/14   AKB Quyet （変更内容）*********************************
                    'objCard.PersonPicImage = Global.GiaPha.My.Resources.Resources.NewImg_Fa
                    'If .intGender = clsEnum.emGender.FEMALE Then objCard.PersonPicImage = Global.GiaPha.My.Resources.Resources.NewImg_Mo

                    objCard.PersonPicImage = Global.GiaPha.My.Resources.Resources.no_avatar_m
                    If .intGender = clsEnum.emGender.FEMALE Then objCard.PersonPicImage = Global.GiaPha.My.Resources.Resources.no_avatar_f
                    ' △ 2012/12/14   AKB Quyet *********************************************
                End If

                'relationship
                objCard.PersonRelation = ""
                If .intRel = CInt(clsEnum.emRelation.ADOPT) Then objCard.PersonRelation = mcstrAdopt

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillCard", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetInfo, get infor of a member
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetInfo() As Boolean

        xGetInfo = False

        Try
            'get active member's detail infor
            xGetDetailInfo(mstActive)

            'get his/her spouse
            xGetSpouse(mstActive.intID, mstSpouse)

            'get parent
            xGetParent(mstActive.intID, mstFather, mstMother)

            'get children
            xGetChild(mstActive.intID, mstSpouse.intID)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetInfo", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetData, get data from database
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetData() As Boolean

        xGetData = False

        Try
            Dim tblData As DataTable

            'get all 
            tblData = gobjDB.fncGetMemberMain()
            mvwDetail = New DataView(tblData)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetData", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetDetailInfo, get detail infor of an member
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : stMem   stMemberInfo, structure that store infor
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 2011/12/13  AKB Quyet
    '   ******************************************************************
    Private Function xGetDetailInfo(ByRef stMem As stMemberInfo) As Boolean

        xGetDetailInfo = False

        Try
            Dim intGender As Integer = 0
            Dim intFamilyFlag As Integer = 0
            Dim strAvatar As String = String.Empty

            If mvwDetail Is Nothing Then Exit Function

            'create filter to find member
            mvwDetail.RowFilter = String.Format(gcstrRowFilterFormat, mcstrMemIdCol, stMem.intID)

            If mvwDetail.Count <= 0 Then

                stMem = Nothing
                Exit Function

            End If

            With stMem
                'name, address
                .strFirstName = basCommon.fncCnvNullToString(mvwDetail(0)("FIRST_NAME"))
                .strMidName = basCommon.fncCnvNullToString(mvwDetail(0)("MIDDLE_NAME"))
                .strLastName = basCommon.fncCnvNullToString(mvwDetail(0)("LAST_NAME"))
                .strAlias = basCommon.fncCnvNullToString(mvwDetail(0)("ALIAS_NAME"))
                .strHometown = basCommon.fncCnvNullToString(mvwDetail(0)("HOMETOWN"))
                .strBirthPlace = basCommon.fncCnvNullToString(mvwDetail(0)("BIRTH_PLACE"))

                'fullname
                .strFullName = String.Format(basConst.gcstrNameFormat, .strLastName, .strMidName, .strFirstName)
                .strFullName = basCommon.fncRemove2Space(.strFullName)
                .strFullNameWAlias = basCommon.fncGetFullName(.strFirstName, .strMidName, .strLastName, .strAlias)

                'avatar
                strAvatar = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath
                .strImage = basCommon.fncCnvNullToString(mvwDetail(0)("AVATAR_PATH"))
                If Not basCommon.fncIsBlank(.strImage) Then .strImage = strAvatar & .strImage

                'birth date
                'Date.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("BIRTH_DAY")), .dtBirth)
                Integer.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("BIR_DAY")), .intBday)
                Integer.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("BIR_MON")), .intBmon)
                Integer.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("BIR_YEA")), .intByea)

                'Date.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("DECEASED_DATE")), .dtDie)
                Integer.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("DEA_DAY")), .intDday)
                Integer.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("DEA_MON")), .intDmon)
                Integer.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("DEA_YEA")), .intDyea)

                'die or not
                Integer.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("DECEASED")), .intDie)

                'gender
                Integer.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("GENDER")), intGender)
                .intGender = clsEnum.emGender.UNKNOW
                If intGender = clsEnum.emGender.FEMALE Then .intGender = clsEnum.emGender.FEMALE
                If intGender = clsEnum.emGender.MALE Then .intGender = clsEnum.emGender.MALE

                'family flag
                'Integer.TryParse(basCommon.fncCnvNullToString(mvwDetail(0)("FAMILY_FLAG")), intFamilyFlag)
                '.intFamilyFlag = clsEnum.emFamily_Flag.NOT_IN_FAMILY
                'If intFamilyFlag = clsEnum.emFamily_Flag.IN_FAMILY Then .intFamilyFlag = clsEnum.emFamily_Flag.IN_FAMILY

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetDetailInfo", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetSpouse, get spouse's information
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : intID   Integer, active member id
    '      PARAMS2    : stMem   stMemberInfo, structure that store infor
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetSpouse(ByVal intID As Integer, ByRef stMem As stMemberInfo) As Boolean

        xGetSpouse = False

        Dim tblData As DataTable = Nothing

        Try
            Dim stTempMemberInfo As stMemberInfo = Nothing

            'reset
            stMem = Nothing
            xClearCard(mobjMotherCard)

            tblData = gobjDB.fncGetHusWife(intID)

            If tblData Is Nothing Then
                mlstSpouse = New List(Of stMemberInfo)(0)
                Exit Function
            End If

            mlstSpouse = New List(Of stMemberInfo)(tblData.Rows.Count)

            For i As Integer = 0 To tblData.Rows.Count - 1

                'reset struc before reusing
                stTempMemberInfo = Nothing

                'get member id then detail info
                Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i).Item("REL_FMEMBER_ID")), stTempMemberInfo.intID)
                xGetDetailInfo(stTempMemberInfo)

                'add to spouse list
                mlstSpouse.Add(stTempMemberInfo)

            Next

            'return first member
            stMem = mlstSpouse(0)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetSpouse", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetParent, get parents's info
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intID   Integer, active member id
    '      PARAMS     : stFa    stMemberInfo, info structure of father
    '      PARAMS     : stMo    stMemberInfo, info structure of mother
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetParent(ByVal intID As Integer, ByRef stFa As stMemberInfo, ByRef stMo As stMemberInfo) As Boolean

        xGetParent = False

        Dim tblData As DataTable = Nothing

        Try
            Dim intMemID As Integer = 0
            Dim intGender As Integer = clsEnum.emGender.UNKNOW

            'reset
            stFa = Nothing
            stMo = Nothing

            xClearCard(mobjFatherCard)
            xClearCard(mobjMotherCard)

            'get parent
            'Edit by: 2019.08.30 AKB TungNT
            'tblData = gobjDB.fncGetParent(intID, False)
            tblData = gobjDB.fncGetParent(intID)
            'Edit by: 2019.08.30 AKB TungNT

            If tblData Is Nothing OrElse tblData.Rows.Count = 0 Then Exit Function

            'Add by: 2019.08.30 AKB TungNT
            Using vFilter As New DataView(tblData)
                vFilter.RowFilter = "RELID = " & CInt(clsEnum.emRelation.NATURAL).ToString()
                tblData = vFilter.ToTable()
                If tblData Is Nothing OrElse tblData.Rows.Count = 0 Then
                    vFilter.RowFilter = "RELID = " & CInt(clsEnum.emRelation.ADOPT).ToString()
                    tblData = vFilter.ToTable()
                End If
            End Using
            'Add by: 2019.08.30 AKB TungNT

            For i As Integer = 0 To tblData.Rows.Count - 1

                'get id and gender
                Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i).Item("REL_FMEMBER_ID")), intMemID)
                Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i).Item("GENDER")), intGender)

                'use gender to determine father or mother
                If intGender = clsEnum.emGender.MALE Then
                    stFa.intID = intMemID
                Else
                    stMo.intID = intMemID
                End If

            Next

            'get detail info
            xGetDetailInfo(stFa)
            xGetDetailInfo(stMo)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetParent", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetChild, get child of active member
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intID Integer, active member id
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetChild(ByVal intFaID As Integer, Optional ByVal intMoID As Integer = basConst.gcintNO_MEMBER) As Boolean

        xGetChild = False

        Dim tblData As DataTable = Nothing
        Dim stTemp As stMemberInfo = Nothing

        Try
            Dim intMemID As Integer = 0
            Dim intRel As Integer = clsEnum.emRelation.NATURAL

            Dim objCard As FamilyCard

            'clear panel
            xClearChildCard()

            'reset
            mlstChild.Clear()
            mlstChildCard.Clear()

            'get all 
            'tblData = gobjDB.fncGetKids(intFaID, intMoID)
            tblData = basCommon.fncGetKids(intFaID, intMoID)

            If tblData Is Nothing Then Exit Function

            For i As Integer = 0 To tblData.Rows.Count - 1

                'get id and relation id
                Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i).Item("MEMBER_ID")), intMemID)
                Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i).Item("RELID")), intRel)

                'clear struc before reuse
                stTemp = Nothing
                stTemp.intID = intMemID
                stTemp.intRel = intRel

                'get info
                xGetDetailInfo(stTemp)

                'create new card then add to list
                objCard = New FamilyCard()

                'add card to list
                mlstChildCard.Add(objCard)

                'add infor to list
                mlstChild.Add(stTemp)

                'fill card
                xFillCard(mlstChildCard(i), stTemp)

                'add handler
                'AddHandler mlstChildCard(i).MouseDoubleClick, AddressOf xCard_DoubleClick
                AddHandler mlstChildCard(i).LabelClick, AddressOf xLink_Click
                AddHandler mlstChildCard(i).CardClick, AddressOf xCard_Click

                'add menu
                'mlstChildCard(i).ContextMenuStrip = mmnuContext


            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetChild", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            stTemp = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xClearChildCard, clear child card from panel
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xClearChildCard() As Boolean

        xClearChildCard = False

        Try
            'remove child card from panel
            For i As Integer = 0 To mlstChildCard.Count - 1

                If mlstChildCard(i).PersonPicImage IsNot Nothing Then mlstChildCard(i).PersonPicImage.Dispose()
                mpnDrawPanel.Controls.Remove(mlstChildCard(i))

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClearChildCard", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xClearCard, clear card
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : objCard FamilyCard, card to clear
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xClearCard(ByRef objCard As FamilyCard) As Boolean

        xClearCard = False

        Try
            'clear card
            objCard.PersonID = basConst.gcintNO_MEMBER

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClearCard", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddNewParent, add new father or mother
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMember  Integer
    '      PARAMS     : emRel      clsEnum.emRelation
    '      PARAMS     : blnAddFather  Boolean
    '      PARAMS     : emCardGender  clsEnum.emGender
    '      PARAMS     : blnIsRollBack  Boolean
    '      PARAMS     : intNewID  Integer
    '      PARAMS     : blnAddSpouse  Boolean
    '      MEMO       : 
    '      CREATE     : 2011/11/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddNewParent(ByVal intMember As Integer, _
                                   ByVal emRel As clsEnum.emRelation, _
                                   ByVal blnAddFather As Boolean, _
                                   ByVal emCardGender As clsEnum.emGender, _
                                   Optional ByVal blnIsRollBack As Boolean = True, _
                                   Optional ByVal intNewID As Integer = basConst.gcintNO_MEMBER, _
                                   Optional ByVal blnAddSpouse As Boolean = True) As Boolean

        xAddNewParent = False

        Dim blnBeginTrans As Boolean = False
        Dim blnSuccess As Boolean = False

        Try
            Dim intFatherID As Integer
            Dim intMotherID As Integer
            Dim intTempID As Integer

            Dim intRoot As Integer
            Dim intMemberLevel As Integer           'level of child
            Dim blnUnderRoot As Boolean             'flag if a member is downline of root

            'add new member
            If intNewID = basConst.gcintNO_MEMBER Then _
                If Not xAddNewMember(intNewID, emCardGender) Then Exit Function

            'get father and mother of current member
            basCommon.fncGetFaMoID(intMember, intFatherID, intMotherID)

            'exit if parent is already exit
            If intNewID = intFatherID Or intNewID = intMotherID Then Exit Function

            'exit if new member is same with current member
            If intNewID = intMember Then Exit Function

            'start stransaction
            If blnIsRollBack Then blnBeginTrans = gobjDB.BeginTransaction()

            'catch of duplication when insert new rel
            Try
                'add new father/mother
                blnSuccess = gobjDB.fncInsertRel(intMember, intNewID, emRel, False)

                'adopt relation does not do the processes below
                If emRel <> clsEnum.emRelation.ADOPT Then

                    'also mark father and mother as husband and wife
                    If intFatherID > basConst.gcintNO_MEMBER Then intTempID = intFatherID
                    If intMotherID > basConst.gcintNO_MEMBER Then intTempID = intMotherID

                    If intTempID > basConst.gcintNO_MEMBER And blnAddSpouse Then

                        blnSuccess = gobjDB.fncInsertRel(intTempID, intNewID, clsEnum.emRelation.MARRIAGE, False) And blnSuccess
                        blnSuccess = gobjDB.fncInsertRel(intNewID, intTempID, clsEnum.emRelation.MARRIAGE, False) And blnSuccess

                    End If

                    'if current member is the ancentor, his father will become new ancentor
                    'If basCommon.fncIsAncentor(intMember, basConst.gcintRootID) _
                    '    And blnAddFather And mobjHusbandCard.PersonGender = clsEnum.emGender.MALE Then

                    '    'reset current father
                    '    blnSuccess = gobjDB.fncDelRel(intMember, basConst.gcintRootID, False) And blnSuccess

                    '    'mark new member is ancentor
                    '    blnSuccess = gobjDB.fncInsertRel(intNewID, basConst.gcintRootID, clsEnum.emRelation.NATURAL, False) And blnSuccess

                    'End If

                End If

                'get generation of current member
                intMemberLevel = gobjDB.fncGetMemberGeneration(intMember)
                intRoot = basCommon.fncGetRoot()

                'if child has generation -> father does too.
                If intMemberLevel > 0 Then

                    blnUnderRoot = basCommon.fncIsDownLineOf(intRoot, intMember)

                    'if child is in family -> father has generation
                    If blnUnderRoot Then
                        blnSuccess = blnSuccess And gobjDB.fncSetMemberGeneration(intMemberLevel - 1, intNewID, False)
                    End If

                End If

            Catch ex As Exception
                blnSuccess = False
            End Try

            'exit if don't need to commit
            If Not blnIsRollBack Then

                If blnSuccess Then
                    Return True
                Else : Return False
                End If

            End If

            'commit if success
            If blnBeginTrans And blnSuccess Then

                gobjDB.Commit()

            Else
                'fail - rollback
                gobjDB.RollBack()

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddNewParent", ex)
            If blnBeginTrans Then gobjDB.RollBack()
        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddNewChild, add new child
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intFather  Integer
    '      PARAMS     : intMother  Integer
    '      PARAMS     : emRel  clsEnum.emRelation
    '      PARAMS     : emCardGender  clsEnum.emGender
    '      PARAMS     : blnIsRollBack  Boolean
    '      PARAMS     : intNewID  Integer
    '      MEMO       : 
    '      CREATE     : 2011/11/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddNewChild(ByVal intFather As Integer, _
                                  ByVal intMother As Integer, _
                                  ByVal emRel As clsEnum.emRelation, _
                                  ByVal emCardGender As clsEnum.emGender, _
                                  Optional ByVal blnIsRollBack As Boolean = True, _
                                  Optional ByVal intNewID As Integer = basConst.gcintNO_MEMBER) As Boolean

        xAddNewChild = False

        Dim blnBeginTrans As Boolean = False
        Dim blnSuccess As Boolean = True

        Try
            Dim intRootID As Integer
            Dim intParentLevel As Integer           'generation of parent
            Dim blnUnderRoot As Boolean             'flag if parent is under root

            'add new member
            If intNewID = basConst.gcintNO_MEMBER Then _
                If Not xAddNewMember(intNewID, emCardGender, intFather, intMother) Then Exit Function

            'start stransaction
            If blnIsRollBack Then blnBeginTrans = gobjDB.BeginTransaction()

            'catch of duplication when insert new rel
            Try
                'if new member is father/mother (dragdrop catching)
                If intNewID = intFather Or intNewID = intMother Then
                    blnSuccess = False
                    Exit Try
                End If

                'add father and mother for this child
                If intFather <> basConst.gcintNO_MEMBER Then blnSuccess = gobjDB.fncInsertRel(intNewID, intFather, emRel, False) And blnSuccess
                If intMother <> basConst.gcintNO_MEMBER Then blnSuccess = gobjDB.fncInsertRel(intNewID, intMother, emRel, False) And blnSuccess

                'start setting generation process
                intParentLevel = gobjDB.fncGetMemberGeneration(intFather)
                If intParentLevel < 0 Then intParentLevel = gobjDB.fncGetMemberGeneration(intMother)

                'continue process if generation is available
                If intParentLevel > 0 Then

                    intRootID = basCommon.fncGetRoot()
                    If intRootID = intFather Or intRootID = intMother Then
                        blnUnderRoot = True
                    Else
                        blnUnderRoot = gobjDB.fncIsDownlineOf(intRootID, intFather, intMother)
                    End If

                    If blnUnderRoot Then
                        blnSuccess = blnSuccess And gobjDB.fncSetMemberGeneration(intParentLevel + 1, intNewID, False)
                    End If

                End If

            Catch ex As Exception
                blnSuccess = False
            End Try

            'exit if don't need to commit
            If Not blnIsRollBack Then

                If blnSuccess Then
                    Return True
                Else : Return False
                End If

            End If

            'commit if success
            If blnBeginTrans And blnSuccess Then

                gobjDB.Commit()

            Else
                'fail - rollback
                gobjDB.RollBack()

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddNewChild", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddNewSpouse, add new spouse
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMember  Integer
    '      PARAMS     : blnAddChild  Boolean
    '      PARAMS     : blnIsRollBack  Boolean
    '      PARAMS     : intNewID  Integer
    '      MEMO       : 
    '      CREATE     : 2011/11/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddNewSpouse(ByVal intMember As Integer, _
                                   ByVal blnAddChild As Boolean, _
                                   Optional ByVal blnIsRollBack As Boolean = True, _
                                   Optional ByVal intNewID As Integer = basConst.gcintNO_MEMBER) As Boolean

        xAddNewSpouse = False

        Dim blnBeginTrans As Boolean = False
        Dim blnSuccess As Boolean = True

        Try
            Dim emCardGender As clsEnum.emGender
            Dim intRoot As Integer
            Dim intMemLv As Integer
            Dim blnUnderRoot As Boolean

            'set gender for new member
            emCardGender = clsEnum.emGender.FEMALE
            If mstActive.intGender = clsEnum.emGender.FEMALE Then emCardGender = clsEnum.emGender.MALE

            'for first load
            If mstActive.intID = basConst.gcintNO_MEMBER Then emCardGender = clsEnum.emGender.MALE

            'add new member
            If intNewID = basConst.gcintNO_MEMBER Then _
                If Not xAddNewMember(intNewID, emCardGender) Then Exit Function

            'start stransaction
            If blnIsRollBack Then blnBeginTrans = gobjDB.BeginTransaction()

            'catch of duplication when insert new rel
            Try
                'if it's the first load, there is no active member
                If mstActive.intID <= basConst.gcintNO_MEMBER Then

                    If emCardGender <> clsEnum.emGender.MALE Then Return True

                    'set active id
                    mstActive.intID = intNewID
                    mintActiveID = intNewID

                    'set as ancentor
                    'blnSuccess = gobjDB.fncInsertRel(intNewID, basConst.gcintRootID, clsEnum.emRelation.NATURAL, False) And blnSuccess

                    'set as root
                    blnSuccess = gobjDB.fncInsertRoot(intNewID, False) And blnSuccess
                    blnSuccess = blnSuccess And gobjDB.fncSetMemberGeneration(My.Settings.intInitGeneration, intNewID, False)

                    'set flag is in family
                    'blnSuccess = gobjDB.fncUpdateFamilyFlag(intNewID, clsEnum.emFamily_Flag.IN_FAMILY, False) And blnSuccess

                    'enable parent card
                    xEnableControl()

                    Return True

                End If

                'add new spouse - add 2 record for 2 way relationship
                blnBeginTrans = gobjDB.fncInsertRel(intNewID, intMember, clsEnum.emRelation.MARRIAGE, False) And blnSuccess
                blnBeginTrans = gobjDB.fncInsertRel(intMember, intNewID, clsEnum.emRelation.MARRIAGE, False) And blnSuccess

                'get generation of current member
                intMemLv = gobjDB.fncGetMemberGeneration(intMember)
                intRoot = basCommon.fncGetRoot()

                'if husband has generation -> wife does too.
                If intMemLv > 0 Then

                    If intMember = intRoot Then
                        'if member is root
                        blnSuccess = blnSuccess And gobjDB.fncSetMemberGeneration(intMemLv, intNewID, False)

                    Else
                        'if member is not root
                        blnUnderRoot = basCommon.fncIsDownlineOf(intRoot, intMember)

                        'if husband is in family -> wife has generation
                        If blnUnderRoot Then
                            blnSuccess = blnSuccess And gobjDB.fncSetMemberGeneration(intMemLv, intNewID, False)
                        End If

                    End If

                End If

                'add the new spouse is mother/father of current children
                If blnAddChild Then
                    For i As Integer = 0 To mlstChildCard.Count - 1
                        ' ▽ 2013/03/29  AKB Quyet （変更内容）*********************************
                        'blnBeginTrans = gobjDB.fncInsertRel(mlstChildCard(i).PersonID, intNewID, clsEnum.emRelation.NATURAL, False) And blnSuccess

                        blnSuccess = gobjDB.fncInsertRel(mlstChildCard(i).PersonID, intNewID, clsEnum.emRelation.NATURAL, False) And blnSuccess
                        ' △ 2013/03/29  AKB Quyet *********************************************
                    Next
                End If

            Catch ex As Exception
                blnSuccess = False
            End Try

            'exit if don't need to commit
            If Not blnIsRollBack Then

                If blnSuccess Then
                    Return True
                Else : Return False
                End If

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddNewSpouse", ex)
            If blnBeginTrans Then gobjDB.RollBack()
        Finally

            'do nothing if don't need to commit
            If blnIsRollBack Then

                'commit if success
                If blnBeginTrans And blnSuccess Then

                    gobjDB.Commit()

                Else
                    'fail - rollback
                    gobjDB.RollBack()

                End If

            End If

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddNewSpouse, add new spouse, handle link click
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSpouseListAdd()

        Try
            Dim blnSuccess As Boolean = True
            Dim intNewMem As Integer
            Dim intGender As clsEnum.emGender
            Dim emGender As clsEnum.emGender

            If mstActive.intID <= basConst.gcintNO_MEMBER Then Exit Sub

            intGender = clsEnum.emGender.FEMALE
            If mstActive.intGender = clsEnum.emGender.FEMALE Then intGender = clsEnum.emGender.MALE

            'add new member and get new id
            blnSuccess = blnSuccess And xAddNewMember(intNewMem, intGender)

            If Not blnSuccess Then Exit Sub

            If mstSpouse.intID = basConst.gcintNO_MEMBER Then
                'there is no spouse, the new spouse is fa/mo of current children
                blnSuccess = blnSuccess And xAddNewSpouse(mstActive.intID, True, True, intNewMem)
            Else
                blnSuccess = blnSuccess And xAddNewSpouse(mstActive.intID, False, True, intNewMem)
            End If

            If blnSuccess Then
                'get new data
                xGetData()
                xGetInfo()

                emGender = clsEnum.emGender.FEMALE
                If mstActive.intGender = clsEnum.emGender.FEMALE Then emGender = clsEnum.emGender.MALE

                'add to spouse list board
                For i As Integer = 0 To mlstSpouse.Count - 1
                    If mlstSpouse(i).intID <> intNewMem Then Continue For

                    mobjSpouseList.fncAddItem(emGender, intNewMem, mlstSpouse(i).strFullName)

                Next

                'raise refresh event to update quick search grid in main form
                RaiseEvent evnRefresh(mintActiveID, False)

                'show new member
                xSpouseChange(intNewMem)

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSpouseListAdd", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xSpouseListAddFromList, add new spouse, handle link click
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSpouseListAddFromList()

        Try
            xSpouseMenuNewFromList()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSpouseListAddFromList", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xSpouseListClicked, change spouse index
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xChangeSpouseIndex()

        Dim fncRel As New frmRelationMem
        Try
            fncRel.RootID = mstActive.intID
            fncRel.FormMode = frmRelationMem.emRelMode.Spouse
            AddHandler fncRel.evnRefresh, AddressOf xRaiseRefreshEvent
            AddHandler fncRel.evnRefreshRelMemList, AddressOf fncRefreshSpouseList
            fncRel.fncShow()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmPersonalSpouse_Click", ex)
        Finally
            fncRel.Dispose()
            fncRel = Nothing
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xRaiseRefreshEvent, raise event
    '      PARAMS     : 
    '      MEMO       : use for method xChangeSpouseIndex() only
    '      CREATE     : 2012/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xRaiseRefreshEvent()
        Try
            RaiseEvent evnRefresh(mintActiveID, True)
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRaiseRefreshEvent", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xAddNewMember, add new member
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intReturnMember  Integer
    '      PARAMS     : emCardGender  clsEnum.emGender
    '      MEMO       : 
    '      CREATE     : 2011/11/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddNewMember(ByRef intReturnMember As Integer, ByRef emCardGender As clsEnum.emGender, _
                                   Optional ByVal intFather As Integer = -1, _
                                   Optional ByVal intMother As Integer = -1) As Boolean

        xAddNewMember = False

        Try
            'set form mode to ADD 
            mfrmPerInfo.FormMode = clsEnum.emMode.ADD

            'set gender for add-member form
            mfrmPerInfo.MemberGender = clsEnum.emGender.UNKNOW
            If emCardGender = clsEnum.emGender.MALE Then mfrmPerInfo.MemberGender = clsEnum.emGender.MALE
            If emCardGender = clsEnum.emGender.FEMALE Then mfrmPerInfo.MemberGender = clsEnum.emGender.FEMALE

            'show form
            If Not mfrmPerInfo.fncShowForm(False, False, intFather, intMother) Then Exit Function

            'if member is added
            If Not mfrmPerInfo.FormModified Then Exit Function

            'get new member id and gender
            intReturnMember = mfrmPerInfo.MemberID
            emCardGender = mfrmPerInfo.MemberGender

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddNewMember", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDeleteFaMoRel, delete relationship with current member
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMemId  Integer
    '      PARAMS     : blnDelFather  Boolean, true - del father, false - del mother
    '      MEMO       : 
    '      CREATE     : 2011/11/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDeleteFaMoRel(ByVal intMemId As Integer, ByVal blnDelFather As Boolean) As Boolean

        xDeleteFaMoRel = False

        Dim blnSuccess As Boolean = True

        Try
            Dim intFaID As Integer
            Dim intMoID As Integer

            'get father and mother id of current member
            If Not basCommon.fncGetFaMoID(intMemId, intFaID, intMoID) Then Exit Function

            'delete father or mother
            If blnDelFather Then

                'if father exist
                If intFaID >= 0 Then blnSuccess = gobjDB.fncDelRel(intMemId, intFaID, False)

            Else

                'if mother exist
                If intMoID >= 0 Then blnSuccess = gobjDB.fncDelRel(intMemId, intMoID, False)

            End If

            If Not blnSuccess Then Return False

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDeleteFaMoRel", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xChangeParentRelation, change relationship
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMemId  Integer
    '      PARAMS     : blnChangeFather  Boolean, true - father, false - mother
    '      PARAMS     : emNewRel  clsEnum.emRelation
    '      PARAMS     : blnIsRollBack  Boolean
    '      MEMO       : 
    '      CREATE     : 2011/11/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xChangeParentRelation(ByVal intMemId As Integer, _
                                           ByVal blnChangeFather As Boolean, _
                                           ByVal emNewRel As clsEnum.emRelation, _
                                           Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        xChangeParentRelation = False

        Dim blnBeginTrans As Boolean = False
        Dim blnSuccess As Boolean = False

        Try
            Dim intFaID As Integer
            Dim intMoID As Integer
            Dim intTemID As Integer

            'get father and mother id of current member
            If Not basCommon.fncGetFaMoID(intMemId, intFaID, intMoID) Then Exit Function

            intTemID = intFaID
            If Not blnChangeFather Then intTemID = intMoID

            'do nothing if there is no member
            If intTemID < 0 Then Return True

            If blnIsRollBack Then blnBeginTrans = gobjDB.BeginTransaction()

            'delete current relationship
            blnSuccess = gobjDB.fncDelRel(intMemId, intTemID, False)

            'insert new relationship
            blnSuccess = gobjDB.fncInsertRel(intMemId, intTemID, emNewRel, False)

            If Not blnIsRollBack Then Return True

            'commit if success
            If blnBeginTrans And blnSuccess Then

                gobjDB.Commit()

            Else
                'fail - rollback
                gobjDB.RollBack()

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xChangeParentRelation", ex)
            If blnBeginTrans Then gobjDB.RollBack()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xChangeFaMo, change parents
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : emSelect  clsEnum.emSelect, selected case
    '      PARAMS     : intMemId  Integer
    '      PARAMS     : emGender  clsEnum.emGende
    '      PARAMS     : intNewId  Integer
    '      MEMO       : 
    '      CREATE     : 2011/11/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xChangeFaMo(ByVal emSelect As clsEnum.emSelect, _
                                 ByVal intMemId As Integer, _
                                 ByVal emGender As clsEnum.emGender, _
                                 Optional ByVal blnDelBothFaMo As Boolean = False, _
                                 Optional ByVal intNewId As Integer = gcintNO_MEMBER) As Boolean

        xChangeFaMo = False
        Dim blnTrans As Boolean = False
        Dim blnSuccess As Boolean = True

        Try
            Dim intFather As Integer
            Dim intMother As Integer

            blnTrans = gobjDB.BeginTransaction()

            'get father and mother of current member
            basCommon.fncGetFaMoID(intMemId, intFather, intMother)

            Select Case emSelect

                'delete current relationship then add new relationship
                Case clsEnum.emSelect.CASE1

                    'delete current relation with father and add new father
                    If blnDelBothFaMo Then
                        blnSuccess = xDeleteFaMoRel(intMemId, True) And blnSuccess
                        blnSuccess = xDeleteFaMoRel(intMemId, False) And blnSuccess
                    Else
                        If emGender = clsEnum.emGender.MALE Then blnSuccess = xDeleteFaMoRel(intMemId, True) And blnSuccess
                        If emGender = clsEnum.emGender.FEMALE Then blnSuccess = xDeleteFaMoRel(intMemId, False) And blnSuccess
                    End If

                    'add new father or mother
                    If intNewId = gcintNO_MEMBER Then
                        If emGender = clsEnum.emGender.MALE Then blnSuccess = xAddNewParent(intMemId, clsEnum.emRelation.NATURAL, True, emGender, False) And blnSuccess
                        If emGender = clsEnum.emGender.FEMALE Then blnSuccess = xAddNewParent(intMemId, clsEnum.emRelation.NATURAL, False, emGender, False) And blnSuccess
                    Else
                        If emGender = clsEnum.emGender.MALE Then blnSuccess = xAddNewParent(intMemId, clsEnum.emRelation.NATURAL, True, emGender, False, intNewId, False) And blnSuccess
                        If emGender = clsEnum.emGender.FEMALE Then blnSuccess = xAddNewParent(intMemId, clsEnum.emRelation.NATURAL, False, emGender, False, intNewId, False) And blnSuccess
                    End If

                    'current relationship is adoptive, new one is blood
                Case clsEnum.emSelect.CASE2

                    'change current relationship to adopt
                    If blnDelBothFaMo Then
                        If intFather >= 0 Then blnSuccess = xChangeParentRelation(intMemId, True, clsEnum.emRelation.ADOPT, False) And blnSuccess
                        If intMother >= 0 Then blnSuccess = xChangeParentRelation(intMemId, False, clsEnum.emRelation.ADOPT, False) And blnSuccess
                    Else
                        If emGender = clsEnum.emGender.MALE Then blnSuccess = xChangeParentRelation(intMemId, True, clsEnum.emRelation.ADOPT, False) And blnSuccess
                        If emGender = clsEnum.emGender.FEMALE Then blnSuccess = xChangeParentRelation(intMemId, False, clsEnum.emRelation.ADOPT, False) And blnSuccess
                    End If

                    'new relationship is blood
                    If intNewId = gcintNO_MEMBER Then
                        blnSuccess = xAddNewParent(intMemId, clsEnum.emRelation.NATURAL, True, emGender, False) And blnSuccess
                    Else
                        blnSuccess = xAddNewParent(intMemId, clsEnum.emRelation.NATURAL, True, emGender, False, intNewId, False) And blnSuccess
                    End If

                    'new relation is adopt
                Case clsEnum.emSelect.CASE3
                    If intNewId = gcintNO_MEMBER Then
                        blnSuccess = xAddNewParent(intMemId, clsEnum.emRelation.ADOPT, True, emGender, False) And blnSuccess
                    Else
                        blnSuccess = xAddNewParent(intMemId, clsEnum.emRelation.ADOPT, True, emGender, False, intNewId, False) And blnSuccess
                    End If

            End Select

            If blnTrans And blnSuccess Then
                gobjDB.Commit()
            Else
                gobjDB.RollBack()
                Return False
            End If

            Return True

        Catch ex As Exception
            If blnTrans Then gobjDB.RollBack()
            basCommon.fncSaveErr(mcstrClsName, "xChangeFaMo", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xShowInfoForm, show personal infor form
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : intID   Integer, member id to show
    '      PARAMS2    : objcard FamilyCard, card instance
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xShowInfoForm(ByVal intID As Integer, Optional ByVal objcard As FamilyCard = Nothing) As Boolean

        xShowInfoForm = False

        Try
            mfrmPerInfo.MemberID = intID
            mfrmPerInfo.FormMode = clsEnum.emMode.EDIT
            mfrmPerInfo.MemberGender = clsEnum.emGender.UNKNOW

            'show form 
            If Not mfrmPerInfo.fncShowForm() Then Exit Function

            'if member is edied
            If Not mfrmPerInfo.FormModified Then Exit Function

            'get new data
            xGetData()

            If objcard Is Nothing Then

                objcard = mobjWifeCard
                If mstActive.intGender = clsEnum.emGender.MALE Or mstActive.intGender = clsEnum.emGender.UNKNOW Then objcard = mobjHusbandCard

            End If

            'Redraw the card
            xRedrawCard(objcard)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowInfoForm", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xRedrawCard, redraw card
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : objCard FamilyCard, card instance
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 2011/12/13  AKB Quyet
    '   ******************************************************************
    Private Function xRedrawCard(ByVal objCard As FamilyCard) As Boolean

        xRedrawCard = False

        Try
            'always redraw child because we cleared child card in some other functions
            xGetChild(mstActive.intID, mstSpouse.intID)

            'get center point
            xGetCenterPoint()

            xDrawChild()
            xDrawAddChild()

            'redraw the card
            If objCard Is mobjHusbandCard Or objCard Is mobjWifeCard Then

                'get active member's detail infor
                xGetDetailInfo(mstActive)

                'get his/her spouse
                xGetSpouse(mstActive.intID, mstSpouse)

                xDrawMember()

            End If

            If objCard Is mobjFatherCard Or objCard Is mobjMotherCard Then

                'get parent
                xGetParent(mstActive.intID, mstFather, mstMother)
                xDrawParent()

            End If

            xSetDisplayArea()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRedrawCard", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xEnableControl, enable / disable card
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xEnableControl() As Boolean

        xEnableControl = False

        Try
            'disable control when there is no active member
            mobjFatherCard.Enabled = True
            mobjMotherCard.Enabled = True
            mobjWifeCard.Enabled = True
            mobjAddChildCard.Enabled = True

            'disable parent if there is no active member
            If mstActive.intID <= 0 Then
                mobjFatherCard.Enabled = False
                mobjMotherCard.Enabled = False
                mobjWifeCard.Enabled = False
                mobjAddChildCard.Enabled = False
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDisableControl", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncClear, clear panel
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncClear() As Boolean

        fncClear = False

        Try
            'clear panel
            mpnDrawPanel.Controls.Clear()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncClear", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncClear, clear panel
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/11/17  AKB Quyet
    '      UPDATE     : 2011/12/13  AKB Quyet
    '   ******************************************************************
    Private Function xDrawLine() As Boolean

        Dim p As Pen = Nothing
        Dim g As Graphics = Nothing

        Try
            Dim intShortLine As Integer = 0
            Dim intLongLine As Integer = 14

            Dim ptFather As Point
            Dim ptMother As Point
            Dim ptActive As Point
            Dim pt1 As Point
            Dim pt2 As Point
            Dim pt3 As Point
            Dim pt4 As Point
            Dim pt5 As Point

            'pen to draw
            p = New Pen(Color.Blue, 1)
            p.DashStyle = Drawing2D.DashStyle.Dash

            'graphic
            g = mpnDrawPanel.CreateGraphics()

            'find point
            ptFather = mobjFatherCard.MidPointBottom
            ptMother = mobjMotherCard.MidPointBottom
            ptActive = mobjHusbandCard.MidPointTop
            If mstActive.intGender = clsEnum.emGender.FEMALE Then ptActive = mobjWifeCard.MidPointTop

            'points position
            '
            '        +-----+        +-----+           
            '        |     |        |     |
            '        +--f--+        +--m--+
            '          1|______3_______|2        
            '                  |
            '                  |
            '        5---------4  
            '        |
            '+-------a-------+    +---------------+ 
            '|               |    |               |
            '|               |    |               |
            '+---------------+    +---------------+

            'calculate shortline
            intShortLine = (ptActive.Y - ptFather.Y - intLongLine) \ 2

            'find point to draw
            pt1 = ptFather : pt1.Y = ptFather.Y + intShortLine
            pt2 = ptMother : pt2.Y = ptMother.Y + intShortLine
            'pt3 = pt1 : pt3.X = mptCenter.X
            pt3 = pt1 : pt3.X = mobjWifeCard.Location.X - mcintMARGIN
            pt4 = pt3 : pt4.Y = pt3.Y + intLongLine
            pt5 = ptActive : pt5.Y = ptActive.Y - intShortLine

            'clear
            g.Clear(Color.White)

            'draw lines
            g.DrawLine(p, ptFather, pt1)
            g.DrawLine(p, ptMother, pt2)
            g.DrawLine(p, pt1, pt2)
            g.DrawLine(p, pt3, pt4)
            g.DrawLine(p, pt4, pt5)
            g.DrawLine(p, pt5, ptActive)

            'draw border
            If mstActive.intGender = clsEnum.emGender.FEMALE Then
                g.FillRectangle(Brushes.Gray, mobjWifeCard.Location.X - 2, mobjWifeCard.Location.Y - 2, clsDefine.CARD_LARG_W + 4, clsDefine.CARD_LARG_H + 4)
            Else
                g.FillRectangle(Brushes.Gray, mobjHusbandCard.Location.X - 2, mobjHusbandCard.Location.Y - 2, clsDefine.CARD_LARG_W + 4, clsDefine.CARD_LARG_H + 4)
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawLine", ex)
        Finally
            If p IsNot Nothing Then p.Dispose()
            If g IsNot Nothing Then g.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xCreateCustomMenu, create menu for card
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function xCreateRightMenu() As Boolean

        xCreateRightMenu = False

        Try
            mmnuRightMouse = mobjRightMenu.fncGetMenu(mstActive.intID, mstActive.strFullNameWAlias, mstActive.intGender)

            If mstActive.intGender = clsEnum.emGender.MALE Or mstActive.intGender = clsEnum.emGender.UNKNOW Then
                mobjHusbandCard.ContextMenuStrip = mmnuRightMouse
            Else
                mobjWifeCard.ContextMenuStrip = mmnuRightMouse
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCreateCustomMenu", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSpouseMenuNew, handle menu item clicked
    '      MEMO       : 
    '      CREATE     : 2011/12/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSpouseMenuNew() As Boolean

        xSpouseMenuNew = False

        Dim blnSuccess As Boolean = True
        Dim blnTrans As Boolean = False

        Try
            Dim enGender As clsEnum.emGender
            Dim intNewID As Integer

            'determine gender
            enGender = clsEnum.emGender.FEMALE
            If mstActive.intGender = clsEnum.emGender.FEMALE Then enGender = clsEnum.emGender.MALE

            'start transaction
            blnTrans = gobjDB.BeginTransaction()

            'catch of duplication when insert new rel
            Try
                'create new member
                If Not xAddNewMember(intNewID, enGender) Then blnSuccess = False

                'insert new relation ship
                blnSuccess = gobjDB.fncInsertRel(intNewID, mstActive.intID, clsEnum.emRelation.MARRIAGE, False) And blnSuccess
                blnSuccess = gobjDB.fncInsertRel(mstActive.intID, intNewID, clsEnum.emRelation.MARRIAGE, False) And blnSuccess

            Catch ex As Exception
                blnSuccess = False
            End Try

            'check for rolling back or committing
            If blnTrans And blnSuccess Then
                gobjDB.Commit()
                xGetData()
                xGetInfo()
                'xCreateRightMenu()
                If mstActive.intGender = clsEnum.emGender.MALE Then
                    xRedrawCard(mobjHusbandCard)
                Else
                    xRedrawCard(mobjWifeCard)
                End If
            Else
                gobjDB.RollBack()
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSpouseMenuNew", ex)
            If blnTrans Then gobjDB.RollBack()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSpouseMenuDelete, delete spouse relationship
    '      PARAMS1    : sender  Object, card object
    '      PARAMS2    : e       System.EventArgs
    '      MEMO       : 
    '      CREATE     : 2011/12/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSpouseMenuDelete(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim blnSuccess As Boolean = True
        Dim blnTrans As Boolean = False

        Try
            'Dim enGender As clsEnum.emGender
            'Dim intNewID As Integer

            'If Not basCommon.fncMessageConfirm(mcstrDelSpouseRelation) Then Exit Sub

            ''determine gender
            'enGender = clsEnum.emGender.FEMALE
            'If mstActive.intGender = clsEnum.emGender.FEMALE Then enGender = clsEnum.emGender.MALE

            ''start transaction
            'blnTrans = gobjDB.BeginTransaction()

            ''catch for exception
            'Try
            '    'delete relationship

            'Catch ex As Exception
            '    blnSuccess = False
            'End Try

            ''check for rolling back or committing
            'If blnTrans And blnSuccess Then
            '    gobjDB.Commit()
            '    xGetData()
            '    xGetInfo()
            '    xCreateCustomMenu()
            '    If mstActive.intGender = clsEnum.emGender.MALE Then
            '        xRedrawCard(mobjHusbandCard)
            '    Else
            '        xRedrawCard(mobjWifeCard)
            '    End If
            'Else
            '    gobjDB.RollBack()
            'End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSpouseMenuDelete", ex)
            If blnTrans Then gobjDB.RollBack()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xSpouseMenuNewFromList, handle menu item clicked
    '      PARAMS1    : sender  Object, card object
    '      PARAMS2    : e       System.EventArgs
    '      MEMO       : 
    '      CREATE     : 2011/12/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSpouseMenuNewFromList() As Boolean

        xSpouseMenuNewFromList = False

        Dim frmMemList As frmPersonList = Nothing
        Dim blnSuccess As Boolean = True
        Dim blnTrans As Boolean = False

        Try
            Dim intNewID As Integer

            frmMemList = New frmPersonList()

            'show form and get member id
            If mstActive.intGender = clsEnum.emGender.FEMALE Then
                If Not frmMemList.fncShowForm(frmPersonList.enFormMode.SELECT_MEMBER, 0, clsEnum.emGender.MALE) Then Exit Function
            Else
                If Not frmMemList.fncShowForm(frmPersonList.enFormMode.SELECT_MEMBER, 0, clsEnum.emGender.FEMALE) Then Exit Function
            End If

            If Not frmMemList.MemberSelected Then Exit Function

            'get new member id
            intNewID = frmMemList.MemberID

            'start transaction
            blnTrans = gobjDB.BeginTransaction()

            'catch of duplication when insert new rel
            Try
                'insert new relation ship
                blnSuccess = gobjDB.fncInsertRel(intNewID, mstActive.intID, clsEnum.emRelation.MARRIAGE, False) And blnSuccess
                blnSuccess = gobjDB.fncInsertRel(mstActive.intID, intNewID, clsEnum.emRelation.MARRIAGE, False) And blnSuccess

            Catch ex As Exception
                blnSuccess = False
            End Try

            'check for rolling back or committing
            If blnTrans And blnSuccess Then
                gobjDB.Commit()
                xGetData()
                xGetInfo()
                'xCreateRightMenu()
                'redraw
                Me.ActiveMemberID = mstActive.intID
            Else
                gobjDB.RollBack()
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSpouseMenuNewFromList", ex)
            If blnTrans Then gobjDB.RollBack()
        Finally
            If frmMemList IsNot Nothing Then frmMemList.Dispose()
        End Try

    End Function

#End Region


#Region "Event handler"


    '   ******************************************************************
    '　　　FUNCTION   : xAddHandler, add handler of cards
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddHandler() As Boolean

        xAddHandler = False

        Try
            'Dim mni1 As ToolStripMenuItem

            'handler for panel
            AddHandler mpnDrawPanel.Paint, AddressOf xPaintLine

            'handle click on card
            AddHandler mobjHusbandCard.CardClick, AddressOf xCard_Click
            AddHandler mobjWifeCard.CardClick, AddressOf xCard_Click
            AddHandler mobjFatherCard.CardClick, AddressOf xCard_Click
            AddHandler mobjMotherCard.CardClick, AddressOf xCard_Click

            'handle click on Member Name Label
            AddHandler mobjHusbandCard.LabelClick, AddressOf xLink_Click
            AddHandler mobjWifeCard.LabelClick, AddressOf xLink_Click
            AddHandler mobjFatherCard.LabelClick, AddressOf xLink_Click
            AddHandler mobjMotherCard.LabelClick, AddressOf xLink_Click
            AddHandler mobjAddChildCard.LabelClick, AddressOf xLink_Click

            'handle click on image
            AddHandler mobjHusbandCard.ImageClick, AddressOf xImageClick
            AddHandler mobjWifeCard.ImageClick, AddressOf xImageClick

            'handle spouse list clicked
            AddHandler mobjSpouseList.evnSpouseChange, AddressOf xSpouseListClick
            AddHandler mobjSpouseList.evnAddMember, AddressOf xSpouseListAdd
            AddHandler mobjSpouseList.evnSubLinkClicked, AddressOf xSpouseListAddFromList
            AddHandler mobjSpouseList.evnSpouseListClicked, AddressOf xChangeSpouseIndex

            'handle dragdrop event
            AddHandler mobjFatherCard.DragEnter, AddressOf xCard_DragEnter
            AddHandler mobjMotherCard.DragEnter, AddressOf xCard_DragEnter
            AddHandler mobjHusbandCard.DragEnter, AddressOf xCard_DragEnter
            AddHandler mobjWifeCard.DragEnter, AddressOf xCard_DragEnter
            AddHandler mobjAddChildCard.DragEnter, AddressOf xCard_DragEnter

            AddHandler mobjFatherCard.CardDragDrop, AddressOf xCard_DragDrop
            AddHandler mobjMotherCard.CardDragDrop, AddressOf xCard_DragDrop
            AddHandler mobjHusbandCard.CardDragDrop, AddressOf xCard_DragDrop
            AddHandler mobjWifeCard.CardDragDrop, AddressOf xCard_DragDrop
            AddHandler mobjAddChildCard.CardDragDrop, AddressOf xCard_DragDrop

            'handle click on link child manager
            AddHandler mobjBoardCard.ManageChildClick, AddressOf xManageChild_Click
            AddHandler mobjBoardCard.DelSpouseRel, AddressOf xDelSpouseRel

            '|¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯|
            '|handler for child card is added in xGetChild function|
            '|_____________________________________________________|

            'handler for contextmenustrip
            'AddHandler mmnuContext.Opening, AddressOf xMenuOpening
            AddHandler mobjRightMenu.evnMenuItemClick, AddressOf xMenuItemClick
            AddHandler mobjRightMenu.evnSpouseChange, AddressOf xSpouseChange


            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddHandler", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xPaintLine, draw lines between cards
    '      PARAMS     : sender  Object, label object
    '      PARAMS     : e       MouseEventArgs, mouse event
    '      MEMO       : 
    '      CREATE     : 2011/11/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xPaintLine(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

        Try
            xDrawLine()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawLine", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCard_Click, handle click on card
    '      PARAMS     : sender  Object, card object
    '      PARAMS     : e       EventArgs, mouse event
    '      PARAMS     : intID   Integer, id on card
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCard_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs, ByVal intID As Integer)

        Dim objEffect As clsMovingEffect = Nothing

        Try
            Dim ptEnd As Point
            Dim objCard As FamilyCard

            'exit if it's right click
            If e.Button <> MouseButtons.Left Then Exit Sub

            'exit if member doesn't change or there is no member
            If intID <= basConst.gcintNO_MEMBER Or intID = mintActiveID Then Exit Sub

            'update new member
            'mintActiveID = intID

            'get card object
            objCard = CType(sender, FamilyCard)


            If objCard Is mobjHusbandCard Or objCard Is mobjWifeCard Then

                'doesn't do any effect if the card is husband or wife
                Me.ActiveMemberID = intID

            Else

                'create new effect object and determine moving location
                objEffect = New clsMovingEffect(Me, mpnDrawPanel, basConst.gcintTimerInterval, basConst.gcintAnimateTime)
                ptEnd = mobjHusbandCard.Location
                If objCard.PersonGender = clsEnum.emGender.FEMALE Then ptEnd = mobjWifeCard.Location

                'do effect
                objEffect.fncStartEffect(objCard.Location, ptEnd, intID)

            End If

            'raise event to change mintId in main form
            RaiseEvent evnCardClick(intID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCard_Click", ex)
        Finally
            objEffect = Nothing
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xLink_Click, handle click on Member Name Label
    '      PARAMS     : sender  Object, label object
    '      PARAMS     : e       MouseEventArgs, mouse event
    '      PARAMS     : intID   Integer, id on card
    '      PARAMS     : objCard FamilyCard, card instance
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xLink_Click(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs, ByVal intID As Integer, ByVal objCard As FamilyCard)

        Dim blnBeginTrans As Boolean
        Dim blnSuccess As Boolean

        Try
            Dim objLabel As LinkLabel
            Dim strText As String
            Dim intTempID As Integer
            Dim intCurMo As Integer             'current mother
            Dim blnAddChild As Boolean = False

            'exit if it is right click
            If e.Button = MouseButtons.Right Then Exit Sub

            objLabel = CType(sender, LinkLabel)
            strText = objLabel.Text
            blnBeginTrans = False
            blnSuccess = True
            intTempID = basConst.gcintNO_MEMBER

            'if there is an member - show detail form
            If intID > basConst.gcintNO_MEMBER Then

                xShowInfoForm(intID, objCard)
                Exit Sub

            End If

            '============ ▼add new member▼ =============

            blnBeginTrans = gobjDB.BeginTransaction()

            'set relationship for new member
            Select Case strText

                Case My.Resources.StrAddFather
                    blnSuccess = xAddNewParent(mstActive.intID, clsEnum.emRelation.NATURAL, True, clsEnum.emGender.MALE, False) And blnSuccess

                Case My.Resources.StrAddMother
                    blnSuccess = xAddNewParent(mstActive.intID, clsEnum.emRelation.NATURAL, False, clsEnum.emGender.FEMALE, False) And blnSuccess

                Case My.Resources.StrAddChild

                    If mstActive.intGender = clsEnum.emGender.MALE Then
                        blnSuccess = xAddNewChild(mstActive.intID, mstSpouse.intID, clsEnum.emRelation.NATURAL, clsEnum.emGender.MALE, False) And blnSuccess
                    Else
                        blnSuccess = xAddNewChild(mstSpouse.intID, mstActive.intID, clsEnum.emRelation.NATURAL, clsEnum.emGender.MALE, False) And blnSuccess
                    End If


                    intCurMo = mstSpouse.intID
                    blnAddChild = True

                Case My.Resources.StrNewMem
                    blnSuccess = xAddNewSpouse(mstActive.intID, True, False) And blnSuccess

            End Select


            If blnSuccess And blnBeginTrans Then
                'If blnSuccess Then
                gobjDB.Commit()

                'get new data
                xGetData()
                xGetInfo()

                'redraw
                'if a new child added
                If blnAddChild Then
                    xSpouseChange(intCurMo)
                    blnAddChild = False
                Else
                    fncDraw()
                End If

                'raise refresh event to update quick search grid in main form
                RaiseEvent evnRefresh(mintActiveID, blnAddChild)

            Else
                If blnBeginTrans Then gobjDB.RollBack()

            End If

        Catch ex As Exception
            If blnBeginTrans Then gobjDB.RollBack()
            basCommon.fncSaveErr(mcstrClsName, "xLink_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xImageClick, handle click on image
    '      PARAMS     : objCard FamilyCard, id on card
    '      PARAMS     : intID   Integer, id on card
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xImageClick(ByVal objCard As FamilyCard, ByVal intID As Integer)

        Dim blnTrans As Boolean = False
        Dim blnSuccess As Boolean = True

        Try
            Dim strAvatar As String = String.Empty
            Dim strFileName As String = String.Empty

            'exit if there is no member
            If intID <= basConst.gcintNO_MEMBER Then Exit Sub

            'show dialog
            If Not basCommon.fncOpenFileDlg(strAvatar, basConst.gcstrImageFilter) Then Exit Sub

            'check validation of image and get path
            If Not basCommon.fncIsValidImage(strAvatar) Then Exit Sub


            Using frmCropt As New frmCropImage(strAvatar)

                frmCropt.ShowDialog()
                If Not frmCropt.ReturnOK Then Exit Sub

                strFileName = String.Format(basConst.gcstrImgFormat, intID)

                blnTrans = gobjDB.BeginTransaction()

                'try copying image file to "images" folder then set the path
                Try
                    'blnSuccess = basCommon.fncCreateThumbnailAndSave(strAvatar, basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath, strFileName, clsDefine.THUMBNAIL_W, clsDefine.THUMBNAIL_H) And blnSuccess
                    blnSuccess = basCommon.fncCreateThumbnailAndSave(frmCropt.PatientPicture, basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath, strFileName, clsDefine.THUMBNAIL_W, clsDefine.THUMBNAIL_H) And blnSuccess

                    'blnSuccess = basCommon.fncCopyFile(strAvatar, basConst.gcstrImageFolder & basConst.gcstrAvatarPath, strFileName, strFileName) And blnSuccess
                    blnSuccess = basCommon.fncSaveImage(frmCropt.PatientPicture, basConst.gcstrImageFolder & basConst.gcstrAvatarPath, strFileName, strAvatar) And blnSuccess

                    blnSuccess = gobjDB.fncUpdateAvatar(intID, strFileName, False) And blnSuccess

                Catch exx As Exception
                    basCommon.fncSaveErr(mcstrClsName, "xImageClick", exx)
                End Try


                If blnTrans And blnSuccess Then

                    gobjDB.Commit()

                    'show image
                    objCard.PersonPicLocate = strAvatar
                    'objCard.PersonPicImage = frmCropt.PatientPicture

                Else
                    gobjDB.RollBack()
                End If

            End Using

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xImageClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xManageChild_Click, handle click on link
    '      PARAMS1    : sender  Object, card object
    '      PARAMS2    : e       LinkLabelLinkClickedEventArgs, mouse event
    '      MEMO       : 
    '      CREATE     : 2011/08/30  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xManageChild_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)

        Dim fncRel As New frmRelationMem

        Try
            fncRel.RootID = mintActiveID
            fncRel.FormMode = frmRelationMem.emRelMode.Childs
            AddHandler fncRel.evnRefresh, AddressOf xRefresh
            AddHandler fncRel.evnRefreshRelMemList, AddressOf xRefreshChildOrder
            fncRel.fncShow()



        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xManageChild_Click", ex)

        Finally
            fncRel.Dispose()
            fncRel = Nothing

        End Try

    End Sub


    ''' <summary>
    ''' Refresh child order
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub xRefreshChildOrder()
        Try
            For i As Integer = 0 To mlstChildCard.Count - 1
                mlstChildCard(i).Dispose()
            Next

            xGetData()
            xGetChild(mstActive.intID, mstSpouse.intID)
            xDrawChild()
            xSetDisplayArea()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRefreshChildOrder", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCard_DragEnter, drag enter event
    '      PARAMS     : sender  Object, label object
    '      PARAMS     : e       MouseEventArgs, mouse event
    '      MEMO       : 
    '      CREATE     : 2011/11/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCard_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs)

        Try
            ' See if the data includes a integer value.
            If e.Data.GetDataPresent(GetType(Integer)) Then
                'Allow copy.
                e.Effect = DragDropEffects.Copy

            Else
                'Prohibit drop.
                e.Effect = DragDropEffects.None
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCard_DragEnter", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCard_DragDrop, drag drop event
    '      PARAMS     : sender  Object, label object
    '      PARAMS     : e       MouseEventArgs, mouse event
    '      PARAMS     : intCurMem   Integer, current id on card
    '      PARAMS     : strLabel    String,  label on card
    '      MEMO       : 
    '      CREATE     : 2011/11/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCard_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs, ByVal intCurMem As Integer, ByVal strLabel As String)

        Try
            Dim intNewID As Integer
            Dim blnSuccess As Boolean = True

            'get data
            intNewID = CType(e.Data.GetData(GetType(Integer)), Integer)

            'exit if there is a member
            If intCurMem > basConst.gcintNO_MEMBER Then Exit Sub

            'exit if new id already has a relationship
            If basCommon.fncHasRel(intNewID) Then Exit Sub

            'exit if new member is upline or downline of active member
            If basCommon.fncIsDownLineOf(intNewID, mstActive.intID) Or basCommon.fncIsDownLineOf(mstActive.intID, intNewID) Then Exit Sub

            'add new member
            Select Case strLabel
                Case My.Resources.StrAddFather, My.Resources.StrAddMother
                    blnSuccess = xAddNewParent(mstActive.intID, clsEnum.emRelation.NATURAL, True, clsEnum.emGender.MALE, True, intNewID)

                Case My.Resources.StrAddChild
                    'exit if new member is upline or downline of spouse member
                    If basCommon.fncIsDownLineOf(intNewID, mstSpouse.intID) Or basCommon.fncIsDownLineOf(mstSpouse.intID, intNewID) Then Exit Select
                    blnSuccess = xAddNewChild(mstActive.intID, mstSpouse.intID, clsEnum.emRelation.NATURAL, clsEnum.emGender.MALE, True, intNewID)

                Case My.Resources.StrNewMem
                    blnSuccess = xAddNewSpouse(mstActive.intID, True, True, intNewID)

            End Select

            If blnSuccess Then Me.ActiveMemberID = mintActiveID

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCard_DragDrop", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xMenuItemClick, handle menu item clicked
    '      PARAMS1    : sender  Object, card object
    '      PARAMS2    : e       System.EventArgs
    '      MEMO       : 
    '      CREATE     : 2011/11/23  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xMenuItemClick(ByVal intMemId As Integer, ByVal sender As System.Object)

        Dim dlgConfirm As frmConfirm = Nothing
        Dim frmMemList As frmPersonList = Nothing
        Dim frmSpouse As frmSpouseList = Nothing
        Dim blnSuccess As Boolean = False

        Try
            Dim objDropMenu As ToolStripDropDown = Nothing
            Dim objMenuItem As ToolStripMenuItem = Nothing
            Dim objOwnerItem As ToolStripMenuItem = Nothing
            Dim objMenu As ContextMenuStrip = Nothing
            Dim intID As Integer
            Dim intRootID As Integer
            Dim intFather As Integer
            Dim intMother As Integer
            Dim intCurMo As Integer
            Dim blnRefresh As Boolean
            Dim blnAddChild As Boolean
            Dim intHusband As Integer = mstActive.intID
            Dim intWife As Integer = mstSpouse.intID

            blnRefresh = True

            'get menu item
            objMenuItem = CType(sender, ToolStripMenuItem)

            dlgConfirm = New frmConfirm()
            frmMemList = New frmPersonList()

            'member ID
            intID = intMemId

            'get father and mother id
            basCommon.fncGetFaMoID(intID, intFather, intMother)
            If mstActive.intGender = clsEnum.emGender.FEMALE Then

                intHusband = mstSpouse.intID
                intWife = mstActive.intID


            End If

            Select Case objMenuItem.Text

                '================== ▼ Start add Father Mother ▼ ====================

                Case My.Resources.StrPersonInfo                                                              'show detail info
                    'if there is an member - show detail form
                    If intID > basConst.gcintNO_MEMBER Then xShowInfoForm(intID)

                Case My.Resources.StrAddFather                                                               'add father
                    'check if current member has in-blood relation
                    If basCommon.fncHasFaMo(intID, clsEnum.emGender.MALE) Then

                        If dlgConfirm.ShowDialog() = DialogResult.OK Then
                            blnSuccess = xChangeFaMo(dlgConfirm.SelectCase, intID, clsEnum.emGender.MALE, True)
                        End If

                    Else
                        blnSuccess = xAddNewParent(intID, clsEnum.emRelation.NATURAL, True, clsEnum.emGender.MALE)
                    End If

                Case My.Resources.StrAddMother                                                               'add mother
                    'check if current member has in-blood relation
                    If basCommon.fncHasFaMo(intID, clsEnum.emGender.FEMALE) Then

                        If dlgConfirm.ShowDialog() = DialogResult.OK Then

                            blnSuccess = xChangeFaMo(dlgConfirm.SelectCase, intID, clsEnum.emGender.FEMALE, True)

                        End If

                    Else
                        blnSuccess = xAddNewParent(intID, clsEnum.emRelation.NATURAL, True, clsEnum.emGender.FEMALE)
                    End If

                Case My.Resources.StrAddAdoptFather                                                          'add adopt father
                    blnSuccess = xAddNewParent(intID, clsEnum.emRelation.ADOPT, True, clsEnum.emGender.MALE)

                Case My.Resources.StrAddAdoptMother                                                          'add adopt mother
                    blnSuccess = xAddNewParent(intID, clsEnum.emRelation.ADOPT, False, clsEnum.emGender.FEMALE)

                Case My.Resources.StrDelFaMoRel                                                           'delete current relationship with parent
                    'Edit by: 2019.08.30 AKB TungNT
                    'If intFather >= 0 Then blnSuccess = gobjDB.fncDelRel(intID, intFather)
                    'If intMother >= 0 Then blnSuccess = gobjDB.fncDelRel(intID, intMother)
                    If mstFather.intID >= 0 Then blnSuccess = gobjDB.fncDelRel(intID, mstFather.intID)
                    If mstMother.intID >= 0 Then blnSuccess = gobjDB.fncDelRel(intID, mstMother.intID)
                    'Edit by: 2019.08.30 AKB TungNT
                Case My.Resources.StrAddFaMoFromList                                                         'add father or mother from list
                    frmSpouse = New frmSpouseList(intID)
                    If Not frmSpouse.fncShowForm() Then blnRefresh = False : Exit Select
                    If Not frmSpouse.MemberSelected Then blnRefresh = False : Exit Select

                    'check if current member has in-blood relation
                    If basCommon.fncHasFaMo(intID, clsEnum.emGender.MALE) Or basCommon.fncHasFaMo(intID, clsEnum.emGender.FEMALE) Then

                        If dlgConfirm.ShowDialog() = DialogResult.OK Then
                            blnSuccess = xChangeFaMo(dlgConfirm.SelectCase, intID, clsEnum.emGender.MALE, False, frmSpouse.HusbandId)
                            blnSuccess = xChangeFaMo(dlgConfirm.SelectCase, intID, clsEnum.emGender.FEMALE, False, frmSpouse.WifeId) And blnSuccess
                        End If

                    Else
                        blnSuccess = xAddNewParent(intID, clsEnum.emRelation.NATURAL, True, clsEnum.emGender.MALE, True, frmSpouse.HusbandId, False)
                        blnSuccess = xAddNewParent(intID, clsEnum.emRelation.NATURAL, True, clsEnum.emGender.FEMALE, True, frmSpouse.WifeId, False) And blnSuccess
                    End If


                    '================== ▼ Start add husband / wife ▼ ====================
                Case My.Resources.StrAddHusWif
                    blnSuccess = xSpouseMenuNew()

                Case My.Resources.StrAddHusWifFromList
                    blnSuccess = xSpouseMenuNewFromList() And blnSuccess

                    '================== ▼ Start add brother / sister ▼ ====================

                    'add brother
                Case My.Resources.StrAddBro
                    If mstFather.intID > basConst.gcintNO_MEMBER Or mstMother.intID > basConst.gcintNO_MEMBER Then
                        blnSuccess = xAddNewChild(intFather, intMother, clsEnum.emRelation.NATURAL, clsEnum.emGender.MALE)
                    Else
                        basCommon.fncMessageInfo(mcstrNoParent)
                    End If

                    'add sister
                Case My.Resources.StrAddSis
                    If mstFather.intID > basConst.gcintNO_MEMBER Or mstMother.intID > basConst.gcintNO_MEMBER Then
                        blnSuccess = xAddNewChild(intFather, intMother, clsEnum.emRelation.NATURAL, clsEnum.emGender.FEMALE)
                    Else
                        basCommon.fncMessageInfo(mcstrNoParent)
                    End If

                    'add youger brother
                Case My.Resources.StrAddYoBro
                    If mstFather.intID > basConst.gcintNO_MEMBER Or mstMother.intID > basConst.gcintNO_MEMBER Then
                        blnSuccess = xAddNewChild(intFather, intMother, clsEnum.emRelation.NATURAL, clsEnum.emGender.MALE)
                    Else
                        basCommon.fncMessageInfo(mcstrNoParent)
                    End If

                    'add younger sister
                Case My.Resources.StrAddYoSis
                    If mstFather.intID > basConst.gcintNO_MEMBER Or mstMother.intID > basConst.gcintNO_MEMBER Then
                        blnSuccess = xAddNewChild(intFather, intMother, clsEnum.emRelation.NATURAL, clsEnum.emGender.FEMALE)
                    Else
                        basCommon.fncMessageInfo(mcstrNoParent)
                    End If

                    'add brother/sister from list
                Case My.Resources.StrAddBroSisFromList
                    If mstFather.intID > basConst.gcintNO_MEMBER Or mstMother.intID > basConst.gcintNO_MEMBER Then

                        'show form and get member id
                        If Not frmMemList.fncShowForm(frmPersonList.enFormMode.SELECT_MEMBER) Then blnRefresh = False : Exit Select
                        If Not frmMemList.MemberSelected Then blnRefresh = False : Exit Select

                        'check if current member is downline of selected member
                        If basCommon.fncIsDownLineOf(frmMemList.MemberID, mstActive.intID) Or basCommon.fncIsDownLineOf(mstActive.intID, frmMemList.MemberID) Then Exit Select
                        blnSuccess = xAddNewChild(intFather, intMother, clsEnum.emRelation.NATURAL, clsEnum.emGender.FEMALE, True, frmMemList.MemberID)

                    Else
                        basCommon.fncMessageInfo(mcstrNoParent)
                    End If



                    '================== ▼ Start add child ▼ ====================

                    'add son

                Case My.Resources.StrAddSon
                    blnSuccess = xAddNewChild(intHusband, intWife, clsEnum.emRelation.NATURAL, clsEnum.emGender.MALE)
                    blnAddChild = True
                    intCurMo = mstSpouse.intID

                    'add daughter
                Case My.Resources.StrAddDaughter
                    blnSuccess = xAddNewChild(intHusband, intWife, clsEnum.emRelation.NATURAL, clsEnum.emGender.FEMALE)
                    blnAddChild = True
                    intCurMo = mstSpouse.intID

                Case My.Resources.StrAddAdoptChild
                    blnSuccess = xAddNewChild(intHusband, intWife, clsEnum.emRelation.ADOPT, clsEnum.emGender.FEMALE)
                    blnAddChild = True
                    intCurMo = mstSpouse.intID

                    'add kids from list
                Case My.Resources.StrAddKidFromList
                    'show form and get member id
                    If Not frmMemList.fncShowForm(frmPersonList.enFormMode.SELECT_MEMBER) Then blnRefresh = False : Exit Select
                    If Not frmMemList.MemberSelected Then blnRefresh = False : Exit Select

                    'check if current member is downline of selected member
                    If basCommon.fncIsDownLineOf(frmMemList.MemberID, mstActive.intID) Or basCommon.fncIsDownLineOf(mstActive.intID, frmMemList.MemberID) Then Exit Select
                    blnSuccess = xAddNewChild(mstActive.intID, mstSpouse.intID, clsEnum.emRelation.NATURAL, clsEnum.emGender.FEMALE, True, frmMemList.MemberID)
                    blnAddChild = True
                    intCurMo = mstSpouse.intID


                    '================== ▼ Start other process ▼ ====================

                    'add member to root
                Case My.Resources.StrAddRoot
                    'If intGender <> clsEnum.emGender.MALE Then Exit Select
                    If Not gobjDB.fncInsertRoot(intID) Then
                        basCommon.fncMessageError(gcstrFail)
                    Else
                        basCommon.fncSetGeneration(intID, My.Settings.intInitGeneration)
                        blnSuccess = True
                    End If

                    'add member to family head list
                Case My.Resources.StrAddFamilyHead
                    'If intGender <> clsEnum.emGender.MALE Then Exit Select
                    If Not gobjDB.fncInsertFHead(intID) Then basCommon.fncMessageError(gcstrFail)

                    'remove member from root list
                Case My.Resources.StrDelFromRoot
                    If Not gobjDB.fncDelRoot(intID) Then
                        blnSuccess = False
                        basCommon.fncMessageError(gcstrFail)
                    Else
                        intRootID = basCommon.fncGetRoot()
                        If intRootID > basConst.gcintNO_MEMBER Then
                            basCommon.fncSetGeneration(intRootID, My.Settings.intInitGeneration)
                        Else
                            'clear generation
                            gobjDB.fncSetMemberGeneration(-1)
                        End If
                        blnSuccess = True
                    End If

                    'remove member from family head list
                Case My.Resources.StrDelFromFamilyHead
                    If Not gobjDB.fncDelFhead(intID) Then
                        blnSuccess = False
                        basCommon.fncMessageError(gcstrFail)
                    Else
                        blnSuccess = True
                    End If


                    'delete member
                Case My.Resources.StrDelMember
                    If Not basCommon.fncMessageConfirm(String.Format(basConst.gcstrMessageConfirm, mstActive.strFullNameWAlias)) Then blnRefresh = False : Exit Select

                    If Not basCommon.fncDeleteMember(intID) Then
                        basCommon.fncMessageError(gcstrFail)
                        blnSuccess = False
                    Else
                        mintActiveID = mintPreviousID
                        mstActive.intID = mintActiveID

                        blnSuccess = True
                        'if there is no previous member
                        If Not basCommon.fncMemberExist(mstActive.intID) Then

                            'go to root
                            intRootID = basCommon.fncGetRoot
                            mstActive.intID = intRootID

                            'if root doesn't exist
                            If intRootID <= basConst.gcintNO_MEMBER Then
                                mintActiveID = basConst.gcintNO_MEMBER
                                mstActive.intID = mintActiveID
                                Exit Select
                            End If

                        End If

                        'intRootID = basCommon.fncGetRoot
                        'mstActive.intID = intRootID

                    End If

            End Select

            If blnRefresh Then
                If blnSuccess Then
                    'redraw
                    'if a new child added
                    If blnAddChild Then
                        xGetData()
                        xGetInfo()
                        xSpouseChange(intCurMo)
                        blnAddChild = False
                    Else
                        Me.ActiveMemberID = mstActive.intID
                    End If

                    'raise refresh event to update quick search grid in main form
                    RaiseEvent evnRefresh(mintActiveID, blnAddChild)

                End If
            End If
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMenuItemClick", ex)
        Finally
            If dlgConfirm IsNot Nothing Then dlgConfirm.Dispose()
            If frmMemList IsNot Nothing Then frmMemList.Dispose()
            If frmSpouse IsNot Nothing Then frmSpouse.Dispose()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xSpouseMenuClick, handle menu item clicked
    '      PARAMS1    : sender  Object, card object
    '      PARAMS2    : e       System.EventArgs
    '      MEMO       : 
    '      CREATE     : 2011/12/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSpouseChange(ByVal intSpouseID As Integer)

        Try
            'get spouse infor and draw children     
            For i As Integer = 0 To mlstSpouse.Count - 1

                If intSpouseID = mlstSpouse(i).intID Then mstSpouse = mlstSpouse(i)

            Next

            xGetChild(mstActive.intID, mstSpouse.intID)
            fncDraw()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSpouseMenuClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xSpouseListClick, handle spouse item clicked
    '      PARAMS1    : intSpouseID Integer
    '      MEMO       : 
    '      CREATE     : 2012/11/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSpouseListClick(ByVal intSpouseID As Integer)
        Try

            If intSpouseID = mstSpouse.intID Then Exit Sub

            xSpouseChange(intSpouseID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSpouseMenuClick", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xDelSpouseRel, handle delete spouse rel event
    '      PARAMS     :  
    '      MEMO       : 
    '      CREATE     : 2011/12/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDelSpouseRel()
        Try
            Const cstrFormat As String = "Quan hệ giữa {0} với vợ/chồng và các con sẽ bị hủy. Bạn có chắc chắn?"

            Dim blnSuccess As Boolean

            If mstSpouse.intID <= basConst.gcintNO_MEMBER Then Exit Sub
            If Not basCommon.fncMessageConfirm(String.Format(cstrFormat, mstSpouse.strFullNameWAlias)) Then Exit Sub

            blnSuccess = True
            blnSuccess = blnSuccess And gobjDB.BeginTransaction()

            'delete husband - wife relation ship
            blnSuccess = blnSuccess And gobjDB.fncDelRel(mstActive.intID, mstSpouse.intID, False)
            blnSuccess = blnSuccess And gobjDB.fncDelRel(mstSpouse.intID, mstActive.intID, False)

            'delete relationship with the kids
            For i As Integer = 0 To mlstChild.Count - 1

                blnSuccess = blnSuccess And gobjDB.fncDelRel(mlstChild(i).intID, mstSpouse.intID, False)

            Next

            If blnSuccess Then
                gobjDB.Commit()

                'redraw
                Me.ActiveMemberID = mstActive.intID

                'raise refresh event to update quick search grid in main form
                RaiseEvent evnRefresh(mintActiveID, True)

            Else
                gobjDB.RollBack()
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSpouseMenuClick", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xRefresh, handle kid-list changes the detail
    '      PARAMS1    : none
    '      MEMO       : 
    '      CREATE     : 2011/12/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xRefresh()

        Try
            'redraw kid
            xGetData()
            xRedrawCard(Nothing)

            'raise event to refresh quick search
            RaiseEvent evnRefresh(mintActiveID, False)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRefresh", ex)
        End Try

    End Sub


#End Region


#Region " IDisposable Support "

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)

        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free unmanaged resources when explicitly called
                mobjHusbandCard.Dispose()
                mobjWifeCard.Dispose()
                mobjFatherCard.Dispose()
                mobjMotherCard.Dispose()
                mobjBoardCard.Dispose()
                mobjSpouseList.Dispose()
                mobjAddChildCard.Dispose()
                mlstChildCard.Clear()
                mobjVnCal = Nothing
                mlstChild.Clear()
                mvwDetail.Dispose()
                mfrmPerInfo.Dispose()

            End If

            ' TODO: free shared unmanaged resources
        End If
        Me.disposedValue = True

    End Sub


    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region


End Class
