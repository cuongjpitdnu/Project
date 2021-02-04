'   ****************************************************************** 
'      TITLE      : MEMBER CARD BASE
'　　　FUNCTION   :  
'      MEMO       :  
'      CREATE     : 2012/02/14　AKB　Quyet 
'      UPDATE     :  
' 
'           2012 AKB SOFTWARE 
'   ******************************************************************
Option Explicit On
Option Strict Off


''' <summary>
''' MEMBER CARD BASE CLASS
''' </summary>
''' <remarks></remarks>
''' <Create>2012/02/14  AKB Quyet</Create>
Public Class usrMemCardBase

    Private Const mcstrClsName As String = "usrMemCardBase"             'class name

    Private mintID As Integer                                           'card id
    Private mintDrawLv As Integer                                       'level when drawing
    Private mintParentID As Integer = basConst.gcintNONE_VALUE          'parent id
    Private mintSpouseID As Integer = basConst.gcintNONE_VALUE          'spouse id
    Private mblnSelected As Boolean = False                             'card selected
    Private mblnMouseDown As Boolean = False                            'mouse down flag
    Private mclsCoord As clsCoordinate
    Private mintXCoor As Integer
    Private mintYCoor As Integer

    Public Event evnCardLocationChange(ByVal objCard As usrMemCardBase, ByVal intX As Integer, ByVal intY As Integer)
    Public Event evnCardMove(ByVal objCard As usrMemCardBase)
    Public Event evnNotDraw(ByVal objCard As usrMemCardBase, ByVal intID As Integer)

    ''' <summary>
    ''' CardID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Gets or Set card id</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Property CardID() As Integer
        Get
            Return mintID
        End Get
        Set(ByVal value As Integer)
            mintID = value
        End Set
    End Property


    ''' <summary>
    ''' CardMidTop
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Gets mid top point</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property CardMidTop() As clsCoordinate

        Get
            'get current location
            Dim intX As Integer = mintXCoor
            Dim intY As Integer = mintYCoor

            'middle top: X changes, Y doesn't
            intX += Me.Width \ 2

            Return New clsCoordinate(intX, intY)

        End Get

    End Property

    Public Property CardXCoor() As Integer
        Get
            Return mintXCoor
        End Get
        Set(ByVal value As Integer)

            mintXCoor = value

        End Set
    End Property

    Public Property CardYCoor() As Integer
        Get
            Return mintYCoor
        End Get
        Set(ByVal value As Integer)

            mintYCoor = value

        End Set
    End Property

    Public Property CardCoor() As clsCoordinate
        Get
            Return mclsCoord
        End Get
        Set(ByVal value As clsCoordinate)

            mclsCoord = value

            If Not IsNothing(value) Then

                mintXCoor = value.X
                mintYCoor = value.Y

            Else

                mintXCoor = 0
                mintYCoor = 0

            End If
        End Set
    End Property


    ''' <summary>
    ''' CardMidBottom
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Gets mid bottom point</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property CardMidBottom() As clsCoordinate

        Get
            'get current location
            Dim intX As Integer = mintXCoor
            Dim intY As Integer = mintYCoor


            'middle top: X changes, Y changes
            intX += Me.Width \ 2

            '2017/01/10 Need to minus some because of windows 7
            intY += Me.Height - gintHeightDiff

            Return New clsCoordinate(intX, intY)

        End Get

    End Property


    ''' <summary>
    ''' CardMidLeft
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Gets middle left point</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property CardMidLeft() As clsCoordinate

        Get
            'get current location
            Dim intX As Integer = mintXCoor
            Dim intY As Integer = mintYCoor

            'middle top: Y changes, X doesn't
            intY += Me.Height \ 2

            Return New clsCoordinate(intX, intY)

        End Get

    End Property


    ''' <summary>
    ''' CardMidRight
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Get middle right point</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public ReadOnly Property CardMidRight() As clsCoordinate

        Get
            'get current location
            Dim intX As Integer = mintXCoor
            Dim intY As Integer = mintYCoor

            'middle top: X Y changes
            intX += Me.Width
            intY += Me.Height \ 2

            Return New clsCoordinate(intX, intY)

        End Get

    End Property


    ''' <summary>
    ''' ParentID
    ''' </summary>
    ''' <value></value>
    ''' <returns>returns -1 if there is no parent</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/09/17  AKB Quyet</Create>
    Public Property ParentID() As Integer
        Get
            Return Me.mintParentID
        End Get
        Set(ByVal value As Integer)
            Me.mintParentID = value
        End Set
    End Property


    ''' <summary>
    ''' SpouseID
    ''' </summary>
    ''' <value></value>
    ''' <returns>returns -1 if there is no spouse</returns>
    ''' <remarks></remarks>
    ''' <Create>2012/09/17  AKB Quyet</Create>
    Public Property SpouseID() As Integer
        Get
            Return Me.mintSpouseID
        End Get
        Set(ByVal value As Integer)
            Me.mintSpouseID = value
        End Set
    End Property


    ''' <summary>
    ''' DrawLv
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>level when drawing</remarks>
    Public Property DrawLv() As Integer
        Get
            Return mintDrawLv
        End Get
        Set(ByVal value As Integer)
            mintDrawLv = value
        End Set
    End Property


    ''' <summary>
    ''' Card selected flag
    ''' </summary>
    ''' <value>boolean</value>
    ''' <returns>boolean</returns>
    ''' <remarks></remarks>
    Public Property CardSelected() As Boolean
        Get
            Return mblnSelected
        End Get
        Set(ByVal value As Boolean)
            mblnSelected = value
            Me.Invalidate()
        End Set
    End Property


    ''' <summary>
    ''' CardMouseDown Flag
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CardMouseDown()
        Get
            Return mblnMouseDown
        End Get
        Set(ByVal value)
            mblnMouseDown = value
        End Set
    End Property


    ''' <summary>
    ''' usrMemCardBase_Move
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Card move event</remarks>
    Private Sub usrMemCardBase_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        Try
            If Not CardMouseDown Then Exit Sub
            RaiseEvent evnCardMove(Me)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "usrMemberCard1_Move", ex)
        End Try
    End Sub


    ''' <summary>
    ''' xSaveImage
    ''' </summary>
    ''' <param name="strPath">String</param>
    ''' <returns>True - success, False - fail</returns>
    ''' <remarks>Save image</remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Private Function xSaveImage(ByVal strPath As String) As Boolean

        xSaveImage = False

        Dim objImage As Bitmap = Nothing

        Try
            Dim objRectangle As Rectangle

            'drawing area
            objRectangle = New Rectangle(0, 0, Me.Width, Me.Height)
            objImage = New Bitmap(Me.Width, Me.Height)

            'get bitmap
            Me.DrawToBitmap(objImage, objRectangle)

            'save to JPG image
            objImage.Save(strPath, System.Drawing.Imaging.ImageFormat.Png)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncGetImage", ex)
        Finally
            If objImage IsNot Nothing Then objImage.Dispose()
        End Try

    End Function


    ''' <summary>
    ''' fncGetImage
    ''' </summary>
    ''' <param name="strPath">String</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <Create>2012/02/13  AKB Quyet</Create>
    Public Function fncGetImage(ByVal strPath As String, Optional ByVal blnSaveImg As Boolean = True) As String

        Dim strReturn As String = String.Empty

        Try
            strPath = String.Format(basConst.gcstrUsrCardFileFormat, strPath, Me.CardID)

            'remove selected bound
            If Me.CardSelected Then Me.CardSelected = False

            If Not blnSaveImg Then
                strReturn = strPath
            Else
                'try to save image
                If xSaveImage(strPath) Then strReturn = strPath
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncGetImage", ex)
        End Try

        Return strReturn

    End Function


    ''' <summary>
    ''' tsmNotDraw_Click - do not draw this member 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/03/23  AKB Quyet</Create>
    Private Sub tsmNotDraw_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmNotDraw.Click
        Try
            RaiseEvent evnNotDraw(Me, mintID)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsmNotDraw_Click", ex)
        End Try
    End Sub


    '''' <summary>
    '''' On Paint Event
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub usrMemCardBase_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
    '    Try
    '        If Not mblnSelected Then

    '            'draw a transparent layer on the card
    '            Using g As Graphics = e.Graphics

    '                g.Clear(Me.BackColor)
    '                g.FillRectangle(New SolidBrush(Color.FromArgb(20, 0, 0, 255)), 0, 0, Me.Width, Me.Height)

    '            End Using

    '        Else

    '            'clear transparent layer
    '            Using g As Graphics = e.Graphics
    '                g.Clear(Me.BackColor)
    '            End Using

    '        End If

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "usrMemCardBase_Paint", ex)
    '    End Try
    'End Sub


End Class
