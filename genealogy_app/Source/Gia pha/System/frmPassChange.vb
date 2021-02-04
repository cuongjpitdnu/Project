'   ****************************************************************** 
'      TITLE      : Change Password Form 
'　　　FUNCTION   :  
'      MEMO       :  
'      CREATE     : 2011/07/20　AKB　Quyet 
'      UPDATE     :  
' 
'           2011 AKB SOFTWARE 
'   ******************************************************************
Option Explicit On
Option Strict On


'   ****************************************************************** 
'　　　FUNCTION   : Change Password class 
'      MEMO       :  
'      CREATE     : 2011/07/20　AKB　Quyet 
'      UPDATE     :  
'   ******************************************************************
Public Class frmPassChange

#Region "Form variable"

    Private Const mcstrClsName As String = "frmPassChange"                                                  'class name

    Private Const mcstrMessageAlphabetError As String = "Tên đăng nhập không được bao gồm các ký tự ngoài Alphabet."      'message when check alphabet matches
    Private Const mcstrMessageAskConfirm As String = "Dữ liệu sẽ được ghi, bạn đã chắc chắn?"                         'message to confirm before updating

    Private Const mcstrMissingName As String = "Tên đăng nhập chưa được nhập."                                    'message when missing username
    Private Const mcstrWrongPassword As String = "Mật khẩu hiện tại chưa đúng."                                    'message when current password is incorrect
    Private Const mcstrMissingNewPass As String = "Mật khẩu mới chưa được nhập."                                  'message when missing new password
    Private Const mcstrWrongConfirmPass As String = "Mật khẩu xác nhận không trùng với mật khẩu mới."                   'message when new password and confirm password is not match

    Private mintUserID As Integer                                                                           'ID of user, default is 1
    Private mstrNameCurrent As String                                                                       'current username
    Private mstrPassCurrent As String                                                                       'current user's password

#End Region


#Region "Form controls events"


    '   ******************************************************************
    '　　　FUNCTION   : frmPassChange_Load, Form load
    '      MEMO       : 
    '      CREATE     : 2011/07/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmPassChange_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            'initialize values
            xInit()

            txtName.Text = Me.mstrNameCurrent

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "frmPassChange_Load", ex)
            Me.Dispose()

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnChange_Click, Change button clicked
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click

        Dim stUserData As clsDbAccess.stUserInfo                'structure that store user's information

        Dim strName As String = String.Empty
        Dim strPass As String = String.Empty

        strName = txtName.Text.Trim()                           'username
        strPass = txtPassNew.Text                               'password

        Try

            'check validation of input
            If Not xIsValidInput() Then Exit Sub

            'show confirm message
            If Not basCommon.fncMessageConfirm(mcstrMessageAskConfirm, txtName) Then Exit Sub

            strPass = basCommon.fncEncyptPass(strPass)              'encrypted password

            'set user's information to update
            stUserData.intUserID = Me.mintUserID
            stUserData.strName = strName
            stUserData.strPass = strPass

            'update user'information then close window
            If gobjDB.fncUpdateUser(stUserData) Then Me.Dispose()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "btnChange_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click, Cancel Button Clicked, close windows
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try

            Me.Dispose()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtName_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName.KeyPress
        Try

            'Focus to txtPassOld if Enter key is pressed
            basCommon.fncSendTAB(e)

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "txtName_KeyPress", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtPassOld_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtPassOld_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassOld.KeyPress
        Try

            'Focus to txtPassNew if Enter key is pressed
            basCommon.fncSendTAB(e)

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "txtPassOld_KeyPress", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtPassNew_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtPassNew_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassNew.KeyPress
        Try

            'Focus to txtPassConfirm if Enter key is pressed
            basCommon.fncSendTAB(e)

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "txtPassNew_KeyPress", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtPassConfirm_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtPassConfirm_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassConfirm.KeyPress

        Try

            'Focus to btnChange if Enter key is pressed
            basCommon.fncSendTAB(e)

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "txtPassConfirm_KeyPress", ex)

        End Try

    End Sub


#End Region


#Region "Form's functions"


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, initialize values
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm() As Boolean

        fncShowForm = False

        Try

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xInit, initialize values
    '      MEMO       : 
    '      CREATE     : 2011/07/21  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xInit()

        Me.mintUserID = 1
        Me.mstrNameCurrent = String.Empty
        Me.mstrPassCurrent = String.Empty

        'get user information
        If Not xGetUserInfo() Then Me.Close()

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xIsValidInput, check validation of input value
    '　　　VALUE      : boolean, true - valid, false - invalid
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xIsValidInput() As Boolean

        xIsValidInput = False

        Dim strName As String                           'username
        Dim strPassOld As String                        'current password
        Dim strPassNew As String                        'new password
        Dim strPassConfirm As String                    'confirm password

        Try

            strName = txtName.Text.Trim()
            strPassOld = txtPassOld.Text
            strPassNew = txtPassNew.Text
            strPassConfirm = txtPassConfirm.Text

            'check blank username
            If basCommon.fncIsBlank(strName, mcstrMissingName, txtName) Then Exit Function

            'check special character in username
            If basCommon.fncHasSpecialCharacter(strName, mcstrMessageAlphabetError, txtName) Then Exit Function

            'check blank current password
            If basCommon.fncIsBlank(strPassOld, mcstrWrongPassword, txtPassOld) Then Exit Function


            'ecrypt password
            strPassOld = basCommon.fncEncyptPass(strPassOld)

            'compare current password
            If String.Compare(strPassOld, Me.mstrPassCurrent, False) <> 0 Then

                basCommon.fncMessageWarning(mcstrWrongPassword, txtPassOld)

                Exit Function

            End If


            'check blank new password
            If basCommon.fncIsBlank(strPassNew, mcstrMissingNewPass, txtPassNew) Then Exit Function

            'check blank confirm password
            If basCommon.fncIsBlank(strPassConfirm, mcstrWrongConfirmPass, txtPassConfirm) Then Exit Function


            'compare new password
            If String.Compare(strPassNew, strPassConfirm, False) <> 0 Then

                basCommon.fncMessageWarning(mcstrWrongConfirmPass, txtPassConfirm)

                Exit Function

            End If

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xIsValidInput", ex)

        End Try

        Return True

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetPassword, get current password from database
    '　　　VALUE      : boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : get userid, username, password
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetUserInfo() As Boolean

        xGetUserInfo = False

        'instance to store user's info
        Dim objUserInfo As DataTable
        objUserInfo = Nothing

        Try
            'load user's info
            objUserInfo = gobjDB.fncGetUser()

            If objUserInfo Is Nothing Then Return False

            With objUserInfo.Rows(0)

                'get UserID
                If Not Integer.TryParse(basCommon.fncCnvNullToString(.Item("USERID")), Me.mintUserID) Then Exit Function

                'get username
                Me.mstrNameCurrent = basCommon.fncCnvNullToString(.Item("USERNAME"))

                'get password
                Me.mstrPassCurrent = basCommon.fncCnvNullToString(.Item("PASS_WORD"))

            End With

            Return True


        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xGetUserInfo", ex)

        Finally

            If objUserInfo IsNot Nothing Then objUserInfo.Dispose()

        End Try

        Exit Function

    End Function

#End Region

End Class