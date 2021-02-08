
Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TestDB()
    End Sub



    Private Shared Sub TestDB()

        Const mcstrConnectionString As String = "Data Source=localhost\sqlexpress;Initial Catalog=info_db;Persist Security Info=True;User ID=info_user;Password=info_pass;"  ' DB接続文字列.

        Dim clsDb As clsDbAccess
        clsDb = New clsDbAccess

        Try
            Dim intIdx As Integer

            clsDb.Open(mcstrConnectionString)

            MsgBox("Open")


            intIdx = clsDb.GetCount()
            MsgBox("SELECT=" & CStr(intIdx))

            clsDb.UpdateUser()
            MsgBox("UPDATE")

            clsDb.Close()
            clsDb = Nothing
            MsgBox("Close")

        Catch ex As Exception

        Finally

            If clsDb IsNot Nothing Then
                clsDb.Close()
            End If

            clsDb = Nothing

        End Try



    End Sub



End Class
