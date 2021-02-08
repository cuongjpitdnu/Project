'   ******************************************************************
'      TITLE      : Information sharing system.
'      FUNCTION   : Common variable definition.
'      MEMO       : None.
'      CREATE     : 2020/02/20　AKB　Cuong.
'      UPDATE     : .
'
'           2020 AKBSOFTWARE CORPORATION
'   ******************************************************************

Public Class clsConst

#Region "共通変数 定義"

    ' Fomat Time
    Public Const gcstr_DATETIME_FORMAT_NO_TIME As String = "yyyyMMdd"
    Public Const gcstr_DATETIME_FORMAT_SHOW As String = "yyyy/MM/dd HH:mm:ss"
    '▽2020/04/22 Cuong --- S --- に変更
    'Public Const gcstr_DATE_FORMAT_SHOW As String = "MMdd"

    Public Const gcstr_DATE_FORMAT_SHOW As String = "yyyyMMdd"
    '△2020/04/22 Cuong --- E --- に変更
    Public Const gcstr_DATETIME_FORMAT As String = "yyyy-MM-dd 00:00:00.000"
    Public Const gcstr_DATE_FORMAT_NO_TIME As String = "yyyy-MM-dd"

#End Region

End Class
