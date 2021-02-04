'   ******************************************************************
'      TITLE      : Coordinate Class
'　　　FUNCTION   : Beacause maximum value of Point variable is 32767, so we create new class to store coordinate > 32767
'      MEMO       : 
'      CREATE     : 2014/10/10　AKB Vinh
'      UPDATE     : 
'
'           2014 AKB SOFTWARE
'   ******************************************************************
Public Class clsCoordinate
    Private mintX As Integer
    Private mintY As Integer

    Public Property X() As Integer
        Get
            Return mintX
        End Get
        Set(ByVal value As Integer)
            mintX = value
        End Set
    End Property

    Public Property Y() As Integer
        Get
            Return mintY
        End Get
        Set(ByVal value As Integer)
            mintY = value
        End Set
    End Property


    Public Sub New(ByVal intX As Integer, ByVal intY As Integer)
        mintX = intX
        mintY = intY
    End Sub
End Class
