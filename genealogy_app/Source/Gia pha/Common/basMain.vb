'   ******************************************************************
'      TITLE      : Main Class
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/07/15　AKB　Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************


'   ******************************************************************
'　　　FUNCTION   : Main module, starts program
'      MEMO       : 
'      CREATE     : 2011/07/15　AKB　Quyet
'      UPDATE     : 
'   ******************************************************************
Module basMain


    Private Const mcstrClsName As String = "basMain"                        'class name
    Private Const mcstrConnectionError As String = "Lỗi kết nối database."        'message connection error
    Private Const mcstrMultiOpenError As String = "Chương trình đang đuợc mở"     'message multiple open error
    
    Public gobjDB As clsDbAccess                                    'create database connection
    Public gblnActivated As Boolean = False                         'software activated
    Public gblnFirstUsed As Boolean = True                          'first time of starting program


    '   ******************************************************************
    '　　　FUNCTION   : Main method
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    <STAThread()>
    Public Sub Main()
        Try


            Application.EnableVisualStyles()

            Dim objCard1 As New usrMemberCard1
            clsDefine.MEM_CARD_W_L = objCard1.Width
            clsDefine.MEM_CARD_H_L = objCard1.Height
            clsDefine.MEM_CARD_W_S = objCard1.Width
            clsDefine.MEM_CARD_H_S = clsDefine.MEM_CARD_H_L - clsDefine.THUMBNAIL_H

            fncSetBufferBetween2Card()

            objCard1.Dispose()

            'QUYET comment dong nay ▼
            'If Not xCheckActive() Then Return
            '▲▲▲▲▲▲▲▲▲▲▲▲▲



            'Dim strCatalog As String = fncReadRegistry(gcstrRegeditKey, clsEnum.RegistryLocation.CurrentUser)
            'If strCatalog = "" Then
            '    strCatalog = fncReadRegistry(gcstrRegeditKey, clsEnum.RegistryLocation.Machine)
            'End If
            'If strCatalog = "" Then
            '    strCatalog = fncReadRegistry(gcstrRegeditKey, clsEnum.RegistryLocation.Users)
            'End If
            'If strCatalog.Trim <> gcstrRegeditValue.Trim Then Return
            'check for multiple open of program
            'If xIsMultiOpen() Then Exit Sub

            If Not xDatabaseExist() Then Exit Sub

            'Create and open database
            gobjDB = New clsDbAccess()

            Dim frmSysLogin As frmLogin                     'login form instance
            Dim frmSysMain As frmMain                       'main form instance

            If gobjDB.Open() Then

                'Load info version
                Call basCommon.fncLoadInfoVersion()

                'check for trial version
                If Not basCommon.fncTrialCheck(False, False) Then Exit Sub
                gblnFirstUsed = False

                'start login form
                frmSysLogin = New frmLogin
                frmSysLogin.ShowDialog()

                If frmSysLogin.SystemLogined Then

                    'start main form
                    frmSysMain = New frmMain()
                    frmSysMain.fncShowForm()
                    If frmSysMain IsNot Nothing Then frmSysMain.Dispose()

                End If

                frmSysLogin.Dispose()

            Else

                'message connection error
                basCommon.fncMessageError(mcstrConnectionError)

            End If

        Catch ex As Exception

            Call basCommon.fncSaveErr(mcstrClsName, "Main", ex)

        Finally


            If gobjDB IsNot Nothing Then

                'close database
                If gobjDB.IsConnect() Then gobjDB.Close()
                gobjDB.Dispose()

            End If

        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xIsMultiOpen, check for multiple open of program
    '　　　VALUE      : Boolean
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xIsMultiOpen() As Boolean

        xIsMultiOpen = True

        Try

            Dim log As Long

            log = UBound(Diagnostics.Process.GetProcessesByName(Diagnostics.Process.GetCurrentProcess.ProcessName))

            If log > 0 Then

                basCommon.fncMessageError(mcstrMultiOpenError)

                Exit Function

            End If

            Return False

        Catch ex As Exception
            Throw ex
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
    Private Function xDatabaseExist() As Boolean

        xDatabaseExist = False

        Try
            Dim strDbPath As String

            strDbPath = My.Application.Info.DirectoryPath + basConst.gcstrDBPATH + basConst.gcstrDBNAME

            If Not System.IO.File.Exists(strDbPath) Then

                basCommon.fncMessageError(mcstrConnectionError)
                Exit Function

            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''   ******************************************************************
    ''　　　FUNCTION   : xIsDatabaseExist, check for existance of database
    ''　　　VALUE      : Boolean
    ''      PARAMS     : 
    ''      MEMO       : 
    ''      CREATE     : 2012/01/17  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xCheckActive() As Boolean
    '    xCheckActive = False
    '    Try

    '        'check active file
    '        Dim mstrActiveFile As String = Application.StartupPath + "\Activekey.txt"

    '        Dim Computer As New clsComputerInfo
    '        Dim strComputerID As String
    '        strComputerID = Environment.MachineName + "-" + Computer.GetProcessorId

    '        If System.IO.File.Exists(mstrActiveFile) Then

    '            'Get data from file to Array
    '            Dim strlines() As String = My.Computer.FileSystem.ReadAllText(mstrActiveFile).Replace(vbLf, "").Split(CChar(vbCr))

    '            If strlines.Length < 2 Then
    '                MessageBox.Show("Mã sản phẩm không phù hợp.")
    '                basCommon.fncDeleteFile(mstrActiveFile)
    '                Return False
    '            End If

    '            If IsConnectedToInternet() Then

    '                If fncGetComputerID(strlines(1)) <> strComputerID Then

    '                    MessageBox.Show("Mã sản phẩm không phù hợp." + vbCrLf + "Xin vui lòng nhập lại mã sản phẩm tại màn hình sau đây.")
    '                    basCommon.fncDeleteFile(mstrActiveFile)

    '                    Dim frmActive As frmActiveKey = New frmActiveKey

    '                    'QUYET comment dong nay ▼
    '                    frmActive.Run(strComputerID, 2)

    '                    If frmActive.mblnActiveOk = False Then Return False

    '                End If

    '            Else

    '                If strlines(0) <> getMD5Hash(strComputerID + "AKB") Then

    '                    MessageBox.Show("Mã sản phẩm không phù hợp." + vbCrLf + "Xin vui lòng nhập lại mã sản phẩm tại màn hình sau đây.")
    '                    basCommon.fncDeleteFile(mstrActiveFile)

    '                    Dim frmActive As frmActiveKey = New frmActiveKey

    '                    'QUYET comment dong nay ▼
    '                    frmActive.Run(strComputerID, 2)


    '                    If frmActive.mblnActiveOk = False Then Return False

    '                End If

    '            End If

    '        Else

    '            Dim frmActive As frmActiveKey = New frmActiveKey

    '            'QUYET comment dong nay ▼
    '            frmActive.Run(strComputerID, 1)

    '            If frmActive.mblnActiveOk = False Then Return False

    '        End If

    '        Return True
    '    Catch ex As Exception

    '        Throw

    '    End Try

    'End Function

    ''   ******************************************************************
    ''　　　FUNCTION   : fncGetComputerID
    ''　　　VALUE      : Boolean
    ''      PARAMS     : 
    ''      MEMO       : 
    ''      CREATE     : 2012/01/17  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function fncGetComputerID(ByVal strKey As String) As String
    '    fncGetComputerID = ""
    '    Try

    '        Dim strGETCOMPUTERURL As String = "http://akb.com.vn/Giapha/ActiveKey.aspx?CID={0}&KEY={1}&Phone={2}&Name={3}&Birth={4}&Type=1"
    '        'Dim strGETCOMPUTERURL As String = "http://localhost:1272/GiaphaActive/ActiveKey.aspx?CID={0}&KEY={1}&Phone={2}&Name={3}&Birth={4}&Type=1"
    '        Dim strLink = String.Format(strGETCOMPUTERURL, " ", strKey, " ", " ", " ")

    '        Dim postData As String = ""

    '        ' Read the content.
    '        Dim responseFromServer As String = fncResponse(strLink, gcstrServerPass)

    '        fncGetComputerID = responseFromServer

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message)

    '    End Try

    'End Function

End Module
