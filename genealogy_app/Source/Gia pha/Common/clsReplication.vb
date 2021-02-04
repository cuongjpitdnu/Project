'   ******************************************************************
'      TITLE      : RESTORE / BACKUP
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/12/20　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************

Option Explicit On
Option Strict On

Imports System.IO
Imports Config_Gia_Pha

'   ******************************************************************
'　　　FUNCTION   : Restore / Backup
'      MEMO       : 
'      CREATE     : 2011/12/20　AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class clsReplication

    Private Const mcstrBackupFail As String = "Sao lưu dữ liệu thất bại."               'backup failed
    Private Const mcstrRestoreFail As String = "Phục hồi dữ liệu thất bại."              'restore failed

    'Private Const mcstrDocTemp As String = "\docs_temp\"
    Private Const mcstrDocTemp As String = "docs_temp"

    Public Event evnBackedUp()
    Public Event evnRestored(ByVal blnStatus As Boolean)

    '   ******************************************************************
    '　　　FUNCTION   : Constructor
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New()

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncBackup, backup
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : strPath String
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncBackup(ByVal strPath As String) As Boolean

        fncBackup = False

        Dim objZip As ICSharpCode.SharpZipLib.Zip.ZipFile = Nothing


        Try
            Dim strImage As String
            'Dim strDocs As String
            Dim strData As String

            strImage = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder
            'strDocs = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder
            strData = My.Application.Info.DirectoryPath & basConst.gcstrDBPATH

            'close database before backing up
            gobjDB.Close()

            objZip = ICSharpCode.SharpZipLib.Zip.ZipFile.Create(strPath)

            objZip.BeginUpdate()
            objZip.Password = Config.Decrypt(basConst.gcstrBackupPass)

            xAddDir(objZip, strImage)
            'xAddDir(objZip, strDocs)
            xAddDir(objZip, strData)

            Try
                objZip.CommitUpdate()

            Catch ex As Exception
                basCommon.fncMessageWarning(mcstrBackupFail)
                Return False
            End Try

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            're-open database
            If Not gobjDB.IsConnect Then gobjDB.Open()

            If objZip IsNot Nothing Then objZip.Close()
            objZip = Nothing

            RaiseEvent evnBackedUp()

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncRestore, restore data
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : strPath String, data file path
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncRestore(ByVal strPath As String) As Boolean

        fncRestore = False

        Dim blnBackedup As Boolean = False
        Dim blnSuccess As Boolean = False
        Dim strTempFolder As String

        Try

            'close database before restoring
            gobjDB.Close()

            blnBackedup = xBackupCurrentData()

            If Not blnBackedup Then Exit Try

            If Not xDeleteCurrentData() Then

                xRestoreCurrentData()
                Exit Try

            End If

            If Not xExtract(strPath) Then

                xRestoreCurrentData()
                Exit Try

            End If

            blnSuccess = True

            'delete temp folder
            Try
                strTempFolder = My.Application.Info.DirectoryPath & basConst.gcstrBackupFolder
                basCommon.fncDeleteFolder(strTempFolder)
            Catch ex As Exception
            End Try

            Return True

        Catch ex As Exception

            Try
                If blnBackedup Then xRestoreCurrentData()
            Catch e As Exception
            End Try

            Throw ex

        Finally
            're-open database
            If Not gobjDB.IsConnect Then gobjDB.Open()

            If Not blnSuccess Then
                basCommon.fncMessageWarning(mcstrRestoreFail)
                'delete temp folder
                Try
                    strTempFolder = My.Application.Info.DirectoryPath & basConst.gcstrBackupFolder
                    basCommon.fncDeleteFolder(strTempFolder)
                Catch ex As Exception
                End Try
            End If

            'raise event
            RaiseEvent evnRestored(blnSuccess)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddDir, add a dir to zip file
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : objZip  ZipFile
    '      PARAMS     : strDir  String
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddDir(ByVal objZip As ICSharpCode.SharpZipLib.Zip.ZipFile, ByVal strDir As String) As Boolean

        xAddDir = False

        Dim objdir As System.IO.DirectoryInfo = Nothing

        Try
            objdir = New System.IO.DirectoryInfo(strDir)
            xAddRecusiveDir(objZip, objdir, "")

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If objdir IsNot Nothing Then objdir = Nothing
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddRecusiveDir, add dir to zip file
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : objZip  ZipFile
    '      PARAMS     : objDir  DirectoryInfo
    '      PARAMS     : strFolderName  String
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddRecusiveDir(ByVal objZip As ICSharpCode.SharpZipLib.Zip.ZipFile, ByVal objDir As System.IO.DirectoryInfo, ByVal strFolderName As String) As Boolean

        xAddRecusiveDir = False

        Try
            Dim strSubFolderName As String

            strSubFolderName = objDir.Name

            'add file to zip
            For Each file As System.IO.FileInfo In objDir.GetFiles()

                objZip.Add(file.FullName, strFolderName & "\" & strSubFolderName & "\" & file.Name)

            Next

            'recusive sub-folder
            For Each dir As System.IO.DirectoryInfo In objDir.GetDirectories()

                xAddRecusiveDir(objZip, dir, strFolderName & "\" & strSubFolderName & "\")

            Next

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xExtract, extract zip file
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : strPath String, destination folder
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xExtract(ByVal strPath As String) As Boolean

        xExtract = False

        Dim objZip As ICSharpCode.SharpZipLib.Zip.FastZip = Nothing

        Try
            ' Edit by: 2019.02.18 AKB Nguyen Thanh Tung - Start

            Dim strExtractFolder As String

            strExtractFolder = My.Application.Info.DirectoryPath

            Try

                'read zip file
                objZip = New ICSharpCode.SharpZipLib.Zip.FastZip()

                'extract
                objZip.Password = Config.Decrypt(basConst.gcstrBackupPass)
                objZip.ExtractZip(strPath, strExtractFolder, "")
            Catch ex As Exception
                Return False
            End Try

            Return True

            'Dim strExtractFolder As String
            'Dim strDocs As String
            'Dim strDocsTemp As String

            'strExtractFolder = My.Application.Info.DirectoryPath
            'strDocs = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder
            ''strDocs = basConst.gcstrDocsFolder.Replace("\", "")
            'strDocsTemp = Path.Combine(My.Application.Info.DirectoryPath, mcstrDocTemp)

            ''read zip file
            'objZip = New ICSharpCode.SharpZipLib.Zip.FastZip()

            ''extract
            'objZip.Password = Config.Decrypt(basConst.gcstrBackupPass)

            'Try
            '    objZip.ExtractZip(strPath, strExtractFolder, "")

            '    basCommon.fncDeleteFolder(strDocs)
            '    basCommon.fncRenameFolder(strDocsTemp, basConst.gcstrDocsFolder.Replace("\", ""))

            'Catch ex As Exception
            '    Return False
            'End Try

            'Return True
            ' Edit by: 2019.02.18 AKB Nguyen Thanh Tung - End
        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xBackupCurrentData,backup data before restoring
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xBackupCurrentData() As Boolean

        xBackupCurrentData = False

        Dim strTempFolder As String = String.Empty

        Try
            ' Edit by: 2019.02.18 AKB Nguyen Thanh Tung - Start

            Dim strImage As String
            Dim strData As String

            strImage = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder
            strData = My.Application.Info.DirectoryPath & basConst.gcstrDBPATH
            strTempFolder = My.Application.Info.DirectoryPath & basConst.gcstrBackupFolder

            Try
                'create temp folder
                basCommon.fncCreateFolder(strTempFolder, True)

                'copy current data to temp folder
                If Not basCommon.fncCopyFolder(strImage, strTempFolder & "\" & basConst.gcstrImageFolder, False, True) Then Return False
                If Not basCommon.fncCopyFolder(strData, strTempFolder & "\" & basConst.gcstrDBPATH, False, True) Then Return False

            Catch ex As Exception
                Return False
            End Try

            Return True


            'Dim strImage As String
            'Dim strDocs As String
            'Dim strDocsTemp As String
            'Dim strData As String

            'strImage = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder
            'strDocs = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder
            ''strDocsTemp = My.Application.Info.DirectoryPath & mcstrDocTemp
            'strDocsTemp = mcstrDocTemp
            'strData = My.Application.Info.DirectoryPath & basConst.gcstrDBPATH
            'strTempFolder = My.Application.Info.DirectoryPath & basConst.gcstrBackupFolder

            'Try
            '    'create temp folder
            '    basCommon.fncCreateFolder(strTempFolder, True)

            '    'copy current data to temp folder
            '    If Not basCommon.fncCopyFolder(strImage, strTempFolder & "\" & basConst.gcstrImageFolder, False, True) Then Return False
            '    'If Not basCommon.fncCopyFolder(strDocs, strTempFolder & "\" & basConst.gcstrDocsFolder, False, True) Then Return False
            '    If Not basCommon.fncRenameFolder(strDocs, strDocsTemp) Then Return False
            '    If Not basCommon.fncCopyFolder(strData, strTempFolder & "\" & basConst.gcstrDBPATH, False, True) Then Return False

            'Catch ex As Exception
            '    Return False
            'End Try

            'Return True
            ' Edit by: 2019.02.18 AKB Nguyen Thanh Tung - End
        Catch ex As Exception
            'delete temp folder when error occurs
            basCommon.fncDeleteFolder(strTempFolder)

            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xRestoreCurrentData, restore current data when error
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xRestoreCurrentData() As Boolean

        xRestoreCurrentData = False

        Dim strTempFolder As String = String.Empty

        Try

            ' Edit by: 2019.02.18 AKB Nguyen Thanh Tung - Start
            Dim strImage As String
            Dim strData As String

            strImage = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder
            strData = My.Application.Info.DirectoryPath & basConst.gcstrDBPATH
            strTempFolder = My.Application.Info.DirectoryPath & gcstrBackupFolder

            'copy current data to temp folder
            Try
                basCommon.fncCopyFolder(strTempFolder & "\" & basConst.gcstrDBPATH, strData, True, True)
                basCommon.fncCopyFolder(strTempFolder & "\" & basConst.gcstrImageFolder, strImage, True, True)
            Catch ex As Exception
                Return False
            End Try

            'delete temp folder 
            basCommon.fncDeleteFolder(strTempFolder)

            Return True

            'Dim strImage As String
            'Dim strDocs As String
            'Dim strDocsTemp As String
            'Dim strData As String

            'strImage = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder
            'strDocs = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder
            ''strDocsTemp = My.Application.Info.DirectoryPath & mcstrDocTemp
            'strDocsTemp = mcstrDocTemp
            'strData = My.Application.Info.DirectoryPath & basConst.gcstrDBPATH
            'strTempFolder = My.Application.Info.DirectoryPath & gcstrBackupFolder

            ''copy current data to temp folder
            'Try
            '    basCommon.fncCopyFolder(strTempFolder & "\" & basConst.gcstrDBPATH, strData, True, True)
            '    basCommon.fncCopyFolder(strTempFolder & "\" & basConst.gcstrImageFolder, strImage, True, True)
            '    'basCommon.fncCopyFolder(strTempFolder & "\" & basConst.gcstrDocsFolder, strDocs, True, True)
            '    basCommon.fncRenameFolder(strDocsTemp, strDocs)
            'Catch ex As Exception
            '    Return False
            'End Try

            ''delete temp folder 
            'basCommon.fncDeleteFolder(strTempFolder)

            'Return True
            ' Edit by: 2019.02.18 AKB Nguyen Thanh Tung - End
        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDeleteCurrentData, delete current data
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDeleteCurrentData() As Boolean

        xDeleteCurrentData = False

        Try
            Dim strImage As String
            'Dim strDocs As String
            Dim strData As String

            strImage = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder
            'strDocs = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder
            strData = My.Application.Info.DirectoryPath & basConst.gcstrDBPATH

            'copy current data to temp folder
            Try
                If Not basCommon.fncDeleteFolder(strImage) Then Return False
                'If Not basCommon.fncDeleteFolder(strDocs) Then Return False
                If Not basCommon.fncDeleteFolder(strData) Then Return False
            Catch ex As Exception
                Return False
            End Try

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


End Class