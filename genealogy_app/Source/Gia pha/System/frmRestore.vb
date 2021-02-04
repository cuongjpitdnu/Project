'   ******************************************************************
'      TITLE      : RESTORING FORM
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/12/20　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************

'   ******************************************************************
'　　　FUNCTION   : Retore data class
'      MEMO       : 
'      CREATE     : 2011/12/20  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmRestore

    Private Const mcstrClsName As String = "frmRestore"                                     'class name
    Private Const mcstrFileNotExist As String = "File không tồn tại hoặc không đúng định dạng."          'file does not exist

    Private mblnRestored As Boolean                                     'process result
    Private mstrRestorePath As String                                   'restore path
    Private mfrmWaitingForm As frmProgress                              'waiting form
    Private mobjWaitingThread As System.Threading.Thread                'waiting thread

    '   ******************************************************************
    '　　　FUNCTION   : Restored Property, Process result
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property Restored() As Boolean
        Get
            Return mblnRestored
        End Get
    End Property


    '   ******************************************************************
    '　　　FUNCTION   : Restored Property, Process result
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mblnRestored = False

    End Sub


#Region "Form's events"

    '   ******************************************************************
    '　　　FUNCTION   : bntBrowse_Click, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub bntBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntBrowse.Click

        Try
            xSelectFile()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnRestore_Click, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestore.Click

        Try
            mstrRestorePath = txtFileName.Text.Trim()

            'check valid of file path
            If Not xIsValid(mstrRestorePath) Then

                basCommon.fncMessageWarning(mcstrFileNotExist)
                Exit Sub

            End If

            If Not fncMessageConfirm("Dữ liệu hiện tại sẽ bị thay thế bởi dữ liệu trong tệp tin vừa chọn." & vbCrLf & "Bạn đã chắc chắn chưa?", "Phần mềm quản lý gia phả") Then Return

            'new thread
            mobjWaitingThread = New System.Threading.Thread(AddressOf xRestore)
            mfrmWaitingForm = New frmProgress()

            'start thread and show waiting screen
            mobjWaitingThread.Start()
            mfrmWaitingForm.ShowDialog()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click, button clicked
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Me.Close()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try

    End Sub


#End Region


#Region "Function and method"

    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show form
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm() As Boolean

        fncShowForm = False

        Try

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSelectFile, select restore file
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSelectFile() As Boolean

        xSelectFile = False

        Dim objOpenFile As OpenFileDialog = Nothing

        Try
            objOpenFile = New OpenFileDialog()

            objOpenFile.DefaultExt = basConst.gcstrBackupFileExt
            objOpenFile.Filter = basConst.gcstrBackupFileFilter

            If objOpenFile.ShowDialog() = Windows.Forms.DialogResult.OK Then

                txtFileName.Text = objOpenFile.FileName

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSelectFile", ex)
        Finally
            If objOpenFile IsNot Nothing Then objOpenFile.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xRestore, start restoring
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xRestore()

        Dim objRestore As clsReplication = Nothing

        Try
            objRestore = New clsReplication()

            AddHandler objRestore.evnRestored, AddressOf xRestoredSuccess

            objRestore.fncRestore(mstrRestorePath)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xRestore", ex)
        Finally
            objRestore = Nothing
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xIsValid, check validation of file
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : strPath String, file path
    '      MEMO       : 
    '      CREATE     : 2011/12/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xIsValid(ByVal strPath As String) As Boolean

        xIsValid = False

        Try
            If String.IsNullOrEmpty(strPath) Then Return False

            If Not System.IO.File.Exists(strPath) Then Return False

            'does not end with .gpb
            If Not strPath.EndsWith(basConst.gcstrBackupFileExt) Then Return False

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xIsValid", ex)

        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xBackedupSuccess, handle backup success
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xRestoredSuccess(ByVal blnSuccess As Boolean)

        Try
            Dim objCloseWaitForm As MethodInvoker

            Me.mblnRestored = blnSuccess

            'close thread
            mobjWaitingThread = Nothing

            'close waiting form
            objCloseWaitForm = New MethodInvoker(AddressOf xCloseWaitForm)
            Me.Invoke(objCloseWaitForm)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xBackup", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCloseWaitForm, close waiting form
    '      MEMO       : 
    '      CREATE     : 2011/12/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCloseWaitForm()

        Try
            mfrmWaitingForm.Close()
            mfrmWaitingForm.Dispose()

            'update flag and close form
            'Me.mblnRestored = True
            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xBackup", ex)
        End Try

    End Sub

#End Region


End Class