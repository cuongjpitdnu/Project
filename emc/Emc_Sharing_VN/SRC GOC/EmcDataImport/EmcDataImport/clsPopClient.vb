'******************************************************************
'   TITLE    : clsPopClient.vb
'   機能     : Pop Client processing class
'   備考     : 無し.
'   CREATE   : 2020/02/17  AKB Cuong.
'   UPDATE   : .
'
'       2020 AKBSOFTWARE CORPORATION
'******************************************************************

Imports System
Imports System.Collections
Imports System.IO
Imports System.Net.Sockets
Imports System.Text

'******************************************************************
'   機能     : POP よりメールを受信するクラスです。
'   備考     : 無し.
'   CREATE   : 2020/02/17  AKB Cuong.
'   UPDATE   : .
'******************************************************************
Public Class clsPopClient : Implements IDisposable

    Private tcp As TcpClient = Nothing      'TCP 接続

    Private reader As StreamReader = Nothing        'TCP 接続からのリーダー


    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです。POPサーバと接続します
    '　　　VALUE      : None.
    '      PARAMS     : (hostname String, POPサーバのホスト名。)
    '                   (port Integer, POPサーバのポート番号（通常は110）。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal hostname As String, ByVal port As Integer)
        ' サーバと接続
        Me.tcp = New TcpClient(hostname, port)
        Me.reader = New StreamReader(Me.tcp.GetStream(), Encoding.ASCII)

        ' オープニング受信
        Dim strResult As String = ReadLine()
        If Not strResult.StartsWith("+OK") Then
            Throw New clsPopClientException("接続時に POP サーバが """ & strResult & """ を返しました。")
        End If
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : 解放処理を行います。
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
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


    '   ******************************************************************
    '　　　FUNCTION   : POP サーバにログインします。
    '　　　VALUE      : None.
    '      PARAMS     : (username String, ユーザ名。)
    '                   (password String, パスワード。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub Login(ByVal username As String, ByVal password As String)
        ' ユーザ名送信
        SendLine("USER " & username)
        Dim strResult As String = ReadLine()
        If Not strResult.StartsWith("+OK") Then
            Throw New clsPopClientException("USER 送信時に POP サーバが """ & strResult & """ を返しました。")
        End If

        ' パスワード送信
        SendLine("PASS " & password)
        strResult = ReadLine()
        If Not strResult.StartsWith("+OK") Then
            Throw New clsPopClientException("PASS 送信時に POP サーバが """ & strResult & """ を返しました。")
        End If
    End Sub



    '   ******************************************************************
    '　　　FUNCTION   : POP サーバに溜まっているメールのリストを取得します。
    '　　　VALUE      : System.String を格納した ArrayList。
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetList() As ArrayList
        ' LIST 送信
        SendLine("LIST")
        Dim strResult As String = ReadLine()
        If Not strResult.StartsWith("+OK") Then
            Throw New clsPopClientException("LIST 送信時に POP サーバが """ & strResult & """ を返しました。")
        End If

        ' サーバにたまっているメールの数を取得
        Dim list As ArrayList = New ArrayList
        Do While True
            strResult = ReadLine()
            If strResult = "." Then
                ' 終端に到達
                Exit Do
            End If
            ' メール番号部分のみを取り出し格納
            Dim intIndex As Integer = strResult.IndexOf(" "c)
            If intIndex > 0 Then
                strResult = strResult.Substring(0, intIndex)
            End If
            list.Add(strResult)
        Loop
        Return list
    End Function



    '   ******************************************************************
    '　　　FUNCTION   : POP サーバからメールを 1つ取得します。
    '　　　VALUE      : String メールの本体。
    '      PARAMS     : (num String, GetList() メソッドで取得したメールの番号。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetMail(ByVal num As String) As String
        ' RETR 送信
        SendLine("RETR " & num)
        Dim strResult As String = ReadLine()
        If Not strResult.StartsWith("+OK") Then
            Throw New clsPopClientException("RETR 送信時に POP サーバが """ & strResult & """ を返しました。")
        End If

        ' メール取得
        Dim sb As StringBuilder = New StringBuilder
        Do While True
            strResult = ReadLine()
            If strResult = "." Then
                ' "." のみの場合はメールの終端を表す
                Exit Do
            End If
            sb.Append(strResult)
            sb.Append(vbCrLf)
        Loop
        Return sb.ToString()
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : POP サーバのメールを 1つ削除します。
    '　　　VALUE      : String メールの本体。
    '      PARAMS     : (num String, GetList() メソッドで取得したメールの番号。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub DeleteMail(ByVal num As String)
        ' DELE 送信
        SendLine("DELE " & num)
        Dim strResult As String = ReadLine()
        If Not strResult.StartsWith("+OK") Then
            Throw New clsPopClientException("DELE 送信時に POP サーバが """ & strResult & """ を返しました。")
        End If
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : POP サーバと切断します。
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub Close()
        ' QUIT 送信
        SendLine("QUIT")
        Dim strResult As String = ReadLine()
        If Not strResult.StartsWith("+OK") Then
            Throw New clsPopClientException("QUIT 送信時に POP サーバが """ & strResult & """ を返しました。")
        End If

        CType(Me.reader, IDisposable).Dispose()
        Me.reader = Nothing
        CType(Me.tcp, IDisposable).Dispose()
        Me.tcp = Nothing
    End Sub



    '   ******************************************************************
    '　　　FUNCTION   : POP サーバにコマンドを送信します。
    '　　　VALUE      : None.
    '      PARAMS     : (s String,送信する文字列。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub Send(ByVal s As String)
        Print("送信: " & s)
        Dim bytData As Byte() = Encoding.ASCII.GetBytes(s)
        Me.tcp.GetStream().Write(bytData, 0, bytData.Length)
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : POP サーバにコマンドを送信します。末尾に改行を付加します。
    '　　　VALUE      : None.
    '      PARAMS     : (s String,送信する文字列。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub SendLine(ByVal s As String)
        Print("送信: " & s & "\r\n")
        Dim bytData As Byte() = Encoding.ASCII.GetBytes(s & vbCrLf)
        Me.tcp.GetStream().Write(bytData, 0, bytData.Length)
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : POP サーバから 1行読み込みます。
    '　　　VALUE      : String 読み込んだ文字列。
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    ''' <summary>
    ''' POP サーバから 1行読み込みます。
    ''' </summary>
    ''' <returns>読み込んだ文字列。</returns>
    Private Function ReadLine() As String
        Dim strResult As String = Me.reader.ReadLine()
        Print("受信: " & strResult & "\r\n")
        Return strResult
    End Function

    ''' <summary>
    ''' チェック用にコンソールに出力します。
    ''' </summary>
    ''' <param name="msg">出力する文字列。</param>
    Private Sub Print(ByVal msg As String)
        'Console.WriteLine(msg)
    End Sub

End Class
