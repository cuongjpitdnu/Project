Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms

Public Class EmcDataImport

    ''' <summary>  接続文字列  </summary>
    Private Shared connectionSql As String = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", My.Settings.DB_HOSTNAME, My.Settings.DB_DBNAME, My.Settings.DB_USERNAME, My.Settings.DB_USERPASSWORD)  ' DB接続文字列.
    ''' <summary> ログフォルダーのパス  </summary>
    Public Shared logFolderPath As String
    ''' <summary> ログファイル名  </summary>
    Public Shared fileNameLog As String
    ''' <summary> エラーログフォルダーのパス  </summary>
    Public Shared logErrFolderPath
    ''' <summary> エラーログファイル名  </summary>
    Public Shared fileErrNameLog As String

    Private Shared goingToDie As Boolean = False
    Private Shared mainThread As Thread

    Private Shared strBuf As String

    ''' <summary>
    ''' サービスを開始する
    ''' </summary>
    ''' <param name="args"></param>
    Protected Overrides Sub OnStart(ByVal args() As String)

        Dim strBuilder As New System.Text.StringBuilder

        If Not xCheckSetting(strBuilder) Then
            'エラーログを出力
            Call xOutPutErrLog(strBuilder)

            OnStop()
        Else
            mainThread = New Thread(AddressOf Runner)
            mainThread.Start()
        End If

    End Sub

    ''' <summary>
    ''' サービスを停止する
    ''' </summary>
    Protected Overrides Sub OnStop()

        If mainThread.ThreadState <> ThreadState.Stopped And mainThread.ThreadState <> ThreadState.Aborted Then
            goingToDie = True
            mainThread.Join()
        End If

    End Sub

    ''' <summary>
    ''' ストリーム処理
    ''' </summary>
    Private Shared Sub Runner()

        Dim strBuf As String = My.Settings.EXECUTION_CYCLE

        If IsNumeric(strBuf) Then
            Dim intExe As Integer
            intExe = CInt(strBuf)

            '実行周期で実行
            Do Until goingToDie

                logFolderPath = My.Settings.LOG_FOLDER
                fileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.cstrDateFomatShow) & ".log"
                logErrFolderPath = My.Settings.ERRLOG_FOLDER
                fileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.cstrDateFomatShow) & ".log"

                Call ExportWebLogFile()
                Call DeleteInquiryData()
                Call EMCDataImportProcessing()
                System.Threading.Thread.Sleep(intExe)

            Loop
        End If

    End Sub

    ''' <summary>
    ''' 設定ファイルチェック
    ''' </summary>
    ''' <param name="strErrMsg"></param>
    ''' <returns></returns>
    Public Shared Function xCheckSetting(ByRef strErrMsg As StringBuilder) As Boolean
        xCheckSetting = False

        Try
            If My.Settings.DB_HOSTNAME.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "DBホスト名に値がありません。")

            End If

            If My.Settings.DB_DBNAME.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "DB名称に値がありません。")

            End If

            If My.Settings.DB_USERNAME.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "接続ユーザ名に値がありません。")

            End If

            If My.Settings.DB_USERPASSWORD.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "接続ユーザパスワードに値がありません。")

            End If

            If My.Settings.MAIL_POP_HOST.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "POPサーバのホスト名に値がありません。")

            End If

            If My.Settings.MAIL_POP_PORT.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "POPサーバのポートに値がありません。")

            End If

            If My.Settings.MAIL_POP_USER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "POPサーバのユーザ名に値がありません。")

            End If

            If My.Settings.MAIL_POP_PASS.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "POPサーバのパスワードに値がありません。")

            End If

            If My.Settings.MAIL_SMTP_HOST.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "SMTPサーバのホスト名に値がありません。")

            End If

            If My.Settings.MAIL_SMTP_PORT.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "SMTPサーバのポートに値がありません。")

            End If

            If My.Settings.MAIL_SMTP_USER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "SMTPサーバのユーザ名に値がありません。")

            End If

            If My.Settings.MAIL_SMTP_PASS.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "SMTPサーバのパスワードに値がありません。")

            End If

            If My.Settings.EXECUTION_CYCLE.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "実行周期に値がありません。")

            ElseIf Not IsNumeric(My.Settings.EXECUTION_CYCLE) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "実行周期が数値ではありません。")

            End If

            If My.Settings.LOG_FOLDER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "ログ出力フォルダに値がありません。")

            ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.LOG_FOLDER) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "ログ出力フォルダパスが存在しません。")

            End If

            If My.Settings.WEB_LOG_FOLDER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "Webシステムログ出力フォルダに値がありません。")

            ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.WEB_LOG_FOLDER) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "Webシステムログ出力フォルダのパスが存在しません。")

            End If

            If My.Settings.ERRLOG_FOLDER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "エラーログ出力フォルダに値がありません。")

            ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.ERRLOG_FOLDER) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "エラーログ出力フォルダのパスが存在しません。")

            End If

            If My.Settings.QUERY_RETENTION_PERIOD.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "問い合わせ保存期間に値がありません。")

            ElseIf Not IsNumeric(My.Settings.QUERY_RETENTION_PERIOD) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "問い合わせ保存期間が数値ではありません。")

            End If

            If My.Settings.REG_USER_NO.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "登録者に値がありません。")

            End If

            If My.Settings.FROM_MAILADDRESS.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "メール送信者のメールアドレスに値がありません。")

            End If

            If strErrMsg.ToString() <> "" Then
                Return False
            End If

            xCheckSetting = True
        Catch ex As Exception

            'Write log
            Dim messageLog As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EmcDataImport.vb" & " " & "xCheckSetting" & " " & ex.Message
            xWriteLog(logErrFolderPath, fileErrNameLog, messageLog)

        End Try

    End Function

    ''' <summary>
    ''' 設定ファイルチェック
    ''' エラーログを出力
    ''' </summary>
    ''' <param name="strErrMsg"></param>
    Public Shared Sub xOutPutErrLog(ByVal strErrMsg As StringBuilder)

        Dim errFolderPath As String = Application.StartupPath & "\ErrLog"

        If (Not System.IO.Directory.Exists(errFolderPath)) Then
            System.IO.Directory.CreateDirectory(errFolderPath)
        End If

        Dim fileNameLog As String = "ErrLog_" & DateTime.Now.ToString(clsConst.cstrDateFomatShow) & ".log"
        Dim messageLog As String = strErrMsg.ToString()
        xWriteLog(errFolderPath, fileNameLog, messageLog)

    End Sub

    ''' <summary>
    ''' WEBシステムログファイル出力
    ''' </summary>
    Private Shared Sub ExportWebLogFile()
        Dim hasTransaction As Boolean
        Dim webLogFile As StreamWriter
        Dim clsDb As clsDbAccess
        clsDb = New clsDbAccess
        Dim reader As SqlDataReader = Nothing
        Try
            clsDb.Open(connectionSql)
            reader = clsDb.GetDataTLog()

            If reader.HasRows Then

                Dim reg_date As String = ""

                While reader.Read()

                    Dim res As String = DateTime.Parse(reader("REG_DATE")).ToString(clsConst.cstrDateFomatShow)
                    If reg_date = "" Or res <> reg_date Then
                        webLogFile = New StreamWriter(My.Settings.WEB_LOG_FOLDER & "\" & res & ".txt")
                        webLogFile.WriteLine(reader("CONTENT").Trim().TrimEnd(vbCrLf))
                        webLogFile.Close()

                        reg_date = res
                    Else
                        webLogFile = New StreamWriter(My.Settings.WEB_LOG_FOLDER & "\" & res & ".txt", True)
                        webLogFile.WriteLine(reader("CONTENT").Trim().TrimEnd(vbCrLf))
                        webLogFile.Close()
                    End If

                End While

                reader.Close()

                hasTransaction = clsDb.BeginTransaction()
                clsDb.DeleteDataWebLog()
                If hasTransaction Then clsDb.Commit()
                'Write log
                Dim messageLog As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "WEBシステムログファイル出力"
                xWriteLog(logFolderPath, fileNameLog, messageLog)
                '---------------'

            End If

        Catch ex As Exception

            If hasTransaction Then clsDb.RollBack()
            'Write log
            Dim messageLog As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EmcDataImport.vb" & " " & "ExportWebLogFile" & " " & ex.Message
            xWriteLog(logErrFolderPath, fileErrNameLog, messageLog)

        Finally
            If reader IsNot Nothing Then
                reader.Close()
            End If

            reader = Nothing

            If clsDb IsNot Nothing Then
                clsDb.Close()
            End If

            clsDb = Nothing

        End Try

    End Sub

    ''' <summary>
    '''	問い合わせデータ削除
    ''' </summary>
    Private Shared Sub DeleteInquiryData()
        Dim hasTransaction As Boolean
        Dim clsDb As clsDbAccess = Nothing

        Try
            Dim reader As SqlDataReader
            clsDb = New clsDbAccess
            clsDb.Open(connectionSql)
            reader = clsDb.GetInquiryData()

            If reader.HasRows Then

                reader.Close()

                hasTransaction = clsDb.BeginTransaction()
                clsDb.DeleteInquiryData()

                If hasTransaction Then clsDb.Commit()
                'Write log
                Dim messageLog As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "問い合わせデータ削除"
                xWriteLog(logFolderPath, fileNameLog, messageLog)
                '---------------'
            End If

        Catch ex As Exception

            If hasTransaction Then clsDb.RollBack()
            'Write log
            Dim messageLog As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EmcDataImport.vb" & " " & "DeleteInquiryData" & " " & ex.Message
            xWriteLog(logErrFolderPath, fileErrNameLog, messageLog)

        Finally
            If clsDb IsNot Nothing Then
                clsDb.Close()
            End If

            clsDb = Nothing

        End Try

    End Sub

    ''' <summary>
    ''' Funtion Common Write log
    ''' </summary>
    ''' <param name="logPath"></param>
    ''' <param name="fileName"></param>
    ''' <param name="messageLog"></param>
    Public Shared Sub xWriteLog(logPath As String, fileName As String, messageLog As String)

        If String.IsNullOrWhiteSpace(messageLog) Then
            Return
        End If

        Dim sw As StreamWriter = Nothing

        Dim fileLogPath As String = logPath & "\" & fileName

        Try
            If File.Exists(fileLogPath) Then

                Dim fileUpdateDate As Date = File.GetLastWriteTime(fileLogPath)
                Dim dtCheck As Date = fileUpdateDate.AddYears(1)
                Dim dtNow As Date = DateTime.Now

                If dtCheck.ToString("yyyyMMdd") = dtNow.ToString("yyyyMMdd") Then
                    File.SetCreationTime(fileLogPath, DateTime.Now)
                    sw = New StreamWriter(fileLogPath)
                Else
                    sw = New StreamWriter(fileLogPath, True)
                End If
            Else
                sw = New StreamWriter(fileLogPath, True)
            End If

            messageLog = messageLog.Trim().TrimEnd(vbCrLf)
            sw.WriteLine(messageLog)

        Catch ex As Exception
            Throw
        Finally
            If Not sw Is Nothing Then
                CType(sw, IDisposable).Dispose()
            End If
        End Try

    End Sub

    ''' <summary>
    ''' EMCデータインポート処理
    ''' </summary>
    Private Shared Sub EMCDataImportProcessing()

        'Write log
        Dim messageLog As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EMCデータインポート開始"
        xWriteLog(logFolderPath, fileNameLog, messageLog)
        '---------------'

        Dim clsDb As clsDbAccess = Nothing
        Dim hasTransaction As Boolean

        Dim hostname As String
        Dim username As String
        Dim password As String
        Dim strPort As String

        Dim intIdx As Integer
        Dim intIdx2 As Integer
        Dim intIdx3 As Integer


        hostname = My.Settings.MAIL_POP_HOST
        username = My.Settings.MAIL_POP_USER
        password = My.Settings.MAIL_POP_PASS
        strPort = My.Settings.MAIL_POP_PORT

        If Not IsNumeric(strPort) Then
            Exit Sub
        End If

        Try
            ' POP サーバに接続します。
            Dim pop As PopClient = New PopClient(hostname, CInt(strPort))
            pop.Login(username, password)

            ' POP サーバに溜まっているメールのリストを取得します。
            Dim listMail As ArrayList = pop.GetList()

            If listMail.Count = 0 Or listMail Is Nothing Then
                'Write log
                Dim messLogEmailCountEmpty As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "メール件数 0"
                xWriteLog(logFolderPath, fileNameLog, messLogEmailCountEmpty)

                Dim messLogEnd As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EMCデータインポート 終了"
                xWriteLog(logFolderPath, fileNameLog, messLogEnd)
                '---------------'
                pop.Close()
                Exit Sub
            End If

            'Get listMailaddress from m_user
            Dim lstMailAddress As List(Of String)
            lstMailAddress = GetListMailAddress()

            If lstMailAddress.Count = 0 Or lstMailAddress Is Nothing Then
                'Write log
                Dim messLogEmailCountEmpty As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "メール件数 0"
                xWriteLog(logFolderPath, fileNameLog, messLogEmailCountEmpty)

                Dim messLogEnd As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EMCデータインポート 終了"
                xWriteLog(logFolderPath, fileNameLog, messLogEnd)
                '---------------'
                pop.Close()
                Exit Sub
            End If

            'when listMail > 0
            'Write log
            Dim messLogEmailCount As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "メール件数 " & " " & listMail.Count
            xWriteLog(logFolderPath, fileNameLog, messLogEmailCount)
            '---------------'

            'Get subject and body mail
            For i As Integer = 0 To listMail.Count - 1
                hasTransaction = False
                Try
                    ' メール本体を取得します。
                    Dim mail As String = pop.GetMail(CType(listMail(i), String))

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

                        Dim bytes As Byte()
                        Dim mymailbody As String
                        Dim mymailSubject As String()

                        mymailSubject = mymail.Header("Subject")


                        '"Content-Transfer-Encoding:"検索
                        Dim strEncoding As String = ""
                        intIdx = InStr(mail, "Content-Transfer-Encoding")
                        If intIdx > 0 Then
                            strEncoding = Mid(mail, intIdx)
                            intIdx = InStr(strEncoding, ":")
                            intIdx3 = InStr(strEncoding, " ")
                            If intIdx + 1 = intIdx3 Then
                                strEncoding = Mid(strEncoding, intIdx3 + 1)
                                intIdx3 = InStr(strEncoding, vbCrLf)
                                strEncoding = Mid(strEncoding, 1, intIdx3)
                                strEncoding = Trim(Replace(strEncoding, vbCr, ""))
                                strEncoding = Trim(Replace(strEncoding, vbCrLf, ""))
                                strEncoding = Trim(Replace(strEncoding, """", ""))
                            End If
                        End If



                        If mymail.Body.Multiparts.Length = 0 Then
                            '添付ファイルなし
                            If StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("base64", VbStrConv.Uppercase) Then

                                Dim data() As Byte
                                data = System.Convert.FromBase64String(mymail.Body.Text)
                                mymailbody = Encoding.GetEncoding(strBuf).GetString(data)

                            ElseIf StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("quoted-printable", VbStrConv.Uppercase) Then
                                mymailbody = DecodeQuotedPrintable(mymail.Body.Text, strBuf)
                            Else

                                bytes = Encoding.ASCII.GetBytes(mymail.Body.Text)
                                mymailbody = Encoding.GetEncoding(strBuf).GetString(bytes)


                            End If

                        Else

                            If StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("base64", VbStrConv.Uppercase) Then
                                Dim part1 As MailMultipart = mymail.Body.Multiparts(0)
                                Dim data() As Byte
                                data = System.Convert.FromBase64String(part1.Body.Text)
                                mymailbody = Encoding.GetEncoding(strBuf).GetString(data)

                            ElseIf StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("quoted-printable", VbStrConv.Uppercase) Then
                                Dim part1 As MailMultipart = mymail.Body.Multiparts(0)
                                mymailbody = DecodeQuotedPrintable(part1.Body.Text, strBuf)
                            Else
                                Dim part1 As MailMultipart = mymail.Body.Multiparts(0)
                                bytes = Encoding.ASCII.GetBytes(part1.Body.Text)
                                mymailbody = Encoding.GetEncoding(strBuf).GetString(bytes)
                            End If


                        End If


                        'Check mailsubject and mailbody 
                        Dim subjectMail As String = Nothing

                        If mymailSubject.Count > 0 Then
                            subjectMail = Trim(Mid(MailHeader.Decode(mymail.Header("Subject")(0)), 9))
                        End If

                        If ((mymailSubject.Count = 0 Or subjectMail.Trim = "") And String.IsNullOrEmpty(mymailbody.Trim)) Then
                            'Write err log
                            Dim errMessLogEmail As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名および本文に値がありませんでした。"
                            xWriteLog(logErrFolderPath, fileErrNameLog, errMessLogEmail)
                            '---------------'
                            pop.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

                        ElseIf String.IsNullOrEmpty(mymailbody.Trim) Then

                            'Write err log
                            Dim errMessLogEmail As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "本文に値がありませんでした。"
                            xWriteLog(logErrFolderPath, fileErrNameLog, errMessLogEmail)
                            '---------------'
                            pop.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

                        ElseIf mymailSubject.Count = 0 Or subjectMail.Trim = "" Then
                            'Write err log
                            Dim errMessLogEmail As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名に値がありませんでした。"
                            xWriteLog(logErrFolderPath, fileErrNameLog, errMessLogEmail)
                            '---------------'
                            pop.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

                        End If


                        Dim announceNo As Integer
                        Dim titleJPN As String
                        Dim contentsJPN As String

                        announceNo = CreateKeyAnnounceNo()
                        titleJPN = Trim(Mid(MailHeader.Decode(mymail.Header("Subject")(0)), 9))
                        contentsJPN = Trim(mymailbody)

                        clsDb = New clsDbAccess
                        clsDb.Open(connectionSql)

                        hasTransaction = clsDb.BeginTransaction()

                        clsDb.InsertDataAnnounce(announceNo, titleJPN, contentsJPN)

                        Dim toMail = String.Join(",", lstMailAddress)

                        SendMail(titleJPN, contentsJPN, toMail)

                        pop.DeleteMail(CType(listMail(i), String))

                        If hasTransaction Then clsDb.Commit()

                    Else

                        'Write err log 
                        Dim errMessLogEmail As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名および本文に値がありませんでした。"
                        xWriteLog(logErrFolderPath, fileErrNameLog, errMessLogEmail)
                        '---------------'
                        pop.DeleteMail(CType(listMail(i), String))
                        '---------------'
                        Continue For

                    End If

                Catch ex As Exception

                    If hasTransaction Then clsDb.RollBack()
                    'Write log
                    Dim messageErrLog As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & ex.Message
                    xWriteLog(logErrFolderPath, fileErrNameLog, messageErrLog)
                Finally
                    If clsDb IsNot Nothing Then
                        clsDb.Close()
                    End If

                    clsDb = Nothing
                End Try
            Next

            pop.Close()

            'wirte log
            Dim messLogExportEnd As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EMCデータインポート 終了"
            xWriteLog(logFolderPath, fileNameLog, messLogExportEnd)

        Catch ex As Exception
            'Write log
            Dim messageErrLog As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & ex.Message
            xWriteLog(logErrFolderPath, fileErrNameLog, messageErrLog)

        Finally
            If clsDb IsNot Nothing Then
                clsDb.Close()
            End If

            clsDb = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' メールを送る
    ''' </summary>
    ''' <param name="subject"></param>
    ''' <param name="bodyMail"></param>
    ''' <param name="toMail"></param>
    ''' <returns></returns>
    Private Shared Function SendMail(subject As String, bodyMail As String, toMail As String) As Boolean

        Dim fromMail As String = My.Settings.FROM_MAILADDRESS
        Dim userName As String = My.Settings.MAIL_SMTP_USER
        Dim password As String = My.Settings.MAIL_SMTP_PASS
        Dim smtpAddress As String = My.Settings.MAIL_SMTP_HOST
        Dim portNumber As Integer = My.Settings.MAIL_SMTP_PORT

        Try
            Using mail As New MailMessage()
                mail.From = New MailAddress(fromMail)
                mail.[To].Add(toMail)
                mail.Subject = subject
                mail.Body = bodyMail

                Using smtp As New SmtpClient(smtpAddress, portNumber)
                    smtp.UseDefaultCredentials = False
                    smtp.Credentials = New NetworkCredential(fromMail, password)
                    smtp.EnableSsl = False
                    smtp.Send(mail)

                    Return True

                End Using

            End Using
        Catch ex As Exception

            Return False
        End Try

    End Function

    ''' <summary>
    ''' リストのメールアドレスを取得する
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function GetListMailAddress() As List(Of String)
        Dim dtMailAddress As DataTable
        Dim lstMailAddress As List(Of String)
        Dim clsDb As clsDbAccess
        clsDb = New clsDbAccess

        Try

            clsDb.Open(connectionSql)

            dtMailAddress = clsDb.GetAllMailAddress()

            lstMailAddress = (From r In dtMailAddress.AsEnumerable() Select r.Field(Of String)(0)).ToList()

            clsDb.Close()
            clsDb = Nothing

            Return lstMailAddress

        Catch ex As Exception

            Return Nothing

        Finally
            If clsDb IsNot Nothing Then
                clsDb.Close()
            End If

            clsDb = Nothing

        End Try

    End Function

    ''' <summary>
    ''' Create key: [announce_no]
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function CreateKeyAnnounceNo() As Integer
        Dim clsDb As clsDbAccess
        clsDb = New clsDbAccess
        Dim announceNo As Integer

        Try
            Dim result As DataTable
            clsDb.Open(connectionSql)
            result = clsDb.GetKeyForAnnounceNo()
            If result.Rows.Count > 0 Then
                For i As Integer = 0 To result.Rows.Count - 1
                    announceNo = (result.Rows(i)(0))
                Next
            End If

            clsDb.Close()
            clsDb = Nothing

        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        Finally
            If clsDb IsNot Nothing Then
                clsDb.Close()
            End If

            clsDb = Nothing

        End Try

        Return announceNo
    End Function


    Private Shared Function DecodeQuotedPrintable(input As String, chatset As String) As String
        Dim occurences = New Regex("(=[0-9A-Z][0-9A-Z])+", RegexOptions.Multiline)
        Dim matches = occurences.Matches(input)
        For Each m As Match In matches
            Dim bytes As Byte() = New Byte(m.Value.Length / 3 - 1) {}
            Dim i As Integer = 0
            While i < bytes.Length
                Dim hex As String = m.Value.Substring(i * 3 + 1, 2)
                Dim iHex As Integer = Convert.ToInt32(hex, 16)
                bytes(i) = Convert.ToByte(iHex)
                System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            End While
            input = input.Replace(m.Value, Encoding.GetEncoding(chatset).GetString(bytes))
        Next
        Return input.Replace("=rn", "")
    End Function

End Class
