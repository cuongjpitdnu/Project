<System.ComponentModel.RunInstaller(True)> Partial Class EmcDataImportInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
        Me.serviceProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller()
        Me.serviceInstaller = New System.ServiceProcess.ServiceInstaller()
        '
        'serviceProcessInstaller
        '
        Me.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
        Me.serviceProcessInstaller.Password = Nothing
        Me.serviceProcessInstaller.Username = Nothing
        '
        'serviceInstaller
        '
        Me.serviceInstaller.ServiceName = "EmcDataImport"
        Me.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'EmcDataImportInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.serviceProcessInstaller, Me.serviceInstaller})

    End Sub

    Friend WithEvents serviceProcessInstaller As ServiceProcess.ServiceProcessInstaller
    Friend WithEvents serviceInstaller As ServiceProcess.ServiceInstaller
End Class
