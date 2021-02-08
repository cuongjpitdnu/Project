''' <summary>
''' PopClient の例外クラスです。
''' </summary>
Public Class PopClientException : Inherits Exception

    ''' <summary>
    ''' コンストラクタです。
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' コンストラクタです。
    ''' </summary>
    ''' <param name="message"></param>
    Public Sub New(ByVal message As String)
        MyBase.new(message)
    End Sub

    ''' <summary>
    ''' コンストラクタです。
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="innerException"></param>
    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

End Class
