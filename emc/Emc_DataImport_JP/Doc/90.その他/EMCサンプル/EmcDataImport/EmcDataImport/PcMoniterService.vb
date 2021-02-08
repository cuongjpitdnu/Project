Imports System.ServiceProcess
Imports System.IO
Imports System.Text

Namespace PcMoniter
    Partial Public Class PcMoniterService
        Inherits ServiceBase

        Private goingToDie As Boolean = False
        Private mainThread As Threading.Thread
        Private args() As String
        Private strErrName As String

        Protected Overrides Sub OnStart(ByVal args As String())
            ' サービスを開始するコードをここに追加します。このメソッドによって、
            ' サービスが正しく実行されるようになります
            strErrName = ""
            Me.args = args
            mainThread = New Threading.Thread(AddressOf Runner)
            mainThread.Start()
        End Sub

        Protected Overrides Sub OnStop()
            ' サービスを停止するのに必要な終了処理を実行するコードをここに追加します。
            goingToDie = True
            mainThread.Join()
        End Sub



        Private Sub Runner()
            Dim strErrMsg As String = ""

            If Not xCheckSetting(strErrMsg) Then
                'エラーログを出力
                Call xOutPutErrLog(strErrMsg)
            Else
                Dim strBuf As String = My.Settings.EXECUTION_CYCLE
                If IsNumeric(strBuf) Then
                    Dim intExe As Integer
                    intExe = CInt(strBuf)

                    '実行周期で実行
                    Do Until goingToDie

                        System.Threading.Thread.Sleep(intExe)

                        Call PopTest()

                    Loop
                End If
            End If

        End Sub

        ''' <summary>
        ''' 設定ファイルチェック
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function xCheckSetting(ByRef strErrMsg As String) As Boolean
            xCheckSetting = False
            strErrMsg = ""

            Try

                If My.Settings.DB_HOSTNAME.Trim.Length = 0 Then
                    strErrMsg = "DBホスト名に値がありません"
                    Return False
                End If
                If My.Settings.DB_DBNAME.Trim.Length = 0 Then
                    strErrMsg = "DB名称に値がありません"
                    Return False
                End If
                If My.Settings.DB_USERNAME.Trim.Length = 0 Then
                    strErrMsg = "接続ユーザ名に値がありません"
                    Return False
                End If
                If My.Settings.DB_USERPASSWORD.Trim.Length = 0 Then
                    strErrMsg = "接続ユーザパスワードに値がありません"
                    Return False
                End If
                If My.Settings.MAIL_POP_HOST.Trim.Length = 0 Then
                    strErrMsg = "POPサーバのホスト名に値がありません"
                    Return False
                End If
                If My.Settings.MAIL_POP_PORT.Trim.Length = 0 Then
                    strErrMsg = "POPサーバのポートに値がありません"
                    Return False
                End If
                If My.Settings.MAIL_POP_USER.Trim.Length = 0 Then
                    strErrMsg = "POPサーバのユーザ名に値がありません"
                    Return False
                End If
                If My.Settings.MAIL_POP_PASS.Trim.Length = 0 Then
                    strErrMsg = "POPサーバのパスワードに値がありません"
                    Return False
                End If
                If My.Settings.MAIL_SMTP_HOST.Trim.Length = 0 Then
                    strErrMsg = "SMTPサーバのホスト名に値がありません"
                    Return False
                End If
                If My.Settings.MAIL_SMTP_PORT.Trim.Length = 0 Then
                    strErrMsg = "SMTPサーバのポートに値がありません"
                    Return False
                End If
                If My.Settings.MAIL_SMTP_USER.Trim.Length = 0 Then
                    strErrMsg = "SMTPサーバのユーザ名に値がありません"
                    Return False
                End If
                If My.Settings.MAIL_SMTP_PASS.Trim.Length = 0 Then
                    strErrMsg = "SMTPサーバのパスワードに値がありません"
                    Return False
                End If
                If My.Settings.EXECUTION_CYCLE.Trim.Length = 0 Then
                    strErrMsg = "実行周期に値がありません"
                    Return False
                End If
                If My.Settings.LOG_FOLDER.Trim.Length = 0 Then
                    strErrMsg = "ログ出力フォルダに値がありません"
                    Return False
                ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.LOG_FOLDER) Then
                    strErrMsg = "ログ出力フォルダパスが存在しません"
                    Return False
                End If
                If My.Settings.ERRLOG_FOLDER.Trim.Length = 0 Then
                    strErrMsg = "エラーログ出力フォルダに値がありません"
                    Return False
                ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.ERRLOG_FOLDER) Then
                    strErrMsg = "ログ出力フォルダパスが存在しません"
                    Return False
                End If
                If My.Settings.DEBUGFLG.Trim.Length = 0 Then
                    My.Settings.DEBUGFLG = 0    '出力なしに設定
                End If

                xCheckSetting = True
            Catch ex As Exception
                strErrName = "xCheckSetting    " & ex.Message
                Throw
            End Try

        End Function

        Private Function xOutPutErrLog(ByVal strErrMsg As String) As Boolean
            xOutPutErrLog = False
            Dim sw As StreamWriter = Nothing

            Try
                sw = New StreamWriter(System.IO.Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly.Location.ToString).ToString & "\" & DateTime.Now.ToString("yyyyMMddHHmmssfff") & "_Err.txt")
                sw.Write(strErrMsg)
                xOutPutErrLog = True

            Catch ex As Exception
                Throw
            Finally
                If Not sw Is Nothing Then
                    CType(sw, IDisposable).Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' POP に接続しメールを取得するテストを実行します。
        ''' </summary>
        Private Shared Sub PopTest()

            Dim hostname As String
            Dim username As String
            Dim password As String
            Dim strPort As String

            Dim intIdx As Integer
            Dim intIdx2 As Integer
            Dim intIdx3 As Integer
            Dim strBuf As String

            hostname = My.Settings.MAIL_POP_HOST
            username = My.Settings.MAIL_POP_USER
            password = My.Settings.MAIL_POP_PASS
            strPort = My.Settings.MAIL_POP_PORT
            If Not IsNumeric(strPort) Then
                Exit Sub
            End If

            ' POP サーバに接続します。
            Dim pop As PopClient = New PopClient(hostname, CInt(strPort))

            ' ログインします。
            pop.Login(username, password)

            ' POP サーバに溜まっているメールのリストを取得します。
            Dim list As ArrayList = pop.GetList()

            For i As Integer = 0 To list.Count - 1
                ' メール本体を取得します。
                Dim mail As String = pop.GetMail(CType(list(i), String))

                ' 確認用に取得したメールをそのままカレントディレクトリに書き出します。
                Dim sw As StreamWriter = Nothing
                Try

                    ' Mail クラスを作成します。
                    Dim mymail As Mail = New Mail(mail)

                    '"charset"検索
                    intIdx = InStr(mail, "charset")
                    If intIdx > 0 Then
                        strBuf = Mid(mail, intIdx)

                        intIdx = InStr(strBuf, "=")
                        intIdx3 = InStr(strBuf, """")
                        If (intIdx + 1 = intIdx3) Or (intIdx + 2 = intIdx3) Then
                            strBuf = Mid(strBuf, intIdx3 + 1)
                            intIdx = 0
                            intIdx2 = InStr(strBuf, """")
                        Else
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(strBuf, "&")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(strBuf, " ")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(strBuf, ";")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(strBuf, """")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(strBuf, vbCr)
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(strBuf, vbCrLf)
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        If intIdx2 > 0 Then
                            strBuf = Mid(strBuf, intIdx + 1, (intIdx2 - intIdx) - 1)
                        End If
                        strBuf = Trim(Replace(strBuf, vbCr, ""))
                        strBuf = Trim(Replace(strBuf, vbCrLf, ""))
                        strBuf = Trim(Replace(strBuf, """", ""))

                        If StrConv(strBuf, VbStrConv.Uppercase + VbStrConv.Narrow) = StrConv("utf-8", VbStrConv.Uppercase + VbStrConv.Narrow) Then

                            Dim bytes As Byte()
                            Dim mymailbody As String

                            sw = New StreamWriter("C:\Project\NTTネオメイト\情報共有システム\Test\EmcDataImport\EmcDataImport\bin\Debug\" & DateTime.Now.ToString("yyyyMMddHHmmssfff") & i.ToString("0000") & "_utf81.txt")

                            sw.Write(MailHeader.Decode(mymail.Header("From")(0)))
                            sw.Write(MailHeader.Decode(mymail.Header("To")(0)))
                            sw.Write(MailHeader.Decode(mymail.Header("Subject")(0)))


                            If Not sw Is Nothing Then
                                CType(sw, IDisposable).Dispose()
                            End If

                            sw = Nothing

                            sw = New StreamWriter(DateTime.Now.ToString("C:\Project\NTTネオメイト\情報共有システム\Test\EmcDataImport\EmcDataImport\bin\Debug\" & "yyyyMMddHHmmssfff") & i.ToString("0000") & "_utf82.txt", False, System.Text.Encoding.GetEncoding(65001))


                            Dim uniEnc As System.Text.Encoding = System.Text.Encoding.ASCII
                            Dim eucEnc As System.Text.Encoding = System.Text.Encoding.GetEncoding(65001)  'utf-8

                            ''If mymail.Body.Multiparts.Length = 0 Then
                            ''    '添付ファイルなし

                            ''    bytes = Encoding.ASCII.GetBytes(mymail.Body.Text)
                            ''    mymailbody = Encoding.GetEncoding(65001).GetString(bytes)   '"utf-8"に変換

                            ''Else
                            ''    '添付ファイルあり

                            ''    Dim part1 As MailMultipart = mymail.Body.Multiparts(0)

                            ''    bytes = Encoding.ASCII.GetBytes(part1.Body.Text)
                            ''    mymailbody = Encoding.GetEncoding(65001).GetString(bytes)   '"utf-8"に変換

                            ''End If

                            ''sw.Write(mymailbody)


                        ElseIf StrConv(strBuf, VbStrConv.Uppercase + VbStrConv.Narrow) = StrConv("ISO-2022-JP", VbStrConv.Uppercase + VbStrConv.Narrow) Then

                            Dim bytes As Byte()
                            Dim mymailbody As String

                            sw = New StreamWriter("C:\Project\NTTネオメイト\情報共有システム\Test\EmcDataImport\EmcDataImport\bin\Debug\" & DateTime.Now.ToString("yyyyMMddHHmmssfff") & i.ToString("0000") & "_iso1.txt")

                            sw.Write(MailHeader.Decode(mymail.Header("From")(0)))
                            sw.Write(MailHeader.Decode(mymail.Header("To")(0)))
                            sw.Write(MailHeader.Decode(mymail.Header("Subject")(0)))


                            If Not sw Is Nothing Then
                                CType(sw, IDisposable).Dispose()
                            End If

                            sw = Nothing

                            sw = New StreamWriter("C:\Project\NTTネオメイト\情報共有システム\Test\EmcDataImport\EmcDataImport\bin\Debug\" & DateTime.Now.ToString("yyyyMMddHHmmssfff") & i.ToString("0000") & "_iso2.txt")

                            If mymail.Body.Multiparts.Length = 0 Then
                                '添付ファイルなし

                                bytes = Encoding.ASCII.GetBytes(mymail.Body.Text)
                                mymailbody = Encoding.GetEncoding(50220).GetString(bytes)   '"ISO-2022-JP"に変換
                                sw.Write(mymailbody)

                            Else
                                '添付ファイルあり

                                Dim part1 As MailMultipart = mymail.Body.Multiparts(0)

                                bytes = Encoding.ASCII.GetBytes(part1.Body.Text)
                                mymailbody = Encoding.GetEncoding(50220).GetString(bytes)   '"ISO-2022-JP"に変換
                                sw.Write(mymailbody)

                            End If

                        End If

                    End If

                Finally
                    If Not sw Is Nothing Then
                        CType(sw, IDisposable).Dispose()
                    End If
                End Try

                'Exit For

                ' メールを POP サーバから取得します。
                ' ★注意★
                ' 削除したメールを元に戻すことはできません。
                ' 本当に削除していい場合は以下のコメントをはずしてください。
                'pop.DeleteMail(CType(list(i), String))
            Next

            ' 切断します。
            pop.Close()
        End Sub
    End Class
End Namespace
