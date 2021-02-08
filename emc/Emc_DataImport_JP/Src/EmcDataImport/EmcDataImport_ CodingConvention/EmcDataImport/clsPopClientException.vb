'******************************************************************
'   機能     : PopClient の例外クラスです。
'   備考     : 無し.
'   CREATE   : 2020/02/17　AKB   Cuong.
'   UPDATE   : .
'******************************************************************
Public Class clsPopClientException : Inherits Exception

    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです。
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New()
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです。
    '　　　VALUE      : None.
    '      PARAMS     : (strMessage String, message err)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal strMessage As String)
        MyBase.New(strMessage)
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです。
    '　　　VALUE      : None.
    '      PARAMS     : (strMessage String, message)
    '                   (innerException Exception, message err)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal strMessage As String, ByVal innerException As Exception)
        MyBase.New(strMessage, innerException)
    End Sub

End Class
