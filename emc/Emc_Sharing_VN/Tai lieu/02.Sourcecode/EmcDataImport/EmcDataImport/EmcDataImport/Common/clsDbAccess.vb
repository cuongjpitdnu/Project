﻿'   ******************************************************************
'      TITLE      : 情報共有システム.
'　　　機能       : データベース処理定義.
'      備考       : .
'      CREATE     : 2020/02/14　KBS　泉.
'      UPDATE     : .
'
'           2020 KBSOFTWARE CORPORATION.
'   ******************************************************************

Imports System.Data.SqlClient

Public Class clsDbAccess
    Inherits clsDbCore

#Region "処理"

    '   ******************************************************************
    '      機能       : SELECT sample.
    '      返り値     : Integer .
    '      引き数     : 無し.
    '      備考       : .
    '      CREATE     : 2020/02/14　KBS　泉.
    '      UPDATE     : 
    '   ******************************************************************
    Public Overridable Function GetCount() As Integer

        Dim intResult As Integer = 1
        Dim strSQL As String
        Dim dt As DataTable

        strSQL = "SELECT "
        strSQL &= "count(*) AS user_no"
        strSQL &= " FROM m_user"

        dt = GetTable(strSQL)

        If dt.Rows.Count > 0 AndAlso Not IsDBNull(dt.Rows(0).Item("user_no")) Then
            'データが存在する場合.
            intResult = CType(dt.Rows(0).Item("user_no"), Integer)
        Else
            '該当するデータが存在しない場合.
            intResult = 0
        End If

        Return intResult
    End Function

    '   ******************************************************************
    '      機能       : UPDATE sample.
    '      返り値     : boolean    true    正常.
    '                              false   更新エラー.
    '      引き数     : なし.
    '      備考       : .
    '      CREATE     : 2020/02/14　KBS　泉.
    '      UPDATE     : 
    '   ******************************************************************
    Public Overridable Function UpdateUser() As Boolean

        Dim strSQL As String
        Dim intRet As Integer
        Dim blnResult As Boolean = False

        strSQL = "UPDATE m_user SET announce_reg_perm = 1"
        strSQL &= " WHERE user_no=1"

        intRet = Execute(strSQL)
        If intRet > 0 Then
            blnResult = True
        End If

        Return blnResult

    End Function

    Public Function GetDataTLog() As SqlDataReader
        Dim currentDay As String
        Dim strSQL As String

        currentDay = DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000")

        strSQL = "SELECT * FROM t_log"
        strSQL &= " WHERE REG_DATE < "
        strSQL &= "'"
        strSQL &= currentDay
        strSQL &= "'"
        strSQL &= " ORDER BY  REG_DATE, LOGID "

        Dim command As New SqlCommand(strSQL, mobjConnection)
        Dim dataReader As SqlDataReader = command.ExecuteReader()

        Return dataReader

    End Function

    Public Sub DeleteDataWebLog()
        Dim currentDay As String
        Dim strSQL As String
        Dim intRet As Integer

        currentDay = DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000")

        strSQL = "DELETE FROM t_log"
        strSQL &= " Where REG_DATE < "
        strSQL &= "'"
        strSQL &= currentDay
        strSQL &= "'"
        intRet = Execute(strSQL)
    End Sub

    Public Sub DeleteInquiryData()
        Dim currentDay As DateTime = DateTime.Now
        Dim strSQL As String
        Dim intRet As Integer

        Dim intNumberDay As Integer = CInt(My.Settings.QUERY_RETENTION_PERIOD)
        Dim searchDay = DateAdd("d", -intNumberDay, currentDay).ToString("yyyy-MM-dd")

        strSQL = "DELETE FROM t_query"
        strSQL &= " WHERE CAST(QUERY_DATE AS DATE) <= "
        strSQL &= "'"
        strSQL &= searchDay
        strSQL &= "'"

        intRet = Execute(strSQL)

    End Sub

    Public Function GetInquiryData() As SqlDataReader
        Dim currentDay As DateTime
        Dim strSQL As String

        currentDay = DateTime.Now

        Dim intNumberDay As Integer = CInt(My.Settings.QUERY_RETENTION_PERIOD)
        Dim searchDay = DateAdd("d", -intNumberDay, currentDay).ToString("yyyy-MM-dd")

        strSQL = "SELECT TOP 1 * FROM t_query"
        strSQL &= " WHERE CAST(QUERY_DATE AS DATE) <= "
        strSQL &= "'"
        strSQL &= searchDay
        strSQL &= "'"

        Dim command As New SqlCommand(strSQL, mobjConnection)
        Dim dataReader As SqlDataReader = command.ExecuteReader()

        Return dataReader
    End Function

    Public Function GetAllMailAddress() As DataTable

        Dim strSQL As String
        Dim dt As DataTable

        strSQL = "SELECT [MAIL_ADDRESS]"
        strSQL &= " FROM m_user"
        strSQL &= " WHERE ANNOUNCE_MAIL = 1"

        If My.Settings.DEBUGFLG = 1 Then
            'write log
            Dim message As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & strSQL
            AppManager.xWriteLog(AppManager.logFolderPath, AppManager.fileNameLog, message)
        End If

        dt = GetTable(strSQL)

        Return dt
    End Function

    Public Function GetKeyForAnnounceNo() As DataTable
        Dim strSQL As String
        Dim dt As DataTable

        strSQL = "SELECT NEXT VALUE"
        strSQL &= " FOR announce_sequence"

        If My.Settings.DEBUGFLG = 1 Then
            'write log
            Dim message As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & strSQL
            AppManager.xWriteLog(AppManager.logFolderPath, AppManager.fileNameLog, message)
        End If

        dt = GetTable(strSQL)

        Return dt
    End Function

    Public Sub InsertDataAnnounce(announceNo As Integer, tilteJPN As String, contentsJPN As String)
        'Dim intRet As Integer
        'Dim strSQL As String
        'Dim currentDay As String
        'currentDay = DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss.fff")

        'Dim regUserNo As Integer
        'regUserNo = My.Settings.REG_USER_NO

        'strSQL = "INSERT INTO t_announce"
        'strSQL &= " ("
        'strSQL &= " [ANNOUNCE_NO], [TITLE_JPN], [CONTENTS_JPN], [LANGUAGE_TYPE], [CORRECTION_FLAG], [UNTRANSLATED], [REG_USER_NO], [REG_DATE], [UP_DATE]"
        'strSQL &= " )"
        'strSQL &= " VALUES"
        'strSQL &= " (" & announceNo & ",'" & tilteJPN & "','" & contentsJPN & "', " & 0 & ", " & 0 & ", " & 0 & ", " & regUserNo & ",'" & currentDay & "','" & currentDay & "' )"

        'If My.Settings.DEBUGFLG = 1 Then
        '    'write log
        '    Dim message As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & strSQL
        '    AppManager.xWriteLog(AppManager.logFolderPath, AppManager.fileNameLog, message)
        'End If

        'intRet = Execute(strSQL)


        Dim querySQL As String
        Dim currentDay As String
        currentDay = DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss.fff")

        Dim intRet As Integer
        Dim strSQL As String
        Dim regUserNo As Integer
        regUserNo = My.Settings.REG_USER_NO

        strSQL = "INSERT INTO t_announce"
        strSQL &= " ("
        strSQL &= " [ANNOUNCE_NO], [TITLE_JPN], [CONTENTS_JPN], [LANGUAGE_TYPE], [CORRECTION_FLAG], [UNTRANSLATED], [REG_USER_NO], [REG_DATE], [UP_DATE]"
        strSQL &= " )"
        strSQL &= " VALUES"
        strSQL &= " (@announceNo, @title, @contents, @languagetype, @correctionflag, @untraslated, @reguserno, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)"

        querySQL = "INSERT INTO t_announce"
        querySQL &= " ("
        querySQL &= " [ANNOUNCE_NO], [TITLE_JPN], [CONTENTS_JPN], [LANGUAGE_TYPE], [CORRECTION_FLAG], [UNTRANSLATED], [REG_USER_NO], [REG_DATE], [UP_DATE]"
        querySQL &= " )"
        querySQL &= " VALUES"
        querySQL &= " (" & announceNo & ",'" & tilteJPN & "','" & contentsJPN & "', " & 0 & ", " & 0 & ", " & 0 & ", " & regUserNo & ",'" & currentDay & "','" & currentDay & "' )"

        If My.Settings.DEBUGFLG = 1 Then
            'write log
            Dim message As String = DateTime.Now.ToString(clsConst.cstrDateTimeFomatShow) & " " & querySQL
            AppManager.xWriteLog(AppManager.logFolderPath, AppManager.fileNameLog, message)
        End If

        Dim parameters() As SqlParameter = New SqlParameter() _
            {
                New SqlParameter("@announceNo", SqlDbType.Int) With {.Value = announceNo},
                New SqlParameter("@title", SqlDbType.VarChar, 8000) With {.Value = tilteJPN},
                New SqlParameter("@contents", SqlDbType.VarChar, 8000) With {.Value = contentsJPN},
                New SqlParameter("@languagetype", SqlDbType.Decimal) With {.Value = 0},
                New SqlParameter("@correctionflag", SqlDbType.Decimal) With {.Value = 0},
                New SqlParameter("@untraslated", SqlDbType.Decimal) With {.Value = 0},
                New SqlParameter("@reguserno", SqlDbType.Int) With {.Value = regUserNo}
            }

        intRet = executeParam(parameters, strSQL)

    End Sub

    Public Function executeParam(ByVal objParam() As SqlParameter, ByVal strSQL As String) As Integer
        Dim intRet As Integer = 0
        Dim i As Integer

        Try

            ' コマンドの作成.
            mobjCommand = New SqlCommand(strSQL, mobjConnection)
            mobjCommand.Transaction = mobjTarn

            ' コマンドタイプを指定.
            mobjCommand.CommandType = CommandType.Text

            ' 戻り値用のパラメータ.
            Dim objRetParam As New SqlParameter

            objRetParam.ParameterName = "rtnnum"
            objRetParam.SqlDbType = SqlDbType.Int
            objRetParam.Direction = ParameterDirection.ReturnValue
            mobjCommand.Parameters.Add(objRetParam)

            If Not IsNothing(objParam) Then

                For i = 0 To objParam.Length - 1

                    If Not IsNothing(objParam(i)) Then

                        ' パラメータの追加.
                        mobjCommand.Parameters.Add(objParam(i))

                    End If

                Next

            End If

            ' SQLの実行.
            intRet = mobjCommand.ExecuteNonQuery()

            ' ストアドの戻り値を取得.
            intRet = CType(objRetParam.Value, Integer)

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

        Return intRet

    End Function

#End Region

End Class

