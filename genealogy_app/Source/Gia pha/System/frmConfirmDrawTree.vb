'   ******************************************************************
'      TITLE      : frmConfirmDrawTree
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2012/10/03 AKB Vinh 
'      UPDATE     : 
'   ******************************************************************
Public Class frmConfirmDrawTree
    Public mblnIsConfirm As Boolean = False
    Public Property IsConfirm() As Boolean
        Get
            Return mblnIsConfirm
        End Get
        Set(ByVal value As Boolean)
            mblnIsConfirm = value
        End Set
    End Property
    Private mstrCheckedCheckBox As String = "Bạn phải chọn kiểu xuất dữ liệu"


    '   ******************************************************************
    '　　　FUNCTION   : btnOK_Click
    '      MEMO       : 
    '      CREATE     : 2012/10/03 AKB Vinh 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            If chkBoxAdvance.Checked = False And chkBoxNomal.Checked = False Then
                basCommon.fncMessageWarning(mstrCheckedCheckBox)
                Return
            End If
            mblnIsConfirm = True
            Me.Close()
        Catch ex As Exception

        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : chkBoxNomal_MouseClick
    '      MEMO       : 
    '      CREATE     : 2012/10/03 AKB Vinh 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub chkBoxNomal_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles chkBoxNomal.MouseClick
        If chkBoxAdvance.Checked = True Then
            chkBoxAdvance.Checked = False

        End If
        If chkBoxNomal.Checked = True Then
            gblnDrawTreeAdvance = False
        Else
            chkBoxNomal.Checked = False
        End If
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : chkBoxAdvance_MouseClick
    '      MEMO       : 
    '      CREATE     : 2012/10/03 AKB Vinh 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub chkBoxAdvance_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles chkBoxAdvance.MouseClick
        If chkBoxNomal.Checked = True Then
            chkBoxNomal.Checked = False

        End If
        If chkBoxAdvance.Checked = True Then
            gblnDrawTreeAdvance = True
        Else
            chkBoxAdvance.Checked = False
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            Me.Dispose()
        Catch ex As Exception

        End Try
    End Sub
End Class