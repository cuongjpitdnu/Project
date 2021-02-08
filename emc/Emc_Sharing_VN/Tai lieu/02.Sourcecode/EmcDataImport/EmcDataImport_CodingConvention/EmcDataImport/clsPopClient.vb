'******************************************************************
'   TITLE    : clsPopClient.vb
'   機能     : Pop Client processing class
'   備考     : 無し.
'   CREATE   : 2020/02/17  AKB Cuong.
'   UPDATE   : 2020/04/06  KBS E.Izumi SSL通信に変更.
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

    Private mobjTcpClient As TcpClient = Nothing      'TCP 接続

    Private mobjStreamReader As StreamReader = Nothing        'TCP 接続からのリーダー

    '▽2020/04/06 E.Izumi --- S --- 追加
    Private mobjSslStream As System.Net.Security.SslStream = Nothing
    '△2020/04/06 E.Izumi --- E --- 追加

    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです。POPサーバと接続します
    '　　　VALUE      : None.
    '      PARAMS     : (strHostname String, POPサーバのホスト名。)
    '                   (intPort Integer, POPサーバのポート番号（通常は110）。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 2020/04/06  KBS E.Izumi SSL通信に変更.
    '   ******************************************************************
    Public Sub New(ByVal strHostname As String, ByVal intPort As Integer)


        ' サーバと接続
        Me.mobjTcpClient = New TcpClient(strHostname, intPort)
        '▽2020/04/06 E.Izumi --- S --- SSLに変更
        'Me.mobjStreamReader = New StreamReader(Me.mobjTcpClient.GetStream(), Encoding.ASCII)

        ' SSLの実装-------------------------------------------------------------------
        '' 通信開始(SSL有り)
        mobjSslStream = New System.Net.Security.SslStream(Me.mobjTcpClient.GetStream())
        mobjSslStream.AuthenticateAsClient(strHostname)
        Me.mobjStreamReader = New StreamReader(mobjSslStream, Encoding.ASCII)
        ' ----------------------------------------------------------------------------
        '△2020/04/06 E.Izumi --- E --- SSLに変更

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
        If Not Me.mobjStreamReader Is Nothing Then
            CType(Me.mobjStreamReader, IDisposable).Dispose()
            Me.mobjStreamReader = Nothing
        End If
        If Not Me.mobjTcpClient Is Nothing Then
            CType(Me.mobjTcpClient, IDisposable).Dispose()
            Me.mobjTcpClient = Nothing
        End If
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : POP サーバにログインします。
    '　　　VALUE      : None.
    '      PARAMS     : (strUsername String, ユーザ名。)
    '                   (strPassword String, パスワード。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub Login(ByVal strUsername As String, ByVal strPassword As String)
        ' ユーザ名送信
        SendLine("USER " & strUsername)
        Dim strResult As String = ReadLine()
        If Not strResult.StartsWith("+OK") Then
            Throw New clsPopClientException("USER 送信時に POP サーバが """ & strResult & """ を返しました。")
        End If

        ' パスワード送信
        SendLine("PASS " & strPassword)
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
        Dim arrayList As ArrayList = New ArrayList
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
            arrayList.Add(strResult)
        Loop
        Return arrayList
    End Function



    '   ******************************************************************
    '　　　FUNCTION   : POP サーバからメールを 1つ取得します。
    '　　　VALUE      : String メールの本体。
    '      PARAMS     : (strnum String, GetList() メソッドで取得したメールの番号。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetMail(ByVal strnum As String) As String
        ' RETR 送信
        SendLine("RETR " & strnum)
        Dim strResult As String = ReadLine()
        If Not strResult.StartsWith("+OK") Then
            Throw New clsPopClientException("RETR 送信時に POP サーバが """ & strResult & """ を返しました。")
        End If

        ' メール取得
        Dim objStringBuilder As StringBuilder = New StringBuilder
        Do While True
            strResult = ReadLine()
            If strResult = "." Then
                ' "." のみの場合はメールの終端を表す
                Exit Do
            End If
            objStringBuilder.Append(strResult)
            objStringBuilder.Append(vbCrLf)
        Loop
        Return objStringBuilder.ToString()
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : POP サーバのメールを 1つ削除します。
    '　　　VALUE      : String メールの本体。
    '      PARAMS     : (strnum String, GetList() メソッドで取得したメールの番号。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub DeleteMail(ByVal strnum As String)
        ' DELE 送信
        SendLine("DELE " & strnum)
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

        CType(Me.mobjStreamReader, IDisposable).Dispose()
        Me.mobjStreamReader = Nothing
        CType(Me.mobjTcpClient, IDisposable).Dispose()
        Me.mobjTcpClient = Nothing
    End Sub



    '   ******************************************************************
    '　　　FUNCTION   : POP サーバにコマンドを送信します。
    '　　　VALUE      : None.
    '      PARAMS     : (strContent String,送信する文字列。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 2020/04/06  KBS E.Izumi SSL通信に変更.
    '   ******************************************************************
    Private Sub Send(ByVal strContent As String)
        Print("送信: " & strContent)
        Dim bytData As Byte() = Encoding.ASCII.GetBytes(strContent)
        '▽2020/04/06 E.Izumi --- S --- SSLに変更
        'Me.mobjTcpClient.GetStream().Write(bytData, 0, bytData.Length)
        mobjSslStream.Write(bytData, 0, bytData.Length)
        '△2020/04/06 E.Izumi --- E --- SSLに変更
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : POP サーバにコマンドを送信します。末尾に改行を付加します。
    '　　　VALUE      : None.
    '      PARAMS     : (strContent String,送信する文字列。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 2020/04/06  KBS E.Izumi SSL通信に変更.
    '   ******************************************************************
    Private Sub SendLine(ByVal strContent As String)
        Print("送信: " & strContent & "\r\n")
        Dim bytData As Byte() = Encoding.ASCII.GetBytes(strContent & vbCrLf)
        '▽2020/04/06 E.Izumi --- S --- SSLに変更
        'Me.mobjTcpClient.GetStream().Write(bytData, 0, bytData.Length)
        mobjSslStream.Write(bytData, 0, bytData.Length)
        '△2020/04/06 E.Izumi --- E --- SSLに変更

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : POP サーバから 1行読み込みます。
    '　　　VALUE      : String 読み込んだ文字列。
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Function ReadLine() As String
        Dim strResult As String = Me.mobjStreamReader.ReadLine()
        Print("受信: " & strResult & "\r\n")
        Return strResult
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : チェック用にコンソールに出力します。
    '　　　VALUE      : None.
    '      PARAMS     : (strmsg String,出力する文字列。)
    '      MEMO       : None.
    '      CREATE     : 2020/02/17 　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub Print(ByVal strmsg As String)
        'Console.WriteLine(msg)
    End Sub

End Class
