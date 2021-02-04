Public NotInheritable Class frmAbout

    Private Sub frmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("Thông tin {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the "Application" pane of the project 
        '    properties dialog (under the "Project" menu).
        'Me.LabelProductName.Text = My.Application.Info.ProductName
        'Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        'Me.LabelCopyright.Text = My.Application.Info.Copyright
        'Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        'Me.TextBoxDescription.Text = My.Application.Info.Description

        lblProductName.Text = My.Application.Info.ProductName
        lblCopyright.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        lblVersion.Text = My.Application.Info.Copyright & " " & Now.Year.ToString()
        lblCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = ""
        Me.TextBoxDescription.Text &= "Mọi thắc mắc xin liên hệ:" & vbCrLf & vbCrLf
        Me.TextBoxDescription.Text &= "Công ty TNHH Liên doanh phần mềm AKB Software" & vbCrLf
        Me.TextBoxDescription.Text &= "Số 15, Ngõ 64, Lê Trọng Tấn - Khương Mai - Thanh Xuân - Hà Nội" & vbCrLf
        Me.TextBoxDescription.Text &= "Tel:  (84.24) 37877.529" & vbCrLf
        Me.TextBoxDescription.Text &= "Fax: (84.24) 37877.533" & vbCrLf
        Me.TextBoxDescription.Text &= "Email: hotro@akb.com.vn" & vbCrLf
        Me.TextBoxDescription.Text &= "Website: http://www.akb.com.vn/CompanyInfo/phan-mem-quan-ly-gia-pha.html"

    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

End Class
