'   ******************************************************************
'      TITLE      : 情報共有システム.
'　　　機能       : 共通変数クラス.
'      備考       : .
'      CREATE     : 2020/02/14　KBS　泉.
'      UPDATE     : .
'
'           2020 KBSOFTWARE CORPORATION.
'   ******************************************************************

Imports System.Reflection

Imports System.Net
Imports System.Text
Imports sqlTest.clsConst

Public Class clsCommon

#Region "共通変数 定義"

    Public Overridable Property DBAccess As clsDbAccess     ' DB操作クラス.

#End Region


#Region "モジュール変数 定義"

#End Region

#Region "処理"
    '   ******************************************************************
    '      機能       : システム起動時の共通初期処理.
    '      返り値     : 成否.
    '      引き数     : strConnectionString             String    DB接続文字列.
    '      備考       : .
    '      CREATE     : 2016/02/01　KBS　吉田.
    '      UPDATE     : .
    '   ******************************************************************
    Public Overridable Function InitializeSystem(ByVal strConnectionString As String) As Boolean

        Try
            'データベース接続開始.
            If Not Me.DBAccess.Open(strConnectionString) Then
                '"データベースに接続できません。"
                Return False
            End If
        Catch ex As Exception
            '"データベースに接続できません。"
            Return False
        End Try


        '初期化正常終了.
        Return True

    End Function

    '------------------------------------------------------------------
    '      FUNCTION   : DB値をString型に変換する.
    '      VALUE      : String, 変換後の値.
    '      PARAMS     : 引数1  Object, 変換対象DB値.
    '      MEMO       : 
    '      CREATE     : 2016/02/01　KBS　吉田.
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function ConvDbToStr(ByVal objVal As Object) As String
        Dim strRet As String = String.Empty

        Try

            If IsDBNull(objVal) = True Then
                strRet = ""
            Else
                If objVal Is vbNullString Then
                    strRet = ""
                Else
                    strRet = CType(objVal, String)
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

        Return strRet
    End Function

    '------------------------------------------------------------------
    '      FUNCTION   : DB値をString型に変換する.
    '      VALUE      : String, 変換後の値.
    '      PARAMS     : 引数1  Object, 変換対象DB値.
    '      MEMO       : .
    '      CREATE     : 2016/02/01　KBS　吉田.
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Function ConvCsvToStr(ByVal objVal As Object, _
                                 Optional ByVal blnNewLineFlg As Boolean = False, _
                                 Optional ByVal strNewLineString As String = "\n") As String
        Dim strRet As String = String.Empty
        Dim strVal As String = String.Empty

        Try

            strVal = ConvDbToStr(objVal)

            'ダブルクオテーションで囲まれている場合.
            If strVal.StartsWith("""") = True And strVal.EndsWith("""") = True Then
                strVal = strVal.Substring(1)
                If strVal.Length > 0 Then
                    strVal = strVal.Substring(0, strVal.Length - 1)
                End If
            End If

            If strVal.Length > 0 Then
                strVal = strVal.Trim()
            End If

            '改行コード変換.
            If blnNewLineFlg = True Then
                strVal = strVal.Replace(strNewLineString, vbNewLine)
            End If

            strRet = strVal

        Catch ex As Exception
            Throw
        End Try

        Return strRet
    End Function

#End Region

#Region "Shared処理"
    '------------------------------------------------------------------
    '      FUNCTION   : 文字列をDB値に変換する.
    '      VALUE      : DateTime, 変換後の値.
    '      PARAMS     : 引数1  Object, 変換対象文字列.
    '      MEMO       : 
    '      CREATE     : 2012/01/05　KBS　H.Motomura.
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function ConvStrToDB(ByVal strVal As String) As String
        Dim strRet As String = String.Empty

        Try

            If strVal = vbNullString Then
                strRet = "NULL"
            Else
                If strVal.Length = 0 Then
                    strRet = "NULL"
                Else
                    strRet = "'" & strVal.Replace("'", "''") & "'"
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

        Return strRet

    End Function

    '------------------------------------------------------------------
    '      FUNCTION   : 日付型をDB値に変換する.
    '      VALUE      : String, 変換後の値.
    '      PARAMS     : 引数1  DateTime, 変換対象の日付.


    '      MEMO       : 
    '      CREATE     : 2012/01/05　KBS　H.Motomura
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function ConvDateTimeToDB(ByVal dtmDate As DateTime) As String
        Dim strRet As String = String.Empty

        Try

            If dtmDate = clsConst.gcdtm_DATETIME_NULL Then
                strRet = "NULL"
            Else
                strRet = "CONVERT(NVARCHAR," & ConvStrToDB(dtmDate.ToString("yyyy/MM/dd")) & ",121)"
            End If

        Catch ex As Exception
            Return strRet
        End Try

        Return strRet
    End Function
    '------------------------------------------------------------------
    '      FUNCTION   : 文字列をWHERE句用に変換する.
    '      VALUE      : DateTime, 変換後の値.
    '      PARAMS     : 引数1  Object, 変換対象文字列.
    '      MEMO       : 
    '      CREATE     : 2012/01/05　KBS　H.Motomura.
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function ConvStrToDBWHERE(ByVal strVal As String) As String
        Dim strRet As String = String.Empty

        Try


            If strVal = vbNullString Then
                strRet = " IS NULL"
            Else
                If strVal.Length = 0 Then
                    strRet = " IS NULL"
                Else
                    strRet = " = '" & strVal.Replace("'", "''") & "'"
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

        Return strRet
    End Function

    '------------------------------------------------------------------
    '      FUNCTION   : DB値をDateTime型に変換する.
    '      VALUE      : DateTime, 変換後の値.
    '      PARAMS     : 引数1  Object, 変換対象DB値.
    '      MEMO       : .
    '      CREATE     : 2012/01/05　KBS　H.Motomura
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function ConvDbToDateTime(ByVal objVal As Object) As DateTime

        Dim dtmRet As DateTime = gcdtm_DATETIME_NULL

        Try
            If IsDBNull(objVal) = False Then
                If Not IsNothing(objVal) Then
                    If DateTime.TryParse(objVal.ToString, dtmRet) = False Then
                        dtmRet = gcdtm_DATETIME_NULL
                    End If
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

        Return dtmRet

    End Function

    '------------------------------------------------------------------
    '      FUNCTION   : 文字列長さを返す.
    '      VALUE      : Integer 文字列の長さをバイト値.
    '      PARAMS     : 引数1  strTarget チェックする文字列.
    '      MEMO       : .
    '      CREATE     : 2012/01/05　KBS　H.Motomura
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function LenB(ByVal stTarget As String) As Integer
        Return System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(stTarget)
    End Function

    '------------------------------------------------------------------
    '      FUNCTION   : 文字列の左側から指定された文字数分切り取る.
    '      VALUE      : String       切り取った文字列.
    '      PARAMS     : 引数1  String     対象の文字列.
    '                   引数2  Integer    切り取る文字数（バイト）. 
    '      MEMO       : .
    '      CREATE     : 2012/01/05　KBS　H.Motomura
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function LeftB(ByVal stTarget As String, ByVal iByteSize As Integer) As String
        Return MidB(stTarget, 1, iByteSize)
    End Function

    '------------------------------------------------------------------
    '      FUNCTION   : 文字列の左側から指定された文字位置から文字数分切り取る.
    '      VALUE      : String       切り取った文字列.
    '      PARAMS     : 引数1  String     対象の文字列.
    '                   引数2  Integer    切り取る文字数（バイト）. 
    '      MEMO       : .
    '      CREATE     : 2012/01/05　KBS　H.Motomura
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function MidB(ByVal stTarget As String, ByVal iStart As Integer) As String
        Dim hEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
        Dim btBytes As Byte() = hEncoding.GetBytes(stTarget)

        Return hEncoding.GetString(btBytes, iStart - 1, btBytes.Length - iStart + 1)
    End Function

    '------------------------------------------------------------------
    '      FUNCTION   : 文字列の右側から指定された文字数分切り取る.
    '      VALUE      : String       切り取った文字列.
    '      PARAMS     : 引数1  String     対象の文字列.
    '                   引数2  Integer    切り取る文字数（バイト）. 
    '      MEMO       : .
    '      CREATE     : 2012/01/05　KBS　H.Motomura
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function RightB(ByVal stTarget As String, ByVal iByteSize As Integer) As String
        Dim hEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
        Dim btBytes As Byte() = hEncoding.GetBytes(stTarget)

        Return hEncoding.GetString(btBytes, btBytes.Length - iByteSize, iByteSize)
    End Function

    '------------------------------------------------------------------
    '      FUNCTION   : 文字列の左側から指定された文字位置から文字数分切り取る.
    '      VALUE      : String       切り取った文字列.
    '      PARAMS     : 引数1  String     対象の文字列.
    '                   引数2  Integer    開始位置（バイト）. 
    '                   引数3  Integer    切り取る文字数（バイト）. 
    '      MEMO       : .
    '      CREATE     : 2012/01/05　KBS　H.Motomura
    '      UPDATE     : 
    '------------------------------------------------------------------
    Public Shared Function MidB(ByVal str As String, ByVal Start As Integer, Optional ByVal Length As Integer = 0) As String
        '▼空文字に対しては常に空文字を返す

        If str = "" Then
            Return ""
        End If

        '▼Lengthのチェック

        'Lengthが0か、Start以降のバイト数をオーバーする場合はStart以降の全バイトが指定されたものとみなす。
        Dim RestLength As Integer = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(str) - Start + 1

        If Length = 0 OrElse Length > RestLength Then
            Length = RestLength
        End If

        '▼切り抜き

        Dim SJIS As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift-JIS")
        Dim B() As Byte = CType(Array.CreateInstance(GetType(Byte), Length), Byte())

        Array.Copy(SJIS.GetBytes(str), Start - 1, B, 0, Length)

        Dim st1 As String = SJIS.GetString(B)

        '▼切り抜いた結果、最後の１バイトが全角文字の半分だった場合、その半分は切り捨てる。
        Dim ResultLength As Integer = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(st1) - Start + 1

        If Length = ResultLength - 1 Then
            Return st1.Substring(0, st1.Length - 1)
        Else
            Return st1
        End If

    End Function


#End Region

End Class
