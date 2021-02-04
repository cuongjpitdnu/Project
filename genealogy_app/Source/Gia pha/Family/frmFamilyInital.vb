'   ******************************************************************
'      TITLE      : Family Information
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2012/01/17　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : Family Information
'      MEMO       : 
'      CREATE     : 2012/01/17　AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmFamilyInital

    Private Const mcstrClsName As String = "frmImage"       'class name

    Private mblnUpdate As Boolean = False                   'OK button pressed
    Private mblnIsEditMode As Boolean                       'form in edit mode
    Private mstrFName As String                             'family name
    Private mstrFAnni As String                             'family anniversary
    Private mstrFHome As String                             'family hometown

    '   ****************************************************************** 
    '      FUNCTION   : FamilyUpdated Property, information updated
    '      MEMO       :  
    '      CREATE     : 2012/01/17　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Public ReadOnly Property FamilyUpdated() As Boolean
        Get
            Return mblnUpdate
        End Get
    End Property


    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2012/01/17　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    '   ****************************************************************** 
    '      FUNCTION   : frmFamilyInital_Load, form loaded 
    '      MEMO       :  
    '      CREATE     : 2012/01/17　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Private Sub frmFamilyInital_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            If mblnIsEditMode Then
                xLoad()
            End If

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "frmFamilyInital_Load", ex, Nothing, False)

        End Try

    End Sub


    '   ****************************************************************** 
    '      FUNCTION   : btnOK_Click, ok button clicked 
    '      MEMO       :  
    '      CREATE     : 2012/01/17　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Try
            Dim blnSuccess As Boolean = False
            Dim intGen As Integer

            If mblnIsEditMode Then
                blnSuccess = xUpdate()
            Else
                blnSuccess = xInsert()
            End If

            'init gen is 1
            intGen = 1
            If Not basCommon.fncIsBlank(txtGeneration.Text.Trim) Then intGen = CInt(txtGeneration.Text.Trim)
            My.Settings.intInitGeneration = intGen
            My.Settings.strCreateMember = txtCreateMember.Text.Trim()
            My.Settings.Save()

            If blnSuccess Then
                'saved successfully!
                'update flag and close form
                mblnUpdate = True
                Me.Close()

            End If

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "btnOK_Click", ex, Nothing, False)

        End Try

    End Sub


    '   ****************************************************************** 
    '      FUNCTION   : btnCancel_Click, cancel button clicked
    '      MEMO       :  
    '      CREATE     : 2012/01/17　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Me.Close()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex, Nothing, False)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show this form
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2012/01/17　AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm(Optional ByVal blnIsEditMode As Boolean = False) As Boolean

        fncShowForm = False

        Try
            mblnIsEditMode = blnIsEditMode

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex, Nothing, False)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xLoad, load family information
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/08/16  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoad() As Boolean

        xLoad = False

        Try
            Dim strFname As String = ""
            Dim strFanni As String = ""
            Dim strFhome As String = ""

            btnOK.Text = "        Hoàn tất"

            If Not basCommon.fncGetFamilyInfo(strFname, strFanni, strFhome) Then Exit Function

            txtFamilyName.Text = strFname
            txtAnni.Text = strFanni
            txtHomeTown.Text = strFhome
            txtGeneration.Text = My.Settings.intInitGeneration.ToString()
            txtCreateMember.Text = My.Settings.strCreateMember

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoad", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xInsert, insert into database
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2012/01/17　AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xInsert() As Boolean

        xInsert = False

        Try
            mstrFName = txtFamilyName.Text.Trim()
            mstrFAnni = txtAnni.Text.Trim()
            mstrFHome = txtHomeTown.Text.Trim()

            'check blank
            If Not xIsValid() Then Exit Function

            'try to insert
            If Not gobjDB.fncInsertFamilyInfo(mstrFName, mstrFHome, mstrFAnni) Then

                basCommon.fncMessageWarning("Cập nhật dữ liệu không thành công.")
                Exit Function

            End If

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xInsert", ex, Nothing, False)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xUpdate, update into database
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2012/01/17　AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xUpdate() As Boolean

        xUpdate = False

        Try
            mstrFName = txtFamilyName.Text.Trim()
            mstrFAnni = txtAnni.Text.Trim()
            mstrFHome = txtHomeTown.Text.Trim()

            'check blank
            If Not xIsValid() Then Exit Function

            'try to insert
            If Not gobjDB.fncUpdateFamilyInfo(mstrFName, mstrFHome, mstrFAnni) Then

                basCommon.fncMessageWarning("Cập nhật dữ liệu không thành công.")
                Exit Function

            End If

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xUpdate", ex, Nothing, False)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xIsValid, is valid information
    '      VALUE      : boolean, true - yes, false - no
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2012/01/17　AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xIsValid() As Boolean

        xIsValid = False

        Try
            Dim strGen As String
            Dim intGen As Integer

            'check blank
            If basCommon.fncIsBlank(mstrFName) Then

                basCommon.fncMessageInfo("Hãy nhập tên dòng họ để tiếp tục.")
                txtFamilyName.Focus()
                Exit Function

            End If

            'check generation
            strGen = txtGeneration.Text.Trim()

            'exit if generation is null
            If basCommon.fncIsBlank(strGen) Then Return True

            'check numeric
            If Not IsNumeric(strGen) Then

                basCommon.fncMessageInfo("Đời bắt đầu của dòng họ phải là giá trị số.")
                txtGeneration.Focus()
                Exit Function

            End If

            'generation must be greater than 0
            intGen = CInt(strGen)

            'check numeric
            If intGen <= 0 Then

                basCommon.fncMessageInfo("Đời bắt đầu của dòng họ phải lớn hơn 0.")
                txtGeneration.Focus()
                Exit Function

            End If

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xIsValid", ex, Nothing, False)

        End Try

    End Function


End Class
