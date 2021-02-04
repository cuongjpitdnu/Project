'   ******************************************************************
'      TITLE      : BACKUP
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/12/20　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Imports System.Windows.Forms


'   ******************************************************************
'　　　FUNCTION   : Backup
'      MEMO       : 
'      CREATE     : 2011/12/20　AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmProgress


    Private Const mcstrClsName As String = "frmRestore"             'class name


    '   ******************************************************************
    '　　　FUNCTION   : frmProgress_FormClosing, 
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmProgress_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Try
            e.Cancel = True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xBackup", ex)

        End Try

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
