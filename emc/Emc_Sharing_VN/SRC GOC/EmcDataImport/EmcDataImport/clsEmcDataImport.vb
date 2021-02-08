'   ******************************************************************
'      TITLE      : Information sharing system.
'      FUNCTION   : Service processing.
'      MEMO       : None.
'      CREATE     : 2020/02/18　AKB　Cuong.
'      UPDATE     : .
'
'           2020 AKBSOFTWARE CORPORATION
'   ******************************************************************

Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms


'   ******************************************************************
'      FUNCTION   : Service processing.
'      MEMO       : None.
'      CREATE     : 2020/02/28　AKB　Cuong.
'      UPDATE     : 
'   ******************************************************************
Public Class clsEmcDataImport

    Private Shared mstrConnectionSql As String = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", My.Settings.DB_HOSTNAME, My.Settings.DB_DBNAME, My.Settings.DB_USERNAME, My.Settings.DB_USERPASSWORD)  ' DB Connection string.
    Public Shared gstrLogFolderPath As String           'Folder log path
    Public Shared gstrFileNameLog As String             'File log name
    Public Shared gstrLogErrFolderPath As String        'Folder error log path 
    Public Shared gstrFileErrNameLog As String          'File error log name

    Private Shared mblnGoingToDie As Boolean = False     'Variable check loop
    Private Shared mMainThread As Thread                 'Thread name
    Private Shared mstrCharset As String                 'Charset name


    '   ******************************************************************
    '      FUNCTION   : サービスを開始する
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong.
    '      UPDATE     : 
    '   ******************************************************************
    Protected Overrides Sub OnStart(ByVal args() As String)

        Dim strBuilder As New System.Text.StringBuilder

        If Not xCheckSetting(strBuilder) Then
            'エラーログを出力
            Call xOutPutErrLog(strBuilder)

            OnStop()
        Else
            mMainThread = New Thread(AddressOf Runner)
            mMainThread.Start()
        End If

    End Sub


    '   ******************************************************************
    '      FUNCTION   : サービスを停止する
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong.
    '      UPDATE     : 
    '   ******************************************************************
    Protected Overrides Sub OnStop()

        If mMainThread.ThreadState <> ThreadState.Stopped And mMainThread.ThreadState <> ThreadState.Aborted Then
            mblnGoingToDie = True
            mMainThread.Join()
        End If

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : ストリーム処理.
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Sub Runner()

        Dim strExcutionCycle As String = My.Settings.EXECUTION_CYCLE

        If IsNumeric(strExcutionCycle) Then
            Dim intExcutionCycle As Integer
            intExcutionCycle = CInt(strExcutionCycle)

            '実行周期で実行
            Do Until mblnGoingToDie

                Call ExportWebLogFile()
                Call DeleteInquiryData()
                Call EMCDataImportProcessing()
                System.Threading.Thread.Sleep(intExcutionCycle)

            Loop
        End If

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : 設定ファイルチェック.
    '　　　VALUE      : None.
    '      PARAMS     : (strErrMsg StringBuilder, Content log append to)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Function xCheckSetting(ByRef strErrMsg As StringBuilder) As Boolean
        xCheckSetting = False

        Try
            If My.Settings.DB_HOSTNAME.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "DBホスト名に値がありません。")

            End If

            If My.Settings.DB_DBNAME.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "DB名称に値がありません。")

            End If

            If My.Settings.DB_USERNAME.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "接続ユーザ名に値がありません。")

            End If

            If My.Settings.DB_USERPASSWORD.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "接続ユーザパスワードに値がありません。")

            End If

            If My.Settings.MAIL_POP_HOST.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのホスト名に値がありません。")

            End If

            If My.Settings.MAIL_POP_PORT.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのポートに値がありません。")

            End If

            If My.Settings.MAIL_POP_USER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのユーザ名に値がありません。")

            End If

            If My.Settings.MAIL_POP_PASS.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのパスワードに値がありません。")

            End If

            If My.Settings.MAIL_SMTP_HOST.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのホスト名に値がありません。")

            End If

            If My.Settings.MAIL_SMTP_PORT.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのポートに値がありません。")

            End If

            If My.Settings.MAIL_SMTP_USER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのユーザ名に値がありません。")

            End If

            If My.Settings.MAIL_SMTP_PASS.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのパスワードに値がありません。")

            End If

            If My.Settings.EXECUTION_CYCLE.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "実行周期に値がありません。")

            ElseIf Not IsNumeric(My.Settings.EXECUTION_CYCLE) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "実行周期が数値ではありません。")

            End If

            If My.Settings.LOG_FOLDER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "ログ出力フォルダに値がありません。")

            ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.LOG_FOLDER) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "ログ出力フォルダパスが存在しません。")

            End If

            If My.Settings.WEB_LOG_FOLDER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "Webシステムログ出力フォルダに値がありません。")

            ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.WEB_LOG_FOLDER) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "Webシステムログ出力フォルダのパスが存在しません。")

            End If

            If My.Settings.ERRLOG_FOLDER.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "エラーログ出力フォルダに値がありません。")

            ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.ERRLOG_FOLDER) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "エラーログ出力フォルダのパスが存在しません。")

            End If

            If My.Settings.QUERY_RETENTION_PERIOD.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "問い合わせ保存期間に値がありません。")

            ElseIf Not IsNumeric(My.Settings.QUERY_RETENTION_PERIOD) Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "問い合わせ保存期間が数値ではありません。")

            End If

            If My.Settings.REG_USER_NO.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "登録者に値がありません。")

            End If

            If My.Settings.FROM_MAILADDRESS.Trim.Length = 0 Then
                strErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "メール送信者のメールアドレスに値がありません。")

            End If

            If strErrMsg.ToString() <> "" Then
                Return False
            End If

            xCheckSetting = True
        Catch ex As Exception

            'Write log
            gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "xCheckSetting" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageLog)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : 設定ファイルチェック エラーログを出力
    '　　　VALUE      : None.
    '      PARAMS     : (strErrMsg StringBuilder, Content log)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Sub xOutPutErrLog(ByVal strErrMsg As StringBuilder)

        Dim strErrFolderPath As String = Application.StartupPath & "\ErrLog"

        If (Not System.IO.Directory.Exists(strErrFolderPath)) Then
            System.IO.Directory.CreateDirectory(strErrFolderPath)
        End If

        'Write log
        Dim strFileNameLog As String = "ErrLog_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
        Dim strMessageLog As String = strErrMsg.ToString()
        xWriteLog(strErrFolderPath, strFileNameLog, strMessageLog)

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : WEBシステムログファイル出力
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Sub ExportWebLogFile()
        Dim blnHasTransaction As Boolean
        Dim wrWebLogFile As StreamWriter
        Dim clsDb As clsDbAccess
        clsDb = New clsDbAccess
        Dim sqlDataReader As SqlDataReader = Nothing
        Try
            clsDb.Open(mstrConnectionSql)
            sqlDataReader = clsDb.GetDataTLog()

            If sqlDataReader.HasRows Then

                Dim reg_date As String = ""

                While sqlDataReader.Read()

                    Dim res As String = DateTime.Parse(sqlDataReader("REG_DATE")).ToString(clsConst.gcstr_DATE_FORMAT_SHOW)
                    If reg_date = "" Or res <> reg_date Then
                        wrWebLogFile = New StreamWriter(My.Settings.WEB_LOG_FOLDER & "\" & res & ".txt")
                        wrWebLogFile.WriteLine(sqlDataReader("CONTENT").Trim().TrimEnd(vbCrLf))
                        wrWebLogFile.Close()

                        reg_date = res
                    Else
                        wrWebLogFile = New StreamWriter(My.Settings.WEB_LOG_FOLDER & "\" & res & ".txt", True)
                        wrWebLogFile.WriteLine(sqlDataReader("CONTENT").Trim().TrimEnd(vbCrLf))
                        wrWebLogFile.Close()
                    End If

                End While

                sqlDataReader.Close()

                blnHasTransaction = clsDb.BeginTransaction()
                clsDb.DeleteDataWebLog()
                If blnHasTransaction Then clsDb.Commit()

                'Write log
                gstrLogFolderPath = My.Settings.LOG_FOLDER
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "WEBシステムログファイル出力"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessageLog)
                '---------------'

            End If

        Catch ex As Exception

            If blnHasTransaction Then clsDb.RollBack()

            'Write error log
            gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "ExportWebLogFile" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageLog)

        Finally
            If sqlDataReader IsNot Nothing Then
                sqlDataReader.Close()
            End If

            sqlDataReader = Nothing

            If clsDb IsNot Nothing Then
                clsDb.Close()
            End If

            clsDb = Nothing

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : 問い合わせデータ削除
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Sub DeleteInquiryData()
        Dim blnHasTransaction As Boolean
        Dim clsDb As clsDbAccess = Nothing

        Try
            Dim sqlDataReader As SqlDataReader
            clsDb = New clsDbAccess
            clsDb.Open(mstrConnectionSql)
            sqlDataReader = clsDb.GetInquiryData()

            If sqlDataReader.HasRows Then

                sqlDataReader.Close()

                blnHasTransaction = clsDb.BeginTransaction()
                clsDb.DeleteInquiryData()

                If blnHasTransaction Then clsDb.Commit()
                'Write log
                gstrLogFolderPath = My.Settings.LOG_FOLDER
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "問い合わせデータ削除"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessageLog)
                '---------------'
            End If

        Catch ex As Exception

            If blnHasTransaction Then clsDb.RollBack()

            'Write error log
            gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "DeleteInquiryData" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageLog)

        Finally
            If clsDb IsNot Nothing Then
                clsDb.Close()
            End If

            clsDb = Nothing

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : Common write log.
    '　　　VALUE      : None.
    '      PARAMS     : (logPath String, Folder log path)
    '                   (fileName String, File log name)
    '                   (messageLog String, Content log)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Sub xWriteLog(logPath As String, fileName As String, messageLog As String)

        If String.IsNullOrWhiteSpace(messageLog) Then
            Return
        End If

        Dim sw As StreamWriter = Nothing

        Dim strFileLogPath As String = logPath & "\" & fileName

        Try
            If File.Exists(strFileLogPath) Then

                Dim dtmFileUpdateDate As Date = File.GetLastWriteTime(strFileLogPath)
                Dim dtmCheck As Date = dtmFileUpdateDate.AddYears(1)
                Dim dtmNow As Date = DateTime.Now

                If dtmCheck.ToString(clsConst.gcstr_DATETIME_FORMAT_NO_TIME) = dtmNow.ToString(clsConst.gcstr_DATETIME_FORMAT_NO_TIME) Then

                    sw = New StreamWriter(strFileLogPath)
                Else
                    sw = New StreamWriter(strFileLogPath, True)
                End If
            Else
                sw = New StreamWriter(strFileLogPath, True)
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


    '   ******************************************************************
    '　　　FUNCTION   : EMCデータインポート処理.
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Sub EMCDataImportProcessing()

        'Write log
        gstrLogFolderPath = My.Settings.LOG_FOLDER
        gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
        Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EMCデータインポート開始"
        xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessageLog)
        '---------------'

        Dim clsDb As clsDbAccess = Nothing
        Dim blnHasTransaction As Boolean

        Dim strHostname As String
        Dim strUsername As String
        Dim strPassword As String
        Dim strPort As String

        Dim intIdx As Integer
        Dim intIdx2 As Integer
        Dim intIdx3 As Integer

        strHostname = My.Settings.MAIL_POP_HOST
        strUsername = My.Settings.MAIL_POP_USER
        strPassword = My.Settings.MAIL_POP_PASS
        strPort = My.Settings.MAIL_POP_PORT

        If Not IsNumeric(strPort) Then
            Exit Sub
        End If

        Try
            ' POP サーバに接続します。
            Dim pop As clsPopClient = New clsPopClient(strHostname, CInt(strPort))
            pop.Login(strUsername, strPassword)

            ' POP サーバに溜まっているメールのリストを取得します。
            Dim listMail As ArrayList = pop.GetList()

            If listMail.Count = 0 Or listMail Is Nothing Then

                'Write log
                gstrLogFolderPath = My.Settings.LOG_FOLDER
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessLogEmailCountEmpty As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "メール件数 0"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessLogEmailCountEmpty)

                gstrLogFolderPath = My.Settings.LOG_FOLDER
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessLogEnd As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EMCデータインポート 終了"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessLogEnd)
                '---------------'
                pop.Close()
                Exit Sub
            End If

            'Get listMailaddress from [m_user]
            Dim lstMailAddress As List(Of String)
            lstMailAddress = GetListMailAddress()

            If lstMailAddress.Count = 0 Or lstMailAddress Is Nothing Then

                'Write log
                gstrLogFolderPath = My.Settings.LOG_FOLDER
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessLogEmailCountEmpty As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "メール件数 0"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessLogEmailCountEmpty)

                gstrLogFolderPath = My.Settings.LOG_FOLDER
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessLogEnd As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EMCデータインポート 終了"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessLogEnd)
                '---------------'
                pop.Close()
            End If

            'when listMail > 0
            'Write log
            gstrLogFolderPath = My.Settings.LOG_FOLDER
            gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessLogEmailCount As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "メール件数 " & " " & listMail.Count
            xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessLogEmailCount)
            '---------------'

            'Get subject and body mail
            For i As Integer = 0 To listMail.Count - 1
                blnHasTransaction = False
                Try
                    ' メール本体を取得します。
                    Dim strMail As String = pop.GetMail(CType(listMail(i), String))

                    ' Mail クラスを作成します。
                    Dim mymail As clsMail = New clsMail(strMail)

                    '"charset"検索
                    intIdx = InStr(strMail, "charset")
                    If intIdx > 0 Then
                        mstrCharset = Mid(strMail, intIdx)

                        intIdx = InStr(mstrCharset, "=")
                        intIdx3 = InStr(mstrCharset, """")
                        If (intIdx + 1 = intIdx3) Or (intIdx + 2 = intIdx3) Then
                            mstrCharset = Mid(mstrCharset, intIdx3 + 1)
                            intIdx = 0
                            intIdx2 = InStr(mstrCharset, """")
                        Else
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, "&")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, " ")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, ";")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, """")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, vbCr)
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, vbCrLf)
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then
                            intIdx2 = intIdx3
                        End If

                        If intIdx2 > 0 Then
                            mstrCharset = Mid(mstrCharset, intIdx + 1, (intIdx2 - intIdx) - 1)
                        End If
                        mstrCharset = Trim(Replace(mstrCharset, vbCr, ""))
                        mstrCharset = Trim(Replace(mstrCharset, vbCrLf, ""))
                        mstrCharset = Trim(Replace(mstrCharset, """", ""))

                        Dim bytes As Byte()
                        Dim strMyMailBody As String
                        Dim strMyMailSubject As String()

                        strMyMailSubject = mymail.Header("Subject")

                        '"Content-Transfer-Encoding:"検索
                        Dim strEncoding As String = ""
                        intIdx = InStr(strMail, "Content-Transfer-Encoding")
                        If intIdx > 0 Then
                            strEncoding = Mid(strMail, intIdx)
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

                                Dim bytData() As Byte
                                bytData = System.Convert.FromBase64String(mymail.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytData)

                            ElseIf StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("quoted-printable", VbStrConv.Uppercase) Then
                                strMyMailBody = DecodeQuotedPrintable(mymail.Body.Text, mstrCharset)
                            Else

                                bytes = Encoding.ASCII.GetBytes(mymail.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytes)


                            End If

                        Else

                            If StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("base64", VbStrConv.Uppercase) Then
                                Dim part1 As clsMailMultipart = mymail.Body.Multiparts(0)
                                Dim bytData() As Byte
                                bytData = System.Convert.FromBase64String(part1.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytData)

                            ElseIf StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("quoted-printable", VbStrConv.Uppercase) Then
                                Dim part1 As clsMailMultipart = mymail.Body.Multiparts(0)
                                strMyMailBody = DecodeQuotedPrintable(part1.Body.Text, mstrCharset)
                            Else
                                Dim part1 As clsMailMultipart = mymail.Body.Multiparts(0)
                                bytes = Encoding.ASCII.GetBytes(part1.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytes)
                            End If


                        End If


                        'Check mailsubject and mailbody: null, empty
                        Dim strSubjectMail As String = Nothing

                        If strMyMailSubject.Count > 0 Then
                            strSubjectMail = Trim(Mid(clsMailHeader.Decode(mymail.Header("Subject")(0)), 9))
                        End If

                        If ((strMyMailSubject.Count = 0 Or strSubjectMail.Trim = "") And String.IsNullOrEmpty(strMyMailBody.Trim)) Then

                            'Write error log
                            gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
                            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                            Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名および本文に値がありませんでした。"
                            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                            '---------------'
                            pop.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

                        ElseIf String.IsNullOrEmpty(strMyMailBody.Trim) Then

                            'Write error log
                            gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
                            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                            Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "本文に値がありませんでした。"
                            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                            '---------------'
                            pop.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

                        ElseIf strMyMailSubject.Count = 0 Or strSubjectMail.Trim = "" Then

                            'Write error log
                            gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
                            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                            Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名に値がありませんでした。"
                            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                            '---------------'
                            pop.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

                        End If

                        Dim intAnnounceNo As Integer
                        Dim strTitleJPN As String
                        Dim strContentsJPN As String

                        intAnnounceNo = CreateKeyAnnounceNo()
                        strTitleJPN = Trim(Mid(clsMailHeader.Decode(mymail.Header("Subject")(0)), 9))
                        strContentsJPN = Trim(strMyMailBody)

                        clsDb = New clsDbAccess
                        clsDb.Open(mstrConnectionSql)
                        blnHasTransaction = clsDb.BeginTransaction()

                        clsDb.InsertDataAnnounce(intAnnounceNo, strTitleJPN, strContentsJPN)

                        Dim strToMail = String.Join(",", lstMailAddress)
                        SendMail(strTitleJPN, strContentsJPN, strToMail)       'Send mail

                        pop.DeleteMail(CType(listMail(i), String))          'Delete mail from pop server

                        If blnHasTransaction Then clsDb.Commit()

                    Else

                        'Write error log
                        gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
                        gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                        Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名および本文に値がありませんでした。"
                        xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                        '---------------'
                        pop.DeleteMail(CType(listMail(i), String))
                        '---------------'
                        Continue For

                    End If
                Catch ex As Exception

                    If blnHasTransaction Then clsDb.RollBack()
                    'Write err log
                    gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
                    gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                    Dim strMessageErrLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & ex.Message
                    xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageErrLog)
                Finally
                    If clsDb IsNot Nothing Then
                        clsDb.Close()
                    End If

                    clsDb = Nothing
                End Try
            Next

            pop.Close()

            'Write log
            gstrLogFolderPath = My.Settings.LOG_FOLDER
            gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessLogExportEnd As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EMCデータインポート 終了"
            xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessLogExportEnd)

        Catch ex As Exception
            'Write error log
            gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageErrLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageErrLog)

        Finally
            If clsDb IsNot Nothing Then
                clsDb.Close()
            End If

            clsDb = Nothing
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : メールを送る.
    '　　　VALUE      : Boolean   True: send mail success, Fail: send mail fail.
    '      PARAMS     : (subject String, subject mail)
    '                   (bodyMail String, content mail body text)
    '                   (toMail String, list mail address)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Function SendMail(subject As String, bodyMail As String, toMail As String) As Boolean
        Dim strFromMail As String = My.Settings.FROM_MAILADDRESS
        Dim strUserName As String = My.Settings.MAIL_SMTP_USER
        Dim strPassword As String = My.Settings.MAIL_SMTP_PASS
        Dim strSmtpAddress As String = My.Settings.MAIL_SMTP_HOST
        Dim intPortNumber As Integer = My.Settings.MAIL_SMTP_PORT

        Try
            Using mail As New MailMessage()
                mail.From = New MailAddress(strFromMail)
                mail.[To].Add(toMail)
                mail.Subject = subject
                mail.Body = bodyMail
                mail.BodyEncoding = Text.Encoding.GetEncoding(mstrCharset)

                Using smtp As New SmtpClient(strSmtpAddress, intPortNumber)
                    smtp.UseDefaultCredentials = False
                    smtp.Credentials = New NetworkCredential(strFromMail, strPassword)
                    smtp.EnableSsl = False
                    smtp.Send(mail)

                    Return True

                End Using

            End Using

        Catch ex As Exception

            Return False

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : リストのメールアドレスを取得する.
    '　　　VALUE      : List<Of String>.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Function GetListMailAddress() As List(Of String)
        Dim dtMailAddress As DataTable
        Dim lstMailAddress As List(Of String)
        Dim clsDb As clsDbAccess
        clsDb = New clsDbAccess

        Try

            clsDb.Open(mstrConnectionSql)

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


    '   ******************************************************************
    '　　　FUNCTION   : Create key: [announce_no].
    '　　　VALUE      : Integer.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Function CreateKeyAnnounceNo() As Integer
        Dim clsDb As clsDbAccess
        clsDb = New clsDbAccess
        Dim intAnnounceNo As Integer

        Try
            Dim result As DataTable
            clsDb.Open(mstrConnectionSql)
            result = clsDb.GetKeyForAnnounceNo()
            If result.Rows.Count > 0 Then
                For i As Integer = 0 To result.Rows.Count - 1
                    intAnnounceNo = (result.Rows(i)(0))
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

        Return intAnnounceNo
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : Decode: QuatedPrintable.
    '　　　VALUE      : String.
    '      PARAMS     : (input String, Content needs decode)
    '                   (chatset String, GetEncoding by chatset)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Function DecodeQuotedPrintable(input As String, chatset As String) As String
        Dim occurences = New Regex("(=[0-9A-Z][0-9A-Z])+", RegexOptions.Multiline)
        Dim matches = occurences.Matches(input)
        For Each m As Match In matches
            Dim bytes As Byte() = New Byte(m.Value.Length / 3 - 1) {}
            Dim i As Integer = 0
            While i < bytes.Length
                Dim strHex As String = m.Value.Substring(i * 3 + 1, 2)
                Dim intHex As Integer = Convert.ToInt32(strHex, 16)
                bytes(i) = Convert.ToByte(intHex)
                System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            End While
            input = input.Replace(m.Value, Encoding.GetEncoding(chatset).GetString(bytes))
        Next
        Return input.Replace("=rn", "")
    End Function

End Class
