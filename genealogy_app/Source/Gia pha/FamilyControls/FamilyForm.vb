Option Explicit On

Public Class FamilyForm

    Private miCurID As Integer
    Private miRootID As Integer
    Private miRootSubID As Integer
    'Private miChildID As Integer

    Public Property PersonCurID() As Integer
        Get
            Return Me.miCurID
        End Get
        Set(ByVal value As Integer)
            Me.miCurID = value
        End Set
    End Property

    Public Property PersonRootID() As Integer

        Get
            Return Me.miRootID
        End Get

        Set(ByVal value As Integer)
            Me.miRootID = value
        End Set

    End Property

    Public Property PersonRootSubID() As Integer

        Get
            Return Me.miRootSubID
        End Get

        Set(ByVal value As Integer)
            Me.miRootSubID = value
        End Set

    End Property

    'Public Property ChildID() As Integer

    '    Get
    '        Return Me.miChildID
    '    End Get

    '    Set(ByVal value As Integer)
    '        Me.miChildID = value
    '    End Set

    'End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.miCurID = clsDefine.NONE_VALUE
        Me.miRootID = clsDefine.NONE_VALUE
        Me.miRootSubID = clsDefine.NONE_VALUE

    End Sub
End Class