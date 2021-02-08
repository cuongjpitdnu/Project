'   ******************************************************************
'      TITLE      : 情報共有システム.
'　　　機能       : 共通変数定義.
'      備考       : .
'      CREATE     : 2020/02/14　KBS　泉.
'      UPDATE     : .
'
'           2020 KBSOFTWARE CORPORATION.
'   ******************************************************************

Public Class clsConst

#Region "共通変数 定義"

    '日付NULLデータ.
    Public Const gcdtm_DATETIME_NULL As Date = #12:00:00 AM#

    ' Fomat Time
    Public Const cstrDateTimeFormatNoTime As String = "yyyy/MM/dd"
    Public Const cstrDateTimeFomatShow As String = "yyyy/MM/dd HH:mm:ss"
    Public Const cstrDateFomatShow As String = "MMdd"
    Public Const cstrDateFomat As String = "yyyy-MM-dd 00:00:00.000"

#End Region

End Class
