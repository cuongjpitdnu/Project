'   ******************************************************************
'      TITLE      : KBS用　ADO.Net DBコア
'　　　FUNCTION   : 
'      MEMO       : 
'      CREATE     : 2006/08/09　KBS　岩永
'      UPDATE     : 
'
'
'           2005 KBSOFTWARE CORPORATION
'   ******************************************************************

Option Explicit On
Option Strict On

' インポート宣言
Imports System
Imports System.Data
Imports System.Data.OleDb

Namespace KBS_COMMON_DB

    '   ******************************************************************
    '　　　FUNCTION   : DBコアクラス
    '      MEMO       : 
    '      CREATE     : 2006/08/09　KBS　岩永　
    '      UPDATE     : 
    '   ******************************************************************
    Public Class clsDbCore
        Implements IDisposable

        ' フィールド宣言
        Protected m_objConnection As OleDbConnection        ' ADO.Net接続オブジェクト

        Protected m_objTarn As OleDbTransaction             ' ADO.Netトランザクションオブジェクト

        Protected m_strConnect As String                    ' DB接続文字列
        Protected m_objCommand As OleDbCommand              ' ADO.Netコマンドオブジェクト


        Protected m_strMDbName As String                    ' MDB名称（フルパス）
        Protected m_strMDbPass As String                    ' Password

        ' 定数
        Protected ReadOnly mc_strDataTblName As String = "GetTable"        ' GetTableメソッドのテーブル名


        '   ******************************************************************
        '　　　FUNCTION   : ＤＢコネクションプロパティ
        '　　　VALUE      : 戻り値　OleConnection、ＤＢコネクション取得

        '      PARAMS     : なし

        '      MEMO       : 読み取り専用
        '      CREATE     : 2006/08/09　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public ReadOnly Property DbConnection() As OleDbConnection
            Get
                Return m_objConnection
            End Get
        End Property


        '   ******************************************************************
        '　　　FUNCTION   : ＤＢトランザクションプロパティ
        '　　　VALUE      : 戻り値　OleDbTransaction、ＤＢトランザクション取得

        '      PARAMS     : なし

        '      MEMO       : 読み取り専用
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public ReadOnly Property DbTransaction() As OleDbTransaction
            Get
                Return m_objTarn
            End Get
        End Property


        '   ******************************************************************
        '　　　FUNCTION   : 接続文字列プロパティ
        '　　　VALUE      : 戻り値　String、接続文字列を取得

        '      PARAMS     : 引数１　String、接続文字列を設定

        '      MEMO       : 
        '      CREATE     : 2005/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Property DbConnectStr() As String
            Get
                Return m_strConnect
            End Get
            Set(ByVal Value As String)
                m_strConnect = Value
            End Set
        End Property


        '   ******************************************************************
        '　　　FUNCTION   : MDB名称プロパティ
        '　　　VALUE      : 戻り値　String、MDB名称を取得

        '      PARAMS     : 引数１　String、MDB名称を設定

        '      MEMO       : 
        '      CREATE     : 2005/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Property MDbName() As String
            Get
                Return m_strMDbName
            End Get
            Set(ByVal Value As String)
                m_strMDbName = Value
            End Set
        End Property

        '   ******************************************************************
        '　　　FUNCTION   : コンストラクタ
        '　　　VALUE      : なし

        '      PARAMS     : なし

        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Sub New()
            Try
                ' 初期化

                InitCtl()
            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try
        End Sub


        '   ******************************************************************
        '　　　FUNCTION   : デストラクタ
        '　　　VALUE      : なし

        '      PARAMS     : なし

        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Sub Dispose() Implements IDisposable.Dispose

            Try

                '終了処理

                If Not IsNothing(m_objConnection) Then

                    ' DB接続オブジェクトの破棄

                    m_objConnection.Dispose()

                End If
                m_objConnection = Nothing

            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try

        End Sub


        '   ******************************************************************
        '　　　FUNCTION   : 初期化

        '　　　VALUE      : なし

        '      PARAMS     : なし

        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Private Sub InitCtl()

            m_objConnection = Nothing
            m_strConnect = ""
            m_objTarn = Nothing
            m_strMDbName = ""
            m_objCommand = Nothing

        End Sub


        '   ******************************************************************
        '　　　FUNCTION   : ＤＢ接続処理（MDB）

        '　　　VALUE      : True-成功、False-失敗

        '      PARAMS     : 1:MDB名称（フルパス）

        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Overloads Function OpenMDB(ByVal strMdbName As String) As Boolean
            Dim blnRet As Boolean = False

            Try
                ' DBオープン
                m_strMDbName = strMdbName
                ' 接続文字列の作成
                m_strConnect = "provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_strMDbName + ";"

                ' AOD.Net接続オブジェクトの作成
                m_objConnection = New OleDbConnection
                ' 接続文字列の設定

                m_objConnection.ConnectionString = m_strConnect
                ' DBオープン
                m_objConnection.Open()

                blnRet = True

            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try

            Return blnRet

        End Function

        '   ******************************************************************
        '      FUNCTION   : OpenMDB
        '      VALUE      : Boolean, true - success, false - failure
        '      PARAMS     : strUser as string, username
        '      MEMO       : 
        '      CREATE     : 2011/07/14 AKB Quyet
        '      UPDATE     : 
        '   ******************************************************************
        Public Overloads Function OpenMDB(ByVal strMdbName As String, ByVal strMdbPass As String) As Boolean
            Dim blnRet As Boolean = False

            Try
                ' DBオープン
                m_strMDbName = strMdbName
                m_strMDbPass = strMdbPass

                ' 接続文字列の作成

                'm_strConnect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + My.Application.Info.DirectoryPath + basConst.gcstrDBPATH + strMdbName + ";Jet OLEDB:Database Password=" + m_strMDbPass + ";Mode=Share Deny None;" 'Mode=Share Exclusive;
                m_strConnect = "provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + My.Application.Info.DirectoryPath + basConst.gcstrDBPATH + strMdbName + ";Jet OLEDB:Database Password=" + m_strMDbPass + ";Mode=Share Deny None;" 'Mode=Share Exclusive;

                ' AOD.Net接続オブジェクトの作成
                m_objConnection = New OleDbConnection
                ' 接続文字列の設定

                m_objConnection.ConnectionString = m_strConnect
                ' DBオープン
                m_objConnection.Open()

                blnRet = True

            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try

            Return blnRet

        End Function


        '   ******************************************************************
        '　　　FUNCTION   :DB状態取得

        '　　　VALUE      : 戻り値　Boolean、接続中：True,未接続：False
        '      PARAMS     : 
        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Function IsConnect() As Boolean
            Dim ret As Boolean = False

            Try
                ' コネクションオブジェクトの存在チェック
                If IsNothing(m_objConnection) Then
                    Return False
                End If

                Select Case m_objConnection.State
                    Case ConnectionState.Open   ' DB Open
                        ret = True

                    Case ConnectionState.Closed  ' DB Close
                        ret = False
                End Select

            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
                ret = False

            End Try

            Return ret

        End Function


        '   ******************************************************************
        '　　　FUNCTION   :DB切断
        '　　　VALUE      : 戻り値　Boolean、成功：True,失敗：False
        '      PARAMS     : 
        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Function Close() As Boolean

            Dim ret As Boolean = False

            Try

                ' DBクローズ処理

                If Not IsNothing(m_objConnection) Then

                    ' DB切断
                    If Me.IsConnect Then
                        m_objConnection.Close()
                    End If

                End If

                ret = True

            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try
            Return ret
        End Function


        '   ******************************************************************
        '　　　FUNCTION   :データを取得

        '　　　VALUE      : 戻り値　DataTable、取得データをDataTableにセットして返す
        '      PARAMS     : 
        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Function GetTable(ByVal strSQL As String) As DataTable

            Dim objRetTable As New DataTable(mc_strDataTblName)
            Dim i As Integer
            Dim objDataReader As OleDbDataReader

            Try

                ' コマンドの作成
                m_objCommand = New OleDbCommand(strSQL, m_objConnection, m_objTarn)

                ' データの取得

                objDataReader = m_objCommand.ExecuteReader

                ' カラム作成
                For i = 0 To objDataReader.FieldCount - 1
                    objRetTable.Columns.Add(objDataReader.GetName(i), objDataReader.GetFieldType(i))
                Next

                Do While objDataReader.Read
                    Dim objVal(objDataReader.FieldCount - 1) As Object
                    ' データをオブジェクト配列に取得

                    objDataReader.GetValues(objVal)
                    ' データテーブルに行を追加
                    objRetTable.Rows.Add(objVal)
                Loop

                ' DataReaderのクローズ
                objDataReader.Close()

                Return objRetTable

            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try

        End Function


        '   ******************************************************************
        '　　　FUNCTION   :SQLの実行

        '　　　VALUE      : 戻り値　Integer、処理された行数を返す
        '      PARAMS     : 引数1：SQL文

        '      NOTE       : 
        '      CREATE     : 2006/08/08　KBS　岩永
        '      UPDATE     : 
        '   ******************************************************************
        Public Overloads Function Execute(ByVal strSQL As String) As Integer

            Dim Ret As Integer

            Try

                ' コマンドの作成
                m_objCommand = New OleDbCommand(strSQL, m_objConnection, m_objTarn)

                ' SQLの実行

                Ret = m_objCommand.ExecuteNonQuery()

                Return Ret

            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try

        End Function


        '   ******************************************************************
        '　　　FUNCTION   :Begin
        '　　　VALUE      : 戻り値　Boolean、正常：True,異常：False
        '      PARAMS     : 
        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Function BeginTransaction() As Boolean
            Dim ret As Boolean = False
            Try
                Try
                    If IsNothing(m_objTarn) = False Then
                        m_objTarn.Rollback()
                    End If
                Catch e As Exception
                End Try

                m_objTarn = m_objConnection.BeginTransaction
                ret = True
            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try
            Return ret
        End Function

        '   ******************************************************************
        '　　　FUNCTION   :Commit
        '　　　VALUE      : 戻り値　Boolean、正常：True,異常：False
        '      PARAMS     : 
        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Function Commit() As Boolean
            Dim ret As Boolean = False
            Try

                m_objTarn.Commit()
                ret = True
            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try
            Return ret
        End Function

        '   ******************************************************************
        '　　　FUNCTION   :RollBack
        '　　　VALUE      : 戻り値　Boolean、正常：True,異常：False
        '      PARAMS     : 
        '      MEMO       : 
        '      CREATE     : 2006/08/08　KBS　岩永　
        '      UPDATE     : 
        '   ******************************************************************
        Public Function RollBack() As Boolean
            Dim ret As Boolean = False
            Try

                m_objTarn.Rollback()
                ret = True
            Catch ex As Exception
                Throw New clsDbAException(ex.Message, ex)
            End Try
            Return ret
        End Function

        '   ******************************************************************
        '　　　FUNCTION   : Date 型用 IsNothing
        '　　　VALUE      : 戻り値　Boolean、True=Nothing,False=非Nothing,
        '      PARAMS     : 引数１　Date、対象
        '      MEMO       : 
        '      CREATE     : 2005/12/22　KBS　吉原
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
    End Class



#Region " 処理例外クラス "

    '   ******************************************************************
    '　　　FUNCTION   : DB処理例外クラス
    '      MEMO       : 
    '      CREATE     : 2006/08/08　岩永
    '      UPDATE     : 
    '   ******************************************************************
    Public Class clsDbAException
        Inherits ApplicationException

        ' コンストラクタ
        Public Sub New()
            MyBase.New()
        End Sub

        ' コンストラクタ
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ' コンストラクタ
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

    End Class

#End Region

End Namespace

