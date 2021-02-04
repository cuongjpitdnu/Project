'   ******************************************************************
'      TITLE      : DRAW FAMILY TREE
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/09/14　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************

Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : Draw family tree class
'      MEMO       : 
'      CREATE     : 2011/09/14  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class clsDrawTree1
    Implements IDisposable


#Region "Constants"

    Private Const mcstrClsName As String = "clsDrawTree1"                                   'class name

    Private Const mcstrFindUsrFormat As String = "MEMBER_ID = {0}"                          'format to find member
    Private Const mcstrFindRelFormat As String = "REL_FMEMBER_ID = {0} AND RELID = {1}"     'format to find member with relationship

    Private Const mcintStartLv As Integer = -1                                              'start level to draw
    Private Const mcintStartX As Integer = 20                                               'starting X cordinate
    Private Const mcintStartY As Integer = 20                                               'starting Y cordinate
    Private Const mcintStartMaxWidth As Integer = 0                                         'starting max width
    Private Const mcintStartMaxWLv As Integer = -1                                          'starting max width level
    Private Const mcintNONE_VALUE As Integer = 0                                            'default none value

#End Region


#Region "Variables"

    Private mintRootID As Integer                       'root id to draw
    Private mpnDraw As pnTreePanel                      'panel to draw
    Private mtblUser As DataTable                       'table to store members
    Private mtblRel As DataTable                        'table to store relationship
    Private mtblRelMarriage As DataTable                'table to store relationship of Marriage member
    Private mtblRelNaturalChild As DataTable            'table to store relationship of NaturalChild Member
    Private mtblDrawLv As DataTable                     'table to store drawing level in the form of lv/key(id)

    Private mtblControl As Hashtable                    'table to store drawing card
    Private mtblWife As Hashtable                       'table to store not drawing card
    Private mlstNormalLine As List(Of usrLine)          'table to store control (line)
    Private mlstSpecialLine As List(Of usrLine)         'table to store control (line)
    Private mlstNotDraw As List(Of Integer)             'table to store invisible control
    Private mlstSelectedCtrl As List(Of usrMemCardBase) 'list to store selected controls

    Private mobjCardLeft As usrMemberCard1              'left card
    Private mobjCardRight As usrMemberCard1             'right card
    Private mobjTempSelectedCard As usrMemberCard1      'temporary selected card

    Private mintMaX(20) As Integer                      'max X-cordinate of each level

    Private mintLv As Integer                           'level counter
    Private mintX As Integer                            'X counter
    Private mintY As Integer                            'Y counter
    Private mintWifeCount As Integer                    'wife counter
    Private mintMaxGeneration As Integer

    Private mintMEM_CARD_SPACE_LEFT As Integer          'wi
    Private mintMEM_CARD_SPACE_DOWN As Integer          'wi
    Private mintMEM_CARD_W As Integer
    Private mintMEM_CARD_H As Integer

    Private mintMaxWidth As Integer                     'max card's width of member and his wifes
    Private mintMaxWLv As Integer                       'max card's width levels

    Private mintMaxPanelWith As Integer                 'max with of panel
    Private mintMaxPanelHeight As Integer               'max height of panel

    Private mblnIsSmallCard As Boolean                  'draw small card

    Private mfrmPerInfo As frmPersonInfo                'personal information form
    Private mstCardInfo As stCardInfo                   'information


#End Region

    Public Event evnCardClicked(ByVal intMemID As Integer)
    Public Event evnCardDoubleClicked(ByVal intMemID As Integer)
    Public Event evnRefresh(ByVal intMemID As Integer, ByVal blnRedraw As Boolean)
    Public Event evnProgressDone()


#Region "Property"

    '   ****************************************************************** 
    '      FUNCTION   : DrawingCard Property, return list of drawing control 
    '      MEMO       :  
    '      CREATE     : 2011/12/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property DrawingCard() As Hashtable
        Get
            Return mtblControl
        End Get
    End Property


    '   ****************************************************************** 
    '      FUNCTION   : NotDrawingCard Property, return list of not drawing control 
    '      MEMO       :  
    '      CREATE     : 2011/12/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property NotDrawingCard() As Hashtable
        Get
            Return mtblWife
        End Get
    End Property


    '   ****************************************************************** 
    '      FUNCTION   : DrawList Property, return list of mem by level
    '      MEMO       :  
    '      CREATE     : 2011/12/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property DrawList() As DataView
        Get
            mtblDrawLv.DefaultView.Sort = "Level ASC"
            Return mtblDrawLv.DefaultView
        End Get
    End Property


    '   ****************************************************************** 
    '      FUNCTION   : NormalLine Property, 
    '      MEMO       :  
    '      CREATE     : 2011/12/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property NormalLine() As List(Of usrLine)
        Get
            Return mlstNormalLine
        End Get
    End Property


    '   ****************************************************************** 
    '      FUNCTION   : SpecialLine Property,
    '      MEMO       :  
    '      CREATE     : 2011/12/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property SpecialLine() As List(Of usrLine)
        Get
            Return mlstSpecialLine
        End Get
    End Property


    '   ****************************************************************** 
    '      FUNCTION   : NotDrawingCard Property, return list of not drawing control 
    '      MEMO       :  
    '      CREATE     : 2011/12/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property RootID() As Integer

        Get
            Return mintRootID
        End Get

    End Property


    '   ****************************************************************** 
    '      FUNCTION   : MaxWidth Property, max width of panel
    '      MEMO       :  
    '      CREATE     : 2011/12/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property MaxWidth() As Integer
        Get
            'Return mintMaxPanelWith + mintMEM_CARD_SPACE_LEFT
            Dim objCard As usrMemberCard1

            mintMaxPanelWith = mintMEM_CARD_SPACE_LEFT

            For Each element As DictionaryEntry In mtblControl

                objCard = CType(element.Value, usrMemberCard1)
                If objCard.Location.X > mintMaxPanelWith Then mintMaxPanelWith = objCard.Location.X

            Next

            Return mintMaxPanelWith + mintMEM_CARD_SPACE_LEFT

        End Get
    End Property


    '   ****************************************************************** 
    '      FUNCTION   : MaxHeight Property, max height of panel
    '      MEMO       :  
    '      CREATE     : 2011/12/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property MaxHeight() As Integer
        Get
            'Return mintMaxPanelHeight + mintMEM_CARD_SPACE_LEFT

            Dim objCard As usrMemberCard1

            mintMaxPanelHeight = mintMEM_CARD_SPACE_DOWN

            For Each element As DictionaryEntry In mtblControl

                objCard = CType(element.Value, usrMemberCard1)
                If objCard.Location.Y > mintMaxPanelHeight Then mintMaxPanelHeight = objCard.Location.Y

            Next

            Return mintMaxPanelHeight + mintMEM_CARD_SPACE_LEFT

        End Get
    End Property


    Public ReadOnly Property MARGIN_LEFT() As Integer
        Get
            Return mintMEM_CARD_SPACE_LEFT
        End Get
    End Property


    Public ReadOnly Property MARGIN_BOTTOM() As Integer
        Get
            Return mintMEM_CARD_SPACE_DOWN
        End Get
    End Property


    Public ReadOnly Property CARD_HEIGHT() As Integer
        Get
            Return mintMEM_CARD_H
        End Get
    End Property


    Public ReadOnly Property CARD_WIDTH() As Integer
        Get
            Return mintMEM_CARD_W
        End Get
    End Property

#End Region


    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/09/14  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New(ByVal pnDraw As pnTreePanel, ByVal frmPerInfo As frmPersonInfo)

        Me.mpnDraw = pnDraw
        Me.mfrmPerInfo = frmPerInfo
        Me.mblnIsSmallCard = True

        mtblUser = Nothing
        mtblRel = Nothing
        mtblRelMarriage = Nothing
        mtblRelNaturalChild = Nothing
        mtblControl = New Hashtable()
        mtblWife = New Hashtable()
        mlstNormalLine = New List(Of usrLine)
        mlstSpecialLine = New List(Of usrLine)
        mlstNotDraw = New List(Of Integer)

        mlstSelectedCtrl = New List(Of usrMemCardBase)

        mtblDrawLv = New DataTable()
        mtblDrawLv.Columns.Add("Level", System.Type.GetType("System.Int32"))
        mtblDrawLv.Columns.Add("ID", System.Type.GetType("System.Int32"))

        xResetValue()

    End Sub


    '   ****************************************************************** 
    '      FUNCTION   : xSetCardSize, init value 
    '      MEMO       :  
    '      CREATE     : 2012/01/11  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Sub xSetCardSize()

        Try
            If mblnIsSmallCard Then
                mintMEM_CARD_SPACE_LEFT = clsDefine.MEM_CARD_SPACE_LEFT_SMALL
                mintMEM_CARD_SPACE_DOWN = clsDefine.MEM_CARD_SPACE_DOWN_SMALL
                mintMEM_CARD_W = clsDefine.MEM_CARD_W_S
                mintMEM_CARD_H = clsDefine.MEM_CARD_H_S
            Else
                mintMEM_CARD_SPACE_LEFT = clsDefine.MEM_CARD_SPACE_LEFT_LARGE
                mintMEM_CARD_SPACE_DOWN = clsDefine.MEM_CARD_SPACE_DOWN_LARGE
                mintMEM_CARD_W = clsDefine.MEM_CARD_W_L
                mintMEM_CARD_H = clsDefine.MEM_CARD_H_L
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetCardSize", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncDraw, draw family tree
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncDraw(ByVal intRootID As Integer, ByVal intMaxGeneration As Integer) As Boolean

        fncDraw = False
        mpnDraw.Visible = False
        Try
            gintPercent = 0

            'Dim dtStart As Date = DateTime.Now

            mintMaxGeneration = intMaxGeneration

            'add handler of panel
            AddHandler mpnDraw.evnMultiSelection, AddressOf xMultiSelect

            xResetValue()
            If mtblDrawLv IsNot Nothing Then mtblDrawLv.Rows.Clear() 'reset 

            mblnIsSmallCard = True
            If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then mblnIsSmallCard = False

            xSetCardSize()

            Me.mintRootID = intRootID
            'mintMaxPanelWith = mintMEM_CARD_SPACE_LEFT
            'mintMaxPanelHeight = mintMEM_CARD_SPACE_DOWN

            Application.DoEvents()
            'read data
            xReadData()

            Application.DoEvents()

            gintPercent = 10
            'clear controls
            fncClearControls()
            gintPercent = 20
            'draw father card
            xDrawFather(intRootID, False)
            gintPercent = 30

            'do recusive to add member to table
            xRecusiveDraw(intRootID, False)
            gintPercent = 40
            'align father card
            xAlignFather(intRootID)
            gintPercent = 50
            'center align control
            xAlignControls(intRootID)
            gintPercent = 60
            xResetValue()
            gintPercent = 70

            'add mother
            xAddMother(intRootID, False)
            gintPercent = 80
            'add wife to table

            'Start Manh 2012/11/14 Change Addwife function
            'xAddWife(intRootID, False)
            xAddWifeNew(intRootID, False, mintMaxGeneration)
            'End Manh 2012/11/14

            gintPercent = 85
            'add control from table to panel
            xAddCtrl2Panel()
            gintPercent = 90
            'set viewing region
            gintPercent = 95
            'lines
            xDrawConnector()
            gintPercent = 100
            
            'MsgBox(-dtStart.Minute * 60 - dtStart.Second + DateTime.Now.Minute * 60 + DateTime.Now.Second)
            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncDraw", ex)

        Finally
            gintPercent = 100
            'raise event for closing waiting form
            RaiseEvent evnProgressDone()
            mpnDraw.Visible = True
            xSetScrollView(intRootID)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncDrawConnector, draw connector
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : Optional bmpReturn Bitmap, return value
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawConnector() As Boolean

        xDrawConnector = False

        Dim objCard1 As usrMemberCard1 = Nothing
        Dim objCard2 As usrMemberCard1 = Nothing

        Try
            Dim intID1 As Integer
            Dim intID2 As Integer
            Dim intRel As Integer

            'catch when all member has no relationship
            If mtblRel Is Nothing Then Exit Function

            'loop for all member to draw connector
            For i As Integer = 0 To mtblRel.Rows.Count - 1

                'reset value
                intID1 = 0
                intID2 = 0
                intRel = 0

                'get id from database
                Integer.TryParse(basCommon.fncCnvNullToString(mtblRel.Rows(i).Item("MEMBER_ID")), intID1)
                Integer.TryParse(basCommon.fncCnvNullToString(mtblRel.Rows(i).Item("REL_FMEMBER_ID")), intID2)
                Integer.TryParse(basCommon.fncCnvNullToString(mtblRel.Rows(i).Item("RELID")), intRel)

                'exit if member doesn't exist in hastable
                If Not mtblControl.ContainsKey(intID1) Then Continue For
                If Not mtblControl.ContainsKey(intID2) Then Continue For

                'exit if this member should not be drawn
                If intRel = CInt(clsEnum.emRelation.NATURAL) And mtblWife.ContainsKey(intID2) Then Continue For

                objCard1 = CType(mtblControl.Item(intID1), usrMemberCard1)
                objCard2 = CType(mtblControl.Item(intID2), usrMemberCard1)

                xDrawConnector(objCard1, objCard2)
                Application.DoEvents()

            Next

            Return True

        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncRedrawCard, redraw a card
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncRedrawCard(ByVal intMemID As Integer) As Boolean

        fncRedrawCard = False

        Dim objCard As usrMemberCard1

        Try
            'read data
            xReadData()

            'read detail of this member
            xReadDetail(intMemID)

            'exit if control doesn't exist
            If Not mtblControl.ContainsKey(intMemID) Then Exit Function

            'get card
            objCard = CType(mtblControl.Item(intMemID), usrMemberCard1)

            xFillCardBase(objCard, basConst.gcintNONE_VALUE, basConst.gcintNONE_VALUE)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncRedrawCard", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncSetFocus, scroll to a card
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncSetFocus(ByVal intMemID As Integer) As Boolean

        fncSetFocus = False

        Try
            Return xSetScrollView(intMemID, True)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncSetFocus", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xReadData, read data from database
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xReadData() As Boolean

        xReadData = False

        Try
            If mtblUser IsNot Nothing Then mtblUser.Clear()
            If mtblRel IsNot Nothing Then mtblRel.Clear()

            'get data table
            mtblUser = gobjDB.fncGetMemberMain()
            mtblRel = gobjDB.fncGetRel()
            mtblRelMarriage = gobjDB.fncGetRel(-1, -1, clsEnum.emRelation.MARRIAGE)
            mtblRelNaturalChild = gobjDB.fncGetRel(-1, -1, clsEnum.emRelation.NATURAL)

            If mtblUser Is Nothing Then Exit Function
            If mtblRel Is Nothing Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xReadData", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawFather, draw father card
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawFather(ByVal intFather As Integer, ByVal blnRedraw As Boolean) As Boolean

        xDrawFather = False

        Try
            'add father card
            If Not blnRedraw Then
                xAddControl(mintX, mintY, intFather, basConst.gcintNONE_VALUE)
            Else
                xResetLocation(intFather, mintX, mintY)
            End If

            mintY = mcintStartY + mintMEM_CARD_SPACE_DOWN

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawFather", ex)
        End Try

    End Function


    Private Function xGetKidList(ByVal intFather As Integer) As List(Of Integer)

        'Return basCommon.fncGetKidList(intFather, clsEnum.emRelation.NATURAL)
        Dim lstKidID As List(Of Integer) = New List(Of Integer)

        Try
            Dim vwRel As DataView
            Dim intChildID As Integer

            vwRel = New DataView(mtblRelNaturalChild)
            vwRel.RowFilter = String.Format(mcstrFindRelFormat, intFather, CInt(clsEnum.emRelation.NATURAL))

            'loop for each husband/wife then add to hastable
            For i As Integer = 0 To vwRel.Count - 1

                Integer.TryParse(fncCnvNullToString(vwRel(i).Item("MEMBER_ID")), intChildID)
                lstKidID.Add(intChildID)

            Next

        Catch ex As Exception
            Throw ex
        End Try

        Return lstKidID
    End Function

    Private Function xGetKidList2(ByVal intFather As Integer) As DataView

        Try

            Dim vwRel As New DataView(mtblRelNaturalChild)
            vwRel.RowFilter = String.Format(mcstrFindRelFormat, intFather, CInt(clsEnum.emRelation.NATURAL))
            Return vwRel

        Catch ex As Exception
            Throw ex
        End Try

        Return Nothing
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xRecusiveDraw, calculate space to draw and add
    '                   member into list
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intFather Integer, member id to start
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xRecusiveDraw(ByVal intFather As Integer, ByVal blnRedraw As Boolean) As Boolean

        xRecusiveDraw = False

        'Dim vwUser As DataView = Nothing
        'Dim lstChild As List(Of Integer)
        Dim dvChild As DataView

        Try

            Dim intKid As Integer

            mintLv += 1

            If mintLv > 0 Then mintY += mintMEM_CARD_SPACE_DOWN
            'If mintLv > 0 Then mintY = mtblControl.Count * 111 + 20 'mintY += mintMEM_CARD_SPACE_DOWN

            'limit level
            If mintLv > mintMaxGeneration - 2 Then 'My.Settings.intGeneration - 2 Then
                mintLv -= 1
                mintX += mintMEM_CARD_SPACE_LEFT + mintWifeCount * mintMEM_CARD_SPACE_LEFT
                Return True
            End If

            'vwUser = New DataView(mtblRel)
            'Start Manh 2012/11/14 Get Kid List by using data which aready got
            'lstChild = basCommon.fncGetKidList(intFather, clsEnum.emRelation.NATURAL)
            'lstChild = xGetKidList(intFather)
            dvChild = xGetKidList2(intFather)
            'End Manh 2012/11/14

            'find how many kids?
            'vwUser.RowFilter = String.Format(mcstrFindRelFormat, intFather, CInt(clsEnum.emRelation.NATURAL))

            'If lstChild.Count > 0 Then
            If Not dvChild Is Nothing Then

                'draw each kid
                'For i As Integer = 0 To lstChild.Count - 1
                For i As Integer = 0 To dvChild.Count - 1


                    'get member id 
                    'Integer.TryParse(basCommon.fncCnvNullToString(vwUser(i)(0)), intKid)
                    'intKid = lstChild(i)
                    Integer.TryParse(fncCnvNullToString(dvChild(i).Item("MEMBER_ID")), intKid)

                    'if X cordinate is smaller than or equal to the left card
                    If mintX <= mintMaX(mintLv) Then mintX = mintMaX(mintLv) + mintMEM_CARD_SPACE_LEFT

                    ' ▽ 2012/11/22   AKB Quyet （Fix bug ）*********************************
                    If mintX <= mintMaX(mintLv + 1) Then mintX = mintMaX(mintLv + 1) + mintMEM_CARD_SPACE_LEFT
                    ' △ 2012/11/22   AKB Quyet *********************************************


                    'If mintX < mintMaX(mintLv) Then mintX = mintMaX(mintLv) + mintMEM_CARD_SPACE_LEFT

                    'reset max X
                    mintMaX(mintLv) = mintX

                    'xAddControl(mintX, mintY, intKid)

                    If Not blnRedraw Then
                        'draw current member
                        xAddControl(mintX, mintY, intKid, intFather)
                    Else
                        If Not xResetLocation(intKid, mintX, mintY) Then
                            'mintLv -= 1
                            'mintX -= mintMEM_CARD_SPACE_LEFT
                            'Exit Function
                            Continue For
                        End If
                    End If

                    'how many wife?
                    mintWifeCount = xWifeCount(intKid)

                    'reserve space for his wife
                    If mintWifeCount > 0 Then

                        For j As Integer = 0 To mintWifeCount - 1

                            mintX += mintMEM_CARD_SPACE_LEFT

                            'store max X
                            If mintX <= mintMaX(mintLv) Then mintX = mintMaX(mintLv) + mintMEM_CARD_SPACE_LEFT
                            mintMaX(mintLv) = mintX

                        Next

                        mintX -= mintMEM_CARD_SPACE_LEFT * mintWifeCount

                    End If

                    Application.DoEvents()

                    If Not xRecusiveDraw(intKid, blnRedraw) Then Exit Function

                    Application.DoEvents()

                    mintY -= mintMEM_CARD_SPACE_DOWN

                Next

            Else
                mintX += mintMEM_CARD_SPACE_LEFT + mintWifeCount * mintMEM_CARD_SPACE_LEFT

            End If

            mintLv -= 1

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRecusiveDraw", ex, Nothing, False)
        Finally
            'If vwUser IsNot Nothing Then vwUser.Dispose()
            'lstChild = Nothing
            dvChild = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAlignFather, align father card
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAlignFather(ByVal intFather As Integer) As Boolean

        xAlignFather = False

        Dim objCard As usrMemberCard1 = Nothing

        Try
            Dim ptMove As Point

            Dim intWidth As Integer
            Dim intMove As Integer
            Dim intMaxAt As Integer = 0

            Dim ptMinPoint As New Point

            objCard = CType(mtblControl.Item(intFather), usrMemberCard1)
            If objCard Is Nothing Then Exit Function

            intWidth = xWidthOfMem(intFather)

            'get max with of child
            mintMaxWidth = 0
            xGetMaxWidth(intFather, intMaxAt, ptMinPoint)

            If mintMaxWidth < intWidth Then Exit Function

            'calculate length to move
            ptMove = objCard.Location
            ptMove.X = ptMinPoint.X

            If mintMaxWidth = ptMove.X + mintMEM_CARD_W Then Exit Function

            intMove = (mintMaxWidth - ptMove.X - intWidth) \ 2

            ptMove.X += intMove
            objCard.Location = ptMove

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAlignFather", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAlignControls, center align control
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intFather Integer, member to start
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAlignControls(ByVal intFather As Integer) As Boolean

        xAlignControls = False

        Dim objCard As usrMemberCard1 = Nothing
        'Dim vwUser As DataView = Nothing
        'Dim lstChild As List(Of Integer)
        Dim dvChild As DataView

        Try
            Dim ptMove As Point

            Dim intWidth As Integer
            Dim intMove As Integer
            Dim intMaxAt As Integer = 0

            Dim intKid As Integer
            Dim ptMinPoint As New Point

            'count level by 1 each call
            mintLv += 1

            'vwUser = New DataView(mtblRel)
            'lstChild = basCommon.fncGetKidList(intFather, clsEnum.emRelation.NATURAL)

            'Start Manh 2012/11/14
            'lstChild = xGetKidList(intFather)
            dvChild = xGetKidList2(intFather)
            'End Manh 2012/11/14

            'find how many kids?
            'vwUser.RowFilter = String.Format(mcstrFindRelFormat, intFather, CInt(clsEnum.emRelation.NATURAL))

            'If lstChild.Count <= 0 Then Exit Function
            If dvChild Is Nothing Then Exit Function

            'For i As Integer = 0 To lstChild.Count - 1
            For i As Integer = 0 To dvChild.Count - 1

                Application.DoEvents()

                'get child's id
                'Integer.TryParse(basCommon.fncCnvNullToString(vwUser(i)(0)), intKid)
                'intKid = lstChild(i)
                Integer.TryParse(fncCnvNullToString(dvChild(i).Item("MEMBER_ID")), intKid)

                '2012 03 27 - test
                If mlstNotDraw.Contains(intKid) Then Continue For

                objCard = CType(mtblControl.Item(intKid), usrMemberCard1)
                If objCard Is Nothing Then Continue For

                intWidth = xWidthOfMem(intKid)

                'get max with of child
                mintMaxWidth = 0
                xGetMaxWidth(intKid, intMaxAt, ptMinPoint)

                If Not xHasChild(intKid) Then Continue For

                If mintMaxWidth < intWidth Then Continue For

                'calculate length to move
                ptMove = objCard.Location
                ptMove.X = ptMinPoint.X


                If mintMaxWidth = ptMove.X + mintMEM_CARD_W Then Continue For

                intMove = (mintMaxWidth - ptMove.X - intWidth) \ 2

                ptMove.X += intMove
                objCard.Location = ptMove

                Application.DoEvents()
                xAlignControls(intKid)

            Next

            mintLv -= 1

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAlignControls", ex)
        Finally
            'If vwUser IsNot Nothing Then vwUser.Dispose()
            'lstChild = Nothing
            dvChild = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddMother, draw mother card
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/11/22  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddMother(ByVal intFather As Integer, ByVal blnRedraw As Boolean) As Boolean

        xAddMother = False


        Dim vwRel As DataView = Nothing
        Dim objCard As usrMemberCard1 = Nothing

        Try
            Dim intWife As Integer
            Dim ptLocate As Point

            'count the wife
            'vwRel = New DataView(mtblRel)
            vwRel = New DataView(mtblRelMarriage)
            vwRel.RowFilter = String.Format(mcstrFindRelFormat, intFather, CInt(clsEnum.emRelation.MARRIAGE))

            mintWifeCount = vwRel.Count

            'and his wife
            If mintWifeCount <= 0 Then Return True

            objCard = CType(mtblControl.Item(intFather), usrMemberCard1)
            If objCard Is Nothing Then Exit Function
            ptLocate = objCard.Location

            For j As Integer = 0 To vwRel.Count - 1

                Integer.TryParse(basCommon.fncCnvNullToString(vwRel(j)("MEMBER_ID")), intWife)

                ptLocate.X += mintMEM_CARD_SPACE_LEFT

                'xAddControl(ptLocate.X, ptLocate.Y, intWife, True)

                If Not blnRedraw Then
                    'draw current member
                    xAddControl(ptLocate.X, ptLocate.Y, intWife, basConst.gcintNONE_VALUE, True, intFather, objCard.DrawLv)
                Else
                    If Not xResetLocation(intWife, ptLocate.X, ptLocate.Y) Then
                        'mintLv -= 1
                        'mintX -= mintMEM_CARD_SPACE_LEFT
                        'Exit Function
                        Continue For
                    End If
                End If

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddMother", ex)
        Finally
            If vwRel IsNot Nothing Then vwRel.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddWife, add wife to list
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intFather Integer, husband id
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddWife(ByVal intFather As Integer, ByVal blnRedraw As Boolean) As Boolean

        xAddWife = False

        'Dim vwUser As DataView = Nothing
        Dim vwRel As DataView = Nothing
        Dim objCard As usrMemberCard1 = Nothing
        Dim lstChild As List(Of Integer)

        Try
            Dim intKid As Integer
            Dim intWife As Integer
            Dim ptLocate As Point

            'vwUser = New DataView(mtblRel)

            'lstChild = basCommon.fncGetKidList(intFather, clsEnum.emRelation.NATURAL)

            'Start Manh 2012/11/14
            lstChild = xGetKidList(intFather)
            'End Manh 2012/11/14

            'find how many kids?
            'vwUser.RowFilter = String.Format(mcstrFindRelFormat, intFather, CInt(clsEnum.emRelation.NATURAL))

            If lstChild.Count <= 0 Then Return True

            mintLv += 1
            For i As Integer = 0 To lstChild.Count - 1

                'Integer.TryParse(basCommon.fncCnvNullToString(vwUser(i)(0)), intKid)
                intKid = lstChild(i)

                'count the wife
                vwRel = New DataView(mtblRel)
                vwRel.RowFilter = String.Format(mcstrFindRelFormat, intKid, CInt(clsEnum.emRelation.MARRIAGE))

                mintWifeCount = vwRel.Count

                'and his wife
                If mintWifeCount > 0 Then

                    objCard = CType(mtblControl.Item(intKid), usrMemberCard1)

                    If objCard IsNot Nothing Then

                        ptLocate = objCard.Location

                        For j As Integer = 0 To vwRel.Count - 1

                            Integer.TryParse(basCommon.fncCnvNullToString(vwRel(j)("MEMBER_ID")), intWife)

                            ptLocate.X += mintMEM_CARD_SPACE_LEFT

                            'xAddControl(ptLocate.X, ptLocate.Y, intWife, True)

                            If Not blnRedraw Then
                                'draw current member
                                xAddControl(ptLocate.X, ptLocate.Y, intWife, basConst.gcintNONE_VALUE, True, intKid, objCard.DrawLv)
                            Else
                                If Not xResetLocation(intWife, ptLocate.X, ptLocate.Y) Then
                                    'mintLv -= 1
                                    'mintX -= mintMEM_CARD_SPACE_LEFT
                                    'Exit Function
                                    Continue For
                                End If
                            End If

                        Next

                    End If

                End If

                xAddWife(intKid, blnRedraw)

            Next

            mintLv -= 1

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddWife", ex)
        Finally
            'If vwUser IsNot Nothing Then vwUser.Dispose()
            If vwRel IsNot Nothing Then vwRel.Dispose()
            lstChild = Nothing
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xAddWife_Manh, add wife to list
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intFather Integer, husband id
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddWifeNew(ByVal intFather As Integer, _
                                   ByVal blnRedraw As Boolean, _
                                   ByVal intMaxGeneration As Integer) As Boolean

        xAddWifeNew = False

        'Dim vwUser As DataView = Nothing
        Dim vwRel As DataView = Nothing
        Dim objCard As usrMemberCard1 = Nothing
        'Dim lstChild As List(Of Integer)
        Dim dvChild As DataView

        If intMaxGeneration < 0 Then Return True

        Try
            Dim intKid As Integer
            Dim intWife As Integer
            Dim ptLocate As Point

            'vwUser = New DataView(mtblRel)
            'lstChild = basCommon.fncGetKidList(intFather, clsEnum.emRelation.NATURAL)

            'Start Manh 2012/11/14
            'lstChild = xGetKidList(intFather)
            dvChild = xGetKidList2(intFather)
            'End Manh 2012/11/14

            'find how many kids?
            'vwUser.RowFilter = String.Format(mcstrFindRelFormat, intFather, CInt(clsEnum.emRelation.NATURAL))

            'If lstChild.Count <= 0 Then Return True
            If dvChild Is Nothing Then Return True
            mintLv += 1
            'For i As Integer = 0 To lstChild.Count - 1
            For i As Integer = 0 To dvChild.Count - 1


                'Integer.TryParse(basCommon.fncCnvNullToString(vwUser(i)(0)), intKid)
                'intKid = lstChild(i)
                Integer.TryParse(fncCnvNullToString(dvChild(i).Item("MEMBER_ID")), intKid)

                'count the wife
                'vwRel = New DataView(mtblRel)
                vwRel = New DataView(mtblRelMarriage)
                vwRel.RowFilter = String.Format(mcstrFindRelFormat, intKid, CInt(clsEnum.emRelation.MARRIAGE))

                mintWifeCount = vwRel.Count

                'and his wife
                If mintWifeCount > 0 Then

                    objCard = CType(mtblControl.Item(intKid), usrMemberCard1)

                    If objCard IsNot Nothing Then

                        ptLocate = objCard.Location

                        For j As Integer = 0 To vwRel.Count - 1

                            Application.DoEvents()

                            Integer.TryParse(basCommon.fncCnvNullToString(vwRel(j)("MEMBER_ID")), intWife)

                            ptLocate.X += mintMEM_CARD_SPACE_LEFT

                            'xAddControl(ptLocate.X, ptLocate.Y, intWife, True)

                            If Not blnRedraw Then
                                'draw current member
                                xAddControl(ptLocate.X, ptLocate.Y, intWife, basConst.gcintNONE_VALUE, True, intKid, objCard.DrawLv)
                            Else
                                If Not xResetLocation(intWife, ptLocate.X, ptLocate.Y) Then
                                    'mintLv -= 1
                                    'mintX -= mintMEM_CARD_SPACE_LEFT
                                    'Exit Function
                                    Continue For
                                End If
                            End If

                            Application.DoEvents()

                        Next

                    End If

                End If

                xAddWifeNew(intKid, blnRedraw, intMaxGeneration - 1)

            Next

            mintLv -= 1

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddWife_Manh", ex)
        Finally
            'If vwUser IsNot Nothing Then vwUser.Dispose()
            If vwRel IsNot Nothing Then vwRel.Dispose()
            'lstChild = Nothing
            dvChild = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddCtrl2Panel, add controls to panel
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddCtrl2Panel() As Boolean

        xAddCtrl2Panel = False

        Try
            Dim objCard As usrMemberCard1

            For Each element As DictionaryEntry In mtblControl

                objCard = CType(element.Value, usrMemberCard1)
                mpnDraw.Controls.Add(objCard)
                'MessageBox.Show("sdgasd")
                Application.DoEvents()
            Next

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xAddCtrl2Panel", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddControl, create a card and add to list
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : intX Integer, X cordinate
    '      PARAMS2    : intY Integer, Y cordinate
    '      PARAMS3    : intID Integer, user id
    '      PARAMS4    : blnNotDraw Boolean, draw connector or not?
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddControl(ByVal intX As Integer, _
                                 ByVal intY As Integer, _
                                 ByVal intID As Integer, _
                                 ByVal intParentID As Integer, _
                                 Optional ByVal blnIsWife As Boolean = False, _
                                 Optional ByVal intSpouseID As Integer = -1, _
                                 Optional ByVal intDrawLv As Integer = -1) As Boolean

        xAddControl = False

        Dim objCard As usrMemberCard1 = Nothing
        Dim intLv(1) As Object

        Try
            'clear
            mstCardInfo = Nothing

            'create card and detail of user
            objCard = New usrMemberCard1(intID, mblnIsSmallCard)

            'read info
            xReadDetail(intID)

            'fill card
            xFillCardBase(objCard, intX, intY)
            objCard.ParentID = intParentID
            objCard.SpouseID = intSpouseID
            objCard.DrawLv = mintLv
            intLv(0) = mintLv
            intLv(1) = intID

            'add card to panel and to the list
            If Not mtblControl.ContainsKey(intID) Then mtblControl.Add(intID, objCard)
            If blnIsWife Then
                'If Not mtblWife.ContainsKey(intID) Then mtblWife.Add(intID, objCard)
                If Not mtblWife.ContainsKey(intID) Then mtblWife.Add(intID, intID)
                objCard.DrawLv = intDrawLv
                intLv(0) = intDrawLv
            End If

            mtblDrawLv.Rows.Add(intLv)

            'add handler
            AddHandler objCard.evnCardDoubleClick, AddressOf xShowPerInfo
            AddHandler objCard.evnCardClick, AddressOf xCardClicked
            AddHandler objCard.evnNotDraw, AddressOf xHandleNotDraw
            AddHandler objCard.evnCardLocationChange, AddressOf xCardMove

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xAddControl", ex)

        Finally

            Erase intLv
            
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xReadDetail, read infor and set to struc
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intId Integer, card id
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xReadDetail(ByVal intId As Integer) As Boolean

        xReadDetail = False

        Dim vwUser As DataView = Nothing

        Try
            vwUser = New DataView(mtblUser)

            vwUser.RowFilter = String.Format(mcstrFindUsrFormat, intId)

            If vwUser.Count > 0 Then

                With mstCardInfo

                    'get data
                    .stBasicInfo.strFirstName = basCommon.fncCnvNullToString(vwUser(0)("FIRST_NAME"))
                    .stBasicInfo.strMidName = basCommon.fncCnvNullToString(vwUser(0)("MIDDLE_NAME"))
                    .stBasicInfo.strLastName = basCommon.fncCnvNullToString(vwUser(0)("LAST_NAME"))
                    .stBasicInfo.strAlias = basCommon.fncCnvNullToString(vwUser(0)("ALIAS_NAME"))
                    .stBasicInfo.strImgLocation = fncCnvNullToString(vwUser(0)("AVATAR_PATH"))

                    'Date.TryParse(fncCnvNullToString(vwUser(0)("BIRTH_DAY")), .dtBirth)
                    Integer.TryParse(fncCnvNullToString(vwUser(0)("BIR_DAY")), .stBasicInfo.stBirthDaySun.intDay)
                    Integer.TryParse(fncCnvNullToString(vwUser(0)("BIR_MON")), .stBasicInfo.stBirthDaySun.intMonth)
                    Integer.TryParse(fncCnvNullToString(vwUser(0)("BIR_YEA")), .stBasicInfo.stBirthDaySun.intYear)

                    'Date.TryParse(fncCnvNullToString(vwUser(0)("DECEASED_DATE")), .dtDeath)
                    Integer.TryParse(fncCnvNullToString(vwUser(0)("DEA_DAY")), .stBasicInfo.stDeadDayMoon.intDay)
                    Integer.TryParse(fncCnvNullToString(vwUser(0)("DEA_MON")), .stBasicInfo.stDeadDayMoon.intMonth)
                    Integer.TryParse(fncCnvNullToString(vwUser(0)("DEA_YEA")), .stBasicInfo.stDeadDayMoon.intYear)

                    Integer.TryParse(fncCnvNullToString(vwUser(0)("DEA_DAY_SUN")), .stBasicInfo.stDeadDaySun.intDay)
                    Integer.TryParse(fncCnvNullToString(vwUser(0)("DEA_MON_SUN")), .stBasicInfo.stDeadDaySun.intMonth)
                    Integer.TryParse(fncCnvNullToString(vwUser(0)("DEA_YEA_SUN")), .stBasicInfo.stDeadDaySun.intYear)

                    Integer.TryParse(fncCnvNullToString(vwUser(0)("GENDER")), .stBasicInfo.intGender)
                    Integer.TryParse(fncCnvNullToString(vwUser(0)("DECEASED")), .stBasicInfo.intDecease)

                End With

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xReadDetail", ex)
        Finally
            If vwUser IsNot Nothing Then vwUser.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillCardBase, base function for filling card
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : objCard usrMemberCard, user card
    '      PARAMS     : intX    Integer, X location
    '      PARAMS     : intY    Integer, Y location
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillCardBase(ByVal objCard As usrMemberCard1, _
                                   Optional ByVal intX As Integer = basConst.gcintNONE_VALUE, _
                                   Optional ByVal intY As Integer = basConst.gcintNONE_VALUE) As Boolean
        xFillCardBase = False

        Try
            Dim strBirth As String = ""
            Dim strDeath As String = ""

            With mstCardInfo
                'fullname
                .stBasicInfo.strFullName = String.Format(basConst.gcstrNameFormat, .stBasicInfo.strLastName, .stBasicInfo.strMidName, .stBasicInfo.strFirstName)
                .stBasicInfo.strFullName = basCommon.fncRemove2Space(.stBasicInfo.strFullName)
                If Not basCommon.fncIsBlank(.stBasicInfo.strAlias) Then .stBasicInfo.strFullName = String.Format("{0}{1}({2})", .stBasicInfo.strFullName, vbCrLf, .stBasicInfo.strAlias)

                'other values for card
                objCard.CardName = .stBasicInfo.strFullName
                'objCard.CardBirthDie = basCommon.fncGetBirthDieText(.dtBirth, .dtDeath, .intDecease)
                'objCard.CardBirth = basCommon.fncGetDateStatus(.intByea, .intBmon, .intBday, basConst.gcintALIVE) 'basCommon.fncGetBirthDieText(.intByea, .intDyea, .intDecease)
                strBirth = basCommon.fncGetDateStatus(.stBasicInfo.stBirthDaySun, basConst.gcintALIVE) 'basCommon.fncGetBirthDieText(.intByea, .intDyea, .intDecease)
                objCard.AliveStatus = .stBasicInfo.intDecease <> basConst.gcintDIED

                '2016/12/27 Start Manh Add to show Birth of Dead in Sun Calendar
                strDeath += fncGetDeadDateStringDisplaybyOption(.stBasicInfo, My.Settings.intDeadDateShowType)
                '2016/12/27 End

                objCard.CardGender = .stBasicInfo.intGender
                objCard.CardName = String.Format("{0}" & vbCrLf & "{1}" & vbCrLf & "{2}", .stBasicInfo.strFullName, strBirth, strDeath)

                'set image if available and is large card
                If Not mblnIsSmallCard Then
                    '.strImgLocation = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & .strImgLocation
                    'objCard.CardImage = basCommon.fncCreateThumbnail(.strImgLocation, clsDefine.THUMBNAIL_W, clsDefine.THUMBNAIL_H, .intGender)
                    .stBasicInfo.strImgLocation = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath & .stBasicInfo.strImgLocation
                    objCard.CardImageLocation = .stBasicInfo.strImgLocation
                End If

                If intX > basConst.gcintNONE_VALUE And intY > basConst.gcintNONE_VALUE Then objCard.Location = New Point(intX, intY)

                'set max with and height of panel for exporting to excel and pdf
                'If intX > mintMaxPanelWith Then mintMaxPanelWith = intX
                'If intY > mintMaxPanelHeight Then mintMaxPanelHeight = intY

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillCardBase", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector, draw family tree
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawConnector(ByVal objCard1 As usrMemberCard1, ByVal objCard2 As usrMemberCard1) As Boolean

        xDrawConnector = False

        Try

            mobjCardLeft = objCard1
            mobjCardRight = objCard2

            If objCard1.Location.Y = objCard2.Location.Y Then
                'in case 2 cards have same Y - spouse relationship

                If objCard1.Location.X > objCard2.Location.X Then

                    mobjCardLeft = objCard2
                    mobjCardRight = objCard1

                End If

            Else
                '2 cards have different Y - parent-son relationship
                'the higher will be the cardleft

                If objCard1.Location.Y > objCard2.Location.Y Then

                    mobjCardLeft = objCard2
                    mobjCardRight = objCard1

                End If

            End If

            If mobjCardLeft.Location.Y = mobjCardRight.Location.Y Then
                'draw same level
                'xDrawSameLv(g)
                xDrawSameLv(mobjCardLeft, mobjCardRight)

            Else
                'draw different level
                'xDrawDiffLv(g)             
                xDrawDiffLv(mobjCardLeft, mobjCardRight)
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawConnector", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawSameLv, draw same level
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawSameLv(ByVal objLeftCard As usrMemberCard1, ByVal objRightCard As usrMemberCard1)
        Try
            Dim intLen As Integer
            Dim objHorzLine1 As usrLine
            Dim objHorzLine2 As usrLine

            intLen = Math.Abs(objLeftCard.CardMidBottom.Y - objRightCard.CardMidTop.Y) \ 2
            objHorzLine1 = New usrLine(clsEnum.emLineType.SINGLE_LINE, clsEnum.emLineDirection.VERTICAL, intLen)
            objHorzLine2 = New usrLine(clsEnum.emLineType.SINGLE_LINE, clsEnum.emLineDirection.VERTICAL, intLen)

            objHorzLine1.fncAddSpouseLine(objLeftCard, objRightCard, 0)
            objHorzLine2.fncAddSpouseLine(objLeftCard, objRightCard, -4)

            mpnDraw.Controls.Add(objHorzLine1)
            mpnDraw.Controls.Add(objHorzLine2)

            'add to list
            mlstNormalLine.Add(objHorzLine1)
            mlstNormalLine.Add(objHorzLine2)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawConnector", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xDrawDiffLv, draw different level
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawDiffLv(ByVal objUpperCard As usrMemberCard1, ByVal objLowerCard As usrMemberCard1)

        Try

            Dim objVerLine1 As usrLine
            Dim objVerLine2 As usrLine
            Dim objHorzLine As usrLine

            Dim intLen As Integer
            Dim intWei As Integer
            Dim blnIsFHead As Boolean = False

            'find length and thick of line
            intLen = Math.Abs(objUpperCard.CardMidBottom.Y - objLowerCard.CardMidTop.Y) \ 2
            intWei = 1
            'If basCommon.fncIsFhead(objUpperCard.CardID) And basCommon.fncIsFhead(objLowerCard.CardID) Then intWei = 3

            'create line
            objVerLine1 = New usrLine(clsEnum.emLineType.SINGLE_LINE, clsEnum.emLineDirection.VERTICAL, intLen)
            objVerLine2 = New usrLine(clsEnum.emLineType.SINGLE_LINE, clsEnum.emLineDirection.VERTICAL, intLen)
            objHorzLine = New usrLine(clsEnum.emLineType.SINGLE_LINE, clsEnum.emLineDirection.VERTICAL, intLen)


            'set color and line weight
            If basCommon.fncIsFhead(objUpperCard.CardID) And basCommon.fncIsFhead(objLowerCard.CardID) Then

                intWei = 2
                objVerLine1.LineWeight = intWei
                objVerLine2.LineWeight = intWei
                objHorzLine.LineWeight = intWei

                objVerLine1.LineColor = Color.Red
                objVerLine2.LineColor = Color.Red
                objHorzLine.LineColor = Color.Red

                blnIsFHead = True

            End If

            'draw line
            objVerLine1.fncAddVerticalLine(objUpperCard, clsEnum.emCardPoint.MID_BOTTOM)
            objVerLine2.fncAddVerticalLine(objLowerCard, clsEnum.emCardPoint.MID_TOP)
            objHorzLine.fncAddHorizontalLine(objVerLine1, objVerLine2)

            'add to panel
            mpnDraw.Controls.Add(objVerLine1)
            mpnDraw.Controls.Add(objVerLine2)
            mpnDraw.Controls.Add(objHorzLine)

            'bring connector to front
            If blnIsFHead Then

                objVerLine1.BringToFront()
                objVerLine2.BringToFront()
                objHorzLine.BringToFront()

                'add to list
                mlstSpecialLine.Add(objVerLine1)
                mlstSpecialLine.Add(objVerLine2)
                mlstSpecialLine.Add(objHorzLine)

            Else
                'add to list
                mlstNormalLine.Add(objVerLine1)
                mlstNormalLine.Add(objVerLine2)
                mlstNormalLine.Add(objHorzLine)

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawDiffLv", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xHasChild, member has child or not
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intID Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xHasChild(ByVal intID As Integer) As Boolean

        xHasChild = False

        Dim vwdata As DataView = Nothing

        Try

            'vwdata = New DataView(mtblRel)
            vwdata = New DataView(mtblRelNaturalChild)
            vwdata.RowFilter = String.Format(mcstrFindRelFormat, intID, CInt(clsEnum.emRelation.NATURAL))

            If vwdata.Count = 0 Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xHasChild", ex)
        Finally
            If vwdata IsNot Nothing Then vwdata.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetMaxWidth, get max width of lower level
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetMaxWidth(ByVal intFather As Integer, ByRef intAtID As Integer, ByRef ptMin As Point) As Boolean

        xGetMaxWidth = False
        Dim objCard As usrMemberCard1 = Nothing
        'Dim vwUser As DataView = Nothing
        Dim lstChild As List(Of Integer)


        Try
            Dim intKid As Integer

            mintLv += 1

            'vwUser = New DataView(mtblRel)
            'lstChild = basCommon.fncGetKidList(intFather, clsEnum.emRelation.NATURAL)

            'Start Manh 2012/11/14
            lstChild = xGetKidList(intFather)
            'End Manh 2012/11/14

            'find how many kids?
            'vwUser.RowFilter = String.Format(mcstrFindRelFormat, intFather, CInt(clsEnum.emRelation.NATURAL))

            If lstChild.Count <= 0 Then
                mintLv -= 1
                Exit Function
            End If

            For i As Integer = 0 To lstChild.Count - 1

                'Integer.TryParse(basCommon.fncCnvNullToString(vwUser(i)(0)), intKid)
                intKid = lstChild(i)

                '2012 03 27 - test
                If mlstNotDraw.Contains(intKid) Then Continue For

                objCard = CType(mtblControl.Item(intKid), usrMemberCard1)

                If objCard Is Nothing Then Continue For

                If objCard.Location.X > mintMaxWidth Then

                    mintMaxWidth = objCard.Location.X + xWifeCount(intKid) * mintMEM_CARD_SPACE_LEFT + mintMEM_CARD_W
                    intAtID = intKid

                End If

                xGetMaxWidth(intKid, intAtID, ptMin)

            Next

            'Integer.TryParse(basCommon.fncCnvNullToString(vwUser(0)(0)), intKid)
            intKid = lstChild(0)

            objCard = CType(mtblControl.Item(intKid), usrMemberCard1)

            If objCard IsNot Nothing Then ptMin = objCard.Location

            mintLv -= 1

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetMaxWidth", ex)
        Finally
            'If vwUser IsNot Nothing Then vwUser.Dispose()
            lstChild = Nothing
        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xWidthOfMem, get max width of member and his wives
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xWidthOfMem(ByVal intID As Integer) As Integer

        xWidthOfMem = 0

        Try

            xWidthOfMem = mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT * xWifeCount(intID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xWithOfMem", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xWifeCount, count total wives of member
    '      VALUE      : Integer
    '      PARAMS     : intId Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xWifeCount(ByVal intId As Integer) As Integer

        xWifeCount = 0

        Dim vwUser As DataView = Nothing

        Try

            'vwUser = New DataView(mtblRel)

            vwUser = New DataView(mtblRelMarriage)
            'how many wife?
            vwUser.RowFilter = String.Format(mcstrFindRelFormat, intId, CInt(clsEnum.emRelation.MARRIAGE))
            '86 is the width of card
            xWifeCount = vwUser.Count

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xWifeCount", ex)
        Finally
            If vwUser IsNot Nothing Then vwUser.Dispose()
        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xResetValue, reset values
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xResetValue() As Boolean

        xResetValue = False

        Try
            mintLv = mcintStartLv
            mintX = mcintStartX
            mintY = mcintStartY
            mintWifeCount = mcintNONE_VALUE

            mintMaxWidth = mcintStartMaxWidth
            mintMaxWLv = mcintStartMaxWLv

            If mlstNotDraw IsNot Nothing Then mlstNotDraw.Clear()
            If mlstNormalLine IsNot Nothing Then mlstNormalLine.Clear()
            If mlstSpecialLine IsNot Nothing Then mlstSpecialLine.Clear()
            Array.Clear(mintMaX, 0, mintMaX.Length)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xResetValue", ex)
        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSetScrollView, set scroll view after drawing
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRootID   Integer, root id
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetScrollView(ByVal intRootID As Integer, Optional ByVal blnSetSelected As Boolean = False) As Boolean

        xSetScrollView = False

        Try
            Dim objCard As usrMemberCard1
            Dim intX As Integer
            Dim intY As Integer

            If Not mtblControl.ContainsKey(intRootID) Then Exit Function

            objCard = CType(mtblControl.Item(intRootID), usrMemberCard1)


            intX = mpnDraw.HorizontalScroll.Value + objCard.Location.X
            intY = mpnDraw.VerticalScroll.Value + objCard.Location.Y

            'catch for maximum and minimun value
            If intX < mpnDraw.HorizontalScroll.Minimum Then
                intX = mpnDraw.HorizontalScroll.Minimum
            ElseIf intX > mpnDraw.HorizontalScroll.Maximum Then
                intX = mpnDraw.HorizontalScroll.Maximum
            End If

            If intY < mpnDraw.VerticalScroll.Minimum Then
                intY = mpnDraw.VerticalScroll.Minimum
            ElseIf intY > mpnDraw.VerticalScroll.Maximum Then
                intY = mpnDraw.VerticalScroll.Maximum
            End If

            mpnDraw.AutoScrollPosition = New Point(intX, intY)

            'set selected bound
            If blnSetSelected Then
                'reset previous card
                If mobjTempSelectedCard IsNot Nothing Then mobjTempSelectedCard.CardSelected = False
                objCard.CardSelected = True

                'store this card
                mobjTempSelectedCard = objCard
                'mlstSelectedCtrl.Clear()
                mlstSelectedCtrl.Add(mobjTempSelectedCard)
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetScrollView", ex)
        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xClearControls, reset values
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncClearControls() As Boolean

        fncClearControls = False

        Try

            Array.Clear(mintMaX, 0, mintMaX.Length)

            'dipose control in hastable
            xDisposeCard(mtblControl)
            xDisposeCard(mtblWife)

            mtblControl.Clear()
            mtblWife.Clear()

            For Each ctrl As Control In mpnDraw.Controls
                ctrl.Dispose()
            Next

            mpnDraw.Controls.Clear()
            mpnDraw.CreateGraphics.Clear(Color.White)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncClearControls", ex)
        Finally

        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDisposeCard, reset values
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : mtblCards   Hashtable, table of control
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDisposeCard(ByVal mtblCards As Hashtable) As Boolean

        xDisposeCard = False

        Dim objCard As usrMemberCard1 = Nothing

        Try
            'dipose control in hastable
            For Each element As DictionaryEntry In mtblCards

                objCard = CType(element.Value, usrMemberCard1)
                If objCard IsNot Nothing Then

                    If objCard.CardImage IsNot Nothing Then objCard.CardImage.Dispose()
                    objCard.CardImageLocation = Nothing
                    objCard.Dispose()

                End If

                objCard = Nothing

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDisposeCard", ex)
        Finally
            If objCard IsNot Nothing Then objCard.Dispose()
        End Try

    End Function


    ''' <summary>
    ''' xRemoveFromList - remove card and its downline from list
    ''' </summary>
    ''' <param name="intId">card id</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xRemoveFromList(ByVal intId As Integer) As Boolean

        xRemoveFromList = False

        Dim lstKid As List(Of Integer)

        Try
            If mtblWife.ContainsKey(intId) Then

                mtblWife.Remove(intId)
                If mtblControl.ContainsKey(intId) Then mtblControl.Remove(intId)
                If Not mlstNotDraw.Contains(intId) Then mlstNotDraw.Add(intId)
                Return True

            End If

            mintLv += 1

            'limit level
            If mintLv > mintMaxGeneration Then 'My.Settings.intGeneration Then
                mintLv -= 1
                Exit Function
            End If

            'remove from list
            If mtblControl.ContainsKey(intId) Then mtblControl.Remove(intId)
            If Not mlstNotDraw.Contains(intId) Then mlstNotDraw.Add(intId)

            'remove his wife
            xRemoveWifeFromList(intId)

            'draw child

            'lstKid = basCommon.fncGetKidList(intId, clsEnum.emRelation.NATURAL)

            'Start Manh 2012/11/14
            lstKid = xGetKidList(intId)
            'End Manh 2012/11/14

            ''draw each  child
            If lstKid.Count > 0 Then

                For i As Integer = 0 To lstKid.Count - 1

                    xRemoveFromList(lstKid(i))

                Next

            End If

            mintLv -= 1

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRemoveFromList", ex)
        Finally
            lstKid = Nothing
        End Try

    End Function


    Private Function xRemoveWifeFromList(ByVal intHusId As Integer) As Boolean
        xRemoveWifeFromList = False

        Dim objDictWife As Dictionary(Of Integer, String) = Nothing

        Try
            objDictWife = basCommon.fncGetHusWifeList(intHusId)

            If objDictWife Is Nothing Then Return True

            For Each element As KeyValuePair(Of Integer, String) In objDictWife

                'continue if it is not in list
                If Not mtblWife.ContainsKey(element.Key) Then Continue For

                If mtblControl.ContainsKey(element.Key) Then mtblControl.Remove(element.Key)
                If Not mlstNotDraw.Contains(CInt(element.Key)) Then mlstNotDraw.Add(CInt(element.Key))

            Next

            Return True

        Catch ex As Exception

        End Try
    End Function

    Private Function xResetLocation(ByVal intID As Integer, ByVal intX As Integer, ByVal intY As Integer) As Boolean

        xResetLocation = False

        Try
            Dim objCard As usrMemberCard1

            If mlstNotDraw.Contains(intID) Then Exit Function

            If Not mtblControl.ContainsKey(intID) Then Exit Function

            objCard = CType(mtblControl.Item(intID), usrMemberCard1)

            'reset location and width
            objCard.Location = New Point(intX, intY)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClearLine", ex)
        End Try

    End Function

    Private Function xClearLine() As Boolean

        xClearLine = False

        Try
            'If mlstNormalLine Is Nothing Then Exit Sub
            If mlstNormalLine IsNot Nothing Then

                'dispose
                For i As Integer = 0 To mlstNormalLine.Count - 1
                    mlstNormalLine(i).Dispose()
                Next
                mlstNormalLine.Clear()

            End If

            If mlstSpecialLine IsNot Nothing Then

                'dispose
                For i As Integer = 0 To mlstSpecialLine.Count - 1
                    mlstSpecialLine(i).Dispose()
                Next
                mlstSpecialLine.Clear()

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClearLine", ex)
        End Try
    End Function

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free unmanaged resources when explicitly called
            End If

            ' TODO: free shared unmanaged resources
            xDisposeCard(mtblControl)
            xDisposeCard(mtblWife)
            If mtblUser IsNot Nothing Then mtblUser.Dispose()
            If mtblRel IsNot Nothing Then mtblRel.Dispose()
            If mtblDrawLv IsNot Nothing Then mtblDrawLv.Dispose()
            If mtblControl IsNot Nothing Then mtblControl.Clear()
            If mtblWife IsNot Nothing Then mtblWife.Clear()

            If mobjCardLeft IsNot Nothing Then mobjCardLeft.Dispose()
            If mobjCardRight IsNot Nothing Then mobjCardRight.Dispose()

            Erase mintMaX

        End If
        Me.disposedValue = True
    End Sub


#Region "Event handler"


    '   ******************************************************************
    '　　　FUNCTION   : xShowPerInfo, handler double click on card
    '      PARAMS     : intMemID   Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xShowPerInfo(ByVal intMemID As Integer)

        Try
            If intMemID <= basConst.gcintNO_MEMBER Then Exit Sub

            'raise event for set selected user in quick search list
            RaiseEvent evnCardDoubleClicked(intMemID)

            mfrmPerInfo.FormMode = clsEnum.emMode.EDIT
            mfrmPerInfo.MemberID = intMemID

            'show form 
            If Not mfrmPerInfo.fncShowForm() Then Exit Sub

            'if member is not edied
            If Not mfrmPerInfo.FormModified Then Exit Sub

            'redraw this card
            fncRedrawCard(intMemID)

            'member is edited, raise event for refreshing
            RaiseEvent evnRefresh(intMemID, True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowPerInfo", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xCardClicked, handler click on card
    '      PARAMS     : intMemID   Integer, member id
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCardClicked(ByVal intMemID As Integer)

        Try
            If intMemID <= basConst.gcintNO_MEMBER Then Exit Sub

            'Dim objCard As usrMemberCard1

            'If Not mtblControl.ContainsKey(intMemID) Then Exit Sub

            'objCard = CType(mtblControl.Item(intMemID), usrMemberCard1)

            ''reset previous card
            'If mobjTempSelectedCard IsNot Nothing Then mobjTempSelectedCard.CardSelected = False
            'objCard.CardSelected = True

            ''store this card
            'mobjTempSelectedCard = objCard
            ''mlstSelectedCtrl.Clear()
            'mlstSelectedCtrl.Add(mobjTempSelectedCard)

            'raise event for set selected user in quick search list
            RaiseEvent evnCardClicked(intMemID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowPerInfo", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xHandleNotDraw, remove a member from tree
    ''' </summary>
    ''' <param name="objCard"></param>
    ''' <param name="intID"></param>
    ''' <remarks></remarks>
    Private Sub xHandleNotDraw(ByVal objCard As usrMemCardBase, ByVal intID As Integer)
        Try
            'mtblNotDraw.Add(intID, objCard)
            If Not mtblControl.ContainsKey(intID) Then Exit Sub

            'mtblControl.Remove(intID)
            xRemoveFromList(intID)

            ''clear line for drawing new one
            xClearLine()
            mpnDraw.Controls.Clear()

            xResetValue()

            'draw father card
            xDrawFather(mintRootID, True)

            'do recusive to add member to table
            xRecusiveDraw(mintRootID, True)

            'align father card
            xAlignFather(mintRootID)

            'center align control
            xAlignControls(mintRootID)

            xResetValue()

            'add mother
            xAddMother(mintRootID, True)

            'add wife to table
            xAddWife(mintRootID, True)

            'add control from table to panel
            xAddCtrl2Panel()

            'set viewing region
            'xSetScrollView(intID)

            'lines
            xDrawConnector()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xHandlerNotDraw", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Multi-select controls
    ''' </summary>
    ''' <param name="rectArea">selection area</param>
    ''' <Create>2012/04/09  AKB Quyet</Create>
    ''' <remarks></remarks>
    Private Sub xMultiSelect(ByVal rectArea As Rectangle)
        Try

            basCommon.fncMultiSelectCtrl(rectArea, mlstSelectedCtrl, mtblControl)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMultiSelect", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Move cards
    ''' </summary>
    ''' <param name="objCard">card be moved</param>
    ''' <param name="intX">offset of X</param>
    ''' <param name="intY">offset of Y</param>
    ''' <Create>2012/04/09  AKB Quyet</Create>
    ''' <remarks></remarks>
    Private Sub xCardMove(ByVal objCard As usrMemCardBase, ByVal intX As Integer, ByVal intY As Integer)
        Try

            basCommon.fncMoveCards(objCard, intX, intY, mlstSelectedCtrl)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCardMove", ex)
        End Try
    End Sub



#End Region


#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region


#Region "NOT USED"


#End Region

    Public Function fncGetMaxWidthInMM() As Integer

        Dim objGraphic As Graphics = mpnDraw.CreateGraphics()
        Dim intDPI As Integer = CInt(Me.MaxWidth / objGraphic.DpiX * 25.4F)

        objGraphic = Nothing

        Return intDPI

    End Function

    Public Function fncGetMaxHeightInMM() As Integer

        Dim objGraphic As Graphics = mpnDraw.CreateGraphics()
        Dim intDPI As Integer = CInt(Me.MaxHeight / objGraphic.DpiY * 25.4F)

        objGraphic = Nothing
        Return intDPI

    End Function

End Class
