Imports System.Windows.Forms

Public Class frmConfirm

    Public ReadOnly Property SelectCase() As clsEnum.emSelect
        Get

            If rdCase1.Checked Then Return clsEnum.emSelect.CASE1
            If rdCase2.Checked Then Return clsEnum.emSelect.CASE2
            If rdCase3.Checked Then Return clsEnum.emSelect.CASE3

        End Get
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
