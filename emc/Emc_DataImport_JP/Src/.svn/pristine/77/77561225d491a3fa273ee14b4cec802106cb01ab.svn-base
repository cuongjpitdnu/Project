'   ******************************************************************
'      TITLE      : 情報共有システム.
'　　　機能       : データベース処理定義.
'      備考       : .
'      CREATE     : 2020/02/14　KBS　泉.
'      UPDATE     : .
'
'           2020 KBSOFTWARE CORPORATION.
'   ******************************************************************

Imports sqlTest.clsConst

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


#End Region

End Class

