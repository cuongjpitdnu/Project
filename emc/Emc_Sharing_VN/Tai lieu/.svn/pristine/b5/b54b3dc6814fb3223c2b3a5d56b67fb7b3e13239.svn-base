<System.ComponentModel.RunInstaller(True)> Partial Class clsEmcDataImportInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ServiceProcessInstaller1 = New System.ServiceProcess.ServiceProcessInstaller()
        Me.ServiceInstaller1 = New System.ServiceProcess.ServiceInstaller()
        '
        'ServiceProcessInstaller1
        '
        Me.ServiceProcessInstaller1.Password = Nothing
        Me.ServiceProcessInstaller1.Username = Nothing
        '
        'ServiceInstaller1
        '
        Me.ServiceInstaller1.DisplayName = "EmcDataImport"
        Me.ServiceInstaller1.ServiceName = "EmcDataImport"
        Me.ServiceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'clsEmcDataImportInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.ServiceProcessInstaller1, Me.ServiceInstaller1})

    End Sub
    Private WithEvents ServiceInstaller1 As ServiceProcess.ServiceInstaller
    Public WithEvents ServiceProcessInstaller1 As ServiceProcess.ServiceProcessInstaller
End Class
