Imports System
Imports System.Collections
Imports System.IO
Imports System.Net.Sockets
Imports System.Text

''' <summary>
''' POP よりメールを受信するクラスです。
''' </summary>
Public Class PopClient : Implements IDisposable

    ''' <summary>TCP 接続</summary>
    Private tcp As TcpClient = Nothing

    ''' <summary>TCP 接続からのリーダー</summary>
    Private reader As StreamReader = Nothing

    ''' <summary>
    ''' コンストラクタです。POPサーバと接続します。
    ''' </summary>
    ''' <param name="hostname">POPサーバのホスト名。</param>
    ''' <param name="port">POPサーバのポート番号（通常は110）。</param>
    Public Sub New(ByVal hostname As String, ByVal port As Integer)
        ' サーバと接続
        Me.tcp = New TcpClient(hostname, port)
        Me.reader = New StreamReader(Me.tcp.GetStream(), Encoding.ASCII)

        ' オープニング受信
        Dim s As String = ReadLine()
        If Not s.StartsWith("+OK") Then
            Throw New PopClientException("接続時に POP サーバが """ & s & """ を返しました。")
        End If
    End Sub

    ''' <summary>
    ''' 解放処理を行います。
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        If Not Me.reader Is Nothing Then
            CType(Me.reader, IDisposable).Dispose()
            Me.reader = Nothing
        End If
        If Not Me.tcp Is Nothing Then
            CType(Me.tcp, IDisposable).Dispose()
            Me.tcp = Nothing
        End If
    End Sub

    ''' <summary>
    ''' POP サーバにログインします。
    ''' </summary>
    ''' <param name="username">ユーザ名。</param>
    ''' <param name="password">パスワード。</param>
    Public Sub Login(ByVal username As String, ByVal password As String)
        ' ユーザ名送信
        SendLine("USER " & username)
        Dim s As String = ReadLine()
        If Not s.StartsWith("+OK") Then
            Throw New PopClientException("USER 送信時に POP サーバが """ & s & """ を返しました。")
        End If

        ' パスワード送信
        SendLine("PASS " & password)
        s = ReadLine()
        If Not s.StartsWith("+OK") Then
            Throw New PopClientException("PASS 送信時に POP サーバが """ & s & """ を返しました。")
        End If
    End Sub

    ''' <summary>
    ''' POP サーバに溜まっているメールのリストを取得します。
    ''' </summary>
    ''' <returns>System.String を格納した ArrayList。</returns>
    Public Function GetList() As ArrayList
        ' LIST 送信
        SendLine("LIST")
        Dim s As String = ReadLine()
        If Not s.StartsWith("+OK") Then
            Throw New PopClientException("LIST 送信時に POP サーバが """ & s & """ を返しました。")
        End If

        ' サーバにたまっているメールの数を取得
        Dim list As ArrayList = New ArrayList
        Do While True
            s = ReadLine()
            If s = "." Then
                ' 終端に到達
                Exit Do
            End If
            ' メール番号部分のみを取り出し格納
            Dim p As Integer = s.IndexOf(" "c)
            If p > 0 Then
                s = s.Substring(0, p)
            End If
            list.Add(s)
        Loop
        Return list
    End Function

    ''' <summary>
    ''' POP サーバからメールを 1つ取得します。
    ''' </summary>
    ''' <param name="num">GetList() メソッドで取得したメールの番号。</param>
    ''' <returns>メールの本体。</returns>
    Public Function GetMail(ByVal num As String) As String
        ' RETR 送信
        SendLine("RETR " & num)
        Dim s As String = ReadLine()
        If Not s.StartsWith("+OK") Then
            Throw New PopClientException("RETR 送信時に POP サーバが """ & s & """ を返しました。")
        End If

        ' メール取得
        Dim sb As StringBuilder = New StringBuilder
        Do While True
            s = ReadLine()
            If s = "." Then
                ' "." のみの場合はメールの終端を表す
                Exit Do
            End If
            sb.Append(s)
            sb.Append(vbCrLf)
        Loop
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' POP サーバのメールを 1つ削除します。
    ''' </summary>
    ''' <param name="num">GetList() メソッドで取得したメールの番号。</param>
    Public Sub DeleteMail(ByVal num As String)
        ' DELE 送信
        SendLine("DELE " & num)
        Dim s As String = ReadLine()
        If Not s.StartsWith("+OK") Then
            Throw New PopClientException("DELE 送信時に POP サーバが """ & s & """ を返しました。")
        End If
    End Sub

    ''' <summary>
    ''' POP サーバと切断します。
    ''' </summary>
    Public Sub Close()
        ' QUIT 送信
        SendLine("QUIT")
        Dim s As String = ReadLine()
        If Not s.StartsWith("+OK") Then
            Throw New PopClientException("QUIT 送信時に POP サーバが """ & s & """ を返しました。")
        End If

        CType(Me.reader, IDisposable).Dispose()
        Me.reader = Nothing
        CType(Me.tcp, IDisposable).Dispose()
        Me.tcp = Nothing
    End Sub

    ''' <summary>
    ''' POP サーバにコマンドを送信します。
    ''' </summary>
    ''' <param name="s">送信する文字列。</param>
    Private Sub Send(ByVal s As String)
        Print("送信: " & s)
        Dim b As Byte() = Encoding.ASCII.GetBytes(s)
        Me.tcp.GetStream().Write(b, 0, b.Length)
    End Sub

    ''' <summary>
    ''' POP サーバにコマンドを送信します。末尾に改行を付加します。
    ''' </summary>
    ''' <param name="s">送信する文字列。</param>
    Private Sub SendLine(ByVal s As String)
        Print("送信: " & s & "\r\n")
        Dim b As Byte() = Encoding.ASCII.GetBytes(s & vbCrLf)
        Me.tcp.GetStream().Write(b, 0, b.Length)
    End Sub

    ''' <summary>
    ''' POP サーバから 1行読み込みます。
    ''' </summary>
    ''' <returns>読み込んだ文字列。</returns>
    Private Function ReadLine() As String
        Dim s As String = Me.reader.ReadLine()
        Print("受信: " & s & "\r\n")
        Return s
    End Function

    ''' <summary>
    ''' チェック用にコンソールに出力します。
    ''' </summary>
    ''' <param name="msg">出力する文字列。</param>
    Private Sub Print(ByVal msg As String)
        'Console.WriteLine(msg)
    End Sub

End Class
