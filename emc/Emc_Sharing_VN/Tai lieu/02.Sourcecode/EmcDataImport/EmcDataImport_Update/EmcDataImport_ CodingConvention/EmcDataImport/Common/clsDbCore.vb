'******************************************************************
'   TITLE    : clsDbCore.vb
'   機能     : DB処理機能コアクラス.
'   備考     : 無し.
'   CREATE   : 2016/02/01　KBS　吉田.
'   UPDATE   : .
'
'       2016 KBSOFTWARE CORPORATION
'******************************************************************

Imports System.Data.SqlClient

'******************************************************************
'   機能     : DB処理機能コアクラス.
'   備考     : 無し.
'   CREATE   : 2016/02/01　KBS　吉田.
'   UPDATE   : .
'******************************************************************
Public Class clsDbCore
    Implements IDisposable

#Region "共通変数 定義"

    Private mstrClsName As String = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name 'クラス名.

    ' フィールド宣言.
    Protected mobjConnection As SqlConnection      ' SQLDP 接続オブジェクト.
    Protected mobjCommand As SqlCommand            ' SQLDP コマンドオブジェクト.
    Protected mobjDataAdapter As SqlDataAdapter    ' SQLDP データアダプタオブジェクト.
    Protected mstrConnect As String                ' DB接続文字列.
    Protected mobjTarn As SqlTransaction           ' SQLDP トランザクションオブジェクト.

    ' 定数.
    Protected ReadOnly mstrDataTableName As String = "GetTable"      ' GetTableメソッドのテーブル名.
    Protected ReadOnly mintMaxReTryCnt As Integer = 3                ' リトライの最大回数.
    Protected ReadOnly mintSleepTime As Integer = 5000               ' 待ち時間（ミリ秒）.

#End Region

#Region "コンストラクタ"

    '******************************************************************
    '   機能     : コンストラクタ.
    '   返り値   : 無し.
    '   引き数   : 無し.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************
    Public Sub New()

        '初期化.
        mobjConnection = Nothing
        mobjCommand = Nothing
        mobjDataAdapter = Nothing
        mobjTarn = Nothing

    End Sub
#End Region


#Region "処理"

    '******************************************************************
    '   機能     : 接続文字列プロパティ.
    '   返り値   : String   接続文字列を取得.
    '   引き数   : String   接続文字列を設定.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************
    Public Property Connect() As String

        Get

            Return mstrConnect

        End Get

        Set(ByVal strValue As String)

            mstrConnect = strValue

        End Set

    End Property

    '******************************************************************
    '   機能     : DB接続.
    '   返り値   : Boolean  成功：True、  失敗：False
    '   引き数   : 無し.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************        
    Public Overloads Function Open() As Boolean

        Dim blnRet As Boolean = False

        Try

            If IsConnect() = False Then

                ' 接続オブジェクトの作成.
                mobjConnection = New SqlConnection
                ' 接続文字列の設定.
                mobjConnection.ConnectionString = mstrConnect
                ' DBオープン.
                mobjConnection.Open()

            End If

            blnRet = True

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return blnRet

    End Function

    '******************************************************************
    '   機能     : DB接続.
    '   返り値   : Boolean  成功：True、  失敗：False.
    '   引き数   : String   DB接続文字列.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************        
    Public Overloads Function Open(ByVal strConnectStr As String) As Boolean

        Dim blnRet As Boolean = False

        Try

            If IsConnect() = False Then

                ' 接続オブジェクトの作成.
                mobjConnection = New SqlConnection

                ' 接続文字列の設定.
                mobjConnection.ConnectionString = strConnectStr

                ' DBオープン.
                mobjConnection.Open()

            End If

            blnRet = True

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return blnRet

    End Function

    '******************************************************************
    '   機能     : DB状態取得.
    '   返り値   : Boolean、接続中：True,未接続：False
    '   引き数   : 無し.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************         
    Public Function IsConnect() As Boolean

        Dim blnRet As Boolean = False

        Try

            ' コネクションオブジェクトの存在チェック.
            If IsNothing(mobjConnection) Then

                Return False

            End If

            Select Case mobjConnection.State

                Case ConnectionState.Open   ' DB Open

                    blnRet = True

                    Try
                        ' SQL サーバへの接続チェック.
                        Dim objDataReader As SqlDataReader
                        Dim objCom As SqlCommand

                        objCom = New SqlCommand("SELECT * FROM INFORMATION_SCHEMA.TABLES", mobjConnection)

                        objDataReader = objCom.ExecuteReader
                        objDataReader.Close()

                    Catch ex As Exception

                        blnRet = False

                    End Try

                Case ConnectionState.Closed  ' DB Close

                    blnRet = False

            End Select

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return blnRet

    End Function

    '******************************************************************
    '   機能     : DB切断.
    '   返り値   : Boolean、成功：True,失敗：False.
    '   引き数   : 無し.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************          
    Public Function Close() As Boolean

        Dim blnRet As Boolean = False

        Try

            ' DB切断.
            mobjConnection.Close()

            ' DB接続オブジェクトの破棄.
            mobjConnection.Dispose()

            mobjConnection = Nothing

            blnRet = True
        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return blnRet

    End Function

    '******************************************************************
    '   機能     : BeginTransaction.
    '   返り値   : Boolean、正常：True,異常：False.
    '   引き数   : 無し.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************          
    Public Function BeginTransaction() As Boolean

        Dim blnRet As Boolean = False

        Try

            Try

                If IsNothing(mobjTarn) = False Then

                    mobjTarn.Rollback()

                End If

            Catch e As Exception

            End Try

            mobjTarn = mobjConnection.BeginTransaction

            blnRet = True

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return blnRet

    End Function

    '******************************************************************
    '   機能     : Commit.
    '   返り値   : Boolean、正常：True,異常：False.
    '   引き数   : 無し.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************        
    Public Function Commit() As Boolean

        Dim blnRet As Boolean = False

        Try

            mobjTarn.Commit()

            blnRet = True

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return blnRet

    End Function

    '******************************************************************
    '   機能     : RollBack.
    '   返り値   : Boolean、正常：True,異常：False.
    '   引き数   : 無し.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************                
    Public Function RollBack() As Boolean

        Dim blnRet As Boolean = False

        Try

            mobjTarn.Rollback()

            blnRet = True

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return blnRet

    End Function

    '******************************************************************
    '   機能     : ロックのチェック.
    '   返り値   : Boolean、ロック状態ならTrue　ロック以外のExceptionならFalse.
    '   引き数   : SqlException, .
    '              Integer          リトライ回数.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************        
    Private Function ChkLock(ByVal inner As SqlException, ByRef intReTryCnt As Integer) As Boolean

        Dim blnRet As Boolean = False

        Try

            If inner.Number = 54 Then

                intReTryCnt += 1

                If intReTryCnt <= mintMaxReTryCnt Then

                    System.Threading.Thread.Sleep(mintSleepTime)

                    blnRet = True

                End If

            End If

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return blnRet

    End Function

    '******************************************************************
    '   機能     : データを取得.
    '   返り値   : DataTable、取得データをDataTableにセットして返す.
    '   引き数   : 無し.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 2014/10/21 KBS 吉原 例外発生時に SqlDataReader が閉じられていないのを修正.
    '******************************************************************             
    Public Function GetTable(ByVal strSQL As String) As DataTable

        Dim objRetTable As New DataTable(mstrDataTableName)

        Dim i As Integer
        Dim blnLock As Boolean = True
        Dim intReTryCnt As Integer = 0

        Try

            ' コマンドの作成.
            mobjCommand = New SqlCommand(strSQL, mobjConnection)
            mobjCommand.Transaction = mobjTarn

            ' データの取得.
            Using objDataReader As SqlDataReader = mobjCommand.ExecuteReader

                Do While blnLock = True

                    Try

                        blnLock = False

                    Catch ex As SqlException

                        blnLock = ChkLock(ex, intReTryCnt)

                        If blnLock = False Then

                            Throw New clsDbAException(ex.Message, ex)

                        End If

                    Catch ex As Exception

                        Throw New clsDbAException(ex.Message, ex)

                        blnLock = False

                    End Try

                Loop

                ' カラム作成.
                For i = 0 To objDataReader.FieldCount - 1

                    objRetTable.Columns.Add(objDataReader.GetName(i), objDataReader.GetFieldType(i))

                Next

                Do While objDataReader.Read

                    Dim objVal(objDataReader.FieldCount - 1) As Object

                    ' データをオブジェクト配列に取得.
                    objDataReader.GetValues(objVal)

                    ' データテーブルに行を追加.
                    objRetTable.Rows.Add(objVal)

                Loop

                ' DataReaderのクローズ.
                'objDataReader.Close()

            End Using

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objRetTable

    End Function

    '******************************************************************
    '   機能     : SQLの実行.
    '   返り値   : Integer、処理された行数を返す.
    '   引き数   : String   SQL文.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************        
    Public Overloads Function Execute(ByVal strSQL As String) As Integer

        Dim intRet As Integer = 0

        Try

            ' コマンドの作成.
            mobjCommand = New SqlCommand(strSQL, mobjConnection)
            mobjCommand.CommandType = CommandType.Text
            mobjCommand.Transaction = mobjTarn

            ' SQLの実行.
            intRet = mobjCommand.ExecuteNonQuery()

        Catch ex As Exception

            Throw New Exception(ex.Message, ex)

        End Try

        Return intRet
    End Function

    '******************************************************************
    '   機能     : PL/SQLの実行.
    '   返り値   : Integer、処理された行数を返す.
    '   引き数   : SqlParameter     パラメータ.
    '              String           SQL文.
    '   備考     : 無し.
    '   CREATE   : 2009/08/27 KBS
    '   UPDATE   : 
    '******************************************************************                
    Public Function spExecute(ByVal objParam() As SqlParameter, ByVal strSQL As String) As Integer
        Dim intRet As Integer = 0
        Dim i As Integer

        Try

            ' コマンドの作成.
            mobjCommand = New SqlCommand(strSQL, mobjConnection)
            mobjCommand.Transaction = mobjTarn

            ' コマンドタイプを指定.
            mobjCommand.CommandType = CommandType.StoredProcedure

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

#Region "SQL整形"

    '   ******************************************************************
    '      機能       : OracleのIn句を作成(文字列型のフィールド用)
    '　　　返り値     : 戻り値　String、作成In句
    '      引き数     : 引き数１　String、対象データ
    '                   引き数２　String、フィールド名称
    '      備考       : 
    '      CREATE     : 2005/12/16　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function MakeInString(ByVal intValues() As Integer,
                                    ByVal strFieldName As String) As String

        Const cintMax As Integer = 250

        Dim i As Integer
        Dim strInArray() As String
        Dim intInCount As Integer
        Dim intCount As Integer
        Dim strRet As String

        Try

            'In句作成
            intCount = 0
            intInCount = 0

            ReDim strInArray(0)

            i = 0
            For Each strLoopValue As String In intValues

                If intCount >= cintMax Then

                    '配列拡張
                    intInCount += 1
                    ReDim Preserve strInArray(intInCount)

                    'カウンタ初期化
                    intCount = 0

                End If

                '文字列を追加
                If intCount > 0 Then strInArray(intInCount) += "," '中間
                strInArray(intInCount) += CngStrFormat(strLoopValue)
                intCount += 1

                i += 1
            Next

            strRet = ""

            strRet += "(" '最初

            'ORで連結
            strRet += strFieldName + " IN("
            strRet += String.Join(")OR " + strFieldName + " IN(", strInArray)
            strRet += ")"

            strRet += ")" '最後


            Return strRet

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : OracleのIn句を作成(文字列型のフィールド用)
    '　　　返り値     : 戻り値　String、作成In句
    '      引き数     : 引き数１　String、対象データ
    '                   引き数２　String、フィールド名称
    '      備考       : 
    '      CREATE     : 2005/12/16　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function MakeInString(ByVal strValues() As String,
                                    ByVal strFieldName As String) As String

        Const cintMax As Integer = 250

        Dim i As Integer
        Dim strInArray() As String
        Dim intInCount As Integer
        Dim intCount As Integer
        Dim strRet As String

        Try

            'In句作成
            intCount = 0
            intInCount = 0

            ReDim strInArray(0)

            i = 0
            For Each strLoopValue As String In strValues

                If intCount >= cintMax Then

                    '配列拡張
                    intInCount += 1
                    ReDim Preserve strInArray(intInCount)

                    'カウンタ初期化
                    intCount = 0

                End If

                '文字列を追加
                If intCount > 0 Then strInArray(intInCount) += "," '中間
                strInArray(intInCount) += CngStrFormat(strLoopValue)
                intCount += 1

                i += 1
            Next

            strRet = ""

            strRet += "(" '最初

            'ORで連結
            strRet += strFieldName + " IN("
            strRet += String.Join(")OR " + strFieldName + " IN(", strInArray)
            strRet += ")"

            strRet += ")" '最後


            Return strRet

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null文字(Chr(0))が途中に入っていたら、それを取り除く.
    '　　　返り値     : 戻り値　String、整形後の文字列.
    '      引き数     : 引き数1：String、文字列.
    '      NOTE       : 
    '      CREATE     : 2006/03/09　KBS　吉原.
    '      UPDATE     : 
    '   ******************************************************************
    Protected Function RemoveNullChar(ByVal strValue As String) As String

        Dim strTmp As String
        Dim intIndex As Integer

        Try

            'Null文字が途中に入っていたら、それを取り除く.
            'ここでのNull文字とは、 Chr(0) とする.

            strTmp = strValue

            'Nothing は "" に置き換える.
            If IsNothing(strTmp) Then
                strTmp = ""
            End If

            '文字列が NULL ではないか？.
            If strTmp.Length > 0 Then

                'Null文字の位置を検索.
                intIndex = strTmp.IndexOf(Chr(0))

                'Null文字があったか？.
                If intIndex > 0 Then
                    'あった.

                    '最初の位置がNull文字ではないか？.
                    If intIndex = 0 Then
                        '最初の位置がNull文字なら、空文字.
                        strTmp = ""
                    Else
                        'Null文字の直前までの文字列を取得.
                        strTmp = strTmp.Substring(0, intIndex)
                    End If
                End If

            End If

            Return strTmp

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : SQL用文字列整形.
    '　　　返り値     : 戻り値　String、整形後の文字列.
    '      引き数     : 引き数1：String、SQL文.
    '      NOTE       : 
    '      CREATE     : 2005/12/09　KBS　吉原.
    '      UPDATE     : 
    '   ******************************************************************
    Public Function CngStrFormat(ByVal strSQL As String) As String

        Try

            Dim strTmp As String

            'Null文字が途中に入っていたら、それを取り除く.
            'ここでのNull文字とは、 Chr(0) とする.
            strTmp = Me.RemoveNullChar(strSQL)

            'この後はこれまでどおり.
            If strTmp.Length <= 0 Then
                Return "NULL"
            Else
                Return "'" + strTmp.Replace("'", "''") + "'"
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : SQL の Where 句用文字列整形.
    '　　　返り値     : 戻り値　String、整形後の文字列.
    '      引き数     : 引き数1：String、文字列.
    '      NOTE       : 
    '      CREATE     : 2005/12/12　KBS　吉原.
    '      UPDATE     : 
    '   ******************************************************************
    Public Function CngWhereStrFormat(ByVal strSQL As String) As String

        Try

            Dim strTmp As String

            'Null文字が途中に入っていたら、それを取り除く.
            'ここでのNull文字とは、 Chr(0) とする.
            strTmp = Me.RemoveNullChar(strSQL)

            'この後はこれまでどおり.
            If strTmp.Length <= 0 Then
                Return "IS NULL"
            Else
                Return "= '" + strTmp.Replace("'", "''") + "'"
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Date 型用 IsNothing
    '　　　返り値     : 戻り値　Boolean、True=Nothing,False=非Nothing,
    '      引き数     : 引き数１　Date、対象.
    '      備考       : 
    '      CREATE     : 2005/12/22　KBS　吉原.
    '      UPDATE     : 
    '   ******************************************************************
    Protected Function IsNothingDate(ByVal dtmValue As Date) As Boolean

        Try

            If dtmValue = #12:00:00 AM# Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : SQL の Where 句用文字列整形.
    '　　　返り値     : 戻り値　String、整形後の文字列.
    '      引き数     : 引き数1：Date、日付.
    '      NOTE       : 
    '      CREATE     : 2014/09/18　KBS　吉原.
    '      UPDATE     : 
    '   ******************************************************************
    Public Function CngWhereDateFormat(ByVal dtmValue As Date) As String

        Const cstrDateFormat As String = "yyyy/MM/dd HH:mm:dd"
        Const cstrDBToDate As String = "120"

        Try

            If IsNothingDate(dtmValue) OrElse IsNothing(dtmValue) Then
                Return "IS NULL"
            Else
                Return "= CONVERT(DateTime,'" + dtmValue.ToString(cstrDateFormat) + "', " & cstrDBToDate & ")"
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : 日付型をDBの日付の文字列に変換.
    '　　　返り値     : 戻り値　String、整形後の文字列.
    '      引き数     : 引き数1：Date、日付.
    '      NOTE       : 
    '      CREATE     : 2005/12/09　KBS　吉原.
    '      UPDATE     : 
    '   ******************************************************************
    Public Function To_DateTime(ByVal dtmValue As Date) As String

        Const cstrDateFormat As String = "yyyy/MM/dd HH:mm:ss"
        Const cstrDBToDate As String = "120"

        Try

            If IsNothingDate(dtmValue) OrElse IsNothing(dtmValue) Then
                Return "NULL"
            Else
                Return "CONVERT(DateTime,'" + dtmValue.ToString(cstrDateFormat) + "', " & cstrDBToDate & ")"
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　機能       : 日付型をDBの日付の文字列に変換.
    '　　　返り値     : 戻り値　String、整形後の文字列.
    '      引き数     : 引き数1：Date、日付.
    '      NOTE       : 
    '      CREATE     : 2015/03/02　KBS　吉原.
    '      UPDATE     : 
    '   ******************************************************************
    Public Function To_Date(ByVal dtmValue As Date) As String

        Const cstrDateFormat As String = "yyyy/MM/dd"
        Const cstrDBToDate As String = "111"

        Try

            If IsNothingDate(dtmValue) OrElse IsNothing(dtmValue) Then
                Return "NULL"
            Else
                Return "CONVERT(DateTime,'" + dtmValue.ToString(cstrDateFormat) + "', " & cstrDBToDate & ")"
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '　　　機能       : 指定された日付型フィールドを日付部分だけ取り出した値に整形.
    '　　　返り値     : 戻り値　String、SQL.
    '      引き数     : 引き数1：String、フィールド名.
    '      NOTE       : 
    '      CREATE     : 2015/03/02　KBS　吉原.
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ToDateOnly(ByVal strFieldName As String) As String

        Try

            Return "CONVERT(DATETIME,CONVERT(VARCHAR," & strFieldName & ",111),111)"

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    ' CONVERT(VARCHAR,GETDATE(),111)　⇒　2007/08/01　

    '   ******************************************************************
    '      機能       : Null か判定 (DBの文字列型フィールド用)
    '　　　返り値     : 戻り値　Boolean、True=Null,False=非Null,
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2006/01/09　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function IsNullString(ByVal objValue As Object) As Boolean

        Try

            If IsDBNull(objValue) OrElse IsNothing(objValue) Then
                Return True
            Else
                If Me.RemoveNullChar(CStr(objValue)).Length <= 0 Then
                    Return True
                Else
                    Return False
                End If
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null の場合空文字に変換して返す (DBの文字列型フィールド用)
    '　　　返り値     : 戻り値　String、変換後の文字列
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2006/01/09　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ConvNullToEmptyString(ByVal objValue As Object) As String

        Try

            If IsNullString(objValue) OrElse IsNothing(objValue) Then
                Return ""
            Else
                Return Me.RemoveNullChar(CStr(objValue))
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null の場合0に変換して返す (DBの数値型フィールド用)
    '　　　返り値     : 戻り値　Integer、変換後の数値
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2006/02/16　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ConvNullToZeroInteger(ByVal objValue As Object) As Integer

        Try

            If IsDBNull(objValue) OrElse IsNothing(objValue) Then
                Return 0
            Else
                Return CInt(objValue)
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null の場合 Nothing を入れた Nullable に変換して返す (DBの数値型フィールド用)
    '　　　返り値     : 戻り値　Nullable(Of Integer)、変換後の数値
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2014/10/21　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ConvNullToNullableInteger(ByVal objValue As Object) As Nullable(Of Integer)

        Try

            If IsDBNull(objValue) OrElse IsNothing(objValue) Then
                Return Nothing
            Else
                Return CInt(objValue)
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null の場合 Nothing を入れた Nullable に変換して返す (DBの数値型フィールド用)
    '　　　返り値     : 戻り値　Nullable(Of Double)、変換後の数値
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2014/10/21　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ConvNullToNullableDouble(ByVal objValue As Object) As Nullable(Of Double)

        Try

            If IsDBNull(objValue) OrElse IsNothing(objValue) Then
                Return Nothing
            Else
                Return CDbl(objValue)
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null の場合 Nothing を入れた Nullable に変換して返す (DBの日付型フィールド用)
    '　　　返り値     : 戻り値　Nullable(Of Date)、変換後の数値
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2014/10/21　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ConvNullToNullableDate(ByVal objValue As Object) As Nullable(Of Date)

        Try

            If IsDBNull(objValue) OrElse IsNothing(objValue) Then
                Return Nothing
            Else
                Return CDate(objValue)
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null の場合"NULL"に変換して返す (DBの文字列型フィールド用)
    '　　　返り値     : 戻り値　String、変換後の文字列
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2006/02/16　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ConvNullToNullString(ByVal objValue As Object) As String

        Try

            If IsDBNull(objValue) OrElse IsNothing(objValue) Then
                Return "NULL"
            Else
                Return Me.CngStrFormat(CStr(objValue))
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null の場合"NULL"に変換して返す (DBの数値型フィールド用)
    '　　　返り値     : 戻り値　String、変換後の文字列
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2006/02/16　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ConvNullToNullInteger(ByVal objValue As Object) As String

        Try

            If IsDBNull(objValue) OrElse IsNothing(objValue) Then
                Return "NULL"
            Else
                Return CStr(CInt(objValue))
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null の場合"NULL"に変換して返す (DBの数値型フィールド用)
    '　　　返り値     : 戻り値　String、変換後の文字列
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2014/10/22　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ConvNullToNullDouble(ByVal objValue As Object) As String

        Try

            If IsDBNull(objValue) OrElse IsNothing(objValue) Then
                Return "NULL"
            Else
                Return CStr(CDbl(objValue))
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : Null の場合"NULL"に変換して返す (DBの日付型フィールド用)
    '　　　返り値     : 戻り値　String、変換後の文字列
    '      引き数     : 引き数１　Object、対象
    '      備考       : 
    '      CREATE     : 2014/10/22　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ConvNullToNullDateTime(ByVal objValue As Object) As String

        Try

            If IsDBNull(objValue) OrElse IsNothing(objValue) Then
                Return "NULL"
            Else
                Return To_DateTime(CDate(objValue))
            End If

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : システム日付を表す文字列を返す
    '　　　返り値     : 戻り値　String、システム日付を表す文字列
    '      引き数     : 無し
    '      備考       : 
    '      CREATE     : 2014/10/22　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function Sysdate() As String

        Try

            Return "GETDATE()"

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

    '   ******************************************************************
    '      機能       : SUBSTRING関数を表す文字列を返す
    '　　　返り値     : 戻り値　String、SUBSTRING関数を表す文字列
    '      引き数     : strFieldName String フィールド名
    '                   intStartPosition Integer 開始位置(1開始)
    '                   intLength Integer 取得する長さ
    '      備考       : 
    '      CREATE     : 2014/11/05　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function SubString(ByVal strFieldName As String,
                              ByVal intStartPosition As Integer,
                              ByVal intLength As Integer) As String

        Try

            Return "SUBSTRING(" & strFieldName & "," & intStartPosition.ToString() & "," & intLength.ToString() & ")"

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function


    '   ******************************************************************
    '      機能       : SUBSTRING関数を表す文字列を返す
    '　　　返り値     : 戻り値　String、SUBSTRING関数を表す文字列
    '      引き数     : strFieldName String フィールド名
    '                   intStartPosition Integer 開始位置(1開始)
    '                   intLength Integer 取得する長さ
    '      備考       : 
    '      CREATE     : 2015/02/16　KBS　吉原
    '      UPDATE     : 
    '   ******************************************************************
    Public Function NullToZero(ByVal strFieldName As String) As String

        Try

            Return "ISNULL(" & strFieldName & ",0)"

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

    End Function

#End Region

#Region "IDisposable Support"
    Private blnDisposedValue As Boolean ' 重複する呼び出しを検出するには.

    ' IDisposable
    Protected Overridable Sub Dispose(blnDisposing As Boolean)

        If Not Me.blnDisposedValue Then
            If blnDisposing Then
                'マネージ状態を破棄します (マネージ オブジェクト).
                '終了処理.
                If Not IsNothing(mobjConnection) Then
                    Try
                        ' DB切断.
                        mobjConnection.Close()
                        ' DB接続オブジェクトの破棄.
                        mobjConnection.Dispose()
                    Catch
                    End Try
                End If
                mobjConnection = Nothing
            End If
            ' アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします.
            ' 大きなフィールドを null に設定します。
        End If
        Me.blnDisposedValue = True
    End Sub

    ' 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします.
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class

#Region "DB例外処理クラス"

'******************************************************************
'　機能       : DB処理例外クラス.
'  備考       : 無し.
'  CREATE     : 2009/08/27 KBS
'  UPDATE     : 
'******************************************************************
Public Class clsDbAException
    Inherits ApplicationException

    '******************************************************************
    '　機能       : コンストラクタ.
    '　機能       : コンストラクタ.
    '  備考       : 
    '  CREATE     : 2009/08/27 KBS
    '  UPDATE     : 
    '******************************************************************
    Public Sub New()
        MyBase.New()
    End Sub

    '******************************************************************
    '　機能       : コンストラクタ.
    '  備考       : 
    '  CREATE     : 2009/08/27 KBS
    '  UPDATE     : 
    '******************************************************************
    Public Sub New(ByVal strmessage As String)
        MyBase.New(strmessage)
    End Sub

    '******************************************************************
    '　機能       : コンストラクタ.
    '  備考       : 
    '  CREATE     : 2009/08/27 KBS
    '  UPDATE     : 
    '******************************************************************
    Public Sub New(ByVal strmessage As String, ByVal objInner As Exception)

        MyBase.New(strmessage, objInner)

    End Sub

End Class
#End Region

