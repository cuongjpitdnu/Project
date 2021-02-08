Imports System.Diagnostics
Imports System.ServiceProcess

Namespace PcMoniter
    Partial Public Class PcMoniterService
        Inherits ServiceBase

        Private components As System.ComponentModel.IContainer

        <DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso Me.components IsNot Nothing Then
                Me.components.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.AutoLog = False
            Me.ServiceName = "PcMoniterService"
        End Sub
    End Class
End Namespace
