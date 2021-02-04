'   ******************************************************************
'      TITLE      : frmProgressBar
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2012/10/03　AKB Vinh
'      UPDATE     : 
'   ******************************************************************
Public Class frmProgressBar
    Private mstrConfirmClose As String = "Bạn có chắc chắn muốn hủy bỏ việc đưa dữ liệu ra file Excel?"
    Delegate Sub UpdateValue(ByVal intprogress As Integer)
    Delegate Sub CloseForm()

    '   ******************************************************************
    '　　　FUNCTION   : UpdateProgressInit
    '      MEMO       : 
    '      CREATE     :2012/10/03　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub UpdateProgressInit(ByVal intprogress As Integer)

        Try
            prgbarsGiapha.Value = intprogress
            lblPercent.Text = intprogress.ToString + "%"
        Catch ex As Exception

        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : UpdatePro
    '      MEMO       : 
    '      CREATE     : 2012/10/03　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub UpdatePro(ByVal intprogress As Integer)

        Try
            If Me.prgbarsGiapha.InvokeRequired Then
                Me.prgbarsGiapha.Invoke(New UpdateValue(AddressOf UpdatePro), New Object() {intprogress})
            Else
                UpdateProgressInit(intprogress)
            End If

        Catch ex As Exception

        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   :CloseTheForm
    '      MEMO       : 
    '      CREATE     : 2012/10/03　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub CloseTheForm()

        Try
            If Me.prgbarsGiapha.InvokeRequired Then
                Me.Invoke(New CloseForm(AddressOf xClose))
            Else
                Me.Close()
            End If

        Catch ex As Exception

        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xClose
    '      MEMO       : 
    '      CREATE     : 2012/10/03　AKB Vinh 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xClose()
        Try
            Me.Close()
        Catch ex As Exception

        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click
    '      MEMO       : 
    '      CREATE     : 2012/10/03　AKB Vinh 
    '      UPDATE     : 
    '   ******************************************************************

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            If fncMessageConfirm(mstrConfirmClose) Then
                Me.Close()
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class