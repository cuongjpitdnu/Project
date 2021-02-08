'   ******************************************************************
'      TITLE      : Information sharing system.
'      FUNCTION   : Database processing definition.
'      MEMO       : None.
'      CREATE     : 2020/02/28　AKB　Cuong.
'      UPDATE     : .
'
'           2020 AKBSOFTWARE CORPORATION
'   ******************************************************************

Imports System.Data.SqlClient


'   ******************************************************************
'      FUNCTION   : Database processing definition.
'      MEMO       : None.
'      CREATE     : 2020/02/28　AKB　Cuong.
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
    '      CREATE     : 2020/02/28　AKB　Cuong
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

        Dim command As New SqlCommand(strSQL, mobjConnection)
        Dim dataReader As SqlDataReader = command.ExecuteReader()

        Return dataReader

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : Delete data in [t_log] table.
    '　　　VALUE      : None.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/28　AKB　Cuong
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
    '      CREATE     : 2020/02/28　AKB　Cuong
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
    '      CREATE     : 2020/02/28　AKB　Cuong
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

        Dim command As New SqlCommand(strSQL, mobjConnection)
        Dim dataReader As SqlDataReader = command.ExecuteReader()

        Return dataReader
    End Function



    '   ******************************************************************
    '　　　FUNCTION   : Get mail address in [m_user].[mail_address].
    '　　　VALUE      : DataTable.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/28　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetAllMailAddress() As DataTable

        Dim strSQL As String
        Dim dt As DataTable

        strSQL = "SELECT [MAIL_ADDRESS]"
        strSQL &= " FROM m_user"
        strSQL &= " WHERE ANNOUNCE_MAIL = 1"

        If My.Settings.DEBUGFLG = 1 Then
            'write log
            clsEmcDataImport.gstrLogFolderPath = My.Settings.LOG_FOLDER
            clsEmcDataImport.gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessage As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & strSQL
            clsEmcDataImport.xWriteLog(clsEmcDataImport.gstrLogFolderPath, clsEmcDataImport.gstrFileNameLog, strMessage)
        End If

        dt = GetTable(strSQL)

        Return dt
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : Get announce_sequence.
    '　　　VALUE      : DataTable.
    '      PARAMS     : None.
    '      MEMO       : None.
    '      CREATE     : 2020/02/28　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Function GetKeyForAnnounceNo() As DataTable
        Dim strSQL As String
        Dim dt As DataTable

        strSQL = "SELECT NEXT VALUE"
        strSQL &= " FOR announce_sequence"

        If My.Settings.DEBUGFLG = 1 Then
            'write log
            clsEmcDataImport.gstrLogFolderPath = My.Settings.LOG_FOLDER
            clsEmcDataImport.gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessage As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & strSQL
            clsEmcDataImport.xWriteLog(clsEmcDataImport.gstrLogFolderPath, clsEmcDataImport.gstrFileNameLog, strMessage)
        End If

        dt = GetTable(strSQL)

        Return dt
    End Function



    '   ******************************************************************
    '　　　FUNCTION   : Insert data to [t_announce] table.
    '　　　VALUE      : None.
    '      PARAMS     : (announceNo String, AnnounceNo Value)
    '                   (tilteJPN String, Subject mail)
    '                   (contentsJPN String, Content mail body)
    '      MEMO       : None.
    '      CREATE     : 2020/02/28　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub InsertDataAnnounce(announceNo As Integer, tilteJPN As String, contentsJPN As String)
        Dim intRet As Integer
        Dim strSQL As String
        Dim intRegUserNo As Integer
        intRegUserNo = My.Settings.REG_USER_NO

        Dim strQuerySQL As String
        Dim strCurrentDay As String
        strCurrentDay = DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss.fff")

        strQuerySQL = "INSERT INTO t_announce"
        strQuerySQL &= " ("
        strQuerySQL &= " [ANNOUNCE_NO], [TITLE_JPN], [CONTENTS_JPN], [LANGUAGE_TYPE], [CORRECTION_FLAG], [UNTRANSLATED], [REG_USER_NO], [REG_DATE], [UP_DATE]"
        strQuerySQL &= " )"
        strQuerySQL &= " VALUES"
        strQuerySQL &= " (" & announceNo & ",'" & tilteJPN & "','" & contentsJPN & "', " & 0 & ", " & 0 & ", " & 0 & ", " & intRegUserNo & ",'" & strCurrentDay & "','" & strCurrentDay & "' )"

        strSQL = "INSERT INTO t_announce"
        strSQL &= " ("
        strSQL &= " [ANNOUNCE_NO], [TITLE_JPN], [CONTENTS_JPN], [LANGUAGE_TYPE], [CORRECTION_FLAG], [UNTRANSLATED], [REG_USER_NO], [REG_DATE], [UP_DATE]"
        strSQL &= " )"
        strSQL &= " VALUES"
        strSQL &= " (@announceNo, @title, @contents, @languagetype, @correctionflag, @untraslated, @reguserno, CURRENT_TIMESTAMP,CURRENT_TIMESTAMP)"

        If My.Settings.DEBUGFLG = 1 Then
            'write log
            clsEmcDataImport.gstrLogFolderPath = My.Settings.LOG_FOLDER
            clsEmcDataImport.gstrFileNameLog = "LOG_" & DateTime.Now.ToString(clsConst.gcstr_DATE_FORMAT_SHOW) & ".log"
            Dim strMessage As String = DateTime.Now.ToString(clsConst.gcstr_DATETIME_FORMAT_SHOW) & " " & strQuerySQL
            clsEmcDataImport.xWriteLog(clsEmcDataImport.gstrLogFolderPath, clsEmcDataImport.gstrFileNameLog, strMessage)
        End If

        Dim parameters() As SqlParameter = New SqlParameter() _
            {
                New SqlParameter("@announceNo", SqlDbType.Int) With {.Value = announceNo},
                New SqlParameter("@title", SqlDbType.VarChar, 8000) With {.Value = tilteJPN},
                New SqlParameter("@contents", SqlDbType.VarChar, 8000) With {.Value = contentsJPN},
                New SqlParameter("@languagetype", SqlDbType.Decimal) With {.Value = 0},
                New SqlParameter("@correctionflag", SqlDbType.Decimal) With {.Value = 0},
                New SqlParameter("@untraslated", SqlDbType.Decimal) With {.Value = 0},
                New SqlParameter("@reguserno", SqlDbType.Int) With {.Value = intRegUserNo}
            }

        intRet = executeParam(parameters, strSQL)

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : Execute sql pramameter.
    '　　　VALUE      : None.
    '      PARAMS     : (objParam SqlParameter, Array parameter)
    '                   (strSQL String, SQL query)
    '      MEMO       : None.
    '      CREATE     : 2020/02/28　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
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

