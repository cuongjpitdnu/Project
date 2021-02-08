﻿'   ******************************************************************
'      TITLE      : clsDbAccess.vb
'      FUNCTION   : Database processing definition.
'      MEMO       : None.
'      CREATE     : 2020/02/20　AKB　Cuong.
'      UPDATE     : .
'
'           2020 AKBSOFTWARE CORPORATION
'   ******************************************************************

Imports System.Data.SqlClient


'   ******************************************************************
'      FUNCTION   : Database processing definition.
'      MEMO       : None.
'      CREATE     : 2020/02/20　AKB　Cuong.
'      UPDATE     : 
'   ******************************************************************
Public Class clsDbAccess
    Inherits clsDbCore

#Region "処理"


    '   ******************************************************************
    '　　　FUNCTION   : Get data from [t_log] table.
    '　　　VALUE      : SqlDataReader
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetDataTLog() As SqlDataReader
        Dim strCurrentDay As String
        Dim strSQL As String

        strCurrentDay = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT)

        strSQL = "SELECT * FROM t_log"
        strSQL &= " WHERE REG_DATE < "
        strSQL &= "'"
        strSQL &= strCurrentDay
        strSQL &= "'"
        strSQL &= " ORDER BY  REG_DATE, LOGID "

        Dim objCommand As New SqlCommand(strSQL, mobjConnection)
        Dim objDataReader As SqlDataReader = objCommand.ExecuteReader()

        Return objDataReader

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : Delete data in [t_log] table.
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub DeleteDataWebLog()
        Dim strCurrentDay As String
        Dim strSQL As String
        Dim intRet As Integer

        strCurrentDay = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT)

        strSQL = "DELETE FROM t_log"
        strSQL &= " Where REG_DATE < "
        strSQL &= "'"
        strSQL &= strCurrentDay
        strSQL &= "'"
        intRet = Execute(strSQL)
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : Delete data in [t_query] table.
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub DeleteInquiryData()
        Dim dtmCurrentDay As DateTime = DateTime.Now
        Dim strSQL As String
        Dim intRet As Integer

        Dim intNumberDay As Integer = CInt(My.Settings.QUERY_RETENTION_PERIOD)
        Dim strSearchDay = DateAdd("d", -intNumberDay, dtmCurrentDay).ToString(clsConst.gcstr_DATE_FORMAT_NO_TIME)

        strSQL = "DELETE FROM t_query"
        strSQL &= " WHERE CAST(QUERY_DATE AS DATE) <= "
        strSQL &= "'"
        strSQL &= strSearchDay
        strSQL &= "'"

        intRet = Execute(strSQL)

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : Get data from [t_query] table.
    '　　　VALUE      : SqlDataReader.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetInquiryData() As SqlDataReader
        Dim dtmCurrentDay As DateTime
        Dim strSQL As String

        dtmCurrentDay = DateTime.Now

        Dim intNumberDay As Integer = CInt(My.Settings.QUERY_RETENTION_PERIOD)
        Dim strSearchDay = DateAdd("d", -intNumberDay, dtmCurrentDay).ToString(clsConst.gcstr_DATE_FORMAT_NO_TIME)

        strSQL = "SELECT TOP 1 * FROM t_query"
        strSQL &= " WHERE CAST(QUERY_DATE AS DATE) <= "
        strSQL &= "'"
        strSQL &= strSearchDay
        strSQL &= "'"

        Dim objCommand As New SqlCommand(strSQL, mobjConnection)
        Dim objDataReader As SqlDataReader = objCommand.ExecuteReader()

        Return objDataReader
    End Function



    '   ******************************************************************
    '　　　FUNCTION   : Get mail address in [m_user].[mail_address].
    '　　　VALUE      : DataTable.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetAllMailAddress() As DataTable

        Dim strSQL As String
        Dim objDataTable As DataTable
        Dim strCurrentDay = DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_NO_TIME)

        strSQL = "SELECT [MAIL_ADDRESS],[LANGUAGE_TYPE]"
        strSQL &= " FROM m_user"
        strSQL &= " WHERE ANNOUNCE_MAIL = 1 "
        strSQL &= " AND "
        strSQL &= " CAST(EXPIRATION_DATE_S AS DATE) <= "
        strSQL &= "'"
        strSQL &= strCurrentDay
        strSQL &= "'"
        strSQL &= " AND "
        strSQL &= "'"
        strSQL &= strCurrentDay
        strSQL &= "'"
        strSQL &= " <= CAST(EXPIRATION_DATE_E AS DATE)"
        strSQL &= " ORDER BY LANGUAGE_TYPE, USER_ID"

        If My.Settings.REG_USER_NO = 1 Then
            'write log
            clsEmcDataImport.gstrLogFolderPath = My.Settings.SHARE_FOLDER + "\" + My.Settings.LOG_FOLDER
            clsEmcDataImport.gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessage As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & strSQL
            clsEmcDataImport.xWriteLog(clsEmcDataImport.gstrLogFolderPath, clsEmcDataImport.gstrFileNameLog, strMessage)
        End If

        objDataTable = GetTable(strSQL)

        Return objDataTable
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : Get announce_sequence.
    '　　　VALUE      : DataTable.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetKeyForAnnounceNo() As DataTable
        Dim strSQL As String
        Dim objDataTable As DataTable

        strSQL = "Select Next VALUE"
        strSQL &= " For announce_sequence"

        If My.Settings.MAIL_POP_HOST = 1 Then
            'write log
            clsEmcDataImport.gstrLogFolderPath = My.Settings.LOG_FOLDER
            clsEmcDataImport.gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessage As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & strSQL
            clsEmcDataImport.xWriteLog(clsEmcDataImport.gstrLogFolderPath, clsEmcDataImport.gstrFileNameLog, strMessage)
        End If

        objDataTable = GetTable(strSQL)

        Return objDataTable
    End Function



    '   ******************************************************************
    '　　　FUNCTION   : Insert data to [t_announce] table.
    '　　　VALUE      : None.
    '      PARAMS     : (intAnnounceNo String, AnnounceNo Value)
    '                   (strTilteJPN String, Subject mail)
    '                   (strContentsJPN String, Content mail body)
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub InsertDataAnnounce(ByVal intAnnounceNo As Integer, ByVal strTilteJPN As String, ByVal strContentsJPN As String)
        Dim intRet As Integer
        Dim strSQL As String
        Dim intRegUserNo As Integer
        intRegUserNo = My.Settings.REG_USER_NO

        Dim strQuerySQL As String
        Dim strCurrentDay As String
        strCurrentDay = DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss.fff")

        strQuerySQL = "INSERT INTO t_announce"
        strQuerySQL &= " ("
        strQuerySQL &= " [ANNOUNCE_NO], [TITLE_JPN], [CONTENTS_JPN], [LANGUAGE_TYPE], [CORRECTION_FLAG], [UNTRANSLATED], [DATA_TYPE], [REG_USER_NO], [REG_DATE]"
        strQuerySQL &= " )"
        strQuerySQL &= " VALUES"
        strQuerySQL &= " (" & intAnnounceNo & ",'" & strTilteJPN & "','" & strContentsJPN & "', " & 0 & ", " & 0 & ", " & 0 & "," & 1 & ", " & intRegUserNo & ",'" & strCurrentDay & "' )"

        strSQL = "INSERT INTO t_announce"
        strSQL &= " ("
        strSQL &= " [ANNOUNCE_NO], [TITLE_JPN], [CONTENTS_JPN], [LANGUAGE_TYPE], [CORRECTION_FLAG], [UNTRANSLATED], [DATA_TYPE], [REG_USER_NO], [REG_DATE]"
        strSQL &= " )"
        strSQL &= " VALUES"
        strSQL &= " (@announceNo, @title, @contents, @languagetype, @correctionflag, @untraslated, @datatype, @reguserno, CURRENT_TIMESTAMP)"

        If My.Settings.MAIL_POP_HOST = 1 Then
            'write log
            clsEmcDataImport.gstrLogFolderPath = My.Settings.LOG_FOLDER
            clsEmcDataImport.gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessage As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & strQuerySQL
            clsEmcDataImport.xWriteLog(clsEmcDataImport.gstrLogFolderPath, clsEmcDataImport.gstrFileNameLog, strMessage)
        End If

        Dim objParameters() As SqlParameter = New SqlParameter() _
            {
                New SqlParameter("@announceNo", SqlDbType.Int) With {.Value = intAnnounceNo},
                New SqlParameter("@title", SqlDbType.VarChar, 8000) With {.Value = strTilteJPN},
                New SqlParameter("@contents", SqlDbType.VarChar, 8000) With {.Value = strContentsJPN},
                New SqlParameter("@languagetype", SqlDbType.Decimal) With {.Value = 0},
                New SqlParameter("@correctionflag", SqlDbType.Decimal) With {.Value = 0},
                New SqlParameter("@untraslated", SqlDbType.Decimal) With {.Value = 0},
                New SqlParameter("@datatype", SqlDbType.Decimal) With {.Value = 1},
                New SqlParameter("@reguserno", SqlDbType.Int) With {.Value = intRegUserNo}
            }

        intRet = executeParam(objParameters, strSQL)

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : Execute sql pramameter.
    '　　　VALUE      : None.
    '      PARAMS     : (objParam SqlParameter, Array parameter)
    '                   (strSQL String, SQL query)
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function executeParam(ByVal objParam() As SqlParameter, ByVal strSQL As String) As Integer
        Dim intRet As Integer = 0
        Dim intIndex As Integer

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

                For intIndex = 0 To objParam.Length - 1

                    If Not IsNothing(objParam(intIndex)) Then

                        ' パラメータの追加.
                        mobjCommand.Parameters.Add(objParam(intIndex))

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

