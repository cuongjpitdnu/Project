'   **********************************************************************
'       FUNCTION   : clsProgressBar
'       MEMO       : 
'       CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
'       UPDATE     : 
'   **********************************************************************

Public Class clsProgressBar
    Implements IDisposable

    Const mcstrClsName As String = "clsProcessBar"

    Private mintPercent As Integer
    Private mfrmProcessBar As frmProgressBarCommon = Nothing
    Private mobjProgressBarThread As System.Threading.Thread = Nothing
    Private mobjProcessThread As System.Threading.Thread = Nothing
    Private mblnComplete As Boolean = False
    Private mblnErr As Boolean = False

    Private mblnComfirmClose As String = "Bạn có muốn dừng xử lý?"

    '   ******************************************************************
    '　　　	FUNCTION   : Percent
    '		VALUE      : Integer
    '      	MEMO       : Status ProgressBar
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Property Percent() As Integer

        Get
            Return mintPercent
        End Get
        Set(value As Integer)
            mintPercent = value
            Application.DoEvents()
            If value >= Me.mfrmProcessBar.Maximum Then
                Me.CompleteProcess = True
            End If
            Application.DoEvents()
        End Set
    End Property

    '   ******************************************************************
    '　　　	FUNCTION   : CompleteProcess
    '		VALUE      : True - Complete/False - Processing
    '      	MEMO       : 
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Property CompleteProcess() As Boolean

        Get
            Return mblnComplete
        End Get
        Set(value As Boolean)
            mblnComplete = value
            Application.DoEvents()
        End Set
    End Property

    '   ******************************************************************
    '　　　	FUNCTION   : ComfirmClose
    '		VALUE      : Content Comfirm When Click Button Close
    '      	MEMO       : 
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Property ComfirmClose() As String

        Get

            Return mblnComfirmClose

        End Get
        Set(value As String)

            mblnComfirmClose = value

        End Set
    End Property

    '   ******************************************************************
    '　　　	FUNCTION   : ProcessThread
    '      	MEMO       : 
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Property ProcessThread() As System.Threading.Thread

        Get

            Return mobjProcessThread

        End Get
        Set(value As System.Threading.Thread)

            mobjProcessThread = value

        End Set
    End Property

    '   ******************************************************************
    '　　　	FUNCTION   : ErrorProcess
    '      	MEMO       : 
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Property ErrorProcess As Boolean
        Get

            Return mblnErr

        End Get
        Set(value As Boolean)

            mblnErr = value

        End Set
    End Property

    '   ******************************************************************
    '		FUNCTION   : fncCreateProgressBar
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(Optional) - String - Title ProgressBar
    '                    ARG2(Optional) - Integer - Maximun ProgressBar
    '		MEMO       : Create Thread ProgressBar
    '		CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncCreateProgressBar(Optional strTitle As String = "",
                                         Optional blnShowButtonClose As Boolean = True,
                                         Optional intMaximum As Integer = 100) As Boolean

        fncCreateProgressBar = False

        Try

            If Not IsNothing(mfrmProcessBar) Then mfrmProcessBar.Dispose()

            mfrmProcessBar = New frmProgressBarCommon

            If Not String.IsNullOrWhiteSpace(strTitle) Then
                mfrmProcessBar.Title = strTitle
            End If

            mfrmProcessBar.ButtonClose = blnShowButtonClose
            AddHandler mfrmProcessBar.btnClose.Click, AddressOf xBtnCloseClick

            mfrmProcessBar.Maximum = intMaximum

            mobjProgressBarThread = New System.Threading.Thread(AddressOf xSetProcessBar)

            fncCreateProgressBar = True
        Catch ex As Exception
            Call Me.fncFreeMemory()
            basCommon.fncSaveErr(mcstrClsName, "fncCreateProgressBar", ex)
            Throw
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : fncCreateProgressBar
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(Optional) - Boolean - Show dialog Form ProgressBar
    '		MEMO       : Start Thead ProgressBar
    '		CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncStartProgressBar(Optional blnShowDialog As Boolean = True) As Boolean

        fncStartProgressBar = False

        Try

            If IsNothing(mobjProgressBarThread) OrElse IsNothing(mfrmProcessBar) Then
                Exit Function
            End If

            mintPercent = 0
            mblnComplete = False
            mblnErr = False

            If Not IsNothing(mobjProcessThread) Then mobjProcessThread.Start()
            mobjProgressBarThread.Start()

            If blnShowDialog Then
                mfrmProcessBar.StartPosition = FormStartPosition.CenterParent
                mfrmProcessBar.ShowDialog()
            Else
                mfrmProcessBar.StartPosition = FormStartPosition.CenterScreen
                mfrmProcessBar.Show()
            End If

            fncStartProgressBar = True
        Catch ex As Exception
            Call Me.fncFreeMemory()
            basCommon.fncSaveErr(mcstrClsName, "fncStartProgressBar", ex)
            Throw
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : fncEndProgressBar
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(Optional) - Boolean - Show dialog Form ProgressBar
    '		MEMO       : End Thead ProgressBar
    '		CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncEndProgressBar() As Boolean

        fncEndProgressBar = False

        Try

            If Not IsNothing(mobjProcessThread) Then mobjProcessThread.Abort()
            If Not IsNothing(mobjProgressBarThread) Then mobjProgressBarThread.Abort()
            
            fncEndProgressBar = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncEndProgressBar", ex)
        Finally
            Call Me.fncFreeMemory()
        End Try
    End Function

    '   ******************************************************************
    '　　　	FUNCTION   : fncFreeMemory
    '      	MEMO       : Free Memory
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Sub fncFreeMemory()
        Try

            mintPercent = 0
            mblnComplete = False

            mobjProcessThread = Nothing
            mobjProgressBarThread = Nothing

            If Not IsNothing(mfrmProcessBar) Then mfrmProcessBar.fncClose()
            mfrmProcessBar = Nothing
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncFreeMemory", ex)
        End Try
    End Sub

    '   ******************************************************************
    '		FUNCTION   : fncCalculatePercent
    '		VALUE      : Integer - Percent 
    '		PARAMS     : ARG1(IN) - Integer - Curent
    '                    ARG2(IN) - Integer - Max
    '		MEMO       : Calculate Percent
    '		CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncCalculatePercent(ByVal intCurent As Integer,
                                        ByVal intMax As Integer) As Integer
        Try

            fncCalculatePercent = Math.Ceiling((intCurent / intMax) * 100)

            If fncCalculatePercent > 100 Then fncCalculatePercent = 100

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncCalculate", ex)
            Throw
        End Try
    End Function

    '   ******************************************************************
    '　　　	FUNCTION   : xSetProcessBar
    '      	MEMO       : Set Status ProgressBar
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub xSetProcessBar()
        Try

            While Not mblnComplete

                If mblnErr Then Exit Sub

                Application.DoEvents()
                mfrmProcessBar.fncUpdateProgressBar(mintPercent)
                Application.DoEvents()
            End While
        Catch ex As Exception
            Call Me.fncFreeMemory()
            basCommon.fncSaveErr(mcstrClsName, "xSetProcessBar", ex)
            Throw
        Finally
            If Not IsNothing(mfrmProcessBar) Then mfrmProcessBar.fncClose()
        End Try
    End Sub

    '   ******************************************************************
    '　　　	FUNCTION   : xBtnCloseClick
    '      	MEMO       : 
    '      	CREATE     : 2017/07/28 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Private Sub xBtnCloseClick()
        Try

            If fncMessageConfirm(mblnComfirmClose) Then
                mfrmProcessBar.fncClose()
                fncEndProgressBar()
            End If
        Catch ex As Exception
            Call Me.fncFreeMemory()
            basCommon.fncSaveErr(mcstrClsName, "xBtnCloseClick", ex)
        End Try
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                Me.fncFreeMemory()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
