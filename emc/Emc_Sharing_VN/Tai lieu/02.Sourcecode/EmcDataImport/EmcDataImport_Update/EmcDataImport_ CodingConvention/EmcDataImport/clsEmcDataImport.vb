﻿'   ******************************************************************
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
'      CREATE     : 2020/02/18　AKB　Cuong.
'      UPDATE     : 
'   ******************************************************************
Public Class clsEmcDataImport

    Private Shared mstrConnectionSql As String = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", My.Settings.DB_DBNAME, My.Settings.DB_HOSTNAME, My.Settings.DB_USERNAME, My.Settings.DB_USERPASSWORD)  ' DB Connection string.
    Public Shared gstrLogFolderPath As String           'Folder log path
    Public Shared gstrFileNameLog As String             'File log name
    Public Shared gstrLogErrFolderPath As String        'Folder error log path 
    Public Shared gstrFileErrNameLog As String          'File error log name

    Private Shared mblnGoingToDie As Boolean = False     'Variable check loop
    Private Shared mMainThread As Thread                 'Thread name
    Private Shared mstrCharset As String                 'Charset name
    Private Shared mclsDb As clsDbAccess


    '   ******************************************************************
    '      FUNCTION   : サービスを開始する
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong.
    '      UPDATE     : 
    '   ******************************************************************
    Protected Overrides Sub OnStart(ByVal args() As String)

        Dim objStrBuilder As New System.Text.StringBuilder

        If Not xCheckSetting(objStrBuilder) Then
            'エラーログを出力
            Call xOutPutErrLog(objStrBuilder)

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

        If mMainThread.ThreadState <> ThreadState.Stopped Or mMainThread.ThreadState <> ThreadState.Aborted Then
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

        Dim strExcutionCycle As String = My.Settings.MAIL_POP_USER

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
    '      PARAMS     : (objStrErrMsg StringBuilder, Content log append to)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Function xCheckSetting(ByRef objStrErrMsg As StringBuilder) As Boolean
        xCheckSetting = False

        Try
            If My.Settings.DB_DBNAME.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "DBホスト名に値がありません。")

            End If

            If My.Settings.DB_HOSTNAME.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "DB名称に値がありません。")

            End If

            If My.Settings.DB_USERNAME.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "接続ユーザ名に値がありません。")

            End If

            If My.Settings.DB_USERPASSWORD.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "接続ユーザパスワードに値がありません。")

            End If

            If My.Settings.DEBUGFLG.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのホスト名に値がありません。")

            End If

            If My.Settings.ERRLOG_FOLDER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのポートに値がありません。")

            End If

            If My.Settings.EXECUTION_CYCLE.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのユーザ名に値がありません。")

            End If

            If My.Settings.FROM_MAILADDRESS.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのパスワードに値がありません。")

            End If

            If My.Settings.MAIL_SMTP_HOST.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのホスト名に値がありません。")

            End If

            If My.Settings.MAIL_SMTP_PORT.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのポートに値がありません。")

            End If

            If My.Settings.MAIL_SMTP_USER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのユーザ名に値がありません。")

            End If

            If My.Settings.MAIL_SMTP_PASS.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのパスワードに値がありません。")

            End If

            If My.Settings.MAIL_POP_USER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "実行周期に値がありません。")

            ElseIf Not IsNumeric(My.Settings.MAIL_POP_USER) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "実行周期が数値ではありません。")

            End If

            If My.Settings.LOG_FOLDER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "ログ出力フォルダに値がありません。")

            ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.LOG_FOLDER) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "ログ出力フォルダパスが存在しません。")

            End If

            If My.Settings.WEB_LOG_FOLDER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "Webシステムログ出力フォルダに値がありません。")

            ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.WEB_LOG_FOLDER) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "Webシステムログ出力フォルダのパスが存在しません。")

            End If

            If My.Settings.MAIL_POP_PORT.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "エラーログ出力フォルダに値がありません。")

            ElseIf Not My.Computer.FileSystem.DirectoryExists(My.Settings.MAIL_POP_PORT) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "エラーログ出力フォルダのパスが存在しません。")

            End If

            If My.Settings.QUERY_RETENTION_PERIOD.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "問い合わせ保存期間に値がありません。")

            ElseIf Not IsNumeric(My.Settings.QUERY_RETENTION_PERIOD) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "問い合わせ保存期間が数値ではありません。")

            End If

            If My.Settings.REG_USER_NO.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "登録者に値がありません。")

            End If

            If My.Settings.MAIL_POP_PASS.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "メール送信者のメールアドレスに値がありません。")

            End If

            If My.Settings.MAIL_DESTINATION_ADMIN_ADDRESS.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "送信先管理者メールアドレスに値がありません。")

            End If

            If My.Settings.MAIL_SUBMIT_NUMBER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "一斉送信件数に値がありません。")

            ElseIf Not IsNumeric(My.Settings.MAIL_SUBMIT_NUMBER) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "一斉送信件数が数値ではありません。")

            End If

            If My.Settings.MAIL_SUBMIT_TITLE_JPN.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "日本語メール送信タイトルに値がありません。")

            End If

            If My.Settings.MAIL_SUBMIT_TITLE_ENG.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "英語メール送信タイトルに値がありません。")

            End If

            If My.Settings.MAIL_CONTENTS_JPN.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "日本語メール内容ファイル名に値がありません。")

            ElseIf Not System.IO.File.Exists(Application.StartupPath + "\" + My.Settings.MAIL_CONTENTS_JPN) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "日本語メール内容ファイル名が存在しません。")

            End If

            If My.Settings.MAIL_CONTENTS_ENG.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "英語メール内容ファイル名に値がありません。")

            ElseIf Not System.IO.File.Exists(Application.StartupPath + "\" + My.Settings.MAIL_CONTENTS_ENG) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "英語メール内容ファイル名に値がありません。")

            End If

            If objStrErrMsg.ToString() <> "" Then
                Return False
            End If

            xCheckSetting = True
        Catch ex As Exception

            'Write log
            gstrLogErrFolderPath = My.Settings.MAIL_POP_PORT
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "xCheckSetting" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageLog)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : 設定ファイルチェック エラーログを出力
    '　　　VALUE      : None.
    '      PARAMS     : (objStrErrMsg StringBuilder, Content log)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Sub xOutPutErrLog(ByVal objStrErrMsg As StringBuilder)

        Dim strErrFolderPath As String = Application.StartupPath & "\ErrLog"

        If (Not System.IO.Directory.Exists(strErrFolderPath)) Then
            System.IO.Directory.CreateDirectory(strErrFolderPath)
        End If

        'Write log
        Dim strFileNameLog As String = "ErrLog_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
        Dim strMessageLog As String = objStrErrMsg.ToString()
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
        Dim objStreamWriteWebLogFile As StreamWriter
        mclsDb = New clsDbAccess
        Dim objSqlDataReader As SqlDataReader = Nothing
        Try
            mclsDb.Open(mstrConnectionSql)
            objSqlDataReader = mclsDb.GetDataTLog()

            If objSqlDataReader.HasRows Then

                Dim strReg_date As String = ""

                While objSqlDataReader.Read()

                    Dim strRes As String = DateTime.Parse(objSqlDataReader("REG_DATE")).ToString(clsConst.gcstr_DATE_FORMAT_SHOW)
                    If strReg_date = "" Or strRes <> strReg_date Then
                        objStreamWriteWebLogFile = New StreamWriter(My.Settings.WEB_LOG_FOLDER & "\" & strRes & ".txt")
                        objStreamWriteWebLogFile.WriteLine(objSqlDataReader("CONTENT").Trim().TrimEnd(vbCrLf))
                        objStreamWriteWebLogFile.Close()

                        strReg_date = strRes
                    Else
                        objStreamWriteWebLogFile = New StreamWriter(My.Settings.WEB_LOG_FOLDER & "\" & strRes & ".txt", True)
                        objStreamWriteWebLogFile.WriteLine(objSqlDataReader("CONTENT").Trim().TrimEnd(vbCrLf))
                        objStreamWriteWebLogFile.Close()
                    End If

                End While

                objSqlDataReader.Close()

                blnHasTransaction = mclsDb.BeginTransaction()
                mclsDb.DeleteDataWebLog()
                If blnHasTransaction Then mclsDb.Commit()

                'Write log
                gstrLogFolderPath = My.Settings.LOG_FOLDER
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "WEBシステムログファイル出力"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessageLog)
                '---------------'

            End If

        Catch ex As Exception

            If blnHasTransaction Then mclsDb.RollBack()

            'Write error log
            gstrLogErrFolderPath = My.Settings.MAIL_POP_PORT
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "ExportWebLogFile" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageLog)

        Finally
            If objSqlDataReader IsNot Nothing Then
                objSqlDataReader.Close()
            End If

            objSqlDataReader = Nothing

            If mclsDb IsNot Nothing Then
                mclsDb.Close()
            End If

            mclsDb = Nothing

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
        Try
            Dim objSqlDataReader As SqlDataReader
            mclsDb = New clsDbAccess
            mclsDb.Open(mstrConnectionSql)
            objSqlDataReader = mclsDb.GetInquiryData()

            If objSqlDataReader.HasRows Then

                objSqlDataReader.Close()

                blnHasTransaction = mclsDb.BeginTransaction()
                mclsDb.DeleteInquiryData()

                If blnHasTransaction Then mclsDb.Commit()
                'Write log
                gstrLogFolderPath = My.Settings.LOG_FOLDER
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "問い合わせデータ削除"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessageLog)
                '---------------'
            End If

        Catch ex As Exception

            If blnHasTransaction Then mclsDb.RollBack()

            'Write error log
            gstrLogErrFolderPath = My.Settings.MAIL_POP_PORT
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "DeleteInquiryData" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageLog)

        Finally
            If mclsDb IsNot Nothing Then
                mclsDb.Close()
            End If

            mclsDb = Nothing

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : Common write log.
    '　　　VALUE      : None.
    '      PARAMS     : (strLogPath String, Folder log path)
    '                   (strFileName String, File log name)
    '                   (strMessageLog String, Content log)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Sub xWriteLog(ByVal strLogPath As String, ByVal strFileName As String, ByRef strMessageLog As String)

        If String.IsNullOrWhiteSpace(strMessageLog) Then
            Return
        End If

        Dim objStreamWrite As StreamWriter = Nothing

        Dim strFileLogPath As String = strLogPath & "\" & strFileName

        Try
            If File.Exists(strFileLogPath) Then

                Dim dtmFileUpdateDate As Date = File.GetLastWriteTime(strFileLogPath)
                Dim dtmCheck As Date = dtmFileUpdateDate.AddYears(1)
                Dim dtmNow As Date = DateTime.Now

                If dtmCheck.ToString(clsConst.gcstr_DATETIME_FORMAT_NO_TIME) = dtmNow.ToString(clsConst.gcstr_DATETIME_FORMAT_NO_TIME) Then

                    objStreamWrite = New StreamWriter(strFileLogPath)
                Else
                    objStreamWrite = New StreamWriter(strFileLogPath, True)
                End If
            Else
                objStreamWrite = New StreamWriter(strFileLogPath, True)
            End If

            strMessageLog = strMessageLog.Trim().TrimEnd(vbCrLf)
            objStreamWrite.WriteLine(strMessageLog)

        Catch ex As Exception
            Throw
        Finally
            If Not objStreamWrite Is Nothing Then
                CType(objStreamWrite, IDisposable).Dispose()
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

        Dim blnHasTransaction As Boolean

        Dim strHostname As String
        Dim strUsername As String
        Dim strPassword As String
        Dim strPort As String

        Dim intIdx As Integer
        Dim intIdx2 As Integer
        Dim intIdx3 As Integer

        strHostname = My.Settings.DEBUGFLG
        strUsername = My.Settings.EXECUTION_CYCLE
        strPassword = My.Settings.FROM_MAILADDRESS
        strPort = My.Settings.ERRLOG_FOLDER

        If Not IsNumeric(strPort) Then
            Exit Sub
        End If

        Try
            ' POP サーバに接続します。
            Dim clsPopclient As clsPopClient = New clsPopClient(strHostname, CInt(strPort))
            clsPopclient.Login(strUsername, strPassword)

            ' POP サーバに溜まっているメールのリストを取得します。
            Dim listMail As ArrayList = clsPopclient.GetList()

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
                clsPopclient.Close()
                Exit Sub
            End If

            'Get listMailaddress from [m_user]
            Dim lstObjMailAddress As List(Of clsMailAddress)
            lstObjMailAddress = GetListMailAddress()

            If lstObjMailAddress Is Nothing Or lstObjMailAddress.Count = 0 Then

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
                clsPopclient.Close()
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
                    Dim strMail As String = clsPopclient.GetMail(CType(listMail(i), String))

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

                        Dim bytDatas As Byte()
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

                                bytDatas = Encoding.ASCII.GetBytes(mymail.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytDatas)


                            End If

                        Else

                            If StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("base64", VbStrConv.Uppercase) Then
                                Dim clsMailMulti As clsMailMultipart = mymail.Body.Multiparts(0)
                                Dim bytData() As Byte
                                bytData = System.Convert.FromBase64String(clsMailMulti.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytData)

                            ElseIf StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("quoted-printable", VbStrConv.Uppercase) Then
                                Dim clsMailMulti As clsMailMultipart = mymail.Body.Multiparts(0)
                                strMyMailBody = DecodeQuotedPrintable(clsMailMulti.Body.Text, mstrCharset)
                            Else
                                Dim clsMailMulti As clsMailMultipart = mymail.Body.Multiparts(0)
                                bytDatas = Encoding.ASCII.GetBytes(clsMailMulti.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytDatas)
                            End If


                        End If


                        'Check mailsubject and mailbody: null, empty
                        Dim strSubjectMail As String = Nothing

                        If strMyMailSubject.Count > 0 Then
                            strSubjectMail = Trim(Mid(clsMailHeader.Decode(mymail.Header("Subject")(0)), 9))
                        End If

                        If ((strMyMailSubject.Count = 0 Or strSubjectMail.Trim = "") And String.IsNullOrEmpty(strMyMailBody.Trim)) Then

                            'Write error log
                            gstrLogErrFolderPath = My.Settings.MAIL_POP_PORT
                            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                            Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名および本文に値がありませんでした。"
                            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                            '---------------'
                            clsPopclient.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

                        ElseIf String.IsNullOrEmpty(strMyMailBody.Trim) Then

                            'Write error log
                            gstrLogErrFolderPath = My.Settings.MAIL_POP_PORT
                            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                            Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "本文に値がありませんでした。"
                            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                            '---------------'
                            clsPopclient.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

                        ElseIf strMyMailSubject.Count = 0 Or strSubjectMail.Trim = "" Then

                            'Write error log
                            gstrLogErrFolderPath = My.Settings.MAIL_POP_PORT
                            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                            Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名に値がありませんでした。"
                            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                            '---------------'
                            clsPopclient.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

                        End If

                        Dim intAnnounceNo As Integer
                        Dim strTitleJPN As String
                        Dim strContentsJPN As String

                        intAnnounceNo = CreateKeyAnnounceNo()
                        strTitleJPN = Trim(Mid(clsMailHeader.Decode(mymail.Header("Subject")(0)), 9))
                        strContentsJPN = Trim(strMyMailBody)

                        mclsDb = New clsDbAccess
                        mclsDb.Open(mstrConnectionSql)
                        blnHasTransaction = mclsDb.BeginTransaction()

                        mclsDb.InsertDataAnnounce(intAnnounceNo, strTitleJPN, strContentsJPN)

                        SendMail(lstObjMailAddress)       'Send mail

                        clsPopclient.DeleteMail(CType(listMail(i), String))          'Delete mail from pop server

                        If blnHasTransaction Then mclsDb.Commit()

                    Else

                        'Write error log
                        gstrLogErrFolderPath = My.Settings.MAIL_POP_PORT
                        gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                        Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名および本文に値がありませんでした。"
                        xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                        '---------------'
                        clsPopclient.DeleteMail(CType(listMail(i), String))
                        '---------------'
                        Continue For

                    End If
                Catch ex As Exception

                    If blnHasTransaction Then mclsDb.RollBack()
                    'Write err log
                    gstrLogErrFolderPath = My.Settings.MAIL_POP_PORT
                    gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                    Dim strMessageErrLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & ex.Message
                    xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageErrLog)
                Finally
                    If mclsDb IsNot Nothing Then
                        mclsDb.Close()
                    End If

                    mclsDb = Nothing
                End Try
            Next

            clsPopclient.Close()

            'Write log
            gstrLogFolderPath = My.Settings.LOG_FOLDER
            gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessLogExportEnd As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EMCデータインポート 終了"
            xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessLogExportEnd)

        Catch ex As Exception
            'Write error log
            gstrLogErrFolderPath = My.Settings.MAIL_POP_PORT
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageErrLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageErrLog)

        Finally
            If mclsDb IsNot Nothing Then
                mclsDb.Close()
            End If

            mclsDb = Nothing
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : メールを送る.
    '　　　VALUE      : Boolean   True: send mail success, Fail: send mail fail.
    '      PARAMS     : (strSubject String, subject mail)
    '                   (strBodyMail String, content mail body text)
    '                   (strToMail String, list mail address)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Sub SendMail(ByVal lstObjMailAddress As List(Of clsMailAddress))

        Dim intChuckSize As Integer = My.Settings.MAIL_SUBMIT_NUMBER
        Dim listMailAddresJPN = (From objMailAddress In lstObjMailAddress Where objMailAddress.languagetype = 0 Select objMailAddress.mailaddress).ToList()
        Dim listMailAddresENG = (From objMailAddress In lstObjMailAddress Where objMailAddress.languagetype = 1 Select objMailAddress.mailaddress).ToList()

        Dim strBodyMail As String

        If listMailAddresJPN IsNot Nothing And listMailAddresJPN.Count > 0 Then

            strBodyMail = My.Computer.FileSystem.ReadAllText(Application.StartupPath + "\" + My.Settings.MAIL_CONTENTS_JPN)
            strBodyMail = strBodyMail.Replace("%0%", Format(DateTime.Now(), "M月dd日 HH時mm分"))
            strBodyMail = strBodyMail.Replace("%1%", "お知らせ（EMC）")
            strBodyMail = strBodyMail.Replace(vbCrLf, "<br/>")

            Dim lstSplits = SplitIntoChunks(listMailAddresJPN, intChuckSize)
            Dim strSubject = My.Settings.MAIL_SUBMIT_TITLE_JPN

            For index = 0 To lstSplits.Count - 1
                Dim strBccMailJPN = String.Join(",", lstSplits(index))
                Send(strSubject, strBodyMail, strBccMailJPN)
            Next

        End If

        If listMailAddresENG IsNot Nothing And listMailAddresENG.Count > 0 Then

            strBodyMail = My.Computer.FileSystem.ReadAllText(Application.StartupPath + "\" + My.Settings.MAIL_CONTENTS_JPN)
            strBodyMail = strBodyMail.Replace("%0%", Format(DateTime.Now(), "HH:mm, dd ") & DateTime.Now.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture))
            strBodyMail = strBodyMail.Replace("%1%", "Information (EMC)")
            strBodyMail = strBodyMail.Replace(vbCrLf, "<br/>")

            Dim lstSplitsMailAddress = SplitIntoChunks(listMailAddresJPN, intChuckSize)
            Dim strSubject = My.Settings.MAIL_SUBMIT_TITLE_ENG

            For index = 0 To lstSplitsMailAddress.Count - 1
                Dim strBccMailJPN = String.Join(",", lstSplitsMailAddress(index))
                Send(strSubject, strBodyMail, strBccMailJPN)
            Next

        End If

    End Sub

    Private Shared Function Send(ByVal strSubject As String, ByVal strBodyMail As String, ByVal strBccMailJPN As String) As Boolean
        Dim strFromMail As String = My.Settings.MAIL_POP_PASS
        Dim strToMail As String = My.Settings.MAIL_DESTINATION_ADMIN_ADDRESS
        Dim strUserName As String = My.Settings.MAIL_SMTP_USER
        Dim strPassword As String = My.Settings.MAIL_SMTP_PASS
        Dim strSmtpAddress As String = My.Settings.MAIL_SMTP_HOST
        Dim intPortNumber As Integer = My.Settings.MAIL_SMTP_PORT

        Try
            Using mail As New MailMessage()
                mail.From = New MailAddress(strFromMail)
                mail.[To].Add(strToMail)
                mail.Bcc.Add(strBccMailJPN)
                mail.Subject = strSubject
                mail.Body = strBodyMail
                'mail.BodyEncoding = Text.Encoding.GetEncoding(mstrCharset)

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


    Private Shared Function SplitIntoChunks(ByVal keys As List(Of String), chunkSize As Integer) As List(Of List(Of String))
        Return keys.
            Select(Function(x, i) New With {Key .Index = i, Key .Value = x}).
            GroupBy(Function(x) (x.Index \ chunkSize)).
            Select(Function(x) x.Select(Function(v) v.Value).ToList()).
            ToList()
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : リストのメールアドレスを取得する.
    '　　　VALUE      : List<Of String>.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Function GetListMailAddress() As List(Of clsMailAddress)
        Dim objDataTableMailAddress As DataTable
        Dim convertedListObjMailAddress As List(Of clsMailAddress) = New List(Of clsMailAddress)()
        'Dim lstMailAddress As List(Of String) = Nothing
        mclsDb = New clsDbAccess

        Try

            mclsDb.Open(mstrConnectionSql)

            objDataTableMailAddress = mclsDb.GetAllMailAddress()

            'lstMailAddress = (From r In objDataTableMailAddress.AsEnumerable() Select r.Field(Of String)(0)).ToList()

            If objDataTableMailAddress IsNot Nothing And objDataTableMailAddress.Rows.Count > 0 Then

                convertedListObjMailAddress = (From rw In objDataTableMailAddress.AsEnumerable()
                                               Select New clsMailAddress With {
                                                       .mailaddress = Convert.ToString(rw("MAIL_ADDRESS")),
                                                       .languagetype = Convert.ToDecimal(rw("LANGUAGE_TYPE"))
                                                       }).ToList()
            End If

            Return convertedListObjMailAddress

        Catch ex As Exception

            Throw New Exception(ex.Message, ex)

        Finally
            If mclsDb IsNot Nothing Then
                mclsDb.Close()
            End If

            mclsDb = Nothing

        End Try

        'Return convertedListObjMailAddress

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

        mclsDb = New clsDbAccess
        Dim intAnnounceNo As Integer

        Try
            Dim objResult As DataTable
            mclsDb.Open(mstrConnectionSql)
            objResult = mclsDb.GetKeyForAnnounceNo()

            If objResult Is Nothing Or objResult.Rows.Count = 0 Then
                Return intAnnounceNo = -1
            End If

            If objResult.Rows.Count > 0 Then
                For i As Integer = 0 To objResult.Rows.Count - 1
                    intAnnounceNo = (objResult.Rows(i)(0))
                Next
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        Finally
            If mclsDb IsNot Nothing Then
                mclsDb.Close()
            End If

            mclsDb = Nothing

        End Try

        Return intAnnounceNo
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : Decode: QuatedPrintable.
    '　　　VALUE      : String.
    '      PARAMS     : (strInput String, Content needs decode)
    '                   (strChatset String, GetEncoding by chatset)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Function DecodeQuotedPrintable(ByRef strInput As String, ByVal strChatset As String) As String
        Dim occurences = New Regex("(=[0-9A-Z][0-9A-Z])+", RegexOptions.Multiline)
        Dim matches = occurences.Matches(strInput)
        For Each match As Match In matches
            Dim bytes As Byte() = New Byte(match.Value.Length / 3 - 1) {}
            Dim intIndex As Integer = 0
            While intIndex < bytes.Length
                Dim strHex As String = match.Value.Substring(intIndex * 3 + 1, 2)
                Dim intHex As Integer = Convert.ToInt32(strHex, 16)
                bytes(intIndex) = Convert.ToByte(intHex)
                System.Math.Max(System.Threading.Interlocked.Increment(intIndex), intIndex - 1)
            End While
            strInput = strInput.Replace(match.Value, Encoding.GetEncoding(strChatset).GetString(bytes))
        Next
        Return strInput.Replace("=rn", "")
    End Function

    Class clsMailAddress
        Public mailaddress As String
        Public languagetype As Decimal
    End Class
End Class
