'   ****************************************************************** 
'      TITLE      : DRAW FAMILY TREE
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
''' CLASS DRAW TREE
''' </summary>
''' <remarks></remarks>
''' <Create>2012/02/13  AKB Quyet</Create>
''' ▽ 2017/07/27 AKB Nguyen Thanh Tung --------------------------------
'''Public Class clsDrawTreeS2
Public Class clsDrawTreeS2
    Implements IDisposable
    ' △ 2017/07/27 AKB Nguyen Thanh Tung --------------------------------


    Private Const mcstrClsName As String = "clsDrawTree2"               'class name
    Private Const mcstrMemberFilter As String = "MEMBER_ID = {0}"       'filter string

    Private mtblData As DataTable                                       'member data
    Private mtblRel As DataTable                                        'relationship
    Private mtblDrawLv As DataTable                                     'table to store drawing level in the form of lv/key(id)
    Private mtblControl As Hashtable                                    'table to store control (container card)
    Private mtblDetailCard As Hashtable                                 'table to store control (detail card)
    Private mlstNormalLine As List(Of usrLine)                          'table to store control (line)
    Private mlstSpecialLine As List(Of usrLine)                         'table to store control (line)
    Private mlstNotDraw As List(Of Integer)                             'table to store invisible control
    Private mlstSelectedCtrl As List(Of usrMemCardBase)                 'list to store selected controls

    Private mblnIsSmallCard As Boolean                                  'draw small card
    Private mpnDraw As pnTreePanel                                      'panel to draw
    Private mfrmPerInfo As frmPersonInfo                                'person info form

    Private mintStartID As Integer
    Private mintRight As Integer = 0                                    'margin righ
    Private mintBottom As Integer = 0                                   'margin bottom
    Private mintLv As Integer = 0                                       'level counter
    Private mintX As Integer = 0                                        'X - counter
    Private mintY As Integer = 0                                        'Y - counter
    Private mintCardWidth As Integer                                    'card width
    Private mintMaxPanelWith As Integer = 0                             'max panel width
    Private mintMaxPanelHeight As Integer = 0                           'max panel height
    Private mintMaxGeneration As Integer

    Private mobjTempSelectedCard As usrMemberCard2                      'temporary selected card


    ''' <summary>
    ''' Structure stCardSize
    ''' </summary>
    ''' <remarks></remarks>
    Private Structure stCardSize

        Dim intHeight As Integer
        Dim intWidth As Integer

    End Structure

    ''' <summary>
    ''' Structure stDetail
    ''' </summary>
    ''' <remarks>Structure to store filling information</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Structure stCardDetail

        '2016/12/27 - Manh Start
        Dim stBasicInfo As stBasicCardInfo
        '2016/12/27 - Manh End

        Dim strAvatar As String
        Dim strRemark As String
        Dim intLevel As Integer

    End Structure


    Public Event evnCardClicked(ByVal intMemID As Integer)                                  'double click on card event
    Public Event evnCardDoubleClicked(ByVal intMemID As Integer)                            'double click on card event
    Public Event evnRefresh(ByVal intMemID As Integer, ByVal blnRedraw As Boolean)          'need to refresh event
    Public Event evnProgressDone()                                                          'draw finished event


#Region "PROPERTIES"

    ''' <summary>
    ''' DrawingCard - Gets list of cards
    ''' </summary>
    ''' <value></value>
    ''' <returns>Hashtable</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property DrawingCard() As Hashtable
        Get
            Return mtblControl
        End Get
    End Property


    ''' <summary>
    ''' DrawList Property, return list of mem by level
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DrawList() As DataView
        Get
            mtblDrawLv.DefaultView.Sort = "Level ASC"
            Return mtblDrawLv.DefaultView
        End Get
    End Property


    ''' <summary>
    ''' NormalLine -  Gets list of Normal line
    ''' </summary>
    ''' <value>List(Of usrLine)</value>
    ''' <returns>List(Of usrLine)</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property NormalLine() As List(Of usrLine)
        Get
            Return mlstNormalLine
        End Get
    End Property


    ''' <summary>
    ''' SpecialLine - Gets list of special line (connects head members)
    ''' </summary>
    ''' <value>List(Of usrLine)</value>
    ''' <returns>List(Of usrLine)</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property SpecialLine() As List(Of usrLine)
        Get
            Return mlstSpecialLine
        End Get
    End Property


    ''' <summary>
    ''' MaxWidth - Gets max width of panel
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property MaxWidth() As Integer
        Get
            Dim objCard As usrMemberCard2

            mintMaxPanelWith = 0

            For Each element As DictionaryEntry In mtblControl

                objCard = CType(element.Value, usrMemberCard2)

                If mintMaxPanelWith <= objCard.Location.X Then mintMaxPanelWith = objCard.Location.X + objCard.Width

            Next

            Return mintMaxPanelWith + 20    '20 is left space

        End Get
    End Property


    ''' <summary>
    ''' MaxHeight - Gets max height of panel
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property MaxHeight() As Integer
        Get
            Dim objCard As usrMemberCard2

            mintMaxPanelHeight = 0

            For Each element As DictionaryEntry In mtblControl

                objCard = CType(element.Value, usrMemberCard2)

                'If objCard.CardLevel < My.Settings.intGeneration - 1 Then Continue For

                If mintMaxPanelHeight < objCard.Location.Y + objCard.Height Then mintMaxPanelHeight = objCard.Location.Y + objCard.Height

            Next

            Return mintMaxPanelHeight

        End Get
    End Property


    ''' <summary>
    ''' RootMemberInfo - return root member info string
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property RootMemberInfo() As String
        Get
            Dim objCard As usrMemberCard2

            objCard = CType(mtblControl.Item(mintStartID), usrMemberCard2)

            'Dim intGen As Integer = mintMaxGeneration + stCard.intLevel - 1

            If objCard.CardMember(0).CardGender = clsEnum.emGender.FEMALE Then
                Return "Bà " & objCard.CardMember(0).CardName.ToUpper & " " & objCard.CardMember(0).CardAlias.ToUpper '& " (Đời " & CStr(intGen) & ")"
            ElseIf objCard.CardMember(0).CardGender = clsEnum.emGender.MALE Then
                Return "Ông " & objCard.CardMember(0).CardName.ToUpper & " " & objCard.CardMember(0).CardAlias.ToUpper '& " (Đời " & CStr(intGen) & ")"

            End If

            Return "Thành viên " & objCard.CardMember(0).CardName.ToUpper & " " & objCard.CardMember(0).CardAlias.ToUpper '& " (Đời " & CStr(intGen) & ")"

        End Get
    End Property

    '   ******************************************************************
    '　　　	FUNCTION   : RootID
    '      	MEMO       : Get Root ID
    '      	CREATE     : 2017/07/27 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property RootID() As Integer
        Get
            Return mintStartID
        End Get
    End Property
#End Region


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <param name="pnDraw">Panel to draw</param>
    ''' <param name="frmPerInfo">person info form</param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Sub New(ByVal pnDraw As pnTreePanel, ByVal frmPerInfo As frmPersonInfo)

        Me.mpnDraw = pnDraw
        Me.mfrmPerInfo = frmPerInfo
        Me.mblnIsSmallCard = True

        mtblDrawLv = New DataTable()
        mtblDrawLv.Columns.Add("Level", System.Type.GetType("System.Int32"))
        mtblDrawLv.Columns.Add("ID", System.Type.GetType("System.Int32"))

        mlstSelectedCtrl = New List(Of usrMemCardBase)

    End Sub


    ''' <summary>
    ''' fncDraw
    ''' </summary>
    ''' <param name="intStartID">Draw from this id</param>
    ''' <param name="tblData">member data</param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Sub fncDraw(ByVal intStartID As Integer, ByVal tblData As DataTable, ByVal intMaxGeneration As Integer)

        mpnDraw.Visible = False
        Try
            mintMaxGeneration = intMaxGeneration
            If tblData Is Nothing Then
                'raise event for closing waiting form
                RaiseEvent evnProgressDone()
                Exit Sub
            End If

            'add handler of panel
            AddHandler mpnDraw.evnMultiSelection, AddressOf xMultiSelect

            Me.mtblData = tblData
            Me.mintStartID = intStartID

            'Me.mblnIsSmallCard = blnIsSmallCard
            mblnIsSmallCard = True
            If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then mblnIsSmallCard = False

            fncClear()
            xInit(False)

            Application.DoEvents()
            ''read data
            xReadData()

            Application.DoEvents()

            'do recusive to add member to table
            xRecusiveDraw(intStartID, False, basConst.gcintNONE_VALUE, mintX)


            'align Y-coodinate
            xAlignX()
            xAlignY()

            xAddCtrl2Panel()

            xDrawConnector()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncDraw", ex)
        Finally
            'Erase mintMaxH
            mpnDraw.Visible = True
            xSetScrollView(intStartID)
            'raise event for closing waiting form
            RaiseEvent evnProgressDone()

        End Try
    End Sub


    ''' <summary>
    ''' fncRedrawCard - redraw a card
    ''' </summary>
    ''' <param name="intID">member to be redrawn</param>
    ''' <remarks></remarks>
    Public Sub fncRedrawCard(ByVal intID As Integer)

        Try
            Dim objCardDetail As usrMemberDetail = Nothing

            If Not mtblDetailCard.ContainsKey(intID) Then Exit Sub

            'get card then redraw
            objCardDetail = CType(mtblDetailCard(intID), usrMemberDetail)

            xReDraw(intID, objCardDetail)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncRedrawCard", ex)
        End Try

    End Sub


    ''' <summary>
    ''' fncClear - Clear all controls
    ''' </summary>
    ''' <remarks>Clear all controls</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Sub fncClear()


        Try

            Dim objCard As usrMemberCard2
            Dim objCardDetail As usrMemberDetail

            If mtblRel IsNot Nothing Then mtblRel.Dispose()

            If mtblDetailCard IsNot Nothing Then

                For Each element As DictionaryEntry In mtblDetailCard
                    objCardDetail = CType(element.Value, usrMemberDetail)
                    objCardDetail.Dispose()
                Next

            End If

            If mtblControl IsNot Nothing Then

                For Each element As DictionaryEntry In mtblControl
                    objCard = CType(element.Value, usrMemberCard2)
                    objCard.Dispose()
                Next

            End If

            If mtblDrawLv IsNot Nothing Then mtblDrawLv.Rows.Clear() 'reset 

            xClearLine()

            mpnDraw.Controls.Clear()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncClear", ex)
        Finally

        End Try
    End Sub


    ''' <summary>
    ''' fncSetFocus - scroll to a card
    ''' </summary>
    ''' <param name="intMemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncSetFocus(ByVal intMemID As Integer) As Boolean

        fncSetFocus = False

        Try
            Return xSetScrollView(intMemID, True)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncSetFocus", ex)
        End Try

    End Function


    ''' <summary>
    ''' xInit
    ''' </summary>
    ''' <remarks>init data</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xInit(ByVal blnResetValueOnly As Boolean)
        Try
            If Not blnResetValueOnly Then
                mtblControl = New Hashtable()
                mtblDetailCard = New Hashtable()
                mlstNotDraw = New List(Of Integer)
                mlstNormalLine = New List(Of usrLine)
                mlstSpecialLine = New List(Of usrLine)
            End If

            mintLv = 0
            mintX = 20
            mintY = 0
            mintRight = clsDefine.MEMCARD_2_MARGIN_RIGHT
            mintBottom = clsDefine.MEMCARD_2_VERTICAL_BUFFER

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xReadData
    ''' </summary>
    ''' <returns>reading success</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xReadData() As Boolean

        xReadData = False

        Try
            If mtblRel IsNot Nothing Then mtblRel.Clear()

            'get data table
            mtblRel = gobjDB.fncGetRel()

            If mtblRel Is Nothing Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xReadData", ex)
        End Try

    End Function

    Private mintLastX As Integer
    Private mintLastWidth As Integer

    ''' <summary>
    ''' xRecusiveDraw
    ''' </summary>
    ''' <param name="intStartID">Draw from member</param>
    ''' <returns>true - success / false - fail</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xRecusiveDraw(ByVal intStartID As Integer, ByVal blnRedraw As Boolean, ByVal intParentID As Integer, ByVal intX As Integer) As stCardSize

        Dim intSize As stCardSize
        xRecusiveDraw = intSize

        Dim lstKid As List(Of Integer)

        Try
            Dim intHeight As Integer
            'Dim i As Integer

            mintLv += 1

            'limit level
            If mintLv > mintMaxGeneration Then 'My.Settings.intGeneration Then
                mintLv -= 1
                Exit Function
            End If

            If Not blnRedraw Then
                'draw current member
                intSize = xCreateCard(intStartID, intX, mintY, mintLv, intParentID)
            Else
                If Not xResetLocation(intStartID, intX, mintY) Then
                    mintLv -= 1
                    Exit Function
                End If
            End If

            mintY += intHeight + mintBottom

            'draw child
            'lstKid = basCommon.fncGetKidList(intStartID, clsEnum.emRelation.NATURAL)   'get natural son only
            ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
            lstKid = New List(Of Integer)
            Dim stCard As stCardInfo = fncGetMemberInfo(intStartID, mtblData)

            If My.Settings.intSelectedTypeShowTree <> CInt(clsEnum.emTypeShowTree.OnlyShowMember) _
            OrElse stCard.stBasicInfo.intGender = clsEnum.emGender.MALE Then
                lstKid = basCommon.fncGetKidList(intStartID)
            End If
            'lstKid = basCommon.fncGetKidList(intStartID)                                'get both natural and adopted
            ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------

            ''draw each  child
            If lstKid.Count > 0 Then

                Dim stTemp As stCardSize

                For i As Integer = 0 To lstKid.Count - 1

                    ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
                    If My.Settings.intSelectedTypeShowTree = clsEnum.emTypeShowTree.OnlyShowMale Then
                        Dim stChild As stCardInfo = fncGetMemberInfo(lstKid(i), mtblData)
                        If stChild.stBasicInfo.intGender = clsEnum.emGender.FEMALE Then Continue For
                    End If
                    ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------

                    Application.DoEvents()
                    stTemp = xRecusiveDraw(lstKid(i), blnRedraw, intStartID, intX)

                    'do not increase x if the last child reached.
                    'If mintLv < My.Settings.intGeneration Then If i < lstKid.Count - 1 Then mintX += mintCardWidth + mintRight
                    If mintLv < mintMaxGeneration Then
                        If i < lstKid.Count - 1 Then

                            'the code below for determinng next X value
                            'it can be (brother X-value + width) or (brother's last child X-value + width)
                            Dim intTemp As Integer

                            intX += stTemp.intWidth + mintRight                     'brother X-value + width
                            intTemp = mintLastX + mintLastWidth + mintRight         'brother's last child X-value + width

                            If intTemp > intX Then intX = intTemp

                        End If
                    End If

                    Application.DoEvents()

                Next

            End If


            mintY -= intHeight + mintBottom
            mintLv -= 1

            Return intSize

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRecusiveDraw", ex)
        Finally
            lstKid = Nothing
        End Try

    End Function


    ''' <summary>
    ''' xCreateCard
    ''' </summary>
    ''' <param name="intID">Family member id</param>
    ''' <param name="intX">X coodinate</param>
    ''' <param name="intY">Y coodinate</param>
    ''' <param name="intLevel">Level of these members</param>
    ''' <returns></returns>
    ''' <remarks>Create container card</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xCreateCard(ByVal intID As Integer,
                                 ByVal intX As Integer,
                                 ByVal intY As Integer,
                                 ByVal intLevel As Integer,
                                 ByVal intParentID As Integer) As stCardSize

        Dim stSize As stCardSize
        Dim intCardHeight As Integer = 0
        Dim intLv(1) As Object

        Try
            Dim objCard As usrMemberCard2
            Dim objDictSpouseList As Dictionary(Of Integer, String)
            Dim objDetailCard As usrMemberDetail

            objCard = New usrMemberCard2()

            'add current member
            objDetailCard = xCreateCardDetail(intID, objCard)
            objCard.fncAddItem(objDetailCard)
            objCard.Visible = True
            mtblControl.Add(intID, objCard)

            'and spouse
            ' ▽ 2018/02/07 AKB Nguyen Thanh Tung --------------------------------
            If My.Settings.intSelectedTypeShowTree = clsEnum.emTypeShowTree.All Then
                objDictSpouseList = basCommon.fncGetHusWifeList(intID)
            Else
                objDictSpouseList = New Dictionary(Of Integer, String)
            End If
            'objDictSpouseList = basCommon.fncGetHusWifeList(intID)
            ' △ 2018/02/07 AKB Nguyen Thanh Tung --------------------------------

            If objDictSpouseList.Count > 0 Then

                For Each element As KeyValuePair(Of Integer, String) In objDictSpouseList

                    Dim objSpouseCard As usrMemberDetail
                    Dim intSpouseID As Integer

                    intSpouseID = basCommon.fncCnvToInt(element.Key)
                    objSpouseCard = xCreateCardDetail(intSpouseID, objCard)
                    'mtblControl.Add(intSpouseID, objSpouseCard)
                    objCard.fncAddItem(objSpouseCard)

                Next

            End If

            'set position of the card
            objCard.Location = New Point(intX, intY)
            objCard.CardCoor = New clsCoordinate(intX, intY)
            objCard.CardID = intID
            objCard.CardLevel = intLevel
            objCard.ParentID = intParentID

            'store current drawing level
            objCard.DrawLv = mintLv
            intLv(0) = mintLv
            intLv(1) = intID
            mtblDrawLv.Rows.Add(intLv)

            If mblnIsSmallCard Then objCard.CardSize = clsEnum.emCardSize.SMALL

            'get card width 
            stSize.intWidth = objCard.Width
            stSize.intHeight = objCard.Height

            'temporary store last card's value
            mintLastX = intX
            mintLastWidth = objCard.Width

            'addhandler
            AddHandler objCard.evnNotDraw, AddressOf xHandleNotDraw
            AddHandler objCard.evnCardLocationChange, AddressOf xCardMove

            'store max height
            'If mintMaxH(intLevel) < intCardHeight Then mintMaxH(intLevel) = intCardHeight

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCreateCard", ex)
        Finally
            Erase intLv
        End Try

        Return stSize

    End Function


    ''' <summary>
    ''' xCreateCardDetail
    ''' </summary>
    ''' <param name="intID">ID of member</param>
    ''' <param name="objContainer">Container</param>
    ''' <returns>usrMemberDetail card</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xCreateCardDetail(ByVal intID As Integer, ByVal objContainer As usrMemberCard2) As usrMemberDetail

        Dim objCardDetail As usrMemberDetail = Nothing

        Try
            'create new
            objCardDetail = New usrMemberDetail(intID)

            'fill card
            xFillMemberDetail(intID, objCardDetail, mtblData)
            objCardDetail.CardContainer = objContainer
            objCardDetail.fncResize()

            'add handler
            AddHandler objCardDetail.evnCardDoubleClick, AddressOf xCardDoubleClick
            AddHandler objCardDetail.evnCardClick, AddressOf xCardClick

            'add to list
            Me.mtblDetailCard.Add(intID, objCardDetail)


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCreateCardDetail", ex)
        End Try

        Return objCardDetail

    End Function


    ''' <summary>
    ''' xReadMemberDetail
    ''' </summary>
    ''' <param name="intID">Member ID</param>
    ''' <param name="tblData">Member Data</param>
    ''' <param name="blnGetLevel">Get Level or not</param>
    ''' <returns>stDetail - data structure</returns>
    ''' <remarks>Read member detail to fill on card</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xReadMemberDetail(ByVal intID As Integer, ByVal tblData As DataTable, Optional ByVal blnGetLevel As Boolean = True) As stCardDetail

        Dim stMemDetail As stCardDetail = Nothing

        Try
            With stMemDetail

                For Each row As DataRow In tblData.Select(String.Format(mcstrMemberFilter, intID))

                    If blnGetLevel Then Integer.TryParse(basCommon.fncCnvNullToString(row("LEVEL")), .intLevel)
                    Integer.TryParse(basCommon.fncCnvNullToString(row("GENDER")), .stBasicInfo.intGender)

                    .stBasicInfo.strLastName = basCommon.fncCnvNullToString(row("LAST_NAME"))
                    .stBasicInfo.strMidName = basCommon.fncCnvNullToString(row("MIDDLE_NAME"))
                    .stBasicInfo.strFirstName = basCommon.fncCnvNullToString(row("FIRST_NAME"))
                    .stBasicInfo.strFullName = basCommon.fncGetFullName(.stBasicInfo.strFirstName, .stBasicInfo.strMidName, .stBasicInfo.strLastName, "")
                    .stBasicInfo.strAlias = basCommon.fncCnvNullToString(row("ALIAS_NAME"))
                    .strAvatar = basCommon.fncCnvNullToString(row("AVATAR_PATH"))
                    .strRemark = basCommon.fncCnvNullToString(row("T_FMEMBER_MAIN.REMARK"))

                    'Date.TryParse(basCommon.fncCnvNullToString(row("BIRTH_DAY")), .dtBirth)
                    Integer.TryParse(fncCnvNullToString(row("BIR_DAY")), .stBasicInfo.stBirthDaySun.intDay)
                    Integer.TryParse(fncCnvNullToString(row("BIR_MON")), .stBasicInfo.stBirthDaySun.intMonth)
                    Integer.TryParse(fncCnvNullToString(row("BIR_YEA")), .stBasicInfo.stBirthDaySun.intYear)

                    'Date.TryParse(basCommon.fncCnvNullToString(row("DECEASED_DATE")), .dtDecease)
                    Integer.TryParse(fncCnvNullToString(row("DEA_DAY")), .stBasicInfo.stDeadDayMoon.intDay)
                    Integer.TryParse(fncCnvNullToString(row("DEA_MON")), .stBasicInfo.stDeadDayMoon.intMonth)
                    Integer.TryParse(fncCnvNullToString(row("DEA_YEA")), .stBasicInfo.stDeadDayMoon.intYear)

                    Integer.TryParse(fncCnvNullToString(row("DEA_DAY_SUN")), .stBasicInfo.stDeadDaySun.intDay)
                    Integer.TryParse(fncCnvNullToString(row("DEA_MON_SUN")), .stBasicInfo.stDeadDaySun.intMonth)
                    Integer.TryParse(fncCnvNullToString(row("DEA_YEA_SUN")), .stBasicInfo.stDeadDaySun.intYear)

                    Integer.TryParse(fncCnvNullToString(row("DECEASED")), .stBasicInfo.intDecease)

                Next

            End With

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCreateCardDetail", ex)
        End Try

        Return stMemDetail

    End Function


    ''' <summary>
    ''' xFillMemberDetail
    ''' </summary>
    ''' <param name="intID">Member ID</param>
    ''' <param name="objCardDetail">Card to fill</param>
    ''' <param name="tblData">Data</param>
    ''' <param name="blnGetLevel">Fill Level or not</param>
    ''' <remarks>Fill data on card</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xFillMemberDetail(ByVal intID As Integer,
                                  ByRef objCardDetail As usrMemberDetail,
                                  ByVal tblData As DataTable,
                                  Optional ByVal blnGetLevel As Boolean = True)

        Try
            Dim stMemDetail As stCardDetail

            Dim strBirth As String = ""
            Dim strDeath As String = ""

            'fill data
            stMemDetail = xReadMemberDetail(intID, tblData, blnGetLevel)

            With stMemDetail

                'fill info
                objCardDetail.CardName = .stBasicInfo.strFullName.ToUpper()
                objCardDetail.CardAlias = .stBasicInfo.strAlias
                objCardDetail.CardRemark = .strRemark
                If blnGetLevel Then objCardDetail.CardLevel = .intLevel
                objCardDetail.CardGender = .stBasicInfo.intGender


                ' ▽ 2012/11/14   AKB Quyet （変更内容）*********************************
                'objCardDetail.CardBirth = ""
                'objCardDetail.CardDie = ""
                'objCardDetail.CardBirth = basCommon.fncGetDateName("", .intBday, .intBmon, .intByea, True, False)
                'objCardDetail.CardDie = basCommon.fncGetDateName("", .intDday, .intDmon, .intDyea, True, False)

                strBirth = basCommon.fncGetDateStatus(.stBasicInfo.stBirthDaySun, basConst.gcintALIVE)
                If strBirth = "Ngày sinh: không rõ" Then strBirth = ""

                '2016/12/27 Start Manh Add to show Date of Dead in Sun Calendar
                strDeath += fncGetDeadDateStringDisplaybyOption(.stBasicInfo, My.Settings.intDeadDateShowType).Replace(vbCrLf, " ")
                '2016/12/27 End

                objCardDetail.CardBirth = strBirth
                objCardDetail.CardDie = strDeath

                ' △ 2012/11/14   AKB Quyet *********************************************

                .strAvatar = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath & .strAvatar
                objCardDetail.CardImageLocation = .strAvatar


                If basCommon.fncIsFhead(intID) Then objCardDetail.IsHead = True

            End With

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillMemberDetail", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xAlignX
    ''' </summary>
    ''' <remarks>Align X coordinate</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xAlignX()
        Try
            Dim objcard As usrMemberCard2
            Dim intCardID As Integer
            Dim intMaxX As Integer
            Dim intX As Integer

            'loop for each element to find max X
            For Each element As DictionaryEntry In mtblControl

                objcard = CType(element.Value, usrMemberCard2)
                intCardID = basCommon.fncCnvToInt(element.Key)

                intMaxX = xGetMaxX(intCardID)

                'If intMaxX <= objcard.Location.X Then Continue For
                'If intMaxX <= objcard.CardMidTop.X Then Continue For

                'calculate length to move
                'intX = (intMaxX - objcard.Location.X) \ 2
                intX = (intMaxX - objcard.CardMidTop.X) \ 2

                objcard.Location = New Point(objcard.Location.X + intX, objcard.Location.Y)
                objcard.CardCoor = New clsCoordinate(objcard.CardCoor.X + intX, objcard.CardCoor.Y)


            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAlignX", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xAlignY
    ''' </summary>
    ''' <remarks>Align Y coordinate</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xAlignY()

        'Dim intY() As Integer = New Integer(My.Settings.intGeneration) {}
        Dim intY() As Integer = New Integer(mintMaxGeneration) {}

        Dim intMaxH() As Integer

        Try
            Dim objcard As usrMemberCard2

            intMaxH = xGetMaxY()

            'calculate space for each level
            For i As Integer = 1 To mintMaxGeneration 'My.Settings.intGeneration
                intY(i) = intY(i - 1) + intMaxH(i - 1) + mintBottom
            Next

            'loop for each card and reset position
            For Each element As DictionaryEntry In mtblControl

                objcard = CType(element.Value, usrMemberCard2)

                objcard.Location = New Point(objcard.Location.X, intY(objcard.CardLevel))
                objcard.CardCoor = New clsCoordinate(objcard.CardCoor.X, intY(objcard.CardLevel))

            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAlignY", ex)
        Finally
            Erase intY
            Erase intMaxH
        End Try
    End Sub


    ''' <summary>
    ''' xGetMaxX
    ''' </summary>
    ''' <param name="intID">Member id</param>
    ''' <returns></returns>
    ''' <remarks>Get Max X to align control</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xGetMaxX(ByVal intID As Integer) As Integer

        Dim intMaxX As Integer = 0
        Dim lstKid As List(Of Integer)

        Try
            Dim objCard As usrMemberCard2
            Dim intTemp As Integer

            mintLv += 1

            'limit level
            If mintLv > mintMaxGeneration Then 'My.Settings.intGeneration Then
                mintLv -= 1
                Exit Function
            End If

            objCard = CType(mtblControl.Item(intID), usrMemberCard2)

            'prevent unknown exception
            If objCard Is Nothing Then
                mintLv -= 1
                Exit Function
            End If

            'If intMaxX < objCard.Location.X Then intMaxX = objCard.Location.X
            If intMaxX < objCard.CardMidTop.X Then intMaxX = objCard.CardMidTop.X

            'draw child
            'lstKid = basCommon.fncGetKidList(intID, clsEnum.emRelation.NATURAL)
            lstKid = basCommon.fncGetKidList(intID)

            'draw each  child
            If lstKid.Count > 0 Then

                intTemp = 0

                For i As Integer = 0 To lstKid.Count - 1

                    'If mintLv > My.Settings.intGeneration Then Continue For
                    intTemp = xGetMaxX(lstKid(i))
                    If intMaxX < intTemp Then intMaxX = intTemp

                Next

            End If

            mintLv -= 1

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetMaxX", ex)
        Finally
            lstKid = Nothing
        End Try

        Return intMaxX

    End Function


    ''' <summary>
    ''' xGetMaxY
    ''' </summary>
    ''' <returns>Array of max Height each level</returns>
    ''' <remarks>Get max Y to align control</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xGetMaxY() As Integer()

        'Dim intHeight() As Integer = New Integer(My.Settings.intGeneration) {}
        Dim intHeight() As Integer = New Integer(mintMaxGeneration) {}

        Try
            Dim objcard As usrMemberCard2

            For Each element As DictionaryEntry In mtblControl

                objcard = CType(element.Value, usrMemberCard2)

                If intHeight(objcard.CardLevel) < objcard.Height Then intHeight(objcard.CardLevel) = objcard.Height

            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetMaxY", ex)
        End Try

        Return intHeight

    End Function


    ''' <summary>
    ''' xAddCtrl2Panel
    ''' </summary>
    ''' <remarks>add control to panel</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xAddCtrl2Panel()

        Try
            Dim objcard As usrMemberCard2

            'add control to panel
            For Each element As DictionaryEntry In mtblControl
                Application.DoEvents()

                objcard = CType(element.Value, usrMemberCard2)
                mpnDraw.Controls.Add(objcard)
                Application.DoEvents()
            Next

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xAddCtrl2Panel", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xSetScrollView
    ''' </summary>
    ''' <param name="intRootID">Member id</param>
    ''' <remarks>View a member</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xSetScrollView(ByVal intRootID As Integer, Optional ByVal blnSetSelected As Boolean = False) As Boolean

        xSetScrollView = False

        Try
            Dim objCard As usrMemberCard2
            Dim dicHusWif As Dictionary(Of Integer, String)
            Dim intCount As Integer = 0

            Dim intX As Integer
            Dim intY As Integer

            'if member is not in the list (member may be husband or wife)
            If Not mtblControl.ContainsKey(intRootID) Then

                'get husband/wife list
                dicHusWif = basCommon.fncGetHusWifeList(intRootID)

                'has no husband/wife -> exit function
                If dicHusWif.Count < 1 Then Exit Function

                'search if husband or wife is in the list
                For Each intMemID As Integer In dicHusWif.Keys

                    'if hus/wif is not in the list, continue searching, if found, exit looping
                    If Not mtblControl.ContainsKey(intMemID) Then
                        intCount = intCount + 1
                        Continue For
                    Else
                        intRootID = intMemID
                        Exit For
                    End If

                Next

                'reach the end of list but not found -> exit
                If intCount >= dicHusWif.Count Then Exit Function

            End If

            objCard = CType(mtblControl.Item(intRootID), usrMemberCard2)


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
                ''reset previous card
                'If mobjTempSelectedCard IsNot Nothing Then mobjTempSelectedCard.CardSelected = False
                'objCard.CardSelected = True

                ''store this card
                'mobjTempSelectedCard = objCard
                ''mlstSelectedCtrl.Clear()
                'mlstSelectedCtrl.Add(mobjTempSelectedCard)

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
    Private Function xSetSelected(ByVal objCard As usrMemberCard2) As Boolean

        xSetSelected = False

        Try
            'reset previous card
            If mobjTempSelectedCard IsNot Nothing Then mobjTempSelectedCard.CardSelected = False
            objCard.CardSelected = True

            'store this card
            mobjTempSelectedCard = objCard
            'mlstSelectedCtrl.Clear()
            mlstSelectedCtrl.Add(mobjTempSelectedCard)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetSelected", ex)
        End Try

    End Function


    ''' <summary>
    ''' xDrawConnector
    ''' </summary>
    ''' <remarks>Draw Lines</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xDrawConnector()

        Dim tblRel As DataTable = Nothing

        Try
            Dim intID1 As Integer
            Dim intID2 As Integer
            Dim intRel As Integer
            Dim objCard1 As usrMemberCard2
            Dim objCard2 As usrMemberCard2

            'clear line
            xClearLine()

            'graphic instance from panel
            mtblRel = gobjDB.fncGetRel()

            'catch when all member has no relationship
            If mtblRel Is Nothing Then Exit Sub

            'loop for all member to draw connector
            For i As Integer = 0 To mtblRel.Rows.Count - 1
                Application.DoEvents()
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

                objCard1 = CType(mtblControl.Item(intID1), usrMemberCard2)
                objCard2 = CType(mtblControl.Item(intID2), usrMemberCard2)

                'card2 is father of card1 so it's upper card
                xDrawConnector(objCard2, objCard1)
                Application.DoEvents()
            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawConnector", ex)
        Finally
            If tblRel IsNot Nothing Then tblRel.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' xDrawConnector
    ''' </summary>
    ''' <param name="objUpperCard">high card</param>
    ''' <param name="objLowerCard">low card</param>
    ''' <remarks>Draw Lines</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xDrawConnector(ByVal objUpperCard As usrMemberCard2, ByVal objLowerCard As usrMemberCard2)

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
            basCommon.fncSaveErr(mcstrClsName, "xDrawConnector", ex)
        End Try

    End Sub


    ''' <summary>
    ''' xCardDoubleClick
    ''' </summary>
    ''' <param name="intMemID">member id be clicked</param>
    ''' <param name="objCardDetail">card be clicked</param>
    ''' <remarks>handle double click on card</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xCardDoubleClick(ByVal intMemID As Integer, ByVal objCardDetail As usrMemberDetail)

        Dim tblData As DataTable = Nothing

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

            'redraw card
            xReDraw(intMemID, objCardDetail)

            'member is edited, raise event for refreshing
            RaiseEvent evnRefresh(intMemID, True)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCardDoubleClick", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' xCardClick
    ''' </summary>
    ''' <param name="intMemID">member id be clicked</param>
    ''' <param name="objCardDetail">card be clicked</param>
    ''' <remarks>handle click on card</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xCardClick(ByVal intMemID As Integer, ByVal objCardDetail As usrMemberDetail)

        Dim tblData As DataTable = Nothing

        Try
            If intMemID <= basConst.gcintNO_MEMBER Then Exit Sub

            Dim objCard As usrMemberCard2
            Dim dicHusWif As Dictionary(Of Integer, String)
            Dim intCount As Integer = 0
            Dim intSelectedID As Integer = intMemID

            'if member is not in the list (member may be husband or wife)
            If Not mtblControl.ContainsKey(intMemID) Then

                'get husband/wife list
                dicHusWif = basCommon.fncGetHusWifeList(intMemID)

                'has no husband/wife -> exit function
                If dicHusWif.Count < 1 Then Exit Sub

                'search if husband or wife is in the list
                For Each intID As Integer In dicHusWif.Keys

                    'if hus/wif is not in the list, continue searching, if found, exit looping
                    If Not mtblControl.ContainsKey(intID) Then
                        intCount = intCount + 1
                        Continue For
                    Else
                        intMemID = intID
                        Exit For
                    End If

                Next

                'reach the end of list but not found -> exit
                If intCount >= dicHusWif.Count Then Exit Sub

            End If

            objCard = CType(mtblControl.Item(intMemID), usrMemberCard2)

            ''reset previous card
            'If mobjTempSelectedCard IsNot Nothing Then mobjTempSelectedCard.CardSelected = False
            'objCard.CardSelected = True

            ''store this card
            'mobjTempSelectedCard = objCard
            ''mlstSelectedCtrl.Clear()
            'mlstSelectedCtrl.Add(mobjTempSelectedCard)

            xSetSelected(objCard)

            'raise event for set selected user in quick search list
            RaiseEvent evnCardClicked(intSelectedID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCardClick", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' xReDraw
    ''' </summary>
    ''' <param name="intMemID">member id be redrawn</param>
    ''' <param name="objCardDetail">card be redrawn</param>
    ''' <remarks>redraw a detail card</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xReDraw(ByVal intMemID As Integer, ByVal objCardDetail As usrMemberDetail)

        Dim tblData As DataTable = Nothing

        Try
            'get new data and redraw this card
            tblData = gobjDB.fncGetMemberMain(intMemID)
            If tblData Is Nothing Then Exit Sub

            xFillMemberDetail(intMemID, objCardDetail, tblData, False)
            objCardDetail.CardContainer.fncAlignControls()

            're-align Y
            xAlignY()
            xDrawConnector()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xReDraw", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Sub


    ''' <summary>
    ''' xHandlerNotDraw - add a card to list of not drawing card
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

            'clear line for drawing new one
            xClearLine()
            mpnDraw.Controls.Clear()

            xInit(True)

            xRecusiveDraw(mintStartID, True, basConst.gcintNONE_VALUE, mintX)

            'align Y-coodinate
            xAlignX()
            xAlignY()

            xAddCtrl2Panel()
            xDrawConnector()
            'xSetScrollView(mintStartID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xHandlerNotDraw", ex)
        End Try
    End Sub


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
            mintLv += 1

            'limit level
            If mintLv > mintMaxGeneration Then 'My.Settings.intGeneration Then
                mintLv -= 1
                Exit Function
            End If

            'remove from list
            If mtblControl.ContainsKey(intId) Then mtblControl.Remove(intId)
            If Not mlstNotDraw.Contains(intId) Then mlstNotDraw.Add(intId)

            'draw child
            'lstKid = basCommon.fncGetKidList(intId, clsEnum.emRelation.NATURAL)
            lstKid = basCommon.fncGetKidList(intId)

            ''draw each  child
            If lstKid.Count > 0 Then

                For i As Integer = 0 To lstKid.Count - 1

                    'xRecusiveDraw(lstKid(i))
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


    ''' <summary>
    ''' xClearLine - clear lines
    ''' </summary>
    ''' <remarks>clear lines</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Sub xClearLine()
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

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClearLine", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xResetLocation - relocate card
    ''' </summary>
    ''' <param name="intID">card id</param>
    ''' <param name="intX">X coordinate</param>
    ''' <param name="intY">Y coordinate</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xResetLocation(ByVal intID As Integer, ByVal intX As Integer, ByVal intY As Integer) As Boolean

        xResetLocation = False

        Try
            Dim objCard As usrMemberCard2

            If mlstNotDraw.Contains(intID) Then Exit Function

            If Not mtblControl.ContainsKey(intID) Then Exit Function

            objCard = CType(mtblControl.Item(intID), usrMemberCard2)

            'reset location and width
            objCard.Location = New Point(intX, intY)
            objCard.CardCoor = New clsCoordinate(intX, intY)
            mintCardWidth = objCard.Width

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClearLine", ex)
        End Try

    End Function


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

    'Public Function fncGetMaxWidthInMM() As Integer

    '    Dim objGraphic As Graphics = mpnDraw.CreateGraphics()
    '    Dim intDPI As Integer = CInt(Me.MaxWidth / objGraphic.DpiX * 25.4F)

    '    objGraphic = Nothing

    '    Return intDPI

    'End Function

    'Public Function fncGetMaxHeightInMM() As Integer

    '    Dim objGraphic As Graphics = mpnDraw.CreateGraphics()
    '    Dim intDPI As Integer = CInt(Me.MaxHeight / objGraphic.DpiY * 25.4F)

    '    objGraphic = Nothing
    '    Return intDPI

    'End Function

#Region "Add By: 2017/07/27 AKB Nguyen Thanh Tung"

    '   ******************************************************************
    '		FUNCTION   : Constructor
    '		MEMO       : Initialize Class
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Sub New()
        Try

            mtblDrawLv = New DataTable()
            mtblDrawLv.Columns.Add("Level", System.Type.GetType("System.Int32"))
            mtblDrawLv.Columns.Add("ID", System.Type.GetType("System.Int32"))

            mlstSelectedCtrl = New List(Of usrMemCardBase)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "New", ex)
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
    '		PARAMS     : ARG1(IN) - Integer - ID Start Draw
    '                    ARG2(IN) - DataTable - Data Member
    '                    ARG3(IN) - Integer - Max Generation
    '                    ARG4(OUT) - List(Of Integer) - List Root ID Max Generation
    '		MEMO       : Draw Tree
    '		CREATE     : 2017/07/25 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncDrawPDF(ByVal intStartID As Integer,
                               ByVal tblData As DataTable,
                               ByVal intMaxGeneration As Integer,
                               ByRef lstRootIDGenerationMax As List(Of Integer)) As Boolean

        fncDrawPDF = False

        Try
            mintMaxGeneration = intMaxGeneration

            If tblData Is Nothing Then
                Exit Function
            End If

            Me.mtblData = tblData
            Me.mintStartID = intStartID

            'Me.mblnIsSmallCard = blnIsSmallCard
            mblnIsSmallCard = True
            If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then mblnIsSmallCard = False

            Call xClearDataDrawPDF()
            xInit(False)

            'do recusive to add member to table
            xRecusiveDrawPDF(intStartID, False, basConst.gcintNONE_VALUE, mintX, lstRootIDGenerationMax)

            'align Y-coodinate
            xAlignX()
            xAlignY()

            xDrawConnectorPDF()

            fncDrawPDF = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncDrawPDF", ex)
            Throw
        Finally

        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xClearDataDrawPDF
    '		MEMO       : Clear data draw
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Sub xClearDataDrawPDF()
        Try

            Dim objCard As usrMemberCard2
            Dim objCardDetail As usrMemberDetail

            If mtblDetailCard IsNot Nothing Then

                For Each element As DictionaryEntry In mtblDetailCard
                    objCardDetail = CType(element.Value, usrMemberDetail)
                    objCardDetail.Dispose()
                Next

            End If

            If mtblControl IsNot Nothing Then

                For Each element As DictionaryEntry In mtblControl
                    objCard = CType(element.Value, usrMemberCard2)
                    objCard.Dispose()
                Next

            End If

            If mtblDrawLv IsNot Nothing Then mtblDrawLv.Rows.Clear() 'reset 

            xClearLine()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClearDataDrawPDF", ex)
            Throw
        End Try
    End Sub

    Private Function xRecusiveDrawPDF(ByVal intStartID As Integer,
                                      ByVal blnRedraw As Boolean,
                                      ByVal intParentID As Integer,
                                      ByVal intX As Integer,
                                      ByRef lstRootIDGenerationMax As List(Of Integer)) As stCardSize

        Dim intSize As stCardSize
        xRecusiveDrawPDF = intSize

        Dim lstKid As List(Of Integer)

        Try
            Dim intHeight As Integer
            'Dim i As Integer

            mintLv += 1

            'limit level
            If mintLv > mintMaxGeneration Then 'My.Settings.intGeneration Then
                mintLv -= 1
                Exit Function
            End If

            If Not blnRedraw Then
                'draw current member
                intSize = xCreateCardPDF(intStartID, intX, mintY, mintLv, intParentID)
            Else
                If Not xResetLocation(intStartID, intX, mintY) Then
                    mintLv -= 1
                    Exit Function
                End If
            End If

            mintY += intHeight + mintBottom

            'draw child
            'lstKid = basCommon.fncGetKidList(intStartID, clsEnum.emRelation.NATURAL)   'get natural son only
            ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
            lstKid = New List(Of Integer)
            Dim stCard As stCardInfo = fncGetMemberInfo(intStartID, mtblData)

            If My.Settings.intSelectedTypeShowTree <> CInt(clsEnum.emTypeShowTree.OnlyShowMember) _
            OrElse stCard.stBasicInfo.intGender = clsEnum.emGender.MALE Then
                lstKid = basCommon.fncGetKidList(intStartID)
            End If
            'lstKid = basCommon.fncGetKidList(intStartID)                                'get both natural and adopted
            ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------

            ''draw each  child
            If lstKid.Count > 0 Then

                If mintLv = mintMaxGeneration - 1 Then
                    If IsNothing(lstRootIDGenerationMax) Then lstRootIDGenerationMax = New List(Of Integer)
                    lstRootIDGenerationMax.AddRange(lstKid)
                End If

                Dim stTemp As stCardSize

                For i As Integer = 0 To lstKid.Count - 1

                    ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
                    If My.Settings.intSelectedTypeShowTree = clsEnum.emTypeShowTree.OnlyShowMale Then
                        Dim stChild As stCardInfo = fncGetMemberInfo(lstKid(i), mtblData)
                        If stChild.stBasicInfo.intGender = clsEnum.emGender.FEMALE Then Continue For
                    End If
                    ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------

                    Application.DoEvents()
                    stTemp = xRecusiveDrawPDF(lstKid(i), blnRedraw, intStartID, intX, lstRootIDGenerationMax)

                    'do not increase x if the last child reached.
                    'If mintLv < My.Settings.intGeneration Then If i < lstKid.Count - 1 Then mintX += mintCardWidth + mintRight
                    If mintLv < mintMaxGeneration Then
                        If i < lstKid.Count - 1 Then

                            'the code below for determinng next X value
                            'it can be (brother X-value + width) or (brother's last child X-value + width)
                            Dim intTemp As Integer

                            intX += stTemp.intWidth + mintRight                     'brother X-value + width
                            intTemp = mintLastX + mintLastWidth + mintRight         'brother's last child X-value + width

                            If intTemp > intX Then intX = intTemp

                        End If
                    End If

                    Application.DoEvents()

                Next

            End If


            mintY -= intHeight + mintBottom
            mintLv -= 1

            Return intSize

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRecusiveDraw", ex)
        Finally
            lstKid = Nothing
        End Try

    End Function

    Private Function xCreateCardPDF(ByVal intID As Integer,
                                    ByVal intX As Integer,
                                    ByVal intY As Integer,
                                    ByVal intLevel As Integer,
                                    ByVal intParentID As Integer) As stCardSize

        Dim stSize As stCardSize
        Dim intCardHeight As Integer = 0
        Dim intLv(1) As Object

        Try
            Dim objCard As usrMemberCard2
            Dim objDictSpouseList As Dictionary(Of Integer, String)
            Dim objDetailCard As usrMemberDetail

            objCard = New usrMemberCard2()

            'add current member
            objDetailCard = xCreateCardDetail(intID, objCard)
            objCard.fncAddItem(objDetailCard)
            objCard.Visible = True
            mtblControl.Add(intID, objCard)

            'and spouse
            ' ▽ 2018/02/07 AKB Nguyen Thanh Tung --------------------------------
            If My.Settings.intSelectedTypeShowTree = clsEnum.emTypeShowTree.All Then
                objDictSpouseList = basCommon.fncGetHusWifeList(intID)
            Else
                objDictSpouseList = New Dictionary(Of Integer, String)
            End If
            'objDictSpouseList = basCommon.fncGetHusWifeList(intID)
            ' △ 2018/02/07 AKB Nguyen Thanh Tung --------------------------------

            If objDictSpouseList.Count > 0 Then

                For Each element As KeyValuePair(Of Integer, String) In objDictSpouseList

                    Dim objSpouseCard As usrMemberDetail
                    Dim intSpouseID As Integer

                    intSpouseID = basCommon.fncCnvToInt(element.Key)
                    objSpouseCard = xCreateCardDetailPDF(intSpouseID, objCard)
                    'mtblControl.Add(intSpouseID, objSpouseCard)
                    objCard.fncAddItem(objSpouseCard)

                Next

            End If

            'set position of the card
            objCard.Location = New Point(intX, intY)
            objCard.CardCoor = New clsCoordinate(intX, intY)
            objCard.CardID = intID
            objCard.CardLevel = intLevel
            objCard.ParentID = intParentID

            'store current drawing level
            objCard.DrawLv = mintLv
            intLv(0) = mintLv
            intLv(1) = intID
            mtblDrawLv.Rows.Add(intLv)

            If mblnIsSmallCard Then objCard.CardSize = clsEnum.emCardSize.SMALL

            'get card width 
            stSize.intWidth = objCard.Width
            stSize.intHeight = objCard.Height

            'temporary store last card's value
            mintLastX = intX
            mintLastWidth = objCard.Width

            'store max height
            'If mintMaxH(intLevel) < intCardHeight Then mintMaxH(intLevel) = intCardHeight

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCreateCardPDF", ex)
        Finally
            intLv = Nothing
        End Try

        Return stSize

    End Function

    Private Function xCreateCardDetailPDF(ByVal intID As Integer, ByVal objContainer As usrMemberCard2) As usrMemberDetail

        Dim objCardDetail As usrMemberDetail = Nothing

        Try
            'create new
            objCardDetail = New usrMemberDetail(intID)

            'fill card
            xFillMemberDetail(intID, objCardDetail, mtblData)
            objCardDetail.CardContainer = objContainer
            objCardDetail.fncResize()

            'add to list
            Me.mtblDetailCard.Add(intID, objCardDetail)


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCreateCardDetailPDF", ex)
        End Try

        Return objCardDetail

    End Function

    '   ******************************************************************
    '　　　	FUNCTION   : xDrawConnectorPDF
    '      	MEMO       : Draw Line
    '      	CREATE     : 2017/07/27 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawConnectorPDF()

        Dim tblRel As DataTable = Nothing

        Try
            Dim intID1 As Integer
            Dim intID2 As Integer
            Dim intRel As Integer
            Dim objCard1 As usrMemberCard2
            Dim objCard2 As usrMemberCard2

            'clear line
            xClearLine()

            'catch when all member has no relationship
            If mtblRel Is Nothing Then Exit Sub

            'loop for all member to draw connector
            For i As Integer = 0 To mtblRel.Rows.Count - 1
                Application.DoEvents()
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

                objCard1 = CType(mtblControl.Item(intID1), usrMemberCard2)
                objCard2 = CType(mtblControl.Item(intID2), usrMemberCard2)

                'card2 is father of card1 so it's upper card
                xDrawConnectorPDF(objCard2, objCard1)
                Application.DoEvents()
            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawConnectorPDF", ex)
        Finally
            If tblRel IsNot Nothing Then tblRel.Dispose()
        End Try

    End Sub

    '   ******************************************************************
    '　　　	FUNCTION   : xDrawConnectorPDF
    '      	MEMO       : Draw Line
    '		PARAMS     : ARG1(IN) - usrMemberCard2 - High Card
    '                    ARG2(IN) - usrMemberCard2 - Low Card
    '      	CREATE     : 2017/07/27 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawConnectorPDF(ByVal objUpperCard As usrMemberCard2, ByVal objLowerCard As usrMemberCard2)

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
            basCommon.fncSaveErr(mcstrClsName, "xDrawConnectorPDF(usrMemberCard2,usrMemberCard2)", ex)
        End Try

    End Sub

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
#End Region
End Class
