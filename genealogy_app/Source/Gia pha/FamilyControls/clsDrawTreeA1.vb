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
Public Class clsDrawTreeA1
    Implements IDisposable

#Region "Constants"
    Private Const mcstrClsName As String = "clsDrawTreeNew1"                                   'class name
    Private Const mcstrFindRelFormat As String = "REL_FMEMBER_ID = {0} AND RELID = {1}"     'format to find member with relationship
    Private Const mcstrFindUsrFormat As String = "MEMBER_ID = {0}"                          'format to find member

#End Region

    Private mintRootID As Integer                       'root id to draw
    Private mintMaxGeneration As Integer                'Max Generatoion from RootID
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

    ' ▽2018/04/27 AKB Nguyen Thanh Tung --------------------------------
    Private mobjFont As Font

    Public ReadOnly Property FontUser As Font
        Get
            Return mobjFont
        End Get
    End Property
    ' △2018/04/27 AKB Nguyen Thanh Tung --------------------------------

    Public Event evnCardClicked(ByVal intMemID As Integer)
    Public Event evnCardDoubleClicked(ByVal intMemID As Integer)
    Public Event evnRefresh(ByVal intMemID As Integer, ByVal blnRedraw As Boolean)

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
            'xDisposeCard(mtblCardCtrl)
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

    Private Sub xAddHandler(ByRef objCard As usrMemberCard1)
        AddHandler objCard.evnCardDoubleClick, AddressOf xShowPerInfo
        AddHandler objCard.evnCardClick, AddressOf xCardClicked
        'AddHandler objCard.evnNotDraw, AddressOf xHandleNotDraw
        AddHandler objCard.evnCardLocationChange, AddressOf xCardMove
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2Child, Draw To 2 Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************    
    Private Sub xDrawConnector2Child(ByVal intParentID As Integer, _
                                     ByVal intChildID As Integer, _
                                     Optional ByVal blnStepChild As Boolean = False, _
                                     Optional ByVal intPos As Integer = 1)
        Dim objParent As usrMemberCard1
        Dim stParent As stCardInfo

        Dim objChild As usrMemberCard1
        Dim stChild As stCardInfo

        stParent = xGetMemberCard(intParentID)
        objParent = CType(mtblCardCtrl.Item(intParentID), usrMemberCard1)

        stChild = xGetMemberCard(intChildID)
        objChild = CType(mtblCardCtrl.Item(intChildID), usrMemberCard1)

        If blnStepChild Then

            If objChild Is Nothing Then
                return
            Else
                xDrawDiffLv(objParent, objChild)
            End If

        Else

            'This CODE to draw child line at the center of Parent
            'Dim objFather As usrMemberCard1 = CType(mtblCardCtrl.Item(stChild.intFatherID), usrMemberCard1)
            'Dim objMother As usrMemberCard1 = CType(mtblCardCtrl.Item(stChild.intMotherID), usrMemberCard1)
            'Dim intDelta As Integer = CInt((Math.Abs(objFather.Left - objMother.Left) - mintMEM_CARD_W) / 2)
            'xDrawDiffLv2(objParent, objChild, intPos, intPos * intDelta)
            xDrawDiffLv2(objParent, objChild, intPos)

        End If

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnectorListChildOfCouple, Draw To 2 List Child of a couple
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawConnectorListChildOfCouple(ByVal intParentLeftID As Integer, _
                                                ByVal intParentRightID As Integer)

        Dim stParentLeft As stCardInfo
        Dim stParentRight As stCardInfo
        Dim stChild As stCardInfo

        Dim objParentLeft As usrMemberCard1
        Dim objChild As usrMemberCard1

        Dim lstChild As List(Of Integer)
        Dim intDeltaX As Integer
        Dim i As Integer
        stParentLeft = xGetMemberCard(intParentLeftID)
        stParentRight = xGetMemberCard(intParentRightID)

        intDeltaX = CInt((stParentRight.intX - stParentLeft.intX - mintMEM_CARD_W) / 2)

        lstChild = stParentLeft.lstChild


        If lstChild Is Nothing Then Return

        ' Edit by: 2019.08.23 AKB Nguyen Thanh Tung
        Dim blnFirstChild As Boolean = True

        For i = 0 To lstChild.Count - 1
            objParentLeft = CType(mtblCardCtrl.Item(intParentLeftID), usrMemberCard1)
            stChild = xGetMemberCard(lstChild(i))
            objChild = CType(mtblCardCtrl.Item(lstChild(i)), usrMemberCard1)
            xDrawDiffLv2(objParentLeft, objChild, 1, intDeltaX, blnFirstChild)
            blnFirstChild = False
            Application.DoEvents()
        Next
        'For i = 0 To lstChild.Count - 1

        '    objParentLeft = CType(mtblCardCtrl.Item(intParentLeftID), usrMemberCard1)
        '    stChild = xGetMemberCard(lstChild(i))
        '    objChild = CType(mtblCardCtrl.Item(lstChild(i)), usrMemberCard1)
        '    xDrawDiffLv2(objParentLeft, objChild, 1, intDeltaX)

        'Next
        ' Edit by: 2019.08.23 AKB Nguyen Thanh Tung
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2ListChild, Draw To 2 List of Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawConnector2ListChild(ByVal intParentID As Integer, _
                                         ByVal blnStepChild As Boolean, _
                                         Optional ByVal intPos As Integer = 1)

        Dim stParent As stCardInfo
        Dim lstChild As List(Of Integer)
        Dim i As Integer
        stParent = xGetMemberCard(intParentID)

        If blnStepChild Then

            lstChild = stParent.lstStepChild

        Else

            lstChild = stParent.lstChild

        End If

        If lstChild Is Nothing Then Return

        For i = 0 To lstChild.Count - 1
            xDrawConnector2Child(intParentID, lstChild(i), blnStepChild, intPos)
        Next

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2Spouse, Draw To 2 Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************    
    Private Sub xDrawConnector2Spouse(ByVal intLeftSpouseID As Integer, _
                                      ByVal intRightSpouse As Integer)


        Dim objLeft As usrMemberCard1
        Dim objRight As usrMemberCard1

        objLeft = CType(mtblCardCtrl.Item(intLeftSpouseID), usrMemberCard1)
        objRight = CType(mtblCardCtrl.Item(intRightSpouse), usrMemberCard1)
        xDrawSameLv(objLeft, objRight)

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2ListSpouse, Draw To 2 Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************    
    Private Sub xDrawConnector2ListSpouse(ByVal intRootID As Integer)


        Dim stRootCard As stCardInfo
        stRootCard = xGetMemberCard(CInt(intRootID))
        Dim lstSpouse As List(Of Integer)
        Dim i As Integer
        Dim intLeftSpouse As Integer

        lstSpouse = stRootCard.lstSpouse

        If lstSpouse Is Nothing Then Return

        If lstSpouse.Count = 1 Then
            xDrawConnector2Spouse(intRootID, lstSpouse(0))
            Return
        End If

        xDrawConnector2Spouse(lstSpouse(0), intRootID)

        intLeftSpouse = intRootID
        For i = 1 To lstSpouse.Count - 1
            xDrawConnector2Spouse(intLeftSpouse, lstSpouse(i))
            intLeftSpouse = lstSpouse(i)
        Next


    End Sub

    Private Sub xDrawLineChild(ByVal intParentID As Integer, ByVal blnStepChild As Boolean)

        Dim stParent As stCardInfo
        stParent = xGetMemberCard(CInt(intParentID))
        Dim lstChildID As List(Of Integer)
        Dim i As Integer

        If blnStepChild Then

            lstChildID = stParent.lstStepChild

        Else

            lstChildID = stParent.lstChild

        End If

        If lstChildID Is Nothing Then Return
        For i = 0 To lstChildID.Count - 1
            xDrawLine(lstChildID(i))
        Next

    End Sub

    Private Sub xDrawLine(ByVal intRootID As Integer, _
                          Optional ByVal blnDrawSpouse As Boolean = True, _
                          Optional ByVal intPos As Integer = 1)

        Dim objRootCard As usrMemberCard1
        Dim stRootCard As stCardInfo
        Dim i As Integer

        stRootCard = xGetMemberCard(CInt(intRootID))
        objRootCard = CType(mtblCardCtrl.Item(intRootID), usrMemberCard1)

        If Not stRootCard.lstSpouse Is Nothing And blnDrawSpouse Then

            If stRootCard.lstSpouse.Count = 1 Then
                'Connect to StepChild
                xDrawConnector2ListChild(intRootID, True, 1)


                'Connect to Child
                xDrawConnectorListChildOfCouple(intRootID, stRootCard.lstSpouse(0))
                'xDrawConnector2ListChild(stRootCard.lstSpouse(0), False, -1)

                'Connect to Spouse
                xDrawConnector2Spouse(intRootID, stRootCard.lstSpouse(0))

                'Call DrawLine for Child
                'Step child
                xDrawLineChild(intRootID, True)
                'Child
                xDrawLineChild(intRootID, False)


                xDrawLineChild(stRootCard.lstSpouse(0), True)
                xDrawConnector2ListChild(stRootCard.lstSpouse(0), True)

            Else

                xDrawLine(stRootCard.lstSpouse(0), False)
                xDrawConnector2ListChild(intRootID, True, 1)

                For i = 1 To stRootCard.lstSpouse.Count - 1
                    xDrawLine(stRootCard.lstSpouse(i), False, -1)
                Next

                xDrawLineChild(intRootID, True)
                xDrawConnector2ListSpouse(intRootID)

            End If

        Else

            If blnDrawSpouse Then
                xDrawConnector2ListChild(intRootID, True, 1)
                xDrawLineChild(intRootID, True)

            Else
                If intPos > 0 Then
                    xDrawConnector2ListChild(intRootID, True)
                    xDrawConnector2ListChild(intRootID, False, 1)

                    xDrawLineChild(intRootID, True)
                    xDrawLineChild(intRootID, False)
                Else
                    xDrawConnector2ListChild(intRootID, False, -1)
                    xDrawConnector2ListChild(intRootID, True)

                    xDrawLineChild(intRootID, False)
                    xDrawLineChild(intRootID, True)

                End If

            End If

        End If



    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xAddCtrl2Panel, add controls to panel
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    'Private Function xAddCtrl2Panel() As Boolean

    '    xAddCtrl2Panel = False

    '    Try
    '        Dim objCard As usrMemberCard1
    '        Dim stCard As stCardInfo

    '        For Each element As DictionaryEntry In mtblCardInfo

    '            If Not mtblCardCtrl.Contains(element.Key) Then
    '                stCard = xGetMemberCard(CInt(element.Key))
    '                objCard = fncMakeCardInfoType1(stCard, mblnIsSmallCard)

    '                objCard.Location = New Point(stCard.intX, stCard.intY)
    '                objCard.DrawLv = stCard.intLevel
    '                'add handler
    '                xAddHandler(objCard)

    '                mtblCardCtrl.Add(stCard.intID, objCard)

    '                mpnDraw.Controls.Add(objCard)

    '            End If
    '            Application.DoEvents()

    '        Next

    '        xDrawLine(mintRootID)
    '        mpnDraw.Visible = True
    '        xSetScrollView(mintRootID)
    '        Return True

    '    Catch ex As Exception

    '        basCommon.fncSaveErr(mcstrClsName, "xAddCtrl2Panel", ex)
    '    End Try

    'End Function

    Private Function xAddCtrl2Panel() As Boolean

        xAddCtrl2Panel = False

        mpnDraw.SuspendLayout()
        mpnDraw.ResumeLayout(False)

        Try
            Dim objCard As usrMemberCard1
            Dim stCard As stCardInfo

            For Each element As DictionaryEntry In mtblCardInfo

                If Not mtblCardCtrl.Contains(element.Key) Then
                    stCard = xGetMemberCard(CInt(element.Key))

                    ' ▽ 2018/04/24 AKB Nguyen Thanh Tung --------------------------------
                    If My.Settings.blnTypeCardShort Then

                        objCard = fncMakeCardInfoType2(stCard, mblnIsSmallCard)
                        objCard.Height = mintMEM_CARD_H
                        objCard.Width = mintMEM_CARD_W
                        'objCard.lblName.Font = mobjFont

                        If objCard.lblName.AutoSize Then

                            objCard.lblName.Left = CInt((objCard.ClientSize.Width - objCard.lblName.Width) / 2)
                            objCard.lblName.Top = CInt((objCard.ClientSize.Height - objCard.lblName.Height) / 2)

                        End If
                    Else

                        objCard = fncMakeCardInfoType1(stCard, mblnIsSmallCard)

                    End If

                    objCard.lblName.Font = mobjFont
                    objCard.AliveStatus = Not (stCard.stBasicInfo.intDecease = basConst.gcintDIED)

                    'objCard = fncMakeCardInfoType1(stCard, mblnIsSmallCard)
                    ' △ 2018/04/24 AKB Nguyen Thanh Tung --------------------------------

                    objCard.Location = New Point(stCard.intX, stCard.intY)
                    objCard.CardCoor = New clsCoordinate(stCard.intX, stCard.intY)
                    objCard.DrawLv = stCard.intLevel
                    'add handler
                    xAddHandler(objCard)

                    mtblCardCtrl.Add(stCard.intID, objCard)

                    mpnDraw.Controls.Add(objCard)

                End If
                Application.DoEvents()

            Next

            xDrawLine(mintRootID)
            mpnDraw.Visible = True
            xSetScrollView(mintRootID)
            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddCtrl2Panel", ex)
        Finally
            mpnDraw.ResumeLayout(True)
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
    '　　　FUNCTION   : xDrawDiffLv, draw different level
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawDiffLv2(ByVal objUpperCard As usrMemberCard1,
                             ByVal objLowerCard As usrMemberCard1,
                             Optional ByVal intMinus As Integer = 1,
                             Optional ByVal intDeltaX As Integer = 0,
                             Optional ByVal addTopLine As Boolean = True)

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
            objVerLine1 = New usrLine(clsEnum.emLineType.SINGLE_LINE, clsEnum.emLineDirection.VERTICAL, CInt(mintMEM_CARD_SPACE_DOWN / 2 + mintMEM_CARD_H / 2))
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
                addTopLine = True 'Add by: 2019.08.23 AKB Nguyen Thanh Tung
            End If

            If intDeltaX = 0 Then
                'draw line
                If intMinus > 0 Then

                    objVerLine1.fncAddVerticalLine(objUpperCard, clsEnum.emCardPoint.MID_RIGHT, CInt(mintMEM_CARD_SPACE_LEFT / 2))

                Else

                    objVerLine1.fncAddVerticalLine(objUpperCard, clsEnum.emCardPoint.MID_LEFT, -1 * CInt(mintMEM_CARD_SPACE_LEFT / 2))

                End If

            Else

                objVerLine1.fncAddVerticalLine(objUpperCard, clsEnum.emCardPoint.MID_RIGHT, intDeltaX)

            End If


            objVerLine2.fncAddVerticalLine(objLowerCard, clsEnum.emCardPoint.MID_TOP)
            objHorzLine.fncAddHorizontalLine(objVerLine1, objVerLine2)

            'add to panel
            ' Edit by: 2019.08.23 AKB Nguyen Thanh Tung
            If addTopLine Then mpnDraw.Controls.Add(objVerLine1)
            'mpnDraw.Controls.Add(objVerLine1)
            ' Edit by: 2019.08.23 AKB Nguyen Thanh Tung
            mpnDraw.Controls.Add(objVerLine2)
            mpnDraw.Controls.Add(objHorzLine)

            'bring connector to front
            If blnIsFHead Then

                objVerLine1.BringToFront()
                objVerLine2.BringToFront()
                objHorzLine.BringToFront()

                'add to list
                ' Edit by: 2019.08.23 AKB Nguyen Thanh Tung
                If addTopLine Then mlstSpecialLine.Add(objVerLine1)
                'mlstSpecialLine.Add(objVerLine1)
                ' Edit by: 2019.08.23 AKB Nguyen Thanh Tung
                mlstSpecialLine.Add(objVerLine2)
                mlstSpecialLine.Add(objHorzLine)

            Else
                'add to list
                ' Edit by: 2019.08.23 AKB Nguyen Thanh Tung
                If addTopLine Then mlstNormalLine.Add(objVerLine1)
                'mlstNormalLine.Add(objVerLine1)
                ' Edit by: 2019.08.23 AKB Nguyen Thanh Tung
                mlstNormalLine.Add(objVerLine2)
                mlstNormalLine.Add(objHorzLine)

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawDiffLv2", ex)
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
            Dim objHorzLine1 As usrLine
            Dim objHorzLine2 As usrLine

            objHorzLine1 = New usrLine
            objHorzLine2 = New usrLine

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
            mtblRelChild = gobjDB.fncGetChildFull()
            'mtblRelChild = gobjDB.fncGetChild()

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

            xMakeInfoTree(mintRootID, 1)
            Dim intX As Integer = intInitX

            mintStartX = intInitX
            mintStartY = intInitY

            ' ▽2018/04/24 AKB Nguyen Thanh Tung --------------------------------
            mobjFont = My.Settings.objFontDefaut
            If My.Settings.blnTypeCardShort Then basCommon.fncModeShortCard(mtblCardInfo, mblnIsSmallCard, mintMEM_CARD_W, mintMEM_CARD_H, mobjFont)
            ' △2018/04/24 AKB Nguyen Thanh Tung --------------------------------

            xCaculateCoordinateTree(mintRootID, mintStartX, mintStartY)

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
    Private Function xCaculateCoordinateTree(ByVal intRootID As Integer, ByVal intX As Integer, ByVal intY As Integer, _
                                                   Optional ByVal intWife As Integer = -1) As stCardInfo

        Dim stRootCard As stCardInfo = xGetMemberCard(intRootID)
        Dim intSX As Integer
        stRootCard.intX = intX
        stRootCard.intY = intY
        stRootCard.intMaxRight = intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT

        'IF there is no Spouse
        If stRootCard.lstSpouse Is Nothing Then

            stRootCard.intMaxRight = xCaculateCoordinatorListID(stRootCard.lstStepChild, intX, intY + (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN))

            If stRootCard.intMaxRight = intX Then
                stRootCard.intMaxRight = intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
            End If

            xAlignSingleParent(stRootCard, stRootCard.lstStepChild)

            mtblCardInfo.Item(intRootID) = stRootCard
            Return stRootCard

        End If

        'IF there is no Child
        If stRootCard.lstChild Is Nothing And stRootCard.lstStepChild Is Nothing Then

            If intWife >= 0 Then
                mtblCardInfo.Item(intRootID) = stRootCard
                Return stRootCard
            End If

            Dim posLeftRight As Point

            stRootCard.intMinLeft = stRootCard.intX
            posLeftRight = xMakeSpouseNoChild(stRootCard)

            stRootCard.intMaxRight = posLeftRight.Y
            stRootCard.intMinLeft = posLeftRight.X

            If stRootCard.intMaxRight < 0 Then

                stRootCard.intMaxRight = stRootCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
                stRootCard.intMinLeft = stRootCard.intX

            End If

            mtblCardInfo.Item(intRootID) = stRootCard

            'Return stRootCard
        End If
        'xCaculateCoordinateMember(stRootCard)

        'If there is Only One Spouse
        If stRootCard.lstSpouse.Count = 1 Then
            Dim stSpouseLeft As stCardInfo = xGetMemberCard(stRootCard.lstSpouse(0))
            If intWife >= 1 Then

                If intWife = 1 Then
                    intSX = xCaculateCoordinatorListID(stRootCard.lstStepChild, intX, intY + mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN)
                    intSX = xCaculateCoordinatorListID(stRootCard.lstChild, intSX, intY + mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN)
                Else

                    If intWife = 2 Then
                        If stSpouseLeft.lstStepChild Is Nothing Then
                            intX = intX - mintMEM_CARD_W - mintMEM_CARD_SPACE_LEFT
                        End If
                    End If

                    intSX = xCaculateCoordinatorListID(stRootCard.lstChild, intX, intY + mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN)
                    intSX = xCaculateCoordinatorListID(stRootCard.lstStepChild, intSX, intY + mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN)
                End If

                xAlignSingleParent(stRootCard)

                If intWife = 2 Then
                    If stRootCard.intX >= intX And stRootCard.intX <= intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT Then
                        stRootCard.intX = intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT

                        If stRootCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT > stRootCard.intMaxRight Then
                            stRootCard.intMaxRight = stRootCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
                        End If
                    End If
                End If

                mtblCardInfo.Item(intRootID) = stRootCard
                Return stRootCard
            End If

            'Get Spouse Right of the Root People
            Dim stSpouseRight As stCardInfo
            stSpouseRight = xGetMemberCard(stRootCard.lstSpouse(0))
            stSpouseRight.intX = stRootCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
            stSpouseRight.intY = stRootCard.intY
            stSpouseRight.intMinLeft = stRootCard.intMinLeft
            stSpouseRight.intMaxRight = stSpouseRight.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
            mtblCardInfo.Item(stRootCard.lstSpouse(0)) = stSpouseRight
            stRootCard.intMaxRight = stSpouseRight.intMaxRight

            'intSX = xCaculateCoordinatorListID(stRootCard.lstStepChild, intX, intY + (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN))
            'intSX = xCaculateCoordinatorListID(stRootCard.lstChild, intSX, intY + (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN))
            'intSX = xCaculateCoordinatorListID(stSpouseRight.lstStepChild, intSX, intY + (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN))

            'if this Spouse there is no StepChild
            'If stSpouseRight.lstStepChild Is Nothing Then
            xCaculateCoordinatorCouple(stRootCard, stSpouseRight, intX, intY)
            stRootCard.intMaxRight = stSpouseRight.intMaxRight
            mtblCardInfo.Item(intRootID) = stRootCard
            Return stRootCard
            'End If



            'stSpouseRight = xCaculateCoordinateTree(stSpouseRight.intID, intSX, intY, 1)
            'mtblCardInfo.Item(stSpouseRight.intID) = stSpouseRight
            'stRootCard.intMaxRight = stSpouseRight.intMaxRight
            'mtblCardInfo.Item(intRootID) = stRootCard
            'Return stRootCard
        End If

        'If there is more than one Spouse
        Dim stSpouse As stCardInfo = xGetMemberCard(stRootCard.lstSpouse(0))

        stSpouse = xCaculateCoordinateTree(stSpouse.intID, intX, intY, 1)
        stRootCard.intX = stSpouse.intMaxRight
        stRootCard.intMaxRight = stRootCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT

        intSX = xCaculateCoordinatorListID(stRootCard.lstStepChild, stSpouse.intMaxRight, intY + mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN)
        'intSX = xCaculateCoordinatorListID(stRootCard.lstChild, intSX, intY + mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN)
        xAlignSingleParent(stRootCard, stRootCard.lstStepChild)

        Dim i As Integer

        For i = 1 To stRootCard.lstSpouse.Count - 1

            stSpouse = xGetMemberCard(stRootCard.lstSpouse(i))

            If i = 1 Then

                intSX = stRootCard.intMaxRight

            Else

                intSX = xGetMemberCard(stRootCard.lstSpouse(i - 1)).intMaxRight

            End If

            stSpouse = xCaculateCoordinateTree(stSpouse.intID, intSX, intY, i + 1)

            mtblCardInfo.Item(stSpouse.intID) = stSpouse

            stRootCard.intMaxRight = stSpouse.intMaxRight

        Next

        mtblCardInfo.Item(intRootID) = stRootCard

        Return stRootCard

    End Function

    Private Function xCaculateCoordinatorCouple(ByRef stRootCard As stCardInfo, _
                                                ByRef stSpouseRight As stCardInfo, _
                                                ByVal intX As Integer, ByVal intY As Integer) As Boolean
        Dim intStartX As Integer
        'Caculate Step Child
        intStartX = xCaculateCoordinatorListID(stRootCard.lstStepChild, intX, intY + (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN))

        'Caculate Child of this couple
        intStartX = xCaculateCoordinatorListID(stRootCard.lstChild, intStartX, intY + (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN))

        'Caculate Step Child of this soupse
        xCaculateCoordinatorListID(stSpouseRight.lstStepChild, intStartX, intY + (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN))

        Dim blnLeft As Boolean = xAlignSingleParent(stRootCard, stRootCard.lstStepChild)
        Dim blnRight As Boolean = xAlignSingleParent(stSpouseRight, stSpouseRight.lstStepChild)

        If Not blnLeft And Not blnRight Then
            xAlignCoupleParent(stRootCard, stSpouseRight, stRootCard.lstChild)
            mtblCardInfo.Item(stSpouseRight.intID) = stSpouseRight
            Return True
        End If

        If blnLeft And Not blnRight Then
            If Not xAlignSingleParent(stSpouseRight, stRootCard.lstChild) Then
                stSpouseRight.intX = stRootCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
            End If
            mtblCardInfo.Item(stSpouseRight.intID) = stSpouseRight
            Return True
        End If

        If Not blnLeft And blnRight Then
            If Not xAlignSingleParent(stRootCard, stRootCard.lstChild) Then
                Dim intOldStartX As Integer = stRootCard.intX
                stRootCard.intX = stSpouseRight.intX - mintMEM_CARD_W - mintMEM_CARD_SPACE_LEFT

                If stRootCard.intX < intOldStartX Then
                    stRootCard.intX = intOldStartX
                    stSpouseRight.intX = stRootCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
                    xCaculateCoordinatorListID(stSpouseRight.lstStepChild, stSpouseRight.intX, intY + (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN))
                    mtblCardInfo.Item(stSpouseRight.intID) = stSpouseRight
                End If

            End If
            Return True
        End If

    End Function


    Private Function xCaculateCoordinatorListID(ByVal lstID As List(Of Integer), ByVal intX As Integer, ByVal intY As Integer) As Integer

        If lstID Is Nothing Then Return intX
        Dim i As Integer
        Dim stCard As stCardInfo
        Dim intSX As Integer = intX

        For i = 0 To lstID.Count - 1

            stCard = xGetMemberCard(lstID(i))
            stCard = xCaculateCoordinateTree(lstID(i), intSX, intY)
            intSX = stCard.intMaxRight

        Next

        Return stCard.intMaxRight

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : Align a single parent center a List of child
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAlignSingleParent(ByRef stCardParent As stCardInfo, _
                                        ByVal lstID As List(Of Integer), _
                                        Optional ByVal intWidth As Integer = 0) As Boolean
        xAlignSingleParent = False
        If lstID Is Nothing Then Return False

        Dim stFirstChild As stCardInfo
        Dim stLastChild As stCardInfo
        stLastChild = CType(mtblCardInfo.Item(lstID(lstID.Count - 1)), stCardInfo)
        stFirstChild = CType(mtblCardInfo.Item(lstID(0)), stCardInfo)

        stCardParent.intX = stFirstChild.intX
        stCardParent.intMinLeft = stFirstChild.intMinLeft

        'If there is more than one child, need to recalculate the X and Max Right
        If lstID.Count > 1 Then

            'stCard.intX = stFirstChild.intMinLeft + CInt((stLastChild.intMaxRight - stCard.intMaxRight) / 2)
            If intWidth = 0 Then intWidth = mintMEM_CARD_W
            'stCardParent.intX = stFirstChild.intX + CInt((stLastChild.intMaxRight - stFirstChild.intX - intWidth - mintMEM_CARD_SPACE_LEFT) / 2)
            stCardParent.intX = stFirstChild.intX + CInt((stLastChild.intX + mintMEM_CARD_W - stFirstChild.intX - intWidth) / 2)
            If stCardParent.intX < 0 Then
                stCardParent.intX = stFirstChild.intX
            End If

            'If stCard.intX <= stFirstChild.intX Then
            'stCard.intX = stFirstChild.intX '+ CInt((stLastChild.intMaxRight - stCard.intMaxRight) / 2)
            'End If
        End If

        'Upadte MaxRight of this card
        stCardParent.intMaxRight = stLastChild.intMaxRight
        mtblCardInfo.Item(stCardParent.intID) = stCardParent
        xAlignSingleParent = True

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : Align a single parent center of her/his child
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAlignSingleParent(ByRef stCardParent As stCardInfo) As Boolean

        xAlignSingleParent = False
        If stCardParent.lstChild Is Nothing And stCardParent.lstStepChild Is Nothing Then Return True
        If stCardParent.lstChild Is Nothing Then Return xAlignSingleParent(stCardParent, stCardParent.lstStepChild)
        If stCardParent.lstStepChild Is Nothing Then Return xAlignSingleParent(stCardParent, stCardParent.lstChild)
        Dim intMinX As Integer = Integer.MaxValue
        Dim intMaxX As Integer = -1
        Dim stCard As stCardInfo
        Dim i As Integer

        'Find Min
        For i = 0 To stCardParent.lstChild.Count - 1
            stCard = xGetMemberCard(stCardParent.lstChild(i))
            If intMinX > stCard.intMinLeft Then
                intMinX = stCard.intMinLeft
            End If
        Next

        For i = 0 To stCardParent.lstStepChild.Count - 1
            stCard = xGetMemberCard(stCardParent.lstStepChild(i))
            If intMinX > stCard.intMinLeft Then
                intMinX = stCard.intMinLeft
            End If
        Next

        'Find Max
        For i = 0 To stCardParent.lstChild.Count - 1
            stCard = xGetMemberCard(stCardParent.lstChild(i))
            If intMaxX < stCard.intMaxRight Then
                intMaxX = stCard.intMaxRight
            End If
        Next

        For i = 0 To stCardParent.lstStepChild.Count - 1
            stCard = xGetMemberCard(stCardParent.lstStepChild(i))
            If intMaxX < stCard.intMaxRight Then
                intMaxX = stCard.intMaxRight
            End If
        Next

        stCardParent.intX = intMinX + CInt((intMaxX - intMinX - mintMEM_CARD_W) / 2)
        xAlignSingleParent = True

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : Align a couple between list of child
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAlignCoupleParent(ByRef stFather As stCardInfo, ByRef stMother As stCardInfo, ByVal lstID As List(Of Integer)) As Boolean
        xAlignCoupleParent = False
        If lstID Is Nothing Then Return False

        'If this couple has only one child
        If lstID.Count = 1 Then

            'Get Child
            Dim stChild As stCardInfo = xGetMemberCard(lstID(0))

            'if child there is no spouse
            If stChild.lstSpouse Is Nothing Then
                If stChild.intX < mintMEM_CARD_W Then
                    stChild.intX = mintStartX + mintMEM_CARD_W + CInt(mintMEM_CARD_SPACE_LEFT / 2)
                End If

                stFather.intX = stChild.intX
                stMother.intX = stFather.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
                stMother.intMaxRight = stMother.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
                stFather.intMaxRight = stMother.intMaxRight

                stChild.intX = stFather.intX + CInt((stFather.intMaxRight - mintMEM_CARD_SPACE_LEFT - stFather.intX - mintMEM_CARD_W) / 2)
                stMother.intY = stFather.intY
            Else

                stFather.intX = stChild.intX
                stMother.intX = stFather.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
                stFather.intMaxRight = stChild.intMaxRight
                stMother.intMaxRight = stChild.intMaxRight

            End If


            mtblCardInfo.Item(stChild.intID) = stChild
            mtblCardInfo.Item(stFather.intID) = stFather
            mtblCardInfo.Item(stMother.intID) = stMother

        Else

            xAlignSingleParent(stFather, lstID, mintMEM_CARD_SPACE_LEFT + 2 * mintMEM_CARD_W)

            'stFather.intX = CInt(stFather.intX * (mintMEM_CARD_SPACE_LEFT + 2 * mintMEM_CARD_W) / mintMEM_CARD_W)

            stMother.intX = stFather.intX + mintMEM_CARD_SPACE_LEFT + mintMEM_CARD_W
            stMother.intMinLeft = stFather.intMinLeft
            stMother.intMaxRight = stFather.intMaxRight
            stMother.intY = stFather.intY

            mtblCardInfo.Item(stFather.intID) = stFather
            mtblCardInfo.Item(stMother.intID) = stMother

        End If
        xAlignCoupleParent = True

    End Function

    Private Function xGetMemberCard(ByVal intID As Integer) As stCardInfo
        Return CType(mtblCardInfo.Item(intID), stCardInfo)
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : Caculate Coordinate of a Member
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCaculateCoordinateMember(ByRef stCard As stCardInfo) As Boolean
        xCaculateCoordinateMember = False
        Dim posLeftRight As Point
        Try
            If stCard.lstChild Is Nothing And stCard.lstStepChild Is Nothing Then
                stCard.intMinLeft = stCard.intX
                posLeftRight = xMakeSpouseNoChild(stCard)

                stCard.intMaxRight = posLeftRight.Y  'stCard.intX + (i + 1) * (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
                stCard.intMinLeft = posLeftRight.X

                If stCard.intMaxRight < 0 Then

                    stCard.intMaxRight = stCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT
                    stCard.intMinLeft = stCard.intX

                End If

                Return True
            End If

            If stCard.lstSpouse Is Nothing Then
                If Not stCard.lstStepChild Is Nothing Then

                End If

            End If

            Return True
        Catch ex As Exception

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xMakeSpouse, Caculate the spouse coordinates
    '      VALUE      : Integer (maxwidth of All Spouse)
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xMakeSpouseNoChild(ByRef stCard As stCardInfo) As Point
        Dim i As Integer
        Dim stSpouse As stCardInfo
        Dim intMinLeft As Integer
        Dim intMaxRight As Integer
        xMakeSpouseNoChild = New Point(-999, -999)

        If stCard.lstSpouse Is Nothing Then Exit Function

        If stCard.lstSpouse.Count > 1 Then

            i = 0
            stSpouse = xGetMemberCard(stCard.lstSpouse(i))

            stSpouse.intY = stCard.intY
            stSpouse.intX = stCard.intX

            stCard.intX = stCard.intX + (i + 1) * (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)

            'If stSpouse.intX < 0 Then
            '    stCard.intX = stCard.intX + (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
            '    stSpouse.intX = stCard.intX - (i + 1) * (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
            'End If

            stSpouse.intMaxRight = stCard.intX + (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
            stSpouse.intMinLeft = stSpouse.intX
            stCard.intMinLeft = stSpouse.intX

            intMaxRight = stSpouse.intMaxRight
            intMinLeft = stSpouse.intX

            mtblCardInfo.Item(stCard.lstSpouse(i)) = stSpouse

            For i = 1 To stCard.lstSpouse.Count - 1

                stSpouse = xGetMemberCard(stCard.lstSpouse(i))
                stSpouse.intY = stCard.intY
                stSpouse.intX = stCard.intX + (i) * (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
                stSpouse.intMaxRight = stSpouse.intX + (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
                stSpouse.intMinLeft = stSpouse.intX

                intMaxRight = stSpouse.intMaxRight
                mtblCardInfo.Item(stCard.lstSpouse(i)) = stSpouse

            Next

        Else

            i = 0
            stSpouse = xGetMemberCard(stCard.lstSpouse(i))
            stSpouse.intY = stCard.intY
            stSpouse.intX = stCard.intX + (i + 1) * (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
            stSpouse.intMaxRight = stSpouse.intX + (mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT)
            stSpouse.intMinLeft = stSpouse.intX
            intMinLeft = stCard.intX
            intMaxRight = stSpouse.intMaxRight

            mtblCardInfo.Item(stCard.lstSpouse(i)) = stSpouse

        End If

        xMakeSpouseNoChild = New Point(intMinLeft, intMaxRight)

    End Function

    Private Function xCaculateMemberNoSpouseHasChild(ByRef stCard As stCardInfo) As Point

        xCaculateMemberNoSpouseHasChild = New Point(-999, -999)
        Dim stFirstChild As stCardInfo
        Dim stLastChild As stCardInfo
        stLastChild = CType(mtblCardInfo.Item(stCard.lstChild(stCard.lstChild.Count - 1)), stCardInfo)
        stFirstChild = CType(mtblCardInfo.Item(stCard.lstChild(0)), stCardInfo)

        stCard.intX = stFirstChild.intX
        stCard.intY = mintStartY + (stCard.intLevel - 1) * (mintMEM_CARD_H + mintMEM_CARD_SPACE_DOWN)
        stCard.intMinLeft = stFirstChild.intMinLeft
        stCard.intMaxRight = stCard.intX + mintMEM_CARD_W + mintMEM_CARD_SPACE_LEFT


        'If there is more than one child, need to recalculate the X and Max Right
        If stCard.lstChild.Count > 1 Then

            'stCard.intX = stFirstChild.intMinLeft + CInt((stLastChild.intMaxRight - stCard.intMaxRight) / 2)
            stCard.intX = stFirstChild.intX + CInt((stLastChild.intMaxRight - stCard.intMaxRight) / 2)
            If stCard.intX < 0 Then
                stCard.intX = stFirstChild.intX
            End If


        End If

        'Upadte MaxRight of this card
        If stCard.intMaxRight <= stLastChild.intMaxRight Then
            stCard.intMaxRight = stLastChild.intMaxRight
        End If

        Return New Point(stCard.intMinLeft, stCard.intMaxRight)

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

    Private Sub xMakeInfoTreeOfList(ByVal lstID As List(Of Integer), ByVal intGeneration As Integer)

        If lstID Is Nothing Then Return

        Dim i As Integer

        For i = 0 To lstID.Count - 1
            Application.DoEvents()
            xMakeInfoTree(lstID(i), intGeneration)
            Application.DoEvents()
        Next

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xMakeInfoTree, Make info family tree
    '      VALUE      : Boolean : true: OK
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xMakeInfoTree(ByVal intRootID As Integer, _
                                   ByVal intGeneration As Integer) As Boolean

        xMakeInfoTree = False
        If mtblCardInfo.Contains(intRootID) Then Return True

        If intGeneration > mintMaxGeneration Then Return True

        Dim stCard As stCardInfo
        Dim drData As DataRow()

        stCard = fncGetMemberInfo(intRootID, mtblUser)
        stCard.intLevel = intGeneration
        'Get Spouse
        drData = fncGetRowsFromDataTable(mtblRelMarriage, String.Format("MEMBER_ID = {0} AND RELID = {1}", intRootID, CInt(clsEnum.emRelation.MARRIAGE)), "ROLE_ORDER ASC")
        'Make Spouse List
        stCard.lstSpouse = fncMakeMemberIDList(drData, "REL_FMEMBER_ID")
        stCard.lstChild = Nothing
        xSetParentID(stCard)

        If intGeneration < mintMaxGeneration Then
            xGetKidList(stCard)
        End If

        mtblCardInfo.Add(intRootID, stCard)

        If Not stCard.lstSpouse Is Nothing Then
            Dim i As Integer
            For i = 0 To stCard.lstSpouse.Count - 1
                If Not mtblCardInfo.Contains(stCard.lstSpouse(i)) Then
                    Dim stSpouse As stCardInfo
                    stSpouse = fncGetMemberInfo(stCard.lstSpouse(i), mtblUser)
                    stSpouse.intLevel = intGeneration

                    If intGeneration < mintMaxGeneration Then
                        'Get Kid
                        xGetKidList(stSpouse, intRootID)
                    End If

                    'Get Spouse, but it might not need this code
                    'drData = fncGetRowsFromDataTable(mtblRelMarriage, String.Format("MEMBER_ID = {0} AND RELID = {1}", stSpouse.intID, CInt(clsEnum.emRelation.MARRIAGE)), "ROLE_ORDER ASC")
                    'stSpouse.lstSpouse = fncMakeMemberIDList(drData, "REL_FMEMBER_ID")
                    stSpouse.lstSpouse = New List(Of Integer)
                    stSpouse.lstSpouse.Add(intRootID)

                    mtblCardInfo.Add(stSpouse.intID, stSpouse)

                    If intGeneration < mintMaxGeneration Then
                        xMakeInfoTreeOfList(stSpouse.lstStepChild, intGeneration + 1)
                    End If
                End If

                Application.DoEvents()
            Next
        Else


        End If

        xMakeInfoTreeOfList(stCard.lstChild, intGeneration + 1)
        xMakeInfoTreeOfList(stCard.lstStepChild, intGeneration + 1)

        Return True
    End Function


    Private Function xMakeIDList(ByVal dtMember As DataTable) As List(Of Integer)

        If dtMember Is Nothing Then Return Nothing

        Dim i As Integer
        Dim intID As Integer
        Dim lstData As List(Of Integer)

        lstData = New List(Of Integer)
        For i = 0 To dtMember.Rows.Count - 1
            Integer.TryParse(fncCnvNullToString(dtMember.Rows(i).Item("MEMBER_ID")), intID)
            lstData.Add(intID)
        Next

        Return lstData

    End Function

    Private Sub xCreateListIfNothing(ByRef lstList As List(Of Integer))
        If lstList Is Nothing Then
            lstList = New List(Of Integer)
        End If
    End Sub

    Private Sub xGetKidList(ByRef stCard As stCardInfo, Optional ByVal intSpouse As Integer = -1)
        Try
            Dim dtKidList As DataRow()
            Dim i As Integer
            Dim intKidID As Integer
            Dim intParentID As Integer
            Dim blnCheckStepChild As Boolean = False

            dtKidList = fncGetRowsFromDataTable(mtblRelChild, "SPOUSE_LEFT = " & stCard.intID.ToString())

            stCard.lstStepChild = Nothing
            stCard.lstChild = Nothing

            If dtKidList Is Nothing Then Return
            If dtKidList.Length = 0 Then Return

            For i = 0 To dtKidList.Length - 1
                intKidID = basCommon.fncCnvToInt(dtKidList(i)("CHILD_ID"))
                intParentID = basCommon.fncCnvToInt(dtKidList(i)("SPOUSE_RIGHT"))

                If intParentID = 0 Then
                    xCreateListIfNothing(stCard.lstStepChild)
                    stCard.lstStepChild.Add(intKidID)
                Else
                    If basCommon.fncCnvToInt(dtKidList(i)("PARENT_RELID")) <> clsEnum.emRelation.MARRIAGE Then

                        xCreateListIfNothing(stCard.lstStepChild)
                        If Not stCard.lstStepChild.Contains(intKidID) Then
                            stCard.lstStepChild.Add(intKidID)
                        End If

                    Else
                        xCreateListIfNothing(stCard.lstChild)
                        stCard.lstChild.Add(intKidID)

                        If stCard.lstStepChild IsNot Nothing Then
                            If stCard.lstStepChild.Contains(intKidID) Then
                                stCard.lstStepChild.Remove(intKidID)

                            End If
                        End If
                    End If
                End If
            Next

            If stCard.lstStepChild IsNot Nothing Then
                If stCard.lstStepChild.Count = 0 Then stCard.lstStepChild = Nothing
            End If
            If stCard.lstChild IsNot Nothing Then
                If stCard.lstChild.Count = 0 Then stCard.lstChild = Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub xSetParentID(ByRef stChild As stCardInfo)

        Try

            Dim drChild As DataRow() = fncGetRowsFromDataTable(mtblRelChild, "CHILD_ID = " & stChild.intID.ToString())
            Dim intTemp As Integer

            stChild.intFatherID = -1
            stChild.intMotherID = -1

            If drChild Is Nothing Then Return
            If drChild.Length <= 0 Then Return

            stChild.intFatherID = basCommon.fncCnvToInt(drChild(0).Item("SPOUSE_LEFT"))
            stChild.intMotherID = basCommon.fncCnvToInt(drChild(0).Item("SPOUSE_RIGHT"))

            Dim drParent As DataRow() = mtblUser.Select(String.Format("MEMBER_ID = {0}", stChild.intFatherID))

            If basCommon.fncCnvToInt(drParent(0)("GENDER")) = clsEnum.emGender.FEMALE Then
                intTemp = stChild.intFatherID
                stChild.intFatherID = stChild.intMotherID
                stChild.intMotherID = intTemp
            End If

            Return

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Function xGetSpouseList(ByVal intID As Integer) As DataTable

        Try

            Dim vwRel As New DataView(mtblRelMarriage)

            vwRel.RowFilter = String.Format(mcstrFindRelFormat, intID, CInt(clsEnum.emRelation.MARRIAGE))

            If vwRel.Count <= 0 Then Return Nothing

            Return vwRel.ToTable

        Catch ex As Exception
            Throw ex
        End Try

        Return Nothing
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

#Region "Add By: 2017/07/26 AKB Nguyen Thanh Tung"

    '   ******************************************************************
    '		FUNCTION   : Constructor
    '		PARAMS     : ARG1(IN) - Integer - Root ID
    '                    ARG2(IN) - Integer - Max Generation
    '		MEMO       : Initialize Class
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal intRootID As Integer, _
                   ByVal intMaxGeneration As Integer)
        Try

            mintRootID = intRootID
            mintMaxGeneration = intMaxGeneration

            mtblUser = Nothing
            mtblRel = Nothing
            mtblRelMarriage = Nothing
            mtblRelChild = Nothing
            mtblCardInfo = New Hashtable()
            mtblCardCtrl = New Hashtable()
            mlstNormalLine = New List(Of usrLine)
            mlstSpecialLine = New List(Of usrLine)
            mlstSelectedCtrl = New List(Of usrMemCardBase)

            mblnIsSmallCard = True
            If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then mblnIsSmallCard = False

            mblnIsSmallCard = True
            If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then mblnIsSmallCard = False
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "New(Integer,Integer)", ex)
            Throw
        End Try
    End Sub

    '   ******************************************************************
    '		FUNCTION   : fncGetData
    '		MEMO       : Get Data DB
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Sub fncGetData()
        Try

            xReadData()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncGetData", ex)
            Throw
        End Try
    End Sub

    '   ******************************************************************
    '		FUNCTION   : fncDrawPDF
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - Integer - Root ID
    '                    ARG2(IN) - Integer - Max Generation
    '                    ARG3(IN) - Integer - Location X
    '                    ARG4(IN) - Integer - Location Y
    '                    ARG5(OUT) - List(Of Integer) - List Root ID Max Generation
    '		MEMO       : Draw Tree For PDF
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncDrawPDF(ByVal intRootID As Integer,
                               ByVal intMaxGeneration As Integer, _
                               ByVal intInitX As Integer, _
                               ByVal intInitY As Integer, _
                               ByRef lstRootIDGenerationMax As List(Of Integer)) As Boolean

        fncDrawPDF = False

        Try
            mintRootID = intRootID
            mintMaxGeneration = intMaxGeneration

            Call xClearDataDrawPDF()   'Clear Data Old

            mblnIsSmallCard = True
            If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then mblnIsSmallCard = False
            xSetCardSize()

            xMakeInfoTreePDF(mintRootID, 1, lstRootIDGenerationMax)
            Dim intX As Integer = intInitX

            mintStartX = intInitX
            mintStartY = intInitY

            ' ▽2018/04/24 AKB Nguyen Thanh Tung --------------------------------
            mobjFont = My.Settings.objFontDefaut
            If My.Settings.blnTypeCardShort Then basCommon.fncModeShortCard(mtblCardInfo, mblnIsSmallCard, mintMEM_CARD_W, mintMEM_CARD_H, mobjFont)
            ' △2018/04/24 AKB Nguyen Thanh Tung --------------------------------

            xCaculateCoordinateTree(mintRootID, mintStartX, mintStartY)

            Call xAddCtrlPDF() 'Draw Line

            fncDrawPDF = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncDrawPDF", ex)
            Throw
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xAddCtrlPDF
    '		MEMO       : Draw Line
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Sub xAddCtrlPDF()

        Dim objCard As usrMemberCard1
        Dim stCard As stCardInfo

        Try

            For Each element As DictionaryEntry In mtblCardInfo

                If Not mtblCardCtrl.Contains(element.Key) Then

                    stCard = xGetMemberCard(CInt(element.Key))

                    ' ▽ 2018/04/24 AKB Nguyen Thanh Tung --------------------------------
                    If My.Settings.blnTypeCardShort Then

                        objCard = fncMakeCardInfoType2(stCard, mblnIsSmallCard)
                        objCard.Height = mintMEM_CARD_H
                        objCard.Width = mintMEM_CARD_W

                    Else

                        objCard = fncMakeCardInfoType1(stCard, mblnIsSmallCard)

                    End If

                    objCard.lblName.Font = mobjFont
                    objCard.AliveStatus = Not (stCard.stBasicInfo.intDecease = basConst.gcintDIED)

                    'objCard = fncMakeCardInfoType1(stCard, mblnIsSmallCard)
                    ' △ 2018/04/24 AKB Nguyen Thanh Tung --------------------------------

                    objCard.Location = New Point(stCard.intX, stCard.intY)
                    objCard.CardCoor = New clsCoordinate(stCard.intX, stCard.intY)
                    objCard.DrawLv = stCard.intLevel

                    mtblCardCtrl.Add(stCard.intID, objCard)

                End If

                Application.DoEvents()
            Next

            xDrawLinePDF(mintRootID)
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddCtrlPDF", ex)
            Throw
        End Try
    End Sub

    Private Sub xDrawLinePDF(ByVal intRootID As Integer, _
                             Optional ByVal blnDrawSpouse As Boolean = True, _
                             Optional ByVal intPos As Integer = 1)

        Dim objRootCard As usrMemberCard1
        Dim stRootCard As stCardInfo
        Dim i As Integer

        stRootCard = xGetMemberCard(CInt(intRootID))
        objRootCard = CType(mtblCardCtrl.Item(intRootID), usrMemberCard1)

        If Not stRootCard.lstSpouse Is Nothing And blnDrawSpouse Then

            If stRootCard.lstSpouse.Count = 1 Then
                'Connect to StepChild
                xDrawConnector2ListChildPDF(intRootID, True, 1)


                'Connect to Child
                xDrawConnectorListChildOfCouplePDF(intRootID, stRootCard.lstSpouse(0))
                'xDrawConnector2ListChild(stRootCard.lstSpouse(0), False, -1)

                'Connect to Spouse
                xDrawConnector2SpousePDF(intRootID, stRootCard.lstSpouse(0))

                'Call DrawLine for Child
                'Step child
                xDrawLineChildPDF(intRootID, True)
                'Child
                xDrawLineChildPDF(intRootID, False)


                xDrawLineChildPDF(stRootCard.lstSpouse(0), True)
                xDrawConnector2ListChildPDF(stRootCard.lstSpouse(0), True)

            Else

                xDrawLinePDF(stRootCard.lstSpouse(0), False)
                xDrawConnector2ListChildPDF(intRootID, True, 1)

                For i = 1 To stRootCard.lstSpouse.Count - 1
                    xDrawLinePDF(stRootCard.lstSpouse(i), False, -1)
                Next

                xDrawLineChildPDF(intRootID, True)
                xDrawConnector2ListSpousePDF(intRootID)

            End If

        Else

            If blnDrawSpouse Then
                xDrawConnector2ListChildPDF(intRootID, True, 1)
                xDrawLineChildPDF(intRootID, True)

            Else
                If intPos > 0 Then
                    xDrawConnector2ListChildPDF(intRootID, True)
                    xDrawConnector2ListChildPDF(intRootID, False, 1)

                    xDrawLineChildPDF(intRootID, True)
                    xDrawLineChildPDF(intRootID, False)
                Else
                    xDrawConnector2ListChildPDF(intRootID, False, -1)
                    xDrawConnector2ListChildPDF(intRootID, True)

                    xDrawLineChildPDF(intRootID, False)
                    xDrawLineChildPDF(intRootID, True)

                End If

            End If

        End If
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2Child, Draw To 2 Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************    
    Private Sub xDrawConnector2ChildPDF(ByVal intParentID As Integer, _
                                        ByVal intChildID As Integer, _
                                        Optional ByVal blnStepChild As Boolean = False, _
                                        Optional ByVal intPos As Integer = 1)
        Dim objParent As usrMemberCard1
        Dim stParent As stCardInfo

        Dim objChild As usrMemberCard1
        Dim stChild As stCardInfo

        stParent = xGetMemberCard(intParentID)
        objParent = CType(mtblCardCtrl.Item(intParentID), usrMemberCard1)

        stChild = xGetMemberCard(intChildID)
        objChild = CType(mtblCardCtrl.Item(intChildID), usrMemberCard1)

        If blnStepChild Then

            If objChild Is Nothing Then
                Return
            Else
                xDrawDiffLvPDF(objParent, objChild)
            End If

        Else

            'This CODE to draw child line at the center of Parent
            'Dim objFather As usrMemberCard1 = CType(mtblCardCtrl.Item(stChild.intFatherID), usrMemberCard1)
            'Dim objMother As usrMemberCard1 = CType(mtblCardCtrl.Item(stChild.intMotherID), usrMemberCard1)
            'Dim intDelta As Integer = CInt((Math.Abs(objFather.Left - objMother.Left) - mintMEM_CARD_W) / 2)
            'xDrawDiffLv2(objParent, objChild, intPos, intPos * intDelta)
            xDrawDiffLv2PDF(objParent, objChild, intPos)

        End If

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawDiffLv, draw different level
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawDiffLvPDF(ByVal objUpperCard As usrMemberCard1, ByVal objLowerCard As usrMemberCard1)

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
            basCommon.fncSaveErr(mcstrClsName, "xDrawDiffLvPDF", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawDiffLv, draw different level
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawDiffLv2PDF(ByVal objUpperCard As usrMemberCard1, _
                             ByVal objLowerCard As usrMemberCard1, _
                             Optional ByVal intMinus As Integer = 1, _
                             Optional ByVal intDeltaX As Integer = 0)

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
            objVerLine1 = New usrLine(clsEnum.emLineType.SINGLE_LINE, clsEnum.emLineDirection.VERTICAL, CInt(mintMEM_CARD_SPACE_DOWN / 2 + mintMEM_CARD_H / 2))
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

            If intDeltaX = 0 Then
                'draw line
                If intMinus > 0 Then

                    objVerLine1.fncAddVerticalLine(objUpperCard, clsEnum.emCardPoint.MID_RIGHT, CInt(mintMEM_CARD_SPACE_LEFT / 2))

                Else

                    objVerLine1.fncAddVerticalLine(objUpperCard, clsEnum.emCardPoint.MID_LEFT, -1 * CInt(mintMEM_CARD_SPACE_LEFT / 2))

                End If

            Else

                objVerLine1.fncAddVerticalLine(objUpperCard, clsEnum.emCardPoint.MID_RIGHT, intDeltaX)

            End If


            objVerLine2.fncAddVerticalLine(objLowerCard, clsEnum.emCardPoint.MID_TOP)
            objHorzLine.fncAddHorizontalLine(objVerLine1, objVerLine2)

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
            basCommon.fncSaveErr(mcstrClsName, "xDrawDiffLv2PDF", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2ListChild, Draw To 2 List of Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawConnector2ListChildPDF(ByVal intParentID As Integer, _
                                            ByVal blnStepChild As Boolean, _
                                            Optional ByVal intPos As Integer = 1)

        Dim stParent As stCardInfo
        Dim lstChild As List(Of Integer)
        Dim i As Integer
        stParent = xGetMemberCard(intParentID)

        If blnStepChild Then

            lstChild = stParent.lstStepChild

        Else

            lstChild = stParent.lstChild

        End If

        If lstChild Is Nothing Then Return

        For i = 0 To lstChild.Count - 1
            xDrawConnector2ChildPDF(intParentID, lstChild(i), blnStepChild, intPos)
        Next

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnectorListChildOfCouple, Draw To 2 List Child of a couple
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawConnectorListChildOfCouplePDF(ByVal intParentLeftID As Integer, _
                                                   ByVal intParentRightID As Integer)

        Dim stParentLeft As stCardInfo
        Dim stParentRight As stCardInfo
        Dim stChild As stCardInfo

        Dim objParentLeft As usrMemberCard1
        Dim objChild As usrMemberCard1

        Dim lstChild As List(Of Integer)
        Dim intDeltaX As Integer
        Dim i As Integer
        stParentLeft = xGetMemberCard(intParentLeftID)
        stParentRight = xGetMemberCard(intParentRightID)

        intDeltaX = CInt((stParentRight.intX - stParentLeft.intX - mintMEM_CARD_W) / 2)

        lstChild = stParentLeft.lstChild


        If lstChild Is Nothing Then Return

        For i = 0 To lstChild.Count - 1

            objParentLeft = CType(mtblCardCtrl.Item(intParentLeftID), usrMemberCard1)
            stChild = xGetMemberCard(lstChild(i))
            objChild = CType(mtblCardCtrl.Item(lstChild(i)), usrMemberCard1)
            xDrawDiffLv2PDF(objParentLeft, objChild, 1, intDeltaX)

        Next

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawSameLv, draw same level
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/09/14  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawSameLvPDF(ByVal objLeftCard As usrMemberCard1, ByVal objRightCard As usrMemberCard1)
        Try
            Dim objHorzLine1 As usrLine
            Dim objHorzLine2 As usrLine

            objHorzLine1 = New usrLine
            objHorzLine2 = New usrLine

            objHorzLine1.fncAddSpouseLine(objLeftCard, objRightCard, 0)
            objHorzLine2.fncAddSpouseLine(objLeftCard, objRightCard, -4)

            'add to list
            mlstNormalLine.Add(objHorzLine1)
            mlstNormalLine.Add(objHorzLine2)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawConnector", ex)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2SpousePDF, Draw To 2 Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************    
    Private Sub xDrawConnector2SpousePDF(ByVal intLeftSpouseID As Integer, _
                                      ByVal intRightSpouse As Integer)


        Dim objLeft As usrMemberCard1
        Dim objRight As usrMemberCard1

        objLeft = CType(mtblCardCtrl.Item(intLeftSpouseID), usrMemberCard1)
        objRight = CType(mtblCardCtrl.Item(intRightSpouse), usrMemberCard1)
        xDrawSameLvPDF(objLeft, objRight)

    End Sub

    Private Sub xDrawLineChildPDF(ByVal intParentID As Integer, ByVal blnStepChild As Boolean)

        Dim stParent As stCardInfo
        stParent = xGetMemberCard(CInt(intParentID))
        Dim lstChildID As List(Of Integer)
        Dim i As Integer

        If blnStepChild Then

            lstChildID = stParent.lstStepChild

        Else

            lstChildID = stParent.lstChild

        End If

        If lstChildID Is Nothing Then Return
        For i = 0 To lstChildID.Count - 1
            xDrawLinePDF(lstChildID(i))
        Next

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2ListSpousePDF, Draw To 2 Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************    
    Private Sub xDrawConnector2ListSpousePDF(ByVal intRootID As Integer)


        Dim stRootCard As stCardInfo
        stRootCard = xGetMemberCard(CInt(intRootID))
        Dim lstSpouse As List(Of Integer)
        Dim i As Integer
        Dim intLeftSpouse As Integer

        lstSpouse = stRootCard.lstSpouse

        If lstSpouse Is Nothing Then Return

        If lstSpouse.Count = 1 Then
            xDrawConnector2SpousePDF(intRootID, lstSpouse(0))
            Return
        End If

        xDrawConnector2SpousePDF(lstSpouse(0), intRootID)

        intLeftSpouse = intRootID
        For i = 1 To lstSpouse.Count - 1
            xDrawConnector2SpousePDF(intLeftSpouse, lstSpouse(i))
            intLeftSpouse = lstSpouse(i)
        Next


    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xMakeInfoTreePDF, Make info family tree
    '      VALUE      : Boolean : true: OK
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xMakeInfoTreePDF(ByVal intRootID As Integer, _
                                      ByVal intGeneration As Integer, _
                                      ByRef lstRootIDGenerationMax As List(Of Integer)) As Boolean

        xMakeInfoTreePDF = False
        If mtblCardInfo.Contains(intRootID) Then Return True

        If intGeneration > mintMaxGeneration Then Return True

        Dim stCard As stCardInfo
        Dim drData As DataRow()

        stCard = fncGetMemberInfo(intRootID, mtblUser)
        stCard.intLevel = intGeneration

        'Get Spouse
        drData = fncGetRowsFromDataTable(mtblRelMarriage, String.Format("MEMBER_ID = {0} AND RELID = {1}", intRootID, CInt(clsEnum.emRelation.MARRIAGE)), "ROLE_ORDER ASC")
        'Make Spouse List
        stCard.lstSpouse = fncMakeMemberIDList(drData, "REL_FMEMBER_ID")

        stCard.lstChild = Nothing
        xSetParentID(stCard)

        If intGeneration < mintMaxGeneration Then
            xGetKidList(stCard)
        End If

        mtblCardInfo.Add(intRootID, stCard)

        If Not stCard.lstSpouse Is Nothing Then
            Dim i As Integer
            For i = 0 To stCard.lstSpouse.Count - 1
                If Not mtblCardInfo.Contains(stCard.lstSpouse(i)) Then
                    Dim stSpouse As stCardInfo
                    stSpouse = fncGetMemberInfo(stCard.lstSpouse(i), mtblUser)
                    stSpouse.intLevel = intGeneration

                    If intGeneration < mintMaxGeneration Then
                        'Get Kid
                        xGetKidList(stSpouse, intRootID)
                    End If

                    'Get Spouse, but it might not need this code
                    'drData = fncGetRowsFromDataTable(mtblRelMarriage, String.Format("MEMBER_ID = {0} AND RELID = {1}", stSpouse.intID, CInt(clsEnum.emRelation.MARRIAGE)), "ROLE_ORDER ASC")
                    'stSpouse.lstSpouse = fncMakeMemberIDList(drData, "REL_FMEMBER_ID")
                    stSpouse.lstSpouse = New List(Of Integer)
                    stSpouse.lstSpouse.Add(intRootID)

                    mtblCardInfo.Add(stSpouse.intID, stSpouse)

                    If intGeneration < mintMaxGeneration Then
                        xMakeInfoTreeOfListPDF(stSpouse.lstStepChild, intGeneration + 1, lstRootIDGenerationMax)
                    End If
                End If

                Application.DoEvents()
            Next
        Else


        End If

        xMakeInfoTreeOfListPDF(stCard.lstChild, intGeneration + 1, lstRootIDGenerationMax)
        xMakeInfoTreeOfListPDF(stCard.lstStepChild, intGeneration + 1, lstRootIDGenerationMax)

        Return True
    End Function

    Private Sub xMakeInfoTreeOfListPDF(ByVal lstID As List(Of Integer),
                                       ByVal intGeneration As Integer,
                                       ByRef lstRootIDGenerationMax As List(Of Integer))

        If lstID Is Nothing Then Return

        If intGeneration = mintMaxGeneration Then
            If IsNothing(lstRootIDGenerationMax) Then lstRootIDGenerationMax = New List(Of Integer)
            lstRootIDGenerationMax.AddRange(lstID)
        End If

        Dim i As Integer

        For i = 0 To lstID.Count - 1
            Application.DoEvents()
            xMakeInfoTreePDF(lstID(i), intGeneration, lstRootIDGenerationMax)
            Application.DoEvents()
        Next

    End Sub

    '   ******************************************************************
    '		FUNCTION   : xClearDataDrawPDF
    '		MEMO       : Clear data draw
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Sub xClearDataDrawPDF()
        Try

            'dipose control in hastable
            xDisposeCard(mtblCardCtrl)
            mtblCardCtrl.Clear()
            mtblCardInfo.Clear()

            If mlstNormalLine IsNot Nothing Then mlstNormalLine.Clear()
            If mlstSpecialLine IsNot Nothing Then mlstSpecialLine.Clear()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClearDataDrawPDF", ex)
            Throw
        End Try
    End Sub
#End Region

End Class
