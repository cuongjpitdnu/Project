'   ******************************************************************
'      TITLE      : DRAW FAMILY TREE WITH NEW METHOD TO IMPROVE perfromance
'　　　FUNCTION   : Using this tree, people can not distinguish who is father of a child
'      MEMO       : 
'      CREATE     : 2012/11/21　AKB Manh
'      UPDATE     : 
'
'                  2012 AKB SOFTWARE
'   ******************************************************************

Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : DRAW FAMILY TREE WITH NEW METHOD
'      MEMO       : 
'      CREATE     : 2012/11/21  AKB Manh
'      UPDATE     : 
'   ******************************************************************
Public Class clsDrawTreeS3
    Implements IDisposable

#Region "Constants"
    Private Const mcstrClsName As String = "clsDrawTreeNew1"                                   'class name
    Private Const mcstrFindRelFormat As String = "REL_FMEMBER_ID = {0} AND RELID = {1}"     'format to find member with relationship
    Private Const mcstrFindUsrFormat As String = "MEMBER_ID = {0}"                          'format to find member

#End Region

    Private mintRootID As Integer                       'root id to draw
    Private mintMaxGeneration As Integer                'Max Generatoion from RootID
    Private mintMaxDrawGen As Integer                   'Max Generatoion from RootID

    Private mpnDraw As pnTreePanel                      'panel to draw
    Private mtblUser As DataTable                       'table to store members
    Private mtblRel As DataTable                        'table to store relationship
    Private mtblRelMarriage As DataTable                'table to store relationship of Marriage member
    Private mtblRelChild As DataTable            'table to store relationship of NaturalChild Member
    Private mtblCardInfo As Hashtable                   'table to store info of drawing card
    Private mtblCardCtrl As Hashtable                   'table to store drawing card
    Private mlstNormalLine As List(Of usrLine)          'table to store control (line)
    Private mlstSpecialLine As List(Of usrLine)         'table to store control (line)
    Private mlstSelectedCtrl As List(Of usrMemCardBase) 'list to store selected controls
    Private mobjTempSelectedCard As usrMemberCard1      'temporary selected card

    Private mintMEM_CARD_SPACE_LEFT As Integer          'wi
    Private mintMEM_CARD_SPACE_DOWN As Integer          'wi
    Private mintMEM_CARD_W As Integer
    Private mintMEM_CARD_H As Integer
    Private mblnIsSmallCard As Boolean                  'draw small card

    Private mintStartX As Integer                       'Start X
    Private mintStartY As Integer                       'Start Y
    Private mfrmPerInfo As frmPersonInfo                'personal information form

    Public Event evnCardClicked(ByVal intMemID As Integer)
    Public Event evnCardDoubleClicked(ByVal intMemID As Integer)
    Public Event evnRefresh(ByVal intMemID As Integer, ByVal blnRedraw As Boolean)

    Private Const mcintDeltaSibling As Integer = 1

    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2012/09/14  AKB Manh 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New(ByVal pnDraw As pnTreePanel, _
                   ByVal intRootID As Integer, _
                   ByVal intMaxGeneration As Integer, _
                   ByVal frmPerInfo As frmPersonInfo)
        Try

            mfrmPerInfo = frmPerInfo
            mpnDraw = pnDraw
            mintRootID = intRootID
            mintMaxGeneration = intMaxGeneration
            mintMaxDrawGen = 1

            mtblUser = Nothing
            mtblRel = Nothing
            mtblRelMarriage = Nothing
            mtblRelChild = Nothing
            mtblCardInfo = New Hashtable()
            mtblCardCtrl = New Hashtable()
            mlstNormalLine = New List(Of usrLine)
            mlstSpecialLine = New List(Of usrLine)
            mlstSelectedCtrl = New List(Of usrMemCardBase)
            'add handler of panel
            AddHandler mpnDraw.evnMultiSelection, AddressOf xMultiSelect

            mblnIsSmallCard = True
            If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then mblnIsSmallCard = False


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "Constructor New", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xClearControls, reset values
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncClearControls() As Boolean

        fncClearControls = False

        Try

            'dipose control in hastable
            xDisposeCard(mtblCardCtrl)
            mtblCardCtrl.Clear()
            mtblCardInfo.Clear()

            For Each ctrl As Control In mpnDraw.Controls

                ctrl.Dispose()
                Application.DoEvents()

            Next

            mpnDraw.Controls.Clear()
            mpnDraw.CreateGraphics.Clear(Color.White)

            If mlstNormalLine IsNot Nothing Then mlstNormalLine.Clear()
            If mlstSpecialLine IsNot Nothing Then mlstSpecialLine.Clear()

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
    '      CREATE     : 2012/09/14  AKB Manh
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
                Application.DoEvents()
            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDisposeCard", ex)
        Finally
            If objCard IsNot Nothing Then objCard.Dispose()
        End Try

    End Function

    ''   ******************************************************************
    ''　　　FUNCTION   : xAddCtrl2Panel, add controls to panel
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : 
    ''      MEMO       : 
    ''      CREATE     : 2012/09/14  AKB Manh
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xAddCtrl2PanelOld() As Boolean

    '    xAddCtrl2PanelOld = False

    '    Try
    '        Dim objCard As usrMemberCard1
    '        Dim stCard As stCardInfo

    '        For Each element As DictionaryEntry In mtblCardInfo

    '            stCard = CType(mtblCardInfo.Item(element.Key), stCardInfo)
    '            objCard = fncMakeCardInfoType1(stCard, mblnIsSmallCard)

    '            'add handler
    '            AddHandler objCard.evnCardDoubleClick, AddressOf xShowPerInfo
    '            AddHandler objCard.evnCardClick, AddressOf xCardClicked
    '            'AddHandler objCard.evnNotDraw, AddressOf xHandleNotDraw
    '            AddHandler objCard.evnCardLocationChange, AddressOf xCardMove


    '            mtblCardCtrl.Add(stCard.intID, objCard)
    '            mpnDraw.Controls.Add(objCard)
    '            Application.DoEvents()

    '        Next

    '        Dim objCard2 As usrMemberCard1
    '        Dim i As Integer
    '        For Each element As DictionaryEntry In mtblCardInfo
    '            stCard = CType(mtblCardInfo.Item(element.Key), stCardInfo)
    '            objCard = CType(mtblCardCtrl.Item(element.Key), usrMemberCard1)

    '            If Not stCard.lstSpouse Is Nothing Then
    '                For i = 0 To stCard.lstSpouse.Count - 1
    '                    objCard2 = CType(mtblCardCtrl.Item(stCard.lstSpouse(i)), usrMemberCard1)
    '                    xDrawSameLv(objCard, objCard2)
    '                    objCard = objCard2
    '                    Application.DoEvents()
    '                Next
    '            End If

    '            objCard = CType(mtblCardCtrl.Item(element.Key), usrMemberCard1)
    '            If Not stCard.lstChild Is Nothing Then
    '                For i = 0 To stCard.lstChild.Count - 1
    '                    objCard2 = CType(mtblCardCtrl.Item(stCard.lstChild(i)), usrMemberCard1)
    '                    xDrawDiffLv(objCard, objCard2)
    '                    Application.DoEvents()
    '                Next
    '            End If

    '        Next
    '        mpnDraw.Visible = True
    '        xSetScrollView(mintRootID)
    '        Return True

    '    Catch ex As Exception

    '        basCommon.fncSaveErr(mcstrClsName, "xAddCtrl2PanelOld", ex)
    '    End Try

    'End Function

    Private Sub xAddHandler(ByRef objCard As usrMemberCard1)
        AddHandler objCard.evnCardDoubleClick, AddressOf xShowPerInfo
        AddHandler objCard.evnCardClick, AddressOf xCardClicked
        'AddHandler objCard.evnNotDraw, AddressOf xHandleNotDraw
        AddHandler objCard.evnCardLocationChange, AddressOf xCardMove
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xAddCtrl2Panel, add controls to panel
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddCtrl2Panel() As Boolean

        xAddCtrl2Panel = False

        Try
            Dim objCard As usrMemberCard1
            Dim stCard As stCardInfo

            For Each element As DictionaryEntry In mtblCardInfo

                stCard = CType(mtblCardInfo.Item(element.Key), stCardInfo)
                objCard = fncMakeCardInfoType1(stCard, mblnIsSmallCard)

                objCard.Location = New Point(stCard.intX, stCard.intY)
                objCard.DrawLv = stCard.intLevel
                'add handler
                xAddHandler(objCard)
                mtblCardCtrl.Add(stCard.intID, objCard)
                mpnDraw.Controls.Add(objCard)
                Application.DoEvents()

            Next

            Dim objCard2 As usrMemberCard1
            Dim i As Integer

            For Each element As DictionaryEntry In mtblCardInfo
                stCard = CType(mtblCardInfo.Item(element.Key), stCardInfo)
                objCard = CType(mtblCardCtrl.Item(element.Key), usrMemberCard1)

                If Not stCard.lstSpouse Is Nothing Then
                    For i = 0 To stCard.lstSpouse.Count - 1
                        objCard2 = CType(mtblCardCtrl.Item(stCard.lstSpouse(i)), usrMemberCard1)
                        xDrawSameLv(objCard, objCard2)
                        objCard = objCard2
                        Application.DoEvents()
                    Next
                End If

                objCard = CType(mtblCardCtrl.Item(element.Key), usrMemberCard1)
                If Not stCard.lstChild Is Nothing Then
                    For i = 0 To stCard.lstChild.Count - 1
                        objCard2 = CType(mtblCardCtrl.Item(stCard.lstChild(i)), usrMemberCard1)
                        xDrawDiffLv(objCard, objCard2)
                        Application.DoEvents()
                    Next
                End If

            Next
            mpnDraw.Visible = True
            xSetScrollView(mintRootID)
            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xAddCtrl2Panel", ex)
        End Try

    End Function


    ''' <summary>
    ''' Multi-select controls
    ''' </summary>
    ''' <param name="rectArea">selection area</param>
    ''' <Create>2012/04/09  AKB Manh</Create>
    ''' <remarks></remarks>
    Private Sub xMultiSelect(ByVal rectArea As Rectangle)
        Try

            basCommon.fncMultiSelectCtrl(rectArea, mlstSelectedCtrl, mtblCardCtrl)
            'basCommon.fncMultiSelectCtrl(rectArea, mlstSelectedCtrl, gtblMemberCard)

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
    ''' <Create>2012/04/09  AKB Manh</Create>
    ''' <remarks></remarks>
    Private Sub xCardMove(ByVal objCard As usrMemCardBase, ByVal intX As Integer, ByVal intY As Integer)
        Try

            basCommon.fncMoveCards(objCard, intX, intY, mlstSelectedCtrl)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCardMove", ex)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xCardClicked, handler click on card
    '      PARAMS     : intMemID   Integer, member id
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCardClicked(ByVal intMemID As Integer)

        Try
            If intMemID <= basConst.gcintNO_MEMBER Then Exit Sub

            Dim objCard As usrMemberCard1

            If Not mtblCardCtrl.ContainsKey(intMemID) Then Exit Sub

            objCard = CType(mtblCardCtrl.Item(intMemID), usrMemberCard1)

            xSetSelected(objCard)

            'raise event for set selected user in quick search list
            RaiseEvent evnCardClicked(intMemID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowPerInfo", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xShowPerInfo, handler double click on card
    '      PARAMS     : intMemID   Integer, member id
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xShowPerInfo(ByVal intMemID As Integer)

        Try
            If intMemID <= basConst.gcintNO_MEMBER Then Exit Sub

            'raise event for set selected user in quick search list
            'RaiseEvent evnCardDoubleClicked(intMemID)

            mfrmPerInfo.FormMode = clsEnum.emMode.EDIT
            mfrmPerInfo.MemberID = intMemID

            'show form 
            If Not mfrmPerInfo.fncShowForm() Then Exit Sub

            'if member is not edied
            If Not mfrmPerInfo.FormModified Then Exit Sub

            'redraw this card
            fncRedrawCard(intMemID)

            'member is edited, raise event for refreshing
            'RaiseEvent evnRefresh(intMemID, True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowPerInfo", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xSetScrollView, set scroll view after drawing
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRootID   Integer, root id
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetScrollView(ByVal intRootID As Integer, Optional ByVal blnSetSelected As Boolean = False) As Boolean

        xSetScrollView = False

        Try
            Dim objCard As usrMemberCard1
            Dim intX As Integer
            Dim intY As Integer

            If Not mtblCardCtrl.ContainsKey(intRootID) Then Exit Function
            'If Not gtblMemberCard.ContainsKey(intRootID) Then Exit Function

            objCard = CType(mtblCardCtrl.Item(intRootID), usrMemberCard1)
            'objCard = CType(gtblMemberCard.Item(intRootID), usrMemberCard1)

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

                xSetSelected(objCard)

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetScrollView", ex)
        End Try


    End Function


    ''' <summary>
    ''' xSetSelected - set selected bound
    ''' </summary>
    ''' <param name="objCard"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xSetSelected(ByVal objCard As usrMemberCard1) As Boolean

        xSetSelected = False

        Try
            'reset previous card
            If mobjTempSelectedCard IsNot Nothing Then mobjTempSelectedCard.CardSelected = False
            objCard.CardSelected = True

            'store this card
            mobjTempSelectedCard = objCard
            mlstSelectedCtrl.Clear()
            mlstSelectedCtrl.Add(mobjTempSelectedCard)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetSelected", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDrawDiffLv, draw different level
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
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
    '　　　FUNCTION   : xDrawSameLv, draw same level
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
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
    '      FUNCTION   : xSetCardSize, init value 
    '      MEMO       :  
    '      CREATE     : 2012/01/11  AKB Manh 
    '      UPDATE     :  
    '   ******************************************************************
    Private Sub xSetCardSize()

        Try

            If mblnIsSmallCard Then
                mintMEM_CARD_SPACE_LEFT = clsDefine.MEM_CARD_HORIZON_BUFFER_S
                mintMEM_CARD_SPACE_DOWN = clsDefine.MEM_CARD_VERTICAL_BUFFER_S
                mintMEM_CARD_W = clsDefine.MEM_CARD_W_S
                mintMEM_CARD_H = clsDefine.MEM_CARD_H_S
            Else
                mintMEM_CARD_SPACE_LEFT = clsDefine.MEM_CARD_HORIZON_BUFFER_L
                mintMEM_CARD_SPACE_DOWN = clsDefine.MEM_CARD_VERTICAL_BUFFER_L
                mintMEM_CARD_W = clsDefine.MEM_CARD_W_L
                mintMEM_CARD_H = clsDefine.MEM_CARD_H_L
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetCardSize", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xReadData, read data from database
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
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
            'mtblRelChild = gobjDB.fncGetChild()
            mtblRelChild = gobjDB.fncGetChildFull(, , " ROLE_ORDER ASC, D.FAMILY_ORDER ASC")

            If mtblUser Is Nothing Then Exit Function
            If mtblRel Is Nothing Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xReadData", ex)
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : Draw Tree
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncDraw(ByVal intRootID As Integer, ByVal intMaxGeneration As Integer, _
                            ByVal intInitX As Integer, _
                            ByVal intInitY As Integer) As Boolean

        Try
            mintRootID = intRootID
            mintMaxGeneration = intMaxGeneration
            mpnDraw.Visible = False
            fncClearControls()

            mblnIsSmallCard = True
            If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then mblnIsSmallCard = False
            xSetCardSize()
            xReadData()

            xMakeInfoTree(mintRootID, 1, Nothing)
            Dim intX As Integer = intInitX

            mintStartX = intInitX
            mintStartY = intInitY

            xCaculateCoordinateTree(mintRootID, 1, intX, intInitY)

            'Making Card and Add to panel
            xAddCtrl2Panel()

        Catch ex As Exception
            Throw ex
        End Try


    End Function

    '   ******************************************************************
    '　　　FUNCTION   : Caculate Coordinate of Tree
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function xCaculateCoordinateTree(ByVal intRootID As Integer, _
                                            ByVal intGeneration As Integer, _
                                            ByRef intX As Integer, _
                                            ByVal intY As Integer) As Boolean

        If intGeneration > mintMaxGeneration Then Return True

        Dim stCard As stCardInfo
        stCard = xGetMemberCard(intRootID)
        Application.DoEvents()
        If Not stCard.lstChild Is Nothing Then
            Dim i As Integer

            For i = 0 To stCard.lstChild.Count - 1
                Application.DoEvents()
                Dim intSiblingID As Integer = xUpperNoChildSiblingID(stCard.lstChild(i)) 'xUpperSiblingID(stCard.lstChild(i))

                If intSiblingID > 0 Then
                    If xGetMemberCard(intSiblingID).lstChild Is Nothing Then
                        intX = xGetMemberCard(intSiblingID).intX
                    End If

                    Dim int2xUpperSiblingID As Integer = xUpperSiblingID(intSiblingID)
                    If int2xUpperSiblingID > 0 Then
                        If xGetMemberCard(int2xUpperSiblingID).lstChild IsNot Nothing Then
                            intX = xGetMemberCard(int2xUpperSiblingID).intMaxRight
                        End If
                    End If
                End If

                xCaculateCoordinateTree(stCard.lstChild(i), intGeneration + 1, intX, intY)
                Application.DoEvents()
            Next

            xCaculateCoordinateMember(stCard, intGeneration, intX, intY)
            mtblCardInfo.Item(intRootID) = stCard
            intX = stCard.intMaxRight

        Else

            xCaculateCoordinateMember(stCard, intGeneration, intX, intY)
            mtblCardInfo.Item(intRootID) = stCard
            intX = stCard.intMaxRight

        End If

    End Function

    Private Function xUpperNoChildSiblingID(ByVal intMemID As Integer) As Integer

        xUpperNoChildSiblingID = -1
        Dim stMember As stCardInfo = xGetMemberCard(intMemID)

        If stMember.lstChild Is Nothing Then Return -1
        If stMember.lstChild.Count <= 1 Then Return -1
        If stMember.lstSibling Is Nothing Then Return -1
        If stMember.lstSibling.Count = 1 Then Return -1
        Dim intMemberIndex As Integer = xFindIndex(stMember.lstSibling, intMemID)
        If intMemberIndex <= 0 Then Return -1

        Dim i As Integer
        Dim intRet As Integer = -1
        Dim intMaxRight As Integer = 0

        For i = 1 To stMember.lstChild.Count - 1
            Dim stSibling As stCardInfo
            Dim intDelta As Integer = intMemberIndex - i

            If intDelta >= 0 Then
                stSibling = xGetMemberCard(stMember.lstSibling(intDelta))
                If stSibling.lstChild IsNot Nothing Then
                    intMaxRight = stSibling.intMaxRight
                    Exit For
                End If

                intRet = stSibling.intID
            End If

        Next

        If intRet < 0 Then Return -1
        If intMaxRight >= xGetMemberCard(intRet).intMaxRight Then Return -1

        Return intRet

    End Function

    Private Function xUpperSiblingID(ByVal intMemID As Integer, _
                                           Optional ByVal intBeforeNum As Integer = 1) As Integer

        Dim stMember As stCardInfo = xGetMemberCard(intMemID)
        Return xUpperSiblingID(stMember.lstSibling, intMemID)

    End Function

    Private Function xUpperSiblingID(ByVal lstSibling As List(Of Integer), ByVal intMemID As Integer) As Integer

        xUpperSiblingID = -1
        Dim i As Integer = xFindIndex(lstSibling, intMemID)
        If i <= 0 Then Return -1
        Return lstSibling(i - 1)

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : Caculate Coordinate of a Member
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCaculateCoordinateMember(ByRef stCard As stCardInfo, _
                                         ByRef intGeneration As Integer, _
                                         ByVal intInitX As Integer, _
                                         ByVal intInitY As Integer) As Boolean
        xCaculateCoordinateMember = False
        Try
            If stCard.lstChild Is Nothing Then
                Dim intUpSiblingID As Integer = xUpperSiblingID(stCard.lstSibling, stCard.intID)
                Dim stUpSibling As stCardInfo

                If intUpSiblingID > 0 Then

                    stUpSibling = xGetMemberCard(intUpSiblingID)
                    intInitX = stUpSibling.intMaxRight

                    If stUpSibling.lstSpouse Is Nothing Then
                        intInitX = stUpSibling.intX + mintMEM_CARD_W + mcintDeltaSibling * mintMEM_CARD_SPACE_LEFT
                    Else
                        Dim stSpouse As stCardInfo = xGetMemberCard(stUpSibling.lstSpouse(stUpSibling.lstSpouse.Count - 1))
                        intInitX = stSpouse.intX + mintMEM_CARD_W + mcintDeltaSibling * mintMEM_CARD_SPACE_LEFT
                    End If

                End If

                stCard.intX = intInitX
                stCard.intY = intInitY + (intGeneration - 1) * (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN)
                stCard.intMinLeft = intInitX

                stCard.intMaxRight = xMakeSpouse(stCard) 'stCard.intX + (i + 1) * (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
                If stCard.intMaxRight < 0 Then

                    stCard.intMaxRight = stCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT

                End If

                If intUpSiblingID > 0 Then
                    If stCard.intMaxRight < stUpSibling.intMaxRight Then
                        stCard.intMaxRight = stUpSibling.intMaxRight
                    End If
                End If

                Return True
            End If

            Dim stFirstChild As stCardInfo
            Dim stLastChild As stCardInfo
            stLastChild = xGetMemberCard(stCard.lstChild(stCard.lstChild.Count - 1))
            stFirstChild = xGetMemberCard(stCard.lstChild(0))

            stCard.intX = stFirstChild.intX
            stCard.intY = intInitY + (intGeneration - 1) * (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN)
            stCard.intMinLeft = stFirstChild.intMinLeft

            stCard.intMaxRight = xMakeSpouse(stCard)
            If stCard.intMaxRight < 0 Then
                stCard.intMaxRight = stCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
            End If

            'If there is more than one child, need to recalculate the X and Max Right
            If stCard.lstChild.Count > 1 Then

                'stCard.intX = stFirstChild.intMinLeft + CInt((stLastChild.intMaxRight - stCard.intMaxRight) / 2)
                'stCard.intX = stFirstChild.intX + CInt((stLastChild.intMaxRight - stCard.intMaxRight) / 2)
                stCard.intX = stFirstChild.intX + CInt((stLastChild.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT - stCard.intMaxRight) / 2)
                If stCard.intX < 0 Then
                    stCard.intX = stFirstChild.intX
                End If

                xAdjustWithSibling(stCard)

                stCard.intMaxRight = xMakeSpouse(stCard)

            End If

            'Upadte MaxRight of this card
            If stCard.intMaxRight <= stLastChild.intMaxRight Then
                stCard.intMaxRight = stLastChild.intMaxRight
            End If


            mtblCardInfo.Item(stCard.intID) = stCard

            'If mintMaxDrawGen - 1 = intGeneration Then
            '    xImpactMember(stCard.intID)
            '    stCard = xGetMemberCard(stCard.intID)
            'End If

            Return True
        Catch ex As Exception

        End Try

    End Function

    Private Sub xAdjustWithSibling(ByRef stMember As stCardInfo)

        Dim intSibblingID As Integer = xUpperSiblingID(stMember.intID)
        If intSibblingID <= 0 Then Return

        Dim stSibling As stCardInfo = xGetMemberCard(intSibblingID)
        If stSibling.lstChild IsNot Nothing Then Return

        If stMember.intX < stSibling.intMaxRight Then
            stMember.intX = stSibling.intMaxRight
        End If

    End Sub

    Private Function xGetMemberCard(ByVal intID As Integer) As stCardInfo
        Return CType(mtblCardInfo.Item(intID), stCardInfo)
    End Function

    Private Function xFindIndex(ByVal lstList As List(Of Integer), ByVal intValue As Integer) As Integer
        xFindIndex = -1

        If lstList Is Nothing Then Return -1
        If lstList.Count = 0 Then Return -1

        Dim i As Integer
        Dim intMemberIndex As Integer = -1

        For i = 0 To lstList.Count - 1
            If lstList(i) = intValue Then
                intMemberIndex = i
                Exit For
            End If
        Next

        Return intMemberIndex

    End Function

    Private Sub xImpactMember(ByVal intMemberID As Integer)

        Dim stMember As stCardInfo = xGetMemberCard(intMemberID)

        If stMember.intID <= 0 Then Return
        If stMember.lstChild Is Nothing Then Return
        If stMember.lstChild.Count = 0 Then Return
        If stMember.lstSibling Is Nothing Then Return
        If stMember.lstSibling.Count = 0 Then Return
        If stMember.lstSibling(0) = stMember.intID Then Return

        Dim i As Integer
        Dim intIndex As Integer
        Dim intMemberIndex As Integer = xFindIndex(stMember.lstSibling, stMember.intID)
        Dim stSibling As stCardInfo = xGetMemberCard(stMember.lstSibling(intMemberIndex - 1))

        If stSibling.lstChild IsNot Nothing Then Return


        Dim intDistance As Integer = stMember.intX - stSibling.intMaxRight
        Dim intNumberMem As Integer = CInt(intDistance / (mintMEM_CARD_W + mcintDeltaSibling * mintMEM_CARD_SPACE_LEFT))

        If intDistance / (mintMEM_CARD_W + mcintDeltaSibling * mintMEM_CARD_SPACE_LEFT) < intNumberMem Then

            intNumberMem = intNumberMem - 1

        End If

        stSibling = xGetMemberCard(stMember.lstSibling(intMemberIndex - 1))
        intDistance = stMember.intMinLeft - stSibling.intX

        For i = intNumberMem To 1 Step -1
            intIndex = intMemberIndex - i
            If intIndex < 0 Then Continue For

            stSibling = xGetMemberCard(stMember.lstSibling(intIndex))
            If stSibling.lstChild IsNot Nothing Then Continue For

            intDistance = xGetMemberCard(stMember.lstChild(0)).intX - stSibling.intX
            Exit For
        Next i


        xMoveMemberNoChild(stMember.intID, intDistance)
        For i = 0 To stMember.lstChild.Count - 1
            'Dim stChild As stCardInfo = xGetMemberCard(stMember.lstChild)
            xMoveMemberNoChild(stMember.lstChild(i), intDistance)

        Next


    End Sub

    Private Sub xMoveMemberNoChild(ByVal intMemberID As Integer, ByVal intDistance As Integer)

        Dim stMember As stCardInfo = xGetMemberCard(intMemberID)

        stMember.intX = stMember.intX - intDistance
        stMember.intMinLeft = stMember.intMinLeft - intDistance
        stMember.intMaxRight = stMember.intMaxRight - intDistance

        mtblCardInfo.Item(intMemberID) = stMember

        If stMember.lstSpouse Is Nothing Then Return
        Dim i As Integer

        For i = 0 To stMember.lstSpouse.Count - 1
            Dim stWife As stCardInfo

            stWife = xGetMemberCard(stMember.lstSpouse(i))
            stWife.intX = stWife.intX - intDistance
            stWife.intMinLeft = stWife.intMinLeft - intDistance
            stWife.intMaxRight = stWife.intMaxRight - intDistance
            mtblCardInfo.Item(stWife.intID) = stWife
        Next

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xMakeSpouse, Caculate the spouse coordinates
    '      VALUE      : Integer (maxwidth of All Spouse)
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xMakeSpouse(ByVal stCard As stCardInfo) As Integer
        Dim i As Integer
        Dim stSpouse As stCardInfo
        xMakeSpouse = -999

        If Not stCard.lstSpouse Is Nothing Then
            For i = 0 To stCard.lstSpouse.Count - 1
                stSpouse = CType(mtblCardInfo.Item(stCard.lstSpouse(i)), stCardInfo)
                stSpouse.intY = stCard.intY
                stSpouse.intX = stCard.intX + (i + 1) * (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
                stSpouse.intMaxRight = stSpouse.intX + (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
                mtblCardInfo.Item(stCard.lstSpouse(i)) = stSpouse
            Next
            xMakeSpouse = stSpouse.intMaxRight
        End If

    End Function



    '   ******************************************************************
    '　　　FUNCTION   : fncRedrawCard, redraw a card
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncRedrawCard(ByVal intMemID As Integer) As Boolean

        fncRedrawCard = False

        Dim stCard As stCardInfo
        Dim objCard As usrMemberCard1

        Try
            'read data
            mtblUser = gobjDB.fncGetMemberMain()
            stCard = fncGetMemberInfo(intMemID, mtblUser)

            mtblCardInfo.Remove(intMemID)
            mtblCardInfo.Add(intMemID, stCard)

            objCard = CType(mtblCardCtrl.Item(intMemID), usrMemberCard1)
            fncUpdateCardBase1(objCard, stCard, mblnIsSmallCard)

            objCard = Nothing

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
    '      CREATE     : 2012/09/14  AKB Manh
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
    '　　　FUNCTION   : xMakeInfoTree, Make info family tree
    '      VALUE      : Boolean : true: OK
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function xMakeInfoTree(ByVal intRootID As Integer, _
                                  ByVal intGeneration As Integer, _
                                  ByVal lstSibling As List(Of Integer)) As Boolean

        xMakeInfoTree = False
        If mtblCardInfo.Contains(intRootID) Then Return True

        If intGeneration > mintMaxGeneration Then Return True

        Dim stCard As stCardInfo
        'Dim dtSpouse As DataTable
        Dim drData As DataRow()

        stCard = fncGetMemberInfo(intRootID, mtblUser)
        stCard.intLevel = intGeneration
        stCard.lstSibling = lstSibling

        drData = fncGetRowsFromDataTable(mtblRelMarriage, String.Format("MEMBER_ID = {0} AND RELID = {1}", intRootID, CInt(clsEnum.emRelation.MARRIAGE)), "ROLE_ORDER ASC")
        stCard.lstSpouse = fncMakeMemberIDList(drData, "REL_FMEMBER_ID")
        stCard.lstChild = Nothing

        If intGeneration < mintMaxGeneration Then
            drData = fncGetRowsFromDataTable(mtblRelChild, "SPOUSE_LEFT = " & intRootID.ToString())
            'stCard.lstChild = fncMakeMemberIDList(drData, "CHILD_ID")
            stCard.lstChild = xGetKidList(drData, "CHILD_ID")

        End If

        mtblCardInfo.Add(intRootID, stCard)

        If Not stCard.lstSpouse Is Nothing Then
            Dim i As Integer
            For i = 0 To stCard.lstSpouse.Count - 1

                If Not mtblCardInfo.Contains(stCard.lstSpouse(i)) Then
                    Dim stSpouse As stCardInfo
                    stSpouse = fncGetMemberInfo(stCard.lstSpouse(i), mtblUser)
                    stSpouse.intLevel = intGeneration
                    stSpouse.lstChild = Nothing
                    stSpouse.lstSpouse = Nothing
                    mtblCardInfo.Add(stSpouse.intID, stSpouse)
                End If

                Application.DoEvents()
            Next
        End If

        If Not stCard.lstChild Is Nothing Then
            Dim i As Integer
            For i = 0 To stCard.lstChild.Count - 1
                Application.DoEvents()
                If mintMaxDrawGen < intGeneration + 1 Then mintMaxDrawGen = intGeneration + 1
                xMakeInfoTree(stCard.lstChild(i), intGeneration + 1, stCard.lstChild)
                Application.DoEvents()
            Next
        End If

        Return True
    End Function

    Private Function xGetKidList(ByVal drMember As DataRow(), ByVal strFieldID As String) As List(Of Integer)
        xGetKidList = Nothing
        Try
            If drMember Is Nothing Then Return Nothing
            If drMember.Length <= 0 Then Return Nothing

            Dim i As Integer
            Dim intID As Integer
            Dim lstData As List(Of Integer)

            lstData = New List(Of Integer)

            For i = 0 To drMember.Length - 1
                Integer.TryParse(fncCnvNullToString(drMember(i).Item(strFieldID)), intID)
                If Not lstData.Contains(intID) Then
                    lstData.Add(intID)
                Else
                    If fncCnvNullToString(drMember(i).Item("MOTHER_RELID")) = CStr(clsEnum.emRelation.NATURAL) Then
                        lstData.Remove(intID)
                        lstData.Add(intID)
                    End If
                End If
            Next

            Return lstData
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : MaxHeight Property, max height of panel
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property MaxHeight() As Integer
        Get
            Dim stCard As stCardInfo
            Dim intMaxHeight As Integer = mintStartY
            For Each element As DictionaryEntry In mtblCardInfo

                stCard = CType(mtblCardInfo.Item(element.Key), stCardInfo)
                If intMaxHeight < stCard.intY Then
                    intMaxHeight = stCard.intY
                End If
            Next

            Return intMaxHeight + mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN
            'Return mintStartY + mintMaxGeneration * (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN) - mintMEM_CARD_SPACE_DOWN

        End Get
    End Property

    '   ****************************************************************** 
    '      FUNCTION   : MaxHeight Property, max height of panel
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property MaxWidth() As Integer
        Get

            Dim stCard As stCardInfo
            Dim intMaxRight As Integer = -1

            For Each element As DictionaryEntry In mtblCardInfo

                stCard = CType(mtblCardInfo.Item(element.Key), stCardInfo)
                If intMaxRight < stCard.intX Then
                    intMaxRight = stCard.intX
                End If
            Next

            Return intMaxRight + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT + mintStartX

        End Get
    End Property

    Public ReadOnly Property MaxWidthInMM() As Integer
        Get
            Dim objGraphic As Graphics = mpnDraw.CreateGraphics()
            Dim intDPI As Integer = CInt(Me.MaxWidth / objGraphic.DpiX * 25.4F)

            objGraphic = Nothing

            Return intDPI
        End Get
    End Property

    Public ReadOnly Property MaxHeightInMM() As Integer
        Get
            Dim objGraphic As Graphics = mpnDraw.CreateGraphics()
            Dim intDPI As Integer = CInt(Me.MaxHeight / objGraphic.DpiY * 25.4F)

            objGraphic = Nothing
            Return intDPI
        End Get
    End Property

    '   ****************************************************************** 
    '      FUNCTION   : DrawingCard Property, return list of drawing control 
    '      MEMO       :  
    '      CREATE     : 2012/11/13  AKB Manh 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property DrawingCard() As Hashtable
        Get

            Return mtblCardCtrl
            'Return gtblMemberCard
        End Get
    End Property

    '   ****************************************************************** 
    '      FUNCTION   : NormalLine Property, 
    '      MEMO       :  
    '      CREATE     : 2012/11/13  AKB Manh 
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
    '      CREATE     : 2012/11/13  AKB Manh 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property SpecialLine() As List(Of usrLine)
        Get
            Return mlstSpecialLine
        End Get
    End Property

    '   ****************************************************************** 
    '      FUNCTION   : DrawList Property, return list of mem by level
    '      MEMO       :  
    '      CREATE     : 2012/11/13  AKB Manh 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property DrawList() As Hashtable
        Get

            Return mtblCardInfo

        End Get
    End Property

    '   ****************************************************************** 
    '      FUNCTION   : RootID
    '      MEMO       :  
    '      CREATE     : 2012/12/17  AKB Manh 
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property RootID() As Integer
        Get

            Return mintRootID

        End Get
    End Property

    Public ReadOnly Property RootMemberInfo() As String
        Get

            Dim stCard As stCardInfo
            stCard = CType(mtblCardInfo.Item(mintRootID), stCardInfo)
            Return fncGetRootMemberInfoDisplay(stCard)

        End Get
    End Property



#Region " IDisposable Support "
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            ' TODO: free unmanaged resources when explicitly called
        End If

    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
