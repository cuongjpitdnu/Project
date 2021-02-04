Option Explicit On

Imports System.Drawing.Drawing2D

Public Class FamilyBaseControl


#Region "Control Variable"

    Dim mpMidCenterPoint As Point
    Dim mpMidTopPoint As Point
    Dim mpMidBottomPoint As Point
    Dim mpMidLeftPoint As Point
    Dim mpMidRightPoint As Point

#End Region

#Region "Line Variable"

    Dim mdsLineStyle As DashStyle = clsDefine.PENSTYLE
    Dim miLineSize As Integer = clsDefine.PENSIZE

#End Region

#Region "Rectangle Variable"

    Dim mbCtrlShowBorder As Boolean = False
    Dim mdsCtrlBorderStyle As DashStyle = clsDefine.PENSTYLE
    Dim miCtrlBorderSize As Integer = clsDefine.PENSIZE
    Dim mclCtrlBorderColor As Color = Color.Black

#End Region

#Region "Control Properties"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MidPointCenter() As Point

        Get
            Return Me.mpMidCenterPoint
        End Get

    End Property

    'AKB Properties
    Public ReadOnly Property MidPointTop() As Point

        Get
            Return Me.mpMidTopPoint
        End Get

    End Property

    'AKB Properties
    Public ReadOnly Property MidPointBottom() As Point

        Get
            Return Me.mpMidBottomPoint
        End Get

    End Property

    'AKB Properties
    Public ReadOnly Property MidPointLeft() As Point

        Get
            Return Me.mpMidLeftPoint
        End Get

    End Property

    'AKB Properties
    Public ReadOnly Property MidPointRight() As Point

        Get
            Return Me.mpMidRightPoint
        End Get

    End Property

#End Region

#Region "Line Properties"

    Public Property LineStyle() As DashStyle

        Get
            Return Me.mdsLineStyle
        End Get

        Set(ByVal value As DashStyle)
            Me.mdsLineStyle = value
        End Set

    End Property

    Public Property LineSize() As Integer

        Get
            Return Me.miLineSize
        End Get

        Set(ByVal value As Integer)
            Me.miLineSize = value
        End Set

    End Property

#End Region

#Region "Rectangle Properties"

    Public Property ControlShowBorder() As Boolean

        Get
            Return mbCtrlShowBorder
        End Get

        Set(ByVal value As Boolean)
            mbCtrlShowBorder = value
        End Set

    End Property

    Public Property ControlBorderStyle() As DashStyle

        Get
            Return mdsCtrlBorderStyle
        End Get

        Set(ByVal value As DashStyle)
            mdsCtrlBorderStyle = value
        End Set

    End Property

    Public Property ControlBorderSize() As Integer

        Get
            Return miCtrlBorderSize
        End Get

        Set(ByVal value As Integer)
            miCtrlBorderSize = value
        End Set

    End Property

    Public Property ControlBorderColor() As Color

        Get
            Return mclCtrlBorderColor
        End Get

        Set(ByVal value As Color)
            mclCtrlBorderColor = value
        End Set

    End Property

#End Region

#Region "Control Function"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetSpecPoint()

        Try
            Dim dbDivHeightTmp As Double
            Dim dbDivWidthTmp As Double

            dbDivHeightTmp = Me.Height / 2
            dbDivWidthTmp = Me.Width / 2

            Dim iHafHeight As Integer = CType(dbDivHeightTmp, Integer) 'Integer.Parse(dbDivHeightTmp.ToString())
            Dim iHafWidth As Integer = CType(dbDivWidthTmp, Integer) 'Integer.Parse(dbDivWidthTmp.ToString())

            Dim pMidCen As New Point(Me.Location.X + iHafWidth, Me.Location.Y + iHafHeight)
            Dim pMidTop As New Point(Me.Location.X + iHafWidth, Me.Location.Y)
            Dim pMidBot As New Point(Me.Location.X + iHafWidth, Me.Location.Y + Me.Height)
            Dim pMidLef As New Point(Me.Location.X, Me.Location.Y + iHafHeight)
            Dim pMidRig As New Point(Me.Location.X + Me.Width, Me.Location.Y + iHafHeight)

            Me.mpMidCenterPoint = pMidCen
            Me.mpMidTopPoint = pMidTop
            Me.mpMidBottomPoint = pMidBot
            Me.mpMidLeftPoint = pMidLef
            Me.mpMidRightPoint = pMidRig

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

#End Region

#Region "Control Event"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FamilyControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        SetSpecPoint()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FamilyControl_LocationChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.LocationChanged

        SetSpecPoint()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FamilyControl_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged

        SetSpecPoint()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region

    
End Class
