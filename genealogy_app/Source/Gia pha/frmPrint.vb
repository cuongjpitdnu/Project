'******************************************************************
'@@@TITLE        : Tax Invoice Management
'@@@FUNCTION     : Tax invoice List
'@@@MEMO         :  
'@@@CREATE       : 2010/12/13 NGHIA
'@@@UPDATE       : 
'******************************************************************

Option Explicit On
Option Strict Off

Public Class frmPrint
    Private mcstrClsName As String = "frmTaxInvoiceList"
    Private mstrPinter As String

    '   ******************************************************************
    '@@@FUNCTION   : Show form
    '@@@VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2010/12/13 AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Public Function ShowForm() As String

        ShowForm = ""
        Try

            Dim strPrinter As String
            cboPrinter.Items.Clear()
            Dim prtdoc As New Printing.PrintDocument()
            Dim strDefaultPrinter As String = prtdoc.PrinterSettings.PrinterName

            mstrPinter = ""
            For Each strPrinter In Printing.PrinterSettings.InstalledPrinters
                cboPrinter.Items.Add(strPrinter)
                If strPrinter = strDefaultPrinter Then
                    cboPrinter.SelectedIndex = cboPrinter.Items.IndexOf(strPrinter)
                End If
            Next

            'Show form
            Me.ShowDialog()

            ShowForm = mstrPinter

        Catch ex As Exception

            fncSaveErr(mcstrClsName, "ShowForm", ex)

        Finally

        End Try

    End Function



    '   ******************************************************************
    '@@@FUNCTION   : btnSave_Click
    '@@@VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2010/12/13 AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try

            mstrPinter = cboPrinter.Text
            If SetActPrinter(mstrPinter) Then
                mstrPinter = mstrPinter
            Else
                fncMessageWarning("Không thiết lập được máy in.")
                mstrPinter = ""
            End If


            Me.Close()

        Catch ex As Exception

            fncSaveErr(mcstrClsName, "btnSave_Click", ex)

        End Try

    End Sub

    '   ******************************************************************
    '@@@FUNCTION   : btnEnd_Click
    '@@@VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2010/12/13 AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnd.Click

        Try

            Me.Close()

        Catch ex As Exception

            fncSaveErr(mcstrClsName, "btnEnd_Click", ex)

        End Try

    End Sub

    '   ******************************************************************
    '@@@FUNCTION   : SetActPrinter
    '@@@VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2010/12/13 AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Public Function SetActPrinter(ByVal strPrinterName As String) As Boolean


        Dim prtdoc As Printing.PrintDocument
        Try

            prtdoc = New Printing.PrintDocument()
            prtdoc.PrinterSettings.PrinterName = strPrinterName

            Return True

        Catch ex As Exception
            fncSaveErr(mcstrClsName, "SetActPrinter", ex)
        Finally
            prtdoc = Nothing
        End Try

    End Function

End Class