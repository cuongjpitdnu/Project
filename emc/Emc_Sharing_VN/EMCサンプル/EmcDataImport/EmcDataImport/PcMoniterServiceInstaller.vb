Imports System.ComponentModel
Imports System.Configuration.Install

Namespace PcMoniter
    <RunInstallerAttribute(True)>
    Partial Public Class PcMoniterServiceInstaller
        Inherits Installer

        Public Sub New()
            Me.InitializeComponent()
        End Sub
    End Class
End Namespace
