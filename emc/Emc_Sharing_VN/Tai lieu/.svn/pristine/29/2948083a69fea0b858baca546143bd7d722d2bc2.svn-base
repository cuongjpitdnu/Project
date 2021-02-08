Imports System.Configuration.Install
Imports System.Diagnostics
Imports System.ServiceProcess

Namespace PcMoniter
    Partial Public Class PcMoniterServiceInstaller
        Inherits Installer

        Private serviceInstaller As ServiceInstaller
        Private serviceProcessInstaller As ServiceProcessInstaller

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.serviceProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller()
            Me.serviceInstaller = New System.ServiceProcess.ServiceInstaller()
            Me.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
            Me.serviceProcessInstaller.Password = Nothing
            Me.serviceProcessInstaller.Username = Nothing
            Me.serviceInstaller.ServiceName = "PcMoniterService"
            Me.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic
            Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.serviceProcessInstaller, Me.serviceInstaller})
        End Sub
    End Class
End Namespace
