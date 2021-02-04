'   ******************************************************************
'      TITLE      : Login form
'　　　FUNCTION   : 
'      MEMO       : 
'      CREATE     : 2011/07/18  AKB Quyet
'      UPDATE     : 
'
'           2011 AKB Software
'   ******************************************************************

Option Explicit On

'   ******************************************************************
'　　　FUNCTION   : Login form
'      MEMO       : 
'      CREATE     : 2011/07/18  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmLogin

    Private Const mcstrClsName As String = "frmLogin"                                                       'class name
    Private Const mcstrMissingLoginUser As String = "Tên đăng nhập chưa được nhập."                               'message when missing username
    Private Const mcstrMissingLoginPass As String = "Mật khẩu chưa được nhập."                                   'message when missing password
    Private Const mcstrLoginWrong As String = "Tên đăng nhập hoặc mật khẩu bị sai."                                  'message when wrong username or password


    Private mblnLogined As Boolean = False                                       'flag to check user loged in


#Region "Properties"


    '   ******************************************************************
    '　　　FUNCTION   : SystemLogined Property
    '      MEMO       : 
    '      CREATE     : 2011/07/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    ReadOnly Property SystemLogined() As Boolean

        Get
            Return Me.mblnLogined
        End Get

    End Property


#End Region

#Region "Controls Events"


    '   ******************************************************************
    '　　　FUNCTION   : btnLogin_Click, Login Button Clicked
    '      MEMO       : 
    '      CREATE     : 2011/07/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click

        Dim objUserTable As DataTable
        objUserTable = Nothing

        Dim strEncryptedPass As String = ""
        Dim strName As String = ""
        Dim strPass As String = ""

        Try
            'check username and password
            If xCheckLogin() Then

                strName = txtName.Text
                strPass = txtPass.Text

                'encrypt password
                strEncryptedPass = basCommon.fncEncyptPass(strPass)

                objUserTable = gobjDB.fncGetUser(strName, strEncryptedPass)

                If objUserTable IsNot Nothing Then

                    'login success
                    Me.mblnLogined = True

                Else

                    'login failed. show message
                    basCommon.fncMessageWarning(mcstrLoginWrong, txtName)

                End If

            End If

        Catch ex As Exception

            Call basCommon.fncSaveErr(mcstrClsName, "btnLogin_Click", ex)

        Finally

            If objUserTable IsNot Nothing Then objUserTable.Dispose()

            If mblnLogined Then Me.Close()

        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click, Cancel Button Clicked
    '      MEMO       : 
    '      CREATE     : 2011/07/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtName_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName.KeyPress

        Try

            'Focus to txtPass if Enter key is pressed
            basCommon.fncSendTAB(e)

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "txtName_KeyPress", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtPass_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtPass_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPass.KeyPress

        Try

            'Focus to btnLogin if Enter key is pressed
            basCommon.fncSendTAB(e)

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "txtPass_KeyPress", ex)

        End Try

    End Sub


#End Region

#Region "Form's function"

    '   ******************************************************************
    '　　　FUNCTION   : xCheckLogin, check input username and password
    '　　　VALUE      : boolean, true - valid, false - invalid
    '      PARAMS1    : objUser Control, username textbox
    '      PARAMS2    : objPass Control, password textbox
    '      MEMO       : 
    '      CREATE     : 2011/07/18  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCheckLogin() As Boolean

        xCheckLogin = False

        Try

            'check blank username and password then show message
            If basCommon.fncIsBlank(txtName.Text, mcstrMissingLoginUser, txtName) Then Exit Function
            If basCommon.fncIsBlank(txtPass.Text, mcstrMissingLoginPass, txtPass) Then Exit Function

        Catch ex As Exception

            Throw ex

        End Try

        Return True

    End Function

#End Region

End Class