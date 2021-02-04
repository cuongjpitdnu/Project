Option Strict On
Option Explicit On

Imports System.IO
Imports System.Net
Imports System.Text
Imports System
Imports System.Management
Imports System.Net.NetworkInformation
Imports System.Security.Cryptography
Imports Config_Gia_Pha
'Imports Finisar.SQLite

Public Class frmActiveKey

    Private Const mcstrClsName As String = "frmActiveKey"               'class name

    Dim mobjResponse As HttpWebResponse = Nothing
    Private strURL As String = "http://akb.com.vn/Giapha/ActiveKey.aspx?CID={0}&KEY={1}&Phone={2}&Name={3}&Birth={4}&Type={5}"
    'Private strURL As String = "http://localhost:1272/GiaphaActive/ActiveKey.aspx?CID={0}&KEY={1}&Phone={2}&Name={3}&Birth={4}&Type={5}"
    Private strCHECKURL As String = "http://akb.com.vn/Giapha/CheckConnect.aspx"
    Private mstrActiveFile As String = Application.StartupPath + "\Activekey.txt"
    Private mstrComputerID As String
    Private mintMode As Integer
    Public mblnActiveOk As Boolean = False

    '   ******************************************************************
    '　　　FUNCTION   : xIsDatabaseExist, check for existance of database
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function Run(ByVal strComputerID As String, ByVal intMode As Integer) As Boolean

        Try
            mblnActiveOk = False
            mstrComputerID = strComputerID
            mintMode = intMode
            Me.ShowDialog()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "Run", ex)
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xIsDatabaseExist, check for existance of database
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnActive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActive.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            'check blank
            If fncIsBlank(txtKey1.Text.Trim()) Or fncIsBlank(txtKey1.Text.Trim()) Or fncIsBlank(txtKey2.Text.Trim()) Or fncIsBlank(txtKey3.Text.Trim()) Or fncIsBlank(txtKey4.Text.Trim()) Or fncIsBlank(txtKey5.Text.Trim()) Then

                basCommon.fncMessageWarning("Xin vui lòng nhập đầy đủ mã sản phẩm.")
                txtKey1.Focus()
                Exit Sub

            End If
            If Not fncCheckVersionKey() Then

                basCommon.fncMessageWarning("Mã sản phẩm không phù hợp.")
                txtKey1.Focus()
                Exit Sub

            End If
            'Dim sql_con As New SQLiteConnection
            'Dim sql_cmd As New SQLiteCommand
            'Dim DB As New SQLiteDataAdapter

            'SetConnection(sql_con)
            'sql_con.Open()

            'sql_cmd = sql_con.CreateCommand()
            'Dim CommandText As String = "select id, desc from  mains"
            'DB = New SQLiteDataAdapter(CommandText, sql_con)
            'Dim DS As New DataSet
            'DS.Reset()
            'DB.Fill(DS)
            'Dim DT As DataTable = DS.Tables(0)

            Dim Computer As New clsComputerInfo
            mstrComputerID = Computer.GetVolumeSerial() + Computer.GetProcessorId
            Dim strKey As String = txtKey1.Text.Trim.ToUpper + txtKey2.Text.Trim.ToUpper + txtKey3.Text.Trim.ToUpper + txtKey4.Text.Trim.ToUpper + txtKey5.Text.Trim.ToUpper
            Dim responseFromServer As String = fncGetResponse(mstrComputerID, strKey)

            If responseFromServer = "" Then Return

            If responseFromServer = getMD5Hash(mstrComputerID + "AKB") Then

                Dim oWrite As System.IO.StreamWriter

                oWrite = File.CreateText(mstrActiveFile)
                oWrite.WriteLine(responseFromServer)
                oWrite.WriteLine(strKey)
                oWrite.Close()

                'Add By: 2018.10.08 AKB Nguyen Thanh Tung
                oWrite.Dispose()
                basCommon.fncLoadInfoVersion()
                'Add By: 2018.10.08 AKB Nguyen Thanh Tung

                basCommon.fncMessageInfo("Bạn đã kích hoạt thành công.")
                mblnActiveOk = True
                gblnActivated = True
                Me.Close()

            Else

                basCommon.fncMessageWarning("Mã sản phẩm không phù hợp.")
                txtKey1.Focus()
            End If


        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "btnActive_Click", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : Check Version key
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/12/10  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Function fncCheckVersionKey() As Boolean

        fncCheckVersionKey = False

        Try
            Dim arrOldKey As String() = {"1B6D", "8C1B", "4FA2", "A272", "591C", "6356", "EBDD", "53CC", "CF8F", "BE9B", "8103", "6138", "4E1A", "2417", "BF7C", "8C9C", "6223", "A7F1", "EE16", "8507", "8685", "4464",
                                         "9463", "62CE", "47D4", "F998", "D4F0", "6335", "01F1", "3A8D", "F895", "4FC6", "0C68", "6DD4", "3571", "58B3", "E0E3", "4710",
                                         "815D", "D4C1", "7BAE", "2FA7", "001C", "EF11", "E9E9", "CF14", "220C", "8665", "0F2F", "F104", "A239", "84DB", "376E", "AAEF", "7BA2", "EA0F", "DFBB", "4C8B", "FA32", "08B4",
                                         "FB04", "6D26", "55B3", "AD12", "EAAB", "AF6D"}

            Dim i As Integer
            For i = 0 To arrOldKey.Length - 1
                If arrOldKey(i) = txtKey1.Text.Trim Then
                    Return True
                    Exit Function
                End If
            Next

            ' Start - Remove by: 2018.10.08 AKB Nguyen Thanh Tung
            ' If txtKey1.Text <> gcstrVersion Then Return False
            ' fncCheckVersionKey = True
            If txtKey1.Text = basConst.gcstrVersion500 _
               OrElse txtKey1.Text = basConst.gcstrVersion1000 _
                OrElse txtKey1.Text = basConst.gcstrVersionUltimate _
            Then
                fncCheckVersionKey = True
            End If
            ' End   - Remove by: 2018.10.08 AKB Nguyen Thanh Tung

        Catch ex As Exception

        End Try

    End Function
    'Private Sub SetConnection(ByRef sql_con As SQLiteConnection)
    '    Try
    '        sql_con = New SQLiteConnection("Data Source=DemoT.db;Version=3;New=False;Compress=True;")
    '    Catch ex As Exception

    '    End Try

    'End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xIsDatabaseExist, check for existance of database
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function fncGetResponse(ByVal strComputerID As String, ByVal strKey As String) As String

        fncGetResponse = ""

        Try

            Dim postData As String = Config.Decrypt(gcstrServerPass)
            Dim strLink As String = ""
            If Not xCheckData(strCHECKURL, strKey) Then Return ""

            Dim strBidth As String = dtpDate.Value.ToString("yyyy-MM-dd")

            If mintMode = 2 Then

                strLink = String.Format(strURL, strComputerID, strKey, txtPhone.Text.Trim + " ", txtName.Text.Trim + " ", strBidth + " ", "2")

            Else

                strLink = String.Format(strURL, strComputerID, strKey, txtPhone.Text.Trim + " ", txtName.Text.Trim + " ", strBidth + " ", "1")

            End If

            Dim strReturn As String = fncResponse(strLink, postData)

            fncGetResponse = strReturn

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncGetResponse", ex)

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xIsDatabaseExist, check for existance of database
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCheckData(ByVal strServer As String, ByVal strKey As String) As Boolean

        xCheckData = False

        Try
            If Not IsConnectedToInternet() Then
                basCommon.fncMessageWarning("Bạn cần kết nối đến Internet để kích hoạt sản phẩm." + vbCrLf + "Hãy kiểm tra lại việc kết nối Internet.")
                Return False
            End If

            If strKey.Trim = "" Then
                basCommon.fncMessageWarning("Hãy nhập mã sản phẩm.")
                txtKey1.Focus()
                Return False
            End If

            xCheckData = True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xCheckData", ex)

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : txtKey_TextChanged, go to next control
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtKey_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKey5.TextChanged, txtKey4.TextChanged, txtKey3.TextChanged, txtKey2.TextChanged, txtKey1.TextChanged

        Dim strKeys() As String

        Try
            Dim objTxtBox As TextBox

            objTxtBox = CType(sender, TextBox)

            If objTxtBox Is txtKey1 Then
                'auto filling
                Dim strKey As String = Clipboard.GetText()
                If strKey.IndexOf("-") > 0 Then

                    strKeys = strKey.Split(CChar("-"))

                    For i As Integer = 0 To strKeys.Length - 1
                        Select Case i
                            Case 0 : txtKey1.Text = strKeys(i).Trim()
                            Case 1 : txtKey2.Text = strKeys(i).Trim()
                            Case 2 : txtKey3.Text = strKeys(i).Trim()
                            Case 3 : txtKey4.Text = strKeys(i).Trim()
                            Case 4 : txtKey5.Text = strKeys(i).Trim()
                        End Select
                    Next

                    btnActive.Focus()
                    Exit Sub

                End If

            End If

            If objTxtBox.Text.Trim().Length >= objTxtBox.MaxLength Then
                SendKeys.Send("{TAB}")
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "txtKey_TextChanged", ex)
        Finally
            Erase strKeys
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : btnTrial_Click, use trial
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/17  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnTrial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrial.Click
        Try
            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnTrial_Click", ex)
        End Try
    End Sub
End Class

