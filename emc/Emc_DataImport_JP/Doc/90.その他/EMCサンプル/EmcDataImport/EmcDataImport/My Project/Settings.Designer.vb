﻿'------------------------------------------------------------------------------
' <auto-generated>
'     このコードはツールによって生成されました。
'     ランタイム バージョン:4.0.30319.42000
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "My.Settings 自動保存機能"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("a")>  _
        Public Property DB_HOSTNAME() As String
            Get
                Return CType(Me("DB_HOSTNAME"),String)
            End Get
            Set
                Me("DB_HOSTNAME") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("a")>  _
        Public Property DB_DBNAME() As String
            Get
                Return CType(Me("DB_DBNAME"),String)
            End Get
            Set
                Me("DB_DBNAME") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("a")>  _
        Public Property DB_USERNAME() As String
            Get
                Return CType(Me("DB_USERNAME"),String)
            End Get
            Set
                Me("DB_USERNAME") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("a")>  _
        Public Property DB_USERPASSWORD() As String
            Get
                Return CType(Me("DB_USERPASSWORD"),String)
            End Get
            Set
                Me("DB_USERPASSWORD") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property MAIL_POP_HOST() As String
            Get
                Return CType(Me("MAIL_POP_HOST"),String)
            End Get
            Set
                Me("MAIL_POP_HOST") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("110")>  _
        Public Property MAIL_POP_PORT() As String
            Get
                Return CType(Me("MAIL_POP_PORT"),String)
            End Get
            Set
                Me("MAIL_POP_PORT") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property MAIL_POP_USER() As String
            Get
                Return CType(Me("MAIL_POP_USER"),String)
            End Get
            Set
                Me("MAIL_POP_USER") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property MAIL_POP_PASS() As String
            Get
                Return CType(Me("MAIL_POP_PASS"),String)
            End Get
            Set
                Me("MAIL_POP_PASS") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property MAIL_SMTP_HOST() As String
            Get
                Return CType(Me("MAIL_SMTP_HOST"),String)
            End Get
            Set
                Me("MAIL_SMTP_HOST") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("25")>  _
        Public Property MAIL_SMTP_PORT() As String
            Get
                Return CType(Me("MAIL_SMTP_PORT"),String)
            End Get
            Set
                Me("MAIL_SMTP_PORT") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property MAIL_SMTP_USER() As String
            Get
                Return CType(Me("MAIL_SMTP_USER"),String)
            End Get
            Set
                Me("MAIL_SMTP_USER") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property MAIL_SMTP_PASS() As String
            Get
                Return CType(Me("MAIL_SMTP_PASS"),String)
            End Get
            Set
                Me("MAIL_SMTP_PASS") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("60000")>  _
        Public Property EXECUTION_CYCLE() As String
            Get
                Return CType(Me("EXECUTION_CYCLE"),String)
            End Get
            Set
                Me("EXECUTION_CYCLE") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("C:\Project\NTTネオメイト\情報共有システム\SVR\a")>  _
        Public Property LOG_FOLDER() As String
            Get
                Return CType(Me("LOG_FOLDER"),String)
            End Get
            Set
                Me("LOG_FOLDER") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("C:\Project\NTTネオメイト\情報共有システム\SVR\b")>  _
        Public Property ERRLOG_FOLDER() As String
            Get
                Return CType(Me("ERRLOG_FOLDER"),String)
            End Get
            Set
                Me("ERRLOG_FOLDER") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
        Public Property DEBUGFLG() As String
            Get
                Return CType(Me("DEBUGFLG"),String)
            End Get
            Set
                Me("DEBUGFLG") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("A")>  _
        Public Property REG_USER_NO() As String
            Get
                Return CType(Me("REG_USER_NO"),String)
            End Get
            Set
                Me("REG_USER_NO") = value
            End Set
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.EmcDataImport.My.MySettings
            Get
                Return Global.EmcDataImport.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
