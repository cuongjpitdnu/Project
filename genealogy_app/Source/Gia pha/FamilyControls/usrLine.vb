Option Explicit On
Option Strict Off
'   ****************************************************************** 
'      TITLE      : MEMBER CARD
'　　　FUNCTION   :  
'      MEMO       :  
'      CREATE     : 2012/02/14　AKB　Quyet 
'      UPDATE     :  
' 
'           2012 AKB SOFTWARE 
'   ******************************************************************
Imports System.Runtime.InteropServices

''' <summary>
''' CUSTOM LINE CLASS
''' </summary>
''' <remarks></remarks>
''' <Create>2012/02/14  AKB Quyet</Create>
Public Class usrLine

    '    Private Const mcstrClsName As String = "clsDrawFamilyTree"                  'class name

    '    Private mintLength As Integer                           'length
    '    Private mintWeight As Integer                           'border weight

    '    Private memLineType As clsEnum.emLineType               'line type
    '    Private memLineDirection As clsEnum.emLineDirection     'direction
    '    Private memAnchor As clsEnum.emCardPoint                '
    '    Private mintOffset As Integer = 0                       '

    '    Private mobjCardLeft As usrMemCardBase                  'left card
    '    Private mobjCardRight As usrMemCardBase                 'right card

    '    Private mobjLine1 As usrLine                            'line 1 to connect
    '    Private mobjLine2 As usrLine                            'line 2 to connect

    '    Private mintOffsetX As Integer
    '    Private mintOffsetY As Integer
    '    Private mclsCoordinate As clsCoordinateinate


    '    Public Event evnControlMove()                           'line moved event

    '#Region "PROPERTIES"


    '    ''' <summary>
    '    ''' LineType
    '    ''' </summary>
    '    ''' <value>clsEnum.emLineType</value>
    '    ''' <returns>clsEnum.emLineType</returns>
    '    ''' <remarks>Gets or Sets line type</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Property LineType() As clsEnum.emLineType

    '        Get
    '            Return Me.memLineType
    '        End Get

    '        Set(ByVal value As clsEnum.emLineType)

    '            'exit if there is nothing changes
    '            'If Me.memLineType = value Then Exit Property

    '            Me.memLineType = value

    '            'reset height and width
    '            If Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL Then

    '                Select Case (memLineType)
    '                    Case clsEnum.emLineType.SINGLE_LINE
    '                        Me.Height = 1
    '                    Case clsEnum.emLineType.DOUBLE_LINE
    '                        Me.Height = 6
    '                End Select

    '            Else

    '                Select Case (memLineType)
    '                    Case clsEnum.emLineType.SINGLE_LINE
    '                        Me.Width = 1
    '                    Case clsEnum.emLineType.DOUBLE_LINE
    '                        Me.Width = 6
    '                End Select

    '            End If

    '        End Set

    '    End Property


    '    ''' <summary>
    '    ''' LineDirection
    '    ''' </summary>
    '    ''' <value>clsEnum.emLineDirection</value>
    '    ''' <returns>clsEnum.emLineDirection</returns>
    '    ''' <remarks>Gets or Sets line direction</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Property LineDirection() As clsEnum.emLineDirection

    '        Get
    '            Return Me.memLineDirection
    '        End Get

    '        Set(ByVal value As clsEnum.emLineDirection)

    '            'exit if there is nothing changes
    '            If Me.memLineDirection = value Then Exit Property

    '            Me.memLineDirection = value

    '            'width <-> height
    '            Dim intTemp As Integer = Me.Width
    '            Me.Width = Me.Height
    '            Me.Height = intTemp

    '        End Set

    '    End Property


    '    ''' <summary>
    '    ''' LineLength
    '    ''' </summary>
    '    ''' <value>Integer</value>
    '    ''' <returns>Integer</returns>
    '    ''' <remarks>Gets or Sets length</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Property LineLength() As Integer
    '        Get
    '            Return mintLength
    '        End Get
    '        Set(ByVal value As Integer)

    '            mintLength = value

    '            Select Case memLineDirection
    '                Case clsEnum.emLineDirection.HORIZONTAL
    '                    Me.Width = mintLength

    '                Case clsEnum.emLineDirection.VERTICAL
    '                    Me.Height = mintLength
    '            End Select

    '        End Set
    '    End Property


    '    ''' <summary>
    '    ''' LineWeight
    '    ''' </summary>
    '    ''' <value>Integer</value>
    '    ''' <returns>Integer</returns>
    '    ''' <remarks>Gets or Sets border weight</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Property LineWeight() As Integer
    '        Get
    '            Return mintWeight
    '        End Get
    '        Set(ByVal value As Integer)

    '            mintWeight = value

    '            Select Case memLineDirection
    '                Case clsEnum.emLineDirection.HORIZONTAL
    '                    Me.Height = mintWeight

    '                Case clsEnum.emLineDirection.VERTICAL
    '                    Me.Width = mintWeight
    '            End Select

    '        End Set
    '    End Property


    '    ''' <summary>
    '    ''' LineColor
    '    ''' </summary>
    '    ''' <value>Color</value>
    '    ''' <returns>Color</returns>
    '    ''' <remarks>Gets or Sets line color</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Property LineColor() As Color

    '        Set(ByVal value As Color)

    '            Me.BackColor = value

    '        End Set

    '        Get
    '            Return Me.BackColor
    '        End Get

    '    End Property

    '    Public Property LineCoor() As clsCoordinateinate
    '        Get
    '            Return mclsCoordinate
    '        End Get
    '        Set(ByVal value As clsCoordinateinate)

    '            mclsCoordinate = value

    '        End Set
    '    End Property
    '#End Region


    '    ''' <summary>
    '    ''' CONSTRUCTOR
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Sub New()

    '        ' This call is required by the Windows Form Designer.
    '        InitializeComponent()

    '        ' Add any initialization after the InitializeComponent() call.
    '        Me.LineType = clsEnum.emLineType.SINGLE_LINE
    '        Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL
    '        Me.LineLength = 100
    '        Me.LineWeight = 1
    '        Me.AutoValidate = Windows.Forms.AutoValidate.Disable
    '        Me.ResizeRedraw = False
    '        Me.DoubleBuffered = True
    '        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)

    '    End Sub


    '    ''' <summary>
    '    ''' CONSTRUCTOR
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Sub New(ByVal emLineType As clsEnum.emLineType, _
    '                   ByVal emLineDirection As clsEnum.emLineDirection, _
    '                   ByVal intLength As Integer)

    '        ' This call is required by the Windows Form Designer.
    '        InitializeComponent()

    '        ' Add any initialization after the InitializeComponent() call.
    '        Me.LineType = emLineType
    '        Me.LineDirection = emLineDirection
    '        Me.LineLength = intLength
    '        Me.LineWeight = 1

    '    End Sub



    '#Region "FORM's EVENTs"


    '    ''' <summary>
    '    ''' fncAddVerticalLine
    '    ''' </summary>
    '    ''' <param name="objCard">card to attach</param>
    '    ''' <param name="emAnchor">point on card</param>
    '    ''' <remarks>Add a vertical line</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Sub fncAddVerticalLine(ByVal objCard As usrMemCardBase, _
    '                                  ByVal emAnchor As clsEnum.emCardPoint, _
    '                                  Optional ByVal intOffsetX As Integer = 0, _
    '                                  Optional ByVal intOffsetY As Integer = 0)

    '        Try
    '            'always be vertical
    '            Me.LineDirection = clsEnum.emLineDirection.VERTICAL
    '            'Me.LineType = clsEnum.emLineType.SINGLE_LINE

    '            mintOffsetX = intOffsetX
    '            mintOffsetY = intOffsetY

    '            ''base on anchor, set position
    '            'Select Case emAnchor
    '            '    Case clsEnum.emCardPoint.MID_BOTTOM
    '            '        Me.Location = objCard.CardMidBottom
    '            '        Me.memAnchor = clsEnum.emCardPoint.MID_BOTTOM
    '            '        AddHandler objCard.evnCardMove, AddressOf xCardMove

    '            '    Case clsEnum.emCardPoint.MID_TOP
    '            '        Me.Location = New Point(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)
    '            '        Me.memAnchor = clsEnum.emCardPoint.MID_TOP
    '            '        AddHandler objCard.evnCardMove, AddressOf xCardMove

    '            '    Case clsEnum.emCardPoint.MID_RIGHT
    '            '        Me.Location = New Point(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)
    '            '        Me.memAnchor = clsEnum.emCardPoint.MID_RIGHT
    '            '        AddHandler objCard.evnCardMove, AddressOf xCardMove

    '            '    Case clsEnum.emCardPoint.MID_LEFT
    '            '        Me.Location = New Point(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)
    '            '        Me.memAnchor = clsEnum.emCardPoint.MID_LEFT
    '            '        AddHandler objCard.evnCardMove, AddressOf xCardMove

    '            'End Select
    '            'base on anchor, set position
    '            Select Case emAnchor
    '                Case clsEnum.emCardPoint.MID_BOTTOM
    '                    Me.LineCoor = objCard.CardMidBottom
    '                    Me.Location = New Point(objCard.CardMidBottom.X, objCard.CardMidBottom.Y)
    '                    Me.memAnchor = clsEnum.emCardPoint.MID_BOTTOM
    '                    AddHandler objCard.evnCardMove, AddressOf xCardMove
    '                Case clsEnum.emCardPoint.MID_TOP
    '                    Me.LineCoor = New clsCoordinateinate(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)
    '                    Me.Location = New Point(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)
    '                    Me.memAnchor = clsEnum.emCardPoint.MID_TOP
    '                    AddHandler objCard.evnCardMove, AddressOf xCardMove

    '                Case clsEnum.emCardPoint.MID_RIGHT
    '                    Me.LineCoor = New clsCoordinateinate(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)
    '                    Me.Location = New Point(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)
    '                    Me.memAnchor = clsEnum.emCardPoint.MID_RIGHT
    '                    AddHandler objCard.evnCardMove, AddressOf xCardMove

    '                Case clsEnum.emCardPoint.MID_LEFT
    '                    Me.LineCoor = New clsCoordinateinate(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)
    '                    Me.Location = New Point(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)
    '                    Me.memAnchor = clsEnum.emCardPoint.MID_LEFT
    '                    AddHandler objCard.evnCardMove, AddressOf xCardMove

    '            End Select

    '        Catch ex As Exception
    '            basCommon.fncSaveErr(mcstrClsName, "fncAddParent", ex)
    '        End Try

    '    End Sub


    '    ''' <summary>
    '    ''' fncAddSpouseLine
    '    ''' </summary>
    '    ''' <param name="objCardLeft">Card 1</param>
    '    ''' <param name="objCardRight">Card 2</param>
    '    ''' <param name="intOffSet">Space between 2 card</param>
    '    ''' <remarks>Add spouse (Paralel line)</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Sub fncAddSpouseLine(ByVal objCardLeft As usrMemberCard1, ByVal objCardRight As usrMemberCard1, ByVal intOffSet As Integer)
    '        Try
    '            'always be horizontal
    '            Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL

    '            Me.mobjCardLeft = objCardLeft
    '            Me.mobjCardRight = objCardRight
    '            Me.mintOffset = intOffSet

    '            xLocateSpouseLine(Nothing)


    '            AddHandler mobjCardLeft.evnCardMove, AddressOf xLocateSpouseLine
    '            AddHandler mobjCardRight.evnCardMove, AddressOf xLocateSpouseLine

    '        Catch ex As Exception
    '            basCommon.fncSaveErr(mcstrClsName, "fncAddSpouseLine", ex)
    '        End Try
    '    End Sub

    '    ''' <summary>
    '    ''' fncAddSpouseLine
    '    ''' </summary>
    '    ''' <param name="objCardLeft">Card 1</param>
    '    ''' <param name="objCardRight">Card 2</param>
    '    ''' <param name="intOffSet">Space between 2 card</param>
    '    ''' <remarks>Add spouse (Paralel line)</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Sub fncAddSpouseLine(ByVal objCardLeft As usrMemberCard3, ByVal objCardRight As usrMemberCard3, ByVal intOffSet As Integer)
    '        Try
    '            'always be horizontal
    '            Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL

    '            Me.mobjCardLeft = objCardLeft
    '            Me.mobjCardRight = objCardRight
    '            Me.mintOffset = intOffSet

    '            xLocateSpouseLine(Nothing)


    '            AddHandler mobjCardLeft.evnCardMove, AddressOf xLocateSpouseLine
    '            AddHandler mobjCardRight.evnCardMove, AddressOf xLocateSpouseLine

    '        Catch ex As Exception
    '            basCommon.fncSaveErr(mcstrClsName, "fncAddSpouseLine", ex)
    '        End Try
    '    End Sub


    '    ''' <summary>
    '    ''' fncAddHorizontalLine
    '    ''' </summary>
    '    ''' <param name="objFather">Line 1</param>
    '    ''' <param name="objMother">Line 2</param>
    '    ''' <remarks>Add horizontal Line, connect 2 line</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Public Sub fncAddHorizontalLine(ByVal objFather As usrLine, ByVal objMother As usrLine)
    '        Try

    '            'always be horizontal
    '            Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL

    '            Me.mobjLine1 = objFather
    '            Me.mobjLine2 = objMother

    '            xLocateHorzLine()

    '            AddHandler mobjLine1.evnControlMove, AddressOf xLocateHorzLine
    '            AddHandler mobjLine2.evnControlMove, AddressOf xLocateHorzLine

    '        Catch ex As Exception
    '            basCommon.fncSaveErr(mcstrClsName, "fncAddHorizontalLine", ex)
    '        End Try
    '    End Sub


    '    ''' <summary>
    '    ''' xCardMove
    '    ''' </summary>
    '    ''' <param name="objCard">usrMemCardBase</param>
    '    ''' <remarks>Handle card move event</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Private Sub xCardMove(ByVal objCard As usrMemCardBase)
    '        Try
    '            ''base on anchor, reset position
    '            'Select Case memAnchor
    '            '    Case clsEnum.emCardPoint.MID_BOTTOM
    '            '        Me.Location = objCard.CardMidBottom

    '            '    Case clsEnum.emCardPoint.MID_TOP
    '            '        Me.Location = New Point(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)

    '            '    Case clsEnum.emCardPoint.MID_RIGHT
    '            '        Me.Location = New Point(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)

    '            '    Case clsEnum.emCardPoint.MID_LEFT
    '            '        Me.Location = New Point(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)

    '            'End Select
    '            'base on anchor, reset position
    '            Select Case memAnchor
    '                Case clsEnum.emCardPoint.MID_BOTTOM
    '                    Me.LineCoor = New clsCoordinateinate(objCard.CardMidBottom.X, objCard.CardMidBottom.Y)
    '                    Me.Location = New Point(objCard.CardMidBottom.X, objCard.CardMidBottom.Y)

    '                Case clsEnum.emCardPoint.MID_TOP
    '                    Me.LineCoor = New clsCoordinateinate(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)
    '                    Me.Location = New Point(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)

    '                Case clsEnum.emCardPoint.MID_RIGHT
    '                    Me.LineCoor = New clsCoordinateinate(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)
    '                    Me.Location = New Point(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)

    '                Case clsEnum.emCardPoint.MID_LEFT
    '                    Me.LineCoor = New clsCoordinateinate(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)
    '                    Me.Location = New Point(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)

    '            End Select

    '            RaiseEvent evnControlMove()

    '        Catch ex As Exception
    '            basCommon.fncSaveErr(mcstrClsName, "xCardMove", ex)
    '        End Try
    '    End Sub


    '    ''' <summary>
    '    ''' xLocateHorzLine
    '    ''' </summary>
    '    ''' <remarks>Set location</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Private Sub xLocateHorzLine()
    '        Try
    '            Dim ptLocation As Point

    '            ptLocation = New Point(0, 0)

    '            ptLocation = mobjLine1.Location

    '            'X is the smaller X
    '            If mobjLine2.Location.X < ptLocation.X Then ptLocation.X = mobjLine2.Location.X

    '            'Y is the greater Y
    '            If mobjLine2.Location.Y > ptLocation.Y Then ptLocation.Y = mobjLine2.Location.Y

    '            'set postion of this control
    '            Me.LineLength = Math.Abs(mobjLine1.Location.X - mobjLine2.Location.X)
    '            Me.Location = ptLocation

    '        Catch ex As Exception
    '            basCommon.fncSaveErr(mcstrClsName, "xLocateHorzLine", ex)
    '        End Try
    '    End Sub


    '    ''' <summary>
    '    ''' xLocateSpouseLine
    '    ''' </summary>
    '    ''' <param name="objCard">usrMemCardBase</param>
    '    ''' <remarks>Set Location</remarks>
    '    ''' <Create>2012/02/14  AKB Quyet</Create>
    '    Private Sub xLocateSpouseLine(ByVal objCard As usrMemCardBase)
    '        Try
    '            Dim pt1 As Point
    '            Dim pt2 As Point
    '            Dim ptTemp As Point

    '            pt1 = mobjCardLeft.CardMidRight
    '            pt2 = mobjCardRight.CardMidLeft

    '            'if pt1 > pt2 => change position
    '            If pt1.X > pt2.X Then
    '                ptTemp = pt1
    '                pt1 = pt2
    '                pt2 = ptTemp
    '            End If

    '            pt1.Y += mintOffset

    '            Me.Location = pt1
    '            Me.Width = Math.Abs(pt1.X - pt2.X)

    '        Catch ex As Exception
    '            basCommon.fncSaveErr(mcstrClsName, "xLocateSpouseLine", ex)
    '        End Try
    '    End Sub


    '    '''' <summary>
    '    '''' usrLine_Move
    '    '''' </summary>
    '    '''' <param name="sender"></param>
    '    '''' <param name="e"></param>
    '    '''' <remarks>Handle line move</remarks>
    '    '''' <Create>2012/02/14  AKB Quyet</Create>
    '    'Private Sub usrLine_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Move
    '    '    Try
    '    '        RaiseEvent evnControlMove()

    '    '    Catch ex As Exception
    '    '        basCommon.fncSaveErr(mcstrClsName, "usrLine_Move", ex)
    '    '    End Try
    '    'End Sub


    '#End Region
    Private Const mcstrClsName As String = "clsDrawFamilyTree"                  'class name

    Private mintLength As Integer                           'length
    Private mintWeight As Integer                           'border weight

    Private memLineType As clsEnum.emLineType               'line type
    Private memLineDirection As clsEnum.emLineDirection     'direction
    Private memAnchor As clsEnum.emCardPoint                '
    Private mintOffset As Integer = 0                       '

    Private mobjCardLeft As usrMemCardBase                  'left card
    Private mobjCardRight As usrMemCardBase                 'right card

    Private mobjLine1 As usrLine                            'line 1 to connect
    Private mobjLine2 As usrLine                            'line 2 to connect

    Private mintOffsetX As Integer
    Private mintOffsetY As Integer

    Public Event evnControlMove()                           'line moved event
    'Private mintXCoor As Integer
    'Private mintYCoor As Integer
    Private mclsCoordinate As clsCoordinate

#Region "PROPERTIES"


    ''' <summary>
    ''' LineType
    ''' </summary>
    ''' <value>clsEnum.emLineType</value>
    ''' <returns>clsEnum.emLineType</returns>
    ''' <remarks>Gets or Sets line type</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Property LineType() As clsEnum.emLineType

        Get
            Return Me.memLineType
        End Get

        Set(ByVal value As clsEnum.emLineType)

            'exit if there is nothing changes
            'If Me.memLineType = value Then Exit Property

            Me.memLineType = value

            'reset height and width
            If Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL Then

                Select Case (memLineType)
                    Case clsEnum.emLineType.SINGLE_LINE
                        Me.Height = 1
                    Case clsEnum.emLineType.DOUBLE_LINE
                        Me.Height = 6
                End Select

            Else

                Select Case (memLineType)
                    Case clsEnum.emLineType.SINGLE_LINE
                        Me.Width = 1
                    Case clsEnum.emLineType.DOUBLE_LINE
                        Me.Width = 6
                End Select

            End If

        End Set

    End Property

    Public Property LineCoor() As clsCoordinate
        Get
            Return mclsCoordinate
        End Get
        Set(ByVal value As clsCoordinate)

            mclsCoordinate = value

        End Set
    End Property
    ''' <summary>
    ''' LineDirection
    ''' </summary>
    ''' <value>clsEnum.emLineDirection</value>
    ''' <returns>clsEnum.emLineDirection</returns>
    ''' <remarks>Gets or Sets line direction</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Property LineDirection() As clsEnum.emLineDirection

        Get
            Return Me.memLineDirection
        End Get

        Set(ByVal value As clsEnum.emLineDirection)

            'exit if there is nothing changes
            If Me.memLineDirection = value Then Exit Property

            Me.memLineDirection = value

            'width <-> height
            Dim intTemp As Integer = Me.Width
            Me.Width = Me.Height
            Me.Height = intTemp

        End Set

    End Property


    ''' <summary>
    ''' LineLength
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>Integer</returns>
    ''' <remarks>Gets or Sets length</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Property LineLength() As Integer
        Get
            Return mintLength
        End Get
        Set(ByVal value As Integer)

            mintLength = value

            Select Case memLineDirection
                Case clsEnum.emLineDirection.HORIZONTAL
                    Me.Width = mintLength

                Case clsEnum.emLineDirection.VERTICAL
                    Me.Height = mintLength
            End Select

        End Set
    End Property


    ''' <summary>
    ''' LineWeight
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>Integer</returns>
    ''' <remarks>Gets or Sets border weight</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Property LineWeight() As Integer
        Get
            Return mintWeight
        End Get
        Set(ByVal value As Integer)

            mintWeight = value

            Select Case memLineDirection
                Case clsEnum.emLineDirection.HORIZONTAL
                    Me.Height = mintWeight

                Case clsEnum.emLineDirection.VERTICAL
                    Me.Width = mintWeight
            End Select

        End Set
    End Property


    ''' <summary>
    ''' LineColor
    ''' </summary>
    ''' <value>Color</value>
    ''' <returns>Color</returns>
    ''' <remarks>Gets or Sets line color</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Property LineColor() As Color

        Set(ByVal value As Color)

            Me.BackColor = value

        End Set

        Get
            Return Me.BackColor
        End Get

    End Property


#End Region


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <remarks></remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.LineType = clsEnum.emLineType.SINGLE_LINE
        Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL
        Me.LineLength = 100
        Me.LineWeight = 1
        Me.AutoValidate = Windows.Forms.AutoValidate.Disable
        Me.ResizeRedraw = False
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)

    End Sub


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <remarks></remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Sub New(ByVal emLineType As clsEnum.emLineType, _
                   ByVal emLineDirection As clsEnum.emLineDirection, _
                   ByVal intLength As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.LineType = emLineType
        Me.LineDirection = emLineDirection
        Me.LineLength = intLength
        Me.LineWeight = 1
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    <DllImportAttribute("uxtheme.dll")>
    Private Shared Function SetWindowTheme(ByVal hWnd As IntPtr, ByVal appname As String, ByVal idlist As String) As Integer
    End Function
    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        SetWindowTheme(Me.Handle, "", "")
        MyBase.OnHandleCreated(e)
    End Sub

#Region "FORM's EVENTs"


    ''' <summary>
    ''' fncAddVerticalLine
    ''' </summary>
    ''' <param name="objCard">card to attach</param>
    ''' <param name="emAnchor">point on card</param>
    ''' <remarks>Add a vertical line</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Sub fncAddVerticalLine(ByVal objCard As usrMemCardBase, _
                                  ByVal emAnchor As clsEnum.emCardPoint, _
                                  Optional ByVal intOffsetX As Integer = 0, _
                                  Optional ByVal intOffsetY As Integer = 0)

        Try
            'always be vertical
            Me.LineDirection = clsEnum.emLineDirection.VERTICAL
            'Me.LineType = clsEnum.emLineType.SINGLE_LINE

            mintOffsetX = intOffsetX
            mintOffsetY = intOffsetY

            'base on anchor, set position
            Select Case emAnchor
                Case clsEnum.emCardPoint.MID_BOTTOM
                    Me.LineCoor = objCard.CardMidBottom
                    Me.Location = New Point(objCard.CardMidBottom.X, objCard.CardMidBottom.Y)
                    Me.memAnchor = clsEnum.emCardPoint.MID_BOTTOM
                    AddHandler objCard.evnCardMove, AddressOf xCardMove
                Case clsEnum.emCardPoint.MID_TOP
                    Me.LineCoor = New clsCoordinate(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)
                    Me.Location = New Point(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)
                    Me.memAnchor = clsEnum.emCardPoint.MID_TOP
                    AddHandler objCard.evnCardMove, AddressOf xCardMove

                Case clsEnum.emCardPoint.MID_RIGHT
                    Me.LineCoor = New clsCoordinate(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)
                    Me.Location = New Point(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)
                    Me.memAnchor = clsEnum.emCardPoint.MID_RIGHT
                    AddHandler objCard.evnCardMove, AddressOf xCardMove

                Case clsEnum.emCardPoint.MID_LEFT
                    Me.LineCoor = New clsCoordinate(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)
                    Me.Location = New Point(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)
                    Me.memAnchor = clsEnum.emCardPoint.MID_LEFT
                    AddHandler objCard.evnCardMove, AddressOf xCardMove

            End Select

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncAddParent", ex)
        End Try

    End Sub


    ''' <summary>
    ''' fncAddSpouseLine
    ''' </summary>
    ''' <param name="objCardLeft">Card 1</param>
    ''' <param name="objCardRight">Card 2</param>
    ''' <param name="intOffSet">Space between 2 card</param>
    ''' <remarks>Add spouse (Paralel line)</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Sub fncAddSpouseLine(ByVal objCardLeft As usrMemberCard1, ByVal objCardRight As usrMemberCard1, ByVal intOffSet As Integer)
        Try
            'always be horizontal
            Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL

            Me.mobjCardLeft = objCardLeft
            Me.mobjCardRight = objCardRight
            Me.mintOffset = intOffSet
            xLocateSpouseLine(Nothing)
            AddHandler mobjCardLeft.evnCardMove, AddressOf xLocateSpouseLine
            AddHandler mobjCardRight.evnCardMove, AddressOf xLocateSpouseLine

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncAddSpouseLine", ex)
        End Try
    End Sub

    ''' <summary>
    ''' fncAddSpouseLine
    ''' </summary>
    ''' <param name="objCardLeft">Card 1</param>
    ''' <param name="objCardRight">Card 2</param>
    ''' <param name="intOffSet">Space between 2 card</param>
    ''' <remarks>Add spouse (Paralel line)</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Sub fncAddSpouseLine(ByVal objCardLeft As usrMemberCard3, ByVal objCardRight As usrMemberCard3, ByVal intOffSet As Integer)
        Try
            'always be horizontal
            Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL

            Me.mobjCardLeft = objCardLeft
            Me.mobjCardRight = objCardRight
            Me.mintOffset = intOffSet

            xLocateSpouseLine(Nothing)


            AddHandler mobjCardLeft.evnCardMove, AddressOf xLocateSpouseLine
            AddHandler mobjCardRight.evnCardMove, AddressOf xLocateSpouseLine

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncAddSpouseLine", ex)
        End Try
    End Sub


    ''' <summary>
    ''' fncAddHorizontalLine
    ''' </summary>
    ''' <param name="objFather">Line 1</param>
    ''' <param name="objMother">Line 2</param>
    ''' <remarks>Add horizontal Line, connect 2 line</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Public Sub fncAddHorizontalLine(ByVal objFather As usrLine, ByVal objMother As usrLine)
        Try

            'always be horizontal
            Me.LineDirection = clsEnum.emLineDirection.HORIZONTAL

            Me.mobjLine1 = objFather
            Me.mobjLine2 = objMother

            xLocateHorzLine()

            AddHandler mobjLine1.evnControlMove, AddressOf xLocateHorzLine
            AddHandler mobjLine2.evnControlMove, AddressOf xLocateHorzLine



        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncAddHorizontalLine", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xCardMove
    ''' </summary>
    ''' <param name="objCard">usrMemCardBase</param>
    ''' <remarks>Handle card move event</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Private Sub xCardMove(ByVal objCard As usrMemCardBase)
        Try
            'base on anchor, reset position
            Select Case memAnchor
                Case clsEnum.emCardPoint.MID_BOTTOM
                    Me.LineCoor = New clsCoordinate(objCard.CardMidBottom.X, objCard.CardMidBottom.Y)
                    Me.Location = New Point(objCard.CardMidBottom.X, objCard.CardMidBottom.Y)

                Case clsEnum.emCardPoint.MID_TOP
                    Me.LineCoor = New clsCoordinate(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)
                    Me.Location = New Point(objCard.CardMidTop.X, objCard.CardMidTop.Y - Me.LineLength)

                Case clsEnum.emCardPoint.MID_RIGHT
                    Me.LineCoor = New clsCoordinate(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)
                    Me.Location = New Point(objCard.CardMidRight.X + mintOffsetX, objCard.CardMidRight.Y + mintOffsetY)

                Case clsEnum.emCardPoint.MID_LEFT
                    Me.LineCoor = New clsCoordinate(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)
                    Me.Location = New Point(objCard.CardMidLeft.X + mintOffsetX, objCard.CardMidLeft.Y + mintOffsetY)

            End Select

            RaiseEvent evnControlMove()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCardMove", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xLocateHorzLine
    ''' </summary>
    ''' <remarks>Set location</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Private Sub xLocateHorzLine()
        Try
            Dim ptLocation As clsCoordinate
            Dim intX As Integer = 0
            Dim intY As Integer = 0

            ptLocation = New clsCoordinate(0, 0)

            ptLocation = mobjLine1.LineCoor
            intX = ptLocation.X
            intY = ptLocation.Y

            'X is the smaller X
            If mobjLine2.LineCoor.X < intX Then intX = mobjLine2.LineCoor.X

            'Y is the greater Y
            If mobjLine2.LineCoor.Y > intY Then intY = mobjLine2.LineCoor.Y

            'set postion of this control
            Me.LineLength = Math.Abs(mobjLine1.LineCoor.X - mobjLine2.LineCoor.X)
            Me.LineCoor = New clsCoordinate(intX, intY)
            Me.Location = New Point(intX, intY)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLocateHorzLine", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xLocateSpouseLine
    ''' </summary>
    ''' <param name="objCard">usrMemCardBase</param>
    ''' <remarks>Set Location</remarks>
    ''' <Create>2012/02/14  AKB Quyet</Create>
    Private Sub xLocateSpouseLine(ByVal objCard As usrMemCardBase)
        Try
            Dim pt1 As clsCoordinate
            Dim pt2 As clsCoordinate
            Dim ptTemp As clsCoordinate


            pt1 = mobjCardLeft.CardMidRight
            pt2 = mobjCardRight.CardMidLeft

            'if pt1 > pt2 => change position
            If pt1.X > pt2.X Then
                ptTemp = pt1
                pt1 = pt2
                pt2 = ptTemp
            End If

            pt1.Y += mintOffset

            Me.LineCoor = pt1
            Me.Location = New Point(pt1.X, pt1.Y)
            Me.Width = Math.Abs(pt1.X - pt2.X)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLocateSpouseLine", ex)
        End Try
    End Sub


    '''' <summary>
    '''' usrLine_Move
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks>Handle line move</remarks>
    '''' <Create>2012/02/14  AKB Quyet</Create>
    'Private Sub usrLine_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Move
    '    Try
    '        RaiseEvent evnControlMove()

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "usrLine_Move", ex)
    '    End Try
    'End Sub


#End Region

End Class
