'   **********************************************************************
'       FUNCTION   : frmProgressBarCommon
'       MEMO       :
'       CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
'       UPDATE     : 
'   **********************************************************************
Public Class frmProgressBarCommon

    Private mblnClose As Boolean = True 'Alow(True)/Not Alow(False) Close Form

    Delegate Sub UpdateValue(ByVal intProgress As Integer)
    Delegate Sub CloseForm()

    '   ******************************************************************
    '　　　	FUNCTION   : Title
    '      	MEMO       : Title of Form
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Property Title() As String

        Get

            Return lblTitle.Text

        End Get

        Set(value As String)

            lblTitle.Text = value.Trim
            Application.DoEvents()
        End Set
    End Property

    '   ******************************************************************
    '　　　	FUNCTION   : Maximum
    '      	MEMO       : Maximum of ProgressBar
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Property Maximum() As Integer

        Get

            Return prgBar.Maximum

        End Get

        Set(value As Integer)

            prgBar.Maximum = value
            Application.DoEvents()
        End Set
    End Property

    '   ******************************************************************
    '　　　	FUNCTION   : ButtonClose
    '      	MEMO       : Display/Not Display Button Close
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Property ButtonClose() As Boolean

        Get

            Return btnClose.Visible

        End Get

        Set(value As Boolean)

            If value Then
                Me.Height = btnClose.Top + btnClose.Height + lblTitle.Top
            Else
                Me.Height = prgBar.Top + prgBar.Height + lblTitle.Top
            End If

            btnClose.Visible = value
            Application.DoEvents()
        End Set
    End Property

    '   ******************************************************************
    '　　　	FUNCTION   : fncUpdateProgressBar
    '		PARAMS     : ARG1(IN) - Integer
    '      	MEMO       : Update Status to ProgressBar
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Sub fncUpdateProgressBar(ByVal intProgress As Integer)
        Try
            If Me.prgBar.InvokeRequired Then
                Me.prgBar.Invoke(New UpdateValue(AddressOf fncUpdateProgressBar), New Object() {intProgress})
            Else
                Application.DoEvents()
                prgBar.Value = intProgress
                lblPercent.Text = intProgress.ToString & "%"
                Application.DoEvents()
            End If
        Catch
            Throw
        End Try
    End Sub

    '   ******************************************************************
    '　　　	FUNCTION   : fncClose
    '      	MEMO       : Close Form
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Sub fncClose()
        Try
            If Me.prgBar.InvokeRequired Then
                Me.prgBar.Invoke(New CloseForm(AddressOf xClose))
            Else
                Call xClose()
            End If
        Catch ex As Exception

        End Try
    End Sub

    '   ******************************************************************
    '　　　	FUNCTION   : xClose
    '      	MEMO       : Close Form
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub xClose()
        Try
            mblnClose = False
            Me.Close()
        Catch
            Me.Dispose()
        End Try
    End Sub

    '   ******************************************************************
    '　　　	FUNCTION   : frmProgressBarCommon_FormClosing
    '      	MEMO       : Disable Close Form When Processing
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub frmProgressBarCommon_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = mblnClose
    End Sub
End Class