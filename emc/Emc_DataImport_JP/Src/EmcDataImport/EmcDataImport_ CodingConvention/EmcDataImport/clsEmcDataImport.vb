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
Imports System.Net.Sockets
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

    Private Shared mstrConnectionSql As String = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", My.Settings.DB_HOSTNAME, My.Settings.DB_DBNAME, My.Settings.DB_USERNAME, My.Settings.DB_USERPASSWORD)  ' DB Connection string.
    '▽2020/04/22 Cuong --- S --- 追加
    Public Shared gstrLogFolderPath As String = My.Settings.SHARE_FOLDER + "\" + My.Settings.LOG_FOLDER             'Folder log path
    Public Shared gstrLogErrFolderPath As String = My.Settings.SHARE_FOLDER + "\" + My.Settings.ERRLOG_FOLDER       'Folder error log path 
    '△2020/04/22 Cuong --- E --- 追加
    Public Shared gstrFileNameLog As String             'File log name
    Public Shared gstrFileErrNameLog As String          'File error log name

    Private Shared mblnGoingToDie As Boolean = False     'Variable check loop
    Private Shared mMainThread As Thread                 'Thread name
    Private Shared mstrCharset As String                 'Charset name
    Private Shared mclsDb As clsDbAccess
    '▽2020/04/08 Cuong --- S --- 追加
    Private Shared mobjQuotedPrintableRegex As New Regex("=(?<hexchars>[0-9a-fA-F]{2,2})", RegexOptions.IgnoreCase Or RegexOptions.Compiled)  'Regex quated-printable.
    '△2020/04/08 Cuong --- E --- 追加

    '   ******************************************************************
    '      FUNCTION   : サービスを開始する
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong.
    '      UPDATE     : 
    '   ******************************************************************
    Protected Overrides Sub OnStart(ByVal args() As String)

        Dim objStrBuilder As New System.Text.StringBuilder

		'check file setting if result false : write log else start service
        If Not xCheckSetting(objStrBuilder) Then
            'エラーログを出力
            Call xOutPutErrLog(objStrBuilder)           'write log setting

            OnStop()                                    'stop service
        Else
            mMainThread = New Thread(AddressOf Runner)
            mMainThread.Start()                         'start service
        End If

    End Sub


    '   ******************************************************************
    '      FUNCTION   : サービスを停止する
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong.
    '      UPDATE     : 
    '   ******************************************************************
    Protected Overrides Sub OnStop()

    'check status mMainThread  -> join thread
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
    '      PARAMS     : (objStrErrMsg StringBuilder, Content log append to)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Function xCheckSetting(ByRef objStrErrMsg As StringBuilder) As Boolean
        xCheckSetting = False

        Try
			'Check DB_HOSTNAME empty: true -> append error message empty.
            If My.Settings.DB_HOSTNAME.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "DBホスト名に値がありません。")

            End If

			'Check DB_DBNAME empty: true -> append error message empty.
            If My.Settings.DB_DBNAME.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "DB名称に値がありません。")

            End If

            'Check DB_USERNAME empty: true -> append error message empty.
			If My.Settings.DB_USERNAME.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "接続ユーザ名に値がありません。")

            End If

			'Check DB_USERPASSWORD empty: true -> append error message empty.
            If My.Settings.DB_USERPASSWORD.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "接続ユーザパスワードに値がありません。")

            End If

            'Check MAIL_POP_HOST empty: true -> append error message empty.
			If My.Settings.MAIL_POP_HOST.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのホスト名に値がありません。")

            End If

			'Check MAIL_POP_PORT empty: true -> append error message empty.
            If My.Settings.MAIL_POP_PORT.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのポートに値がありません。")

            End If

			'Check MAIL_POP_USER empty: true -> append error message empty.
            If My.Settings.MAIL_POP_USER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのユーザ名に値がありません。")

            End If

			'Check MAIL_POP_PASS empty: true -> append error message empty.
            If My.Settings.MAIL_POP_PASS.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "POPサーバのパスワードに値がありません。")

            End If

			'Check MAIL_SMTP_HOST empty: true -> append error message empty.
			If My.Settings.MAIL_SMTP_HOST.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのホスト名に値がありません。")

            End If

			'Check MAIL_SMTP_PORT empty: true -> append error message empty.
            If My.Settings.MAIL_SMTP_PORT.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのポートに値がありません。")

            End If


            '▽2020/04/24 E.Izumi --- S ---　空白可能とするため、コメントアウト
            ''Check MAIL_SMTP_USER empty: true -> append error message empty.
            '         If My.Settings.MAIL_SMTP_USER.Trim.Length = 0 Then
            '             objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのユーザ名に値がありません。")

            '         End If

            ''Check MAIL_SMTP_PASS empty: true -> append error message empty.
            '         If My.Settings.MAIL_SMTP_PASS.Trim.Length = 0 Then
            '             objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "SMTPサーバのパスワードに値がありません。")

            '         End If
            '△2020/04/24 E.Izumi --- E ---　空白可能とするため、コメントアウト


			'Check EXECUTION_CYCLE empty: true -> append error message empty
            If My.Settings.EXECUTION_CYCLE.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "実行周期に値がありません。")

			'Check EXECUTION_CYCLE IsNumeric
            ElseIf Not IsNumeric(My.Settings.EXECUTION_CYCLE) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "実行周期が数値ではありません。")

            End If

			'Check LOG_FOLDER empty: true -> append error message empty
            If My.Settings.LOG_FOLDER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "ログ出力フォルダに値がありません。")

			'Check LOG_FOLDER path Exists
            ElseIf Not Directory.Exists(My.Settings.SHARE_FOLDER + "\" + My.Settings.LOG_FOLDER) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "ログ出力フォルダパスが存在しません。")

            End If

			'Check WEB_LOG_FOLDER empty: true -> append error message empty
            If My.Settings.WEB_LOG_FOLDER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "Webシステムログ出力フォルダに値がありません。")

			'Check WEB_LOG_FOLDER path Exists
            ElseIf Not Directory.Exists(My.Settings.SHARE_FOLDER + "\" + My.Settings.WEB_LOG_FOLDER) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "Webシステムログ出力フォルダのパスが存在しません。")

            End If

			'Check ERRLOG_FOLDER empty: true -> append error message empty
            If My.Settings.ERRLOG_FOLDER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "エラーログ出力フォルダに値がありません。")

			'Check ERRLOG_FOLDER path Exists
            ElseIf Not Directory.Exists(My.Settings.SHARE_FOLDER + "\" + My.Settings.ERRLOG_FOLDER) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "エラーログ出力フォルダのパスが存在しません。")

            End If

			'Check QUERY_RETENTION_PERIOD empty: true -> append error message empty
            If My.Settings.QUERY_RETENTION_PERIOD.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "問い合わせ保存期間に値がありません。")
			ElseIf Not IsNumeric(My.Settings.QUERY_RETENTION_PERIOD) Then
				'Check QUERY_RETENTION_PERIOD IsNumeric
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "問い合わせ保存期間が数値ではありません。")

            End If

			'Check QUERY_RETENTION_PERIOD empty: true -> append error message empty
            If My.Settings.REG_USER_NO.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "登録者に値がありません。")

            End If

			'Check FROM_MAILADDRESS empty: true -> append error message empty
			If My.Settings.FROM_MAILADDRESS.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "メール送信者のメールアドレスに値がありません。")

            End If

			'Check MAIL_DESTINATION_ADMIN_ADDRESS empty: true -> append error message empty
            If My.Settings.MAIL_DESTINATION_ADMIN_ADDRESS.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "送信先管理者メールアドレスに値がありません。")

            End If

			'Check MAIL_SUBMIT_NUMBER empty: true -> append error message empty
            If My.Settings.MAIL_SUBMIT_NUMBER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "一斉送信件数に値がありません。")

            'Check MAIL_SUBMIT_NUMBER Is Numeric
			ElseIf Not IsNumeric(My.Settings.MAIL_SUBMIT_NUMBER) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "一斉送信件数が数値ではありません。")

            End If

			'Check MAIL_SUBMIT_TITLE_JPN empty: true -> append error message empty
            If My.Settings.MAIL_SUBMIT_TITLE_JPN.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "日本語メール送信タイトルに値がありません。")

            End If

			'Check MAIL_SUBMIT_TITLE_ENG empty: true -> append error message empty
            If My.Settings.MAIL_SUBMIT_TITLE_ENG.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "英語メール送信タイトルに値がありません。")

            End If
            '▽2020/04/13 Cuong --- S --- deleteに変更

            'If My.Settings.MAIL_CONTENTS_JPN.Trim.Length = 0 Then
            '    objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "日本語メール内容ファイル名に値がありません。")

            'ElseIf Not System.IO.File.Exists(Application.StartupPath + "\" + My.Settings.MAIL_CONTENTS_JPN) Then
            '    objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "日本語メール内容ファイル名が存在しません。")

            'End If

            'If My.Settings.MAIL_CONTENTS_ENG.Trim.Length = 0 Then
            '    objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "英語メール内容ファイル名に値がありません。")

            'ElseIf Not System.IO.File.Exists(Application.StartupPath + "\" + My.Settings.MAIL_CONTENTS_ENG) Then
            '    objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "英語メール内容ファイル名が存在しません。")

            'End If

            '△2020/04/13 Cuong --- E --- deleteに変更

            '▽2020/04/21 Cuong --- S --- 追加
			'Check SHARE_FOLDER empty: true -> append error message empty
            If My.Settings.SHARE_FOLDER.Trim.Length = 0 Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "共有フォルダパスに値がありません。")

			'Check SHARE_FOLDER path exists
            ElseIf Not Directory.Exists(My.Settings.SHARE_FOLDER) Then
                objStrErrMsg.AppendLine(DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "共有フォルダのパスが存在しません。")

            End If
            '△2020/04/21 Cuong --- E --- 追加

            If objStrErrMsg.ToString() <> "" Then
                Return False
            End If

            xCheckSetting = True
        Catch ex As Exception

            'Write log
            '▽2020/04/22 Cuong --- S --- コメントアウト
            'gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
            '△2020/04/22 Cuong --- E --- コメントアウト
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

		'check folder errlog exists: result false -> create new folder ErrLog
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
        '▽2020/04/22 Cuong --- S --- 追加
        Dim strWebLogPath = My.Settings.SHARE_FOLDER + "\" + My.Settings.WEB_LOG_FOLDER
        '△2020/04/22 Cuong --- E --- 追加
        Dim blnHasTransaction As Boolean
        Dim objStreamWriteWebLogFile As StreamWriter
        mclsDb = New clsDbAccess
        Dim objSqlDataReader As SqlDataReader = Nothing
        Try
            mclsDb.Open(mstrConnectionSql)                  'Open connection DB
            objSqlDataReader = mclsDb.GetDataTLog()         'Get data table [t_log]

			'if has data -> Continue processing
			If objSqlDataReader.HasRows Then

                Dim strReg_date As String = ""
                '▽2020/04/22 Cuong --- S --- 追加
                ' 本日より1年 + 1日以上前のWEBシステムログファイルが存在するなら、ファイルを削除する-
                Dim files() As String = IO.Directory.GetFiles(strWebLogPath)
                For Each file As String In files

                    Dim dtCreateTime = New Date(IO.File.GetCreationTime(file).Year, IO.File.GetCreationTime(file).Month, IO.File.GetCreationTime(file).Day)
                    Dim dtOneYearAgo = New Date(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, DateTime.Now.AddYears(-1).Day)

                    Dim fi As FileInfo = New FileInfo(file)
                    If dtCreateTime < dtOneYearAgo Then fi.Delete()
                Next
                ' ----------------------------------------------------------------------------   
                '△2020/04/22 Cuong --- E --- 追加

                While objSqlDataReader.Read()

                    Dim strRes As String = DateTime.Parse(objSqlDataReader("REG_DATE")).ToString(clsConst.gcstr_DATE_FORMAT_SHOW)
                    If strReg_date = "" Or strRes <> strReg_date Then
                        '▽2020/04/22 Cuong --- S --- 追加
						'file exists -> append "CONTENT" to file ; if file not exists overwite file.
                        If File.Exists(strWebLogPath & "\" & strRes & ".txt") Then
                            objStreamWriteWebLogFile = New StreamWriter(strWebLogPath & "\" & strRes & ".txt", True)
                            objStreamWriteWebLogFile.WriteLine(objSqlDataReader("CONTENT").Trim().TrimEnd(vbCrLf))
                            objStreamWriteWebLogFile.Close()
                        Else
                            objStreamWriteWebLogFile = New StreamWriter(strWebLogPath & "\" & strRes & ".txt")
                            objStreamWriteWebLogFile.WriteLine(objSqlDataReader("CONTENT").Trim().TrimEnd(vbCrLf))
                            objStreamWriteWebLogFile.Close()
                        End If
                        '△2020/04/22 Cuong --- E --- 追加
                        strReg_date = strRes
                    Else
                        objStreamWriteWebLogFile = New StreamWriter(strWebLogPath & "\" & strRes & ".txt", True)
                        objStreamWriteWebLogFile.WriteLine(objSqlDataReader("CONTENT").Trim().TrimEnd(vbCrLf))
                        objStreamWriteWebLogFile.Close()
                    End If

                End While

                objSqlDataReader.Close()

                blnHasTransaction = mclsDb.BeginTransaction()
                mclsDb.DeleteDataWebLog()
                If blnHasTransaction Then mclsDb.Commit()             'check if has transaction (true) -> call Commit()

                'Write log
                '▽2020/04/22 Cuong --- S --- コメントアウト
                'gstrLogFolderPath = My.Settings.LOG_FOLDER
                '△2020/04/22 Cuong --- E --- コメントアウト
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "WEBシステムログファイル出力"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessageLog)
                '---------------'

            End If

        Catch ex As Exception

            If blnHasTransaction Then mclsDb.RollBack()         'check if has transaction (true) -> call RollBack

            'Write error log
            '▽2020/04/22 Cuong --- S --- コメントアウト
            'gstrLogFolderPath = My.Settings.LOG_FOLDER
            '△2020/04/22 Cuong --- E --- コメントアウト
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "ExportWebLogFile" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageLog)

        Finally
            If objSqlDataReader IsNot Nothing Then              'check status SqlDataReader ->Close
                objSqlDataReader.Close()
            End If

            objSqlDataReader = Nothing

            If mclsDb IsNot Nothing Then                        'check status mclsDb  ->Close
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
            objSqlDataReader = mclsDb.GetInquiryData()                  'Get data table [t_query]

			'if has data get from table [t_query] -> Continue processing
            If objSqlDataReader.HasRows Then

                objSqlDataReader.Close()

                blnHasTransaction = mclsDb.BeginTransaction()
                mclsDb.DeleteInquiryData()

                If blnHasTransaction Then mclsDb.Commit()               'check if has transaction (true) -> call RollBack
                'Write log
                '▽2020/04/22 Cuong --- S --- コメントアウト
                'gstrLogFolderPath = My.Settings.LOG_FOLDER
                '△2020/04/22 Cuong --- E --- コメントアウト
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "問い合わせデータ削除"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessageLog)
                '---------------'
            End If

        Catch ex As Exception

            If blnHasTransaction Then mclsDb.RollBack()                 'check if has transaction (true) -> call RollBack

            'Write error log
            '▽2020/04/22 Cuong --- S --- コメントアウト
            'gstrLogErrFolderPath = My.Settings.ERRLOG_FOLDER
            '△2020/04/22 Cuong --- E --- コメントアウト
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "DeleteInquiryData" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageLog)

        Finally
            If mclsDb IsNot Nothing Then                                  'check status object mclsDb  ->Close
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
            '▽2020/04/22 Cuong --- S --- 処理変更

            '    If File.Exists(strFileLogPath) Then

            '        Dim dtmFileUpdateDate As Date = File.GetLastWriteTime(strFileLogPath)
            '        Dim dtmCheck As Date = dtmFileUpdateDate.AddYears(1)
            '        Dim dtmNow As Date = DateTime.Now

            '        If dtmCheck.ToString(clsConst.gcstr_DATETIME_FORMAT_NO_TIME) = dtmNow.ToString(clsConst.gcstr_DATETIME_FORMAT_NO_TIME) Then

            '            objStreamWrite = New StreamWriter(strFileLogPath)
            '        Else
            '            objStreamWrite = New StreamWriter(strFileLogPath, True)
            '        End If
            '    Else
            '        objStreamWrite = New StreamWriter(strFileLogPath, True)
            '    End If

            '▽2020/04/22 Cuong --- S --- 追加
            ' 本日より1年 + 1日以上前のWEBシステムログファイルが存在するなら、ファイルを削除する-
            Dim files() As String = IO.Directory.GetFiles(strLogPath)
            For Each file As String In files

                Dim dtCreateTime = New Date(IO.File.GetCreationTime(file).Year, IO.File.GetCreationTime(file).Month, IO.File.GetCreationTime(file).Day)
                Dim dtOneYearAgo = New Date(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, DateTime.Now.AddYears(-1).Day)

                Dim fi As FileInfo = New FileInfo(file)
                If dtCreateTime <= dtOneYearAgo Then fi.Delete()
            Next
            ' ----------------------------------------------------------------------------   
            '△2020/04/22 Cuong --- E --- 追加

			'if file exists  true-> to append data to the file; false to overwrite the file
            If File.Exists(strFileLogPath) Then

                objStreamWrite = New StreamWriter(strFileLogPath, True)
            Else
                objStreamWrite = New StreamWriter(strFileLogPath, False)
            End If

            strMessageLog = strMessageLog.Trim().TrimEnd(vbCrLf)
            objStreamWrite.WriteLine(strMessageLog)

            '△2020/04/22 Cuong --- E --- 処理変更
        Catch ex As Exception
            Throw
        Finally
			'check status object StreamWrite -> close, dispose
            If Not objStreamWrite Is Nothing Then
                objStreamWrite.Close()
                objStreamWrite.Dispose()
                objStreamWrite = Nothing
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
        '▽2020/04/22 Cuong --- S --- コメントアウト
        'gstrLogFolderPath = My.Settings.LOG_FOLDER
        '△2020/04/22 Cuong --- E --- コメントアウト
        gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
        Dim strMessageLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EMCデータインポート開始"
        xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessageLog)

        Dim blnHasTransaction As Boolean

        Dim strHostname As String
        Dim strUsername As String
        Dim strPassword As String
        Dim strPort As String
        Dim strSmtpAddress As String = My.Settings.MAIL_SMTP_HOST
        Dim intPortNumber As Integer = My.Settings.MAIL_SMTP_PORT

        Dim intIdx As Integer
        Dim intIdx2 As Integer
        Dim intIdx3 As Integer

        strHostname = My.Settings.MAIL_POP_HOST
        strUsername = My.Settings.MAIL_POP_USER
        strPassword = My.Settings.MAIL_POP_PASS
        strPort = My.Settings.MAIL_POP_PORT

        If Not IsNumeric(strPort) Then              'check port is numeric
            Exit Sub
        End If

        Try
            ' POP サーバに接続します。
            Dim clsPopclient As clsPopClient = New clsPopClient(strHostname, CInt(strPort))
            clsPopclient.Login(strUsername, strPassword)

            ' POP サーバに溜まっているメールのリストを取得します。
            Dim listMail As ArrayList = clsPopclient.GetList()
			'check list mail get from pop server: if empty ->write log
            If listMail Is Nothing Or listMail.Count = 0 Then

                'Write log
                '▽2020/04/22 Cuong --- S --- コメントアウト
                'gstrLogFolderPath = My.Settings.LOG_FOLDER
                '△2020/04/22 Cuong --- E --- コメントアウト
                gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                Dim strMessLogEmailCountEmpty As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "メール件数 0"
                xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessLogEmailCountEmpty)

                '▽2020/04/22 Cuong --- S --- コメントアウト
                'gstrLogFolderPath = My.Settings.LOG_FOLDER
                '△2020/04/22 Cuong --- E --- コメントアウト
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

            'when listMail > 0
            'Write log
            '▽2020/04/22 Cuong --- S --- コメントアウト
            'gstrLogFolderPath = My.Settings.LOG_FOLDER
            '△2020/04/22 Cuong --- E --- コメントアウト
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

					'if has not "charset" in mail (false) => title and body mail null -> wirtelog. (true) -> Continue processing
                    If intIdx > 0 Then
                        mstrCharset = Mid(strMail, intIdx)

                        intIdx = InStr(mstrCharset, "=")
                        intIdx3 = InStr(mstrCharset, """")

						'check positon: ->  charset"検索
                        If (intIdx + 1 = intIdx3) Or (intIdx + 2 = intIdx3) Then
                            mstrCharset = Mid(mstrCharset, intIdx3 + 1)
                            intIdx = 0
                            intIdx2 = InStr(mstrCharset, """")
                        Else
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, "&")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then       'check positon: ->  charset"検索
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, " ")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then       'check positon: ->  charset"検索
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, ";")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then       'check positon: ->  charset"検索
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, """")
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then       'check positon: ->  charset"検索
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, vbCr)
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then       'check positon: ->  charset"検索
                            intIdx2 = intIdx3
                        End If

                        intIdx3 = InStr(mstrCharset, vbCrLf)
                        If (intIdx2 = 0 Or intIdx2 > intIdx3) And intIdx3 > 0 Then       'check positon: ->  charset"検索 
                            intIdx2 = intIdx3
                        End If

                        If intIdx2 > 0 Then                                              'check positon: ->  charset"検索
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
                        If intIdx > 0 Then                                               'check positon: ->  Content-Transfer-Encoding:"検索
                            strEncoding = Mid(strMail, intIdx)
                            intIdx = InStr(strEncoding, ":")
                            intIdx3 = InStr(strEncoding, " ")
                            If intIdx + 1 = intIdx3 Then                                 'check positon: ->  Content-Transfer-Encoding:"検索
                                strEncoding = Mid(strEncoding, intIdx3 + 1)
                                intIdx3 = InStr(strEncoding, vbCrLf)
                                strEncoding = Mid(strEncoding, 1, intIdx3)
                                strEncoding = Trim(Replace(strEncoding, vbCr, ""))
                                strEncoding = Trim(Replace(strEncoding, vbCrLf, ""))
                                strEncoding = Trim(Replace(strEncoding, """", ""))
                            End If
                        End If
						'check mail is Multiparts: false-> mail is multipart
                        If mymail.Body.Multiparts.Length = 0 Then
                            '添付ファイルなし
							'check Content-Transfer-Encoding: "base64","quoted-printable", none
                            If StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("base64", VbStrConv.Uppercase) Then

                                Dim bytData() As Byte
                                bytData = System.Convert.FromBase64String(mymail.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytData)

                            ElseIf StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("quoted-printable", VbStrConv.Uppercase) Then
								'check Content-Transfer-Encoding: "base64","quoted-printable", none
                                '▽2020/04/08 Cuong --- S --- decode quoted-printableに変更
                                'strMyMailBody = DecodeQuotedPrintable(mymail.Body.Text, mstrCharset)

                                strMyMailBody = DecodeQuotedPrintableString(mymail.Body.Text)
                                bytDatas = Encoding.ASCII.GetBytes(strMyMailBody)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytDatas)
                                '△2020/04/08 Cuong --- E --- decode quoted-printableに変更
                            Else

                                bytDatas = Encoding.ASCII.GetBytes(mymail.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytDatas)

                            End If

                        Else

							'check Content-Transfer-Encoding: "base64","quoted-printable", none
                            If StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("base64", VbStrConv.Uppercase) Then
                                Dim clsMailMulti As clsMailMultipart = mymail.Body.Multiparts(0)
                                Dim bytData() As Byte
                                bytData = System.Convert.FromBase64String(clsMailMulti.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytData)

                            ElseIf StrConv(strEncoding, VbStrConv.Uppercase) = StrConv("quoted-printable", VbStrConv.Uppercase) Then
								'check Content-Transfer-Encoding: "base64","quoted-printable", none
                                '▽2020/04/08 Cuong --- S --- decode quoted-printableに変更
                                Dim clsMailMulti As clsMailMultipart = mymail.Body.Multiparts(0)
                                strMyMailBody = DecodeQuotedPrintableString(clsMailMulti.Body.Text)
                                bytDatas = Encoding.ASCII.GetBytes(strMyMailBody)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytDatas)

                                '△2020/04/08 Cuong --- E --- decode quoted-printableに変更
                            Else
                                Dim clsMailMulti As clsMailMultipart = mymail.Body.Multiparts(0)
                                bytDatas = Encoding.ASCII.GetBytes(clsMailMulti.Body.Text)
                                strMyMailBody = Encoding.GetEncoding(mstrCharset).GetString(bytDatas)

                            End If


                        End If


                        'Check mailsubject and mailbody: null, empty
                        Dim strSubjectMail As String = Nothing

                        If strMyMailSubject.Count > 0 Then          'check subject empty
                            strSubjectMail = Trim(Mid(clsMailHeader.Decode(mymail.Header("Subject")(0)), 9))
                        End If

						'check subject mail empty, null: if true -> wirte log
                        If ((strMyMailSubject.Count = 0 Or strSubjectMail.Trim = "") And String.IsNullOrEmpty(strMyMailBody.Trim)) Then

                            'Write error log
                            '▽2020/04/22 Cuong --- S --- コメントアウト
                            'gstrLogErrFolderPath =  My.Settings.ERRLOG_FOLDER
                            '△2020/04/22 Cuong --- E --- コメントアウト
                            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                            Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名および本文に値がありませんでした。"
                            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                            '---------------'
                            clsPopclient.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

						'check body mail empty, null: if true -> wirte log
                        ElseIf String.IsNullOrEmpty(strMyMailBody.Trim) Then

                            'Write error log
                            '▽2020/04/22 Cuong --- S --- コメントアウト
                            'gstrLogErrFolderPath =  My.Settings.ERRLOG_FOLDER
                            '△2020/04/22 Cuong --- E --- コメントアウト
                            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                            Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "本文に値がありませんでした。"
                            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                            '---------------'
                            clsPopclient.DeleteMail(CType(listMail(i), String))
                            '---------------'
                            Continue For

						'check subject and body mail: empty, null;  if true -> wirte log
                        ElseIf strMyMailSubject.Count = 0 Or strSubjectMail.Trim = "" Then

                            'Write error log
                            '▽2020/04/22 Cuong --- S --- コメントアウト
                            'gstrLogErrFolderPath =  My.Settings.ERRLOG_FOLDER
                            '△2020/04/22 Cuong --- E --- コメントアウト
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

                        'Insert data to [t_announce]
                        mclsDb.InsertDataAnnounce(intAnnounceNo, strTitleJPN, strContentsJPN)

                        Try
                            'Check connect to smtp
                            Using client = New TcpClient()
                                client.Connect(strSmtpAddress, intPortNumber)
                            End Using

                            '▽2020/04/13 Cuong --- S --- send mailに変更
                            'Send mail
                            'SendMail(lstObjMailAddress)

                            SendMail(lstObjMailAddress, strContentsJPN)
                            '△2020/04/13 Cuong --- E --- send mailに変更

                        Catch ex As Exception
                            'Write err log
                            '▽2020/04/22 Cuong --- S --- コメントアウト
                            'gstrLogErrFolderPath =  My.Settings.ERRLOG_FOLDER
                            '△2020/04/22 Cuong --- E --- コメントアウト
                            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                            Dim strMessageErrLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " SMTP サーバーに接続できませんでした" & " " & strSmtpAddress & ":" & intPortNumber
                            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageErrLog)
                        End Try

                        'Delete mail from pop server
                        clsPopclient.DeleteMail(CType(listMail(i), String))
                        If blnHasTransaction Then mclsDb.Commit()

                    Else

                        'Write error log
                        '▽2020/04/22 Cuong --- S --- コメントアウト
                        'gstrLogErrFolderPath =  My.Settings.ERRLOG_FOLDER
                        '△2020/04/22 Cuong --- E --- コメントアウト
                        gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                        Dim strErrMessLogEmail As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & "件名および本文に値がありませんでした。"
                        xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strErrMessLogEmail)
                        '---------------'
                        clsPopclient.DeleteMail(CType(listMail(i), String))
                        '---------------'
                        Continue For

                    End If
                Catch ex As Exception

                    If blnHasTransaction Then mclsDb.RollBack()        'check if has transaction (true) -> call RollBack
                    'Write err log     
                    '▽2020/04/22 Cuong --- S --- コメントアウト
                    'gstrLogErrFolderPath =  My.Settings.ERRLOG_FOLDER
                    '△2020/04/22 Cuong --- E --- コメントアウト
                    gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
                    Dim strMessageErrLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & ex.Message
                    xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageErrLog)
                Finally
                    If mclsDb IsNot Nothing Then                      'check status object mclsDb -> close
                        mclsDb.Close()
                    End If

                    mclsDb = Nothing
                End Try
            Next

            clsPopclient.Close()

            'Write log
            '▽2020/04/22 Cuong --- S --- コメントアウト
            'gstrLogFolderPath = My.Settings.LOG_FOLDER
            '△2020/04/22 Cuong --- E --- コメントアウト
            gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessLogExportEnd As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EMCデータインポート 終了"
            xWriteLog(gstrLogFolderPath, gstrFileNameLog, strMessLogExportEnd)

        Catch ex As Exception
            'Write error log
            '▽2020/04/22 Cuong --- S --- コメントアウト
            'gstrLogErrFolderPath =  My.Settings.ERRLOG_FOLDER
            '△2020/04/22 Cuong --- E --- コメントアウト
            gstrFileErrNameLog = "ERRLOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessageErrLog As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & "EmcDataImport.vb" & " " & "EMCDataImportProcessing" & " " & ex.Message
            xWriteLog(gstrLogErrFolderPath, gstrFileErrNameLog, strMessageErrLog)

        Finally
            If mclsDb IsNot Nothing Then                              'check status object mclsDb -> close
                mclsDb.Close()
            End If

            mclsDb = Nothing
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : Processing send mail.
    '　　　VALUE      : None.
    '      PARAMS     : (lstObjMailAddress List(Of clsMailAddress), list object clsMailAddress from database)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 2020/04/13　AKB　Cuong
    '   ******************************************************************
    Private Shared Sub SendMail(ByVal lstObjMailAddress As List(Of clsMailAddress), ByVal strContents As String)

        Dim strBodyMail As String
        '▽2020/04/13 Cuong --- S --- 追加
        strBodyMail = strContents.Replace(vbCrLf, "<br/>")
        '△2020/04/13 Cuong --- E --- 追加

		'if list mail address is empty -> send mail not bcc
        If lstObjMailAddress.Count = 0 Then

            '▽2020/04/13 Cuong --- S --- メール本文は設定ファイルから取得処理を削除

            'strBodyMail = My.Computer.FileSystem.ReadAllText(Application.StartupPath + "\" + My.Settings.MAIL_CONTENTS_JPN)
            'strBodyMail = strBodyMail.Replace("%0%", Format(DateTime.Now(), "M月dd日 HH時mm分"))
            'strBodyMail = strBodyMail.Replace("%1%", "お知らせ（EMC）")
            'strBodyMail = strBodyMail.Replace(vbCrLf, "<br/>")

            '△2020/04/13 Cuong --- E --- メール本文は設定ファイルから取得処理を削除

            Dim strSubject = My.Settings.MAIL_SUBMIT_TITLE_JPN
            Send(strSubject, strBodyMail, "")

            Return
        End If

        Dim intChuckSize As Integer = My.Settings.MAIL_SUBMIT_NUMBER
        Dim listMailAddresJPN = (From objMailAddress In lstObjMailAddress Where objMailAddress.decLanguageType = 0 Select objMailAddress.strMailaddress).ToList()
        Dim listMailAddresENG = (From objMailAddress In lstObjMailAddress Where objMailAddress.decLanguageType = 1 Select objMailAddress.strMailaddress).ToList()

		'if has  mail address JPN ->send mail: bcc is list mail address JPN
        If listMailAddresJPN IsNot Nothing And listMailAddresJPN.Count > 0 Then
            '▽2020/04/13 Cuong --- S --- メール本文は設定ファイルから取得処理を削除

            'strBodyMail = My.Computer.FileSystem.ReadAllText(Application.StartupPath + "\" + My.Settings.MAIL_CONTENTS_JPN)
            'strBodyMail = strBodyMail.Replace("%0%", Format(DateTime.Now(), "M月dd日 HH時mm分"))
            'strBodyMail = strBodyMail.Replace("%1%", "お知らせ（EMC）")
            'strBodyMail = strBodyMail.Replace(vbCrLf, "<br/>")

            '△2020/04/13 Cuong --- E --- メール本文は設定ファイルから取得処理を削除

            Dim lstSplitsMailAddress = SplitIntoChunks(listMailAddresJPN, intChuckSize)
            Dim strSubject = My.Settings.MAIL_SUBMIT_TITLE_JPN

            For index = 0 To lstSplitsMailAddress.Count - 1
                Dim strBccMailJPN = String.Join(",", lstSplitsMailAddress(index))
                Send(strSubject, strBodyMail, strBccMailJPN)
            Next

        End If

		'if has  mail address ENG ->send mail: bcc is list mail address ENG
        If listMailAddresENG IsNot Nothing And listMailAddresENG.Count > 0 Then
            '▽2020/04/13 Cuong --- S --- メール本文は設定ファイルから取得処理を削除

            'strBodyMail = My.Computer.FileSystem.ReadAllText(Application.StartupPath + "\" + My.Settings.MAIL_CONTENTS_ENG)
            'strBodyMail = strBodyMail.Replace("%0%", Format(DateTime.Now(), "HH:mm, dd ") & DateTime.Now.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture))
            'strBodyMail = strBodyMail.Replace("%1%", "Information (EMC)")
            'strBodyMail = strBodyMail.Replace(vbCrLf, "<br/>")

            '△2020/04/13 Cuong --- E --- メール本文は設定ファイルから取得処理を削除
            Dim lstSplitsMailAddress = SplitIntoChunks(listMailAddresENG, intChuckSize)
            Dim strSubject = My.Settings.MAIL_SUBMIT_TITLE_ENG

            For index = 0 To lstSplitsMailAddress.Count - 1
                Dim strBccMailENG = String.Join(",", lstSplitsMailAddress(index))
                Send(strSubject, strBodyMail, strBccMailENG)
            Next

        End If

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : メールを送る.
    '　　　VALUE      : Boolean   True: send mail success, Fail: send mail fail.
    '      PARAMS     : (strSubject String, subject mail)
    '                   (strBodyMail String, content mail body text)
    '                   (strBccMailJPN String, Bcc mail)
    '      MEMO       : None.
    '      CREATE     : 2020/02/18　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Function Send(ByVal strSubject As String, ByVal strBodyMail As String, ByVal strBccMail As String) As Boolean

        Dim strFromMail As String = My.Settings.FROM_MAILADDRESS
        Dim strToMail As String = My.Settings.MAIL_DESTINATION_ADMIN_ADDRESS
        Dim strUserName As String = My.Settings.MAIL_SMTP_USER
        Dim strPassword As String = My.Settings.MAIL_SMTP_PASS
        Dim strSmtpAddress As String = My.Settings.MAIL_SMTP_HOST
        Dim intPortNumber As Integer = My.Settings.MAIL_SMTP_PORT

        Try
            Using mail As New MailMessage()
                mail.From = New MailAddress(strFromMail)
                mail.[To].Add(strToMail)
                If Not String.IsNullOrEmpty(strBccMail) Then
                    mail.Bcc.Add(strBccMail)
                End If
                mail.Subject = strSubject
                mail.Body = strBodyMail
                mail.IsBodyHtml = True

                Using smtp As New SmtpClient(strSmtpAddress, intPortNumber)
                    smtp.UseDefaultCredentials = False
                    smtp.Credentials = New NetworkCredential(strUserName, strPassword)
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
    '　　　FUNCTION   : Split a List into parts.
    '　　　VALUE      : List(Of List(Of String)).
    '      PARAMS     : (lstStrInput List(Of String), List of email addresses)
    '                   (intChunkSize Integer, Mail submit number)
    '      MEMO       : None.
    '      CREATE     : 2020/03/25　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Private Shared Function SplitIntoChunks(ByVal lstStrInput As List(Of String), ByVal intChunkSize As Integer) As List(Of List(Of String))
        Return lstStrInput.
            Select(Function(x, i) New With {Key .Index = i, Key .Value = x}).
            GroupBy(Function(x) (x.Index \ intChunkSize)).
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

        mclsDb = New clsDbAccess

        Try

            mclsDb.Open(mstrConnectionSql)

			'Get all mail address from table [m_users]
            objDataTableMailAddress = mclsDb.GetAllMailAddress()

            'Convert datatable mail address to list object cslMailAddress.
			'if has mailaddress  -> convert to list object clsMailAddress
            If objDataTableMailAddress IsNot Nothing And objDataTableMailAddress.Rows.Count > 0 Then

                convertedListObjMailAddress = (From rw In objDataTableMailAddress.AsEnumerable()
                                               Select New clsMailAddress With {
                                                       .strMailaddress = Convert.ToString(rw("MAIL_ADDRESS")),
                                                       .decLanguageType = Convert.ToDecimal(rw("LANGUAGE_TYPE"))
                                                       }).ToList()
            End If

            Return convertedListObjMailAddress

        Catch ex As Exception

            Throw New Exception(ex.Message, ex)

        Finally
            If mclsDb IsNot Nothing Then                 'check status object mclsDb -> close
                mclsDb.Close()
            End If

            mclsDb = Nothing

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

        mclsDb = New clsDbAccess
        Dim intAnnounceNo As Integer

        Try
            Dim objResult As DataTable
            mclsDb.Open(mstrConnectionSql)
            objResult = mclsDb.GetKeyForAnnounceNo()       'get key for announceno

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
            If mclsDb IsNot Nothing Then                   'check status object mclsDb -> close
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

    '   ******************************************************************
    '　　　FUNCTION   :  DecodeQuotedPrintableString.
    '　　　VALUE      : String.
    '      PARAMS     : (strEncodedString String, Content needs decode)
    '      MEMO       : None.
    '      CREATE     : 2020/04/08　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Function DecodeQuotedPrintableString(ByVal strEncodedString As String) As String
        Dim objStrBuilder As New StringBuilder()
        Dim startIndx As Integer = 0

        Dim matches As MatchCollection = mobjQuotedPrintableRegex.Matches(strEncodedString)

        For i As Integer = 0 To matches.Count - 1
            Dim m As Match = matches(i)
            Dim hexchars As String = m.Groups("hexchars").Value
            Dim charcode As Integer = Convert.ToInt32(hexchars, 16)
            Dim c As Char = ChrW(charcode)

            If m.Index > 0 Then
                objStrBuilder.Append(strEncodedString.Substring(startIndx, (m.Index - startIndx)))
            End If

            objStrBuilder.Append(c)

            startIndx = m.Index + 3
        Next

        If startIndx < strEncodedString.Length Then
            objStrBuilder.Append(strEncodedString.Substring(startIndx))
        End If

        Return Regex.Replace(objStrBuilder.ToString(), "=" & vbCr & vbLf, "")
    End Function


    '   ******************************************************************
    '      FUNCTION   : clsMailAddress.
    '      MEMO       : None.
    '      CREATE     : 2020/03/25　AKB　Cuong.
    '      UPDATE     : 
    '   ******************************************************************
    Class clsMailAddress
        Public strMailaddress As String        'Mail address 
        Public decLanguageType As Decimal      'Languae type
    End Class
End Class
